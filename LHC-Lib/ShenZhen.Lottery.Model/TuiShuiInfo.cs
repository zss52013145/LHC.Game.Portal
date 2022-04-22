using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{

    /// <summary>
    /// 盘口信息
    /// </summary>
    public class TuiShuiInfo
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public int lType { get; set; }

        public string PlayName { get; set; }

        public int Max1 { get; set; }

        public int Max2 { get; set; }

        public decimal A { get; set; }
        public decimal B { get; set; }
        public decimal C { get; set; }
        public decimal D { get; set; }


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

      

        public decimal ShowA
        {

            get
            {

                return Util5.FormatDigit(A);
            }
        }


        public decimal ShowB
        {

            get
            {

                return Util5.FormatDigit(B);
            }
        }

        public decimal ShowC
        {

            get
            {

                return Util5.FormatDigit(C);
            }
        }

        public decimal ShowD
        {

            get
            {

                return Util5.FormatDigit(D);
            }
        }

    }
}
