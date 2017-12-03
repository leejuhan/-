using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DANAWA
{
    public class danawa
    {
        //danawaurl
        public String danawalink;//danawa 상품정보 url
        private String realurl = "";// url 정보
        private String name = "";// 이름정보
        private String price = "";//가격정보
        private String profit = "";//할인카드
        private String delivery = "";//배송비
        private String[] datas;//데이터정보
        private List<String[]> danawalist;//다나와리스트
        private String itemNum;

        public void getdanawalink(String danawalink)
        {
            this.danawalink = danawalink;

        }




        //데이터를 저장하는 함수
        public void storedata()
        {
            //url 변수          

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument document = web.Load(danawalink);
            //encoding 해준다
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead(danawalink))
                {
                    var reader = new StreamReader(stream, Encoding.GetEncoding("euc-kr"));
                    var html = reader.ReadToEnd();
                    document.LoadHtml(html);
                }
            }
            //url,이름
            IEnumerable<HtmlAgilityPack.HtmlNode> values1 = document.DocumentNode.SelectSingleNode(".//div[@class='blog_list_area']").Descendants().Where(x => x.Name == "td" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("subject"));
            //가격정보
            IEnumerable<HtmlAgilityPack.HtmlNode> values2 = document.DocumentNode.Descendants().Where(x => x.Name == "span" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("now_price3"));
            //할인카드사 정보
            IEnumerable<HtmlAgilityPack.HtmlNode> values3 = document.DocumentNode.Descendants().Where(x => x.Name == "td" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("card_info"));
            //배송비 정보
            IEnumerable<HtmlAgilityPack.HtmlNode> values4 = document.DocumentNode.Descendants().Where(x => x.Name == "em" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("blog_delivery_price"));

            //데이터를 빼오기위해 필요한 변수들
            String totalhtml;
            int index;
            int index2;
            String urlhtml;
            int count = 0;

            foreach (HtmlAgilityPack.HtmlNode value in values1)
            {

                if (value.NodeType != HtmlAgilityPack.HtmlNodeType.Text)
                {
                    try
                    {
                        //url뽑아낸다
                        
                        urlhtml = value.SelectSingleNode(".//a").GetAttributeValue("href","");
                        realurl = geturl(urlhtml);

                        //사이트이름 뽑아낸다        
                        name =
                            value.SelectSingleNode(".//a")
                                .GetAttributeValue("onclick", "")
                                .Split('\'')
                                .ElementAt(
                                    value.SelectSingleNode(".//a").GetAttributeValue("onclick", "").Split('\'').Length -
                                    2);

                        //가격 뽑아낸다
                        
                        price = values2.ElementAt(count).InnerText.Trim();
                        price = price.Substring(0, price.Length - 1);

                        //카드혜택 뽑아낸다
                        totalhtml = values3.ElementAt(count).InnerText;
                        profit = totalhtml.Trim();

                        //배송비 뽑아낸다
                        totalhtml = values4.ElementAt(count).InnerText;
                        index = totalhtml.IndexOf('(') + 1;
                        index2 = totalhtml.IndexOf(')') ;
                        delivery = totalhtml.Substring(index, index2 - index).Trim();

                    }
                    catch (UriFormatException e)
                    {
                        //url을 뽑아오는 과정에서 exception이 발생 할 수 있다.
                        totalhtml = value.InnerHtml;
                        index = totalhtml.IndexOf(@"=") + 2;
                        index2 = totalhtml.IndexOf(@"target") - 2;
                        urlhtml = totalhtml.Substring(index, index2 - index);
                        urlhtml = "http://prod.danawa.com" + urlhtml;
                        realurl = urlhtml;

                    }
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
                    }
                    catch (ArgumentException) { Console.WriteLine("아이템넘버 exception"); }
                    datas = new string[6];
                    datas[0] = realurl.Trim();
                    datas[1] = name.Trim();
                    datas[2] = itemNum.Trim();
                    datas[3] = price.Trim();
                    datas[4] = profit.Trim();
                    datas[5] = delivery.Trim();
                    danawalist.Add(datas);
                    count++;

                }

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
            int index4 = content2.IndexOf(");") - 1;
            string result = content2.Substring(index3, index4 - index3);
            return result;
        }


        public List<String[]> getdatas()
        {
            danawalist = new List<string[]>();
            storedata();
            return danawalist;
        }



    }
}
