using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class Recharge
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public int Money { get; set; }

        public DateTime SubTime { get; set; }



        //-----------------------------------------------------



        public string ShowTime
        {
            get
            {
                return this.SubTime.ToString("MM-dd HH:mm");
            }
        }

    }
}
