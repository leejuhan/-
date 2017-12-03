using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AUCTION
{
    public class auction
    {
        private HtmlAgilityPack.HtmlWeb web;
        private HtmlAgilityPack.HtmlDocument document;
        private String auctionlink;//auction 상품정보  url
        private String realurl = "";// url 정보
        public String name = "";// 이름정보
        private String category = "";//카테고리
        private String realprice = "";//원래가격정보
        private String discountprice = "";//할인가격
        private String cardprice = "";//카드할인가격 
        private String card = "";//할인해주는 카드사
        private String deliveryprice = "";//배송비
        private String profit1 = "";//적립/기타혜택
        private String profit2 = "";//무이자
        private String profit3 = "";//무이자 카드정보
        private String profit4 = "";//옥션제휴카드
        private String profit5 = "";//옥션제휴카드 정보
        private String coupon = "";//쿠폰
        private String mall;
        private String itemNum = "";
        private String image = "";

        String[] datas;//데이터정보
        ArrayList auctiondatalist = new ArrayList();//auction resorce

        //생성자
        public auction(String auctionlink, String itemNum,String mall)
        {
            this.auctionlink = auctionlink;
            realurl = auctionlink;
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
                document = web.Load(auctionlink);
                //encoding 해준다
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(auctionlink))
                    {
                        var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));
                        var html = reader.ReadToEnd();
                        document.LoadHtml(html);
                    }
                }

            }
            catch(ArgumentException e)
            {
               Console.WriteLine("옥션//url 변수 ArgumentException");
            }
            catch (WebException e)
            {
                Console.WriteLine("옥션 //url 변수 WebException");
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("옥션 //url 변수 WebException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("옥션 //url 변수 WebException");
            }


            //옥션카테고리 정보

            try
               {
                    HtmlNode node = document.DocumentNode.SelectSingleNode("//meta[@name='description']");
                    if (node != null)
                    {
                        category = node.GetAttributeValue("content", "");
                        index = 0;
                        index2 = category.IndexOf(",");
                        category = category.Substring(index, index2 - index);
                    }

                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("옥션카테고리 IndexOutOfRangeException");
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("옥션카테고리 NullReferenceException");
                }
            //옥션제품이름
            try
            {
             name = document.DocumentNode.SelectSingleNode("//div[@class='big-img']").Descendants().Where(x => x.Name == "img").First().GetAttributeValue("title","").Trim();
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("옥션카테고리 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션카테고리 NullReferenceException");
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
                Console.WriteLine("옥션원래가격 정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션원래가격 정보 (NullReferenceException");
            }
              


            //할인가격정보
            try
            {
                HtmlNode node2 = document.DocumentNode.SelectSingleNode("//div[@class='dis mprice']");
                if (node2 != null)
                {
                    discountprice = node2.InnerText;
                    discountprice = discountprice.Trim();
                    if (discountprice == null)
                    {
                        discountprice = "";
                    }



                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("옥션할인가격정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션할인가격정보 (NullReferenceException");
            }





            //카드 할인 
            try
            {
                HtmlNode node3 = document.DocumentNode.SelectSingleNode("//div[@class='txt-st1-addsale']");
                if (node3 != null)
                {
                    cardprice = node3.Element("p").InnerText;
                    card = node3.Element("span").InnerText;
                    cardprice = cardprice.Trim();
                    card = card.Trim();
                    if (cardprice == null || card == null)
                    {
                        card = "";
                        cardprice = "";
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("옥션카드 할인 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션카드 할인 NullReferenceException");
            }
            

            //배송비 
            try
            {
                HtmlNode node4 = document.DocumentNode.SelectSingleNode("//div[@class='del-poplayer']");
                if (node4 != null)
                {
                    deliveryprice = node4.Element("em").InnerText;

                    if (deliveryprice == null)
                    {
                        deliveryprice = "";
                    }
                    deliveryprice = deliveryprice.TrimStart();
                    deliveryprice = deliveryprice.TrimEnd();


                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("옥션배송비 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션배송비 NullReferenceException");
            }


            //auction 이미지 
            try
            {
                image =
                    document.DocumentNode.SelectSingleNode(".//div[@class='big-img']")
                        .SelectSingleNode(".//img")
                        .GetAttributeValue("src", "")
                        .Trim();
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("옥션이미지 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션이미지 NullReferenceException");
            }




            //혜택 1)적립/기타혜택  2)무이자 +할인
            try
            {
                IEnumerable<HtmlAgilityPack.HtmlNode> nodes =
                    document.DocumentNode.Descendants()
                        .Where(
                            x =>
                                x.Name == "ul" && x.Attributes.Contains("class") &&
                                x.Attributes["class"].Value.Split().Contains("benefit-list"));
                HtmlNode node5 = nodes.ElementAt(1);
                if (node5 != null)
                {
                    String tmp;
                    profit1 += node5.SelectSingleNode(".//span[@id='ucBenefit_hdivAuctionPoint']").InnerText;
                    profit1 += node5.SelectSingleNode(".//li[@id='ucBenefit_hdivAsianaOKCashbag']").InnerText;
                    tmp = profit1;
                    profit1 = "";
                    foreach (var VARIABLE in tmp.Split('\n'))
                    {
                        profit1 += VARIABLE.Trim();
                    }

               
               

                }
                //무이자 혜택

                HtmlNode node6 = nodes.First();
                HtmlNode smallnode6 = document.DocumentNode.SelectSingleNode("//script[contains(text(), 'frmAuctionCardInfo')]");
                if (node6 != null)
                {
                    //최대 무이자 혜택
                    
                    var connode6 = node6.Descendants().Where(x => x.Name == "span");
                    var tmp = node6;
                    
                    profit2 = connode6.ElementAt(0).InnerText;
                    if(connode6.ElementAt(1).GetAttributeValue("class","")=="text-month")
                    { profit2+= tmp.SelectSingleNode(".//span[@class='text-month']").InnerText;}
                    
                    
                    //무이자 카드 정보
                    String tmpurl;
                    //card url을 타고 들어가서 값을 구한다.
                    tmpurl = smallnode6.InnerText;
                    tmpurl = tmpurl.Trim();
                    index = tmpurl.IndexOf("frmAuctionCardInfo");
                    index2 = tmpurl.IndexOf("openLayer('hd");
                    tmpurl = tmpurl.Substring(index, index2 - index);
                    String[] profit3link = tmpurl.Split('"');
                    getif((String)profit3link.GetValue(4), 2);



                }


            }
            catch (ArgumentException e)
            {
               
                Console.WriteLine("옥션혜택 1)적립/기타혜택  2)무이자 +할인 ArgumentException");
            }
            catch (UriFormatException e)
            {
               
                Console.WriteLine("옥션혜택 1)적립/기타혜택  2)무이자 +할인 UriFormatException");
            }
            catch (NullReferenceException e)
            {

                Console.WriteLine("옥션혜택 1)적립/기타혜택  2)무이자 +할인 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {

                Console.WriteLine("옥션혜택 1)적립/기타혜택  2)무이자 +할인 IndexOutOfRangeException");
            }





            //쿠폰들을 가지고 온다.
            try
            {
                HtmlNode node7 = document.DocumentNode.SelectSingleNode("//script[contains(text(), 'frmCouponInfo')]");
                if (node7 != null)
                {
                    //쿠폰 rul을 타고 들어가서 값을 구한다.
                    coupon = node7.InnerText;
                    coupon = coupon.Trim();
                    index = coupon.IndexOf("frmCouponInfo");
                    ;
                    index2 = coupon.IndexOf("true");
                    coupon = coupon.Substring(index, index2 - index);
                    String[] couponlink = coupon.Split('"');
                    if (coupon == null)
                    {
                        coupon = "";
                    }

                    getif((String) couponlink.GetValue(4), 1);

                }
            }catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("옥션쿠폰 IndexOutOfRangeException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("옥션쿠폰 UriFormatException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("옥션쿠폰 NullReferenceException");
            }
            catch (ArgumentException e)
            {

                Console.WriteLine("옥션쿠폰 ArgumentException");
            }

            //옥션제휴카드
            try { 
            HtmlNode node8 = document.DocumentNode.SelectSingleNode("//li[@id='ucPlusDiscount_hdivCreditCard']");
            HtmlNode smallnode8 = document.DocumentNode.SelectSingleNode("//script[contains(text(), 'hfrmCreditCardPromotion')]");

                if (node8 != null)
                {

                    
                        String tmpurl;
                        //옥션제휴카드
                        var connode8 = node8.Descendants().Where(x => x.Name == "span");
                        profit4 = connode8.ElementAt(0).InnerText + connode8.ElementAt(1).InnerText;

                        //옥션 제휴 카드 종류
                        tmpurl = smallnode8.InnerHtml;
                        tmpurl = tmpurl.Trim();
                        index = tmpurl.IndexOf("src") + 7;
                        index2 = tmpurl.IndexOf(";") - 1;
                        tmpurl = tmpurl.Substring(index, index2 - index);

                        getif("http://itempage3.auction.co.kr" + tmpurl, 3);

                    

                }

            } catch (ArgumentException e)
            {
                profit5 = "";
                Console.WriteLine("옥션제휴 ArgumentException");
            }
            catch (UriFormatException e)
            {
                profit5 = "";
                Console.WriteLine("옥션제휴 UriFormatException");
            }
            catch (NullReferenceException e)
            {
                profit5 = "";
                Console.WriteLine("옥션제휴 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {
                profit5 = "";
                Console.WriteLine("옥션제휴 IndexOutOfRangeException");
            }



        }




        //링크타서 정보빼오는 함수
        public void getif(String link, int code)
        {
            HtmlNode tmpnode;
            HtmlAgilityPack.HtmlWeb web2 = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument document2 = web2.Load(link);
           
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead(link))
                {
                    var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));
                    var html = reader.ReadToEnd();
                    document2.LoadHtml(html);
                }
            }
            
            switch (code)
            {
                case 1:
                    var tmp = document2.DocumentNode.Descendants().Where(x => x.Name == "li");
                    if (tmp != null)
                    {
                        coupon = "";
                        foreach (var VARIABLE in tmp)
                        {
                            coupon += VARIABLE.InnerText.TrimStart();
                            coupon = coupon.TrimEnd();
                        }

                    }
                    break;
                /////////////////////////////////////////////////무이자 카드 정보
                case 2:
                    tmpnode = document2.DocumentNode.SelectSingleNode(".//table[@class='uxb-card-info']");
                    int count = 0;
                    if (tmpnode != null)
                    {
                        
                       foreach (var VARIABLE in tmpnode.Descendants().Where(x => x.Name == "tr" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("bodtop")))
                       {
                           
                        profit3 += VARIABLE.SelectSingleNode(".//img").GetAttributeValue("alt"," ");
                         
                           for (var i = 0; i <int.Parse(VARIABLE.SelectSingleNode(".//th").GetAttributeValue("rowspan", " "))*2;   i++)
                           {
                                profit3 += tmpnode.Descendants().Where(x => x.Name == "td").ElementAt(count).InnerText;
                                count++;
                            
                           }
                      
                       }
                    
                    }

                    break;

                case 3:
                     tmpnode = document2.DocumentNode.SelectSingleNode("//ul[@class='uxc-discount-list']");
                    if (tmpnode != null)
                    {
                        foreach (var VARIABLE in tmpnode.Descendants().Where(x => x.Name == "li"))
                        {
                            profit5 += VARIABLE.SelectSingleNode(".//p[@class='card-name']").InnerText;
                            foreach (var VARIABLE2 in VARIABLE.SelectSingleNode(".//p[@class='benefit-txt']").Descendants())
                            {
                                profit5 += VARIABLE2.InnerText.TrimStart();
                                profit5 = profit5.TrimEnd();
                            }


                        } 

                    }

                    break;



            }
            
        }


    
        //스트링배열로 데이터를 받는 함수
        public String[] getdatas()
        {
            
       
      
            storedata();
            bean bean = new bean();
            bean.itemnum = itemNum;
            bean.mall = mall;
            bean.name = name;
            bean.url = realurl;
            bean.category = category;
            if (realprice == "")
            {
                if (discountprice == "")
                {
                    if (cardprice=="")
                    {
                        
                    }
                    else
                    {
                        bean.price = cardprice;

                    }
                }
                else
                {
                    bean.price = discountprice;
                }   
            }
            else
            {
                bean.price = realprice;
            }
            
            bean.discountprice = discountprice+"//"+cardprice;
            bean.delivery = deliveryprice;
            bean.timeprofit = card+"//"+"//"+profit4+"//"+profit5;
            bean.etc = profit1+"//"+coupon;
            bean.image = image;
            bean.getdatas();
            return bean.datas;
            datas[4] = cardprice.Trim();
            datas[5] = card.Trim();
            datas[7] =profit1.Trim();
            datas[8] =profit2.Trim();
            datas[9] =profit3.Trim();
            datas[10]=profit4.Trim();
            datas[11]=profit5.Trim();
            datas[12]=coupon.Trim();
           
            return datas;
        }
        public string PdtName()
        {

            return name;
        }



    }
}
