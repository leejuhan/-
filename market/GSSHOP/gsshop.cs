using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace GSSHOP
{
    public class gsshop
    {
        private HtmlAgilityPack.HtmlWeb web;
        private HtmlAgilityPack.HtmlDocument document;
        private String gsshoplink;//gsshop 상품정보  url
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String category = "";//카테고리
        private String realprice = "";//원래가격정보
        private String discountprice = "";//할인가격
        private String deliveryprice = "";//배송비
        private String profit1 = "";//무이자
        private String cardprice = "";//행사카드
        private String itemNum = "";
        private String mall="";
        private String image = "";
        String[] datas;//데이터정보
        ArrayList gsshopdatalist = new ArrayList();//gsshop resorce



        //생성자
        public gsshop(String gsshoplink, String itemNum, String mall)
        {
            this.gsshoplink = gsshoplink;
            realurl = gsshoplink;
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
                document = web.Load(gsshoplink);

            }
            catch (ArgumentException e)
            {
                Console.WriteLine("gsshop//url 변수 ArgumentException");
            }
            catch (WebException e)
            {
                Console.WriteLine("gsshop //url 변수 WebException");
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("gsshop //url 변수 WebException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("gsshop //url 변수 WebException");
            }


            //gsshop카테고리 정보
            try
            {

                foreach (var VARIABLE in document.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("go")))
                {
                    category += VARIABLE.InnerText.Trim() + ">";
                }
                category = category.Substring(0, category.Length - 1).Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop카테고리 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("gsshop카테고리 NullReferenceException");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("gsshop카테고리 InvalidOperationException");
            }

            //원래가격 정보
            try
            {
                HtmlNode node1 = document.DocumentNode.SelectSingleNode("//div[@class='org mprice']");
                if (node1 != null)
                {
                    realprice = node1.InnerText;
                    realprice = realprice.Trim();
                    if (realprice == null)
                    {
                        realprice = "";
                    }

                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop원래가격 정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("gsshop원래가격 정보 (NullReferenceException");
            }



            //진짜 가격 정보
            try
            {

                realprice = document.DocumentNode.SelectSingleNode(".//div[@class='price-definition-upper']").InnerText;

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop할인가격정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {

            }
            //상품이름
            try
            {
                int tmp = 0;
                name =
                    document.DocumentNode.SelectSingleNode(".//head")
                        .Descendants()
                        .Where(x => x.Name == "meta")
                        .ElementAt(1).GetAttributeValue("content", "").Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop상품이름 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("gsshop상품이름 NullReferenceExceptionn");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("gsshop상품이름 ArgumentException");
            }


            //이미지
            try
            {
                image =
                    document.DocumentNode.SelectSingleNode(".//a[@class='btn_img']")
                        .SelectSingleNode(".//img")
                        .GetAttributeValue("src", "")
                        .Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop이미지 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("gsshop이미지 NullReferenceExceptionn");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("gsshop이미지 ArgumentException");
            }



            //할인가격정보
            try
            {
                discountprice =
                     document.DocumentNode.SelectSingleNode(".//span[@class='price-definition-percent']").InnerText + " " +
                     document.DocumentNode.SelectSingleNode(".//span[@class='price-definition-ins']").Descendants().Where(x =>
                                x.Name == "strong").ElementAt(0).InnerText;


            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop할인가격정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                discountprice =
                   document.DocumentNode.SelectSingleNode(".//span[@class='price-definition-gs']").InnerText + " " +
                   document.DocumentNode.SelectSingleNode(".//span[@class='price-definition-ins']").Descendants().Where(x =>
                              x.Name == "strong").ElementAt(0).InnerText;
            }


            //배송비,행사카드,무이자 
            try
            {
                //Console.WriteLine(document.DocumentNode.SelectSingleNode(".//span[@class='label-card-info']").InnerText);

                int tmp = 0;
                foreach (var VARIABLE in document.DocumentNode.Descendants().Where(x =>
                              x.Name == "dt" && x.Attributes.Contains("class") &&
                              x.Attributes["class"].Value.Split().Contains("purchase-merit-title")))
                {
                    if (VARIABLE.InnerText == "배송비")
                    {
                        deliveryprice = document.DocumentNode.Descendants()
                         .Where(
                             x =>
                                 x.Name == "div" && x.Attributes.Contains("class") &&
                                 x.Attributes["class"].Value.Split().Contains("purchase-merit-substance-single"))
                         .ElementAt(tmp).SelectSingleNode(".//strong")
                         .InnerText.Trim();
                    }
                    else if (VARIABLE.InnerText == "무이자")
                    {
                        profit1 = document.DocumentNode.Descendants()
                         .Where(
                             x =>
                                 x.Name == "div" && x.Attributes.Contains("class") &&
                                 x.Attributes["class"].Value.Split().Contains("purchase-merit-substance-single"))
                         .ElementAt(tmp).SelectSingleNode(".//strong")
                         .InnerText.Trim();
                    }
                    else if (VARIABLE.InnerText == "행사카드")
                    {
                        cardprice = document.DocumentNode.Descendants()
                            .Where(
                                x =>
                                    x.Name == "div" && x.Attributes.Contains("class") &&
                                    x.Attributes["class"].Value.Split().Contains("purchase-merit-substance-single"))
                            .ElementAt(tmp).InnerText.Trim();
                        index = cardprice.IndexOf(">") + 1;
                        index2 = cardprice.IndexOf("<!-- 툴핍 -->");
                        cardprice = cardprice.Substring(index, index2 - index);
                    }
                    tmp++;

                }




            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("gsshop배송비 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("gsshop배송비 NullReferenceException");
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
            if (realprice == "")
            {
                bean.price = discountprice;
            }
            else
            {
                bean.price = realprice;
            }
            bean.discountprice = discountprice;
            bean.delivery = deliveryprice;
            bean.image = image;
            bean.timeprofit = "";
            bean.mall = mall;
            bean.cardprofit = cardprice;
            bean.etc = "";
            bean.getdatas();
            return bean.datas;
        }
        public string PdtName()
        {

            return name;
        }



    }


}


    

