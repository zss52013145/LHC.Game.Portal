using ShenZhen.Lottery.Common;
using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHC.Game.Portal.Controllers
{
    public class BettingController : BaseController
    {



        #region 普通下注【下注是触发器 扣钱不用写同步】---BetNormal

        public ActionResult BetNormal(int lType, string bigPlayName, string smallPlayName, string betInfo, int money, int betCount)
        {


            #region 基础校验

            if (LoginUser.Type == 1)
            {
                return Content("代理号不能下注");
            }


            //1.判断该游戏是否在维护
            string isOpenSql = "select Switch from LotteryType where lType = " + lType;
            string isOpnGame = SqlHelper.ExecuteScalar(isOpenSql).ToString();

            if (isOpnGame == "关")
            {
                return Content("游戏维护中");
            }



            ////1.判断是否封盘
            //string time = Util5.GetRemainingTime(lType);
            //if (time == "已封盘")
            //{
            //    return Content("已封盘");
            //}

            //2.判断期号是否正确
            string issue = Util5.GetCurrentIssue(lType); //  根据彩种类型查询当前下注期号，判断时候与传递过来的期号一致



            #endregion




            ////判断单子的正确性 - 针对投注号码
            //if (!Util.JudgeBetCorrect(lType, betInfo))
            //{
            //    return Content("数据错误");
            //}


            string[] infoArr = betInfo.Split('|');  //一条表示一个单子


            #region 4.判断余额是否足够下单

            decimal allMoney = 0;

            if (bigPlayName.Contains("连") || bigPlayName.Contains("合肖") || bigPlayName == "特平中" || bigPlayName == "中一" || bigPlayName == "自选不中")
            {
                allMoney = betCount * money;
            }
            else
            {
                //string smallType, string betNum, int betCount, int betMultiple,int totalMoney

                //4.判断余额是否足够下单
                foreach (string s in infoArr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        string[] arr = s.Split('-');

                        ////判断倍数和注数不能小于1
                        //if (int.Parse(arr[0]) < 1 || int.Parse(arr[4]) < 1)
                        //{
                        //    return Content("数据错误");
                        //}

                        allMoney += int.Parse(arr[2]);

                        if (allMoney < 0)
                        {
                            return Content("数据错误");
                        }

                    }
                }
            }




            #endregion



            //余额不足
            int userId = LoginUser.Id;
            //int userId = 6297;      //----------------------------------------

            decimal userMoney = Common.GetUserMoneyById(userId);


            if (userMoney < 0 || userMoney < allMoney)
            {
                return Content("余额不足");
            }


            ////判断有没有超出限额
            //if (lType % 2 != 0)
            //{
            //    foreach (string s in infoArr)
            //    {
            //        if (!string.IsNullOrEmpty(s))
            //        {
            //            string[] arr = s.Split('|');

            //            //参数
            //            betCount = int.Parse(arr[0]);
            //            string playName = arr[1];
            //            string betNum = arr[2].Trim();
            //            int unitMoney = int.Parse(arr[4]);
            //            decimal betMoney = betCount * unitMoney;

            //            if (Common.IsLimit(lType, betMoney, playName, betNum, issue, userId))
            //            {
            //                return Content("投注超过限额");
            //            }
            //        }
            //    }

            //}


            string sql = "";

            //条件正确开始下单
            foreach (string s in infoArr)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string[] arr = s.Split('-');

                    //参数


                    //string playName = arr[1];
                    string betNum = arr[0].Trim();


                    #region 尾数限制6注

                    //if (playName == "尾数" && lType == 4)
                    //{
                    //    //判断是否超过6注
                    //    string sql5 = "select count(*) from BettingRecord where Issue=@Issue and PlayName='尾数' and lType = 4 and UserId =" + LoginUser.Id;

                    //    int count5 = (int)SqlHelper.ExecuteScalar(sql5, new SqlParameter[]
                    //    {
                    //        new SqlParameter("@Issue",issue), 
                    //    });

                    //    if (count5 >= 6)
                    //    {
                    //        return Content("尾数玩法限投6注");
                    //    }
                    //}

                    #endregion


                    ////判断投注注数是否正确
                    //if (betCount != Util.CalcBetCount(lType, smallPlayName, betNum))
                    //{
                    //    return Content("数据错误");
                    //}


                    //赔率 从数据获取  --------------------未完待续
                    double peilv = Util5.GetPeilv(lType, bigPlayName, smallPlayName, betNum, LoginUser.PanKou);

                    int unitMoney = money;   // int.Parse(arr[2]);

                    //if (unitMoney < 5) {

                    //    return Content("单注额度不能低于5元");
                    //}



                    decimal userCurrMoney = Common.GetUserMoneyById(LoginUser.Id);
                    decimal betYue = userCurrMoney - (unitMoney * betCount);
                    sql = "insert into BettingRecord(lType,Issue,BetCount,PlayName,BetNum,UnitMoney,Peilv,UserId,BetYE) values (@lType,@Issue,@BetCount,@PlayName,@BetNum,@UnitMoney," + peilv + "," + LoginUser.Id + "," + betYue + ")";


                    SqlParameter[] pms =
                    {
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@Issue",issue),
                        new SqlParameter("@BetCount",betCount),
                        new SqlParameter("@PlayName",smallPlayName),
                        new SqlParameter("@BetNum",betNum),
                        new SqlParameter("@UnitMoney",unitMoney),
                    };

                    int betMoney = betCount * unitMoney;

                    sql += "update UserInfo set Money-=" + betMoney + " where Id=" + userId;

                    decimal currentMoney = Common.GetUserMoneyById(userId) - betMoney;
                    sql += Util.GetProfitLossSql(userId, 1, -betMoney, currentMoney);


                    SqlHelper.ExecuteTransaction(sql, pms);


                    //处理上级返点
                    //Common.HandTopRebate(LoginUser.Id, LoginUser.Rebates, unitMoney * betCount);

                }

            }

            return Content("ok");
        }

        #endregion

    }
}
