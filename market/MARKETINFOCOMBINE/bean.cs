using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PROINFO;
namespace MARKETINFOCOMBINE
{
    class bean
    {


        private ArrayList PRODUCT;
        //private String[] Mallinfo;
        private List<string[]> CPNaverList;
        private List<string[]> CPDanawaList;
        private List<string[]> CPTotalList;
        private List<string[]> CPUptalList;
        private List<string[]> CPdownverList;
        private int grade = 0;
        private ProINfo ProINfo;
        private int samemallgrade = 0;
        private int samemallcount = 0;

        public ArrayList GetArrayList(string mall,string itemNum,string PdtName)
        {   ///////////////////////////////초기화 시켜준다
            ProINfo tmp = new ProINfo(mall,itemNum,PdtName);
            PRODUCT=new ArrayList();
            // Mallinfo=new string[11];
            CPNaverList=new List<string[]>();
            CPDanawaList= new List<string[]>();
            CPTotalList= new List<string[]>();
            CPUptalList= new List<string[]>();
            CPdownverList= new List<string[]>();
            /////////////////////////////////
            //Mallinfo = tmp.MALLinfo();
            /*
            ThreadStart ts1=new ThreadStart(tmp.getCPinfoNaverList);
            ThreadStart ts2 = new ThreadStart(tmp.getCPinfoDanawaList);

            Thread th1=new Thread(ts1);
            Thread th2= new Thread(ts2);
            th1.Start();
            th2.Start();
            */        
            tmp.getCPinfoNaverList();
            tmp.getCPinfoNaverList();
            CPNaverList = tmp.CPinfoNaverList;
            try
            {
                tmp.getCPinfoDanawaList();
                CPDanawaList = tmp.CPinfoDanawaList;
            }
            catch (AggregateException)
            {
                CPDanawaList = tmp.CPinfoDanawaList;

            }
           
         
            ////////////////밑에 데이터들은 분석함수를 돌려야 한다.
            
            tmp.AnalysisData();
            CPTotalList = tmp.getCPTotalList();
            CPUptalList = tmp.getCPupList();
            CPdownverList = tmp.getCPdownList();
            grade = tmp.getgrade();
           
            //PRODUCT.Add(Mallinfo);
            PRODUCT.Add(CPNaverList);
            PRODUCT.Add(CPDanawaList);
            PRODUCT.Add(CPTotalList);
            PRODUCT.Add(CPUptalList);
            PRODUCT.Add(CPdownverList);
            PRODUCT.Add(grade);
            PRODUCT.Add(tmp.highprice);
            PRODUCT.Add(tmp.rowprice);
            PRODUCT.Add(tmp.Totallistcount);
            PRODUCT.Add(tmp.avg);
            PRODUCT.Add(tmp.avgper);
            PRODUCT.Add(tmp.samemallcount);
            PRODUCT.Add(tmp.samemallgrade);
            PRODUCT.Add(tmp.lowcompare);
            PRODUCT.Add(tmp.highcompare);
            
            Console.WriteLine("maketengin의 bean함수 완료");
            return PRODUCT;
            

        }
        

       



    }
}
