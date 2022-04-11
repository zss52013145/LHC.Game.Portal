using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    /// <summary>
    /// 押分设置
    /// </summary>
    public class BetLimit
    {
        public int Id { get; set; }

        public int lType { get; set; }

        public string Key { get; set; }

        //public decimal Value1 { get; set; }     //PC28

        public int Value1 { get; set; }     //银河彩票

        public int Value { get; set; }

        //public int Value2 { get; set; }
    }
}
