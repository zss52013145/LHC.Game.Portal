using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    [Serializable]
    public  class Auth
    {
        public int id { get; set; }

        public int pId { get; set; }

        public string name { get; set; }

    }
}
