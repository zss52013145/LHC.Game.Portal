using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class UserReport
    {
        public string type { get; set; }                 //直属

        public string username { get; set; }                 //直属

        public int zhishu { get; set; }                 //直属

        public int tuandui { get; set; }                //团队

        public int chongzhi { get; set; }

        public int tixian { get; set; }

        public int betTotal { get; set; }

        public decimal rebateTotal { get; set; }

        public decimal winTotal { get; set; }

        public decimal tixianSX { get; set; }

        public decimal yingli { get; set; }



    }
}
