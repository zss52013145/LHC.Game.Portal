using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    internal class Partial
    {
    }



    public class LotteryRecordForChangLong : LotteryRecord
    {

        //-------------------------LHHC两面长龙相关

        public bool IsTMBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[6]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[6]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMHeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[6].Substring(0, 1));

                int b = int.Parse(arr[6].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMHeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[6].Substring(0, 1));

                int b = int.Parse(arr[6].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[6].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[6].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[6]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[6]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[6]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTMGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[6]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //-------------------------两面长龙相关


        public bool IsZ1TBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[0]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[0]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1THeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[0].Substring(0, 1));

                int b = int.Parse(arr[0].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1THeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[0].Substring(0, 1));

                int b = int.Parse(arr[0].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[0].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[0].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[0]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[0]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[0]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ1TGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[0]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //----------------Z2T----------------

        public bool IsZ2TBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[1]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[1]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2THeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[1].Substring(0, 1));

                int b = int.Parse(arr[1].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2THeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[1].Substring(0, 1));

                int b = int.Parse(arr[1].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[1].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[1].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[1]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[1]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[1]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ2TGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[1]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //----------------Z3T----------------

        public bool IsZ3TBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[2]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3TDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[2]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3THeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[2].Substring(0, 1));

                int b = int.Parse(arr[2].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3THeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[2].Substring(0, 1));

                int b = int.Parse(arr[2].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3TWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[2].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3TWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[2].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }




        public bool IsZ3TJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[2]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3TRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[2]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3TBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[2]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ3TGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[2]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //----------------Z4T----------------

        public bool IsZ4TBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[3]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[3]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4THeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[3].Substring(0, 1));

                int b = int.Parse(arr[3].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4THeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[3].Substring(0, 1));

                int b = int.Parse(arr[3].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[3].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[3].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[3]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[3]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[3]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ4TGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[3]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //----------------Z5T----------------

        public bool IsZ5TBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[4]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[4]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5THeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[4].Substring(0, 1));

                int b = int.Parse(arr[4].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5THeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[4].Substring(0, 1));

                int b = int.Parse(arr[4].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[4].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[4].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[4]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[4]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[4]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ5TGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[4]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //----------------Z6T----------------

        public bool IsZ6TBig
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[5]);

                if (a > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TDan
        {
            get
            {
                string[] arr = Num.Split(',');
                int a = int.Parse(arr[5]);

                if (a % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6THeBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[5].Substring(0, 1));

                int b = int.Parse(arr[5].Substring(1, 1));

                int sum = a + b;


                if (sum >= 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6THeDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int a = int.Parse(arr[5].Substring(0, 1));

                int b = int.Parse(arr[5].Substring(1, 1));

                int sum = a + b;


                if (sum % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TWeiBig
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[5].Substring(1, 1));

                if (wei >= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TWeiDan
        {
            get
            {
                string[] arr = Num.Split(',');

                int wei = int.Parse(arr[5].Substring(1, 1));

                if (wei % 2 != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TJQ
        {
            get
            {
                string[] arr = Num.Split(',');

                string sx = Util.GetShengxiaoByDigit(int.Parse(arr[5]));

                if (Util.IsJiaQin(sx))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TRed
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[5]);

                if (bose == "红波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TBlue
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[5]);

                if (bose == "蓝波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsZ6TGreen
        {
            get
            {
                string[] arr = Num.Split(',');

                string bose = Util.GetColor2(arr[5]);

                if (bose == "绿波")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }




    }
}
