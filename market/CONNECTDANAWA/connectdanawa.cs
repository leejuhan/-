using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DANAWA;
using HtmlAgilityPack;

namespace CONNECTDANAWA
{
    public class connectdanawa
    {
        private HtmlDocument document;
        private List<string> pages;
        public String PdtName = "";//제품명
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String price = "";//가격정보
        private String profit = "";//할인카드
        private String delivery = "";//배송비
        private String itemNum = "";//아이템넘버
        private String[] datas;//데이터정보
        private String link = "";//danawa 상품정보 url
        private List<String[]> danawadatalist = new List<String[]>();//danawadata resorce
        private danawa danawa;
        /// <summary>
        /// //생성자에서 해당 다나와 리스트 페이지 수를 포스트로 날려서 알아오는 함수이다
        /// </summary>
        /// <param name="PdtName"></param>
        public connectdanawa(String PdtName)
        {
            try
            {
                pages=new List<string>();
                this.PdtName = PdtName;
                int count = 0;
                int num = 0;
                HtmlDocument tmpdocument = new HtmlDocument();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://search.danawa.com/ajax/getProductList.ajax.php");
                request.Method = "POST";
                string postData = "boost=true" + "&" + "limit=0" + "&" + "list=list" + "&" + "originalQuery=" + PdtName + "&" + "page=" + "1" + "&" + "previousKeyword =" + PdtName + "&" + "query=" + PdtName + "&" + "sort=" + "&" + "tab=main" + "&" + "volumeType=allvs";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; SMJB; rv:11.0) like Gecko";
                request.Accept = "text/html, */*; q=0.01";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = "http://search.danawa.com/dsearch.php?k1=%EB%82%98%EC%9D%B4%ED%82%A4+%ED%97%88%EB%9D%BC%EC%B7%A8+%EC%9A%B8%ED%94%84+%EA%B7%B8%EB%A0%88%EC%9D%B4&module=goods&act=dispMain";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                tmpdocument.LoadHtml(reader.ReadToEnd());
                num =
                    tmpdocument.DocumentNode.SelectSingleNode(".//div[@class='paging_number_wrap']")
                        .Descendants()
                        .Where(x => x.Name == "a")
                        .Count();
               
                
                foreach (var VARIABLE in tmpdocument.DocumentNode.SelectSingleNode(".//div[@class='paging_number_wrap']").Descendants().Where(x => x.Name == "a"))
                {
                    pages.Add(VARIABLE.InnerText.Trim());
                    //Console.WriteLine(VARIABLE.InnerText.Trim());
                }
                reader.Close();
                dataStream.Close();
                response.Close();
                
                
            }
            catch (System.AggregateException)
            {

                pages[0] = "1";

            }
            catch (NullReferenceException)
            {
                try
                {
                    //pages[0] = "1";

                }
                catch (AggregateException)
                {

                    //pages[0] = "1";
                }
               
            }

        }

