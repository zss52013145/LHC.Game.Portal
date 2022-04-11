using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class Activity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Rate { get; set; }

        public DateTime SubTime { get; set; }



        //
        public string ShowRate
        {
            get
            {
                return (Rate*100).ToString("0.0") + "%"; 
            }
        }

        public string ShowTime {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

    }
}
