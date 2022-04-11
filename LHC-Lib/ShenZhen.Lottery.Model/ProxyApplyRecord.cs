using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public class ProxyApplyRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public string Reason { get; set; }
        public DateTime SubTime { get; set; }
        public int State { get; set; }
        public string FailReason { get; set; }

        public UserInfo User { get; set; }


        //扩展属性

        public string ShowTime
        {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string UserName
        {
            get
            {
                User = Util.GetEntityById<UserInfo>(this.UserId);

                if (User != null)
                {
                    return User.UserName;
                }

                return "";
            }
        }



        public string RealName
        {
            get
            {
                if (User != null)
                {
                    return User.RealName;
                }

                return "";
            }
        }



        public int LoginCount
        {
            get
            {
                if (User != null)
                {
                    return User.LoginCount;
                }

                return 0;
            }
        }



        
        public string ShowState {
            get
            {
                if (this.State == 0)
                {
                    return "未处理";
                }
                else if (this.State == 1)
                {
                    return "<font color='red'>通过</fond>";
                }
                else if (this.State == 2)
                {
                    return "未通过";
                }

                return "";
            }
        }


    }
}
