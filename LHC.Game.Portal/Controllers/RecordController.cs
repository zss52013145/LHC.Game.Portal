using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHC.Game.Portal.Controllers
{
    public class RecordController : BaseController
    {

        //资金明细
        public ActionResult FundDetail()
        {
            string sql = "select * from  ProfitLoss order by Id desc";

            ViewBag.list = Util.ReaderToList<ProfitLoss>(sql);


            return View();
        }


        //今日结算
        public ActionResult TodaySettlement(int lType)
        {

            string date = DateTime.Now.ToString("yyyy-MM-dd");

            string time1 = date + " 0:0:0";
            string time2 = date + " 23:0:0";


            string sql = "select * from  BettingRecord  where WinState <> 1 and lType = " + lType + " and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "'";

            List<BettingRecord> list = Util.ReaderToList<BettingRecord>(sql);

            ViewBag.tuishui = list.Sum(p => p.TuiShui);

            ViewBag.sy = list.Sum(p => p.TuiShui + p.WinMoney - p.BetCount * p.UnitMoney);

            ViewBag.count = list.Count;

            ViewBag.betMoney = list.Sum(p => p.BetCount * p.UnitMoney);

            if (list.Count > 0)
            {
                ViewBag.issue = list[0].Issue;
            }




            return View();
        }


        //今日结算
        public ActionResult TodaySettlementDetail(int lType)
        {

            string date = DateTime.Now.ToString("yyyy-MM-dd");

            string time1 = date + " 0:0:0";
            string time2 = date + " 23:0:0";


            string sql = "select * from  BettingRecord  where WinState <> 1 and lType = " + lType + " and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "' order by Id desc";

            List<BettingRecord> list = Util.ReaderToList<BettingRecord>(sql);


            ViewBag.list = list;



            return View();
        }




        //历史报表
        public ActionResult HistoryReport()
        {
            //string week = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek); 

            var d = DateTime.Now;
            var week = (int)d.DayOfWeek;

            if (week == 0)
            {
                week = 7;
            }

            //List<string> list = new List<string>();

            //本周的第一天

            d = d.AddDays(1 - week);   //.ToString("yyyy-MM-dd")     

            string dateStr = "";

            string[] dayArr = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };

            string sql = "";
            string time1 = "";
            string time2 = "";

            List<BettingRecord> list = null;





            #region 上周


            DateTime d3 = d;
            List<DayReport> reportList3 = new List<DayReport>();


            for (int i = 7; i > 0; i--)
            {

                d3 = d.AddDays(-i);


                dateStr = d3.ToString("yyyy-MM-dd");

                time1 = dateStr + " 0:0:0";
                time2 = dateStr + " 23:59:59";

                DayReport dr = new DayReport();

                dr.Date = dateStr;

                dr.Week = dayArr[7 - i];

                sql = "select * from BettingRecord where WinState <> 1 and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "'";

                list = Util.ReaderToList<BettingRecord>(sql);

                dr.BetMoney = list.Sum(p => p.BetCount * p.UnitMoney);
                dr.TuiShui = list.Sum(p => p.TuiShui);
                dr.BetCount = list.Count;
                dr.SY = list.Sum(p => p.WinMoney + p.TuiShui - p.BetCount * p.UnitMoney);

                reportList3.Add(dr);

            }

            ViewBag.lastWeekList = reportList3;


            #endregion

            #region 本周


            List<DayReport> reportList = new List<DayReport>();

            DateTime d2 = d;

            for (int i = 0; i < 7; i++)
            {

                d2 = d.AddDays(i);


                dateStr = d2.ToString("yyyy-MM-dd");

                time1 = dateStr + " 0:0:0";
                time2 = dateStr + " 23:59:59";

                DayReport dr = new DayReport();

                dr.Date = dateStr;

                dr.Week = dayArr[i];

                sql = "select * from BettingRecord where WinState <> 1 and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "'";

                list = Util.ReaderToList<BettingRecord>(sql);

                dr.BetMoney = list.Sum(p => p.BetCount * p.UnitMoney);
                dr.TuiShui = list.Sum(p => p.TuiShui);
                dr.BetCount = list.Count;
                dr.SY = list.Sum(p => p.WinMoney + p.TuiShui - p.BetCount * p.UnitMoney);

                reportList.Add(dr);

            }

            ViewBag.thisWeekList = reportList;


            #endregion






            return View();
        }


        //历史报表-2
        public ActionResult Histrory2(string date)
        {
            ViewBag.date = date;


            string time1 = date + " 0:0:0";
            string time2 = date + " 23:59:59";


            try
            {
                DateTime d = DateTime.Parse(time1);
            }
            catch (Exception ex)
            {
                return Content("时间错误");
            }



            string sql = "select * from BettingRecord where lType = 1 and WinState <> 1 and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "'";
            var list = Util.ReaderToList<BettingRecord>(sql);

            ViewBag.count = list.Count;
            ViewBag.betMoney = list.Sum(p => p.BetCount * p.UnitMoney);
            ViewBag.TuiShui = list.Sum(p => p.TuiShui);
            ViewBag.SY = list.Sum(p => p.WinMoney + p.TuiShui - p.BetCount * p.UnitMoney);


            //---------------------------


            sql = "select * from BettingRecord where lType = 3 and WinState <> 1 and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "'";
            list = Util.ReaderToList<BettingRecord>(sql);

            ViewBag.count2 = list.Count;
            ViewBag.betMoney2 = list.Sum(p => p.BetCount * p.UnitMoney);
            ViewBag.TuiShui2 = list.Sum(p => p.TuiShui);
            ViewBag.SY2 = list.Sum(p => p.WinMoney + p.TuiShui - p.BetCount * p.UnitMoney);


            return View();

        }




        //历史报表-3
        public ActionResult Histrory3(string date, int lType)
        {

            ViewBag.lType = lType;


            string time1 = date + " 0:0:0";
            string time2 = date + " 23:59:59";


            try
            {
                DateTime d = DateTime.Parse(time1);
            }
            catch (Exception ex)
            {
                return Content("时间错误");
            }



            string sql = "select * from BettingRecord where lType = " + lType + " and WinState <> 1 and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "'";
            var list = Util.ReaderToList<BettingRecord>(sql);

            ViewBag.count = list.Count;
            ViewBag.betMoney = list.Sum(p => p.BetCount * p.UnitMoney);
            ViewBag.TuiShui = list.Sum(p => p.TuiShui);
            ViewBag.SY = list.Sum(p => p.WinMoney + p.TuiShui - p.BetCount * p.UnitMoney);

            ViewBag.lTypeName = Util.GetlTypeName(lType);



            if (list.Count > 0)
            {
                ViewBag.issue = list[0].Issue;
            }


            return View();

        }


        //历史报表-2
        public ActionResult Histrory4(string issue, int lType)
        {

            ViewBag.lType = lType;

            SqlParameter[] pms =
            {
                new SqlParameter("@issue",issue),
            };


            string sql = "select * from BettingRecord where lType = " + lType + " and  WinState <> 1 and issue=@issue order by Id desc";
            ViewBag.list = Util.ReaderToList<BettingRecord>(sql, pms);



            sql = "select * from LotteryRecord where lType = " + lType + " and issue=@issue";
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql, pms);

            ViewBag.lotteryRecord = lr;

            return View();

        }
    }
}
