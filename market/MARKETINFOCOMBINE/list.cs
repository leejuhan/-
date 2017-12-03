using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARKETINFOCOMBINE
{
    class list
    {
        private List<string[]> PRODUCTlIST;
        public list()
        {
            PRODUCTlIST = new List<string[]>();
        }
        

        public void add(string mall, string itemNum)
        {
             try
            {
                string[] tmp = new string[2];
                tmp[0] = mall;
                tmp[1] = itemNum;
                int decide = 0;//decide는 안에 키 값 즉 itemNum에 같은 것이있나 없나 확인해 준다.
                foreach (var VARIABLE in PRODUCTlIST)
                {
                    if (VARIABLE[1] == itemNum)
                    {
                        decide = 1;
                        Console.WriteLine("같은 itemnumber가 이미 존재한다.");
                    }
                }
                if (decide == 0)
                {
                    Console.WriteLine("add 이상없이 완료");
                    PRODUCTlIST.Add(tmp);
                }
            }
            
            catch (InvalidOperationException)
            {
               
            }
            foreach (var VARIABLE in PRODUCTlIST)
            {
                Console.WriteLine(VARIABLE[1]);
            }
           
        }



        public void remove(int count)
        {

            PRODUCTlIST.RemoveAt(count);
        }


        public List<string[]> getlist()
        {
            return PRODUCTlIST;
        }

        public void removeall()
        {
            PRODUCTlIST.Clear();
        }


    }
}





/*
      try
      {
          string[] tmp = new string[2];
          tmp[0] = mall;
          tmp[1] = itemNum;
          //PRODUCTlIST.Add(tmp);
          if (!(PRODUCTlIST.Last().Contains(itemNum)))
          {
              PRODUCTlIST.Add(tmp);
          }
      }
      catch (InvalidOperationException)
      {
          string[] tmp = new string[2];
          tmp[0] = mall;
          tmp[1] = itemNum;
          PRODUCTlIST.Add(tmp);
      }
      */
/////////////////////////이미 윈도우 폼에서 확인한다.리스트에서 플러스 했을때 
/////////////////////////////////////////////////////////////////////////////////