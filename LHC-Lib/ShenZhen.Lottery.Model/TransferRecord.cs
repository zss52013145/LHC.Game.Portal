using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public class TransferRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Chu { get; set; }
        public string Jin { get; set; }
        public decimal Money { get; set; }
        public DateTime SubTime { get; set; }

        
        
        //扩展属性
        public string ShowTime
        {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ShowInfo
        {
            get { return "从" + Util.GetWalletName(Chu) + "转到" + Util.GetWalletName(Jin); }
        }

        public string ShowChu
        {
            get { return Util.GetWalletName(Chu); }
        }

        public string ShowJin
        {
            get { return Util.GetWalletName(Jin); }
        }

    }
}
