using ShenZhen.Lottery.Common;
using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using ShenZhen.Lottery.Public.Cache;
using ShenZhen.Lottery.Public.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace LHC.Game.Portal.Controllers
{
    public class LotteryController : BaseController
    {


        /// <summary>
        /// 赔率数据
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public ActionResult PeilvData(int lType, string playBigType)
        {
            string sql = "select  *  from  PlayInfo2 where lType = " + lType + " and PanKou=@PanKou and playBigType=@playBigType";

            SqlParameter[] pms =
            {
                new SqlParameter("@PanKou",LoginUser.PanKou),
                new SqlParameter("@playBigType",playBigType),
            };

            List<PlayInfo2> list = Util.ReaderToList<PlayInfo2>(sql, pms);

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 最新开奖数据
        /// </summary>
        /// <returns></returns>
        public ActionResult OpenData(int lType)
        {
            string sql = "select top(1)* from LotteryRecord where lType = " + lType + " order by Issue desc";

            List<LotteryRecord> list = Util.ReaderToList<LotteryRecord>(sql);

            return Json(list, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 开奖结果
        /// </summary>
        /// <returns></returns>
        public ActionResult OpenResult(int lType)
        {
            string sql = "select top(20)* from LotteryRecord where lType = " + lType + " order by Issue desc";

            List<LotteryRecord> list = Util.ReaderToList<LotteryRecord>(sql);

            ViewBag.list = list;

            //return Json(list, JsonRequestBehavior.AllowGet);

            ViewBag.lTypeName = Util.GetlTypeName(lType);

            return View();

        }


        /// <summary>
        /// 玩法规则
        /// </summary>
        /// <returns></returns>
        public ActionResult Rule(int lType)
        {
            ViewBag.lTypeName = Util.GetlTypeName(lType);
            return View();

        }

        /// <summary>
        /// 长龙
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public ActionResult ChangLongPage(int lType)
        {
            string sql = "select lType,Num from LotteryRecord where lType =" + lType + " order by Issue desc";
            List<LotteryRecordForChangLong> list = Util.ReaderToList<LotteryRecordForChangLong>(sql);

            ChangLong cl = Common.GetChangLong(list, lType);

            Dictionary<string, int> dic = new Dictionary<string, int>();


            Type t = cl.GetType();//获得该类的Type
            //再用Type.GetProperties获得PropertyInfo[],然后就可以用foreach 遍历了
            foreach (PropertyInfo pi in t.GetProperties())
            {
                int value = Convert.ToInt32(pi.GetValue(cl, null));//用pi.GetValue获得值
                string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作

                if (value >= 3)
                {
                    name = Util.GetChangLongName(name, lType);
                    dic.Add(name, value);
                }
            }


            ViewBag.data = dic.OrderByDescending(p => p.Value).ToDictionary(a => a.Key, p => p.Value);

            //ViewBag.data = dic.OrderByDescending(p => p.Value) as Dictionary<string, int>;

            return View();

        }


        /// <summary>
        /// 盘口信息-对应左侧【个人信息】
        /// </summary>
        /// <returns></returns>
        public ActionResult PanKouInfo()
        {
            string sql = "select * from PlayInfo  where  PanKou = '" + LoginUser.PanKou + "'";

            ViewBag.data = Util.ReaderToList<PlayInfo>(sql);

            return View();
        }


        public ActionResult GetPeilv(int lType, string bigType, string smallType)
        {
            if (smallType.Contains("正") && smallType.Contains("特") && smallType.Length == 3)
            {
                smallType = "数字";
            }


            string sql = "select peilv from  playinfo2 where  ltype=" + lType + " and playbigtype=@bigType and  playsmalltype = @smallType and pankou='" + LoginUser.PanKou + "'";

            SqlParameter[] pms =
            {
                new SqlParameter("@bigType",bigType),
                new SqlParameter("@smallType",smallType),
            };

            object o = SqlHelper.ExecuteScalar(sql, pms);

            return Content(o.ToString());

        }


        /// <summary>
        /// 最新注单
        /// </summary>
        /// <returns></returns>
        public ActionResult LastBetRecord(int lType)
        {

            string time = DateTime.Now.ToString("yyyy-MM-dd") + " 0:0:0";

            string sql = "select top(10)* from BettingRecord where lType = " + lType + " and  SubTime> '" + time + "' order by Id desc";

            List<BettingRecord> list = Util.ReaderToList<BettingRecord>(sql);

            ViewBag.list = list;


            ViewBag.lTypeName = Util.GetlTypeName(lType);

            return View();

        }



        //当前期号和时间
        public ActionResult GetCurrentIssueAndTime(int lType)
        {

            string time1 = Util5.GetRemainingTime(lType);

            if (time1 == "已封盘")
            {
                return Content("已封盘");

            }
            else
            {

                string issue = Util5.GetCurrentIssue(lType);

                string time2 = Util5.GetOpenRemainingTime(lType);

                string result = issue + "|" + time1 + "|" + time2;

                return Content(result);

            }

        }


        //未结明细1
        public ActionResult NotOpenRecord(int lType)
        {
            string sql = "select count(1) from BettingRecord where lType = 1 and WinState = 1";
            int count1 = Util.GetCountByDataBase(sql);

            sql = "select sum(betcount*unitmoney) from BettingRecord where lType = 1 and WinState = 1";
            int m1 = Util.GetCountByDataBase(sql);


            sql = "select count(1) from BettingRecord where lType = 3 and WinState = 1";
            int count2 = Util.GetCountByDataBase(sql);

            sql = "select sum(betcount*unitmoney) from BettingRecord where lType = 3 and WinState = 1";
            int m2 = Util.GetCountByDataBase(sql);


            ViewBag.count1 = count1;
            ViewBag.m1 = m1;
            ViewBag.count2 = count2;
            ViewBag.m2 = m2;

            return View();

        }




        public ActionResult NotOpenRecordDetail(int lType)
        {

            string sql = "select * from BettingRecord where lType = " + lType + " and WinState = 1";
            ViewBag.list = Util.ReaderToList<BettingRecord>(sql);


            sql = "select sum(betcount*unitmoney) from BettingRecord where lType = " + lType + " and WinState = 1";
            int m1 = Util.GetCountByDataBase(sql);


            ViewBag.m1 = m1;

            ViewBag.lTypeName = Util.GetlTypeName(lType);

            return View();

        }

    }
}
