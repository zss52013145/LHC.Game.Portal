using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public class DayReport
    {
        public string Date { get; set; }
        public string Week { get; set; }
        public decimal BetMoney { get; set; }
        public decimal BetCount { get; set; }
        public decimal TuiShui { get; set; }
        public decimal SY { get; set; }



        public decimal ShowSY
        {

            get
            {
                return Util5.FormatDigit(SY);
            }
        }

        public decimal ShowTuiShui
        {

            get
            {
                return Util5.FormatDigit(TuiShui);
            }
        }


    

    }
}
