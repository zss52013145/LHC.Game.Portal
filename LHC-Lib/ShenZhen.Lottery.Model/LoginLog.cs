using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class LoginLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime SubTime { get; set; }

        public string Ip { get; set; }


        public string ShowTime
        {
            get
            {
                return this.SubTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
