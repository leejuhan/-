using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace _11MAKET
{
    public class eleven
    {
        private HtmlAgilityPack.HtmlWeb web;
        private HtmlAgilityPack.HtmlDocument document;
        private String mall = "";
        private String elevenlink;//eleven 상품정보  url
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String category = "";//카테고리
        private String realprice = "";//원래가격정보
        private String discountprice = "";//할인가격
        private String deliveryprice = "";//배송비
        private String profit1 = "";//무이자
        private String profit2 = "";//혜택
        private String profit3 = "";//괜찮아 할인가
        private String itemNum = "";
        private String image = "";
        String tmp2 = "";
        String[] datas;//데이터정보
        ArrayList elevendatalist = new ArrayList();//eleven resorce

        //생성자
        public eleven(String elevenlink,String itemNum, String mall)
        {
            this.elevenlink = elevenlink;
            realurl = elevenlink;
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
                document = web.Load(elevenlink);
                //encoding 해준다
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(elevenlink))
                    {
                        var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));
                        var html = reader.ReadToEnd();
                        document.LoadHtml(html);
                    }
                }

            }
            catch (ArgumentException e)
            {
                Console.WriteLine("11번가//url 변수 ArgumentException");
            }
            catch (WebException e)
            {
                Console.WriteLine("11번가 //url 변수 WebException");
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine("11번가 //url 변수 WebException");
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("11번가 //url 변수 WebException");
            }


            //11번가카테고리 정보

            try
            {
                String tmp;
                String[] tmp2;
                HtmlNode node = document.DocumentNode.SelectSingleNode("//script[contains(text(), '	areaCode :')]");
                tmp = node.InnerHtml;
                index = tmp.IndexOf("areaCode : 'prd'");
                index2 = tmp.Length;
                tmp = tmp.Substring(index, index2 - index);
                index = tmp.IndexOf("cat1 : ");
                index2 = tmp.IndexOf("}");
                tmp = tmp.Substring(index, index2 - index);
                tmp2 = tmp.Split('\'');
                tmp = "";
                foreach (var VARIABLE in tmp2)
                {
                    tmp += VARIABLE.Trim();
                }
                tmp2 = tmp.Split(',');
                tmp = "";
                foreach (var VARIABLE in tmp2)
                {
                    index = VARIABLE.IndexOf("cat")+6;
                    index2 = VARIABLE.Length;
                    tmp += VARIABLE.Substring(index, index2 - index).Trim()+" ";
                   
                }
                tmp.Trim();
                foreach (var VARIABLE in tmp.Split(' '))
                {
                    category += VARIABLE.Trim() + ">";
                }
                category = category.Substring(0, category.Length - 3);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("11번가카테고리 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("11번가카테고리 NullReferenceException");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("11번가카테고리 NullReferenceException");
            }




            //원래가격 정보
            try
            {
              realprice = document.DocumentNode.SelectSingleNode(".//li[@id='selPrcArea']").InnerText.Trim();
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("11번가원래가격 정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("11번가원래가격 정보 (NullReferenceException");
            }

            //상품이름 
            try
            {
                int tmp=0;
                name =document.DocumentNode.SelectSingleNode(".//div[@class='prdc_heading_v2']").Descendants().Where(x => x.Name == "h2").First().InnerText.Trim();

            }
            catch (ArgumentException e)
            {
                Console.WriteLine("11번가상품이름 ArgumentException");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("11번가상품이름 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("11번가상품이름 (NullReferenceException");
            }




            //할인가격정보
            try
            {
                discountprice = document.DocumentNode.SelectSingleNode(".//li[@id='dscSelPrcArea']").InnerText.Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("11번가할인가격정보 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("11번가할인가격정보 (NullReferenceException");
            }


            //배송비 
            try
            {
                deliveryprice =
                    document.DocumentNode.SelectSingleNode(".//span[@class='prdc_pointxt']").InnerText.Trim();
                deliveryprice +=
                    document.DocumentNode.SelectSingleNode(".//li[@name='dlvCstInfoView']").SelectSingleNode(".//span[@class='prdc_price']").InnerText.Trim();

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("11번가배송비 IndexOutOfRangeException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("11번가배송비 NullReferenceException");
            }


            //11번가무이자 혜택
            try
            {


                String[] tmp = document.DocumentNode.SelectSingleNode(".//div[@id='feeCard']").Descendants().Where(x => x.Name == "script").ElementAt(0).InnerText.Split('"');

                foreach (var VARIABLE in document.DocumentNode.SelectSingleNode(".//div[@id='feeCard']").Descendants().Where(x => x.Name == "script"))
                {
                    tmp = VARIABLE.InnerText.Split('"');
                    profit1 += tmp.ElementAt(7) + tmp.ElementAt(3) + "원" + tmp.ElementAt(9) + "월" + "\n";

                }
                profit1 = profit1.Trim();
            }
            catch (ArgumentException e)
            {

                Console.WriteLine("무이자 혜택 ArgumentException");
            }
            catch (UriFormatException e)
            {

                Console.WriteLine("11번가무이자 혜택 UriFormatException");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("11번가무이자 혜택 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {

                Console.WriteLine("11번가무이자 IndexOutOfRangeException");
            }




            //11번가 혜택
            try
            {
                profit2 = document.DocumentNode.SelectSingleNode(".//li[@id='benefitLi']").InnerText.Trim();
                String[] tmp = profit2.Split('\n');
                profit2 = "";
                foreach (var VARIABLE in tmp)
                {
                    profit2 += VARIABLE.Trim();
                }

            }
            catch (ArgumentException e)
            {

                Console.WriteLine("11번가제휴 ArgumentException");
            }
            catch (UriFormatException e)
            {


                Console.WriteLine("11번가제휴 UriFormatException");
            }
            catch (NullReferenceException e)
            {

                Console.WriteLine("11번가제휴 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {

                Console.WriteLine("11번가제휴 IndexOutOfRangeException");
            }


            //11번가 괜찮아 할인가
            try
            {
                foreach (var VARIABLE in document.DocumentNode.Descendants().Where(x => x.Name == "tr" && x.Attributes.Contains("name") && x.Attributes["name"].Value.Split().Contains("removeCasePrmt")))
                {
                    profit3 += VARIABLE.InnerText.Trim();
                }
                String[] tmp = profit3.Split('\n');
                profit3 = "";
                foreach (var VARIABLE in tmp)
                {
                    profit3 += VARIABLE.Trim();
                }
            }
            catch (ArgumentException e)
            {

                Console.WriteLine("11번가제휴 ArgumentException");
            }
            catch (UriFormatException e)
            {


                Console.WriteLine("11번가제휴 UriFormatException");
            }
            catch (NullReferenceException e)
            {

                Console.WriteLine("11번가제휴 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {

                Console.WriteLine("11번가제휴 IndexOutOfRangeException");
            }
            //11번가 이미지
            try
            {
                image =
                    document.DocumentNode.SelectSingleNode(".//span[@class='v-align']")
                        .SelectSingleNode(".//img")
                        .GetAttributeValue("src", "")
                        .Trim();
            }
            catch (ArgumentException e)
            {

                Console.WriteLine("11번가제휴 ArgumentException");
            }
            catch (UriFormatException e)
            {


                Console.WriteLine("11번가제휴 UriFormatException");
            }
            catch (NullReferenceException e)
            {

                Console.WriteLine("11번가제휴 NullReferenceException");
            }
            catch (IndexOutOfRangeException e)
            {

                Console.WriteLine("11번가제휴 IndexOutOfRangeException");
            }

        }



        //스트링배열로 데이터를 받는 함수
        public String[] getdatas()
        {
             /*
              상품이름
              아이템넘버
              mall 정보
              url
             카테고리
             원래가격
             할인가격
             배송비
             mall 이름
             무이자
             카드할인
             기타
             */
            storedata();
            bean bean =new bean();
            bean.itemnum = itemNum;
            bean.name = name;
            bean.mall = mall;
            bean.url = realurl;
            bean.category = category;
            if(realprice=="")
            {
                bean.price = discountprice;
            }
            else
            {
                bean.price = realprice;
            }
            bean.discountprice = discountprice;
            bean.delivery=deliveryprice;
            bean.timeprofit = profit1;
            bean.etc = profit2 +"//"+ profit3;
            bean.image = image;
            bean.getdatas();
            /*
            datas = new string[10];
            datas[0] = realurl.Trim();
            datas[1] = category.Trim();
            datas[2] = realprice.Trim();
            datas[3] = discountprice.Trim();
            datas[4] = deliveryprice.Trim();
            datas[5] = profit1.Trim();
            datas[6] = profit2.Trim();
            datas[7] = profit3.Trim();
            datas[8] = name.Trim();
            */
            return bean.datas;
        }

        public string PdtName()
        {

            return name;
        }
     








    }
}
