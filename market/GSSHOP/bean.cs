using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSHOP
{
    class bean
    {
        public String mall;
        public String itemnum;
        public String name;
        public String url;
        public String category;
        public String price;
        public String discountprice;
        public String delivery;
        public String timeprofit;
        public String cardprofit;
        public String etc;
        public String image;
        public String[] datas;


        public String[] getdatas()
        {
            datas = new String[12];
            datas[0] = mall;
            datas[1] = itemnum;
            datas[2] = name;
            datas[3] = url;
            datas[4] = category;
            datas[5] = price;
            datas[6] = discountprice;
            datas[7] = delivery;
            datas[8] = timeprofit;
            datas[9] = cardprofit;
            datas[10] = etc;
            datas[11] = image;
            return datas;
        }


    }
}

