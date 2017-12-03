using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AUCTION;
using CJMALL;
using CONNECTDANAWA;
using CONNECTNAVER;
using GMARKET;
using GSSHOP;
using NAVER;
using _11MAKET;


namespace PROINFO
{
    public class ProINfo
    {
        private String mall = "";//사이트몰
        private String name = "";//제품이름
        private int type;//몰의 타입
        private String itemNum = "";//아이템넘버
        private String url = "";//사이트몰 url
        private String PdtName = "";//제품이름
        private String price = "";//가격
        private connectnaver connectnaver;
        private connectdanawa connectdanawa;
        private naver naver;
        private auction auction;
        private gmarket gmarket;
        private gsshop gsshop;
        private eleven eleven;
        private cjmall cjmall;
        private String[] MALLinfos;//몰의 상세정보가 들어있는 데이터
       public List<String[]> CPinfoNaverList;//네이버 가격 비교 리스트
       public List<String[]> CPinfoDanawaList;//다나와 가격비교 리스트
        private List<String[]> CPinfoEnuriList;//에누리 가격비교 리스트
        ///////////////////////////////////////////////////////////////
        private int grade=0; //순위
        private List<String[]> CPTotalList;//총리스트
        private List<String[]> CPupList;//낮은가격 리스트
        private List<String[]> CPdownList;//높은 가격리스트

        /// //////////////////////////////////////////////////////////////
        public int highprice = 0;//가장 높은 가격
        public int rowprice =0;//가장 낮은 가격
        public int Totallistcount =0;//총 해당몰의 개수
        public int avg =0;//평균
        public float avgper = 0;//평균 퍼센트

        /// //////////////////////////////////////////////////////////////
        public int samemallgrade = 0;//같은 쇼핑몰에서 순위
        public int samemallcount = 0;//같은 쇼핑몰 갯수

        /// //////////////////////////////////////////////////////////////
        public List<int> highcompare;//가장 높은 가격-내가격
        public List<int> lowcompare;//내가격-가장 낮은 가격

        public ProINfo(String name, String itemNum,String PdtName)
        {
            this.itemNum = itemNum;
            this.name = name;
            this.PdtName=PdtName;
            if (name == "auction")
            {
                mall = "옥션";
            }
            else if (name == "gmarket")
            {
                mall = "G마켓";
            }
            else if (name == "gsshop")
            {
               
                mall = "GSSHOP";
            }
            else if (name == "eleven")
            {
              
                mall = "11번가";

            }
            else if (name == "cjmall")
            {
                mall = "CJmall";
            }
        }
        //상세데이터를 뽑아오는 함수이다.
        public String[] MALLinfo()
        {
            MALLinfos = new string[12];
            if (name == "auction")
            {
                auction = new auction("http://itempage3.auction.co.kr/DetailView.aspx?ItemNo=" + itemNum, itemNum, "auction");
                MALLinfos = auction.getdatas();
                mall = "옥션";
            }
            else if (name == "gmarket")
            {
                gmarket = new gmarket("http://item2.gmarket.co.kr/Item/detailview/Item.aspx?goodscode=" + itemNum, itemNum, "gmarket");
                MALLinfos = gmarket.getdatas();
                mall = "G마켓";
            }
            else if (name == "gsshop")
            {
                gsshop = new gsshop("http://www.gsshop.com/prd/prd.gs?prdid=" + itemNum, itemNum, "gsshop");
                MALLinfos = gsshop.getdatas();
                mall = "GSSHOP";
            }
            else if (name == "eleven")
            {
                eleven = new eleven("http://www.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=" + itemNum, itemNum, "eleven");
                MALLinfos = eleven.getdatas();
                mall = "11번가";

            }
            else if (name == "cjmall")
            {
                cjmall = new cjmall("http://www.cjmall.com/prd/detail_cate.jsp?item_cd=" + itemNum, itemNum, "cjmall");
                MALLinfos = cjmall.getdatas();
                mall = "CJmall";
            }
            return MALLinfos;
        }


