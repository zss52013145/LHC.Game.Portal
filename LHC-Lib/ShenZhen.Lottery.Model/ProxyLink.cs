using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public class ProxyLink
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Code { get; set; }

        public string TeamName { get; set; }

        public int Type { get; set; }

        public double Rebates { get; set; }

        public DateTime SubTime { get; set; }

        //--------------------

        public string ShowType
        {
            get
            {
                if (Type == 1)
                {
                    return "代理";
                }
                else
                {
                    return "会员";
                }
            }
        }


        public string ShowTime
        {
            get
            {
                return this.SubTime.ToString("yyyy-MM-dd HH:mm");
            }

        }

        public int RegisterCount
        {
            get
            {
                string sql = "select count(1) from UserInfo where Link = '" + this.Code + "'";
                object obj = SqlHelper.ExecuteScalarForFenZhan(77, sql);

                if (obj.GetType() != typeof(int))
                {
                    return 0;
                }


                return Convert.ToInt32(obj);
            }

        }


        public double ShowRebates
        {
            get
            {
                return this.Rebates * 1000;
            }

        }

    }
}
