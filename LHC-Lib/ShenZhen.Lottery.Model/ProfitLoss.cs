using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public class ProfitLoss
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public decimal Money { get; set; }
        public decimal CurrentMoney { get; set; }
        public int Type { get; set; }
        public DateTime SubTime { get; set; }

        public int? OtherId { get; set; }
        
        public string Mark { get; set; }            //备注


        //扩展
        public string UserName { get; set; }
        public string RealName { get; set; }

        public string ShowTime
        {
            get { return this.SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }


        public string ShowUserName
        {
            get
            {
                return Util.GetEntityById<UserInfo>(this.UserId).UserName;
            }

        }


        public decimal ZhangBianQianMoney
        {
            get
            {

                if (Type == 13) 
                {
                    return CurrentMoney + Money;
                }

                return CurrentMoney - Money;
            }
        }


        public string ShowOp
        {
            get
            {
                string result = "";

                if (Type == 1)
                {
                    result = "投注";
                }
                else if (Type == 2)
                {
                    result = "返奖";
                }
                else if (Type == 3)
                {
                    result = "充值";
                }
                else if (Type == 4)
                {
                    result = "提现";
                }
                else if (Type == 5)
                {
                    result = "提现失败";
                }
                else if (Type == 6)
                {
                    result = "管理员操作";
                }
                else if (Type == 7)
                {
                    result = "撤单";
                }
                else if (Type == 8)
                {
                    result = "返水";
                }
                else if (Type == 9)
                {
                    result = "上分";
                }
                else if (Type == 10)
                {
                    result = "下分";
                }
                else if (Type == 11)
                {
                    result = "返佣";
                }
                else if (Type == 12)
                {
                    result = "接受上级转账";

                    if (OtherId != null)
                    {
                        UserInfo user = Util.GetEntityById<UserInfo>((int)OtherId);
                        result = "接受上级转账(" + user.UserName + ")";
                    }

                }
                else if (Type == 13)
                {
                    result = "给下级转账";

                    if (OtherId != null)
                    {
                        UserInfo user = Util.GetEntityById<UserInfo>((int)OtherId);
                        result = "给下级转账(" + user.UserName + ")";
                    }
                }

                return result;
            }
        }

    }
}
