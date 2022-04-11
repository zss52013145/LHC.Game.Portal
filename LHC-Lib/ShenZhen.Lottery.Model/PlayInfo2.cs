using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    /// <summary>
    /// 赔率信息
    /// </summary>
    public class PlayInfo2
    {

        public int Id { get; set; }
        public int lType { get; set; }
        public string PanKou { get; set; }

        public string PlayBigType { get; set; }

        public string PlaySmallType { get; set; }

        public string PlaySmallType2 { get; set; }
      
        public string PeiLv { get; set; }


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
