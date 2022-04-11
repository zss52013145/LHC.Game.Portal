using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 此类用作 用户分析
/// </summary>
namespace ShenZhen.Lottery.Model
{
    public class User
    {

        public int Id { get; set; }
        public decimal Money { get; set; }          //充值金额

        public UserInfo userInfo;


        //----------------

        public string UserName
        {

            get
            {

                userInfo = Util.GetEntityById<UserInfo>(this.Id);

                return userInfo.UserName;
            }
        }


        public string RealName
        {
            get
            {
                return userInfo.RealName;
            }
        }

        public string Mobile
        {
            get
            {
                return userInfo.Mobile;
            }
        }


        public int LoginCount
        {
            get
            {
                return userInfo.LoginCount;
            }
        }



        public object TotalWin
        {
            get
            {
                string sql = "select Sum(winmoney + tuishui - betcount*unitmoney) from BettingRecordMonth where UserId = " + this.Id;

                return SqlHelper.ExecuteScalarForFenZhan(77, sql);

            }
        }


        public string LastLoginTime
        {
            get
            {

                string result = "";

                if (userInfo.LastLoginTime == null) return result;

                DateTime d1 = (DateTime)userInfo.LastLoginTime;

                DateTime d2 = DateTime.Now;


                double days = (d2 - d1).TotalDays;

                if (days >= 1)
                {
                    return Convert.ToInt32(days) + "天前";
                }


                double hours = (d2 - d1).TotalHours;

                if (hours >= 1)
                {
                    return Convert.ToInt32(hours) + "小时前";
                }


                double mins = (d2 - d1).TotalMinutes;

                if (mins >= 1)
                {
                    return Convert.ToInt32(mins) + "分钟前";
                }



                return result;

            }
        }




    }
}
