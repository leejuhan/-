using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using PROINFO;
namespace MARKETINFOCOMBINE
{
    public class marketengin
    {
        public Dictionary<string, ArrayList> PRODUCTDATAS;//키는 제품번호, 값은 데이터를 넣어둔 어레이리스트이다.
        private list list;//리스트는 상세몰에대한 데이터를 넣어두는 클래스이다.
        private bean bean;//bean은 상세몰 데이터를 제외한 리스트데이터와 기타 데이터가 있는 부분이다.
        private List<String[]> Mallinfo;//상세몰에대한 데이터를 가지고 있는 리스틍트이다.
        private ProINfo ProINfo;
        public List<String[]> inquryList;//조회할 상품들이 있는 리스트이다
        public TimeSpan ellapsed;//조회할대 시간이 얼마나 걸리는지 측정하는변수
        public int excelorhand = 0;//엑셀로 추가시키는 함수인지 아닌지 정하는 변수
        public string mall="";//사이트 몰
        public string itemNum = "";//제품 넘버
        public List<string[]> excelstartList;//엑셀로 받아온 조회할 목록데이터가 있는 리스트이다.
        /// 총 조회시간
        public int stop = 0;

        public marketengin()
        {
           
            Mallinfo =new List<string[]>();
            inquryList = new List<string[]>();
            PRODUCTDATAS = new Dictionary<string, ArrayList>();
            excelstartList = new List<string[]>();
            list =new list();
        }

        /// <summary>
        /// ///////입력해서추가시킨 몰과 이템넘버가 있는 리스트 이다.
        /// </summary>
        /// <param name="mall"></param>
        /// <param name="itemNum"></param>
        /// <returns></returns>
        public List<string[]> add(string mall,string itemNum)
        {
            excelorhand = 1;
            this.mall = mall;
            this.itemNum = itemNum;
            list.add(mall, itemNum);
            foreach (var VARIABLE in list.getlist())
            {
                Console.WriteLine(VARIABLE[1]);
            }
            return list.getlist();
        }
        /// <summary>
        /// ////////////엑셀로 가져온 데이터
        /// </summary>
        /// <param name="count"></param>
        /// <param name="itemNum"></param>
        public List<string[]> exceladd(List<string[]> addlist)
        {
            foreach (var VARIABLE in addlist)
            {
               this.list.add(VARIABLE[1], VARIABLE[2]);
            }
            return list.getlist(); ;
        }


        //메인스크린 리스트 뽑는 함수
        public void MainscreenList()
        {
         
            int count = 0;
            if (excelorhand == 1)
            {
                if (Mallinfo.Count == 0)
                {
                    ProINfo = new ProINfo(this.mall, this.itemNum, "");
                    Mallinfo.Add(ProINfo.MALLinfo());
                    excelorhand = 0;
                    this.mall = "";
                    this.itemNum = "";

                }
                else
                {
                    int tmp = 0;
                    foreach (var VARIABLE in Mallinfo)
                    {
                        if (VARIABLE[1] == itemNum)
                        {
                            Console.WriteLine("mallinfo안에 있다.");
                            tmp = 1;
                        }   
                    }
                    if (tmp == 0)
                    {
                        
                        ProINfo = new ProINfo(this.mall, this.itemNum, "");
                        Mallinfo.Add(ProINfo.MALLinfo());
                    }
                    excelorhand = 0;
                    this.mall = "";
                    this.itemNum = "";

                }
               

            }
            else
            {
            ////////////엑셀로 받아온 리스트들의 상세몰 데이터를 테스크쓰레드를 이용해서 받아오는 부분이다.
                Func<Object, string[]> inqury2 = (objRange) =>
                {
                    String[] tmp = (String[])objRange;
                    if (Mallinfo.Count == 0)
                    {

                        ProINfo ProINfo2 = new ProINfo(tmp[0],tmp[1] , "");
                        Console.WriteLine(count + "번째함수");
                        return ProINfo2.MALLinfo();
                    }
                    else
                    {

                        int tmp2 = 0;
                        
                        foreach (var VARIABLE2 in Mallinfo)
                        {
                            if (VARIABLE2[1] == tmp[1])
                            {
                                tmp2 = 1;
                            }
                        }
                        if (tmp2 ==0)
                        {
                            ProINfo ProINfo3 = new ProINfo(tmp[0], tmp[1], "");
                            excelorhand = 0;
                            this.mall = "";
                            this.itemNum = "";
                            count++;
                            return ProINfo3.MALLinfo();
                        }
                        string[] tmp5=new string[2];
                        return tmp5;
                    }
                };

                Task<string[]>[] tasks = new Task<string[]>[excelstartList.Count];
                
                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine("{0} 번째", i);
                    try
                    {
                        Console.WriteLine(excelstartList.ElementAt(i)[1]);
                        tasks[i] = new Task<string[]>(inqury2, excelstartList.ElementAt(i));
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("해당 아이템넘버로 해당 상품이 없다");
                    }

                }

                foreach (Task<string[]> task in tasks)
                {
                    task.Start();
                }
                foreach (Task<string[]> task in tasks)
                {
                    
                    Mallinfo.Add(task.Result);
                    count++;
                    task.Wait();
                  
                }


            }        
           
        }
        /// <summary>
        /// 메인화면에 엑셀로받아온 댁배정보로 리스트를 뽑는 부분이다.
        /// </summary>
        public void MainscreenListstartexcel()
        {

            foreach (var VARIABLE in list.getlist())
            {
                 excelstartList.Add(VARIABLE);
                 
                if (excelstartList.Count == 10)
                {
                    MainscreenList();
                    excelstartList.Clear();
                }

            }
            MainscreenList();
            excelstartList.Clear();
            int count = 0;
            List<int> remove=new List<int>();
            foreach (var VARIABLE in Mallinfo)
            {
                if (VARIABLE.Length == 2)
                {
                    remove.Add(count);
                }
                count++;
            }
            foreach (var VARIABLE in remove)
            {
                Mallinfo.RemoveAt(VARIABLE);                
            }


        }










