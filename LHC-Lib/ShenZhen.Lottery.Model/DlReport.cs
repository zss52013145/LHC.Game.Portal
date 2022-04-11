using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    [Serializable]
    public class DlReport
    {

        public decimal BetYingKui { get; set; }

        public decimal TeamYingKui { get; set; }

        public int RechargeYingKui { get; set; }

        public int TotalRecharge { get; set; }

        public int TeamRecharge { get; set; }

        public int SDAdd { get; set; }

        public int SDJian { get; set; }

        public int TotalTiXian { get; set; }

        public int TeamTiXian { get; set; }

        public int TotalBet { get; set; }

        public decimal TotalFanShui { get; set; }

        public decimal TotalProfit { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ZhanGuProfit { get; set; }

        public decimal CompanyProfit { get; set; }
    }
}
