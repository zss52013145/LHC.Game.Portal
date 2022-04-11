using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{

    /// <summary>
    /// 盘口信息
    /// </summary>
    public class PlayInfo
    {

        public int Id { get; set; }
        public int lType { get; set; }
        public string PanKou { get; set; }

        public string PlayName { get; set; }
        public int MaxForOne { get; set; }
        public int MaxForAll { get; set; }
        public decimal TuiShui { get; set; }


        //---------------------


        public string ShowlTypeName
        {

            get
            {
                if (lType == 1)
                {
                    return "香港六合彩";
                }
                else if (lType == 3)
                {
                    return "澳门六合彩";
                }

                return "";
            }
        }

    }
}
