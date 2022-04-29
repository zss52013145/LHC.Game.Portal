using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Common
{
    public static class Common2
    {

        //获取股东退水比例【顶层】
        public static double GetTuiShuiRate(int userId, int lType, string pankou, string playName, string betNum)
        {

            #region 玩法 处理

            if (playName.Contains("特码"))
            {

                if (betNum == "单" || betNum == "双")
                {
                    playName = "特码单双";
                }
                else if (betNum == "大" || betNum == "小")
                {
                    playName = "特码大小";
                }
                else if (betNum == "合单" || betNum == "合双")
                {
                    playName = "特码合数单双";
                }
                else if (betNum == "合大" || betNum == "合小")
                {
                    playName = "特码合数大小";
                }
                else if (betNum == "尾大" || betNum == "尾小")
                {
                    playName = "特码尾大尾小";
                }
                else if (betNum == "大单" || betNum == "小单")
                {
                    playName = "特码大单小单";
                }
                else if (betNum == "大双" || betNum == "小双")
                {
                    playName = "特码大双小双";
                }
                else if (betNum == "家禽" || betNum == "野禽")
                {
                    playName = "特码家禽野禽";
                }
                else if (betNum == "红波")
                {
                    playName = "特码色波-红波";
                }
                else if (betNum == "蓝波")
                {
                    playName = "特码色波-蓝波";
                }
                else if (betNum == "绿波")
                {
                    playName = "特码色波-绿波";
                }

            }
            else if (playName.Contains("正码"))
            {
                if (betNum == "单" || betNum == "双")
                {
                    playName = "总和单双";
                }
                else if (betNum == "大" || betNum == "小")
                {
                    playName = "总和大小";
                }

            }
            else if (playName.Contains("正") && playName.Contains("特"))
            {

                if (betNum == "单" || betNum == "双")
                {
                    playName = "正特单双";
                }
                else if (betNum == "大" || betNum == "小")
                {
                    playName = "正特大小";
                }
                else if (betNum == "合单" || betNum == "合双")
                {
                    playName = "正特合数单双";
                }
                else if (betNum == "合大" || betNum == "合小")
                {
                    playName = "正特合数大小";
                }
                else if (betNum == "尾大" || betNum == "尾小")
                {
                    playName = "正特尾大尾小";
                }
                else if (betNum == "大单" || betNum == "小单")
                {
                    playName = "正特大单小单";
                }
                else if (betNum == "大双" || betNum == "小双")
                {
                    playName = "正特大双小双";
                }
                else if (betNum == "家禽" || betNum == "野禽")
                {
                    playName = "正特家禽野禽";
                }
                else if (betNum == "红波")
                {
                    playName = "正特色波-红波";
                }
                else if (betNum == "蓝波")
                {
                    playName = "正特色波-蓝波";
                }
                else if (betNum == "绿波")
                {
                    playName = "正特色波-绿波";
                }
                else
                {
                    playName = "正特";
                }

            }




            #endregion

            string sql2 = "select * from TuiShuiInfo where UserId = 1 and ltype =" + lType + " and playname='" + playName + "'";

            TuiShuiInfo tsi = Util.ReaderToModel<TuiShuiInfo>(sql2);

            object obj = tsi.GetType().GetProperty(pankou).GetValue(tsi, null);

            return Convert.ToDouble(obj);

        }




        public static double GetAllKouchuRate(int pid)
        {
            double kouchu = 0;

            UserInfo user = Util.GetEntityById<UserInfo>(pid);

            kouchu += user.TuiShuiRate;

            while (user.Type != 2)
            {
                user = Util.GetEntityById<UserInfo>(user.PId);
                kouchu += user.TuiShuiRate;
            }

            return kouchu;

        }
    }
}
