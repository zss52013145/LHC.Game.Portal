using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class Dzd
    {

        public int  UserId { get; set; }                    //

        public string Name { get; set; }                    //钱包名称

        public string NameForEnglish { get; set; }                    //钱包名称

        public object Current { get; set; }                 //当前额度    

        public object Prev { get; set; }                    //上次余额

        

        public object ZhuanRu { get; set; }                 //转入

        public object ZhuanChu { get; set; }                //转出

        public object Cun { get; set; }                     //存款

        public object Qu { get; set; }                      //取款

        public object YouHui { get; set; }                  //优惠赠送

        public object GameResult { get; set; }              //游戏结果

        public object FanShui { get; set; }                 //返水

        public object WeiJie { get; set; }                  //未结金额

        public object PrevWeiJie { get; set; }              //上次未结

        public object WeiKou { get; set; }                  //未扣金额

        public object Time1 { get; set; }                   //对账时间

        public object Time2 { get; set; }                   //截止时间
        

    }
}
