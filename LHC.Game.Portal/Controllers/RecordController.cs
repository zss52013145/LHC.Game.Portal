using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
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

            ViewBag.issue = list[0].Issue;



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
            return View();
        }
    }
}
