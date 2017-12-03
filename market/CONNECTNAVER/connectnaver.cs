using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CONNECTNAVER
{
    public class connectnaver
    {
        private HtmlAgilityPack.HtmlWeb web;
        private HtmlAgilityPack.HtmlDocument document;
        private HtmlAgilityPack.HtmlDocument document3;
        private String PdtName = "";//제품명
        private String itemNum = "";//아이템넘버
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String price = "";//가격정보
        private String profit = "";//할인카드
        private String delivery = "";//배송비
        private String[] datas;//데이터정보
        private String link = "http://shopping.naver.com/search/all.nhn?query=";//naver 상품정보 url
        private List<String[]> naverdatalist = new List<String[]>();//naverdata resorce

        public connectnaver(String PdtName)
        {
            this.PdtName = PdtName;

        }

        public String naverlink()
        {
            try
            {
               
                web = new HtmlAgilityPack.HtmlWeb();
                document = web.Load(link+ PdtName);
                String tmp = "http://shopping.naver.com";

                String tmp2 =
                     document.DocumentNode.SelectSingleNode(".//li[@class='_model_list']")
                         .SelectSingleNode(".//a")
                         .GetAttributeValue("href", "");
                return tmp + tmp2;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("CONNECTNAVER//url 변수 ArgumentException");
                return "";
            }
            catch (WebException e)
            {
                Console.WriteLine("CONNECTNAVER //url 변수 WebException");
                return "";
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("CONNECTNAVER//url 변수 HtmlWebException");
                return "";
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("CONNECTNAVER//url 변수 UriFormatException");
                return "";
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("CONNECTNAVER//url 변수 UriFormatException");
                return "";
            }
            

        }





        //데이터를 저장하는 함수
        public void collectdata(HtmlAgilityPack.HtmlDocument document)
        {
            
            //list source
            IEnumerable<HtmlAgilityPack.HtmlNode> values =
                document.DocumentNode.Descendants().Where(x =>x.Name == "li" && x.Attributes.Contains("class") &&x.Attributes["class"].Value.Split().Contains("_product_list"));
            
            //데이터를 빼오기위해 필요한 변수들
            String totalhtml;
            int index;
            int index2;
            String urlhtml;
            
            foreach (HtmlAgilityPack.HtmlNode value in values)
            {

                if (value.NodeType != HtmlAgilityPack.HtmlNodeType.Text)
                {
                    try
                    {
                        //사이트이름 뽑아낸다                
                        name =
                            value.SelectSingleNode(".//p[@class='mall_txt']")
                                .Descendants()
                                .Where(x => x.Name == "a")
                                .Last()
                                .GetAttributeValue("mall_name", "")
                                .Trim();                            
                        //가격 뽑아낸다
                        price = value.SelectSingleNode(".//span[@class='num _price_reload']").InnerText.Trim();
                        //배송비 뽑아낸다
                        delivery =
                            value.SelectSingleNode(".//ul[@class='mall_option']")
                                .Descendants()
                                .Where(x => x.Name == "em")
                                .First()
                                .InnerText.Trim();
                        //카드정보 뽑아낸다
                        
                        profit = value.SelectSingleNode(".//span[@class='detail']").InnerText.Trim();
                       
                        
                        //url뽑아낸다
                        geturl(value.SelectSingleNode(".//a").GetAttributeValue("href", ""), 1);
                        


                    }
                    catch (UriFormatException e)
                    {
                        Console.WriteLine("네이버 UriFormatException");

                    }
                    catch (NullReferenceException e)
                    {
                        try { name = value.SelectSingleNode(".//span[@class='mall_type']").InnerText; }
                        catch (NullReferenceException e1) { }

                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("네이버 ArgumentException");

                    }
                    catch (IndexOutOfRangeException e)
                    {

                        Console.WriteLine("네이버 IndexOutOfRangeException");
                    }
                    catch (InvalidOperationException e)
                    {

                        Console.WriteLine("네이버 InvalidOperationException");
                    }

                    finally
                    {
                        
                        if (name != "")
                        {
                            try
                            {
                                int tmp = 0;
                                int tmp2 = 0;
                                if (name != "옥션") { itemNum = ""; }
                                if (name.Trim() == "11번가")
                                {
                                    tmp = realurl.IndexOf(@"prdNo=") + 6;
                                    tmp2 = realurl.IndexOf(@"&Na");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);

                                }
                                else if (name == "G마켓")
                                {
                                    tmp = realurl.IndexOf("=") + 1;
                                    tmp2 = realurl.IndexOf("&");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);

                                }
                                else if (name == "GSSHOP")
                                {
                                    tmp = realurl.IndexOf("=") +1;
                                    tmp2 = realurl.IndexOf("&");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);
                                }
                                else if (name == "CJmall")
                                {

                                    tmp = realurl.IndexOf("=") + 1;
                                    tmp2 = realurl.IndexOf("&");
                                    itemNum = realurl.Substring(tmp, tmp2 - tmp);

                                }

                            }
                            catch (ArgumentException) { Console.WriteLine("아이템넘버 exception"); }
                   

                            datas = new string[6];
                            datas[0] = realurl.Trim();
                            datas[1] = name.Trim();
                            datas[2] = itemNum.Trim();
                            datas[3] = price.Trim();
                            datas[4] = profit.Trim();
                            datas[5] = delivery.Trim();
                            naverdatalist.Add(datas);
                            
                       

                        }


                    }

                }

            }

        }


        //naver링크안에 있는 각각의 상품 url
        public void geturl(String link, int code)
        {
            HtmlAgilityPack.HtmlWeb web2 = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument document2 = web2.Load(link);
            String tmp = document2.DocumentNode.InnerText;

            switch (code)
            {
                case 1:
                    
                    if (name == "G마켓")
                    {
                        int index = 0, index2 = 0;
                        index = tmp.IndexOf("targetUrl") + 13;
                        index2 = tmp.IndexOf("var refererLink") - 5;
                        tmp = tmp.Substring(index, index2 - index).Trim();
                        tmp = ("http://cr2.shopping.naver.com" + tmp);
                        geturl(tmp, 2);
                    }
                    else if (name == "11번가")
                    {
                        int index = 0, index2 = 0;
                        index = tmp.IndexOf("targetUrl") + 13;
                        index2 = tmp.IndexOf("var refererLink") - 5;
                        tmp = tmp.Substring(index, index2 - index).Trim();
                        tmp = ("http://cr2.shopping.naver.com" + tmp);
                        geturl(tmp, 3);

                    }
                    else if (name == "CJmall")
                    {
                        int index = 0, index2 = 0;
                        index = tmp.IndexOf("targetUrl") + 13;
                        index2 = tmp.IndexOf("var refererLink") - 5;
                        tmp = tmp.Substring(index, index2 - index).Trim();
                        tmp = ("http://cr2.shopping.naver.com" + tmp);
                        geturl(tmp, 4);

                    }
                    else if (name == "GSSHOP")
                    {
                        int index = 0, index2 = 0;
                        index = tmp.IndexOf("targetUrl") + 13;
                        index2 = tmp.IndexOf("var refererLink") - 5;
                        tmp = tmp.Substring(index, index2 - index).Trim();
                        tmp = ("http://cr2.shopping.naver.com" + tmp);
                        geturl(tmp, 5);

                    }
                    else if (name == "옥션")
                    {
                      
                        int index = 0, index2 = 0;
                        index = tmp.IndexOf("targetUrl") + 13;
                        index2 = tmp.IndexOf("var refererLink") - 5;
                        tmp = tmp.Substring(index, index2 - index).Trim();
                        tmp = ("http://cr2.shopping.naver.com" + tmp);
                        geturl(tmp, 6);

                    }
                    else
                    {
                        realurl = link;
                    }

                    break;

                //G마켓url일때
                case 2:
                    int index3 = 0, index4 = 0;
                    index3 = tmp.IndexOf("replace") + 9;
                    index4 = tmp.IndexOf("}") - 6;
                    tmp = tmp.Substring(index3, index4 - index3);
                    realurl = tmp.Split('"').ElementAt(tmp.Split('"').Length - 1);
                    break;
                //11번가url일때
                case 3:
                    String tmp2 = document2.DocumentNode.Descendants().Where(x =>
                         x.Name == "script").ElementAt(2).InnerText;
                    realurl = tmp2.Split('"').ElementAt(1);

                    break;
                //cjmall
                case 4:
                    String kk = document2.DocumentNode.Descendants().Where(x =>
                      x.Name == "script").Last().InnerText;

                    realurl = kk.Split('"').ElementAt(1);

                    break;
                //gsshop
                case 5:
                    String kk2 = document2.DocumentNode.Descendants().Where(x =>
                       x.Name == "script").Last().InnerText;
                    String tmp3 = "http://with.gsshop.com";
                    realurl = tmp3 + kk2.Split('"').ElementAt(1);

                    break;
                case 6:
                    String kk3 = document2.DocumentNode.Descendants().Where(x =>
                     x.Name == "script").ElementAt(3).InnerText;
                    itemNum = kk3.Split('"').ElementAt(kk3.Split('"').Length - 2);
                    realurl = "http://itempage3.auction.co.kr/DetailView.aspx?ItemNo=" + itemNum + "&frm3=V2";
                    break;

            }
        }




        public void storedata()
        {
            //url 변수       
            try
            {
                //첫페이지의 데이터를 수집한다.
                web = new HtmlAgilityPack.HtmlWeb();
                document = web.Load(link + PdtName);
                collectdata(document);



                
                //나머지페이지의 데이터를 수집한다.
                document3 = web.Load(link + PdtName);
                int index3 = 0;
                int index4 = 0;
                
                String page;
                String url;
                var VARIABLES = document.DocumentNode.SelectSingleNode(".//div[@class='co_paginate']").Descendants().Where(x => x.Name == "a");
                VARIABLES.Last().Remove();
                foreach (var VARIABLE in VARIABLES)
                {
                    
                    page = VARIABLE.GetAttributeValue("onclick", "");
                    index3 = page.IndexOf("(") + 1;
                    index4 = page.IndexOf(",");
                    page = page.Substring(index3, index4 - index3);
                    url = "http://shopping.naver.com/search/all.nhn?query=" + PdtName +
                          "&pagingIndex=" + page + "&pagingSize=40&productSet=total&viewType=list&sort=rel&frm=NVSHPAG&sps=N";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Referer = "http://shopping.naver.com/search/all.nhn?query=%EB%82%98%EC%9D%B4%ED%82%A4%20%EC%97%90%EC%96%B4%ED%8F%AC%EC%8A%A4%20%EA%B2%80%2F%ED%9D%B0&pagingIndex=2&pagingSize=40&productSet=total&viewType=list&sort=rel&frm=NVSHPAG&sps=N";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    document3.LoadHtml(reader.ReadToEnd());
                    collectdata(document3);
                    

                }
                


            }
            catch (WebException e)
            {
                Console.WriteLine("네이버url 변수 WebException");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("네이버url 변수 ArgumentException");
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("네이버url 변수 HtmlWebException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("네이버url 변수 UriFormatException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("네이버url 변수 NullReferenceException");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("네이버url 변수 InvalidOperationException");
            }

        }


        public List<string[]> getdatas()
        {
            storedata();
            return naverdatalist;

        }





    }
}
