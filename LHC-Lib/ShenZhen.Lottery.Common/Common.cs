using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using ShenZhen.Lottery.Public.Cache;
using ShenZhen.Lottery.Public.Cache.Redis;

namespace ShenZhen.Lottery.Common
{
    public static class Common
    {
        public static readonly ICacheManager CacheManager = new RedisCache();

        public static UserReport GetUserReportById(int userId, bool? isSelf, string time1, string time2)
        {
            UserInfo user = Util.GetEntityById<UserInfo>(userId);
            UserReport report = new UserReport();

            report.username = user.UserName;
            report.type = user.ShowType;

            string sql = "";

            if (user.Type == 2 || (isSelf != null && (bool)isSelf))         //代理自身
            {
                #region 自身

                sql = "select count(1) from UserInfo where PId = " + userId;
                report.zhishu = (int)SqlHelper.ExecuteScalarForFenZhan(77, sql);

                //
                report.tuandui = Common.GetAllNextIds2(userId).Split(',').Length - 1;

                //
                sql = "select sum(Money) from ComeOutRecord where UserId = " + userId + " and Type = 1 and State = 3 and SubTime > @Time1 and SubTime < @Time2";

                SqlParameter[] pms = {
                                     new SqlParameter("@Time1",time1),
                                     new SqlParameter("@Time2",time2),
                                     };

                object obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(int))
                {
                    report.chongzhi = 0;
                }
                else
                {
                    report.chongzhi = (int)obj;
                }




                //
                sql = "select sum(Money) from ComeOutRecord where UserId = " + userId + " and Type = 2 and State = 3 and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(int))
                {
                    report.tixian = 0;
                }
                else
                {
                    report.tixian = (int)obj;
                }

                //

                sql = "select sum(betCount * unitMoney) from BettingRecordMonth where UserId = " + userId + " and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(int))
                {
                    report.betTotal = 0;
                }
                else
                {
                    report.betTotal = (int)obj;
                }

                //
                sql = "select sum(Money) from ProfitLoss where Type = 11 and UserId = " + userId + " and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(decimal))
                {
                    report.rebateTotal = 0;
                }
                else
                {
                    report.rebateTotal = (decimal)obj;
                }

                //

                sql = "select sum(WinMoney + Tuishui) from BettingRecordMonth where UserId = " + userId + " and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(decimal))
                {
                    report.winTotal = 0;
                }
                else
                {
                    report.winTotal = (decimal)obj;
                }


                //
                report.tixianSX = 0;

                //
                report.yingli = report.winTotal + report.rebateTotal - report.betTotal;

                #endregion

            }
            else
            {

                #region 团队

                sql = "select count(1) from UserInfo where PId = " + userId;
                report.zhishu = (int)SqlHelper.ExecuteScalarForFenZhan(77, sql);

                //
                string ids = Common.GetAllNextIdsContainsMe(userId);
                report.tuandui = ids.Split(',').Length - 1;

                //
                sql = "select sum(Money) from ComeOutRecord where UserId in " + ids + " and Type = 1 and State = 3 and SubTime > @Time1 and SubTime < @Time2";

                SqlParameter[] pms = {
                                     new SqlParameter("@Time1",time1),
                                     new SqlParameter("@Time2",time2),
                                     };

                object obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(int))
                {
                    report.chongzhi = 0;
                }
                else
                {
                    report.chongzhi = (int)obj;
                }




                //
                sql = "select sum(Money) from ComeOutRecord where UserId in " + ids + " and Type = 2 and State = 3 and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(int))
                {
                    report.tixian = 0;
                }
                else
                {
                    report.tixian = (int)obj;
                }

                //

                sql = "select sum(betCount * unitMoney) from BettingRecordMonth where UserId in " + ids + " and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(int))
                {
                    report.betTotal = 0;
                }
                else
                {
                    report.betTotal = (int)obj;
                }

                //
                sql = "select sum(Money) from ProfitLoss where Type = 11 and UserId in " + ids + " and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(decimal))
                {
                    report.rebateTotal = 0;
                }
                else
                {
                    report.rebateTotal = (decimal)obj;
                }

                //

                sql = "select sum(WinMoney + Tuishui) from BettingRecordMonth where UserId in " + ids + " and SubTime > @Time1 and SubTime < @Time2";


                obj = SqlHelper.ExecuteScalarForFenZhan(77, sql, pms);

                if (obj.GetType() != typeof(decimal))
                {
                    report.winTotal = 0;
                }
                else
                {
                    report.winTotal = (decimal)obj;
                }


                //
                report.tixianSX = 0;

                //
                report.yingli = report.winTotal + report.rebateTotal - report.betTotal;

                #endregion


            }



