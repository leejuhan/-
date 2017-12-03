using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CJMALL
{
    public class cjmall
    {
        private HtmlAgilityPack.HtmlWeb web;
        private HtmlAgilityPack.HtmlDocument document;
        private String cjmalllink;//cjmall상품정보  url
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String category = "";//카테고리
        private String deliveryprice = "";//배송비
        private String profit1 = "";//무이자
        private String profit2 = "";//cjmallonepoint
        private String profit3 = "";//판매가
        private String profit4 = "";//TV쇼핑가
        private String profit5 = "";//고객맞춤가
        private String profit7 = "";//카드세이브
        private String profit8 = "";//TV쇼핑가
        private String profit9 = "";//신한카드할인가
        private String profit10 = "";//TheCJ카드 할인가
        private String mall;
        private String itemNum = "";
        private string image = "";

        String[] datas;//데이터정보
        ArrayList auctiondatalist = new ArrayList();//cjmall resorce

        //생성자
        public cjmall(String cjmalllink, String itemNum, String mall)
        {
            this.cjmalllink = cjmalllink;
            realurl = cjmalllink;
            this.itemNum = itemNum;
            this.mall = mall;
        }

        //데이터를 저장하는 함수
        public void storedata()
        {
            //데이터를 빼오기위해 필요한 변수들
            String totalhtml;
            int index;
            int index2;
            String urlhtml;
            int count = 0;


            //url 변수       
            try
            {
                web = new HtmlAgilityPack.HtmlWeb();
                document = web.Load(cjmalllink);
                //encoding 해준다
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(cjmalllink))
                    {
                        var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));
                        var html = reader.ReadToEnd();
                        document.LoadHtml(html);
                    }
                }

            }
            catch (ArgumentException e)
            {
                Console.WriteLine("cjmall//url 변수 ArgumentException");
            }
            catch (WebException e)
            {
                Console.WriteLine("cjmall //url 변수 WebException");
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("cjmall //url 변수 WebException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("cjmall //url 변수 WebException");
            }


            //cjmall카테고리 정보

            try
            {
                foreach (var VARIABLE in document.DocumentNode.Descendants().Where(x =>
                               x.Name == "p" && x.Attributes.Contains("class") &&
                               x.Attributes["class"].Value.Split().Contains("selc")))
                {
                    category += VARIABLE.InnerText.Trim() + ">";
                }
                category = category.Substring(0, category.Length - 1).Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("cjmall카테고리 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("cjmall카테고리 NullReferenceException");
            }


            //cjmall 배송 정보

            try
            {
                deliveryprice = document.DocumentNode.SelectSingleNode("//span[@class='deliverTxt delivery_fee']").Descendants().Where(x =>
                                 x.Name == "strong").First().InnerText.Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("cjmall배송 정보IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("cjmall배송 정보 NullReferenceException");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("cjmall배송 정보 InvalidOperationException");
            }

            //무이자
            try
            {

                HtmlNode node = document.DocumentNode.SelectSingleNode("//span[@id='norest_month_txt']");
                if (node != null)
                {
                    profit1 = node.InnerText.TrimStart();

                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("cjmall무이자 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("cjmall무이자 NullReferenceException");
            }

            //이름
            try
            {

              name = document.DocumentNode.SelectSingleNode(".//head").Descendants().Where(x => x.Name == "meta").First().GetAttributeValue("content","").Trim();
                
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("cjmall이름 IndexOutOfRangeException");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("cjmall이름 IArgumentException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("cjmall이름 NullReferenceException");
            }


            //이미지
            try
            {
                image = document.DocumentNode.SelectSingleNode(".//div[@class='detailArea']").SelectSingleNode(".//div").SelectSingleNode(".//img").GetAttributeValue("src", "").Trim();

                

                /*image =document.DocumentNode.SelectSingleNode(".//div[@id='R_div_result']")
                        .SelectSingleNode(".//img")
                        .GetAttributeValue("src", "").Trim();
           */

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("cjmall이미지 IndexOutOfRangeException");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("cjmall이미지 IArgumentException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("cjmall이름 NullReferenceException");
            }





            //cjonepoint ,가격(판매가 tv쇼핑가,고객맞춤가,신한카드할인가,thecj카드할인가

            try
            {
                var nodes = document.DocumentNode.SelectSingleNode(".//div[@class='detailCan']");

                foreach (var node in nodes.Descendants().Where(x => x.Name == "li"))
                {
                    var tmp = node.Descendants().Where(x => x.Name == "strong").First().InnerText.Trim();
                    if (tmp == "CJ ONE 포인트")
                    {
                        profit2 += tmp + ":";
                        profit2 += node.SelectSingleNode(".//div[@class='detailData']").InnerText.Trim();

                    }
                    else if (tmp == "판매가")
                    {
                        profit3 += tmp + ":";
                        profit3 += node.SelectSingleNode(".//span").InnerText.Trim();

                    }
                    else if (tmp == "TV 쇼핑가")
                    {

                        try
                        {
                            profit4 += tmp + ":";
                            profit4 += node.SelectSingleNode(".//div[@class='detailData']").InnerText.Trim();
                            String[] tmp1 = profit4.Split('\n');
                            profit4 = "";
                            foreach (var VARIABLE in tmp1)
                            {
                                profit4 += VARIABLE.Trim();
                            }
                        }
                        catch (NullReferenceException e)
                        {
                            profit4 += node.SelectSingleNode(".//span").InnerText.Trim();
                        }



                    }
                    else if (tmp == "고객맞춤가")
                    {

                        profit5 += tmp + ":";
                        profit5 += node.SelectSingleNode(".//div[@class='detailData']").InnerText.Trim();

                    }
                    else if (tmp == "TheCJ카드 할인가")
                    {

                        profit10 += tmp + ":";
                        profit10 += node.SelectSingleNode(".//div[@class='detailData']").InnerText.Trim();


                    }
                    else if (tmp == "카드세이브")
                    {

                        profit7 += tmp + ":";
                        profit7 += node.SelectSingleNode(".//div[@class='detailData']").Descendants().Where(x => x.Name == "span").First().InnerText.Trim();


                    }
                    else if (tmp == "신한카드 할인가")
                    {

                        profit9 += tmp + ":";
                        profit9 += node.SelectSingleNode(".//div[@class='detailData']").InnerText.Trim();
                        String[] tmp1 = profit9.Split('\n');
                        profit9 = "";
                        foreach (var VARIABLE in tmp1)
                        {

                            profit9 += VARIABLE.Trim();
                        }

                    }



                }



            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("cjonepoint ,가격(판매가 tv쇼핑가,고객맞춤가,신한카드할인가,thecj카드할인가 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("cjonepoint ,가격(판매가 tv쇼핑가,고객맞춤가,신한카드할인가,thecj카드할인가 NullReferenceException");
            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine("cjonepoint ,가격(판매가 tv쇼핑가,고객맞춤가,신한카드할인가,thecj카드할인가 InvalidOperationException");
            }


        }
        
       
        //스트링배열로 데이터를 받는 함수
        public String[] getdatas()
        {
          
            storedata();
            bean bean = new bean();
            bean.itemnum = itemNum;
            bean.name = name;
            bean.url = realurl;
            bean.category = category;
            if (profit3 == "")
            {
                if (profit4 == "")
                {
                    if (profit5 == "")
                    {
                        if (profit9 == "")
                        {

                        }
                        else
                        {
                            bean.price = profit9;
                        }
                    }
                    else
                    {
                        bean.price = profit5;

                    }

                }
                else
                {
                    bean.price = profit4;
                }

            }
            else
            {
                bean.price = profit3;
            }
            
            bean.discountprice = profit4 + "//" + profit5 + "//" + profit8 + "//" + profit9 + "//";
            bean.delivery = deliveryprice;
            bean.timeprofit = profit1;
            bean.mall = mall;
            bean.image = image;
            bean.cardprofit = profit7;
            bean.etc = profit2;
            bean.getdatas();
            return bean.datas;
            
        }

        public string PdtName()
        {

            return name;
        }









    }
}
