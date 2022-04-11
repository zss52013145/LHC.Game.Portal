using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    
    [Serializable]
    public class PossibleWin
    {
        public int lType { get; set; }
        public string PlayName { get; set; }
        public string BetNum { get; set; }
        public decimal WinMoney { get; set; }
        //public int Position { get; set; }
        //public string Num { get; set; }
    }
}
