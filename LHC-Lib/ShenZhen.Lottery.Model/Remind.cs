using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class Remind
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AdminId { get; set; }
        public string Remark { get; set; }
        public DateTime SubTime { get; set; }
    }
}
