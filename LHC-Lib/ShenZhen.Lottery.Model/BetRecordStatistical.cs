using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public class BetRecordStatistical
    {
        public int lType { get; set; }

        public string DateStr { get; set; }
        public string WeekStr { get; set; }
        public int Count { get; set; }
        public decimal WinMoney { get; set; }
        public decimal BetMoney { get; set; }
        public decimal TuiShui { get; set; }

        //
        public int UserId { get; set; }
        public string UserName { get; set; }



        public string ShowWinMoney
        {
            get
            {
                if (WinMoney < 0)
                {
                    return "<font style='color: red;'>" + this.WinMoney.ToString("0.00") + "</font>";
                }
                else if (WinMoney > 0)
                {
                    return "<font style='color: #128a12;'>" + this.WinMoney.ToString("0.00") + "</font>";
                }
                else
                {
                    return this.WinMoney.ToString("0.00");
                }

            }
        }

        public string lTypeName
        {
            get
            {
                return Util.GetlTypeName(lType);


                //string result = "";

                //if (lType == 1)
                //{
                //    result = "重庆彩";
                //}
                //else if (lType == 2)
                //{
                //    result = "快速重庆彩";
                //}
                //else if (lType == 3)
                //{
                //    result = "六合彩";
                //}
                //else if (lType == 4)
                //{
                //    result = "快速六合彩";
                //}
                //else if (lType == 5)
                //{
                //    result = "七星彩";
                //}
                //else if (lType == 6)
                //{
                //    result = "快速七星彩";
                //} 
                //else if (lType == 7)
                //{
                //    result = "北京PK10";
                //}
                //else if (lType == 8)
                //{
                //    result = "快速PK拾";
                //} 
                //else if (lType == 9)
                //{
                //    result = "幸运飞艇";
                //}
                //else if (lType == 11)
                //{
                //    result = "3D";
                //}
                //else if (lType == 12)
                //{
                //    result = "快速3D";
                //}
                //else if (lType == 13)
                //{
                //    result = "广东快乐十分";
                //}
                //else if (lType == 15)
                //{
                //    result = "广东11选5";
                //}
                //else if (lType == 17)
                //{
                //    result = "幸运农场";
                //}
                //else if (lType == 19)
                //{
                //    result = "排列三";
                //}
                //else if (lType == 21)
                //{
                //    result = "江苏快三";
                //}
                //else if (lType == 22)
                //{
                //    result = "快速三";
                //}
                //else if (lType == 23)
                //{
                //    result = "加拿大28";
                //}
                //return result;
            }
        }


        public string ShowBetMoney
        {
            get { return this.BetMoney.ToString("0.00"); }
        }


        public string ShowTuiShui
        {
            get { return this.TuiShui.ToString("0.00"); }
        }
    }
}
