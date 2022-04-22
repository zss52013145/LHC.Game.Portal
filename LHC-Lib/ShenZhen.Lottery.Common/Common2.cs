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
        public static double GetTuiShuiRate(int userId, int lType,string pankou, string playName)
        {

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
