using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    [Serializable]
    public class Admin
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string RealName { get; set; }
        public DateTime SubTime { get; set; }
        public int LoginCount { get; set; }
        public  bool IsDJ { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string LastLoginIp { get; set; }



        

        
        



        //---------------------

        public string ShowLastLoginTime
        {
            get
            {
                if (this.LastLoginTime != null)
                {
                    return ((DateTime) LastLoginTime).ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "";
            }
        }

        public string AuthNames {
            get
            {
                string sql = "select AuthNames from AdminGroup where Id =" + GroupId;


                object o = SqlHelper.ExecuteScalar(sql);

                if (o != DBNull.Value)
                {
                    return o.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public string Duty {
            get
            {
                if (this.GroupId != null && this.GroupId != 0)
                {
                    string sql = "select Duty from AdminGroup where Id =" + GroupId;
                    return SqlHelper.ExecuteScalar(sql).ToString();
                }
                else
                {
                    return "";
                }
            }
        }


    }
}
