using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class OpRecord
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AdminId { get; set; }

        public int Money { get; set; }

        public string Mark { get; set; }

        public DateTime SubTime { get; set; }



        //-----------------

        public string ShowAdmin
        {
            get
            {
                Admin admin = Util.GetEntityById<Admin>(AdminId);

                if (admin != null)
                {
                    return admin.RealName;
                }

                return "";
            }
        }

        public string ShowUserName
        {
            get { return Util.GetEntityById<UserInfo>(UserId).UserName; }
        }


        public string ShowUser
        {
            get { return Util.GetEntityById<UserInfo>(UserId).RealName; }
        }


        public string ShowNameWithStar
        {
            get
            {
                UserInfo user = Util.GetEntityById<UserInfo>(UserId);

                if (!string.IsNullOrEmpty(user.NickName))
                {
                    return Util.GetStarName(user.NickName);
                }
                else
                {
                    return Util.GetStarName(user.UserName);
                }

            }
        }

        public int ShowMoney
        {
            get
            {
                if (Money < 0)
                {
                    return -Money;
                }

                return Money;
            }
        }


        public string ShowTime
        {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ShowOp
        {
            get
            {
                if (Money > 0)
                {
                    return "<font color='red'>加</font>";
                }
                else
                {
                    return "减";
                }
            }
        }

    }
}
