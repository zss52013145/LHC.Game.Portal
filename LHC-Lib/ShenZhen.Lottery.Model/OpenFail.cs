using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class OpenFail
    {
        public int Id { get; set; }

        public int lType { get; set; }

        public string Issue { get; set; }

        public DateTime SubTime { get; set; }

        public DateTime OpenTime { get; set; }
    }
}