        //데이터를 저장하는 함수
        public void collectdata(HtmlAgilityPack.HtmlDocument document)
        {
            danawa = new danawa();
         //   Console.WriteLine("콜레드데이타 함수 속"); 
            try
            {
                
                foreach (var VARIABLE in document.DocumentNode.Descendants().Where(x =>
                  x.Name == "div" && x.Attributes.Contains("class") &&
                  x.Attributes["class"].Value.Split().Contains("prod_pricelist")))
                {
                   
                    //다나와 가격비교 리스트속 리스트 데이터
                    try
                    {
                        
                        VARIABLE.SelectSingleNode(".//p[@class='chk_sect']").InnerText.Trim();
                        realurl = VARIABLE.SelectSingleNode(".//a").GetAttributeValue("href", "");
                       // Console.WriteLine(realurl);
                       // Console.WriteLine("다나와 라이브러리 타는 곳");
                        danawa.getdanawalink(realurl);
                        //danawadatalist.AddRange(danawa.getdatas());

                    }
                    //다나와 가격비교 리스트 
                    catch (NullReferenceException e)
                    {
                        try
                        {
                           // Console.WriteLine("사이트 목록");
                            //Console.WriteLine(VARIABLE.InnerHtml);
                            
                            realurl = VARIABLE.SelectSingleNode(".//a").GetAttributeValue("href", "");
                            if (realurl.Trim() == "#")
                            {
                                realurl =
                                    VARIABLE.Descendants()
                                        .Where(x => x.Name == "a")
                                        .ElementAt(1)
                                        .GetAttributeValue("href", "");
                            }
                          
                            realurl = geturl(realurl);
                            
                         //   Console.WriteLine(realurl);
                            name = VARIABLE.SelectSingleNode(".//img").GetAttributeValue("alt", "").Trim();
                          //  Console.WriteLine(name);
                            price = VARIABLE.SelectSingleNode(".//strong").InnerText.Trim();
                         //   Console.WriteLine(price);
                            delivery = VARIABLE.SelectSingleNode(".//span[@class='ship_sect']").SelectSingleNode(".//strong").InnerText.Trim();
                         //   Console.WriteLine(delivery);
                            try
                            {
                                int tmp = 0;
                                int tmp2 = 0;
                                itemNum = "";
                                if (name.Trim() == "11번가")
                                {
                                    tmp = realurl.IndexOf(@"prdNo=") + 6;
                                    tmp2 = realurl.IndexOf(@"&tid");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);

                                }
                                else if (name == "G마켓")
                                {
                                    tmp = realurl.IndexOf("=") + 1;
                                    tmp2 = realurl.IndexOf("&");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);

                                }
                                else if (name == "옥션")
                                {
                                    tmp = realurl.IndexOf("=") + 1;
                                    tmp2 = realurl.IndexOf("&");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);

                                }
                                else if (name == "GS샵")
                                {
                                    tmp = realurl.IndexOf("ecpid") + 6;
                                    tmp2 = realurl.IndexOf("&u");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);
                                }
                                else if (name == "CJMALL")
                                {

                                    tmp = realurl.IndexOf("item_cd") + 8;
                                    itemNum = realurl.Substring(tmp, realurl.Length - tmp);

                                }
                                // Console.WriteLine(itemNum);



                            }
                            catch (ArgumentException)
                            {
                               // Console.WriteLine("아이템넘버 exception 5개의 사이트가 아니다");
                            }

                            datas = new string[6];
                            datas[0] = realurl.Trim();
                            datas[1] = name.Trim();
                            datas[2] = itemNum.Trim();
                            datas[3] = price.Trim();
                            datas[4] = profit.Trim();
                            datas[5] = delivery.Trim();
                            danawadatalist.Add(datas);
                            
                        }
                        catch (NullReferenceException)
                        {

                           // Console.WriteLine("콜렉트다나와함수에서 단종된 상품");
                        }
                       

                    }
                  // Console.WriteLine("/////////////////////////////////////////");
                }
            }
            catch (UriFormatException e)
            {
            //    Console.WriteLine("danawa UriFormatException");

            }
            
            catch (ArgumentException e)
            {
              //  Console.WriteLine("danawa ArgumentException");
            }
            catch (IndexOutOfRangeException e)
            {
              //  Console.WriteLine("danawa IndexOutOfRangeException");
            }
            catch (InvalidOperationException e)
            {
              //  Console.WriteLine("danawa InvalidOperationException");
            }
            catch (AggregateException e)
            {
               // Console.WriteLine("AggregateException");
            }

        }


        //danawa링크안에 있는 각각의 상품 url
        public String geturl(String link)
        {
            HtmlAgilityPack.HtmlWeb web2 = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument document2 = web2.Load(link);
            String ww = "http://prod.danawa.com";
            if (ww.Substring(0, 20) != link.Substring(0, 20))
            { return link; }
            var root = document2.DocumentNode;
            String content2 = root.InnerHtml;
            int index3 = content2.IndexOf("goLink") + 8;
            int index4 = content2.IndexOf(");") - 9;
            string result = content2.Substring(index3, index4 - index3);
            return result.Trim();

        }

        public void storedata()
        {
            if (pages.Count!=0)
            {
                try
                {

                    foreach (var page in pages)
                    {
                       // Console.WriteLine(page);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://search.danawa.com/ajax/getProductList.ajax.php");
                        request.Method = "POST";
                        string postData = "boost=true" + "&" + "limit=0" + "&" + "list=list" + "&" + "originalQuery=" + PdtName + "&" + "page=" + page + "&" + "previousKeyword =" + PdtName + "&" + "query=" + PdtName + "&" + "sort=" + "&" + "tab=main" + "&" + "volumeType=allvs";
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; SMJB; rv:11.0) like Gecko";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Referer = "http://search.danawa.com/dsearch.php?k1=%EB%82%98%EC%9D%B4%ED%82%A4+%ED%97%88%EB%9D%BC%EC%B7%A8+%EC%9A%B8%ED%94%84+%EA%B7%B8%EB%A0%88%EC%9D%B4&module=goods&act=dispMain";
                        request.ContentLength = byteArray.Length;
                        Stream dataStream = request.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();
                        WebResponse response = request.GetResponse();
                        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                        dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        document = new HtmlDocument();
                        String tmp = reader.ReadToEnd();
                        document.LoadHtml(tmp);
                     //   Console.WriteLine("콜렉트함수 들어가기전");
                        //Console.WriteLine(document.DocumentNode.InnerHtml);
                        collectdata(document);
                        reader.Close();
                        dataStream.Close();
                        response.Close();
                     //   Console.WriteLine("스트림끝");

                    }

                }
                catch (WebException e)
                {
                   // Console.WriteLine("danawaurl 변수 WebException");
                }
                catch (ArgumentException e)
                {
                 //   Console.WriteLine("danawaurl 변수 ArgumentException");
                }
                catch (HtmlWebException e)
                {
                 //   Console.WriteLine("danawaurl 변수 HtmlWebException");
                }
                catch (UriFormatException e)
                {
                  //  Console.WriteLine("danawaurl 변수 UriFormatException");
                }

                catch (InvalidOperationException e)
                {
                   // Console.WriteLine("danawaurl 변수 InvalidOperationException");
                }


            }

        }

        public List<string[]> getdatas()
        {
            storedata();
            return danawadatalist;
        }
    }
}
