using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class Setting
    {
        public string Value { get; set; }

        public double ShowValue
        {
            get { return Convert.ToDouble(Value)*100; }
        }
    }
}
