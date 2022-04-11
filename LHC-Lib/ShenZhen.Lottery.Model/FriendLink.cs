using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class FriendLink
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string Param { get; set; }
        public DateTime SubTime { get; set; }
        public int RegisterCount { get; set; }

        //扩展字段
        public string ShowLink
        {
            get
            {
                return "http://" + Host + "/?id=" + Param;
            }
        }

        public string ShowTime
        {
            get { return this.SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

    }
}