        /// <summary>
        /// ///////MallinfoList 뽑는 함수
        /// </summary>
        /// <param name="count"></param>
        /// <param name="itemNum"></param>
        public List<string[]> getMallinfo()
        {
            return Mallinfo;
        }


        /// <summary>
        /// //삭제버튼을 눌렀을때 버튼
        /// </summary>
        /// <param name="count"></param>
        /// <param name="itemNum"></param>
        public void remove(int count,string itemNum)
        {
            list.remove(count);
            PRODUCTDATAS.Remove(itemNum);
            Mallinfo.RemoveAt(count);
        }

        /// <summary>
        /// ///////추가 버튼을 눌렀을때 작동하는 함수
        /// </summary>
        public void addinqurylist(List<int> list )
        {
            inquryList.Clear();
            foreach (var VARIABLE in list)
            {
                inquryList.Add(Mallinfo.ElementAt(VARIABLE));
            }
            ////////////////그전에 조회했던 키가 있는지 확인한는 것///////////전에 조회했던게 있다면 삭제
            List<string> already = new List<string>();
            int count = 0;
            int samekeycount = 0;
            foreach (var VARIABLE2 in inquryList)
            {
                Console.WriteLine(VARIABLE2[1]);
                
                if (this.PRODUCTDATAS.ContainsKey(VARIABLE2[1]))
                {
                    samekeycount++;
                    already.Add(VARIABLE2.Last());
                 
                }
                else
                {

                }
                count++;
                
            }
            if (samekeycount == 0)
            {
                Console.WriteLine("조회하는 리스트중에 전에 조회했던 상품은 없다.");
            }
            else
            {
                foreach (var VARIABLE3 in already)
                {
                    try
                    {
                       
                        int tmp = inquryList.FindIndex(x => x.Last().Contains(VARIABLE3));
                        inquryList.RemoveAt(tmp);
                    }
                    catch (ArgumentException) {
                       

                    }

                }
                Console.WriteLine("전에 조회했던 적이 있는 상품이"+samekeycount+"개 만큼 있다.");
            }
            

        }
        public List<string[]> getinqurylist()
        {
            return inquryList;
        }


      
        /// <summary>
        /// 아이템넘버 딕션어리의 키를 이용해 에레이리스트를 얻는 부분
        /// </summary>
        /// <param name="itemNum"></param>
        /// <returns></returns>

        public ArrayList getproduct(string itemNum)
        {
            ArrayList PRODUCT=new ArrayList();
            PRODUCT = PRODUCTDATAS[itemNum];
            return PRODUCT;
           
        }

        public List<string[]> getlist()
        {
            return list.getlist();
        }

        
       /// <summary>
       /// 조회버튼을 눌렀을때 작동하는 테스크스레드 부분
       /// </summary>
        
        public void inqury()
        {

            if (inquryList.Count != 0)
            {

                Func<Object, ArrayList> inqury1 = (objRange) =>
                {
                    bean bean = new bean();
                    String[] tmp = (String[]) objRange;
                    ArrayList PRODUCT = new ArrayList();
                    PRODUCT = bean.GetArrayList(tmp[0], tmp[1], tmp[2]);
                    PRODUCT.Add(tmp);
                    return PRODUCT;
                };

                Task<ArrayList>[] tasks = new Task<ArrayList>[inquryList.Count];


                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine("{0} 번째", i);
                    try
                    {
                        tasks[i] = new Task<ArrayList>(inqury1, inquryList.ElementAt(i));
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("해당 아이템넘버로 해당 상품이 없다");
                    }

                }


             

                foreach (Task<ArrayList> task in tasks)
                {
                    task.Start();
                }
                int count = 0;
                foreach (Task<ArrayList> task in tasks)
                {
                    task.Wait();
                    try
                    {
                        PRODUCTDATAS.Add(inquryList.ElementAt(count)[1], task.Result);
                        count++;
                    }
                    catch (ArgumentException)
                    {
                        
                        
                    }
                   
                }

              

            }

        }
        /// <summary>
        /// ////////////////////////////////////테스크가 너무 많이 돌아가면 잘 돌아가지않는다. 때문에 5개씨 나눠서 돌아가게한다.
        /// </summary>
        public void inqurystart(List<int> list)
        {
            stop = 0;
            DateTime starTime = DateTime.Now;
            List<int> tmplist=new List<int>();
            foreach (var VARIABLE in list)
            {
                tmplist.Add(VARIABLE);
                if (tmplist.Count == 12)
                {
                    addinqurylist(tmplist);
                    inqury();
                    tmplist.Clear();
                    foreach (var VARIABLE2 in PRODUCTDATAS)
                    {
                        Console.WriteLine(VARIABLE2.Key);
                    }
                }

            }
            addinqurylist(tmplist);
            inqury();
            tmplist.Clear();
            foreach (var VARIABLE2 in PRODUCTDATAS)
            {
                Console.WriteLine(VARIABLE2.Key);
            }

            DateTime endTime = DateTime.Now;

            ellapsed = endTime - starTime;

            Console.WriteLine("Ellapsed time : {0}", ellapsed);
            stop = 1;

           
        }
        /// <summary>
        /// 모든부분을 삭제하는 버튼이다.
        /// </summary>
        public void removeall()
        {
        PRODUCTDATAS.Clear();
        Mallinfo.Clear();
        inquryList.Clear();
        list.removeall();
        }
            





       












    }
}
