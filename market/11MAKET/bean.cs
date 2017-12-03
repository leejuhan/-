using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11MAKET
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
/*
public String[] getdatas()
{
     상품이름
     아이템넘버
     mall 정보
     urls
    카테고리
    원래가격
    할인가격
    배송비
    mall 이름
    무이자
    카드할인
    기타
           storedata();
            bean bean = new bean();
            bean.itemnum = itemNum;
            bean.name = name;
            bean.url = realurl;
            bean.category = category;
            bean.price = profit3;
            bean.discountprice = profit4+"//"+profit5+"//"+profit8+"//"+profit9+"//"+profit1;
            bean.delivery = deliveryprice;
            bean.timeprofit = profit1;
            bean.mall = mall;
            bean.cardprofit = profit7;
            bean.etc = profit2;
            bean.getdatas();
            return bean.datas;
}
*/
