using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GMARKET
{
    public class gmarket
    {
        ArrayList gmarketdatalist = new ArrayList();//gmarket resorce
        String[] datas;//데이터정보
        private HtmlAgilityPack.HtmlWeb web;
        private HtmlAgilityPack.HtmlDocument document;
        private String gmarketlink;//gmarket 상품정보  url
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String category = "";//카테고리
        private String realprice = "";//원래가격정보
        private String discountprice = "";//할인가격
        private String deliveryprice = "";//배송비
        private String profit1 = "";//구매혜택,바로접속혜택
        private String cardprofit = "";//g마켓제휴
        private String profit2 = "";//세이부할인
        private String profit3 = "";//무이자카드
        private String image = "";
        private String mall;
        private String itemNum = "";


        //생성자
        public gmarket(String gmarketlink, String itemNum, String mall)
        {
            this.gmarketlink = gmarketlink;
            realurl = gmarketlink;
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
                document = web.Load(gmarketlink);
                //encoding 해준다
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(gmarketlink))
                    {
                        var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));
                        var html = reader.ReadToEnd();
                        document.LoadHtml(html);
                    }
                }

            }
            catch (ArgumentException e)
            {
                Console.WriteLine("gmarket//url 변수 ArgumentException");
            }
            catch (WebException e)
            {
                Console.WriteLine("gmarket//url 변수 WebException ");
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("gmarket //url 변수 WebException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("gmarket //url 변수 WebException");
            }



            //g마켓카테고리 정보

            try
            {
                var node1 = document.DocumentNode.SelectSingleNode("//div[@class='category']").Descendants().Where(x => x.Name == "span");
                if (node1 != null)
                {
                    foreach (var VARIABLE in node1)
                    {
                        category += VARIABLE.SelectSingleNode(".//a").InnerText.Trim()+">";
                    }
                    category = category.Substring(6, category.Length - 1-6);



                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("g마켓카테고리 정보 NullReferenceException");
            }






            // g마켓원래가격정보/할인가격정보
            try
            {
                realprice = document.DocumentNode.SelectSingleNode(".//tr[@name='trCostPrice0']").InnerText.Trim();
                String[] tmp;
                discountprice = document.DocumentNode.SelectSingleNode(".//tr[@id='trCostPrice1']").InnerText.Trim();
                tmp = discountprice.Split('\n');
                discountprice = "";
                foreach (var VARIABLE in tmp)
                {
                    discountprice += VARIABLE.Trim();
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("g마켓원래가격정보/할인가격정보 InvalidOperationException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("g마켓원래가격정보/할인가격정보 NullReferenceException");
            }

            // g마켓상품이름
            try
            {
                int tmp = 0;
                name =
                    document.DocumentNode.SelectSingleNode(".//head")
                        .Descendants()
                        .Where(x => x.Name == "title")
                        .First()
                        .InnerText.Trim();
                tmp = name.IndexOf('-')+1;
                name = name.Substring(tmp, name.Length - tmp).Trim();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(" g마켓상품이름 InvalidOperationException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(" g마켓상품이름 NullReferenceException");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(" g마켓상품이름 ArgumentException");
            }


            //배송비 
            try
            {
                HtmlNode node4 = document.DocumentNode.SelectSingleNode("//td[@class='smart_ship']");
                if (node4 != null)
                {

                    // a class="detail"

                    if (node4.SelectSingleNode(".//a").GetAttributeValue("class", "") == "detail")
                    {
                        String[] tmp2 = node4.InnerText.Split('\n');

                        foreach (var VARIABLE in tmp2)
                        {
                            deliveryprice += VARIABLE.Trim();
                        }
                        index = deliveryprice.IndexOf("&nbsp") - 12;
                        deliveryprice = deliveryprice.Substring(0, index).TrimEnd();
                    }



                }
            }
            catch (NullReferenceException e)
            {
                HtmlNode node4 = document.DocumentNode.SelectSingleNode("//td[@class='smart_ship']");
                deliveryprice = node4.SelectSingleNode(".//strong").InnerText;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("g마켓배송비 ArgumentOutOfRangeException");
            }


            //이미지
            try
            {
                image =
                    document.DocumentNode.SelectSingleNode(".//div[@class='img_cv']")
                        .SelectSingleNode(".//span")
                        .SelectSingleNode(".//img")
                        .GetAttributeValue("src", "")
                        .Trim();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("g마켓이미지 ArgumentOutOfRangeException");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("g마켓이미지 ArgumentOutOfRangeException");
            }



            //g마켓구매혜택,바로접속혜택
            try
            {
                IEnumerable<HtmlAgilityPack.HtmlNode> nodes =
                    document.DocumentNode.Descendants()
                        .Where(
                            x =>
                                x.Name == "table" && x.Attributes.Contains("class") &&
                                x.Attributes["class"].Value.Split().Contains("goods-info2"));
                HtmlNode node5 = nodes.ElementAt(1);
                if (node5 != null)
                {
                    //바로구매혜택
                    var tmp = node5.Descendants().Where(x =>
                        x.Name == "div" && x.Attributes.Contains("class") &&
                        x.Attributes["class"].Value.Split().Contains("in-cont")).Last();
                    String[] tmp2 = tmp.InnerText.Split('\n');
                    foreach (var VARIABLE in tmp2)
                    {
                        profit1 += VARIABLE.Trim();
                    }
                    index = profit1.IndexOf("자세히");
                    profit1 = "바로구매혜택 : " + profit1.Substring(0, index);
                    //구매혜택
                    if (node5.Descendants().Where(x =>
                            x.Name == "strong" && x.Attributes.Contains("class") &&
                            x.Attributes["class"].Value.Split().Contains("tit")).ElementAt(0).InnerText
                    == "구매혜택")
                    {
                        tmp = node5.Descendants().Where(x =>
                            x.Name == "div" && x.Attributes.Contains("class") &&
                            x.Attributes["class"].Value.Split().Contains("in-cont")).ElementAt(0);
                        tmp2 = tmp.InnerText.Split('\n');
                        profit1 += "//";
                        foreach (var VARIABLE2 in tmp2)
                        {
                            profit1 += " " + VARIABLE2.Trim();
                        }

                        String tmp3 = node5.Descendants().Where(x =>
                            x.Name == "div" && x.Attributes.Contains("class") &&
                            x.Attributes["class"].Value.Split().Contains("icon-info")).First().GetAttributeValue("id", "");

                        if (tmp3 == "benefitRow")
                        {
                            profit1 += node5.Descendants().Where(x => x.Name == "em").First().InnerText + "P";

                        }
                        else if (tmp3 == "benefitRow2")
                        {
                            profit1 += "사은품 :" + node5.SelectSingleNode(".//p[@class='']").InnerText;
                        }




                    }

                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("g마켓구매혜택,바로접속혜택 NullReferenceException e");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("g마켓구매혜택,바로접속혜택 ArgumentOutOfRangeException");
            }



            //g마켓제휴카드 할인
            try
            {
                IEnumerable<HtmlAgilityPack.HtmlNode> nodes = document.DocumentNode.Descendants().Where(x => x.Name == "table");
                HtmlNode node6 = nodes.ElementAt(3);
                if (node6 != null)
                {
                    var tmp = node6.Descendants().Where(x => x.Name == "tr");
                    foreach (var node in tmp)
                    {
                        cardprofit += node.SelectSingleNode(".//img").GetAttributeValue("alt", "");
                        cardprofit += node.SelectSingleNode(".//strong").InnerText.TrimStart();
                        cardprofit += node.SelectSingleNode(".//a[@class='cl-style01']").InnerText.TrimStart();
                        cardprofit +=
                            node.Descendants()
                                .Where(
                                    x =>
                                        x.Name == "a" && x.Attributes.Contains("class") &&
                                        x.Attributes["class"].Value.Split().Contains("cl-style01"))
                                .Last()
                                .InnerText.TrimStart();
                        cardprofit += "\n";
                    }
                    cardprofit = cardprofit.TrimEnd();
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("g마켓제휴카드 ArgumentOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("g마켓제휴카드 NullReferenceException");
            }

            //////////////무이자 혜택//세이브혜택
            try
            {
                IEnumerable<HtmlAgilityPack.HtmlNode> nodes = document.DocumentNode.Descendants().Where(x => x.Name == "table");
                HtmlNode node7 = nodes.ElementAt(3);
                if (node7 != null)
                {
                    String[] tmp;
                    tmp = node7.SelectSingleNode(".//a[@class='cl-style01']").GetAttributeValue("onmouseover", "").Split('\'');
                    getif(tmp[3], 1);
                    getif(tmp[3], 2);

                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("g마켓무이자 혜택,세이브혜택 ArgumentOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("g마켓무이자 혜택,세이브혜택 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("g마켓무이자 혜택,세이브혜택 IndexOutOfRangeException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("g마켓무이자 혜택,세이브혜택 UriFormatException");
            }



        }




        //링크타서 정보빼오는 함수
        public void getif(String link, int code)
        {
            try
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
                        ;
                        tmpnode = document2.DocumentNode.SelectSingleNode(".//div[@id='free_interest_layer_02']");

                        foreach (var VARIABLE in tmpnode.Descendants().Where(x => x.Name == "img"))
                        {
                            profit2 += VARIABLE.GetAttributeValue("alt", " ");
                        }

                        break;

                    case 2:
                        getif("http://promotion.gmarket.co.kr/Event/Common/gen/cardbenefit_gen.js", 3);

                        break;

                    case 3:
                        //Json
                        String tmp = document2.DocumentNode.InnerText;
                        String jsonstr = System.Text.RegularExpressions.Regex.Unescape(tmp);
                        //수정 변경이 필용함
                        String[] card = {"현대카드","kb국민카드","신한카드","citi","삼성카드","롯데카드","하나","농협"};
                        var index = 0;
                        var index2 = 0;
                        var index3 = 0;
                        index = jsonstr.IndexOf("{\"card_") ;
                        index2 = jsonstr.IndexOf("card_halbu_shop_goods_list")-2;
                        jsonstr = jsonstr.Substring(index, index2-index).Trim()+"}";
                     
                        JObject jobj = JObject.Parse(jsonstr);
                        JArray jarr = JArray.Parse(jobj["card_halbu_list"].ToString());

                        foreach (JObject jobj2 in jarr)
                        {
                            profit3 += card[index3] + jobj2["date"] + jobj2["halbu"] + jobj2["condition"];
                            index3++;
                        }
                        
                        break;


                }

            }
            catch (WebException e)
            {
                Console.WriteLine("g마켓링크타서 정보빼오는 함수 WebException");
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("g마켓링크타서 정보빼오는 함수 JsonReaderException");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("g마켓링크타서 정보빼오는 함수 JsonReaderException");
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
            bean.timeprofit = profit3;
            bean.mall = mall;
            bean.image = image;
            bean.cardprofit = cardprofit;
            bean.etc =profit1+"//"+profit2;
            bean.getdatas();
            return bean.datas;
        }


        public string PdtName()
        {

            return name;
        }









    }
}
