﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class AdminGroup
    {
        public int Id { get; set; }
        public string  Duty { get; set; }

        public string AuthIds { get; set; }
        public string AuthNames { get; set; }
    }
}