            return report;
        }


        //二次处理
        public static string GetAllNextIdsNotContainsMe(int userId)
        {
            string ids = GetAllNextIds2(userId).TrimEnd(',');

            List<string> list = ids.Split(',').ToList();
            list.RemoveAt(0);

            ids = string.Join(",", list);

            return "(" + ids + ")";

        }


        public static string GetAllNextIdsContainsMe(int userId)
        {
            string ids = GetAllNextIds2(userId).TrimEnd(',');

            return "(" + ids + ")";

        }


        public static string GetAllNextIds2(int userId)
        {
            string ids = "";

            List<UserInfo> list = Util.ReaderToList<UserInfo>("select * from UserInfo where PId = " + userId);

            ids += userId + ",";

            if (list.Count >= 1)
            {
                foreach (UserInfo user in list)
                {
                    ids += GetAllNextIds2(user.Id);
                }
            }

            return ids;
        }


        public static string GetZhiShuNextIds(int userId, int? userType)
        {
            string ids = "";

            string sql = "select Id from UserInfo where PId = " + userId;

            if (userType != null && userType != 0)
            {
                sql = "select Id from UserInfo where Type = " + userType + " and PId = " + userId;
            }

            List<UserInfo> list = Util.ReaderToList<UserInfo>(sql);

            ids += userId + ",";

            foreach (UserInfo user in list)
            {
                ids += user.Id + ",";
            }

            return ids.TrimEnd(',');
        }


        public static string GetZhiShuNextIdsNotContainsMe(int userId, int? userType)
        {
            string ids = "";

            string sql = "select Id from UserInfo where PId = " + userId;

            if (userType != null && userType != 0)
            {
                sql = "select Id from UserInfo where Type = " + userType + " and PId = " + userId;
            }

            List<UserInfo> list = Util.ReaderToList<UserInfo>(sql);

            //ids += userId + ",";

            foreach (UserInfo user in list)
            {
                ids += user.Id + ",";
            }

            return ids.TrimEnd(',');
        }


        public static void HandTopRebate(int userId, double rebate, int betMoney)
        {
            UserInfo user = Util.GetEntityById<UserInfo>(userId);
            string sql = "";

            if (user.PId != 0)
            {
                UserInfo user2 = Util.GetEntityById<UserInfo>(user.PId);

                double money = (user2.Rebates - user.Rebates) * betMoney / 2;

                //改变余额
                sql = "update UserInfo set Money = Money + " + money + " where Id=" + user.PId;

                //盈亏记录
                int type = 11;
                double currentMoney = money + (double)user2.Money;
                string subTime = DateTime.Now.ToString();

                sql += "insert into ProfitLoss(UserId, Money, CurrentMoney, Type, SubTime) values(" + user.PId + ", " + money + ", " + currentMoney + ", " + type + ", '" + subTime + "')";

                SqlHelper.ExecuteTransaction(sql);


                if (user2.PId != 0)
                {
                    HandTopRebate(user2.Id, user2.Rebates, betMoney);
                }


            }

        }

        public static List<string> GetAllTopNameList(int userId, int PId)
        {
            string ids = GetAllTopIds(userId);

            string[] idArr = ids.Trim().Split(' ');

            List<string> list = new List<string>();

            foreach (string s in idArr)
            {
                if (s == PId + "")
                {
                    break;
                }

                list.Add(Util.GetUserNameById(s));
            }

            list.Reverse();

            return list;
        }


        public static string GetAllTopIds(int userId)
        {
            string ids = "";

            UserInfo user = Util.GetEntityById<UserInfo>(userId);

            ids += userId + " ";

            if (user.PId != 0)
            {
                ids += GetAllTopIds(user.PId) + " ";
            }

            return ids;

        }


        #region 判断第三方充值限额


        //判断第三方充值限额
        public static bool JudgeThirdLimitMoney(string rechargeLine, int rechargeType, int money)
        {
            bool result = false;

            string sql = "select * from RechargeThird where RechargeLine=@RechargeLine and RechargeType=" + rechargeType;

            SqlParameter[] pms =
            {
                new SqlParameter("@RechargeLine",rechargeLine),
            };

            RechargeThird rechargeThird = Util.ReaderToModel<RechargeThird>(sql, pms);

            if (rechargeThird == null || (money < rechargeThird.Money1 || money > rechargeThird.Money2))
            {
                result = true;
            }

            return result;
        }


        #endregion



        #region 判断投注是否超出限额

        public static bool IsLimit(int lType, decimal betMoney, string playName, string betNum, string issue, int userId)
        {
            //特殊情况 新增加的官方彩种
            #region 特殊情况

            if (lType == 25 || lType == 27 || lType == 61 || lType == 81 || lType == 85)
            {
                lType = 1;
            }
            else if (lType == 19)
            {
                lType = 11;
            }
            else if (lType == 29 || lType == 31 || lType == 33)
            {
                lType = 13;
            }
            else if (lType == 35 || lType == 37 || lType == 39 || lType == 41 || lType == 43 || lType == 45 || lType == 47)
            {
                lType = 15;
            }
            else if (lType == 49 || lType == 51 || lType == 53 || lType == 55 || lType == 57 || lType == 59)
            {
                lType = 21;
            }
            else if (lType == 63)
            {
                lType = 23;
            }
            else if (lType == 83)
            {
                lType = 9;
            }

            #endregion



            bool result = false;

            string sql = "select Value1,Value from BetLimit where lType = " + lType;

            if (lType == 1 || lType == 11 || lType == 15 || lType == 81 || lType == 85)
            {
                #region 重庆时时彩 3D 11选5

                if (playName == "第一球" || playName == "第二球" || playName == "第三球" || playName == "第四球" || playName == "第五球" || playName == "豹顺对")
                {
                    sql += " and [Key]= '" + playName + "'";
                }
                else if (playName == "前三" || playName == "中三" || playName == "后三")
                {
                    sql += " and [Key]= '前中后'";
                }
                else if (betNum.IndexOf("总和") != -1)
                {
                    sql += " and [Key]= '总和'";
                }
                else if (betNum == "龙" || betNum == "虎" || betNum == "和")
                {
                    sql += " and [Key]= '龙虎和'";
                }

                #endregion
            }
            else if (lType == 3)
            {
                #region 六合彩

                if (playName.IndexOf("特码") != -1)
                {
                    sql += " and [Key]= '特码'";
                }
                else if (playName.IndexOf("正A") != -1 || playName.IndexOf("正B") != -1)
                {
                    sql += " and [Key]= '正码'";
                }
                else if (playName.IndexOf("正码") != -1)
                {
                    sql += " and [Key]= '正码1-6'";
                }
                else if (playName == "正1特" || playName == "正2特" || playName == "正3特" || playName == "正4特" || playName == "正5特" || playName == "正6特")
                {
                    sql += " and [Key]= '正特'";
                }
                else if (playName == "三全中" || playName == "三中二" || playName == "二全中" || playName == "二中特" ||
                         playName == "特串")
                {
                    sql += " and [Key]= '连码'";
                }
                else if (playName == "半波" || playName == "尾数" || playName == "一肖" || playName == "特肖")
                {
                    sql += " and [Key]= '" + playName + "'";
                }
                else if (playName == "五不中" || playName == "六不中" || playName == "七不中" || playName == "八不中" ||
                         playName == "九不中" || playName == "十不中")
                {
                    sql += " and [Key]= '不中'";
                }
                else if (playName == "六肖连中" || playName == "六肖连不中")
                {
                    sql += " and [Key]= '六肖'";
                }
                else if (playName.IndexOf("连肖") != -1)
                {
                    sql += " and [Key]= '连肖'";
                }
                else if (playName.IndexOf("尾连") != -1)
                {
                    sql += " and [Key]= '连尾'";
                }
                else if (playName.IndexOf("-") != -1)
                {
                    sql += " and [Key]= '1-6龙虎'";
                }

                #endregion
            }
            else if (lType == 5)
            {
                #region 七星彩

                if (playName == "定千位" || playName == "定百位" || playName == "定十位" || playName == "定个位")
                {
                    sql += " and [Key]= '一定位'";
                }
                else if (playName == "千##个" || playName == "#百十#" || playName == "千#十#" || playName == "#百#个" || playName == "千百##" || playName == "##十个")
                {
                    sql += " and [Key]= '二定位'";
                }
                else if (playName == "千百十#" || playName == "千百#个" || playName == "#百十个" || playName == "千#十个")
                {
                    sql += " and [Key]= '三定位'";
                }
                else if (playName == "四定位")
                {
                    sql += " and [Key]= '四定位'";
                }

                #endregion
            }
            else if (lType == 7 || lType == 9 || lType == 83)
            {
                #region PK10 幸运飞艇

                if (playName == "冠军" || playName == "亚军" || playName == "第三名" || playName == "第四名" || playName == "第五名" ||
                    playName == "第六名" || playName == "第七名" || playName == "第八名" || playName == "第九名" ||
                    playName == "第十名")
                {
                    sql += " and [Key]= '" + playName + "'";
                }
                else
                {
                    sql += " and [Key]= '冠亚和'";
                }

                #endregion
            }
            else if (lType == 13 || lType == 17)
            {
                #region 广东快乐十分 幸运农场

                if (playName == "第一球" || playName == "第二球" || playName == "第三球" || playName == "第四球" || playName == "第五球" ||
                    playName == "第六球" || playName == "第七球" || playName == "第八球")
                {
                    sql += " and [Key]= '" + playName + "'";
                }
                else if (playName.IndexOf("总和") != -1)
                {
                    sql += " and [Key]= '总和'";
                }
                else if (playName.IndexOf("总和") != -1)
                {
                    sql += " and [Key]= '连码'";
                }


                #endregion
            }
            else if (lType == 21)
            {
                #region 江苏快三

                if (string.IsNullOrEmpty(playName))         //特殊情况  18-5-18
                {
                    playName = "豹子";
                }

                sql += " and [Key]= '" + playName + "'";

                #endregion
            }
            else if (lType == 23)
            {
                #region 加拿大28

                if (playName == "特码")
                {
                    sql += " and [Key]= '" + playName + "'";
                }
                else
                {
                    sql += " and [Key]= '混合'";
                }

                #endregion
            }


            BetLimit betLimit = Util.ReaderToModel<BetLimit>(sql);

            //判断
            //int limitMoney = (int)SqlHelper.ExecuteScalar(sql);


            //单注
            if (betLimit.Value1 > 0)
            {
                if (betMoney > betLimit.Value1)
                {
                    result = true;
                }
            }


            //单期
            if (betLimit.Value > 0 && !result)
            {
                //10-29--加上已经投注的金额
                string seaSql = "select sum(BetCount * UnitMoney) from BettingRecordWeek where PlayName=@PlayName and Issue=@Issue and UserId=" + userId;
                SqlParameter[] parameter =
                {
                    new SqlParameter("@PlayName", playName),
                    new SqlParameter("@Issue", issue),
                };

                decimal alreadyBetMoney = 0;
                //object alreadyBetObj = SqlHelper.ExecuteScalarForFenZhan(77, seaSql, parameter);
                object alreadyBetObj = SqlHelper.ExecuteScalar(seaSql, parameter);


                if (alreadyBetObj != DBNull.Value)
                {
                    alreadyBetMoney = Convert.ToDecimal(alreadyBetObj);
                }

                //End10-29


                if ((betMoney + alreadyBetMoney) > betLimit.Value)
                {
                    result = true;
                }

            }


            return result;
        }

        #endregion




        #region 获取用户缓存余额

        public static decimal GetUserMoneyById(int id)
        {
            string sql = "select Money from UserInfo where Id = " + id;
            return decimal.Parse(SqlHelper.ExecuteScalar(sql).ToString());
        }


        public static decimal GetUserMoneyByIdForFenZhan(int fenzhan, int id)
        {
            string sql = "select Money from UserInfo where Id = " + id;
            return decimal.Parse(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql).ToString());
        }

        #endregion

        #region 更新用户缓存余额

        public static void UpdateCacheMoney(int uid, decimal money)
        {
            CacheHelper.SetCache(uid.ToString(), money, DateTime.Now.AddDays(10));
        }

        #endregion

        //两面长龙
        public static ChangLong GetChangLong(List<LotteryRecordForChangLong> list, int lType)
        {
            ChangLong cl = new ChangLong();

            int count = 0;

            foreach (LotteryRecordForChangLong record in list)
            {

                if (count < 2)
                {
                    //前两期直接递增

                    #region 特码

                    if (record.IsTMBig)
                    {
                        cl.TMBig++;
                    }
                    else
                    {
                        cl.TMSmall++;
                    }

                    if (record.IsTMDan)
                    {
                        cl.TMDan++;
                    }
                    else
                    {
                        cl.TMShuang++;
                    }

                    if (record.IsTMHeBig)
                    {
                        cl.TMHeBig++;
                    }
                    else
                    {
                        cl.TMHeSmall++;
                    }

                    if (record.IsTMHeDan)
                    {
                        cl.TMHeDan++;
                    }
                    else
                    {
                        cl.TMHeShuang++;
                    }


                    if (record.IsTMWeiBig)
                    {
                        cl.TMWeiBig++;
                    }
                    else
                    {
                        cl.TMWeiSmall++;
                    }



                    if (record.IsTMJQ)
                    {
                        cl.TMJQ++;
                    }
                    else
                    {
                        cl.TMYS++;
                    }

                    if (record.IsTMRed)
                    {
                        cl.TMRed++;
                    }

                    if (record.IsTMBlue)
                    {
                        cl.TMBlue++;
                    }

                    if (record.IsTMGreen)
                    {
                        cl.TMGreen++;
                    }

                    #endregion

                    #region Z1T

                    if (record.IsZ1TBig)
                    {
                        cl.Z1TBig++;
                    }
                    else
                    {
                        cl.Z1TSmall++;
                    }

                    if (record.IsZ1TDan)
                    {
                        cl.Z1TDan++;
                    }
                    else
                    {
                        cl.Z1TShuang++;
                    }

                    if (record.IsZ1THeBig)
                    {
                        cl.Z1THeBig++;
                    }
                    else
                    {
                        cl.Z1THeSmall++;
                    }

                    if (record.IsZ1THeDan)
                    {
                        cl.Z1THeDan++;
                    }
                    else
                    {
                        cl.Z1THeShuang++;
                    }


                    if (record.IsZ1TWeiBig)
                    {
                        cl.Z1TWeiBig++;
                    }
                    else
                    {
                        cl.Z1TWeiSmall++;
                    }


                    if (record.IsZ1TJQ)
                    {
                        cl.Z1TJQ++;
                    }
                    else
                    {
                        cl.Z1TYS++;
                    }

                    if (record.IsZ1TRed)
                    {
                        cl.Z1TRed++;
                    }

                    if (record.IsZ1TBlue)
                    {
                        cl.Z1TBlue++;
                    }

                    if (record.IsZ1TGreen)
                    {
                        cl.Z1TGreen++;
                    }

                    #endregion

                    #region Z2T

                    if (record.IsZ2TBig)
                    {
                        cl.Z2TBig++;
                    }
                    else
                    {
                        cl.Z2TSmall++;
                    }

                    if (record.IsZ2TDan)
                    {
                        cl.Z2TDan++;
                    }
                    else
                    {
                        cl.Z2TShuang++;
                    }

                    if (record.IsZ2THeBig)
                    {
                        cl.Z2THeBig++;
                    }
                    else
                    {
                        cl.Z2THeSmall++;
                    }

                    if (record.IsZ2THeDan)
                    {
                        cl.Z2THeDan++;
                    }
                    else
                    {
                        cl.Z2THeShuang++;
                    }


                    if (record.IsZ2TWeiBig)
                    {
                        cl.Z2TWeiBig++;
                    }
                    else
                    {
                        cl.Z2TWeiSmall++;
                    }


                    if (record.IsZ2TJQ)
                    {
                        cl.Z2TJQ++;
                    }
                    else
                    {
                        cl.Z2TYS++;
                    }

                    if (record.IsZ2TRed)
                    {
                        cl.Z2TRed++;
                    }

                    if (record.IsZ2TBlue)
                    {
                        cl.Z2TBlue++;
                    }

                    if (record.IsZ2TGreen)
                    {
                        cl.Z2TGreen++;
                    }

                    #endregion

                    #region Z3T

                    if (record.IsZ3TBig)
                    {
                        cl.Z3TBig++;
                    }
                    else
                    {
                        cl.Z3TSmall++;
                    }

                    if (record.IsZ3TDan)
                    {
                        cl.Z3TDan++;
                    }
                    else
                    {
                        cl.Z3TShuang++;
                    }

                    if (record.IsZ3THeBig)
                    {
                        cl.Z3THeBig++;
                    }
                    else
                    {
                        cl.Z3THeSmall++;
                    }

                    if (record.IsZ3THeDan)
                    {
                        cl.Z3THeDan++;
                    }
                    else
                    {
                        cl.Z3THeShuang++;
                    }


                    if (record.IsZ3TWeiBig)
                    {
                        cl.Z3TWeiBig++;
                    }
                    else
                    {
                        cl.Z3TWeiSmall++;
                    }


                    if (record.IsZ3TJQ)
                    {
                        cl.Z3TJQ++;
                    }
                    else
                    {
                        cl.Z3TYS++;
                    }

                    if (record.IsZ3TRed)
                    {
                        cl.Z3TRed++;
                    }

                    if (record.IsZ3TBlue)
                    {
                        cl.Z3TBlue++;
                    }

                    if (record.IsZ3TGreen)
                    {
                        cl.Z3TGreen++;
                    }

                    #endregion

                    #region Z4T

                    if (record.IsZ4TBig)
                    {
                        cl.Z4TBig++;
                    }
                    else
                    {
                        cl.Z4TSmall++;
                    }

                    if (record.IsZ4TDan)
                    {
                        cl.Z4TDan++;
                    }
                    else
                    {
                        cl.Z4TShuang++;
                    }

                    if (record.IsZ4THeBig)
                    {
                        cl.Z4THeBig++;
                    }
                    else
                    {
                        cl.Z4THeSmall++;
                    }

                    if (record.IsZ4THeDan)
                    {
                        cl.Z4THeDan++;
                    }
                    else
                    {
                        cl.Z4THeShuang++;
                    }


                    if (record.IsZ4TWeiBig)
                    {
                        cl.Z4TWeiBig++;
                    }
                    else
                    {
                        cl.Z4TWeiSmall++;
                    }


                    if (record.IsZ4TJQ)
                    {
                        cl.Z4TJQ++;
                    }
                    else
                    {
                        cl.Z4TYS++;
                    }

                    if (record.IsZ4TRed)
                    {
                        cl.Z4TRed++;
                    }

                    if (record.IsZ4TBlue)
                    {
                        cl.Z4TBlue++;
                    }

                    if (record.IsZ4TGreen)
                    {
                        cl.Z4TGreen++;
                    }

                    #endregion

                    #region Z5T

                    if (record.IsZ5TBig)
                    {
                        cl.Z5TBig++;
                    }
                    else
                    {
                        cl.Z5TSmall++;
                    }

                    if (record.IsZ5TDan)
                    {
                        cl.Z5TDan++;
                    }
                    else
                    {
                        cl.Z5TShuang++;
                    }

                    if (record.IsZ5THeBig)
                    {
                        cl.Z5THeBig++;
                    }
                    else
                    {
                        cl.Z5THeSmall++;
                    }

                    if (record.IsZ5THeDan)
                    {
                        cl.Z5THeDan++;
                    }
                    else
                    {
                        cl.Z5THeShuang++;
                    }


                    if (record.IsZ5TWeiBig)
                    {
                        cl.Z5TWeiBig++;
                    }
                    else
                    {
                        cl.Z5TWeiSmall++;
                    }


                    if (record.IsZ5TJQ)
                    {
                        cl.Z5TJQ++;
                    }
                    else
                    {
                        cl.Z5TYS++;
                    }

                    if (record.IsZ5TRed)
                    {
                        cl.Z5TRed++;
                    }

                    if (record.IsZ5TBlue)
                    {
                        cl.Z5TBlue++;
                    }

                    if (record.IsZ5TGreen)
                    {
                        cl.Z5TGreen++;
                    }

                    #endregion


                    #region Z6T

                    if (record.IsZ6TBig)
                    {
                        cl.Z6TBig++;
                    }
                    else
                    {
                        cl.Z6TSmall++;
                    }

                    if (record.IsZ6TDan)
                    {
                        cl.Z6TDan++;
                    }
                    else
                    {
                        cl.Z6TShuang++;
                    }

                    if (record.IsZ6THeBig)
                    {
                        cl.Z6THeBig++;
                    }
                    else
                    {
                        cl.Z6THeSmall++;
                    }

                    if (record.IsZ6THeDan)
                    {
                        cl.Z6THeDan++;
                    }
                    else
                    {
                        cl.Z6THeShuang++;
                    }


                    if (record.IsZ6TWeiBig)
                    {
                        cl.Z6TWeiBig++;
                    }
                    else
                    {
                        cl.Z6TWeiSmall++;
                    }


                    if (record.IsZ6TJQ)
                    {
                        cl.Z6TJQ++;
                    }
                    else
                    {
                        cl.Z6TYS++;
                    }

                    if (record.IsZ6TRed)
                    {
                        cl.Z6TRed++;
                    }

                    if (record.IsZ6TBlue)
                    {
                        cl.Z6TBlue++;
                    }

                    if (record.IsZ6TGreen)
                    {
                        cl.Z6TGreen++;
                    }

                    #endregion

                }
                else
                {
                    //第三期及以上 必须满足 前两期都满足条件  才能继续递增

                    #region 特码

                    if (record.IsTMBig && cl.TMBig == count)
                    {
                        cl.TMBig++;
                    }

                    if (!record.IsTMBig && cl.TMSmall == count)
                    {
                        cl.TMSmall++;
                    }



                    if (record.IsTMDan && cl.TMDan == count)
                    {
                        cl.TMDan++;
                    }

                    if (!record.IsTMDan && cl.TMShuang == count)
                    {
                        cl.TMShuang++;
                    }



                    if (record.IsTMHeBig && cl.TMHeBig == count)
                    {
                        cl.TMHeBig++;
                    }

                    if (!record.IsTMHeBig && cl.TMHeSmall == count)
                    {
                        cl.TMHeSmall++;
                    }


                    if (record.IsTMHeDan && cl.TMHeDan == count)
                    {
                        cl.TMHeDan++;
                    }

                    if (!record.IsTMHeDan && cl.TMHeShuang == count)
                    {
                        cl.TMHeShuang++;
                    }

                    if (record.IsTMWeiBig && cl.TMWeiBig == count)
                    {
                        cl.TMWeiBig++;
                    }

                    if (!record.IsTMWeiBig && cl.TMWeiSmall == count)
                    {
                        cl.TMWeiSmall++;
                    }



                    if (record.IsTMJQ && cl.TMJQ == count)
                    {
                        cl.TMJQ++;
                    }

                    if (!record.IsTMJQ && cl.TMYS == count)
                    {
                        cl.TMYS++;
                    }



                    if (record.IsTMRed && cl.TMRed == count)
                    {
                        cl.TMRed++;
                    }

                    if (record.IsTMBlue && cl.TMBlue == count)
                    {
                        cl.TMBlue++;
                    }

                    if (record.IsTMGreen && cl.TMGreen == count)
                    {
                        cl.TMGreen++;
                    }


                    #endregion

                    #region Z1T

                    if (record.IsZ1TBig && cl.Z1TBig == count)
                    {
                        cl.Z1TBig++;
                    }

                    if (!record.IsZ1TBig && cl.Z1TSmall == count)
                    {
                        cl.Z1TSmall++;
                    }



                    if (record.IsZ1TDan && cl.Z1TDan == count)
                    {
                        cl.Z1TDan++;
                    }

                    if (!record.IsZ1TDan && cl.Z1TShuang == count)
                    {
                        cl.Z1TShuang++;
                    }



                    if (record.IsZ1THeBig && cl.Z1THeBig == count)
                    {
                        cl.Z1THeBig++;
                    }

                    if (!record.IsZ1THeBig && cl.Z1THeSmall == count)
                    {
                        cl.Z1THeSmall++;
                    }


                    if (record.IsZ1THeDan && cl.Z1THeDan == count)
                    {
                        cl.Z1THeDan++;
                    }

                    if (!record.IsZ1THeDan && cl.Z1THeShuang == count)
                    {
                        cl.Z1THeShuang++;
                    }

                    if (record.IsZ1TWeiBig && cl.Z1TWeiBig == count)
                    {
                        cl.Z1TWeiBig++;
                    }

                    if (!record.IsZ1TWeiBig && cl.Z1TWeiSmall == count)
                    {
                        cl.Z1TWeiSmall++;
                    }



                    if (record.IsZ1TJQ && cl.Z1TJQ == count)
                    {
                        cl.Z1TJQ++;
                    }

                    if (!record.IsZ1TJQ && cl.Z1TYS == count)
                    {
                        cl.Z1TYS++;
                    }



                    if (record.IsZ1TRed && cl.Z1TRed == count)
                    {
                        cl.Z1TRed++;
                    }

                    if (record.IsZ1TBlue && cl.Z1TBlue == count)
                    {
                        cl.Z1TBlue++;
                    }

                    if (record.IsZ1TGreen && cl.Z1TGreen == count)
                    {
                        cl.Z1TGreen++;
                    }


                    #endregion

                    #region Z2T

                    if (record.IsZ2TBig && cl.Z2TBig == count)
                    {
                        cl.Z2TBig++;
                    }

                    if (!record.IsZ2TBig && cl.Z2TSmall == count)
                    {
                        cl.Z2TSmall++;
                    }



                    if (record.IsZ2TDan && cl.Z2TDan == count)
                    {
                        cl.Z2TDan++;
                    }

                    if (!record.IsZ2TDan && cl.Z2TShuang == count)
                    {
                        cl.Z2TShuang++;
                    }



                    if (record.IsZ2THeBig && cl.Z2THeBig == count)
                    {
                        cl.Z2THeBig++;
                    }

                    if (!record.IsZ2THeBig && cl.Z2THeSmall == count)
                    {
                        cl.Z2THeSmall++;
                    }


                    if (record.IsZ2THeDan && cl.Z2THeDan == count)
                    {
                        cl.Z2THeDan++;
                    }

                    if (!record.IsZ2THeDan && cl.Z2THeShuang == count)
                    {
                        cl.Z2THeShuang++;
                    }

                    if (record.IsZ2TWeiBig && cl.Z2TWeiBig == count)
                    {
                        cl.Z2TWeiBig++;
                    }

                    if (!record.IsZ2TWeiBig && cl.Z2TWeiSmall == count)
                    {
                        cl.Z2TWeiSmall++;
                    }



                    if (record.IsZ2TJQ && cl.Z2TJQ == count)
                    {
                        cl.Z2TJQ++;
                    }

                    if (!record.IsZ2TJQ && cl.Z2TYS == count)
                    {
                        cl.Z2TYS++;
                    }



                    if (record.IsZ2TRed && cl.Z2TRed == count)
                    {
                        cl.Z2TRed++;
                    }

                    if (record.IsZ2TBlue && cl.Z2TBlue == count)
                    {
                        cl.Z2TBlue++;
                    }

                    if (record.IsZ2TGreen && cl.Z2TGreen == count)
                    {
                        cl.Z2TGreen++;
                    }


                    #endregion

                    #region Z3T

                    if (record.IsZ3TBig && cl.Z3TBig == count)
                    {
                        cl.Z3TBig++;
                    }

                    if (!record.IsZ3TBig && cl.Z3TSmall == count)
                    {
                        cl.Z3TSmall++;
                    }



                    if (record.IsZ3TDan && cl.Z3TDan == count)
                    {
                        cl.Z3TDan++;
                    }

                    if (!record.IsZ3TDan && cl.Z3TShuang == count)
                    {
                        cl.Z3TShuang++;
                    }



                    if (record.IsZ3THeBig && cl.Z3THeBig == count)
                    {
                        cl.Z3THeBig++;
                    }

                    if (!record.IsZ3THeBig && cl.Z3THeSmall == count)
                    {
                        cl.Z3THeSmall++;
                    }


                    if (record.IsZ3THeDan && cl.Z3THeDan == count)
                    {
                        cl.Z3THeDan++;
                    }

                    if (!record.IsZ3THeDan && cl.Z3THeShuang == count)
                    {
                        cl.Z3THeShuang++;
                    }

                    if (record.IsZ3TWeiBig && cl.Z3TWeiBig == count)
                    {
                        cl.Z3TWeiBig++;
                    }

                    if (!record.IsZ3TWeiBig && cl.Z3TWeiSmall == count)
                    {
                        cl.Z3TWeiSmall++;
                    }



                    if (record.IsZ3TJQ && cl.Z3TJQ == count)
                    {
                        cl.Z3TJQ++;
                    }

                    if (!record.IsZ3TJQ && cl.Z3TYS == count)
                    {
                        cl.Z3TYS++;
                    }



                    if (record.IsZ3TRed && cl.Z3TRed == count)
                    {
                        cl.Z3TRed++;
                    }

                    if (record.IsZ3TBlue && cl.Z3TBlue == count)
                    {
                        cl.Z3TBlue++;
                    }

                    if (record.IsZ3TGreen && cl.Z3TGreen == count)
                    {
                        cl.Z3TGreen++;
                    }


                    #endregion

                    #region Z4T

                    if (record.IsZ4TBig && cl.Z4TBig == count)
                    {
                        cl.Z4TBig++;
                    }

                    if (!record.IsZ4TBig && cl.Z4TSmall == count)
                    {
                        cl.Z4TSmall++;
                    }



                    if (record.IsZ4TDan && cl.Z4TDan == count)
                    {
                        cl.Z4TDan++;
                    }

                    if (!record.IsZ4TDan && cl.Z4TShuang == count)
                    {
                        cl.Z4TShuang++;
                    }



                    if (record.IsZ4THeBig && cl.Z4THeBig == count)
                    {
                        cl.Z4THeBig++;
                    }

                    if (!record.IsZ4THeBig && cl.Z4THeSmall == count)
                    {
                        cl.Z4THeSmall++;
                    }


                    if (record.IsZ4THeDan && cl.Z4THeDan == count)
                    {
                        cl.Z4THeDan++;
                    }

                    if (!record.IsZ4THeDan && cl.Z4THeShuang == count)
                    {
                        cl.Z4THeShuang++;
                    }

                    if (record.IsZ4TWeiBig && cl.Z4TWeiBig == count)
                    {
                        cl.Z4TWeiBig++;
                    }

                    if (!record.IsZ4TWeiBig && cl.Z4TWeiSmall == count)
                    {
                        cl.Z4TWeiSmall++;
                    }



                    if (record.IsZ4TJQ && cl.Z4TJQ == count)
                    {
                        cl.Z4TJQ++;
                    }

                    if (!record.IsZ4TJQ && cl.Z4TYS == count)
                    {
                        cl.Z4TYS++;
                    }



                    if (record.IsZ4TRed && cl.Z4TRed == count)
                    {
                        cl.Z4TRed++;
                    }

                    if (record.IsZ4TBlue && cl.Z4TBlue == count)
                    {
                        cl.Z4TBlue++;
                    }

                    if (record.IsZ4TGreen && cl.Z4TGreen == count)
                    {
                        cl.Z4TGreen++;
                    }


                    #endregion

                    #region Z5T

                    if (record.IsZ5TBig && cl.Z5TBig == count)
                    {
                        cl.Z5TBig++;
                    }

                    if (!record.IsZ5TBig && cl.Z5TSmall == count)
                    {
                        cl.Z5TSmall++;
                    }



                    if (record.IsZ5TDan && cl.Z5TDan == count)
                    {
                        cl.Z5TDan++;
                    }

                    if (!record.IsZ5TDan && cl.Z5TShuang == count)
                    {
                        cl.Z5TShuang++;
                    }



                    if (record.IsZ5THeBig && cl.Z5THeBig == count)
                    {
                        cl.Z5THeBig++;
                    }

                    if (!record.IsZ5THeBig && cl.Z5THeSmall == count)
                    {
                        cl.Z5THeSmall++;
                    }


                    if (record.IsZ5THeDan && cl.Z5THeDan == count)
                    {
                        cl.Z5THeDan++;
                    }

                    if (!record.IsZ5THeDan && cl.Z5THeShuang == count)
                    {
                        cl.Z5THeShuang++;
                    }

                    if (record.IsZ5TWeiBig && cl.Z5TWeiBig == count)
                    {
                        cl.Z5TWeiBig++;
                    }

                    if (!record.IsZ5TWeiBig && cl.Z5TWeiSmall == count)
                    {
                        cl.Z5TWeiSmall++;
                    }



                    if (record.IsZ5TJQ && cl.Z5TJQ == count)
                    {
                        cl.Z5TJQ++;
                    }

                    if (!record.IsZ5TJQ && cl.Z5TYS == count)
                    {
                        cl.Z5TYS++;
                    }



                    if (record.IsZ5TRed && cl.Z5TRed == count)
                    {
                        cl.Z5TRed++;
                    }

                    if (record.IsZ5TBlue && cl.Z5TBlue == count)
                    {
                        cl.Z5TBlue++;
                    }

                    if (record.IsZ5TGreen && cl.Z5TGreen == count)
                    {
                        cl.Z5TGreen++;
                    }


                    #endregion

                    #region Z6T

                    if (record.IsZ6TBig && cl.Z6TBig == count)
                    {
                        cl.Z6TBig++;
                    }

                    if (!record.IsZ6TBig && cl.Z6TSmall == count)
                    {
                        cl.Z6TSmall++;
                    }



                    if (record.IsZ6TDan && cl.Z6TDan == count)
                    {
                        cl.Z6TDan++;
                    }

                    if (!record.IsZ6TDan && cl.Z6TShuang == count)
                    {
                        cl.Z6TShuang++;
                    }



                    if (record.IsZ6THeBig && cl.Z6THeBig == count)
                    {
                        cl.Z6THeBig++;
                    }

                    if (!record.IsZ6THeBig && cl.Z6THeSmall == count)
                    {
                        cl.Z6THeSmall++;
                    }


                    if (record.IsZ6THeDan && cl.Z6THeDan == count)
                    {
                        cl.Z6THeDan++;
                    }

                    if (!record.IsZ6THeDan && cl.Z6THeShuang == count)
                    {
                        cl.Z6THeShuang++;
                    }

                    if (record.IsZ6TWeiBig && cl.Z6TWeiBig == count)
                    {
                        cl.Z6TWeiBig++;
                    }

                    if (!record.IsZ6TWeiBig && cl.Z6TWeiSmall == count)
                    {
                        cl.Z6TWeiSmall++;
                    }



                    if (record.IsZ6TJQ && cl.Z6TJQ == count)
                    {
                        cl.Z6TJQ++;
                    }

                    if (!record.IsZ6TJQ && cl.Z6TYS == count)
                    {
                        cl.Z6TYS++;
                    }



                    if (record.IsZ6TRed && cl.Z6TRed == count)
                    {
                        cl.Z6TRed++;
                    }

                    if (record.IsZ6TBlue && cl.Z6TBlue == count)
                    {
                        cl.Z6TBlue++;
                    }

                    if (record.IsZ6TGreen && cl.Z6TGreen == count)
                    {
                        cl.Z6TGreen++;
                    }


                    #endregion

                }

                count++;
            }

            return cl;
        }


        //------------------------------------快速相关----------------------------------
        //SSC
        public static List<int> wan = new List<int>();
        public static List<int> qian = new List<int>();
        public static List<int> bai = new List<int>();
        public static List<int> shi = new List<int>();
        public static List<int> ge = new List<int>();


        public static List<int> wan2 = new List<int>();
        public static List<int> qian2 = new List<int>();
        public static List<int> bai2 = new List<int>();
        public static List<int> shi2 = new List<int>();
        public static List<int> ge2 = new List<int>();

        //3D
        public static List<int> wanFor3D = new List<int>();
        public static List<int> qianFor3D = new List<int>();
        public static List<int> baiFor3D = new List<int>();

        public static List<int> wanFor3D2 = new List<int>();
        public static List<int> qianFor3D2 = new List<int>();
        public static List<int> baiFor3D2 = new List<int>();

        //PK10
        public static List<int> wanForPK10 = new List<int>();
        public static List<int> qianForPK10 = new List<int>();
        public static List<int> baiForPK10 = new List<int>();
        public static List<int> shiForPK10 = new List<int>();
        public static List<int> geForPK10 = new List<int>();
        public static List<int> sixForPK10 = new List<int>();
        public static List<int> sevenForPK10 = new List<int>();
        public static List<int> eightForPK10 = new List<int>();
        public static List<int> nineForPK10 = new List<int>();
        public static List<int> tenForPK10 = new List<int>();

        public static List<int> wanForPK102 = new List<int>();
        public static List<int> qianForPK102 = new List<int>();
        public static List<int> baiForPK102 = new List<int>();
        public static List<int> shiForPK102 = new List<int>();
        public static List<int> geForPK102 = new List<int>();
        public static List<int> sixForPK102 = new List<int>();
        public static List<int> sevenForPK102 = new List<int>();
        public static List<int> eightForPK102 = new List<int>();
        public static List<int> nineForPK102 = new List<int>();
        public static List<int> tenForPK102 = new List<int>();


        //6HeCai
        public static List<int> wanFor6HeCai = new List<int>();
        public static List<int> qianFor6HeCai = new List<int>();
        public static List<int> baiFor6HeCai = new List<int>();
        public static List<int> shiFor6HeCai = new List<int>();
        public static List<int> geFor6HeCai = new List<int>();
        public static List<int> sixFor6HeCai = new List<int>();
        public static List<int> sevenFor6HeCai = new List<int>();


        public static List<int> wanFor6HeCai2 = new List<int>();
        public static List<int> qianFor6HeCai2 = new List<int>();
        public static List<int> baiFor6HeCai2 = new List<int>();
        public static List<int> shiFor6HeCai2 = new List<int>();
        public static List<int> geFor6HeCai2 = new List<int>();
        public static List<int> sixFor6HeCai2 = new List<int>();
        public static List<int> sevenFor6HeCai2 = new List<int>();

        //7XingCai
        public static List<int> wanFor7XingCai = new List<int>();
        public static List<int> qianFor7XingCai = new List<int>();
        public static List<int> baiFor7XingCai = new List<int>();
        public static List<int> shiFor7XingCai = new List<int>();
        public static List<int> geFor7XingCai = new List<int>();
        public static List<int> sixFor7XingCai = new List<int>();
        public static List<int> sevenFor7XingCai = new List<int>();


        public static List<int> wanFor7XingCai2 = new List<int>();
        public static List<int> qianFor7XingCai2 = new List<int>();
        public static List<int> baiFor7XingCai2 = new List<int>();
        public static List<int> shiFor7XingCai2 = new List<int>();
        public static List<int> geFor7XingCai2 = new List<int>();
        public static List<int> sixFor7XingCai2 = new List<int>();
        public static List<int> sevenFor7XingCai2 = new List<int>();



        #region 创建模拟号码-----随机生成

        public static string CreateOpenNumFor10Sc()
        {

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int[] inumber = new int[5];

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                inumber[i] = ran.Next(0, 10);
            }

            string code = String.Join(",", inumber);
            return code;

        }

        public static string CreateOpenNumFor3D()
        {

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int[] inumber = new int[3];

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                inumber[i] = ran.Next(0, 10);
            }

            string code = String.Join(",", inumber);
            return code;

        }

        public static string CreateOpenNumForK3()
        {

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int[] inumber = new int[3];

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                inumber[i] = ran.Next(1, 7);
            }

            string code = String.Join(",", inumber);
            return code;

        }


        public static string CreateOpenNumForPk10()
        {

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int[] inumber = new int[10];

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);

                int temp = ran.Next(1, 11);
                while (inumber.Contains(temp))
                {
                    temp = ran.Next(1, 11);
                }

                inumber[i] = temp;
            }

            //交换-------------------防止前两位形态一样
            int a = inumber[1];
            int b = inumber[6];

            inumber[1] = b;
            inumber[6] = a;

            string code = String.Join(",", inumber);
            return code;

        }

        public static string CreateOpenNumFor6HeCai()
        {

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int[] inumber = new int[7];

            for (int i = 0; i < 7; i++)
            {
                Thread.Sleep(500);
                int temp = ran.Next(1, 50);

                while (inumber.Contains(temp))
                {
                    temp = ran.Next(1, 50);
                }

                inumber[i] = temp;
            }

            string code = String.Join(",", inumber);

            return Hand6HeCaiNum(code);

        }






        public static string CreateOpenNumFor7XingCai()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            string[] inumber = new string[7];

            for (int i = 0; i < 7; i++)
            {
                Thread.Sleep(500);
                int temp = ran.Next(0, 10);
                string s = temp + "";

                while (inumber.Contains(s))
                {
                    temp = ran.Next(0, 10);
                    s = temp + "";
                }

                inumber[i] = s;
            }

            //交换-------------------防止前两位形态一样
            string a = inumber[1];
            string b = inumber[6];

            inumber[1] = b;
            inumber[6] = a;

            string code = String.Join(",", inumber);
            return code;
        }


        public static string CreateOpenNumForKL10F()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            string[] inumber = new string[8];

            for (int i = 0; i < 8; i++)
            {
                Thread.Sleep(500);
                int temp = ran.Next(1, 21);
                string s = temp + "";

                while (inumber.Contains(s))
                {
                    temp = ran.Next(1, 21);
                    s = temp + "";
                }

                inumber[i] = s;
            }

            //交换-------------------防止前两位形态一样
            string a = inumber[1];
            string b = inumber[6];

            inumber[1] = b;
            inumber[6] = a;

            string code = String.Join(",", inumber);
            return code;
        }


        public static string CreateOpenNumFor11X5()
        {

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int[] inumber = new int[5];

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                int temp = ran.Next(1, 12);

                while (inumber.Contains(temp))
                {
                    temp = ran.Next(1, 12);
                }

                inumber[i] = temp;
            }

            string code = String.Join(",", inumber);

            return Hand6HeCaiNum(code);

        }


        public static string GetOpenNumForSJ(int lType)
        {

            if (lType == 61) lType = 2;

            string sql = "select count(1) from Data where lType = " + lType;
            int count = (int)SqlHelper.ExecuteScalarForFenZhan(77, sql);

            if (count == 0)
            {
                string num = "";

                if (lType == 2 || lType == 61)
                {
                    num = CreateOpenNumFor10Sc();
                }
                else if (lType == 4)
                {
                    num = CreateOpenNumFor6HeCai();
                }
                else if (lType == 6)
                {
                    num = CreateOpenNumFor7XingCai();
                }
                else if (lType == 8 || lType == 10 || lType == 62)
                {
                    num = CreateOpenNumForPk10();
                }
                else if (lType == 12 || lType == 20 || lType == 24 || lType == 64 || lType == 84)
                {
                    num = CreateOpenNumFor3D();
                }
                else if (lType == 14)
                {
                    num = CreateOpenNumForKL10F();
                }
                else if (lType == 16)
                {
                    num = CreateOpenNumFor11X5();
                }
                else if (lType == 22)
                {
                    num = CreateOpenNumForK3();
                }


                return num;
            }
            else
            {
                int index = Util.RandomANum(1, count);

                //LogHelper.WriteLog("--------------------->" + index);

                sql = "select top 1 temp.Num from ( select row_number() over(order by Id) as rownumber,* from (select * from Data where lType = " + lType + ") as t1)as temp where rownumber = " + index;

                return SqlHelper.ExecuteScalarForFenZhan(77, sql).ToString();


            }

            //return "";

        }

        public static string GetOpenNumForSJForNumSC(int lType)
        {

            string num = "";

            if (lType == 2)
            {
                num = CreateOpenNumFor10Sc();
            }
            else if (lType == 4)
            {
                num = CreateOpenNumFor6HeCai();
            }
            else if (lType == 6)
            {
                num = CreateOpenNumFor7XingCai();
            }
            else if (lType == 8 || lType == 10 || lType == 62)
            {
                num = CreateOpenNumForPk10();
            }
            else if (lType == 12 || lType == 20 || lType == 24 || lType == 64 || lType == 84)
            {
                num = CreateOpenNumFor3D();
            }
            else if (lType == 14)
            {
                num = CreateOpenNumForKL10F();
            }
            else if (lType == 16)
            {
                num = CreateOpenNumFor11X5();
            }
            else if (lType == 22)
            {
                num = CreateOpenNumForK3();
            }


            return num;


        }


        #endregion


        #region 创建模拟号码-----盈利

        public static string CreateOpenNumFor10ScYingLi(string issue)
        {
            wan.Clear();
            qian.Clear();
            bai.Clear();
            shi.Clear();
            ge.Clear();

            wan2.Clear();
            qian2.Clear();
            bai2.Clear();
            shi2.Clear();
            ge2.Clear();

            string sql = "select * from BettingRecord where lType= 2  and UserId <> 0 and WinState = 1 and Issue='" + issue + "'"; //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));



            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor10Sc();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum }
                           into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();

            //list = list.OrderByDescending(p => p.WinMoney)

            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 2);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }


            //转化成集合 方便使用Remove方法
            //list = data.ToList();


            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor10Sc(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            GetChaNumFor10Sc();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor10Sc();

            for (int i = 0; i < 10; i++)
            {
                if (CheckSecondFor10Sc(num, list))
                {
                    break;
                }

                Thread.Sleep(500);

                num = GetSuiJiNumFor10Sc();
            }

            return num;
        }

        public static string CreateOpenNumFor3DYingLi(int lType, string issue)
        {
            wanFor3D.Clear();
            qianFor3D.Clear();
            baiFor3D.Clear();


            wanFor3D2.Clear();
            qianFor3D2.Clear();
            baiFor3D2.Clear();

            string sql = "select * from BettingRecord where lType= " + lType + "  and UserId <> 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor3D();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();

            //list = list.OrderByDescending(p => p.WinMoney)

            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 12);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }



            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor3D(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            GetChaNumFor3D();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor3D();

            for (int i = 0; i < 20; i++)
            {
                if (CheckSecondFor3D(num, list))
                {
                    break;
                }

                Thread.Sleep(500);

                num = GetSuiJiNumFor3D();
            }

            return num;
        }

        public static string CreateOpenNumForPk10YingLi(string issue)
        {
            wanForPK10.Clear();
            qianForPK10.Clear();
            baiForPK10.Clear();
            shiForPK10.Clear();
            geForPK10.Clear();
            sixForPK10.Clear();
            sevenForPK10.Clear();
            eightForPK10.Clear();
            nineForPK10.Clear();
            tenForPK10.Clear();


            wanForPK102.Clear();
            qianForPK102.Clear();
            baiForPK102.Clear();
            shiForPK102.Clear();
            geForPK102.Clear();
            sixForPK102.Clear();
            sevenForPK102.Clear();
            eightForPK102.Clear();
            nineForPK102.Clear();
            tenForPK102.Clear();

            string sql = "select * from BettingRecord where lType= 8  and UserId <> 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户    
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumForPk10();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();

            //list = list.OrderByDescending(p => p.WinMoney)

            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 8);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }



            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumForPk10(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            GetChaNumForPk10();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumForPk10();

            for (int i = 0; i < 20; i++)
            {
                if (CheckSecondForPk10(num, list))
                {
                    break;
                }

                num = GetSuiJiNumForPk10();
            }


            //检查重复
            return RemoveRepeatForPk10(num);

            //return num;
        }

        public static string CreateOpenNumFor6HeCaiYingLi(string issue)
        {
            wanFor6HeCai.Clear();
            qianFor6HeCai.Clear();
            baiFor6HeCai.Clear();
            shiFor6HeCai.Clear();
            geFor6HeCai.Clear();
            sixFor6HeCai.Clear();
            sevenFor6HeCai.Clear();



            wanFor6HeCai2.Clear();
            qianFor6HeCai2.Clear();
            baiFor6HeCai2.Clear();
            shiFor6HeCai2.Clear();
            geFor6HeCai2.Clear();
            sixFor6HeCai2.Clear();
            sevenFor6HeCai2.Clear();


            string sql = "select * from BettingRecord where lType = 4  and UserId > 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户    
            List<BettingRecord> bettingData = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
            bettingData.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));



            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor6HeCai();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();


            //特殊情况

            #region 特殊情况

            //string playName = list[0].PlayName;
            //string betNum = list[0].BetNum;

            //if (playName == "五不中" || playName == "六不中" || playName == "七不中" || playName == "八不中" || playName == "九不中" ||
            //    playName == "十不中")
            //{
            //    string num2 = CreateOpenNumFor6HeCai();
            //    string[] arr2 = num2.Split(',');

            //    string[] arr = betNum.Split(',');
            //    int count2 = 0;
            //    foreach (string s in arr)
            //    {
            //        if (arr2.Contains(s))
            //        {
            //            count2++;
            //        }
            //    }

            //    if (count2 > 0)
            //    {
            //        return num2;
            //    }
            //    else
            //    {
            //        int index = Util.RandomANum(0, 7);
            //        int index2 = Util.RandomANum(0, 5);
            //        arr2[index] = arr[index2];
            //        return string.Join(",", arr2);
            //    }
            //}


            #endregion





            //list = list.OrderByDescending(p => p.WinMoney)

            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 4);


            int count = list.Count;

            //去掉前半部分 留下大额的
            //if (count > 1)
            //{
            //    if (list.Count() / 2 == 0)
            //    {
            //        list = list.Take(count / 2).ToList();
            //    }
            //    else
            //    {
            //        list = list.Take(count / 2 + 1).ToList();
            //    }
            //}





            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor6HeCai(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            GetChaNumFor6HeCai();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor6HeCai();

            for (int i = 0; i < 20; i++)
            {
                if (CheckSecondFor6HeCai(num, list))
                {
                    break;
                }

                num = GetSuiJiNumFor6HeCai();
            }


            //检查重复号码
            num = RemoveRepeatForLHC(num);

            return Hand6HeCaiNum(num);
        }

        public static string CreateOpenNumFor7XingCaiYingLi(string issue)
        {
            wanFor7XingCai.Clear();
            qianFor7XingCai.Clear();
            baiFor7XingCai.Clear();
            shiFor7XingCai.Clear();
            geFor7XingCai.Clear();
            sixFor7XingCai.Clear();
            sevenFor7XingCai.Clear();



            wanFor7XingCai2.Clear();
            qianFor7XingCai2.Clear();
            baiFor7XingCai2.Clear();
            shiFor7XingCai2.Clear();
            geFor7XingCai2.Clear();
            sixFor7XingCai2.Clear();
            sevenFor7XingCai2.Clear();


            string sql = "select * from BettingRecord where lType= 6 and UserId <> 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户    
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor7XingCai();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();

            //list = list.OrderByDescending(p => p.WinMoney)

            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 6);


            int count = list.Count;

            //去掉前半部分 留下大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }



            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor7XingCai(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            GetChaNumFor7XingCai();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor7XingCai();

            //for (int i = 0; i < 20; i++)
            //{
            //    if (CheckSecondFor6HeCai(num, list))
            //    {
            //        break;
            //    }

            //    num = GetSuiJiNumFor6HeCai();
            //}

            return num;
        }


        #endregion

        #region 创建模拟号码2----------------盈利--------------牛逼到爆的模式


        public static string CreateOpenNumFor6HeCaiYingLi2(int lType, string issue)
        {
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            for (int i = 0; i < 5; i++)
            {

                string num = "";

                if (lType == 4)
                {
                    num = CreateOpenNumFor6HeCai();
                }
                else if (lType == 6)
                {
                    num = CreateOpenNumFor7XingCai();
                }

                decimal money = DealOpen2.HandCurrentBetting(1, lType, issue, num);
                money += DealOpen2.HandCurrentBetting(2, lType, issue, num);
                money += DealOpen2.HandCurrentBetting(3, lType, issue, num);

                if (!dic.ContainsKey(num))
                {
                    dic.Add(num, money);
                }

            }

            Dictionary<string, decimal> dic2 = dic.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value);

            string result = dic2.ElementAt(0).Key;

            return result;
        }

        //提前准备10期号码
        //开杀模式
        public static string GetOpenNumForYingLi(int lType, string issue, string kaiguan)
        {
            DateTime d1 = DateTime.Now;

            int LHCCOUNT = 10;

            string result = "";


            if (kaiguan.EndsWith("2"))
            {
                LHCCOUNT = 20;
            }
            else if (kaiguan.EndsWith("3"))
            {
                LHCCOUNT = 30;
            }
            else if (kaiguan.EndsWith("4"))
            {
                LHCCOUNT = 40;
            }
            else if (kaiguan.EndsWith("5"))
            {
                LHCCOUNT = 50;
            }




            string sql = "select top(" + LHCCOUNT + ") Num from Data where lType = " + lType;
            var data = Util.ReaderToListForFenZhan<DATA>(77, sql);


            //LogHelper.WriteLog(lType + "--" + issue + "--" + kaiguan + "--" + sql);

            List<string> list = new List<string>();



            foreach (DATA d in data)
            {
                list.Add(d.Num);
            }


            //LogHelper.WriteLog("--------->" + list.Count);

            sql = "select * from BettingRecord where UserId > 0 and Issue='" + issue + "' and WinState = 1 and lType =" + lType;
            List<BettingRecord> betData1 = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
            List<BettingRecord> betData2 = Util.ReaderToListForFenZhan<BettingRecord>(2, sql);
            List<BettingRecord> betData3 = Util.ReaderToListForFenZhan<BettingRecord>(3, sql);
            List<BettingRecord> betData4 = Util.ReaderToListForFenZhan<BettingRecord>(4, sql);
            List<BettingRecord> betData5 = Util.ReaderToListForFenZhan<BettingRecord>(5, sql);
            List<BettingRecord> betData6 = Util.ReaderToListForFenZhan<BettingRecord>(6, sql);



            int count = betData1.Count + betData2.Count + betData3.Count + betData4.Count + betData5.Count + betData6.Count;
            //LogHelper.WriteLog("Count------>" + count);

            //特殊情况
            //if (count == 0) return GetOpenNumForSJ(lType);


            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            foreach (string num in list)
            {
                decimal money = DealOpen2.HandCurrentBettingForNoSerch(betData1, 1, lType, issue, num);
                money += DealOpen2.HandCurrentBettingForNoSerch(betData2, 2, lType, issue, num);
                money += DealOpen2.HandCurrentBettingForNoSerch(betData3, 3, lType, issue, num);
                money += DealOpen2.HandCurrentBettingForNoSerch(betData4, 4, lType, issue, num);
                money += DealOpen2.HandCurrentBettingForNoSerch(betData5, 5, lType, issue, num);
                money += DealOpen2.HandCurrentBettingForNoSerch(betData6, 6, lType, issue, num);

                if (!dic.ContainsKey(num))
                {
                    dic.Add(num, money);
                }
            }

            Dictionary<string, decimal> dic2 = dic.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value);


            #region 废品

            //if (lType == 24)
            //{

            //    foreach (KeyValuePair<string, decimal> keyValuePair in dic2)
            //    {
            //        LogHelper.WriteLog(issue + "--->" + keyValuePair.Key + "---->" + keyValuePair.Value);
            //    }

            //}





            //DateTime d2 = DateTime.Now;

            //LogHelper.WriteLog("时间1---->" + (d2 - d1).TotalSeconds);


            //decimal totalBetMoney = betData1.Sum(p => p.BetCount * p.UnitMoney) +
            //                            betData2.Sum(p => p.BetCount * p.UnitMoney) +
            //                            betData3.Sum(p => p.BetCount * p.UnitMoney) +
            //                            betData4.Sum(p => p.BetCount * p.UnitMoney) +
            //                            betData5.Sum(p => p.BetCount * p.UnitMoney) +
            //                            betData6.Sum(p => p.BetCount * p.UnitMoney)
            //                            ;


            //if (kaiguan.Contains("平"))
            //{

            //Dictionary<string, decimal> dic3 = new Dictionary<string, decimal>();

            //foreach (KeyValuePair<string, decimal> keyValuePair in dic2)
            //{
            //    if (totalBetMoney > keyValuePair.Value)
            //    {
            //        decimal cha = totalBetMoney - keyValuePair.Value;
            //        //if (cha < 0) cha = -cha;
            //        dic3.Add(keyValuePair.Key, cha);
            //    }

            //}

            //if (dic3.Count == 0)
            //{
            //    result = dic2.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value).ElementAt(0).Key;
            //}
            //else
            //{
            //    result = dic3.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value).ElementAt(0).Key;
            //}




            //总数平衡
            //string date = DateTime.Now.ToString("yyyy-MM-dd");

            //sql = "select * from BettingRecord where UserId > 0 and SubTime > '" + date + "' and WinState > 1 and lType =" + lType;
            //List<BettingRecord> openedList = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
            //openedList.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
            //openedList.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
            //openedList.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
            //openedList.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
            //openedList.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

            //decimal allBetMoney = openedList.Sum(p => p.BetCount * p.UnitMoney);
            //decimal allWinMoney = openedList.Sum(p => p.WinMoney + p.TuiShui);


            //decimal yingkui = allWinMoney - allBetMoney;

            //LogHelper.WriteLog("盈亏---------->" + yingkui);

            //if (yingkui < 0) yingkui = -yingkui;

            //if (yingkui > 5000)
            //{
            //    if (allBetMoney < allWinMoney)
            //    {
            //        kaiguan = "开";
            //    }
            //    else
            //    {
            //        kaiguan = "送";
            //    }

            //}
            //else
            //{
            //    long tick = DateTime.Now.Ticks;
            //    Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            //    int r = ran.Next(0, 10);

            //    if (r >= 5)
            //    {
            //        kaiguan = "开";
            //    }
            //    else
            //    {
            //        kaiguan = "送";
            //    }
            //}



            //}




            //if (kaiguan.Contains("送"))
            //{

            //    Dictionary<string, decimal> dic3 = new Dictionary<string, decimal>();

            //    foreach (KeyValuePair<string, decimal> keyValuePair in dic2)
            //    {
            //        if (keyValuePair.Value < totalBetMoney * 2)
            //        {

            //            dic3.Add(keyValuePair.Key, keyValuePair.Value);
            //        }
            //    }

            //    result = dic3.OrderByDescending(p => p.Value).ToDictionary(a => a.Key, p => p.Value).ElementAt(0).Key;

            //    //result = dic2.ElementAt(9).Key;

            //}
            //else if (kaiguan.Contains("开"))
            //{
            //    result = dic2.ElementAt(0).Key;
            //}

            #endregion




            if (kaiguan.Contains("开"))
            {

                result = dic2.ElementAt(0).Key;
            }
            else if (kaiguan.Contains("放"))
            {

                result = dic2.ElementAt(dic.Count - 1).Key;
            }


            return result;
        }





        //新版腾讯
        public static string GetOpenNumForYingLiToTXFFC(int lType)
        {
            DateTime d1 = DateTime.Now;

            int LHCCOUNT = 10;

            string result = "";


            string sql = "select top(" + LHCCOUNT + ") Num from Data where lType = " + 2;
            var data = Util.ReaderToListForFenZhan<DATA>(1, sql);


            //LogHelper.WriteLog(lType + "--" + issue + "--" + kaiguan + "--" + sql);

            List<string> list = new List<string>();



            foreach (DATA d in data)
            {
                list.Add(d.Num);
            }


            //LogHelper.WriteLog("--------->" + list.Count);

            //sql = "select * from BettingRecord where UserId = 33157 and WinState = 1 and lType =" + lType;
            sql = "select * from BettingRecord where WinState = 1 and lType =" + lType;

            List<BettingRecord> betData1 = Util.ReaderToListForFenZhan<BettingRecord>(77, sql);
            //List<BettingRecord> betData2 = Util.ReaderToListForFenZhan<BettingRecord>(2, sql);
            //List<BettingRecord> betData3 = Util.ReaderToListForFenZhan<BettingRecord>(3, sql);
            //List<BettingRecord> betData4 = Util.ReaderToListForFenZhan<BettingRecord>(4, sql);
            //List<BettingRecord> betData5 = Util.ReaderToListForFenZhan<BettingRecord>(5, sql);
            //List<BettingRecord> betData6 = Util.ReaderToListForFenZhan<BettingRecord>(6, sql);



            //int count = betData1.Count + betData2.Count + betData3.Count + betData4.Count + betData5.Count + betData6.Count;
            //LogHelper.WriteLog("Count------>" + count);

            //特殊情况
            //if (count == 0) return GetOpenNumForSJ(lType);


            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            foreach (string num in list)
            {
                decimal money = DealOpen2.HandCurrentBettingForNoSerch2ForTXFFC(betData1, 77, lType, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData2, 2, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData3, 3, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData4, 4, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData5, 5, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData6, 6, lType, issue, num);

                if (!dic.ContainsKey(num))
                {
                    dic.Add(num, money);
                }
            }

            Dictionary<string, decimal> dic2 = dic.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value);






            //if (kaiguan.Contains("开"))
            //{
            //    result = dic2.ElementAt(0).Key;
            //}
            //else if (kaiguan.Contains("放"))
            //{
            //    result = dic2.ElementAt(dic.Count - 1).Key;
            //}
            //LogHelper.WriteLog("------------------START----------------");

            //foreach (var d in dic2)
            //{
            //    LogHelper.WriteLog(d.Key + "---->" + d.Value);
            //}

            //LogHelper.WriteLog("------------------END----------------");

            int index = Util.RandomANum(2, 8);

            result = dic2.ElementAt(index).Key;


            return result;
        }



        public static string GetOpenNumForYingLiToAZXY5(int lType)
        {
            DateTime d1 = DateTime.Now;

            int LHCCOUNT = 10;

            string result = "";


            string sql = "select top(" + LHCCOUNT + ") Num from Data where lType = " + 2;
            var data = Util.ReaderToListForFenZhan<DATA>(2, sql);


            //LogHelper.WriteLog(lType + "--" + issue + "--" + kaiguan + "--" + sql);

            List<string> list = new List<string>();



            foreach (DATA d in data)
            {
                list.Add(d.Num);
            }


            //LogHelper.WriteLog("--------->" + list.Count);

            //sql = "select * from BettingRecord where UserId = 33157 and WinState = 1 and lType =" + lType;
            sql = "select * from BettingRecord where WinState = 1 and lType =" + lType;

            List<BettingRecord> betData1 = Util.ReaderToListForFenZhan<BettingRecord>(77, sql);
            //List<BettingRecord> betData2 = Util.ReaderToListForFenZhan<BettingRecord>(2, sql);
            //List<BettingRecord> betData3 = Util.ReaderToListForFenZhan<BettingRecord>(3, sql);
            //List<BettingRecord> betData4 = Util.ReaderToListForFenZhan<BettingRecord>(4, sql);
            //List<BettingRecord> betData5 = Util.ReaderToListForFenZhan<BettingRecord>(5, sql);
            //List<BettingRecord> betData6 = Util.ReaderToListForFenZhan<BettingRecord>(6, sql);



            //int count = betData1.Count + betData2.Count + betData3.Count + betData4.Count + betData5.Count + betData6.Count;
            //LogHelper.WriteLog("Count------>" + count);

            //特殊情况
            //if (count == 0) return GetOpenNumForSJ(lType);


            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            foreach (string num in list)
            {
                decimal money = DealOpen2.HandCurrentBettingForNoSerch2ForTXFFC(betData1, 77, lType, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData2, 2, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData3, 3, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData4, 4, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData5, 5, lType, issue, num);
                //money += DealOpen2.HandCurrentBettingForNoSerch(betData6, 6, lType, issue, num);

                if (!dic.ContainsKey(num))
                {
                    dic.Add(num, money);
                }
            }

            Dictionary<string, decimal> dic2 = dic.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value);






            //if (kaiguan.Contains("开"))
            //{
            //    result = dic2.ElementAt(0).Key;
            //}
            //else if (kaiguan.Contains("放"))
            //{
            //    result = dic2.ElementAt(dic.Count - 1).Key;
            //}
            //LogHelper.WriteLog("------------------START----------------");

            //foreach (var d in dic2)
            //{
            //    LogHelper.WriteLog(d.Key + "---->" + d.Value);
            //}

            //LogHelper.WriteLog("------------------END----------------");

            int index = Util.RandomANum(5, 9);

            result = dic2.ElementAt(index).Key;


            return result;
        }






        public static string CreateOpenNumForPk10YingLi2(int lType, string issue)
        {
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            string num = "";

            for (int i = 0; i < 5; i++)
            {

                if (lType == 2)
                {
                    num = CreateOpenNumFor10Sc();
                }
                else if (lType == 8)
                {
                    num = CreateOpenNumForPk10();
                }
                else if (lType == 12)
                {
                    num = CreateOpenNumFor3D();
                }
                else if (lType == 22)
                {
                    num = CreateOpenNumForK3();
                }

                decimal money = DealOpen2.HandCurrentBetting(1, lType, issue, num);
                money += DealOpen2.HandCurrentBetting(2, lType, issue, num);
                money += DealOpen2.HandCurrentBetting(3, lType, issue, num);

                if (!dic.ContainsKey(num))
                {
                    dic.Add(num, money);
                }

            }

            Dictionary<string, decimal> dic2 = dic.OrderBy(p => p.Value).ToDictionary(a => a.Key, p => p.Value);

            string result = dic2.ElementAt(0).Key;

            return result;
        }



        #endregion

        #region 创建模拟号码-----放水

        public static string CreateOpenNumFor10ScFangShui(string issue)
        {
            wan.Clear();
            qian.Clear();
            bai.Clear();
            shi.Clear();
            ge.Clear();

            string sql = "select * from BettingRecord where lType= 2  and UserId = 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor10Sc();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();


            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 2);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }


            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor10Sc(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            //GetChaNum();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor10ScFangShui();

            //for (int i = 0; i < 20; i++)
            //{
            //    if (CheckSecond(num, list))
            //    {
            //        break;
            //    }

            //    num = GetSuiJiNum();
            //}

            return num;
        }

        public static string CreateOpenNumFor3DFangShui(string issue)
        {
            wanFor3D.Clear();
            qianFor3D.Clear();
            baiFor3D.Clear();

            string sql = "select * from BettingRecord where lType= 12  and UserId = 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor3D();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();


            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 12);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }


            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor3D(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            //GetChaNum();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor3DFangShui();

            //for (int i = 0; i < 20; i++)
            //{
            //    if (CheckSecond(num, list))
            //    {
            //        break;
            //    }

            //    num = GetSuiJiNum();
            //}

            return num;
        }

        public static string CreateOpenNumForPk10FangShui(string issue)
        {
            wanForPK10.Clear();
            qianForPK10.Clear();
            baiForPK10.Clear();
            shiForPK10.Clear();
            geForPK10.Clear();
            sixForPK10.Clear();
            sevenForPK10.Clear();
            eightForPK10.Clear();
            nineForPK10.Clear();
            tenForPK10.Clear();

            string sql = "select * from BettingRecord where lType= 8  and UserId = 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumForPk10();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();


            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 8);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }


            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumForPk10(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            //GetChaNum();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumForPk10FangShui();

            //for (int i = 0; i < 20; i++)
            //{
            //    if (CheckSecond(num, list))
            //    {
            //        break;
            //    }

            //    num = GetSuiJiNum();
            //}

            return num;
        }

        public static string CreateOpenNumFor6HeCaiFangShui(string issue)
        {
            wanFor6HeCai.Clear();
            qianFor6HeCai.Clear();
            baiFor6HeCai.Clear();
            shiFor6HeCai.Clear();
            geFor6HeCai.Clear();
            sixFor6HeCai.Clear();
            sevenFor6HeCai.Clear();


            string sql = "select * from BettingRecord where lType= 4 and UserId = 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor6HeCai();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();


            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 4);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }


            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor6HeCai(win);
                //Console.WriteLine(win.PlayName + "\t" + win.BetNum + "\t" + win.WinMoney);
            }

            //GetChaNum();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor6HeCaiFangShui();

            //for (int i = 0; i < 20; i++)
            //{
            //    if (CheckSecond(num, list))
            //    {
            //        break;
            //    }

            //    num = GetSuiJiNum();
            //}

            return Hand6HeCaiNum(num);
        }

        public static string CreateOpenNumFor7XingCaiFangShui(string issue)
        {
            wanFor7XingCai.Clear();
            qianFor7XingCai.Clear();
            baiFor7XingCai.Clear();
            shiFor7XingCai.Clear();
            geFor7XingCai.Clear();
            sixFor7XingCai.Clear();
            sevenFor7XingCai.Clear();


            string sql = "select * from BettingRecord where lType= 6 and UserId = 0 and WinState = 1 and Issue='" + issue + "'";   //针对 非试玩用户
            List<BettingRecord> bettingData = Util.ReaderToList<BettingRecord>(sql);


            //如果没有数据
            if (bettingData.Count == 0) return CreateOpenNumFor7XingCai();

            List<PossibleWin> list = new List<PossibleWin>();

            foreach (BettingRecord record in bettingData)
            {
                PossibleWin pw = new PossibleWin();
                pw.PlayName = record.PlayName;
                pw.BetNum = record.BetNum;
                pw.WinMoney = GetPossibleWinMoney(record);

                list.Add(pw);
            }


            //按 playName  betNum分组
            var data = from s in list
                       group s by new { s.PlayName, s.BetNum } into g
                       select new PossibleWin
                       {
                           PlayName = g.Key.PlayName,
                           BetNum = g.Key.BetNum,
                           WinMoney = g.Sum(s => s.WinMoney)
                       };

            //排序
            data = data.OrderByDescending(p => p.WinMoney);

            list = data.ToList();


            //删除  大小这种互补的 留下大额的
            list = Common.DeleteHuBuBet(list, 6);


            int count = list.Count;

            //去掉前半部分 大额的
            if (count > 1)
            {
                if (list.Count() / 2 == 0)
                {
                    list = list.Take(count / 2).ToList();
                }
                else
                {
                    list = list.Take(count / 2 + 1).ToList();
                }
            }


            //重新排序
            list = list.OrderBy(p => p.WinMoney).ToList();


            //最后一步 干掉这些单子 得出一组号码  从小到大


            //第一轮 -------------获取每个位置的基础号码
            foreach (PossibleWin win in list)
            {
                KillNumFor7XingCai(win);
            }

            //GetChaNum();


            //第二轮 --------------随机一组号码出来 看是否满足剩余条件
            string num = GetSuiJiNumFor7XingCaiFangShui();



            return Hand6HeCaiNum(num);
        }

        #endregion

        //------------------------------------End快速相关----------------------------------


        //中间方法

        #region 10SC

        //从 wan2中随机一组号码出来
        public static string GetSuiJiNumFor10Sc()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0;

            int len = wan2.Count;
            if (len != 0)
            {
                a = wan2[r.Next(0, len)];
            }
            else
            {
                a = r.Next(0, 10);
            }

            len = qian2.Count;
            if (len != 0)
            {
                b = qian2[r.Next(0, len)];
            }
            else
            {
                b = r.Next(0, 10);
            }

            len = bai2.Count;
            if (len != 0)
            {
                c = bai2[r.Next(0, len)];
            }
            else
            {
                c = r.Next(0, 10);
            }

            len = shi2.Count;
            if (len != 0)
            {
                d = shi2[r.Next(0, len)];
            }
            else
            {
                d = r.Next(0, 10);
            }

            len = ge2.Count;
            if (len != 0)
            {
                e = ge2[r.Next(0, len)];
            }
            else
            {
                e = r.Next(0, 10);
            }

            return a + "," + b + "," + c + "," + d + "," + e;
        }

        public static string GetSuiJiNumFor10ScFangShui()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0;

            int len = wan.Count;
            if (len != 0)
            {
                a = wan[r.Next(0, len)];
            }
            else
            {
                a = r.Next(0, 10);
            }

            len = qian.Count;
            if (len != 0)
            {
                b = qian[r.Next(0, len)];
            }
            else
            {
                b = r.Next(0, 10);
            }

            len = bai.Count;
            if (len != 0)
            {
                c = bai[r.Next(0, len)];
            }
            else
            {
                c = r.Next(0, 10);
            }

            len = shi.Count;
            if (len != 0)
            {
                d = shi[r.Next(0, len)];
            }
            else
            {
                d = r.Next(0, 10);
            }

            len = ge.Count;
            if (len != 0)
            {
                e = ge[r.Next(0, len)];
            }
            else
            {
                e = r.Next(0, 10);
            }

            return a + "," + b + "," + c + "," + d + "," + e;
        }

        //给wan qian求差 差值放在wan2 中
        public static void GetChaNumFor10Sc()
        {
            //万位
            for (int i = 0; i < 10; i++)
            {
                if (!wan.Contains(i)) wan2.Add(i);
                if (!qian.Contains(i)) qian2.Add(i);
                if (!bai.Contains(i)) bai2.Add(i);
                if (!shi.Contains(i)) shi2.Add(i);
                if (!ge.Contains(i)) ge2.Add(i);
            }
        }

        //杀掉普通号码
        public static void KillNumFor10Sc(PossibleWin win)
        {
            string playName = win.PlayName;
            string betNum = win.BetNum;

            if (playName == "第一球")
            {
                if (betNum == "大")
                {
                    if (!wan.Contains(5)) wan.Add(5);
                    if (!wan.Contains(6)) wan.Add(6);
                    if (!wan.Contains(7)) wan.Add(7);
                    if (!wan.Contains(8)) wan.Add(8);
                    if (!wan.Contains(9)) wan.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!wan.Contains(0)) wan.Add(0);
                    if (!wan.Contains(1)) wan.Add(1);
                    if (!wan.Contains(2)) wan.Add(2);
                    if (!wan.Contains(3)) wan.Add(3);
                    if (!wan.Contains(4)) wan.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!wan.Contains(1)) wan.Add(1);
                    if (!wan.Contains(3)) wan.Add(3);
                    if (!wan.Contains(5)) wan.Add(5);
                    if (!wan.Contains(7)) wan.Add(7);
                    if (!wan.Contains(9)) wan.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!wan.Contains(0)) wan.Add(0);
                    if (!wan.Contains(2)) wan.Add(2);
                    if (!wan.Contains(4)) wan.Add(4);
                    if (!wan.Contains(6)) wan.Add(6);
                    if (!wan.Contains(8)) wan.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!wan.Contains(0)) wan.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!wan.Contains(1)) wan.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!wan.Contains(2)) wan.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!wan.Contains(3)) wan.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!wan.Contains(4)) wan.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!wan.Contains(5)) wan.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!wan.Contains(6)) wan.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!wan.Contains(7)) wan.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!wan.Contains(8)) wan.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!wan.Contains(9)) wan.Add(9);
                }
            }
            else if (playName == "第二球")
            {
                if (betNum == "大")
                {
                    if (!qian.Contains(5)) qian.Add(5);
                    if (!qian.Contains(6)) qian.Add(6);
                    if (!qian.Contains(7)) qian.Add(7);
                    if (!qian.Contains(8)) qian.Add(8);
                    if (!qian.Contains(9)) qian.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!qian.Contains(0)) qian.Add(0);
                    if (!qian.Contains(1)) qian.Add(1);
                    if (!qian.Contains(2)) qian.Add(2);
                    if (!qian.Contains(3)) qian.Add(3);
                    if (!qian.Contains(4)) qian.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!qian.Contains(1)) qian.Add(1);
                    if (!qian.Contains(3)) qian.Add(3);
                    if (!qian.Contains(5)) qian.Add(5);
                    if (!qian.Contains(7)) qian.Add(7);
                    if (!qian.Contains(9)) qian.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!qian.Contains(0)) qian.Add(0);
                    if (!qian.Contains(2)) qian.Add(2);
                    if (!qian.Contains(4)) qian.Add(4);
                    if (!qian.Contains(6)) qian.Add(6);
                    if (!qian.Contains(8)) qian.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!qian.Contains(0)) qian.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!qian.Contains(1)) qian.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!qian.Contains(2)) qian.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!qian.Contains(3)) qian.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!qian.Contains(4)) qian.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!qian.Contains(5)) qian.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!qian.Contains(6)) qian.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!qian.Contains(7)) qian.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!qian.Contains(8)) qian.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!qian.Contains(9)) qian.Add(9);
                }
            }
            else if (playName == "第三球")
            {
                if (betNum == "大")
                {
                    if (!bai.Contains(5)) bai.Add(5);
                    if (!bai.Contains(6)) bai.Add(6);
                    if (!bai.Contains(7)) bai.Add(7);
                    if (!bai.Contains(8)) bai.Add(8);
                    if (!bai.Contains(9)) bai.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!bai.Contains(0)) bai.Add(0);
                    if (!bai.Contains(1)) bai.Add(1);
                    if (!bai.Contains(2)) bai.Add(2);
                    if (!bai.Contains(3)) bai.Add(3);
                    if (!bai.Contains(4)) bai.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!bai.Contains(1)) bai.Add(1);
                    if (!bai.Contains(3)) bai.Add(3);
                    if (!bai.Contains(5)) bai.Add(5);
                    if (!bai.Contains(7)) bai.Add(7);
                    if (!bai.Contains(9)) bai.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!bai.Contains(0)) bai.Add(0);
                    if (!bai.Contains(2)) bai.Add(2);
                    if (!bai.Contains(4)) bai.Add(4);
                    if (!bai.Contains(6)) bai.Add(6);
                    if (!bai.Contains(8)) bai.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!bai.Contains(0)) bai.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!bai.Contains(1)) bai.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!bai.Contains(2)) bai.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!bai.Contains(3)) bai.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!bai.Contains(4)) bai.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!bai.Contains(5)) bai.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!bai.Contains(6)) bai.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!bai.Contains(7)) bai.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!bai.Contains(8)) bai.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!bai.Contains(9)) bai.Add(9);
                }
            }
            else if (playName == "第四球")
            {
                if (betNum == "大")
                {
                    if (!shi.Contains(5)) shi.Add(5);
                    if (!shi.Contains(6)) shi.Add(6);
                    if (!shi.Contains(7)) shi.Add(7);
                    if (!shi.Contains(8)) shi.Add(8);
                    if (!shi.Contains(9)) shi.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!shi.Contains(0)) shi.Add(0);
                    if (!shi.Contains(1)) shi.Add(1);
                    if (!shi.Contains(2)) shi.Add(2);
                    if (!shi.Contains(3)) shi.Add(3);
                    if (!shi.Contains(4)) shi.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!shi.Contains(1)) shi.Add(1);
                    if (!shi.Contains(3)) shi.Add(3);
                    if (!shi.Contains(5)) shi.Add(5);
                    if (!shi.Contains(7)) shi.Add(7);
                    if (!shi.Contains(9)) shi.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!shi.Contains(0)) shi.Add(0);
                    if (!shi.Contains(2)) shi.Add(2);
                    if (!shi.Contains(4)) shi.Add(4);
                    if (!shi.Contains(6)) shi.Add(6);
                    if (!shi.Contains(8)) shi.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!shi.Contains(0)) shi.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!shi.Contains(1)) shi.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!shi.Contains(2)) shi.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!shi.Contains(3)) shi.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!shi.Contains(4)) shi.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!shi.Contains(5)) shi.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!shi.Contains(6)) shi.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!shi.Contains(7)) shi.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!shi.Contains(8)) shi.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!shi.Contains(9)) shi.Add(9);
                }
            }
            else if (playName == "第五球")
            {
                if (betNum == "大")
                {
                    if (!ge.Contains(5)) ge.Add(5);
                    if (!ge.Contains(6)) ge.Add(6);
                    if (!ge.Contains(7)) ge.Add(7);
                    if (!ge.Contains(8)) ge.Add(8);
                    if (!ge.Contains(9)) ge.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!ge.Contains(0)) ge.Add(0);
                    if (!ge.Contains(1)) ge.Add(1);
                    if (!ge.Contains(2)) ge.Add(2);
                    if (!ge.Contains(3)) ge.Add(3);
                    if (!ge.Contains(4)) ge.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!ge.Contains(1)) ge.Add(1);
                    if (!ge.Contains(3)) ge.Add(3);
                    if (!ge.Contains(5)) ge.Add(5);
                    if (!ge.Contains(7)) ge.Add(7);
                    if (!ge.Contains(9)) ge.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!ge.Contains(0)) ge.Add(0);
                    if (!ge.Contains(2)) ge.Add(2);
                    if (!ge.Contains(4)) ge.Add(4);
                    if (!ge.Contains(6)) ge.Add(6);
                    if (!ge.Contains(8)) ge.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!ge.Contains(0)) ge.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!ge.Contains(1)) ge.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!ge.Contains(2)) ge.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!ge.Contains(3)) ge.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!ge.Contains(4)) ge.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!ge.Contains(5)) ge.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!ge.Contains(6)) ge.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!ge.Contains(7)) ge.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!ge.Contains(8)) ge.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!ge.Contains(9)) ge.Add(9);
                }
            }

        }


        //检查 随机号码 能不能满足第二组要求
        public static bool CheckSecondFor10Sc(string num, List<PossibleWin> list)
        {
            string playName = "";
            string betNum = "";

            string[] arr = num.Split(',');
            int a = int.Parse(arr[0]);
            int b = int.Parse(arr[1]);
            int c = int.Parse(arr[2]);
            int d = int.Parse(arr[3]);
            int e = int.Parse(arr[4]);

            int sum = a + b + c + d + e;

            foreach (PossibleWin win in list)
            {
                playName = win.PlayName;
                betNum = win.BetNum;

                if (playName == "" && betNum == "总和大")
                {
                    if (sum >= 23) return false;
                }
                else if (playName == "" && betNum == "总和小")
                {
                    if (sum < 23) return false;
                }
                else if (playName == "" && betNum == "总和单")
                {
                    if (sum % 2 != 0) return false;
                }
                else if (playName == "" && betNum == "总和双")
                {
                    if (sum % 2 == 0) return false;
                }
                else if (playName == "" && betNum == "龙")
                {
                    if (a > e) return false;
                }
                else if (playName == "" && betNum == "虎")
                {
                    if (a < e) return false;
                }
                else if (playName == "" && betNum == "和")
                {
                    if (a == e) return false;
                }
                else if (playName == "前三" && betNum == "豹子")
                {
                    if (Util.IsBaoZi(a, b, c)) return false;
                }
                else if (playName == "前三" && betNum == "顺子")
                {
                    if (Util.IsShunZi(a, b, c)) return false;
                }
                else if (playName == "前三" && betNum == "对子")
                {
                    if (Util.IsDuiZi(a, b, c)) return false;
                }
                else if (playName == "前三" && betNum == "半顺")
                {
                    if (Util.IsBanShun(a, b, c)) return false;
                }
                else if (playName == "前三" && betNum == "杂六")
                {
                    if (Util.IsZaLiu(a, b, c)) return false;
                }
                else if (playName == "中三" && betNum == "豹子")
                {
                    if (Util.IsBaoZi(b, c, d)) return false;
                }
                else if (playName == "中三" && betNum == "顺子")
                {
                    if (Util.IsShunZi(b, c, d)) return false;
                }
                else if (playName == "中三" && betNum == "对子")
                {
                    if (Util.IsDuiZi(b, c, d)) return false;
                }
                else if (playName == "中三" && betNum == "半顺")
                {
                    if (Util.IsBanShun(b, c, d)) return false;
                }
                else if (playName == "中三" && betNum == "杂六")
                {
                    if (Util.IsZaLiu(b, c, d)) return false;
                }
                else if (playName == "后三" && betNum == "豹子")
                {
                    if (Util.IsBaoZi(c, d, e)) return false;
                }
                else if (playName == "后三" && betNum == "顺子")
                {
                    if (Util.IsShunZi(c, d, e)) return false;
                }
                else if (playName == "后三" && betNum == "对子")
                {
                    if (Util.IsDuiZi(c, d, e)) return false;
                }
                else if (playName == "后三" && betNum == "半顺")
                {
                    if (Util.IsBanShun(c, d, e)) return false;
                }
                else if (playName == "后三" && betNum == "杂六")
                {
                    if (Util.IsZaLiu(c, d, e)) return false;
                }
            }

            return true;
        }

        #endregion

        #region 3D

        //从 wanFor3D2中随机一组号码出来
        public static string GetSuiJiNumFor3D()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0;

            int len = wanFor3D2.Count;
            if (len != 0)
            {
                a = wanFor3D2[r.Next(0, len)];
            }
            else
            {
                a = r.Next(0, 10);
            }

            len = qianFor3D2.Count;
            if (len != 0)
            {
                b = qianFor3D2[r.Next(0, len)];
            }
            else
            {
                b = r.Next(0, 10);
            }

            len = baiFor3D2.Count;
            if (len != 0)
            {
                c = baiFor3D2[r.Next(0, len)];
            }
            else
            {
                c = r.Next(0, 10);
            }

            return a + "," + b + "," + c;
        }

        public static string GetSuiJiNumFor3DFangShui()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0;

            int len = wanFor3D.Count;
            if (len != 0)
            {
                a = wanFor3D[r.Next(0, len)];
            }
            else
            {
                a = r.Next(0, 10);
            }

            len = qianFor3D.Count;
            if (len != 0)
            {
                b = qianFor3D[r.Next(0, len)];
            }
            else
            {
                b = r.Next(0, 10);
            }

            len = baiFor3D.Count;
            if (len != 0)
            {
                c = baiFor3D[r.Next(0, len)];
            }
            else
            {
                c = r.Next(0, 10);
            }

            return a + "," + b + "," + c;
        }

        //给wanFor3D qianFor3D求差 差值放在wanFor3D2 中
        public static void GetChaNumFor3D()
        {
            //万位
            for (int i = 0; i < 10; i++)
            {
                if (!wanFor3D.Contains(i)) wanFor3D2.Add(i);
                if (!qianFor3D.Contains(i)) qianFor3D2.Add(i);
                if (!baiFor3D.Contains(i)) baiFor3D2.Add(i);
            }
        }

        //杀掉普通号码
        public static void KillNumFor3D(PossibleWin win)
        {
            string playName = win.PlayName;
            string betNum = win.BetNum;

            if (playName == "第一球")
            {
                if (betNum == "大")
                {
                    if (!wanFor3D.Contains(5)) wanFor3D.Add(5);
                    if (!wanFor3D.Contains(6)) wanFor3D.Add(6);
                    if (!wanFor3D.Contains(7)) wanFor3D.Add(7);
                    if (!wanFor3D.Contains(8)) wanFor3D.Add(8);
                    if (!wanFor3D.Contains(9)) wanFor3D.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!wanFor3D.Contains(0)) wanFor3D.Add(0);
                    if (!wanFor3D.Contains(1)) wanFor3D.Add(1);
                    if (!wanFor3D.Contains(2)) wanFor3D.Add(2);
                    if (!wanFor3D.Contains(3)) wanFor3D.Add(3);
                    if (!wanFor3D.Contains(4)) wanFor3D.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!wanFor3D.Contains(1)) wanFor3D.Add(1);
                    if (!wanFor3D.Contains(3)) wanFor3D.Add(3);
                    if (!wanFor3D.Contains(5)) wanFor3D.Add(5);
                    if (!wanFor3D.Contains(7)) wanFor3D.Add(7);
                    if (!wanFor3D.Contains(9)) wanFor3D.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!wanFor3D.Contains(0)) wanFor3D.Add(0);
                    if (!wanFor3D.Contains(2)) wanFor3D.Add(2);
                    if (!wanFor3D.Contains(4)) wanFor3D.Add(4);
                    if (!wanFor3D.Contains(6)) wanFor3D.Add(6);
                    if (!wanFor3D.Contains(8)) wanFor3D.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!wanFor3D.Contains(0)) wanFor3D.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!wanFor3D.Contains(1)) wanFor3D.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!wanFor3D.Contains(2)) wanFor3D.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!wanFor3D.Contains(3)) wanFor3D.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!wanFor3D.Contains(4)) wanFor3D.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!wanFor3D.Contains(5)) wanFor3D.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!wanFor3D.Contains(6)) wanFor3D.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!wanFor3D.Contains(7)) wanFor3D.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!wanFor3D.Contains(8)) wanFor3D.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!wanFor3D.Contains(9)) wanFor3D.Add(9);
                }
            }
            else if (playName == "第二球")
            {
                if (betNum == "大")
                {
                    if (!qianFor3D.Contains(5)) qianFor3D.Add(5);
                    if (!qianFor3D.Contains(6)) qianFor3D.Add(6);
                    if (!qianFor3D.Contains(7)) qianFor3D.Add(7);
                    if (!qianFor3D.Contains(8)) qianFor3D.Add(8);
                    if (!qianFor3D.Contains(9)) qianFor3D.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!qianFor3D.Contains(0)) qianFor3D.Add(0);
                    if (!qianFor3D.Contains(1)) qianFor3D.Add(1);
                    if (!qianFor3D.Contains(2)) qianFor3D.Add(2);
                    if (!qianFor3D.Contains(3)) qianFor3D.Add(3);
                    if (!qianFor3D.Contains(4)) qianFor3D.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!qianFor3D.Contains(1)) qianFor3D.Add(1);
                    if (!qianFor3D.Contains(3)) qianFor3D.Add(3);
                    if (!qianFor3D.Contains(5)) qianFor3D.Add(5);
                    if (!qianFor3D.Contains(7)) qianFor3D.Add(7);
                    if (!qianFor3D.Contains(9)) qianFor3D.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!qianFor3D.Contains(0)) qianFor3D.Add(0);
                    if (!qianFor3D.Contains(2)) qianFor3D.Add(2);
                    if (!qianFor3D.Contains(4)) qianFor3D.Add(4);
                    if (!qianFor3D.Contains(6)) qianFor3D.Add(6);
                    if (!qianFor3D.Contains(8)) qianFor3D.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!qianFor3D.Contains(0)) qianFor3D.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!qianFor3D.Contains(1)) qianFor3D.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!qianFor3D.Contains(2)) qianFor3D.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!qianFor3D.Contains(3)) qianFor3D.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!qianFor3D.Contains(4)) qianFor3D.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!qianFor3D.Contains(5)) qianFor3D.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!qianFor3D.Contains(6)) qianFor3D.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!qianFor3D.Contains(7)) qianFor3D.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!qianFor3D.Contains(8)) qianFor3D.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!qianFor3D.Contains(9)) qianFor3D.Add(9);
                }
            }
            else if (playName == "第三球")
            {
                if (betNum == "大")
                {
                    if (!baiFor3D.Contains(5)) baiFor3D.Add(5);
                    if (!baiFor3D.Contains(6)) baiFor3D.Add(6);
                    if (!baiFor3D.Contains(7)) baiFor3D.Add(7);
                    if (!baiFor3D.Contains(8)) baiFor3D.Add(8);
                    if (!baiFor3D.Contains(9)) baiFor3D.Add(9);
                }
                else if (betNum == "小")
                {
                    if (!baiFor3D.Contains(0)) baiFor3D.Add(0);
                    if (!baiFor3D.Contains(1)) baiFor3D.Add(1);
                    if (!baiFor3D.Contains(2)) baiFor3D.Add(2);
                    if (!baiFor3D.Contains(3)) baiFor3D.Add(3);
                    if (!baiFor3D.Contains(4)) baiFor3D.Add(4);
                }
                else if (betNum == "单")
                {
                    if (!baiFor3D.Contains(1)) baiFor3D.Add(1);
                    if (!baiFor3D.Contains(3)) baiFor3D.Add(3);
                    if (!baiFor3D.Contains(5)) baiFor3D.Add(5);
                    if (!baiFor3D.Contains(7)) baiFor3D.Add(7);
                    if (!baiFor3D.Contains(9)) baiFor3D.Add(9);
                }
                else if (betNum == "双")
                {
                    if (!baiFor3D.Contains(0)) baiFor3D.Add(0);
                    if (!baiFor3D.Contains(2)) baiFor3D.Add(2);
                    if (!baiFor3D.Contains(4)) baiFor3D.Add(4);
                    if (!baiFor3D.Contains(6)) baiFor3D.Add(6);
                    if (!baiFor3D.Contains(8)) baiFor3D.Add(8);
                }
                else if (betNum == "0")
                {
                    if (!baiFor3D.Contains(0)) baiFor3D.Add(0);
                }
                else if (betNum == "1")
                {
                    if (!baiFor3D.Contains(1)) baiFor3D.Add(1);
                }
                else if (betNum == "2")
                {
                    if (!baiFor3D.Contains(2)) baiFor3D.Add(2);
                }
                else if (betNum == "3")
                {
                    if (!baiFor3D.Contains(3)) baiFor3D.Add(3);
                }
                else if (betNum == "4")
                {
                    if (!baiFor3D.Contains(4)) baiFor3D.Add(4);
                }
                else if (betNum == "5")
                {
                    if (!baiFor3D.Contains(5)) baiFor3D.Add(5);
                }
                else if (betNum == "6")
                {
                    if (!baiFor3D.Contains(6)) baiFor3D.Add(6);
                }
                else if (betNum == "7")
                {
                    if (!baiFor3D.Contains(7)) baiFor3D.Add(7);
                }
                else if (betNum == "8")
                {
                    if (!baiFor3D.Contains(8)) baiFor3D.Add(8);
                }
                else if (betNum == "9")
                {
                    if (!baiFor3D.Contains(9)) baiFor3D.Add(9);
                }
            }

        }

        //检查 随机号码 能不能满足第二组要求
        public static bool CheckSecondFor3D(string num, List<PossibleWin> list)
        {
            string playName = "";
            string betNum = "";

            string[] arr = num.Split(',');
            int a = int.Parse(arr[0]);
            int b = int.Parse(arr[1]);
            int c = int.Parse(arr[2]);


            int sum = a + b + c;

            foreach (PossibleWin win in list)
            {
                playName = win.PlayName;
                betNum = win.BetNum;

                if (playName == "" && betNum == "总和大")
                {
                    if (sum >= 13) return false;
                }
                else if (playName == "" && betNum == "总和小")
                {
                    if (sum < 13) return false;
                }
                else if (playName == "" && betNum == "总和单")
                {
                    if (sum % 2 != 0) return false;
                }
                else if (playName == "" && betNum == "总和双")
                {
                    if (sum % 2 == 0) return false;
                }
                else if (playName == "和值")
                {
                    if (int.Parse(betNum) == sum) return false;
                }
                else if (playName == "" && betNum == "龙")
                {
                    if (a > c) return false;
                }
                else if (playName == "" && betNum == "虎")
                {
                    if (a < c) return false;
                }
                else if (playName == "" && betNum == "和")
                {
                    if (a == c) return false;
                }
                else if (playName == "豹顺对" && betNum == "豹子")
                {
                    if (Util.IsBaoZi(a, b, c)) return false;
                }
                else if (playName == "豹顺对" && betNum == "顺子")
                {
                    if (Util.IsShunZi(a, b, c)) return false;
                }
                else if (playName == "豹顺对" && betNum == "对子")
                {
                    if (Util.IsDuiZi(a, b, c)) return false;
                }
                else if (playName == "豹顺对" && betNum == "半顺")
                {
                    if (Util.IsBanShun(a, b, c)) return false;
                }
                else if (playName == "豹顺对" && betNum == "杂六")
                {
                    if (Util.IsZaLiu(a, b, c)) return false;
                }

            }

            return true;
        }

        #endregion

        #region PK10

        public static string HandPk10Num(string num)
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            string[] arr = num.Split(',');

            string result = "";

            foreach (string s in arr)
            {
                if (!result.Contains(s))
                {
                    result += s + ",";
                }
                else
                {
                    result += ran.Next(1, 50) + ",";
                }
            }

            return result.TrimEnd(',');
        }

        //从 wanForPK102中随机一组号码出来
        public static string GetSuiJiNumForPk10()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0, i = 0, j = 0;

            int len = wanForPK102.Count;
            if (len != 0)
            {
                a = wanForPK102[r.Next(0, len)];
            }
            else
            {
                a = r.Next(1, 11);
            }

            len = qianForPK102.Count;
            if (len != 0)
            {
                b = qianForPK102[r.Next(0, len)];
            }
            else
            {
                b = r.Next(1, 11);
            }

            len = baiForPK102.Count;
            if (len != 0)
            {
                c = baiForPK102[r.Next(0, len)];
            }
            else
            {
                c = r.Next(1, 11);
            }

            len = shiForPK102.Count;
            if (len != 0)
            {
                d = shiForPK102[r.Next(0, len)];
            }
            else
            {
                d = r.Next(1, 11);
            }


            len = geForPK102.Count;
            if (len != 0)
            {
                e = geForPK102[r.Next(0, len)];
            }
            else
            {
                e = r.Next(1, 11);
            }

            len = sixForPK102.Count;
            if (len != 0)
            {
                f = sixForPK102[r.Next(0, len)];
            }
            else
            {
                f = r.Next(1, 11);
            }

            len = sevenForPK102.Count;
            if (len != 0)
            {
                g = sevenForPK102[r.Next(0, len)];
            }
            else
            {
                g = r.Next(1, 11);
            }


            len = eightForPK102.Count;
            if (len != 0)
            {
                h = eightForPK102[r.Next(0, len)];
            }
            else
            {
                h = r.Next(1, 11);
            }

            len = nineForPK102.Count;
            if (len != 0)
            {
                i = nineForPK102[r.Next(0, len)];
            }
            else
            {
                i = r.Next(1, 11);
            }


            len = tenForPK102.Count;
            if (len != 0)
            {
                j = tenForPK102[r.Next(0, len)];
            }
            else
            {
                j = r.Next(1, 11);
            }

            return a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g + "," + h + "," + i + "," + j;
        }

        public static string GetSuiJiNumForPk10FangShui()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0, i = 0, j = 0;

            int len = wanForPK102.Count;
            if (len != 0)
            {
                a = wanForPK102[r.Next(0, len)];
            }
            else
            {
                a = r.Next(1, 11);
            }

            len = qianForPK102.Count;
            if (len != 0)
            {
                b = qianForPK102[r.Next(0, len)];
            }
            else
            {
                b = r.Next(1, 11);
            }

            len = baiForPK102.Count;
            if (len != 0)
            {
                c = baiForPK102[r.Next(0, len)];
            }
            else
            {
                c = r.Next(1, 11);
            }

            len = shiForPK102.Count;
            if (len != 0)
            {
                d = shiForPK102[r.Next(0, len)];
            }
            else
            {
                d = r.Next(1, 11);
            }


            len = geForPK102.Count;
            if (len != 0)
            {
                e = geForPK102[r.Next(0, len)];
            }
            else
            {
                e = r.Next(1, 11);
            }

            len = sixForPK102.Count;
            if (len != 0)
            {
                f = sixForPK102[r.Next(0, len)];
            }
            else
            {
                f = r.Next(1, 11);
            }

            len = sevenForPK102.Count;
            if (len != 0)
            {
                g = sevenForPK102[r.Next(0, len)];
            }
            else
            {
                g = r.Next(1, 11);
            }


            len = eightForPK102.Count;
            if (len != 0)
            {
                h = eightForPK102[r.Next(0, len)];
            }
            else
            {
                h = r.Next(1, 11);
            }

            len = nineForPK10.Count;
            if (len != 0)
            {
                i = nineForPK10[r.Next(0, len)];
            }
            else
            {
                i = r.Next(1, 11);
            }


            len = tenForPK102.Count;
            if (len != 0)
            {
                j = tenForPK102[r.Next(0, len)];
            }
            else
            {
                j = r.Next(1, 11);
            }

            return a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g + "," + h + "," + i + "," + j;
        }

        //给wanForPK10 qianForPK10求差 差值放在wanForPK102 中
        public static void GetChaNumForPk10()
        {
            //万位
            for (int i = 1; i < 11; i++)
            {
                if (!wanForPK10.Contains(i)) wanForPK102.Add(i);
                if (!qianForPK10.Contains(i)) qianForPK102.Add(i);
                if (!baiForPK10.Contains(i)) baiForPK102.Add(i);
                if (!shiForPK10.Contains(i)) shiForPK102.Add(i);
                if (!geForPK10.Contains(i)) geForPK102.Add(i);
                if (!sixForPK10.Contains(i)) sixForPK102.Add(i);
                if (!sevenForPK10.Contains(i)) sevenForPK102.Add(i);
                if (!eightForPK10.Contains(i)) eightForPK102.Add(i);
                if (!nineForPK10.Contains(i)) nineForPK102.Add(i);
                if (!tenForPK10.Contains(i)) tenForPK102.Add(i);
            }
        }

        //杀掉普通号码
        public static void KillNumForPk10(PossibleWin win)
        {
            string playName = win.PlayName;
            string betNum = win.BetNum;

            //if (playName == "冠军")
            //{
            if (betNum == "大")
            {
                if (!wanForPK10.Contains(6)) wanForPK10.Add(6);
                if (!wanForPK10.Contains(7)) wanForPK10.Add(7);
                if (!wanForPK10.Contains(8)) wanForPK10.Add(8);
                if (!wanForPK10.Contains(9)) wanForPK10.Add(9);
                if (!wanForPK10.Contains(10)) wanForPK10.Add(10);
            }
            else if (betNum == "小")
            {
                if (!wanForPK10.Contains(1)) wanForPK10.Add(1);
                if (!wanForPK10.Contains(2)) wanForPK10.Add(2);
                if (!wanForPK10.Contains(3)) wanForPK10.Add(3);
                if (!wanForPK10.Contains(4)) wanForPK10.Add(4);
                if (!wanForPK10.Contains(5)) wanForPK10.Add(5);
            }
            else if (betNum == "单")
            {
                if (!wanForPK10.Contains(1)) wanForPK10.Add(1);
                if (!wanForPK10.Contains(3)) wanForPK10.Add(3);
                if (!wanForPK10.Contains(5)) wanForPK10.Add(5);
                if (!wanForPK10.Contains(7)) wanForPK10.Add(7);
                if (!wanForPK10.Contains(9)) wanForPK10.Add(9);
            }
            else if (betNum == "双")
            {
                if (!wanForPK10.Contains(2)) wanForPK10.Add(2);
                if (!wanForPK10.Contains(4)) wanForPK10.Add(4);
                if (!wanForPK10.Contains(6)) wanForPK10.Add(6);
                if (!wanForPK10.Contains(8)) wanForPK10.Add(8);
                if (!wanForPK10.Contains(10)) wanForPK10.Add(10);
            }
            else if (betNum == "1")
            {
                if (!wanForPK10.Contains(1)) wanForPK10.Add(1);
            }
            else if (betNum == "2")
            {
                if (!wanForPK10.Contains(2)) wanForPK10.Add(2);
            }
            else if (betNum == "3")
            {
                if (!wanForPK10.Contains(3)) wanForPK10.Add(3);
            }
            else if (betNum == "4")
            {
                if (!wanForPK10.Contains(4)) wanForPK10.Add(4);
            }
            else if (betNum == "5")
            {
                if (!wanForPK10.Contains(5)) wanForPK10.Add(5);
            }
            else if (betNum == "6")
            {
                if (!wanForPK10.Contains(6)) wanForPK10.Add(6);
            }
            else if (betNum == "7")
            {
                if (!wanForPK10.Contains(7)) wanForPK10.Add(7);
            }
            else if (betNum == "8")
            {
                if (!wanForPK10.Contains(8)) wanForPK10.Add(8);
            }
            else if (betNum == "9")
            {
                if (!wanForPK10.Contains(9)) wanForPK10.Add(9);
            }
            else if (betNum == "10")
            {
                if (!wanForPK10.Contains(9)) wanForPK10.Add(10);
            }
            //}




        }

        //检查 随机号码 能不能满足第二组要求
        public static bool CheckSecondForPk10(string num, List<PossibleWin> list)
        {
            string playName = "";
            string betNum = "";

            string[] arr = num.Split(',');
            int a = int.Parse(arr[0]);
            int b = int.Parse(arr[1]);


            int sum = a + b;

            foreach (PossibleWin win in list)
            {
                playName = win.PlayName;
                betNum = win.BetNum;

                if (playName == "" && betNum == "冠亚大")
                {
                    if (sum >= 10) return false;
                }
                else if (playName == "" && betNum == "冠亚小")
                {
                    if (sum < 10) return false;
                }
                else if (playName == "" && betNum == "冠亚单")
                {
                    if (sum % 2 != 0) return false;
                }
                else if (playName == "" && betNum == "冠亚双")
                {
                    if (sum % 2 == 0) return false;
                }
                else if (playName == "冠亚和" && int.Parse(betNum) == sum)
                {
                    return false;
                }

            }

            return true;
        }

        //检查重复
        public static string RemoveRepeatForPk10(string num)
        {
            string[] numArr = num.Split(',');

            for (int i = 9; i >= 0; i--)
            {
                string temp = numArr[i];

                int index1 = Array.IndexOf(numArr, temp);
                int index2 = Array.LastIndexOf(numArr, temp);

                if (index1 != index2)
                {
                    numArr[i] = GetSYNumForPk10(numArr, i);
                }
            }

            return string.Join(",", numArr);
        }


        public static string GetSYNumForPk10(string[] numArr, int index)
        {
            string[] tempArr = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            List<string> list = tempArr.ToList();

            for (int i = 9; i >= 0; i--)
            {
                string s = numArr[i];
                if (i != index && list.Contains(s))
                {
                    list.Remove(s);
                }
            }

            return list[0];
        }




        #endregion

        #region 6HeCai

        public static string Hand6HeCaiNum(string num)
        {
            string[] arr = num.Split(',');
            string result = "";
            foreach (string s in arr)
            {
                int n = int.Parse(s);
                if (n < 10)
                {
                    result += "0" + n + ",";
                }
                else
                {
                    result += n + ",";
                }
            }
            return result.TrimEnd(',');
        }

        //从 wanFor6HeCai2中随机一组号码出来
        public static string GetSuiJiNumFor6HeCai()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0;

            int len = wanFor6HeCai2.Count;
            if (len != 0)
            {
                a = wanFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                a = r.Next(1, 50);
            }

            len = qianFor6HeCai2.Count;
            if (len != 0)
            {
                b = qianFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                b = r.Next(1, 50);
            }

            len = baiFor6HeCai2.Count;
            if (len != 0)
            {
                c = baiFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                c = r.Next(1, 50);
            }

            len = shiFor6HeCai2.Count;
            if (len != 0)
            {
                d = shiFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                d = r.Next(1, 50);
            }


            len = geFor6HeCai2.Count;
            if (len != 0)
            {
                e = geFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                e = r.Next(1, 50);
            }

            len = sixFor6HeCai2.Count;
            if (len != 0)
            {
                f = sixFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                f = r.Next(1, 50);
            }

            len = sevenFor6HeCai2.Count;
            if (len != 0)
            {
                g = sevenFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                g = r.Next(1, 50);
            }

            return a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g;
        }

        public static string GetSuiJiNumFor6HeCaiFangShui()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0;

            int len = wanFor6HeCai2.Count;
            if (len != 0)
            {
                a = wanFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                a = r.Next(1, 50);
            }

            len = qianFor6HeCai2.Count;
            if (len != 0)
            {
                b = qianFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                b = r.Next(1, 50);
            }

            len = baiFor6HeCai2.Count;
            if (len != 0)
            {
                c = baiFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                c = r.Next(1, 50);
            }

            len = shiFor6HeCai2.Count;
            if (len != 0)
            {
                d = shiFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                d = r.Next(1, 50);
            }


            len = geFor6HeCai2.Count;
            if (len != 0)
            {
                e = geFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                e = r.Next(1, 50);
            }

            len = sixFor6HeCai2.Count;
            if (len != 0)
            {
                f = sixFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                f = r.Next(1, 50);
            }

            len = sevenFor6HeCai2.Count;
            if (len != 0)
            {
                g = sevenFor6HeCai2[r.Next(0, len)];
            }
            else
            {
                g = r.Next(1, 50);
            }



            return a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g;
        }

        //给wanFor6HeCai qianFor6HeCai求差 差值放在wanFor6HeCai2 中
        public static void GetChaNumFor6HeCai()
        {
            //万位
            for (int i = 1; i < 50; i++)
            {
                if (!wanFor6HeCai.Contains(i)) wanFor6HeCai2.Add(i);
                if (!qianFor6HeCai.Contains(i)) qianFor6HeCai2.Add(i);
                if (!baiFor6HeCai.Contains(i)) baiFor6HeCai2.Add(i);
                if (!shiFor6HeCai.Contains(i)) shiFor6HeCai2.Add(i);
                if (!geFor6HeCai.Contains(i)) geFor6HeCai2.Add(i);
                if (!sixFor6HeCai.Contains(i)) sixFor6HeCai2.Add(i);
                if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai2.Add(i);
            }
        }

        //杀掉普通号码
        public static void KillNumFor6HeCai(PossibleWin win)
        {
            string playName = win.PlayName;
            string betNum = win.BetNum;

            if (playName.Contains("特码"))
            {
                #region 特码

                if (betNum == "1-10")
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                }
                else if (betNum == "11-20")
                {
                    for (int i = 11; i <= 20; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "21-30")
                {
                    for (int i = 21; i <= 30; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "31-40")
                {
                    for (int i = 31; i <= 40; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "41-49")
                {
                    for (int i = 41; i < 50; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!sevenFor6HeCai.Contains(i)) sevenFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "家禽")
                {
                    string str = "11,23,35,47,12,24,36,48,01,13,25,37,49,03,15,27,39,04,16,28,40,09,21,33,45";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "野兽")
                {
                    string str = "02,14,26,38,05,17,29,41,06,18,30,42,07,19,31,43,08,20,32,44,10,22,34,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "六肖连中")
            {
                #region 六肖连中

                string[] arr = betNum.Split(',');

                foreach (string s in arr)
                {
                    string num = Util.GetDigitByShengxiao(s);

                    string[] arr2 = num.Split(',');

                    foreach (string s2 in arr2)
                    {
                        int num2 = int.Parse(s2);
                        if (!sevenFor6HeCai.Contains(num2)) sevenFor6HeCai.Add(num2);
                    }
                }

                #endregion
            }
            else if (playName == "特肖")
            {
                #region 特肖

                if (betNum == "鼠")
                {
                    string str = "10,22,34,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "牛")
                {
                    string str = "09,21,33,45";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "虎")
                {
                    string str = "08,20,32,44";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "兔")
                {
                    string str = "07,19,31,43";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "龙")
                {
                    string str = "06,18,30,42";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蛇")
                {
                    string str = "05,17,29,41";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "马")
                {
                    string str = "04,16,28,40";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "羊")
                {
                    string str = "03,15,27,39";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "猴")
                {
                    string str = "02,14,26,38";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "鸡")
                {
                    string str = "01,13,25,37,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "狗")
                {
                    string str = "12,24,36,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "猪")
                {
                    string str = "11,23,35,47";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                    }
                }


                #endregion
            }
            else if (playName == "正码一" || playName == "正1特")
            {
                #region 正码一

                if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!wanFor6HeCai.Contains(i)) wanFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!wanFor6HeCai.Contains(num)) wanFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!wanFor6HeCai.Contains(num)) wanFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!wanFor6HeCai.Contains(num)) wanFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "正码二" || playName == "正2特")
            {
                #region 正码二

                if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!qianFor6HeCai.Contains(i)) qianFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!qianFor6HeCai.Contains(num)) qianFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!qianFor6HeCai.Contains(num)) qianFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!qianFor6HeCai.Contains(num)) qianFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "正码三" || playName == "正3特")
            {
                #region 正码三

                if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!baiFor6HeCai.Contains(i)) baiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!baiFor6HeCai.Contains(num)) baiFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!baiFor6HeCai.Contains(num)) baiFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!baiFor6HeCai.Contains(num)) baiFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "正码四" || playName == "正4特")
            {
                #region 正码四

                if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!shiFor6HeCai.Contains(i)) shiFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!shiFor6HeCai.Contains(num)) shiFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!shiFor6HeCai.Contains(num)) shiFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!shiFor6HeCai.Contains(num)) shiFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "正码五" || playName == "正5特")
            {
                #region 正码五

                if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!geFor6HeCai.Contains(i)) geFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!geFor6HeCai.Contains(num)) geFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!geFor6HeCai.Contains(num)) geFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!geFor6HeCai.Contains(num)) geFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "正码六" || playName == "正6特")
            {
                #region 正码六

                if (betNum == "单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "大")
                {
                    for (int i = 25; i < 50; i++)
                    {
                        if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "小")
                {
                    for (int i = 1; i < 25; i++)
                    {
                        if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                    }
                }
                else if (betNum == "合单")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 != 0)
                        {
                            if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "合双")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int he = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            he = int.Parse(str.Substring(0, 1)) + int.Parse(str.Substring(1, 1));
                        }


                        if (he % 2 == 0)
                        {
                            if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei >= 5)
                        {
                            if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "尾小")
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int wei = i;
                        if (i >= 10)
                        {
                            string str = i.ToString();
                            wei = int.Parse(str.Substring(1, 1));
                        }

                        if (wei < 5)
                        {
                            if (!sixFor6HeCai.Contains(i)) sixFor6HeCai.Add(i);
                        }
                    }
                }
                else if (betNum == "红波")
                {
                    string str = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sixFor6HeCai.Contains(num)) sixFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "蓝波")
                {
                    string str = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sixFor6HeCai.Contains(num)) sixFor6HeCai.Add(num);
                    }
                }
                else if (betNum == "绿波")
                {
                    string str = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                    string[] arr = str.Split(',');

                    foreach (string s in arr)
                    {
                        int num = int.Parse(s);
                        if (!sixFor6HeCai.Contains(num)) sixFor6HeCai.Add(num);
                    }
                }

                #endregion
            }
            else if (playName == "正1特")
            {
                #region 正1特

                if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!wanFor6HeCai.Contains(num)) wanFor6HeCai.Add(num);
                }

                #endregion
            }
            else if (playName == "正2特")
            {
                #region 正2特

                if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!qianFor6HeCai.Contains(num)) qianFor6HeCai.Add(num);
                }

                #endregion
            }
            else if (playName == "正3特")
            {
                #region 正3特

                if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!baiFor6HeCai.Contains(num)) baiFor6HeCai.Add(num);
                }

                #endregion
            }
            else if (playName == "正4特")
            {
                #region 正4特

                if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!shiFor6HeCai.Contains(num)) shiFor6HeCai.Add(num);
                }

                #endregion
            }
            else if (playName == "正5特")
            {
                #region 正5特

                if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!geFor6HeCai.Contains(num)) geFor6HeCai.Add(num);
                }

                #endregion
            }
            else if (playName == "正6特")
            {
                #region 正6特

                if (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" ||
                         betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" ||
                         betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" ||
                         betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" ||
                         betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" ||
                         betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" ||
                         betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" ||
                         betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" ||
                         betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" ||
                         betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49")
                {
                    int num = int.Parse(betNum);
                    if (!sixFor6HeCai.Contains(num)) sixFor6HeCai.Add(num);
                }

                #endregion
            }
            else if (playName == "尾数")
            {
                #region 尾数

                string str = Util.GetWeiNum(betNum);

                string[] arr = str.Split(',');
                foreach (string s in arr)
                {
                    int num = int.Parse(s);
                    if (!wanFor6HeCai.Contains(num)) wanFor6HeCai.Add(num);
                    if (!qianFor6HeCai.Contains(num)) qianFor6HeCai.Add(num);
                    if (!baiFor6HeCai.Contains(num)) baiFor6HeCai.Add(num);
                    if (!shiFor6HeCai.Contains(num)) shiFor6HeCai.Add(num);
                    if (!geFor6HeCai.Contains(num)) geFor6HeCai.Add(num);
                    if (!sixFor6HeCai.Contains(num)) sixFor6HeCai.Add(num);
                    if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                }


                #endregion
            }
            else if (playName == "一肖")
            {
                #region 一肖

                string str = Util.GetDigitByShengxiao(betNum);

                string[] arr = str.Split(',');
                foreach (string s in arr)
                {
                    int num = int.Parse(s);
                    if (!wanFor6HeCai.Contains(num)) wanFor6HeCai.Add(num);
                    if (!qianFor6HeCai.Contains(num)) qianFor6HeCai.Add(num);
                    if (!baiFor6HeCai.Contains(num)) baiFor6HeCai.Add(num);
                    if (!shiFor6HeCai.Contains(num)) shiFor6HeCai.Add(num);
                    if (!geFor6HeCai.Contains(num)) geFor6HeCai.Add(num);
                    if (!sixFor6HeCai.Contains(num)) sixFor6HeCai.Add(num);
                    if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                }


                #endregion
            }
            else if (playName == "十不中" || playName == "九不中" || playName == "八不中" || playName == "七不中" || playName == "六不中" || playName == "五不中")
            {
                #region 不中

                string[] arr = betNum.Split(',');

                foreach (string s in arr)
                {
                    int s2 = int.Parse(s);

                    if (!wanFor6HeCai.Contains(s2)) wanFor6HeCai.Add(s2);
                    if (!qianFor6HeCai.Contains(s2)) qianFor6HeCai.Add(s2);
                    if (!baiFor6HeCai.Contains(s2)) baiFor6HeCai.Add(s2);
                    if (!shiFor6HeCai.Contains(s2)) shiFor6HeCai.Add(s2);
                    if (!geFor6HeCai.Contains(s2)) geFor6HeCai.Add(s2);
                    if (!sixFor6HeCai.Contains(s2)) sixFor6HeCai.Add(s2);
                    if (!sevenFor6HeCai.Contains(s2)) sevenFor6HeCai.Add(s2);
                }

                #endregion
            }
            else if (playName == "半波")
            {
                #region 半波

                string str = Util.GetBanBoNum(betNum);

                string[] arr = str.Split(',');

                foreach (string s in arr)
                {
                    int num = int.Parse(s);
                    if (!sevenFor6HeCai.Contains(num)) sevenFor6HeCai.Add(num);
                }

                #endregion
            }

        }

        //检查 随机号码 能不能满足第二组要求
        public static bool CheckSecondFor6HeCai(string num, List<PossibleWin> list)
        {
            string playName = "";
            string betNum = "";


            foreach (PossibleWin win in list)
            {
                playName = win.PlayName;
                betNum = win.BetNum;

                int count = 0;

                if (playName == "三全中")
                {
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (num.Contains(s)) count++;
                    }

                    if (count >= 3) return false;

                }
                else if (playName == "三中二" || playName == "二全中" || playName == "二中特" || playName == "特串")
                {
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (num.Contains(s)) count++;
                    }

                    if (count >= 2) return false;

                }
                else if (playName == "五不中" || playName == "六不中" || playName == "七不中" || playName == "八不中" || playName == "九不中" || playName == "十不中")
                {
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (num.Contains(s)) count++;
                    }

                    if (count == 0) return false;

                }
                else if (playName == "二连肖(中)")
                {
                    string shengxiao = Util.ChangeOpenNumToShengxiao(num);
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (shengxiao.Contains(s)) count++;
                    }

                    if (count >= 2) return false;
                }
                else if (playName == "三连肖(中)")
                {
                    string shengxiao = Util.ChangeOpenNumToShengxiao(num);
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (shengxiao.Contains(s)) count++;
                    }

                    if (count >= 3) return false;
                }
                else if (playName == "四连肖(中)")
                {
                    string shengxiao = Util.ChangeOpenNumToShengxiao(num);
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (shengxiao.Contains(s)) count++;
                    }

                    if (count >= 4) return false;
                }
                else if (playName == "二连肖(不中)" || playName == "三连肖(不中)" || playName == "四连肖(不中)" || playName == "六肖连不中")
                {
                    string shengxiao = Util.ChangeOpenNumToShengxiao(num);
                    string[] arr = betNum.Split(',');
                    foreach (string s in arr)
                    {
                        if (shengxiao.Contains(s)) count++;
                    }

                    if (count == 0) return false;
                }



            }

            return true;
        }



        //检查重复
        public static string RemoveRepeatForLHC(string num)
        {
            string[] numArr = num.Split(',');

            for (int i = 6; i >= 0; i--)
            {
                string temp = numArr[i];

                int index1 = Array.IndexOf(numArr, temp);
                int index2 = Array.LastIndexOf(numArr, temp);

                if (index1 != index2)
                {
                    numArr[i] = GetSYNumForLHC(numArr, i);
                }
            }

            return string.Join(",", numArr);
        }


        public static string GetSYNumForLHC(string[] numArr, int index)
        {
            string[] tempArr =
            {
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" ,
                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" ,
                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" ,
                "31", "32", "33", "34", "35", "36", "37", "38", "39", "40" ,
                "41", "42", "43", "44", "45", "46", "47", "48", "49"
            };

            List<string> list = tempArr.ToList();

            for (int i = 6; i >= 0; i--)
            {
                string s = numArr[i];
                if (i != index && list.Contains(s))
                {
                    list.Remove(s);
                }
            }

            Random r = new Random();
            int index2 = r.Next(0, list.Count - 1);

            return list[index2];
        }



        #endregion

        #region 7XingCai



        //从 wanFor6HeCai2中随机一组号码出来
        public static string GetSuiJiNumFor7XingCai()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0;

            int len = wanFor7XingCai2.Count;
            if (len != 0)
            {
                a = wanFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                a = r.Next(1, 50);
            }

            len = qianFor7XingCai2.Count;
            if (len != 0)
            {
                b = qianFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                b = r.Next(1, 50);
            }

            len = baiFor7XingCai2.Count;
            if (len != 0)
            {
                c = baiFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                c = r.Next(1, 50);
            }

            len = shiFor7XingCai2.Count;
            if (len != 0)
            {
                d = shiFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                d = r.Next(1, 50);
            }


            len = geFor7XingCai2.Count;
            if (len != 0)
            {
                e = geFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                e = r.Next(1, 50);
            }

            len = sixFor7XingCai2.Count;
            if (len != 0)
            {
                f = sixFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                f = r.Next(1, 50);
            }

            len = sevenFor7XingCai2.Count;
            if (len != 0)
            {
                g = sevenFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                g = r.Next(1, 50);
            }

            return a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g;
        }

        public static string GetSuiJiNumFor7XingCaiFangShui()
        {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0;

            int len = wanFor7XingCai2.Count;
            if (len != 0)
            {
                a = wanFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                a = r.Next(1, 50);
            }

            len = qianFor7XingCai2.Count;
            if (len != 0)
            {
                b = qianFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                b = r.Next(1, 50);
            }

            len = baiFor7XingCai2.Count;
            if (len != 0)
            {
                c = baiFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                c = r.Next(1, 50);
            }

            len = shiFor7XingCai2.Count;
            if (len != 0)
            {
                d = shiFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                d = r.Next(1, 50);
            }


            len = geFor7XingCai2.Count;
            if (len != 0)
            {
                e = geFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                e = r.Next(1, 50);
            }

            len = sixFor7XingCai2.Count;
            if (len != 0)
            {
                f = sixFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                f = r.Next(1, 50);
            }

            len = sevenFor7XingCai2.Count;
            if (len != 0)
            {
                g = sevenFor7XingCai2[r.Next(0, len)];
            }
            else
            {
                g = r.Next(1, 50);
            }



            return a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g;
        }

        //给wanFor6HeCai qianFor6HeCai求差 差值放在wanFor6HeCai2 中
        public static void GetChaNumFor7XingCai()
        {
            //万位
            for (int i = 1; i < 50; i++)
            {
                if (!wanFor7XingCai.Contains(i)) wanFor7XingCai2.Add(i);
                if (!qianFor7XingCai.Contains(i)) qianFor7XingCai2.Add(i);
                if (!baiFor7XingCai.Contains(i)) baiFor7XingCai2.Add(i);
                if (!shiFor7XingCai.Contains(i)) shiFor7XingCai2.Add(i);
                if (!geFor7XingCai.Contains(i)) geFor7XingCai2.Add(i);
                if (!sixFor7XingCai.Contains(i)) sixFor7XingCai2.Add(i);
                if (!sevenFor7XingCai.Contains(i)) sevenFor7XingCai2.Add(i);
            }
        }

        //杀掉普通号码
        public static void KillNumFor7XingCai(PossibleWin win)
        {
            string playName = win.PlayName;
            string betNum = win.BetNum;

            if (playName == "定千位")
            {
                int num = int.Parse(betNum);
                if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
            }
            else if (playName == "定百位")
            {
                int num = int.Parse(betNum);
                if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
            }
            else if (playName == "定十位")
            {
                int num = int.Parse(betNum);
                if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
            }
            else if (playName == "定个位")
            {
                int num = int.Parse(betNum);
                if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
            }
            else if (playName == "千##个")
            {
                //2,5#4,6
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }
            else if (playName == "#百十#")
            {
                //2,5#4,6
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }
            }
            else if (playName == "千#十#")
            {
                //2,5#4,6
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }
            }
            else if (playName == "#百#个")
            {
                //2,5#4,6
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }
            else if (playName == "千百##")
            {
                //2,5#4,6
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }
            }
            else if (playName == "##十个")
            {
                //2,5#4,6
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }
            else if (playName == "千百十#")
            {
                //1,6#2#2
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');
                string[] arr3 = arr[2].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }

                foreach (string s in arr3)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }
            }
            else if (playName == "千百#个")
            {
                //1,6#2#2
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');
                string[] arr3 = arr[2].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }

                foreach (string s in arr3)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }
            else if (playName == "#百十个")
            {
                //1,6#2#2
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');
                string[] arr3 = arr[2].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }

                foreach (string s in arr3)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }
            else if (playName == "千#十个")
            {
                //1,6#2#2
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');
                string[] arr3 = arr[2].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }

                foreach (string s in arr3)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }
            else if (playName == "四定位")
            {
                //2#3#6#4
                string[] arr = betNum.Split('#');
                string[] arr1 = arr[0].Split(',');
                string[] arr2 = arr[1].Split(',');
                string[] arr3 = arr[2].Split(',');
                string[] arr4 = arr[3].Split(',');

                foreach (string s in arr1)
                {
                    int num = int.Parse(s);
                    if (!wanFor7XingCai.Contains(num)) wanFor7XingCai.Add(num);
                }

                foreach (string s in arr2)
                {
                    int num = int.Parse(s);
                    if (!qianFor7XingCai.Contains(num)) qianFor7XingCai.Add(num);
                }

                foreach (string s in arr3)
                {
                    int num = int.Parse(s);
                    if (!baiFor7XingCai.Contains(num)) baiFor7XingCai.Add(num);
                }

                foreach (string s in arr4)
                {
                    int num = int.Parse(s);
                    if (!shiFor7XingCai.Contains(num)) shiFor7XingCai.Add(num);
                }
            }

        }



        #endregion

        //删除  大小这种互补的 留下大额的
        public static List<PossibleWin> DeleteHuBuBet(List<PossibleWin> list, int lType)
        {
            List<PossibleWin> data = Util.Clone<List<PossibleWin>>(list);


            foreach (PossibleWin win in list)
            {
                string betNum = win.BetNum;


                //string num = "0123456789";

                if ((lType == 2 || lType == 12) && (betNum == "0" || betNum == "1" || betNum == "2" || betNum == "3" || betNum == "4" || betNum == "5" || betNum == "6" || betNum == "7" || betNum == "8" || betNum == "9"))
                {
                    int count = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        PossibleWin temp = data.Where(p => p.BetNum == i.ToString() && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                        if (temp != null) count++;
                    }




                    if ((lType == 2 && count >= 8) || (lType == 12 && count == 10))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            PossibleWin temp = data.Where(p => p.BetNum == i.ToString() && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                            if (temp != null)
                            {
                                data.Remove(temp);
                            }
                        }
                    }
                }
                if (lType == 4 && (betNum == "01" || betNum == "02" || betNum == "03" || betNum == "04" || betNum == "05" || betNum == "06" || betNum == "07" || betNum == "08" || betNum == "09" || betNum == "10" || betNum == "11" || betNum == "12" || betNum == "13" || betNum == "14" || betNum == "15" || betNum == "16" || betNum == "17" || betNum == "18" || betNum == "19" || betNum == "20" || betNum == "21" || betNum == "22" || betNum == "23" || betNum == "24" || betNum == "25" || betNum == "26" || betNum == "27" || betNum == "28" || betNum == "29" || betNum == "30" || betNum == "31" || betNum == "32" || betNum == "33" || betNum == "34" || betNum == "35" || betNum == "36" || betNum == "37" || betNum == "38" || betNum == "39" || betNum == "40" || betNum == "41" || betNum == "42" || betNum == "43" || betNum == "44" || betNum == "45" || betNum == "46" || betNum == "47" || betNum == "48" || betNum == "49"))
                {
                    int count = 0;
                    for (int i = 1; i < 50; i++)
                    {
                        string betNum2 = i.ToString();
                        if (i < 10)
                        {
                            betNum2 = "0" + i;
                        }

                        PossibleWin temp = data.Where(p => p.BetNum == betNum2 && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                        if (temp != null) count++;
                    }

                    if (count == 49)
                    {
                        for (int i = 1; i < 50; i++)
                        {
                            string betNum2 = i.ToString();
                            if (i < 10)
                            {
                                betNum2 = "0" + i;
                            }

                            PossibleWin temp = data.Where(p => p.BetNum == betNum2 && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                            data.Remove(temp);
                        }
                    }
                }
                else if (lType == 4 && (betNum == "1-10" || betNum == "11-20" || betNum == "21-30" || betNum == "31-40" || betNum == "41-49"))
                {
                    int count = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        int num1 = int.Parse(i + "1");
                        int num2 = 0;
                        if (i != 4)
                        {
                            num2 = num1 + 9;
                        }
                        else
                        {
                            num2 = num1 + 8;
                        }

                        string betNum2 = num1 + "-" + num2;

                        PossibleWin temp = data.Where(p => p.BetNum == betNum2 && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                        if (temp != null) count++;
                    }

                    if (count == 5)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int num1 = int.Parse(i + "1");
                            int num2 = 0;
                            if (i != 4)
                            {
                                num2 = num1 + 9;
                            }
                            else
                            {
                                num2 = num1 + 8;
                            }

                            string betNum2 = num1 + "-" + num2;

                            PossibleWin temp = data.Where(p => p.BetNum == betNum2 && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                            data.Remove(temp);
                        }
                    }

                }
                else if (lType == 8 && (betNum == "1" || betNum == "2" || betNum == "3" || betNum == "4" || betNum == "5" || betNum == "6" || betNum == "7" || betNum == "8" || betNum == "9" || betNum == "10"))
                {
                    int count = 0;
                    for (int i = 1; i < 11; i++)
                    {
                        PossibleWin temp = data.Where(p => p.BetNum == i.ToString() && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                        if (temp != null) count++;
                    }

                    if (count == 10)
                    {
                        for (int i = 1; i < 11; i++)
                        {
                            PossibleWin temp = data.Where(p => p.BetNum == i.ToString() && p.PlayName == win.PlayName && p.WinMoney == win.WinMoney).FirstOrDefault();
                            data.Remove(temp);
                        }
                    }

                }
                else if (betNum == "大")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "小" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }

                    }
                }
                else if (betNum == "小")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "大" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "单")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "双" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "双")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "单" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总和大")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总和小" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总和小")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总和大" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总和单")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总和双" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总和双")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总和单" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "龙")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "虎" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "虎")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "龙" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }          //六合彩
                else if (betNum == "合单")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "合双" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "家禽")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "野兽" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "尾大")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "尾小" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总单")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总双" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总大")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总小" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }
                else if (betNum == "总尾大")
                {
                    PossibleWin temp = data.Where(p => p.BetNum == "总尾小" && p.PlayName == win.PlayName).FirstOrDefault();
                    if (temp != null)
                    {
                        if (temp.WinMoney > win.WinMoney)
                        {
                            data.Remove(win);
                        }
                        else
                        {
                            data.Remove(temp);
                        }
                    }
                }


            }

            return data;
        }


        #region 根据单子判断可能中奖的最大注数(该方法用于计算追号单注奖金是否超过20W)[区分彩种]

        public static decimal GetPossibleWinMoney(BettingRecord record)
        {
            int lType = record.lType;
            int winCount = 0;
            decimal totalMoney = -1;

            #region 首先计算最大中奖注数

            if (lType == 2 || lType == 12 || lType == 8)
            {
                winCount = 1;
            }

            #endregion

            totalMoney = winCount * record.Peilv * record.UnitMoney;

            return totalMoney;
        }




        #endregion






        public static bool IsTryUser(UserInfo user)
        {
            if (string.IsNullOrEmpty(user.TryId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //即时注单金额
        public static decimal JSZDMoney(UserInfo user)
        {

            string sql = "";

            if (IsTryUser(user))                //试玩
            {
                sql = "select SUM(BetCount * UnitMoney) from BettingRecordWeek where WinState = 1 and TryId= '" + user.TryId + "'";
            }
            else
            {
                sql = "select SUM(BetCount * UnitMoney) from BettingRecordWeek where WinState = 1 and UserId=" + user.Id;
            }

            object o = SqlHelper.ExecuteScalarForFenZhan(77, sql);

            if (o != DBNull.Value)
            {
                return decimal.Parse(o.ToString());
            }
            else
            {
                return 0;
            }
        }

        //今日输赢
        public static decimal TodayMoney(UserInfo user)
        {
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd");

            DateTime d1 = DateTime.Parse(dateStr + " 0:0:0");
            DateTime d2 = DateTime.Parse(dateStr + " 23:59:59");

            string sql = "";

            if (IsTryUser(user))
            {
                //sql = "select Sum(WinMoney) + Sum(TuiShui) - Sum(BetCount * UnitMoney) from BettingRecord where SubTime > '" + d1 + "' and SubTime < '" + d2 + "' and WinState <> 1 and TryId= '" + user.TryId + "'";
                sql = "select Sum(SY) from BettingRecordDay where SubTime > '" + d1 + "' and SubTime < '" + d2 + "' and WinState <> 1 and TryId= '" + user.TryId + "'";
            }
            else
            {
                //sql = "select Sum(WinMoney)+ Sum(TuiShui) - Sum(BetCount * UnitMoney) from BettingRecord where SubTime > '" + d1 + "' and SubTime < '" + d2 + "' and WinState <> 1  and UserId= " + user.Id;
                sql = "select Sum(SY) from BettingRecordDay where SubTime > '" + d1 + "' and SubTime < '" + d2 + "' and WinState <> 1  and UserId= " + user.Id;
            }

            object o = SqlHelper.ExecuteScalarForFenZhan(77, sql);

            if (o != DBNull.Value)
            {
                return (decimal)o;
            }
            else
            {
                return 0;
            }
        }

        //更新彩票余额
        public static void UpdateLotteryMoney(UserInfo user, decimal money)
        {
            //更新缓存余额
            if (IsTryUser(user))
            {
                user.Money += money;
                Common.UpdateCacheUser(user);
            }
            else
            {
                //更新余额
                string sql = "update UserInfo set Money+= + " + money + " where Id=" + user.Id;
                SqlHelper.ExecuteNonQuery(sql);
            }
        }



        //更新缓存中的 LoginUser
        public static void UpdateCacheUser(UserInfo user)
        {
            if (IsTryUser(user))
            {
                //CacheHelper.SetCache(user.TryId, user);
                //RedisCacheHelper.SetCache(user.TryId,user,DateTime.Now.AddMinutes(15));
                CacheManager.Set(user.TryId, user, 15);

            }
        }

        public static void UpdateMoney(UserInfo user, decimal money)
        {
            //更新余额
            string sql = "update UserInfo set Money=Money +" + money + " where Id=" + user.Id;
            SqlHelper.ExecuteNonQuery(sql);

            //更新缓存余额
            user.Money += money;
            Common.UpdateCacheUser(user);
        }

        public static UserInfo GetUserInfoById(long id)
        {
            string sql = "select * from UserInfo where Id=" + id;
            UserInfo user = Util.ReaderToModel<UserInfo>(sql);

            return user;
        }

        public static UserInfo GetUserInfoById(int id)
        {
            string sql = "select * from UserInfo where Id=" + id;
            UserInfo user = Util.ReaderToModel<UserInfo>(sql);

            return user;
        }

        //银行卡数量
        public static int GetBankNum(int uid)
        {
            string sql = "select count(*) from BankInfo where UserId = " + uid;
            return (int)SqlHelper.ExecuteScalar(sql);
        }

        public static List<BankInfo> GetBankList(int uid)
        {
            string sql = "select * from BankInfo where UserId = " + uid;
            return Util.ReaderToList<BankInfo>(sql);
        }


        //采集开奖有关

        #region 采集开奖有关


        public static bool JudgeSanQuanZhong(string num, List<BettingRecord> list)
        {
            num = num.Substring(0, num.Length - 3);

            string[] betArr;
            foreach (BettingRecord record in list)
            {
                betArr = record.BetNum.Split(',');

                int containsCount = 0;

                foreach (string s in betArr)
                {
                    if (num.Contains(s))
                    {
                        containsCount++;
                    }
                }

                if (containsCount >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool JudgeErQuanZhong(string num, List<BettingRecord> list)
        {
            num = num.Substring(0, num.Length - 3);

            string[] betArr;
            foreach (BettingRecord record in list)
            {
                betArr = record.BetNum.Split(',');

                int containsCount = 0;

                foreach (string s in betArr)
                {
                    if (num.Contains(s))
                    {
                        containsCount++;
                    }
                }

                if (containsCount >= 2)
                {
                    return true;
                }
            }

            return false;
        }



        public static bool JudgeTeChuan(string num, List<BettingRecord> list)
        {
            string[] openArr = num.Split(',');
            string tema = openArr[6];

            string[] betArr;
            foreach (BettingRecord record in list)
            {
                betArr = record.BetNum.Split(',');

                if (betArr.Contains(tema))  //包含特码 
                {
                    //包含一个正码
                    for (int i = 0; i < openArr.Length - 1; i++)
                    {
                        if (betArr.Contains(openArr[i])) return true;
                    }
                }

            }

            return false;
        }

        public static bool JudgeBaoZi(string num)
        {
            string[] arr = num.Split(',');

            string a = arr[0];
            string b = arr[1];
            string c = arr[2];
            string d = arr[3];
            string e = arr[4];

            if ((a == b && b == c && a == c) || (b == c && c == d && b == d) || (c == d && d == e && c == e))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool JudgeBaoZiFor3D(string num)
        {
            string[] arr = num.Split(',');

            string a = arr[0];
            string b = arr[1];
            string c = arr[2];

            if (a == b && b == c && a == c)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool JudgeDuiZiFor3D(string num)
        {
            string[] arr = num.Split(',');

            string a = arr[0];
            string b = arr[1];
            string c = arr[2];

            if (a == b || b == c || a == c)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool JudgeHeZhiForK3(string num, List<BettingRecord> list)
        {
            string[] openArr = num.Split(',');

            string sum = (int.Parse(openArr[0]) + int.Parse(openArr[1]) + int.Parse(openArr[2])) + "";

            foreach (BettingRecord record in list)
            {
                if (record.BetNum == sum)
                {
                    return true;
                }
            }

            return false;
        }


        public static bool JudgeGuanJunForPk10(string num, List<BettingRecord> list)
        {
            string gunjun = num.Split(',')[0];

            foreach (BettingRecord record in list)
            {
                if (record.BetNum == gunjun)
                {
                    return true;
                }
            }

            return false;
        }


        public static bool Jude7XC4DW(string num, List<BettingRecord> list)
        {
            string[] arr = num.Split(',');
            foreach (BettingRecord record in list)
            {
                //3#1#4#3
                string[] arr2 = record.BetNum.Split('#');

                if (arr2[0].Contains(arr[0]) && arr2[1].Contains(arr[1]) && arr2[2].Contains(arr[2]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }
            }

            return false;
        }


        public static bool Jude7XC3DW(string num, List<BettingRecord> list)
        {
            string[] arr = num.Split(',');
            foreach (BettingRecord record in list)
            {
                //3#1#4#3
                string[] arr2 = record.BetNum.Split('#');

                string playName = record.PlayName;

                if (playName == "千百十#" && arr2[0].Contains(arr[0]) && arr2[1].Contains(arr[1]) && arr2[2].Contains(arr[2]))
                {
                    return true;
                }
                else if (playName == "千百#个" && arr2[0].Contains(arr[0]) && arr2[1].Contains(arr[1]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }
                else if (playName == "#百十个" && arr2[1].Contains(arr[1]) && arr2[2].Contains(arr[2]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }
                else if (playName == "千#十个" && arr2[0].Contains(arr[0]) && arr2[2].Contains(arr[2]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }



            }

            return false;
        }


        public static bool Jude7XC2DW(string num, List<BettingRecord> list)
        {
            string[] arr = num.Split(',');
            foreach (BettingRecord record in list)
            {
                //3#1#4#3
                string[] arr2 = record.BetNum.Split('#');

                string playName = record.PlayName;

                if (playName == "千##个" && arr2[0].Contains(arr[0]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }
                else if (playName == "#百十#" && arr2[1].Contains(arr[1]) && arr2[2].Contains(arr[2]))
                {
                    return true;
                }
                else if (playName == "千#十#" && arr2[0].Contains(arr[0]) && arr2[2].Contains(arr[2]))
                {
                    return true;
                }
                else if (playName == "#百#个" && arr2[1].Contains(arr[1]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }
                else if (playName == "千百##" && arr2[0].Contains(arr[0]) && arr2[1].Contains(arr[1]))
                {
                    return true;
                }
                else if (playName == "##十个" && arr2[2].Contains(arr[2]) && arr2[3].Contains(arr[3]))
                {
                    return true;
                }


            }

            return false;
        }



        //防止重复进入开奖方法
        private static object OpenObj = new object();



        //改良版
        public static void CreateJisuNum(int lType)
        {

            //if (Util.GetRemainingTime(lType) == "已封盘" && Util.GetOpenRemainingTime(lType) != "正在开奖")
            if (Util.GetRemainingTime(lType) == "已封盘" && Util.GetOpenRemainingTime(lType) != "正在开奖" && int.Parse(Util.GetOpenRemainingTime(lType).Split('&')[2]) < 28)
            {

                //lock (OpenObj)
                //{

                DateTime d1 = DateTime.Now;

                string issue = (long.Parse(Util.GetCurrentIssue(lType))).ToString();

                //判断是否有数据
                string seaSql = "select top(1)*  from LotteryRecord where lType =" + lType + " and Issue='" + issue + "'";
                LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(seaSql);

                if (lr == null)
                {


                    #region 获取开奖号码

                    string num = "";
                    if (lType == 2)
                    {
                        string key = "";

                        if (lType == 2)
                        {
                            key = "KSSSCSha";
                        }
                        else if (lType == 64)
                        {
                            key = "FLBSha";
                        }


                        string sql = "select Value from Setting where [Key] = '" + key + "'";
                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();


                        //LogHelper.WriteLog(kaiguan + "-----------");


                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql =
                                "select Id from BettingRecordDay where UserId > 0 and lType = 2 and BetNum like '豹子' and Issue='" +
                                issue + "'";
                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZi(num) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }

                            #endregion
                        }

                    }
                    else if (lType == 12 || lType == 20 || lType == 24)
                    {
                        string key = "KS3DSha";

                        if (lType == 20)
                        {
                            key = "KSPL3Sha";
                        }


                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = "";

                        if (lType == 24)
                        {
                            kaiguan = SqlHelper.ExecuteScalarForFenZhan(2, sql).ToString();             //2是梦想28  3是虚拟彩世家
                        }
                        else
                        {
                            kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                        }




                        //LogHelper.WriteLog("kaiguan---" + kaiguan + "-----------" + lType);

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                  " and BetNum like '豹子' and Issue='" + issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZiFor3D(num) && times < 10)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有豹子-处理对子问题

                                sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                                      " and BetNum like '对子' and Issue='" + issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeDuiZiFor3D(num) && times < 10)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }

                    }
                    else if (lType == 4)
                    {

                        string sql = "select Value from Setting where [Key] = 'LHCSha'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan == "开0")
                        {
                            num = CreateOpenNumFor6HeCaiYingLi(issue);
                        }
                        else if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理连码问题

                            sql =
                                "select BetNum from BettingRecord where UserId > 0 and lType = 4 and PlayName = '三全中' and Issue='" +
                                issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeSanQuanZhong(num, list) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                sql =
                                    "select BetNum from BettingRecordDay where UserId > 0 and lType = 4 and PlayName = '特串' and Issue='" +
                                    issue + "'";
                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                if (list.Count > 0)
                                {
                                    while (JudgeTeChuan(num, list) && times < 20)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    sql =
                                        "select BetNum from BettingRecordDay where UserId > 0 and lType = 4 and (PlayName = '二全中' or PlayName = '三中二') and Issue='" +
                                        issue + "'";
                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                    if (list.Count > 0)
                                    {
                                        while (JudgeErQuanZhong(num, list) && times < 20)
                                        {
                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }
                                    else
                                    {
                                        sql =
                                            "select BetNum from BettingRecordDay where UserId > 0 and lType = 4 and PlayName = '二中特' and Issue='" +
                                            issue + "'";
                                        list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                        if (list.Count > 0)
                                        {
                                            while ((JudgeTeChuan(num, list) || JudgeErQuanZhong(num, list)) &&
                                                   times < 20)
                                            {
                                                times++;
                                                num = GetOpenNumForSJ(lType);
                                            }
                                        }
                                    }
                                }

                            }

                            #endregion

                            #region 特码处理

                            //sql = "select Value from Setting where [Key] = 'LHCSha'";
                            //string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                            //if (kaiguan == "开")
                            //{
                            //    string tm = GetTuijianTMForK6(issue);
                            //    string[] numArr = num.Split(',');

                            //    if (!string.IsNullOrEmpty(tm) && !tm.Contains(numArr[6]))          //推荐号码不为空
                            //    {
                            //        string[] tmArr = tm.Split(' ');

                            //        foreach (string s in tmArr)
                            //        {
                            //            if (!num.Contains(s))
                            //            {
                            //                //满足条件
                            //                num = numArr[0] + "," + numArr[1] + "," + numArr[2] + "," + numArr[3] + "," + numArr[4] + "," + numArr[5] + "," + s;
                            //                break;
                            //            }
                            //        }
                            //    }

                            //}

                            #endregion
                        }

                    }
                    else if (lType == 6)
                    {
                        string sql = "select Value from Setting where [Key] = 'KS7XCSha'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理四定位问题

                            sql = "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType +
                                  " and PlayName = '四定位' and Issue='" + issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                            int times = 0; //次数  重试10次

                            if (list.Count > 0)
                            {
                                while (Jude7XC4DW(num, list) && times < 10)
                                {
                                    Thread.Sleep(500);

                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 处理三定位问题

                                sql = "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType +
                                      " and (PlayName = '千百十#' or PlayName = '千百#个' or PlayName = '#百十个' or PlayName = '千#十个') and Issue='" +
                                      issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                                times = 0; //次数  重试10次

                                if (list.Count > 0)
                                {
                                    while (Jude7XC3DW(num, list) && times < 10)
                                    {
                                        Thread.Sleep(500);

                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    #region 处理二定位问题

                                    sql = "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType +
                                          " and (PlayName = '千##个' or PlayName = '#百十#' or PlayName = '千#十#' or PlayName = '#百#个' or PlayName = '千百##' or PlayName = '##十个') and Issue='" +
                                          issue + "'";

                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                                    times = 0; //次数  重试10次

                                    if (list.Count > 0)
                                    {
                                        while (Jude7XC2DW(num, list) && times < 10)
                                        {
                                            Thread.Sleep(500);

                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }



                                    #endregion
                                }


                                #endregion
                            }


                            #endregion
                        }
                    }
                    else if (lType == 8 || lType == 10 || lType == 62)
                    {
                        string key = "";

                        if (lType == 10)
                        {
                            key = "KSFTSha";
                        }
                        else if (lType == 62)
                        {
                            key = "LLSFCSha";
                        }
                        else
                        {
                            key = "KSPK10Sha";
                        }


                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理冠亚和问题

                            sql =
                                "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType + " and PlayName = '冠亚和' and Issue='" +
                                issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                string temp = "";
                                foreach (BettingRecord record in list)
                                {
                                    temp += record.BetNum + ",";
                                }

                                string[] arr = num.Split(',');
                                string gyh = (int.Parse(arr[0]) + int.Parse(arr[1])) + "";


                                while (temp.Contains(gyh) && times < 10)
                                {
                                    Thread.Sleep(500);

                                    times++;
                                    num = GetOpenNumForSJ(lType);

                                    arr = num.Split(',');
                                    gyh = (int.Parse(arr[0]) + int.Parse(arr[1])) + "";
                                }
                            }
                            else
                            {
                                //
                                //sql =
                                //    "select BetNum from BettingRecord where UserId > 0 and lType = 8 and PlayName = '冠军' and (BetNum like '0' or BetNum like '1'  or BetNum like '2'  or BetNum like '3'  or BetNum like '4'  or BetNum like '5'  or BetNum like '6'  or BetNum like '7'  or BetNum like '8'  or BetNum like '9' ) and Issue='" +
                                //    issue + "'";

                                //list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                //list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                //list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                //if (list.Count > 0)
                                //{
                                //    while (JudgeGuanJunForPk10(num, list) && times < 6)
                                //    {
                                //        Thread.Sleep(500);

                                //        times++;
                                //        num = GetOpenNumForSJ(lType);

                                //    }
                                //}
                            }


                            #endregion
                        }

                    }
                    else if (lType == 12 || lType == 20 || lType == 24 || lType == 64 || lType == 84)
                    {
                        string key = "KS3DSha";

                        if (lType == 20)
                        {
                            key = "KSPL3Sha";
                        }

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = "";

                        if (lType == 24)
                        {
                            kaiguan = SqlHelper.ExecuteScalarForFenZhan(2, sql).ToString();             //2是梦想28  3是虚拟彩世家
                        }
                        else
                        {
                            kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                        }




                        //LogHelper.WriteLog("kaiguan---" + kaiguan + "-----------" + lType);

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                  " and BetNum like '豹子' and Issue='" + issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZiFor3D(num) && times < 10)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有豹子-处理对子问题

                                sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                                      " and BetNum like '对子' and Issue='" + issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeDuiZiFor3D(num) && times < 10)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }

                    }
                    else if (lType == 14)
                    {
                        string key = "KSKLSFSha";

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);
                        }

                    }
                    else if (lType == 16)
                    {
                        string key = "KS11X5Sha";

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);
                        }

                    }
                    else if (lType == 22)
                    {
                        string key = "KSK3Sha";

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                  " and (PlayName = '豹子' or BetNum like '任意豹子') and Issue='" + issue + "'";
                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZiFor3D(num) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有豹子-处理和值问题

                                sql = "select BetNum from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                      " and PlayName='和值' and (BetNum like '3' or BetNum like '4' or BetNum like '5' or BetNum like '6' or BetNum like '7' or BetNum like '18' or BetNum like '17' or BetNum like '16' or BetNum like '15' or BetNum like '14') and Issue='" +
                                      issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeHeZhiForK3(num, list) && times < 20)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    #region 没有和值-处理对子问题

                                    sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                          " and PlayName='对子' and Issue='" + issue + "'";

                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                    times = 0; //次数  重试20次

                                    if (list.Count > 0)
                                    {
                                        while (JudgeDuiZiFor3D(num) && times < 20)
                                        {
                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }

                                    #endregion
                                }

                                #endregion
                            }

                            #endregion
                        }

                    }

                    #endregion


                    string time = DateTime.Now.ToString();
                    string result = issue + "|" + num + "|" + time;

                    //添加号码
                    AddRecordWithOutOpen(1, lType, result);
                    AddRecordWithOutOpen(2, lType, result);
                    AddRecordWithOutOpen(3, lType, result);
                    AddRecordWithOutOpen(4, lType, result);
                    AddRecordWithOutOpen(5, lType, result);
                    AddRecordWithOutOpen(6, lType, result);


                    DateTime d2 = DateTime.Now;
                    //LogHelper.WriteLog(lType + "---->" + issue + "---->生成号码耗时:" + (d2 - d1).TotalSeconds);

                }
                else
                {

                    //LogHelper.WriteLog("----->");

                    string num = lr.Num;

                    //开奖
                    DealOpen.HandCurrentBetting(1, lType, issue, num);
                    DealOpen.HandCurrentBetting(2, lType, issue, num);
                    DealOpen.HandCurrentBetting(3, lType, issue, num);
                    DealOpen.HandCurrentBetting(4, lType, issue, num);
                    DealOpen.HandCurrentBetting(5, lType, issue, num);
                    DealOpen.HandCurrentBetting(6, lType, issue, num);

                    //LogHelper.WriteLog("----->");

                }
            }
            //}


        }



        //新版腾讯
        public static string CreateJisuNumForTXFFC(int lType)
        {


            string num = "";



            //num = GetOpenNumForYingLiToTXFFC(lType);

            num = GetOpenNumForSJ(lType);

            return num;


        }



        //AZXY5
        public static string CreateJisuNumForAZXY5(int lType)
        {


            string num = "";



            num = GetOpenNumForYingLiToAZXY5(lType);

            //num = GetOpenNumForSJ(lType);

            return num;


        }





        //改良版--花旗
        public static void CreateJisuNumForHuaQi(int lType)
        {

            if (Util.GetRemainingTime(lType) == "已封盘")
            //if(true)
            {

                //lock (OpenObj)
                //{

                DateTime d1 = DateTime.Now;

                string issue2 = Util.GetCurrentIssue(lType);

                string issue = (long.Parse(issue2)).ToString();

                //判断是否有数据
                string seaSql = "select top(1)*  from LotteryRecord where lType =" + lType + " and Issue='" + issue + "'";
                LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(seaSql);

                if (lr == null)
                {


                    #region 获取开奖号码

                    string num = "";
                    if (lType == 2)
                    {
                        string key = "";

                        if (lType == 2)
                        {
                            key = "KSSSCSha";
                        }


                        string sql = "select Value from Setting where [Key] = '" + key + "'";
                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();


                        //LogHelper.WriteLog(kaiguan + "-----------");


                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql =
                                "select Id from BettingRecordDay where UserId > 0 and lType = 2 and BetNum like '豹子' and Issue='" +
                                issue + "'";
                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZi(num) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }

                            #endregion
                        }

                    }
                    else if (lType == 12 || lType == 20 || lType == 24 || lType == 64 || lType == 84)
                    {
                        string key = "KS3DSha";

                        if (lType == 20)
                        {
                            key = "KSPL3Sha";
                        }
                        else if (lType == 24)
                        {
                            key = "KSSSCSha";
                        }
                        else if (lType == 64)
                        {
                            key = "LHCSha";
                        }
                        else if (lType == 84)
                        {
                            key = "KS7XCSha";
                        }

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = "";

                        if (lType == 24)
                        {
                            kaiguan = SqlHelper.ExecuteScalarForFenZhan(2, sql).ToString();             //2是梦想28  3是虚拟彩世家
                        }
                        else
                        {
                            kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                        }




                        //LogHelper.WriteLog("kaiguan---" + kaiguan + "-----------" + lType);

                        if (kaiguan != "关")
                        {

                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                  " and BetNum like '豹子' and Issue='" + issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZiFor3D(num) && times < 10)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有豹子-处理对子问题

                                sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                                      " and BetNum like '对子' and Issue='" + issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeDuiZiFor3D(num) && times < 10)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }

                    }
                    else if (lType == 4)
                    {

                        string sql = "select Value from Setting where [Key] = 'LHCSha'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan == "开0")
                        {
                            num = CreateOpenNumFor6HeCaiYingLi(issue);
                        }
                        else if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理连码问题

                            sql =
                                "select BetNum from BettingRecord where UserId > 0 and lType = 4 and PlayName = '三全中' and Issue='" +
                                issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeSanQuanZhong(num, list) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                sql =
                                    "select BetNum from BettingRecordDay where UserId > 0 and lType = 4 and PlayName = '特串' and Issue='" +
                                    issue + "'";
                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                if (list.Count > 0)
                                {
                                    while (JudgeTeChuan(num, list) && times < 20)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    sql =
                                        "select BetNum from BettingRecordDay where UserId > 0 and lType = 4 and (PlayName = '二全中' or PlayName = '三中二') and Issue='" +
                                        issue + "'";
                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                    if (list.Count > 0)
                                    {
                                        while (JudgeErQuanZhong(num, list) && times < 20)
                                        {
                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }
                                    else
                                    {
                                        sql =
                                            "select BetNum from BettingRecordDay where UserId > 0 and lType = 4 and PlayName = '二中特' and Issue='" +
                                            issue + "'";
                                        list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                        if (list.Count > 0)
                                        {
                                            while ((JudgeTeChuan(num, list) || JudgeErQuanZhong(num, list)) &&
                                                   times < 20)
                                            {
                                                times++;
                                                num = GetOpenNumForSJ(lType);
                                            }
                                        }
                                    }
                                }

                            }

                            #endregion

                            #region 特码处理

                            //sql = "select Value from Setting where [Key] = 'LHCSha'";
                            //string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                            //if (kaiguan == "开")
                            //{
                            //    string tm = GetTuijianTMForK6(issue);
                            //    string[] numArr = num.Split(',');

                            //    if (!string.IsNullOrEmpty(tm) && !tm.Contains(numArr[6]))          //推荐号码不为空
                            //    {
                            //        string[] tmArr = tm.Split(' ');

                            //        foreach (string s in tmArr)
                            //        {
                            //            if (!num.Contains(s))
                            //            {
                            //                //满足条件
                            //                num = numArr[0] + "," + numArr[1] + "," + numArr[2] + "," + numArr[3] + "," + numArr[4] + "," + numArr[5] + "," + s;
                            //                break;
                            //            }
                            //        }
                            //    }

                            //}

                            #endregion
                        }

                    }
                    else if (lType == 6)
                    {
                        string sql = "select Value from Setting where [Key] = 'KS7XCSha'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理四定位问题

                            sql = "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType +
                                  " and PlayName = '四定位' and Issue='" + issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                            int times = 0; //次数  重试10次

                            if (list.Count > 0)
                            {
                                while (Jude7XC4DW(num, list) && times < 10)
                                {
                                    Thread.Sleep(500);

                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 处理三定位问题

                                sql = "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType +
                                      " and (PlayName = '千百十#' or PlayName = '千百#个' or PlayName = '#百十个' or PlayName = '千#十个') and Issue='" +
                                      issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                                times = 0; //次数  重试10次

                                if (list.Count > 0)
                                {
                                    while (Jude7XC3DW(num, list) && times < 10)
                                    {
                                        Thread.Sleep(500);

                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    #region 处理二定位问题

                                    sql = "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType +
                                          " and (PlayName = '千##个' or PlayName = '#百十#' or PlayName = '千#十#' or PlayName = '#百#个' or PlayName = '千百##' or PlayName = '##十个') and Issue='" +
                                          issue + "'";

                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                                    times = 0; //次数  重试10次

                                    if (list.Count > 0)
                                    {
                                        while (Jude7XC2DW(num, list) && times < 10)
                                        {
                                            Thread.Sleep(500);

                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }



                                    #endregion
                                }


                                #endregion
                            }


                            #endregion
                        }
                    }
                    else if (lType == 8 || lType == 10 || lType == 62)
                    {
                        string key = "";

                        if (lType == 10)
                        {
                            key = "KSFTSha";
                        }
                        else if (lType == 62)
                        {
                            key = "LLSFCSha";
                        }
                        else
                        {
                            key = "KSPK10Sha";
                        }


                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理冠亚和问题

                            sql =
                                "select BetNum from BettingRecordDay where UserId > 0 and lType = " + lType + " and PlayName = '冠亚和' and Issue='" +
                                issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));


                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                string temp = "";
                                foreach (BettingRecord record in list)
                                {
                                    temp += record.BetNum + ",";
                                }

                                string[] arr = num.Split(',');
                                string gyh = (int.Parse(arr[0]) + int.Parse(arr[1])) + "";


                                while (temp.Contains(gyh) && times < 10)
                                {
                                    Thread.Sleep(500);

                                    times++;
                                    num = GetOpenNumForSJ(lType);

                                    arr = num.Split(',');
                                    gyh = (int.Parse(arr[0]) + int.Parse(arr[1])) + "";
                                }
                            }
                            else
                            {
                                //
                                //sql =
                                //    "select BetNum from BettingRecord where UserId > 0 and lType = 8 and PlayName = '冠军' and (BetNum like '0' or BetNum like '1'  or BetNum like '2'  or BetNum like '3'  or BetNum like '4'  or BetNum like '5'  or BetNum like '6'  or BetNum like '7'  or BetNum like '8'  or BetNum like '9' ) and Issue='" +
                                //    issue + "'";

                                //list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                //list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                //list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                //if (list.Count > 0)
                                //{
                                //    while (JudgeGuanJunForPk10(num, list) && times < 6)
                                //    {
                                //        Thread.Sleep(500);

                                //        times++;
                                //        num = GetOpenNumForSJ(lType);

                                //    }
                                //}
                            }


                            #endregion
                        }

                    }
                    else if (lType == 12 || lType == 20 || lType == 24 || lType == 64 || lType == 84)
                    {
                        string key = "KS3DSha";

                        if (lType == 20)
                        {
                            key = "KSPL3Sha";
                        }

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = "";

                        if (lType == 24)
                        {
                            kaiguan = SqlHelper.ExecuteScalarForFenZhan(2, sql).ToString();             //2是梦想28  3是虚拟彩世家
                        }
                        else
                        {
                            kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                        }




                        //LogHelper.WriteLog("kaiguan---" + kaiguan + "-----------" + lType);

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                  " and BetNum like '豹子' and Issue='" + issue + "'";

                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZiFor3D(num) && times < 10)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有豹子-处理对子问题

                                sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                                      " and BetNum like '对子' and Issue='" + issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeDuiZiFor3D(num) && times < 10)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }

                    }
                    else if (lType == 14)
                    {
                        string key = "KSKLSFSha";

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);
                        }

                    }
                    else if (lType == 16)
                    {
                        string key = "KS11X5Sha";

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);
                        }

                    }
                    else if (lType == 22)
                    {
                        string key = "KSK3Sha";

                        string sql = "select Value from Setting where [Key] = '" + key + "'";

                        string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                        if (kaiguan != "关")
                        {
                            num = GetOpenNumForYingLi(lType, issue, kaiguan);
                        }
                        else
                        {
                            num = GetOpenNumForSJ(lType);

                            #region 处理豹子问题

                            sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                  " and (PlayName = '豹子' or BetNum like '任意豹子') and Issue='" + issue + "'";
                            List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql));

                            int times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeBaoZiFor3D(num) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有豹子-处理和值问题

                                sql = "select BetNum from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                      " and PlayName='和值' and (BetNum like '3' or BetNum like '4' or BetNum like '5' or BetNum like '6' or BetNum like '7' or BetNum like '18' or BetNum like '17' or BetNum like '16' or BetNum like '15' or BetNum like '14') and Issue='" +
                                      issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeHeZhiForK3(num, list) && times < 20)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    #region 没有和值-处理对子问题

                                    sql = "select Id from BettingRecordDay where  UserId > 0 and lType = " + lType +
                                          " and PlayName='对子' and Issue='" + issue + "'";

                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                    times = 0; //次数  重试20次

                                    if (list.Count > 0)
                                    {
                                        while (JudgeDuiZiFor3D(num) && times < 20)
                                        {
                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }

                                    #endregion
                                }

                                #endregion
                            }

                            #endregion
                        }

                    }

                    #endregion


                    string time = DateTime.Now.ToString();
                    string result = issue + "|" + num + "|" + time;

                    //添加号码
                    AddRecordWithOutOpen(1, lType, result);
                    AddRecordWithOutOpen(2, lType, result);
                    AddRecordWithOutOpen(3, lType, result);
                    AddRecordWithOutOpen(4, lType, result);
                    AddRecordWithOutOpen(5, lType, result);
                    AddRecordWithOutOpen(6, lType, result);


                    DateTime d2 = DateTime.Now;
                    //LogHelper.WriteLog(lType + "---->" + issue + "---->生成号码耗时:" + (d2 - d1).TotalSeconds);

                }
                else
                {

                    //LogHelper.WriteLog("----->");

                    string num = lr.Num;

                    //开奖
                    DealOpen.HandCurrentBetting(1, lType, issue, num);
                    DealOpen.HandCurrentBetting(2, lType, issue, num);
                    DealOpen.HandCurrentBetting(3, lType, issue, num);
                    DealOpen.HandCurrentBetting(4, lType, issue, num);
                    DealOpen.HandCurrentBetting(5, lType, issue, num);
                    DealOpen.HandCurrentBetting(6, lType, issue, num);

                    //LogHelper.WriteLog("----->");

                }
            }
            //}


        }




        public static void CreateJisuNum2(int lType)
        {

            if (Util.GetRemainingTime(lType) == "已封盘" && Util.GetOpenRemainingTime(lType) != "正在开奖" && int.Parse(Util.GetOpenRemainingTime(lType).Split('&')[2]) < 28)
            //if (Util.GetRemainingTime(lType) == "已封盘" && Util.GetOpenRemainingTime(lType) != "正在开奖")
            {
                string issue = (long.Parse(Util.GetCurrentIssue(lType))).ToString();

                string num = "";
                if (lType == 2)
                {
                    string sql = "select Value from Setting where [Key] = 'KSSSCSha'";
                    string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                    if (kaiguan.StartsWith("开"))
                    {
                        num = GetOpenNumForYingLi(lType, issue, kaiguan);
                    }
                    else
                    {
                        num = GetOpenNumForSJ(lType);

                        #region 处理豹子问题

                        sql =
                            "select Id from BettingRecord where UserId > 0 and lType = 2 and BetNum like '豹子' and Issue='" +
                            issue + "'";
                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                        int times = 0; //次数  重试20次

                        if (list.Count > 0)
                        {
                            while (JudgeBaoZi(num) && times < 20)
                            {
                                times++;
                                num = GetOpenNumForSJ(lType);
                            }
                        }

                        #endregion
                    }

                }
                else if (lType == 4)
                {

                    string sql = "select Value from Setting where [Key] = 'LHCSha'";

                    string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                    if (kaiguan == "开0")
                    {
                        num = CreateOpenNumFor6HeCaiYingLi(issue);
                    }
                    else if (kaiguan.StartsWith("开"))
                    {
                        num = GetOpenNumForYingLi(lType, issue, kaiguan);
                    }
                    else
                    {
                        num = GetOpenNumForSJ(lType);

                        #region 处理连码问题

                        sql =
                            "select BetNum from BettingRecord where UserId > 0 and lType = 4 and PlayName = '三全中' and Issue='" +
                            issue + "'";

                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                        int times = 0; //次数  重试20次

                        if (list.Count > 0)
                        {
                            while (JudgeSanQuanZhong(num, list) && times < 20)
                            {
                                times++;
                                num = GetOpenNumForSJ(lType);
                            }
                        }
                        else
                        {
                            sql =
                                "select BetNum from BettingRecord where UserId > 0 and lType = 4 and PlayName = '特串' and Issue='" +
                                issue + "'";
                            list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                            if (list.Count > 0)
                            {
                                while (JudgeTeChuan(num, list) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                sql =
                                    "select BetNum from BettingRecord where UserId > 0 and lType = 4 and (PlayName = '二全中' or PlayName = '三中二') and Issue='" +
                                    issue + "'";
                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                if (list.Count > 0)
                                {
                                    while (JudgeErQuanZhong(num, list) && times < 20)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }
                                else
                                {
                                    sql =
                                        "select BetNum from BettingRecord where UserId > 0 and lType = 4 and PlayName = '二中特' and Issue='" +
                                        issue + "'";
                                    list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                    list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                    if (list.Count > 0)
                                    {
                                        while ((JudgeTeChuan(num, list) || JudgeErQuanZhong(num, list)) && times < 20)
                                        {
                                            times++;
                                            num = GetOpenNumForSJ(lType);
                                        }
                                    }
                                }
                            }

                        }

                        #endregion

                        #region 特码处理

                        //sql = "select Value from Setting where [Key] = 'LHCSha'";
                        //string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();
                        //if (kaiguan == "开")
                        //{
                        //    string tm = GetTuijianTMForK6(issue);
                        //    string[] numArr = num.Split(',');

                        //    if (!string.IsNullOrEmpty(tm) && !tm.Contains(numArr[6]))          //推荐号码不为空
                        //    {
                        //        string[] tmArr = tm.Split(' ');

                        //        foreach (string s in tmArr)
                        //        {
                        //            if (!num.Contains(s))
                        //            {
                        //                //满足条件
                        //                num = numArr[0] + "," + numArr[1] + "," + numArr[2] + "," + numArr[3] + "," + numArr[4] + "," + numArr[5] + "," + s;
                        //                break;
                        //            }
                        //        }
                        //    }

                        //}

                        #endregion
                    }

                }
                else if (lType == 6)
                {
                    string sql = "select Value from Setting where [Key] = 'KS7XCSha'";

                    string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                    if (kaiguan.StartsWith("开"))
                    {
                        num = GetOpenNumForYingLi(lType, issue, kaiguan);
                    }
                    else
                    {
                        num = GetOpenNumForSJ(lType);

                        #region 处理四定位问题

                        sql = "select BetNum from BettingRecord where UserId > 0 and lType = " + lType + " and PlayName = '四定位' and Issue='" + issue + "'";

                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));


                        int times = 0; //次数  重试10次

                        if (list.Count > 0)
                        {
                            while (Jude7XC4DW(num, list) && times < 10)
                            {
                                Thread.Sleep(500);

                                times++;
                                num = GetOpenNumForSJ(lType);
                            }
                        }
                        else
                        {
                            #region 处理三定位问题

                            sql = "select BetNum from BettingRecord where UserId > 0 and lType = " + lType + " and (PlayName = '千百十#' or PlayName = '千百#个' or PlayName = '#百十个' or PlayName = '千#十个') and Issue='" + issue + "'";

                            list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));


                            times = 0; //次数  重试10次

                            if (list.Count > 0)
                            {
                                while (Jude7XC3DW(num, list) && times < 10)
                                {
                                    Thread.Sleep(500);

                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 处理二定位问题

                                sql = "select BetNum from BettingRecord where UserId > 0 and lType = " + lType + " and (PlayName = '千##个' or PlayName = '#百十#' or PlayName = '千#十#' or PlayName = '#百#个' or PlayName = '千百##' or PlayName = '##十个') and Issue='" + issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));


                                times = 0; //次数  重试10次

                                if (list.Count > 0)
                                {
                                    while (Jude7XC2DW(num, list) && times < 10)
                                    {
                                        Thread.Sleep(500);

                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }



                                #endregion
                            }


                            #endregion
                        }


                        #endregion
                    }
                }
                else if (lType == 8)
                {

                    string sql = "select Value from Setting where [Key] = 'KSPK10Sha'";

                    string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                    if (kaiguan.StartsWith("开"))
                    {
                        num = GetOpenNumForYingLi(lType, issue, kaiguan);
                    }
                    else
                    {
                        num = GetOpenNumForSJ(lType);

                        #region 处理冠亚和问题

                        sql =
                            "select BetNum from BettingRecord where UserId > 0 and lType = 8 and PlayName = '冠亚和' and Issue='" +
                            issue + "'";

                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));


                        int times = 0; //次数  重试20次

                        if (list.Count > 0)
                        {
                            string temp = "";
                            foreach (BettingRecord record in list)
                            {
                                temp += record.BetNum + ",";
                            }

                            string[] arr = num.Split(',');
                            string gyh = (int.Parse(arr[0]) + int.Parse(arr[1])) + "";


                            while (temp.Contains(gyh) && times < 10)
                            {
                                Thread.Sleep(500);

                                times++;
                                num = GetOpenNumForSJ(lType);

                                arr = num.Split(',');
                                gyh = (int.Parse(arr[0]) + int.Parse(arr[1])) + "";
                            }
                        }
                        else
                        {
                            //
                            //sql =
                            //    "select BetNum from BettingRecord where UserId > 0 and lType = 8 and PlayName = '冠军' and (BetNum like '0' or BetNum like '1'  or BetNum like '2'  or BetNum like '3'  or BetNum like '4'  or BetNum like '5'  or BetNum like '6'  or BetNum like '7'  or BetNum like '8'  or BetNum like '9' ) and Issue='" +
                            //    issue + "'";

                            //list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            //list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            //list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                            //if (list.Count > 0)
                            //{
                            //    while (JudgeGuanJunForPk10(num, list) && times < 6)
                            //    {
                            //        Thread.Sleep(500);

                            //        times++;
                            //        num = GetOpenNumForSJ(lType);

                            //    }
                            //}
                        }


                        #endregion
                    }

                }
                else if (lType == 12)
                {
                    string key = "KS3DSha";

                    string sql = "select Value from Setting where [Key] = '" + key + "'";

                    string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                    if (kaiguan.StartsWith("开"))
                    {
                        num = GetOpenNumForYingLi(lType, issue, kaiguan);
                    }
                    else
                    {
                        num = GetOpenNumForSJ(lType);

                        #region 处理豹子问题

                        sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                              " and BetNum like '豹子' and Issue='" + issue + "'";

                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                        int times = 0; //次数  重试20次

                        if (list.Count > 0)
                        {
                            while (JudgeBaoZiFor3D(num) && times < 10)
                            {
                                times++;
                                num = GetOpenNumForSJ(lType);
                            }
                        }
                        else
                        {
                            #region 没有豹子-处理对子问题

                            sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                                  " and BetNum like '对子' and Issue='" + issue + "'";

                            list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                            times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeDuiZiFor3D(num) && times < 10)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }

                            #endregion
                        }

                        #endregion
                    }

                }
                else if (lType == 22)
                {
                    string key = "KSK3Sha";

                    string sql = "select Value from Setting where [Key] = '" + key + "'";

                    string kaiguan = SqlHelper.ExecuteScalar(sql).ToString();

                    if (kaiguan.StartsWith("开"))
                    {
                        num = GetOpenNumForYingLi(lType, issue, kaiguan);
                    }
                    else
                    {
                        num = GetOpenNumForSJ(lType);

                        #region 处理豹子问题

                        sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                              " and (PlayName = '豹子' or BetNum like '任意豹子') and Issue='" + issue + "'";
                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                        list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                        int times = 0; //次数  重试20次

                        if (list.Count > 0)
                        {
                            while (JudgeBaoZiFor3D(num) && times < 20)
                            {
                                times++;
                                num = GetOpenNumForSJ(lType);
                            }
                        }
                        else
                        {
                            #region 没有豹子-处理和值问题

                            sql = "select BetNum from BettingRecord where  UserId > 0 and lType = " + lType +
                                  " and PlayName='和值' and (BetNum like '3' or BetNum like '4' or BetNum like '5' or BetNum like '6' or BetNum like '7' or BetNum like '18' or BetNum like '17' or BetNum like '16' or BetNum like '15' or BetNum like '14') and Issue='" +
                                  issue + "'";

                            list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                            list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                            times = 0; //次数  重试20次

                            if (list.Count > 0)
                            {
                                while (JudgeHeZhiForK3(num, list) && times < 20)
                                {
                                    times++;
                                    num = GetOpenNumForSJ(lType);
                                }
                            }
                            else
                            {
                                #region 没有和值-处理对子问题

                                sql = "select Id from BettingRecord where  UserId > 0 and lType = " + lType +
                                      " and PlayName='对子' and Issue='" + issue + "'";

                                list = Util.ReaderToListForFenZhan<BettingRecord>(1, sql);
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql));
                                list.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql));

                                times = 0; //次数  重试20次

                                if (list.Count > 0)
                                {
                                    while (JudgeDuiZiFor3D(num) && times < 20)
                                    {
                                        times++;
                                        num = GetOpenNumForSJ(lType);
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }

                        #endregion
                    }

                }


                string time = DateTime.Now.ToString();
                string result = issue + "|" + num + "|" + time;

                lock (OpenObj)
                {

                    //LogHelper.WriteLog(issue + "-------->Go into");

                    string seaSql = "select top(1)*  from LotteryRecord where lType =" + lType + " and Issue='" + issue + "'";
                    LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(seaSql);

                    if (lr == null)
                    {

                        //LogHelper.WriteLog(issue + "-------->Go into2");

                        //string key = lType + issue + 1;

                        //if (CacheHelper.GetCache(key) == null)
                        //{
                        //添加号码
                        AddRecordWithOutOpen(1, lType, result);
                        AddRecordWithOutOpen(2, lType, result);
                        AddRecordWithOutOpen(3, lType, result);
                        AddRecordWithOutOpen(4, lType, result);

                        //}
                    }
                    else
                    {
                        num = lr.Num;

                        //开奖
                        DealOpen.HandCurrentBetting(1, lType, issue, num);
                        DealOpen.HandCurrentBetting(2, lType, issue, num);
                        DealOpen.HandCurrentBetting(3, lType, issue, num);
                        DealOpen.HandCurrentBetting(4, lType, issue, num);
                    }






                }

            }


        }


        public static void AddRecordWithOutOpen(int fenzhan, int lType, string result)
        {



            if (string.IsNullOrEmpty(result)) return;

            string[] arr = result.Split('|');

            string issue = arr[0];
            string num = arr[1];
            string time = arr[2];


            //string key = lType + issue + fenzhan;

            //if (CacheHelper.GetCache(key) == null)
            //{
            //判断期号是否存在
            string sql = "select count(1) from LotteryRecord where lType= " + lType + " and Issue ='" + issue + "'";

            int count = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);

            if (count == 0)
            {

                //LogHelper.WriteLog(lType + "---->" + issue);

                //添加记录
                sql = "insert into LotteryRecord(lType,Issue,Num,SubTime) values(" + lType + ",'" + issue + "','" + num + "','" + time + "')";

                SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);


                //清理数据
                sql = "delete from Data where lType =" + lType + " and Num ='" + num + "'";
                SqlHelper.ExecuteNonQueryForFenZhan(77, sql);

            }



            //CacheHelper.SetCache(key, 1, DateTime.Now.AddMinutes(2));

            //处理开奖
            //DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);

            //}


        }


        public static void AddRecord(int fenzhan, int lType, string result)
        {

            //LogHelper.WriteLog(fenzhan + "-------------->" + lType + "----------->" + result);

            if (string.IsNullOrEmpty(result)) return;

            string[] arr = result.Split('|');

            string issue = arr[0];
            string num = arr[1];
            string time = arr[2];

            //string key = lType + issue + fenzhan;

            //if (CacheHelper.GetCache(key) == null)
            //{
            //判断期号是否存在
            string sql = "select count(1) from LotteryRecord where lType= " + lType + " and Issue ='" + issue + "'";
            int count = Convert.ToInt32(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql));



            if (count == 0)
            {
                //if (lType == 81)
                //{
                //    num = Common.CreateJisuNumForAZXY5(81);

                //    sql = "delete  from  Data where  lType = 2  and  Num = '" + num + "'";
                //    SqlHelper.ExecuteNonQueryForFenZhan(2, sql);
                //}


                //添加记录
                sql = "insert into LotteryRecord(lType,Issue,Num,SubTime) values(" + lType + ",'" + issue + "','" + num + "','" + time + "')";
                SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);

                //处理开奖
                DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);
                //CacheHelper.SetCache(key, 1, DateTime.Now.AddMinutes(5));
            }
            //}

        }


        //用于补漏程序
        public static void AddRecord2(int fenzhan, int lType, string result)
        {
            try
            {

                if (string.IsNullOrEmpty(result)) return;

                string[] arr = result.Split('|');

                string issue = arr[0];
                string num = arr[1];
                string time = arr[2];

                //string key = lType + issue + fenzhan;

                //if (CacheHelper.GetCache(key) == null)
                //{
                //判断期号是否存在
                string sql = "select count(1) from LotteryRecord where lType= " + lType + " and Issue ='" + issue + "'";
                int count = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);

                if (count == 0)
                {
                    //添加记录
                    sql = "insert into LotteryRecord(lType,Issue,Num,SubTime) values(" + lType + ",'" + issue + "','" + num + "','" + time + "')";
                    SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);
                }

                //处理开奖
                DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);
                //CacheHelper.SetCache(key, 1, DateTime.Now.AddMinutes(5));

                //}

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message + ex.StackTrace);
            }

        }


        public static void AddRecordLHC(int fenzhan, int lType, string result)
        {
            if (string.IsNullOrEmpty(result)) return;


            int calcCount = 5000;

            string[] arr = result.Split('|');

            string issue = arr[0];
            string num = arr[1];
            string time = arr[2];

            //string key = lType + issue + fenzhan;

            //if (CacheHelper.GetCache(key) == null)
            //{
            //判断期号是否存在
            string sql = "select count(*) from LotteryRecord where lType= " + lType + " and Issue ='" + issue + "'";
            int count = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);
            if (count == 0)
            {
                //添加记录
                sql = "insert into LotteryRecord(lType,Issue,Num,SubTime) values(" + lType + ",'" + issue + "','" + num + "','" + time + "')";
                SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);

                //DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);


                #region 真实用户

                DateTime d1 = DateTime.Now;

                sql = "select count(*) from BettingRecord where lType = 3 and UserId > 0 and WinState = 1 and Issue='" + issue + "'";
                count = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);
                while (count > 0)
                {
                    sql = "select top(" + calcCount + ")* from BettingRecord where lType = 3 and UserId > 0 and WinState = 1 and Issue='" + issue + "'";

                    try
                    {
                        List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(fenzhan, sql);

                        foreach (BettingRecord record in list)
                        {
                            try
                            {
                                DealOpen.HandBetting(fenzhan, record, num);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteLog("OpenError---------------->" + ex.Message + "\r\n" + ex.StackTrace);
                            }

                        }

                        count -= calcCount;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog("ReaderToListError---------------->" + ex.Message + "\r\n" + ex.StackTrace);
                    }

                }

                DateTime d2 = DateTime.Now;

                double secs = (d2 - d1).TotalSeconds;
                string text = "REALLY------------------lType=" + lType + "-----------" + secs + "秒-------------";

                LogHelper.WriteLog(text);


                #endregion

                #region 试玩用户

                d1 = DateTime.Now;

                sql = "select * from BettingRecord where lType = 3 and UserId = 0 and WinState = 1 and Issue='" + issue + "'";

                try
                {

                    List<BettingRecord> list = Util.ReaderToListForFenZhan<BettingRecord>(fenzhan, sql);

                    foreach (BettingRecord record in list)
                    {
                        try
                        {
                            DealOpen.HandBetting(fenzhan, record, num);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog("OpenError---------------->" + ex.Message + "\r\n" + ex.StackTrace);
                        }

                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("ReaderToListError---------------->" + ex.Message + "\r\n" + ex.StackTrace);
                }



                d2 = DateTime.Now;

                secs = (d2 - d1).TotalSeconds;
                text = "SHIWAN------------------lType=" + lType + "-----------" + secs + "秒-------------";

                LogHelper.WriteLog(text);

                #endregion

                //CacheHelper.SetCache(key, 1, DateTime.Now.AddMinutes(5));
            }
            //}

        }


        public static void AddRecordForBaoXian(int fenzhan, int lType, string result)
        {

            if (string.IsNullOrEmpty(result)) return;

            string[] arr = result.Split('|');

            string issue = arr[0];
            string num = arr[1];
            string time = arr[2];

            string key = lType + issue + fenzhan;

            if (CacheHelper.GetCache(key) == null)
            {
                //判断期号是否存在
                string sql = "select count(*) from LotteryRecord where lType= " + lType + " and Issue ='" + issue + "'";
                int count = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);
                if (count == 0)
                {
                    //添加记录
                    sql = "insert into LotteryRecord(lType,Issue,Num,SubTime) values(" + lType + ",'" + issue + "','" +
                          num + "','" + time + "')";
                    SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);

                    CacheHelper.SetCache(key, 1, DateTime.Now.AddMinutes(5));

                    if (lType % 2 != 0) //官彩 统计开奖时间
                    {
                        DateTime d1 = DateTime.Now;

                        DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);

                        DateTime d2 = DateTime.Now;

                        double secs = (d2 - d1).TotalSeconds;
                        string text = "------------------lType=" + lType + "-----------" + secs + "秒-------------";

                        LogHelper.WriteLog(text);
                    }
                    else
                    {

                        //处理开奖
                        DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);
                    }

                }
            }
            else
            {
                long lastIssue = long.Parse(issue) - 1;

                string sql = "select count(*) from LotteryRecord where lType=" + lType + " and Issue='" + lastIssue +
                             "'";
                LotteryRecord record = Util.ReaderToModelForFenZhan<LotteryRecord>(fenzhan, sql);

                if (record != null)
                {
                    sql = "select count(*) from  BettingRecord where UserId > 0 and  lType =" + lType + " and Issue='" + lastIssue + "' and WinState <> 1";
                    int c1 = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);

                    sql = "select count(*) from  BettingRecord where UserId > 0 and  lType =" + lType + " and Issue='" + lastIssue + "' and WinState = 1";
                    int c2 = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);

                    if (c1 == 0 && c2 > 0)
                    {
                        issue = lastIssue + "";
                        num = record.Num;

                        DealOpen.HandCurrentBetting(fenzhan, lType, issue, num);
                    }

                }
            }



        }

        #endregion





        public static string DigitZhuanHuan(int num)
        {
            string result = "";

            if (num == 1)
            {
                result = "一";
            }
            else if (num == 2)
            {
                result = "二";
            }
            else if (num == 3)
            {
                result = "三";
            }
            else if (num == 4)
            {
                result = "四";
            }
            else if (num == 5)
            {
                result = "五";
            }
            else if (num == 6)
            {
                result = "六";
            }
            else if (num == 7)
            {
                result = "七";
            }
            else if (num == 8)
            {
                result = "八";
            }

            return result;
        }



        //----------------------------------5-17--------------------------




        public static int GetIdByName(string username)
        {
            string sql = "select top(1) Id from UserInfo  where UserName = @UserName";

            SqlParameter[] pms = { new SqlParameter("@UserName", username), };
            object o = SqlHelper.ExecuteScalar(sql, pms);

            if (o != DBNull.Value)
            {
                return (int)o;
            }
            else
            {
                return 0;
            }
        }

        public static void AddMsg(int userId, int? adminId, string content)
        {
            string sql = "insert into Message(UserId,AdminId,Content,SubTime) values(" + userId + "," + adminId + ",@Content,'" + DateTime.Now.ToString() + "')";

            SqlParameter[] pms = { new SqlParameter("@Content", content), };
            SqlHelper.ExecuteNonQuery(sql, pms);
        }



        #region 判断存取款是否满足流水ForC8TuiGuang


        public static void JudeLiuShuiForCunKuanForC8TuiGuang(int fenzhan, int uid)
        {
            //判断有几笔存款单子
            string sql = "select * from ComeOutRecord where Type = 1 and State = 3 and UserId=" + uid;
            List<ComeOutRecord> list = Util.ReaderToListForFenZhan<ComeOutRecord>(fenzhan, sql);

            decimal rechargeMoney = 0;
            DateTime date;

            if (list.Count == 1)
            {
                rechargeMoney = list[0].Money;

                //判断存款额 和 投注额
                sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid;

                if (!GetCunKuanResult(fenzhan, sql, rechargeMoney)) //差流水
                {
                    sql = "update ComeOutRecord set IsChaLiuShui = 1 where Id=" + list[0].Id;
                    SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);
                }
            }
            else if (list.Count > 1)
            {
                ComeOutRecord temp = list.Where(p => p.IsChaLiuShui == true).FirstOrDefault();
                if (temp != null)   //之前就有差的 //算从这一笔以后的
                {
                    date = temp.SubTime;   //最后一次充值的时间
                    rechargeMoney = list.Where(p => p.SubTime >= date).Sum(p => p.Money);
                    sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid + " and SubTime > '" + date.ToString() + "'";

                    if (GetCunKuanResult(fenzhan, sql, rechargeMoney))
                    {
                        //将之前所有的  都标为 不差流水
                        sql = "update ComeOutRecord set IsChaLiuShui = 0 where IsChaLiuShui = 1 and UserId=" + temp.UserId;
                    }
                    else
                    {
                        //将最后一个标为差流水
                        sql = "update ComeOutRecord set IsChaLiuShui = 1 where Id=" + list.LastOrDefault().Id;
                    }

                    SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);             //执行

                }
                else    //算最后一笔的
                {
                    temp = list.LastOrDefault();
                    date = temp.SubTime;
                    rechargeMoney = temp.Money;

                    sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid + " and SubTime > '" + date.ToString() + "'";

                    if (!GetCunKuanResult(fenzhan, sql, rechargeMoney)) //差流水
                    {
                        sql = "update ComeOutRecord set IsChaLiuShui = 1 where Id=" + temp.Id;
                        SqlHelper.ExecuteNonQueryForFenZhan(fenzhan, sql);
                    }

                }
            }
        }

        public static string JudeLiuShuiForQuKuan(int fenzhan, int uid)
        {
            //判断有几笔存款单子
            string sql = "select * from ComeOutRecord where Type = 1 and State = 3 and UserId=" + uid;
            List<ComeOutRecord> list = Util.ReaderToListForFenZhan<ComeOutRecord>(fenzhan, sql);


            decimal rechargeMoney = 0;
            DateTime date;

            if (list.Count == 1) //只有一笔存款
            {
                rechargeMoney = list[0].Money;

                //判断存款额 和 投注额
                sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid;

                return GetQuKuanResult(fenzhan, sql, rechargeMoney);
            }
            else if (list.Count > 1)
            {
                //在充下一笔的时候 就把上一笔的结算了
                ComeOutRecord temp = list.Where(p => p.IsChaLiuShui == true).FirstOrDefault();

                if (temp != null) //算从这一笔以后的
                {
                    date = temp.SubTime; //最后一次充值的时间

                    rechargeMoney = list.Where(p => p.SubTime >= date).Sum(p => p.Money);
                    sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid + " and SubTime > '" + date.ToString() + "'";
                    return GetQuKuanResult(fenzhan, sql, rechargeMoney);

                }
                else //算最后一笔的
                {
                    temp = list.Last();
                    date = temp.SubTime; //最后一次充值的时间
                    rechargeMoney = temp.Money;

                    sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid + " and SubTime > '" + date.ToString() + "'";
                    return GetQuKuanResult(fenzhan, sql, rechargeMoney);
                }
            }
            else
            {
                return "1";
            }


            return "";


        }

        public static bool GetCunKuanResult(int fenzhan, string sql, decimal rechargeMoney)
        {
            decimal? betMoney = GetDecimal(fenzhan, sql);
            if (betMoney != null)
            {
                if (betMoney < rechargeMoney)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static string GetQuKuanResult(int fenzhan, string sql, decimal rechargeMoney)
        {
            decimal? betMoney = GetDecimal(fenzhan, sql);

            sql = "select Value from Setting where [Key] = 'ChuKuanTimes'";

            int multiple = Convert.ToInt32(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql));  //流水倍数

            decimal liushui = rechargeMoney * multiple;


            if (betMoney != null)
            {
                if (betMoney < liushui)
                {
                    return rechargeMoney + "|" + betMoney + "|" + (liushui - betMoney);
                }
                else
                {
                    return "1";
                }
            }
            else
            {
                return rechargeMoney + "|0|" + liushui;
            }
        }

        public static decimal? GetDecimal(int fenzhan, string sql)
        {
            object o = SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql);
            if (o != DBNull.Value && o != null)
            {
                return Convert.ToDecimal(o);
            }

            return null;
        }


        #endregion




        #region 判断存取款是否满足流水


        public static void JudeLiuShuiForCunKuan(int uid)
        {
            //判断有几笔存款单子
            string sql = "select * from ComeOutRecord where (Type = 1 or Type = 10) and State = 3 and UserId=" + uid;
            List<ComeOutRecord> list = Util.ReaderToList<ComeOutRecord>(sql);

            decimal rechargeMoney = 0;
            DateTime date;


            List<ComeOutRecord> chaLiushuiList = list.Where(p => p.IsChaLiuShui == true).ToList();
            int chaCount = chaLiushuiList.Count;            //

            if (chaCount >= 1)     //之前就有差的 //算从这一笔以后的
            {
                ComeOutRecord temp = chaLiushuiList.FirstOrDefault();

                date = temp.SubTime;   //最后一次充值的时间
                rechargeMoney = chaLiushuiList.Sum(p => p.Money);
                sql = "select sum(BetCount * UnitMoney) from BettingRecord where UserId=" + uid + " and SubTime > '" + date.ToString() + "'";

                if (GetCunKuanResult(sql, rechargeMoney))
                {
                    //将之前所有的  都标为 不差流水
                    sql = "update ComeOutRecord set IsChaLiuShui = 0 where IsChaLiuShui = 1 and UserId=" + temp.UserId;
                }

                SqlHelper.ExecuteNonQuery(sql);             //执行

            }

            sql = "update ComeOutRecord set IsChaLiuShui = 1 where Id =" + list.LastOrDefault().Id;
            SqlHelper.ExecuteNonQuery(sql);

        }



        //
        public static string JudeLiuShuiForQuKuan(int uid)
        {

            //特殊情况
            int pid = Util.GetPId(uid);

            if (pid == 37104 || pid == 37105)
            {

                return "1";
            }



            //判断有几笔存款单子
            string sql = "select * from ComeOutRecord where (Type = 1 or Type = 10) and State = 3 and IsChaLiuShui = 1 and UserId=" + uid;
            List<ComeOutRecord> list = Util.ReaderToList<ComeOutRecord>(sql);

            decimal rechargeMoney = 0;
            DateTime date;

            if (list.Count >= 1)
            {
                //在充下一笔的时候 就把上一笔的结算了
                ComeOutRecord temp = list.FirstOrDefault();

                date = temp.SubTime; //最后一次充值的时间

                rechargeMoney = list.Where(p => p.SubTime >= date).Sum(p => p.Money);
                sql = "select sum(BetCount * UnitMoney) from BettingRecordMonth where UserId=" + uid + " and SubTime > '" + date.ToString() + "'";
                return GetQuKuanResult(sql, rechargeMoney);

            }
            else
            {
                return "1";
            }

        }

        public static bool GetCunKuanResult(string sql, decimal rechargeMoney)
        {
            decimal? betMoney = GetDecimal(sql);
            if (betMoney != null)
            {
                if (betMoney < rechargeMoney)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static string GetQuKuanResult(string sql, decimal rechargeMoney)
        {
            decimal? betMoney = GetDecimal(sql);

            sql = "select Value from Setting where [Key] = 'ChuKuanTimes'";

            int multiple = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));  //流水倍数

            if (multiple == 0)          //如果流水倍数为0
            {
                return "1";
            }

            decimal liushui = rechargeMoney * multiple;


            if (betMoney != null)
            {
                if (betMoney < liushui)
                {
                    return rechargeMoney + "|" + betMoney + "|" + (liushui - betMoney);
                }
                else
                {
                    return "1";
                }
            }
            else
            {
                return rechargeMoney + "|0|" + liushui;
            }
        }

        public static decimal? GetDecimal(string sql)
        {
            object o = SqlHelper.ExecuteScalar(sql);
            if (o != DBNull.Value && o != null)
            {
                return Convert.ToDecimal(o);
            }

            return null;
        }

        #endregion





        #region 判断存取款额度 是否满足要求

        public static bool JudgeChuKuanEDu(int money)
        {
            string sql = "select Value from Setting where [Key] = 'ChuKuanMoney1' or [Key] = 'ChuKuanMoney2'";
            List<Setting> list = Util.ReaderToList<Setting>(sql);

            var money1 = int.Parse(list[0].Value);
            var money2 = int.Parse(list[1].Value);

            if (money < money1 || money > money2)
            {
                return false;
            }

            return true;
        }

        public static bool JudgeCunKuanEDu(int money)
        {
            string sql = "select Value from Setting where [Key] = 'RenGongBnakRechargeMoney1' or [Key] = 'RenGongBnakRechargeMoney2'";
            List<Setting> list = Util.ReaderToList<Setting>(sql);

            var money1 = int.Parse(list[0].Value);
            var money2 = int.Parse(list[1].Value);

            if (money < money1 || money > money2)
            {
                return false;
            }

            return true;
        }

        public static bool JudgeThirdCunKuanEDu(int money)
        {
            string sql = "select Value from Setting where [Key] = 'ThirdMoney1' or [Key] = 'ThirdMoney2'";
            List<Setting> list = Util.ReaderToList<Setting>(sql);

            var money1 = int.Parse(list[0].Value);
            var money2 = int.Parse(list[1].Value);

            if (money < money1 || money > money2)
            {
                return false;
            }

            return true;
        }



        #endregion


        #region 返水

        public static void HandFanShui()
        {
            string sql = "select Value from Setting where [Key] = 'FanShuiForCP'";
            decimal rate = Convert.ToDecimal(SqlHelper.ExecuteScalar(sql));  //返水比例

            string time1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string time2 = DateTime.Now.ToString("yyyy-MM-dd");

            sql = "select UserId,sum(BetCount * UnitMoney) as Money from BettingRecord where TryId = '' and lType <> 3 and lType <> 4 and SubTime > '" + time1 + "' and SubTime < '" + time2 + "' group by UserId";

            List<FanShui> list = Util.ReaderToList<FanShui>(sql);

            string con = "";
            int userId = 0;

            foreach (FanShui fanShui in list)
            {
                decimal money = fanShui.Money * rate;
                string time = DateTime.Now.ToString();
                userId = fanShui.UserId;

                con = "update UserInfo set Money+=" + money + " where Id=" + userId;
                con += "insert into ComeOutRecord(UserId,Type,Money,FanShuiMoney,FanShuiRate,SubTime) values(" + userId + ",4," + fanShui.Money + "," + money + "," + rate + ",'" + time + "')";

                //盈亏
                decimal currentMoeney = GetUserMoneyById(userId) + money;
                con += Util.GetProfitLossSql(userId, 8, money, currentMoeney);

                SqlHelper.ExecuteTransaction(con);

                con = "";
            }

        }


        public static void HandFanShuiForFenZhan(int fenzhan)
        {
            string sql = "select Value from Setting where [Key] = 'FanShuiForCP'";
            decimal rate = Convert.ToDecimal(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql));  //返水比例

            string time1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string time2 = DateTime.Now.ToString("yyyy-MM-dd");

            sql = "select UserId,sum(BetCount * UnitMoney) as Money from BettingRecord where UserId > 0 and (lType < 3 or lType > 4) and SubTime > '" + time1 + "' and SubTime < '" + time2 + "' group by UserId";
            List<FanShui> list = Util.ReaderToListForFenZhan<FanShui>(fenzhan, sql);

            string con = "";
            int userId = 0;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            int fanshuiCount = 0;           //已有的

            foreach (FanShui fanShui in list)
            {
                decimal money = fanShui.Money * rate;
                string time = DateTime.Now.ToString();
                userId = fanShui.UserId;


                ////判断是否已经处理
                //sql = "select count(1) from ComeOutRecord where Type = 4 and UserId = " + userId + " and SubTime > '" + date + "'";
                //fanshuiCount = Convert.ToInt32(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql));
                //if (fanshuiCount >= 1) continue;

                con = "update UserInfo set Money+=" + money + " where Id=" + userId;
                con += "insert into ComeOutRecord(UserId,Type,Money,FanShuiMoney,FanShuiRate,SubTime) values(" + userId + ",4," + fanShui.Money + "," + money + "," + rate + ",'" + time + "')";

                //盈亏
                decimal currentMoeney = GetUserMoneyByIdForFenZhan(fenzhan, userId) + money;
                con += Util.GetProfitLossSql(userId, 8, money, currentMoeney);

                SqlHelper.ExecuteTransactionForFenZhan(fenzhan, con);

                con = "";
            }

        }

        #endregion

        #region 代理返水

        public static void HandProxyFanShui()
        {
            string sql = "select Value from Setting where [Key] = 'FanShuiForProxy'";
            decimal rate = Convert.ToDecimal(SqlHelper.ExecuteScalar(sql));  //返水比例

            string time1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string time2 = DateTime.Now.ToString("yyyy-MM-dd");

            //1.代理列表
            sql = "select Id from UserInfo where Type = 1";
            List<UserInfo> proxyList = Util.ReaderToList<UserInfo>(sql);


            int userId = 0;

            //每个代理的总投注额
            List<FanShui> fanshuiList = new List<FanShui>();
            foreach (UserInfo userInfo in proxyList)
            {
                userId = userInfo.Id;

                sql = "select sum(b.BetCount * b.UnitMoney) as Money from BettingRecord b,UserInfo u where b.UserId = u.Id and b.lType <> 3 and b.lType <> 4 and b.SubTime > '" + time1 + "' and b.SubTime < '" + time2 + "' and u.PId =" + userId;

                FanShui fanshui = Util.ReaderToModel<FanShui>(sql);
                fanshui.UserId = userId;

                fanshuiList.Add(fanshui);
            }


            //返钱
            string con = "";
            foreach (FanShui fanShui in fanshuiList)
            {

                if (fanShui.Money > 0)
                {
                    userId = fanShui.UserId;
                    decimal money = fanShui.Money * rate;
                    string time = DateTime.Now.ToString();

                    con = "update UserInfo set Money+=" + money + " where Id=" + userId;
                    con += "insert into ComeOutRecord(UserId,Type,Money,FanShuiMoney,FanShuiRate,SubTime) values(" + userId + ",5," + fanShui.Money + "," + money + "," + rate + ",'" + time + "')";


                    //盈亏
                    decimal currentMoeney = GetUserMoneyById(userId) + money;
                    con += Util.GetProfitLossSql(userId, 8, money, currentMoeney);

                    SqlHelper.ExecuteTransaction(con);

                    con = "";
                }

            }

        }

        public static void HandProxyFanShuiForFenZhan(int fenzhan)
        {
            string sql = "select Value from Setting where [Key] = 'FanShuiForProxy'";
            decimal rate = Convert.ToDecimal(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql));  //返水比例

            string time1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string time2 = DateTime.Now.ToString("yyyy-MM-dd");

            ////1.代理列表
            //if (fenzhan == 4)
            //{
            //    //银河的代理特殊处理
            //    sql = "select Id from UserInfo where Type = 1 and (ZhanChengRate is null or ZhanChengRate <= 0)";
            //}
            //else
            //{
            //    sql = "select Id from UserInfo where Type = 1";
            //}

            sql = "select Id from UserInfo where Type = 1";

            List<UserInfo> proxyList = Util.ReaderToListForFenZhan<UserInfo>(fenzhan, sql);

            int userId = 0;

            //每个代理的总投注额
            List<FanShui> fanshuiList = new List<FanShui>();
            foreach (UserInfo userInfo in proxyList)
            {
                userId = userInfo.Id;

                sql = "select sum(cast(b.BetCount * b.UnitMoney as bigint)) as Money from BettingRecord b,UserInfo u where b.UserId = u.Id and (b.lType < 3 or b.lType > 4) and b.SubTime > '" + time1 + "' and b.SubTime < '" + time2 + "' and (u.PId =" + userId + " or u.Id =" + userId + ")";

                FanShui fanshui = Util.ReaderToModelForFenZhan<FanShui>(fenzhan, sql);
                fanshui.UserId = userId;

                fanshuiList.Add(fanshui);
            }


            //返钱
            string con = "";
            foreach (FanShui fanShui in fanshuiList)
            {

                if (fanShui.Money > 0)
                {
                    userId = fanShui.UserId;
                    decimal money = fanShui.Money * rate;
                    string time = DateTime.Now.ToString();

                    con = "update UserInfo set Money+=" + money + " where Id=" + userId;
                    con += "insert into ComeOutRecord(UserId,Type,Money,FanShuiMoney,FanShuiRate,SubTime) values(" + userId + ",5," + fanShui.Money + "," + money + "," + rate + ",'" + time + "')";


                    //盈亏
                    decimal currentMoeney = GetUserMoneyByIdForFenZhan(fenzhan, userId) + money;
                    con += Util.GetProfitLossSql(userId, 8, money, currentMoeney);

                    SqlHelper.ExecuteTransactionForFenZhan(fenzhan, con);

                    con = "";
                }

            }

        }

        public static void HandProxyFanShuiForFenZhan2(int fenzhan, string time1, string time2)
        {
            string sql = "select Value from Setting where [Key] = 'FanShuiForProxy'";
            decimal rate = Convert.ToDecimal(SqlHelper.ExecuteScalarForFenZhan(fenzhan, sql));  //返水比例

            //string time1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            //string time2 = DateTime.Now.ToString("yyyy-MM-dd");

            //1.代理列表
            if (fenzhan == 4)
            {
                //银河的代理特殊处理
                sql = "select Id from UserInfo where Type = 1 and (ZhanChengRate is null or ZhanChengRate <= 0)";
            }
            else
            {
                sql = "select Id from UserInfo where Type = 1";
            }

            List<UserInfo> proxyList = Util.ReaderToListForFenZhan<UserInfo>(fenzhan, sql);

            int userId = 0;

            //每个代理的总投注额
            List<FanShui> fanshuiList = new List<FanShui>();
            foreach (UserInfo userInfo in proxyList)
            {
                userId = userInfo.Id;

                sql = "select sum(b.BetCount * b.UnitMoney) as Money from BettingRecord b,UserInfo u where b.UserId = u.Id and (b.lType < 3 or b.lType > 4) and b.SubTime > '" + time1 + "' and b.SubTime < '" + time2 + "' and u.PId =" + userId;

                FanShui fanshui = Util.ReaderToModelForFenZhan<FanShui>(fenzhan, sql);
                fanshui.UserId = userId;

                fanshuiList.Add(fanshui);
            }


            //返钱
            string con = "";
            foreach (FanShui fanShui in fanshuiList)
            {

                if (fanShui.Money > 0)
                {
                    userId = fanShui.UserId;
                    decimal money = fanShui.Money * rate;
                    string time = DateTime.Now.ToString();

                    con = "update UserInfo set Money+=" + money + " where Id=" + userId;
                    con += "insert into ComeOutRecord(UserId,Type,Money,FanShuiMoney,FanShuiRate,SubTime) values(" + userId + ",5," + fanShui.Money + "," + money + "," + rate + ",'" + time + "')";


                    //盈亏
                    decimal currentMoeney = GetUserMoneyByIdForFenZhan(fenzhan, userId) + money;
                    con += Util.GetProfitLossSql(userId, 8, money, currentMoeney);

                    SqlHelper.ExecuteTransactionForFenZhan(fenzhan, con);

                    con = "";
                }

            }

        }

        #endregion

        //查找下级id
        public static string GetAllNextIds(int pId)
        {
            string sql = "select Id from UserInfo where PId=" + pId;
            List<UserInfo> list = Util.ReaderToList<UserInfo>(sql);

            StringBuilder sb = new StringBuilder("(");

            foreach (UserInfo userInfo in list)
            {
                sb.Append(userInfo.Id + ",");
            }

            string str = sb.ToString().TrimEnd(',');

            return str + ")";
        }


        public static List<UserInfo> GetAllNextIdsList(int pId)
        {
            string sql = "select Id,UserName from UserInfo where PId=" + pId;
            List<UserInfo> list = Util.ReaderToList<UserInfo>(sql);

            return list;
        }


        public static string AddQuickTransferRecord(UserInfo LoginUser, string target)
        {

            string[] arr = { "Money", "ElecMoney", "LotteryMoney", "MayaVideoMoney", "AGVideoMoney", "HappyMoney", "MGElecMoney", "ShabaMoney" };

            string sql = "";
            int uid = LoginUser.Id;
            string time = DateTime.Now.ToString();


            if (LoginUser.Money > 0 && target != "Money")
            {
                sql += "insert into TransferRecord values(" + uid + ",'Money','" + target + "'," + LoginUser.Money + ",'" + time + "')";
            }

            if (LoginUser.ElecMoney > 0 && target != "ElecMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'ElecMoney','" + target + "'," + LoginUser.ElecMoney + ",'" + time + "')";
            }

            if (LoginUser.LotteryMoney > 0 && target != "LotteryMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'LotteryMoney','" + target + "'," + LoginUser.LotteryMoney + ",'" + time + "')";
            }

            if (LoginUser.MayaVideoMoney > 0 && target != "MayaVideoMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'MayaVideoMoney','" + target + "'," + LoginUser.MayaVideoMoney + ",'" + time + "')";
            }

            if (LoginUser.AGVideoMoney > 0 && target != "AGVideoMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'AGVideoMoney','" + target + "'," + LoginUser.AGVideoMoney + ",'" + time + "')";
            }

            if (LoginUser.HappyMoney > 0 && target != "HappyMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'HappyMoney','" + target + "'," + LoginUser.HappyMoney + ",'" + time + "')";
            }

            if (LoginUser.MGElecMoney > 0 && target != "MGElecMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'MGElecMoney','" + target + "'," + LoginUser.MGElecMoney + ",'" + time + "')";
            }

            if (LoginUser.ShabaMoney > 0 && target != "ShabaMoney")
            {
                sql += "insert into TransferRecord values(" + uid + ",'ShabaMoney','" + target + "'," + LoginUser.ShabaMoney + ",'" + time + "')";
            }

            return sql;

        }


        //获取推荐特码
        public static string GetTuijianTM(string issue)
        {
            string sql = "select BetNum from BettingRecord where lType = 4 and Issue=@Issue and (PlayName = '特码A' or PlayName='特码B')";

            SqlParameter[] pms = { new SqlParameter("@Issue", issue), };
            List<BettingRecord> list1 = Util.ReaderToListForFenZhan<BettingRecord>(1, sql, pms);

            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql, pms));
            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql, pms));
            //list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql, pms));
            //list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql, pms));
            //list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql, pms));


            List<string> data = new List<string>();
            for (int i = 1; i < 50; i++)
            {
                if (i < 10)
                {
                    data.Add("0" + i);
                }
                else
                {
                    data.Add(i.ToString());
                }
            }


            if (list1.Count == 0)
            {
                return "";
            }
            else
            {
                foreach (BettingRecord record in list1)
                {
                    if (data.Contains(record.BetNum))
                    {
                        data.Remove(record.BetNum);
                    }
                }
            }


            if (data.Count > 0)
            {
                return string.Join(" ", data.ToArray());
            }

            return "";
        }



        //获取推荐特码
        public static string GetTuijianTMForK6(string issue)
        {
            string sql = "select BetNum from BettingRecord where lType = 4 and Issue=@Issue and (PlayName = '特码A' or PlayName='特码B')";

            SqlParameter[] pms = { new SqlParameter("@Issue", issue), };
            List<BettingRecord> list1 = Util.ReaderToListForFenZhan<BettingRecord>(1, sql, pms);

            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(2, sql, pms));
            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(3, sql, pms));
            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(4, sql, pms));
            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(5, sql, pms));
            list1.AddRange(Util.ReaderToListForFenZhan<BettingRecord>(6, sql, pms));


            string[] one =
            {
                "05", "06", "07",  "02", "03", "04", "08", "09"
                , "30", "37","01", "38", "39", "10", "11", "12", "13", "14"
                , "40", "41", "42", "43", "44", "20", "21", "22", "23", "24", "25", "26", "27", "31", "32", "33"
                , "34", "35", "36", "28", "29"
                , "15", "16", "17", "18", "19", "45", "46", "47", "48", "49"
            };


            string[] two =
            {
                "46", "47", "30", "31", "36", "37", "38", "39"
                , "02", "03", "04", "05", "06", "07", "08", "09"
                , "20", "24", "25", "26", "27", "28", "29"
                , "10", "11", "12", "13", "32",  "01","33", "34", "35", "14", "15", "16", "17", "18", "19"
                , "40", "21", "22", "23", "41", "42", "43", "44", "45", "48", "49"
            };


            string[] three =
            {
                "10", "43", "44", "45", "46", "47", "48", "11", "12", "13", "14", "15", "16", "17", "18", "19"
                , "30", "31", "32", "33"
                , "06", "07", "08", "09", "20", "21", "22"
                , "40", "41", "42", "49", "34", "35", "36", "37", "38", "39"
                , "23", "24",  "02", "03", "04", "05", "25", "01","26", "27", "28", "29"
            };

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int index = ran.Next(1, 4);

            List<string> data = null;

            if (index == 1)
            {
                data = new List<string>(one);
            }
            else if (index == 2)
            {
                data = new List<string>(two);
            }
            else
            {
                data = new List<string>(three);
            }


            if (list1.Count == 0)
            {
                return "";
            }
            else
            {
                foreach (BettingRecord record in list1)
                {
                    if (data.Contains(record.BetNum))
                    {
                        data.Remove(record.BetNum);
                    }
                }
            }


            if (data.Count > 0)
            {
                return string.Join(" ", data.ToArray());
            }

            return "";
        }


        //---------------------2018---------------------

        //统一开奖-所有分站
        public static void Open(int lType, string issue, string num)
        {
            DealOpen.HandCurrentBetting(1, lType, issue, num);
            DealOpen.HandCurrentBetting(2, lType, issue, num);
            DealOpen.HandCurrentBetting(3, lType, issue, num);
            DealOpen.HandCurrentBetting(4, lType, issue, num);
            DealOpen.HandCurrentBetting(5, lType, issue, num);
            DealOpen.HandCurrentBetting(6, lType, issue, num);
        }




    }
}
