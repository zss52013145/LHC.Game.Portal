using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class DateLine
    {
        public int Id { get; set; }

        public int lType { get; set; }

        public DateTime DateLine1 { get; set; }

        public DateTime OpenLine { get; set; }


        public string ShowDateLine1
        {
            get { return DateLine1.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ShowOpenLine
        {
            get { return OpenLine.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
    }
}
