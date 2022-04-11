using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Public
{
    public class Util5
    {


        private static readonly string thisYearSX = "虎";            //当年的生肖

        public static decimal GetPeilvFor28(int lType, string betNum)
        {

            if (betNum.StartsWith("点"))
            {
                betNum = betNum.Substring(1, betNum.Length - 1);
            }

            string sql = "select Value1 from BetLimit  where  [Key] = @Key and  lType =" + lType;

            SqlParameter[] pms = { new SqlParameter("@Key", betNum) };

            object obj = SqlHelper.ExecuteScalar(sql, pms);


            if (obj != null && obj != DBNull.Value)
            {
                return (decimal)obj;
            }

            return -1;

        }


        public static string GetPK10Issue(int lType)
        {


            string sql = "select top(1)Issue from LotteryRecord where lType = " + lType + " order by Issue desc";


            //DateTime d = DateTime.Now;
            //string date = d.ToString("yyyy-MM-dd");

            //int month = d.Month;
            //int day = d.Day;
            //int year = d.Year;




            long nextIssue = long.Parse(SqlHelper.ExecuteScalar(sql).ToString()) + 1;


            return nextIssue.ToString();
        }

        public static string GetCurrentIssue(int lType)
        {
            string sql = "select top(1)Issue from LotteryRecord where lType = " + lType + " order by Issue desc";

            long nextIssue = long.Parse(SqlHelper.ExecuteScalar(sql).ToString()) + 1;

            return nextIssue.ToString();

        }



        public static string GetRemainingTime(int lType)
        {

            DateTime d = DateTime.Now;

            DateTime t = DateTime.Parse(d.ToString("yyyy-MM-dd") + " 21:24:00");

            DateTime t2 = DateTime.Parse(d.ToString("yyyy-MM-dd") + " 17:00:00");

            if (d > t) return "已封盘";

            return GetTwoDateCha(d, t);

        }


        public static string GetOpenRemainingTime(int lType)
        {

            DateTime d = DateTime.Now;


            DateTime t = DateTime.Parse(d.ToString("yyyy-MM-dd") + " 21:34:00");

            if (d > t) return "正在开奖";

            return GetTwoDateCha(d, t);


        }





        public static string GetRemainingTimeUnit(int lType, DateTime d1, DateTime d2, int min)
        {


            DateTime d = DateTime.Now;
            DateTime target = d;

            if (d > d2 || d < d1)
            {
                return "已封盘";
            }
            else if (d > d1 && d < d1.AddMinutes(min))      //第一期
            {
                target = d1.AddMinutes(min);
            }
            else
            {
                target = GetLastOpenDate(lType).AddMinutes(min);

                //if (lType == 81 || lType == 83)
                //{
                //    string sql = "select EndTime from Issue where EndTime > '" + d.ToString() + "' and StartTime < '" + d.ToString() + "'";

                //    //DateTime temp = (DateTime)SqlHelper.ExecuteScalar(sql);

                //    DateTime temp = (DateTime)SqlHelper.ExecuteScalarForFenZhan(11, sql);

                //    target = temp.AddMinutes(-2);
                //}

                if (d > target)
                {
                    return "已封盘";
                }
            }

            return GetTwoDateCha(d, target);
        }


        public static string GetTwoDateCha(DateTime dTemp, DateTime target)
        {
            int seconds = (int)((target - dTemp).TotalSeconds);

            int showMinute = 0;
            int showSecond = 0;
            int showHour = 0;

            if (seconds < 60)
            {
                showSecond = seconds;
            }
            else
            {
                showMinute = seconds / 60;
                showSecond = seconds % 60;
            }



            if (showMinute < 60)
            {
                showHour = 0;
            }
            else
            {
                showHour = showMinute / 60;
                showMinute = showMinute % 60;
            }


            if (showSecond >= 10)
            {


                if (showMinute >= 10)
                {
                    if (showHour >= 10)
                    {
                        return showHour + "&" + showMinute + "&" + showSecond;
                    }
                    else
                    {
                        return "0" + showHour + "&" + showMinute + "&" + showSecond;
                    }

                }
                else
                {
                    if (showHour >= 10)
                    {
                        return showHour + "&0" + showMinute + "&" + showSecond;
                    }
                    else
                    {
                        return "0" + showHour + "&0" + showMinute + "&" + showSecond;
                    }
                }
            }
            else
            {
                if (showMinute >= 10)
                {
                    if (showHour >= 10)
                    {
                        return showHour + "&" + showMinute + "&0" + showSecond;
                    }
                    else
                    {
                        return "0" + showHour + "&" + showMinute + "&0" + showSecond;
                    }

                }
                else
                {
                    if (showHour >= 10)
                    {
                        return showHour + "&0" + showMinute + "&0" + showSecond;
                    }
                    else
                    {
                        return "0" + showHour + "&0" + showMinute + "&0" + showSecond;
                    }
                }
            }
        }

        public static DateTime GetLastOpenDate(int lType)
        {
            string sql = "select top(1)SubTime from LotteryRecord where lType=" + lType + " order by Issue desc";

            string time = "";

            while (string.IsNullOrEmpty(time))
            {
                time = SqlHelper.ExecuteScalar(sql).ToString();
            }

            return DateTime.Parse(time);
        }



        public static double GetPeilv(int lType, string bigPlayName, string playName, string betNum, string pankou)
        {

         


            if (bigPlayName == "半波")
            {
                playName = betNum;
            }

            if (bigPlayName.Contains("连"))
            {
                if (playName != "三全中" && playName != "二全中" && playName != "特串")
                {
                    return 1.134;
                }
            }



            #region 前三个


            if (playName.Contains("特码"))
            {

                if (!Util.IsNumberic(betNum))
                {
                    playName = betNum;
                }

            }
            else if (playName.Contains("正码"))
            {
                if (!Util.IsNumberic(betNum))
                {
                    playName = betNum;
                }

            }
            else if (playName.Contains("正") && playName.Contains("特"))
            {
                if (Util.IsNumberic(betNum))
                {
                    playName = "数字";
                }
                else
                {
                    playName = betNum;
                }

            }
            else if (betNum.Contains("总肖单") || betNum.Contains("总肖双"))
            {
                playName = betNum;
            }



            #endregion




            string sql = "";

            sql = "select peilv from  playinfo2  where  lType = @lType and PanKou=@PanKou and PlayBigType=@PlayBigType and PlaySmallType=@PlaySmallType";

            SqlParameter[] pms =
                                {
                                    new SqlParameter("@lType",lType),
                                    new SqlParameter("@PanKou",pankou),
                                    new SqlParameter("@PlayBigType",bigPlayName),
                                    new SqlParameter("@PlaySmallType",playName),
                      
                                };


            object o = SqlHelper.ExecuteScalar(sql, pms);


            #region 特殊情况

            string[] arr = null;



            if (playName == "特肖" || playName == "一肖中")
            {
                arr = o.ToString().Split('/');

                if (betNum == thisYearSX)
                {
                    return double.Parse(arr[0]);
                }
                else
                {
                    return double.Parse(arr[1]);
                }
            }
            else if (playName == "尾数中")
            {
                arr = o.ToString().Split('/');

                if (betNum == "0尾")
                {
                    return double.Parse(arr[1]);
                }
                else
                {
                    return double.Parse(arr[0]);
                }
            }
            else if (playName == "尾数不中")
            {
                arr = o.ToString().Split('/');

                if (betNum == "0尾")
                {
                    return double.Parse(arr[0]);
                }
                else
                {
                    return double.Parse(arr[1]);
                }
            }

            #endregion



            return Convert.ToDouble(o);

        }


    }
}