            /// <summary>
            /// 총리슽를 토대로 분석데이터를 뽑는 함수이다.
            /// </summary>
            public void AnalysisData()
           {
            int count = 0;
            int count2 = 0;
            int decide = 0;//item 넘버로 찾을지 아님 가격으로 위치 찾을지 결정하는 변수
            int decide2 = 0;//up down 리스트를 구별하는 함수
            int count3 = 0;//같은몰
            int count4 = 0;//같은몰중에서도 가격 높은거
            CPTotalList=new List<string[]>();
            CPupList = new List<string[]>();//가격이 높은거
            CPdownList = new List<string[]>();//가격이 낮은거
            CPTotalList.AddRange(CPinfoNaverList);
            CPTotalList.AddRange(CPinfoDanawaList);
           
            try
            {
                CPTotalList.Sort(delegate (String[] x, String[] y)
                {
                    String tmpx = "";
                    foreach (var VARIABLE in x.ElementAt(3).Split(','))
                    {
                        tmpx += VARIABLE;
                    }
                    String tmpy = "";
                    foreach (var VARIABLE2 in y.ElementAt(3).Split(','))
                    {
                        tmpy += VARIABLE2;
                    }
                    return int.Parse(tmpx).CompareTo(int.Parse(tmpy));
                });

               foreach (var VARIABLE3 in CPTotalList)
                {
                    count++;
                    CPdownList.Add(VARIABLE3);
                    if (VARIABLE3.ElementAt(1)==mall)
                    {
                        count3++;
                        count4++;
                    }
                    if (VARIABLE3.ElementAt(2) == itemNum)
                    {
                        decide = 1;
                        break;
                    }
                }
                if (decide == 1)
                {
                    int tmp = count;

                    for (var i = tmp + 1; tmp < CPTotalList.Count; tmp++)
                    {
                        if (CPTotalList.ElementAt(tmp).ElementAt(1) == mall)
                        {
                            count3++;
                        }
                        CPupList.Add(CPTotalList.ElementAt(tmp));
                    }

                }
                Console.WriteLine("현재 상품은 이위치에 있습니다item넘버로찾은위치" + count);
                grade = count;
                if (decide == 0)
                {

                    foreach (var VARIABLE4 in CPTotalList)
                    {

                        count2++;
                        CPdownList.Add(VARIABLE4);
                        if (VARIABLE4.ElementAt(1) == mall)
                        {
                            count3++;
                            count4++;
                        }
                        if (VARIABLE4.ElementAt(2) == itemNum)
                        {
                            decide = 1; break;
                        }
                    }
                    if (decide == 1)
                    {
                        int tmp = count2;
                        for (var i = tmp + 1; tmp < CPTotalList.Count; tmp++)
                        {
                            if (CPTotalList.ElementAt(tmp).ElementAt(1) == mall)
                            {
                                count3++;
                            }
                            CPupList.Add(CPTotalList.ElementAt(tmp));
                        }
                    }
                    Console.WriteLine("현재 상품은 이위치에 있습니다가격으로찾은위치" + count);
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////
                String tmph = "";
                foreach (var VARIABLE in CPTotalList.Last().ElementAt(3).Split(','))
                {
                    tmph += VARIABLE;
                }
                highprice = int.Parse(tmph);
                /////////////////////////
                String tmpd = "";
                foreach (var VARIABLE in CPTotalList.First().ElementAt(3).Split(','))
                {
                    tmpd += VARIABLE;
                }
                rowprice = int.Parse(tmpd);
                ////////////////////////
                Totallistcount = CPTotalList.Count;
                ////////////////////////
                avg = (int)((highprice + rowprice) / 2);
                //////////////
                float tmp3 = rowprice;
                float tmp2 = highprice;
                avgper =((tmp3/tmp2)*100);
                Math.Round(avgper, 1);
                /////////////////////
                samemallcount = count3;
                ////////////////////
                samemallgrade = count4;
                //////////////////////
                
                highcompare=new List<int>();
                highcompare = new List<int>();
               
                try
                {
                    foreach (var VARIABLE in CPTotalList)
                    {
                        String tmp = "";
                        foreach (var VARIABLE2 in VARIABLE.ElementAt(3).Split(','))
                        {
                            tmp += VARIABLE2;
                        }
                     highcompare.Add(highprice-int.Parse(tmp));

                     lowcompare.Add(int.Parse(tmp) - rowprice); 

                    }
                }
                catch (FormatException)
                {
                    
                }
                catch (NullReferenceException)
                {

                }




            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("totallist를 뽑는 과정에서 익셉션 발생 ");
                
            }
        

        }
        /// <summary>
        /// 네이버리스트를 뽑는 함수이다
        /// </summary>
        public void getCPinfoNaverList()
        {
            try
            {
                Console.WriteLine(PdtName);
                connectnaver = new connectnaver(PdtName);
                if (connectnaver.naverlink() != "")
                {
                    naver = new naver(connectnaver.naverlink());
                    naver.storedata();
                    CPinfoNaverList = naver.getdatas();
                    connectnaver.storedata();
                    CPinfoNaverList.AddRange(connectnaver.getdatas());

                }
                else
                {
                    connectnaver.storedata();
                    Console.WriteLine(PdtName);
                    CPinfoNaverList = connectnaver.getdatas();
                }

            }
            catch (System.IO.IOException)
            {

                CPinfoNaverList=new List<string[]>();
            }

          
           

        }
        /// <summary>
        /// 다나와 리스트를 뽑는 함수이다.
        /// </summary>
        public void getCPinfoDanawaList()
        {
         
            try
            {
               
                connectdanawa = new connectdanawa(PdtName);
                CPinfoDanawaList = connectdanawa.getdatas();
                
            }
            catch (System.IO.IOException)
            {
                CPinfoDanawaList=new List<string[]>();
                Console.WriteLine("다나와 리스트getcp다나와 함수 IOException");

            }
            catch (AggregateException)
            {
                CPinfoDanawaList = new List<string[]>();
                Console.WriteLine("다나와 리스트getcp다나와 함수 AggregateException");

            }

        }
        public List<String[]> getCPTotalList()
        {

            return CPTotalList;

        }

        public List<String[]> getCPupList()
        {
            return CPupList;
        }
        public List<String[]> getCPdownList()
        {
            return CPdownList;
        }

        public int getgrade()
        {
            return grade;
        }

        public List<int> lowcompares()
        {
            return lowcompare;
        }

        public List<int> highcompares()
        {
            return highcompare;
        }

    }
}



