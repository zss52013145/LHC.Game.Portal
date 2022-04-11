using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public partial class PlaySamllType
    {
        public int Id { get; set; }
        public int lType { get; set; }
        public string PlayName { get; set; }
        public string BetNum { get; set; }
        public decimal Peilv { get; set; }
    }
}
