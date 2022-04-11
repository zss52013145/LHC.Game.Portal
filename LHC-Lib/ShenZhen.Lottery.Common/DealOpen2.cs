using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Common
{
    /**
     * 用于快速的杀 计算中奖金额
     * 
     */
    public class DealOpen2
    {


        //处理当前下注的结算-----------新版
        public static decimal HandCurrentBettingForNoSerch(List<BettingRecord> data, int fenzhan, int lType, string currentIssue, string openNum)//, int skipCount, int handCount)
        {
            decimal result = 0;

            #region 普通下注的

            try
            {
                //string sql = "select * from BettingRecord where UserId > 0 and Issue='" + currentIssue + "' and WinState = 1 and lType =" + lType;
                //List<BettingRecord> data = Util.ReaderToListForFenZhan<BettingRecord>(fenzhan, sql);

                if (data != null && data.Count > 0)
                {
                    //data = data.OrderByDescending(p => p.BetCount * p.UnitMoney).Take(data.Count / 2 + 1).ToList();

                    #region 遍历处理每个单子

                    foreach (BettingRecord br in data)
                    {
                        try
                        {
                            result += HandBetting(fenzhan, br, openNum);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                        }
                    }


                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message + ex.StackTrace);

            }
            #endregion

            return result;
        }




        //处理当前下注的结算-----------新版
        //新版腾讯
        public static decimal HandCurrentBettingForNoSerch2ForTXFFC(List<BettingRecord> data, int fenzhan, int lType,  string openNum)//, int skipCount, int handCount)
        {
            decimal result = 0;

            #region 普通下注的

            try
            {
                //string sql = "select * from BettingRecord where UserId > 0 and Issue='" + currentIssue + "' and WinState = 1 and lType =" + lType;
                //List<BettingRecord> data = Util.ReaderToListForFenZhan<BettingRecord>(fenzhan, sql);

                if (data != null && data.Count > 0)
                {
                    //data = data.OrderByDescending(p => p.BetCount * p.UnitMoney).Take(data.Count / 2 + 1).ToList();

                    #region 遍历处理每个单子

                    foreach (BettingRecord br in data)
                    {
                        try
                        {
                            result += HandBetting(fenzhan, br, openNum);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                        }
                    }


                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message + ex.StackTrace);

            }
            #endregion

            return result;
        }



        //处理当前下注的结算
        public static decimal HandCurrentBetting(int fenzhan, int lType, string currentIssue, string openNum)//, int skipCount, int handCount)
        {
            decimal result = 0;

            #region 普通下注的

            try
            {
                string sql = "select * from BettingRecord where UserId > 0 and Issue='" + currentIssue + "' and WinState = 1 and lType =" + lType;
                List<BettingRecord> data = Util.ReaderToListForFenZhan<BettingRecord>(fenzhan, sql);

                if (data != null && data.Count > 0)
                {
                    //data = data.OrderByDescending(p => p.BetCount * p.UnitMoney).Take(data.Count / 2 + 1).ToList();

                    #region 遍历处理每个单子

                    foreach (BettingRecord br in data)
                    {
                        try
                        {
                            result += HandBetting(fenzhan, br, openNum);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                        }
                    }


                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message + ex.StackTrace);

            }
            #endregion

            return result;
        }

        //具体开奖算法
        public static decimal HandBetting(int fenzhan, BettingRecord br, string openNum)
        {

            //---------------------------------

            int winCount = 0;
            int winCount1 = 0;
            int winCount2 = 0;
            int lType = br.lType;
            string playName = br.PlayName;
            string betNum = br.BetNum;
            string[] betArr = betNum.Split(',');
            string[] openArr = openNum.Split(',');

            int sum28 = 0;

            //---------------------------------

            #region 判断输赢

            #region 重庆时时彩

            if (lType == 1 || lType == 2 || lType == 61)             //------重庆时时彩
            {
                int a = int.Parse(openArr[0]);
                int b = int.Parse(openArr[1]);
                int c = int.Parse(openArr[2]);
                int d = int.Parse(openArr[3]);
                int e = int.Parse(openArr[4]);

                if (string.IsNullOrEmpty(playName))
                {
                    int sum = a + b + c + d + e;

                    if (betNum == "总和大")
                    {
                        if (sum >= 23)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和小")
                    {
                        if (sum <= 22)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和单")
                    {
                        if (sum % 2 != 0)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和双")
                    {
                        if (sum % 2 == 0)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "龙")
                    {
                        if (a > e)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "虎")
                    {
                        if (a < e)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "和")
                    {
                        if (a == e)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第一球")
                {
                    if (betNum == "大")
                    {
                        if (a > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (a <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (a % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (a % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == a)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第二球")
                {
                    if (betNum == "大")
                    {
                        if (b > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (b <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (b % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (b % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == b)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第三球")
                {
                    if (betNum == "大")
                    {
                        if (c > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (c <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (c % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (c % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == c)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第四球")
                {
                    if (betNum == "大")
                    {
                        if (d > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (d <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (d % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (d % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == d)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第五球")
                {
                    if (betNum == "大")
                    {
                        if (e > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (e <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (e % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (e % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == e)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "前三")
                {
                    if (betNum == "豹子")
                    {
                        if (Util.IsBaoZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "对子")
                    {
                        if (Util.IsDuiZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "顺子")
                    {
                        if (Util.IsShunZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "半顺")
                    {
                        if (Util.IsBanShun(a, b, c)) winCount++;
                    }
                    else if (betNum == "杂六")
                    {
                        if (Util.IsZaLiu(a, b, c)) winCount++;
                    }
                }
                else if (playName == "中三")
                {
                    if (betNum == "豹子")
                    {
                        if (Util.IsBaoZi(b, c, d)) winCount++;
                    }
                    else if (betNum == "对子")
                    {
                        if (Util.IsDuiZi(b, c, d)) winCount++;
                    }
                    else if (betNum == "顺子")
                    {
                        if (Util.IsShunZi(b, c, d)) winCount++;
                    }
                    else if (betNum == "半顺")
                    {
                        if (Util.IsBanShun(b, c, d)) winCount++;
                    }
                    else if (betNum == "杂六")
                    {
                        if (Util.IsZaLiu(b, c, d)) winCount++;
                    }
                }
                else if (playName == "后三")
                {
                    if (betNum == "豹子")
                    {
                        if (Util.IsBaoZi(c, d, e)) winCount++;
                    }
                    else if (betNum == "对子")
                    {
                        if (Util.IsDuiZi(c, d, e)) winCount++;
                    }
                    else if (betNum == "顺子")
                    {
                        if (Util.IsShunZi(c, d, e)) winCount++;
                    }
                    else if (betNum == "半顺")
                    {
                        if (Util.IsBanShun(c, d, e)) winCount++;
                    }
                    else if (betNum == "杂六")
                    {
                        if (Util.IsZaLiu(c, d, e)) winCount++;
                    }
                }
            }

            #endregion

            #region 六合彩

            if (lType == 3 || lType == 4)
            {
                if (playName == "特码A" || playName == "特码B")
                {
                    int tema = int.Parse(openArr[6]);
                    int temahe = int.Parse(openArr[6].Substring(0, 1)) + int.Parse(openArr[6].Substring(1, 1));
                    int wei = int.Parse(openArr[6].Substring(1, 1));

                    if (betNum == "1-10")
                    {
                        if (tema >= 1 && tema <= 10) winCount++;
                    }
                    else if (betNum == "11-20")
                    {
                        if (tema >= 11 && tema <= 20) winCount++;
                    }
                    else if (betNum == "21-30")
                    {
                        if (tema >= 21 && tema <= 30) winCount++;
                    }
                    else if (betNum == "31-40")
                    {
                        if (tema >= 31 && tema <= 40) winCount++;
                    }
                    else if (betNum == "41-49")
                    {
                        if (tema >= 41 && tema <= 49) winCount++;
                    }
                    else if (tema == 49 && (betNum == "大" || betNum == "小" || betNum == "单" || betNum == "双" || betNum == "合单" || betNum == "合双" || betNum == "尾大" || betNum == "尾小"))
                    {
                        winCount = -1;  //和局
                    }
                    else if (betNum == "单")
                    {
                        if (tema % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (tema % 2 == 0) winCount++;
                    }
                    else if (betNum == "大")
                    {
                        if (tema >= 25) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (tema <= 24) winCount++;
                    }
                    else if (betNum == "合单")
                    {
                        if (temahe % 2 != 0) winCount++;
                    }
                    else if (betNum == "合双")
                    {
                        if (temahe % 2 == 0) winCount++;
                    }
                    else if (betNum == "家禽")
                    {
                        string shengxiao = Util.GetShengxiaoByDigit(tema);
                        if (Util.IsJiaQin(shengxiao))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "野兽")
                    {
                        string shengxiao = Util.GetShengxiaoByDigit(tema);
                        if (!Util.IsJiaQin(shengxiao))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "尾大")
                    {
                        if (wei > 4) winCount++;
                    }
                    else if (betNum == "尾小")
                    {
                        if (wei <= 4) winCount++;
                    }
                    else if (betNum == "红波")
                    {
                        if (Util.GetColor(openArr[6]) == "red") winCount++;
                    }
                    else if (betNum == "绿波")
                    {
                        if (Util.GetColor(openArr[6]) == "green") winCount++;
                    }
                    else if (betNum == "蓝波")
                    {
                        if (Util.GetColor(openArr[6]) == "blue") winCount++;
                    }
                    else
                    {
                        //数字01-49
                        if (betNum == openArr[6])
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "正A" || playName == "正B")
                {
                    int zhengmahe = int.Parse(openArr[0]) + int.Parse(openArr[1]) + int.Parse(openArr[2]) +
                                    int.Parse(openArr[3]) + int.Parse(openArr[4]) + int.Parse(openArr[5]) + int.Parse(openArr[6]);


                    string qianliu = openArr[0] + "," + openArr[1] + "," + openArr[2] + "," + openArr[3] + "," + openArr[4] + "," + openArr[5];

                    string zmh = zhengmahe.ToString();

                    int wei = int.Parse(zmh.Substring(zmh.Length - 1, 1));


                    if (betNum == "总大")
                    {
                        if (zhengmahe >= 175) winCount++;
                    }
                    else if (betNum == "总小")
                    {
                        if (zhengmahe <= 174) winCount++;
                    }
                    else if (betNum == "总单")
                    {
                        if (zhengmahe % 2 != 0) winCount++;
                    }
                    else if (betNum == "总双")
                    {
                        if (zhengmahe % 2 == 0) winCount++;
                    }
                    else if (betNum == "总尾大")
                    {
                        if (wei > 4) winCount++;
                    }
                    else if (betNum == "总尾小")
                    {
                        if (wei <= 4) winCount++;
                    }
                    else if (betNum == "龙")
                    {
                        if (int.Parse(openArr[0]) > int.Parse(openArr[6])) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (int.Parse(openArr[0]) < int.Parse(openArr[6])) winCount++;
                    }
                    else
                    {
                        //数字01-49
                        if (qianliu.Contains(betNum)) winCount++;
                    }
                }
                else if (playName == "正码一" || playName == "正1特")
                {
                    winCount = JudgeZM1To6IsWin(betNum, openArr[0]);
                }
                else if (playName == "正码二" || playName == "正2特")
                {
                    winCount = JudgeZM1To6IsWin(betNum, openArr[1]);
                }
                else if (playName == "正码三" || playName == "正3特")
                {
                    winCount = JudgeZM1To6IsWin(betNum, openArr[2]);
                }
                else if (playName == "正码四" || playName == "正4特")
                {
                    winCount = JudgeZM1To6IsWin(betNum, openArr[3]);
                }
                else if (playName == "正码五" || playName == "正5特")
                {
                    winCount = JudgeZM1To6IsWin(betNum, openArr[4]);
                }
                else if (playName == "正码六" || playName == "正6特")
                {
                    winCount = JudgeZM1To6IsWin(betNum, openArr[5]);
                }
                else if (playName == "尾数")
                {
                    string num = betNum.Substring(0, 1);

                    foreach (string s in openArr)
                    {
                        if (s.EndsWith(num))
                        {
                            winCount++;
                            break;
                        }
                    }
                }
                else if (playName == "一肖")
                {
                    winCount = JudegeYiXiaoisWin(betNum, openArr);
                }
                else if (playName == "特肖")
                {
                    winCount = JudegeTeXiaoisWin(betNum, openArr[6]);
                }
                else if (playName == "六肖连中")
                {
                    int tema = int.Parse(openArr[6]);

                    if (tema == 49)
                    {
                        winCount = -1;              //和局
                    }
                    else
                    {
                        string shengxiao = Util.GetShengxiaoByDigit(tema);
                        if (betNum.Contains(shengxiao)) winCount++;
                    }
                }
                else if (playName == "六肖连不中")
                {
                    int tema = int.Parse(openArr[6]);

                    if (tema == 49)
                    {
                        winCount = -1;              //和局
                    }
                    else
                    {
                        string shengxiao = Util.GetShengxiaoByDigit(tema);
                        if (!betNum.Contains(shengxiao)) winCount++;
                    }
                }
                else if (playName == "1-2球")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[1]);

                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "1-3球")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[2]);

                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "1-3球")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[2]);

                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "1-4球")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[3]);

                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "1-5球")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[4]);

                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "1-6球")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[5]);

                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "2-3球")
                {
                    int a = int.Parse(openArr[1]);
                    int b = int.Parse(openArr[2]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "2-4球")
                {
                    int a = int.Parse(openArr[1]);
                    int b = int.Parse(openArr[3]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "2-5球")
                {
                    int a = int.Parse(openArr[1]);
                    int b = int.Parse(openArr[4]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "2-6球")
                {
                    int a = int.Parse(openArr[1]);
                    int b = int.Parse(openArr[5]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "3-4球")
                {
                    int a = int.Parse(openArr[2]);
                    int b = int.Parse(openArr[3]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "3-5球")
                {
                    int a = int.Parse(openArr[2]);
                    int b = int.Parse(openArr[4]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "3-6球")
                {
                    int a = int.Parse(openArr[2]);
                    int b = int.Parse(openArr[5]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "4-5球")
                {
                    int a = int.Parse(openArr[3]);
                    int b = int.Parse(openArr[4]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "4-6球")
                {
                    int a = int.Parse(openArr[3]);
                    int b = int.Parse(openArr[5]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "5-6球")
                {
                    int a = int.Parse(openArr[4]);
                    int b = int.Parse(openArr[5]);
                    winCount = JudgeLongHu(betNum, a, b);
                }
                else if (playName == "半波")
                {
                    string color = "";
                    string daxiao = "";
                    string danshuang = "";

                    string tema = openArr[6];
                    int tm = int.Parse(tema);

                    if (tm == 49)
                    {
                        winCount = -1; //和局
                    }
                    else
                    {
                        if (Util.GetColor(tema) == "red")
                        {
                            color = "红";
                        }
                        else if (Util.GetColor(tema) == "blue")
                        {
                            color = "蓝";
                        }
                        else
                        {
                            color = "绿";
                        }

                        daxiao = tm >= 25 ? "大" : "小";
                        danshuang = tm % 2 == 0 ? "双" : "单";

                        int he = int.Parse(tema.Substring(0, 1)) + int.Parse(tema.Substring(1, 1));
                        string hedanshuang = he % 2 == 0 ? "双" : "单";

                        string xingtai = color + daxiao + "|" + color + danshuang + "|" + color + "合" + hedanshuang;

                        if (xingtai.Contains(betNum)) winCount++;
                    }



                }
                else if (playName == "三全中")
                {
                    int count = 0;
                    for (int i = 0; i < openArr.Length - 1; i++)
                    {
                        if (betNum.Contains(openArr[i])) count++;
                    }

                    if (count >= 3)
                    {
                        winCount = JieCheng(count) / (JieCheng(count - 3) * JieCheng(3));
                    }
                }
                else if (playName == "三中二") //特殊情况
                {
                    int count = 0;
                    for (int i = 0; i < openArr.Length - 1; i++)
                    {
                        if (betNum.Contains(openArr[i])) count++;
                    }

                    if (count == 2)
                    {
                        winCount2 = betArr.Length - count;
                    }
                    else if (count >= 3)
                    {
                        winCount1 = JieCheng(count) / (JieCheng(count - 3) * JieCheng(3));
                        winCount2 = (JieCheng(count) / (JieCheng(count - 2) * JieCheng(2))) * (betArr.Length - count);
                    }

                    winCount = winCount1 + winCount2; //
                }
                else if (playName == "二全中")
                {
                    int count = 0;
                    for (int i = 0; i < openArr.Length - 1; i++)
                    {
                        if (betNum.Contains(openArr[i])) count++;
                    }

                    if (count >= 2)
                    {
                        winCount = JieCheng(count) / (JieCheng(count - 2) * JieCheng(2));
                    }
                }
                else if (playName == "二中特")
                {
                    int count = 0;
                    for (int i = 0; i < openArr.Length - 1; i++)
                    {
                        if (betNum.Contains(openArr[i])) count++;
                    }

                    int count2 = betNum.Contains(openArr[6]) ? 1 : 0;

                    if (count == 1 && count2 == 1)
                    {
                        winCount2 = 1;
                    }
                    else if (count >= 2)
                    {

                        winCount1 = JieCheng(count) / (JieCheng(count - 2) * JieCheng(2));

                        if (count2 == 1)
                        {
                            winCount2 = count;
                        }
                    }

                    winCount = winCount1 + winCount2;

                }
                else if (playName == "特串")
                {
                    if (betNum.Contains(openArr[6])) //包含特码
                    {
                        //包含一个正码
                        for (int i = 0; i < openArr.Length - 1; i++)
                        {
                            if (betNum.Contains(openArr[i]))
                            {
                                winCount++;
                                break;
                            }
                        }
                    }
                }
                else if (playName == "五不中")
                {
                    winCount = BuZhong(5, betArr, openNum);
                }
                else if (playName == "六不中")
                {
                    winCount = BuZhong(6, betArr, openNum);
                }
                else if (playName == "七不中")
                {
                    winCount = BuZhong(7, betArr, openNum);
                }
                else if (playName == "八不中")
                {
                    winCount = BuZhong(8, betArr, openNum);
                }
                else if (playName == "九不中")
                {
                    winCount = BuZhong(9, betArr, openNum);
                }
                else if (playName == "十不中")
                {
                    winCount = BuZhong(10, betArr, openNum);
                }
                else if (playName == "二连肖(中)")
                {
                    winCount = LianXiao(2, betArr, openNum, true);
                }
                else if (playName == "三连肖(中)")
                {
                    winCount = LianXiao(3, betArr, openNum, true);
                }
                else if (playName == "四连肖(中)")
                {
                    winCount = LianXiao(4, betArr, openNum, true);
                }
                else if (playName == "二连肖(不中)")
                {
                    winCount = LianXiao(2, betArr, openNum, false);
                }
                else if (playName == "三连肖(不中)")
                {
                    winCount = LianXiao(3, betArr, openNum, false);
                }
                else if (playName == "四连肖(不中)")
                {
                    winCount = LianXiao(4, betArr, openNum, false);
                }
                else if (playName == "二尾连(中)")
                {
                    winCount = LianWei(2, betArr, openNum, true);
                }
                else if (playName == "三尾连(中)")
                {
                    winCount = LianWei(3, betArr, openNum, true);
                }
                else if (playName == "四尾连(中)")
                {
                    winCount = LianWei(4, betArr, openNum, true);
                }
                else if (playName == "二尾连(不中)")
                {
                    winCount = LianWei(2, betArr, openNum, false);
                }
                else if (playName == "三尾连(不中)")
                {
                    winCount = LianWei(3, betArr, openNum, false);
                }
                else if (playName == "四尾连(不中)")
                {
                    winCount = LianWei(4, betArr, openNum, false);
                }
            }

            #endregion

            #region 七星彩

            if (lType == 5 || lType == 6)
            {
                if (playName == "定千位")
                {

                    int qian = int.Parse(openArr[0]);

                    if (betNum == openArr[0])
                    {
                        winCount++;
                    }
                    else if (betNum == "大" && qian > 4)
                    {
                        winCount++;
                    }
                    else if (betNum == "小" && qian < 5)
                    {
                        winCount++;
                    }
                    else if (betNum == "单" && qian % 2 != 0)
                    {
                        winCount++;
                    }
                    else if (betNum == "双" && qian % 2 == 0)
                    {
                        winCount++;
                    }
                }
                else if (playName == "定百位")
                {
                    int bai = int.Parse(openArr[1]);

                    if (betNum == openArr[1])
                    {
                        winCount++;
                    }
                    else if (betNum == "大" && bai > 4)
                    {
                        winCount++;
                    }
                    else if (betNum == "小" && bai < 5)
                    {
                        winCount++;
                    }
                    else if (betNum == "单" && bai % 2 != 0)
                    {
                        winCount++;
                    }
                    else if (betNum == "双" && bai % 2 == 0)
                    {
                        winCount++;
                    }
                }
                else if (playName == "定十位")
                {
                    int shi = int.Parse(openArr[2]);

                    if (betNum == openArr[2])
                    {
                        winCount++;
                    }
                    else if (betNum == "大" && shi > 4)
                    {
                        winCount++;
                    }
                    else if (betNum == "小" && shi < 5)
                    {
                        winCount++;
                    }
                    else if (betNum == "单" && shi % 2 != 0)
                    {
                        winCount++;
                    }
                    else if (betNum == "双" && shi % 2 == 0)
                    {
                        winCount++;
                    }
                }
                else if (playName == "定个位")
                {
                    int ge = int.Parse(openArr[3]);

                    if (betNum == openArr[3])
                    {
                        winCount++;
                    }
                    else if (betNum == "大" && ge > 4)
                    {
                        winCount++;
                    }
                    else if (betNum == "小" && ge < 5)
                    {
                        winCount++;
                    }
                    else if (betNum == "单" && ge % 2 != 0)
                    {
                        winCount++;
                    }
                    else if (betNum == "双" && ge % 2 == 0)
                    {
                        winCount++;
                    }
                }
                else
                {
                    betArr = betNum.Split('#');

                    if (playName == "千##个")
                    {
                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "#百十#")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[1]) && betArr[1].Contains(openArr[2]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "千#十#")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[2]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "#百#个")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[1]) && betArr[1].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "千百##")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[1]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "##十个")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[2]) && betArr[1].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "千百十#")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[1]) && betArr[2].Contains(openArr[2]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "千百#个")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[1]) && betArr[2].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "#百十个")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[1]) && betArr[1].Contains(openArr[2]) && betArr[2].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "千#十个")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[2]) && betArr[2].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }
                    else if (playName == "四定位")
                    {
                        betArr = betNum.Split('#');

                        if (betArr[0].Contains(openArr[0]) && betArr[1].Contains(openArr[1]) && betArr[2].Contains(openArr[2]) && betArr[3].Contains(openArr[3]))
                        {
                            winCount++;
                        }
                    }

                }

            }

            #endregion

            #region 北京PK10 幸运飞艇

            if (lType == 7 || lType == 8 || lType == 62 || lType == 9 || lType == 10)
            {

                if (string.IsNullOrEmpty(playName))
                {
                    int guanyahe = int.Parse(openArr[0]) + int.Parse(openArr[1]);

                    if (betNum == "冠亚大")
                    {
                        if (guanyahe > 11) winCount++;
                    }
                    else if (betNum == "冠亚小")
                    {
                        if (guanyahe <= 11) winCount++;
                    }
                    else if (betNum == "冠亚单")
                    {
                        if (guanyahe % 2 != 0) winCount++;
                    }
                    else if (betNum == "冠亚双")
                    {
                        if (guanyahe % 2 == 0) winCount++;
                    }
                }
                else if (playName == "冠亚和")
                {
                    int guanyahe = int.Parse(openArr[0]) + int.Parse(openArr[1]);
                    winCount = DaXiaoDanShuang(guanyahe, betNum);
                }
                else if (playName == "冠军")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[9]);

                    if (betNum == "龙")
                    {
                        if (a > b) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (a < b) winCount++;
                    }
                    else
                    {
                        winCount = DaXiaoDanShuang(a, betNum);
                    }
                }
                else if (playName == "亚军")
                {
                    int a = int.Parse(openArr[1]);
                    int b = int.Parse(openArr[8]);

                    if (betNum == "龙")
                    {
                        if (a > b) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (a < b) winCount++;
                    }
                    else
                    {
                        winCount = DaXiaoDanShuang(a, betNum);
                    }
                }
                else if (playName == "第三名")
                {
                    int a = int.Parse(openArr[2]);
                    int b = int.Parse(openArr[7]);

                    if (betNum == "龙")
                    {
                        if (a > b) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (a < b) winCount++;
                    }
                    else
                    {
                        winCount = DaXiaoDanShuang(a, betNum);
                    }
                }
                else if (playName == "第四名")
                {
                    int a = int.Parse(openArr[3]);
                    int b = int.Parse(openArr[6]);

                    if (betNum == "龙")
                    {
                        if (a > b) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (a < b) winCount++;
                    }
                    else
                    {
                        winCount = DaXiaoDanShuang(a, betNum);
                    }
                }
                else if (playName == "第五名")
                {
                    int a = int.Parse(openArr[4]);
                    int b = int.Parse(openArr[5]);

                    if (betNum == "龙")
                    {
                        if (a > b) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (a < b) winCount++;
                    }
                    else
                    {
                        winCount = DaXiaoDanShuang(a, betNum);
                    }
                }
                else if (playName == "第六名")
                {
                    int a = int.Parse(openArr[5]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第七名")
                {
                    int a = int.Parse(openArr[6]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第八名")
                {
                    int a = int.Parse(openArr[7]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第九名")
                {
                    int a = int.Parse(openArr[8]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第十名")
                {
                    int a = int.Parse(openArr[9]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }

            }
            #endregion

            #region 3D 排三

            if (lType == 11 || lType == 12 || lType == 19 || lType == 20)
            {

                int a = int.Parse(openArr[0]);
                int b = int.Parse(openArr[1]);
                int c = int.Parse(openArr[2]);

                if (string.IsNullOrEmpty(playName))
                {
                    int sum = a + b + c;

                    if (betNum == "总和大")
                    {
                        if (sum >= 14)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和小")
                    {
                        if (sum < 14)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和单")
                    {
                        if (sum % 2 != 0)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和双")
                    {
                        if (sum % 2 == 0)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "龙")
                    {
                        if (a > c)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "虎")
                    {
                        if (a < c)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "和")
                    {
                        if (a == c)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第一球")
                {
                    if (betNum == "大")
                    {
                        if (a > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (a <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (a % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (a % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == a)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第二球")
                {
                    if (betNum == "大")
                    {
                        if (b > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (b <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (b % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (b % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == b)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "第三球")
                {
                    if (betNum == "大")
                    {
                        if (c > 4) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (c <= 4) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (c % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (c % 2 == 0) winCount++;
                    }
                    else
                    {
                        //0-9
                        if (int.Parse(betNum) == c)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "豹顺对")
                {
                    if (betNum == "豹子")
                    {
                        if (Util.IsBaoZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "顺子")
                    {
                        if (Util.IsShunZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "对子")
                    {
                        if (Util.IsDuiZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "半顺")
                    {
                        if (Util.IsBanShun(a, b, c)) winCount++;
                    }
                    else if (betNum == "杂六")
                    {
                        if (Util.IsZaLiu(a, b, c)) winCount++;
                    }
                }
                else if (playName == "和值")
                {
                    if (int.Parse(betNum) == (a + b + c)) winCount++;
                }
            }

            #endregion

            #region 江苏快三

            if (lType == 21 || lType == 22)
            {

                int a = int.Parse(openArr[0]);
                int b = int.Parse(openArr[1]);
                int c = int.Parse(openArr[2]);

                if (string.IsNullOrEmpty(playName))
                {


                    if (betNum == "任意豹子")
                    {
                        if (a == b && b == c && a == c)
                        {
                            winCount++;
                        }
                    }
                }
                else if (playName == "和值")
                {
                    int sum = a + b + c;

                    if (betNum == "大" || betNum == "小" || betNum == "单" || betNum == "双")
                    {
                        if (a != b || b != c || a != c)   //非豹子
                        {
                            if (betNum == "大")
                            {
                                if (sum >= 11 && sum <= 17) winCount++;
                            }
                            else if (betNum == "小")
                            {
                                if (sum >= 4 && sum <= 10) winCount++;
                            }
                            else if (betNum == "单")
                            {
                                if (sum % 2 != 0) winCount++;
                            }
                            else if (betNum == "双")
                            {
                                if (sum % 2 == 0) winCount++;
                            }
                        }
                    }

                    else
                    {
                        if (int.Parse(betNum) == sum) winCount++;
                    }
                }
                else if (playName == "独胆")
                {
                    if (openArr.Contains(betNum)) winCount++;
                }
                else if (playName == "豹子")
                {
                    if (betNum == openNum) winCount++;
                }
                else if (playName == "两连")
                {
                    if (openArr.Contains(betArr[0]) && openArr.Contains(betArr[1])) winCount++;
                }
                else if (playName == "对子")
                {
                    string num = betArr[0];
                    if (openNum.IndexOf(num) != openNum.LastIndexOf(num)) winCount++;
                }

            }

            #endregion

            #region 广东快乐十分 幸运农场

            if (lType == 13 || lType == 14 || lType == 17)
            {
                if (string.IsNullOrEmpty(playName))
                {
                    int he = int.Parse(openArr[0]) + int.Parse(openArr[1]) + int.Parse(openArr[2]) +
                             int.Parse(openArr[3]) + int.Parse(openArr[4]) + int.Parse(openArr[5]) +
                             int.Parse(openArr[6]) + int.Parse(openArr[7]);

                    string he2 = he.ToString();
                    int wei = int.Parse(he2.Substring(he2.Length - 1));

                    if (betNum == "总和大")
                    {
                        if (he >= 85 && he <= 132)
                        {
                            winCount++;
                        }
                        else if (he == 84)
                        {
                            winCount = -1;              //和局
                        }
                    }
                    else if (betNum == "总和小")
                    {
                        if (he >= 36 && he <= 83)
                        {
                            winCount++;
                        }
                        else if (he == 84)
                        {
                            winCount = -1;              //和局
                        }
                    }
                    else if (betNum == "总和单")
                    {
                        if (he % 2 != 0) winCount++;
                    }
                    else if (betNum == "总和双")
                    {
                        if (he % 2 == 0) winCount++;
                    }
                    else if (betNum == "总尾大")
                    {
                        if (wei >= 5) winCount++;
                    }
                    else if (betNum == "总尾小")
                    {
                        if (wei <= 5) winCount++;
                    }
                }
                else if (playName == "第一球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[0], betNum, int.Parse(openArr[7]));
                }
                else if (playName == "第二球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[1], betNum, int.Parse(openArr[6]));
                }
                else if (playName == "第三球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[2], betNum, int.Parse(openArr[5]));
                }
                else if (playName == "第四球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[3], betNum, int.Parse(openArr[4]));
                }
                else if (playName == "第五球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[4], betNum, int.Parse(openArr[4]));
                }
                else if (playName == "第六球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[5], betNum, int.Parse(openArr[4]));
                }
                else if (playName == "第七球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[6], betNum, int.Parse(openArr[4]));
                }
                else if (playName == "第八球")
                {
                    winCount = DaXiaoDanShuangForKuaiLe10Fen(openArr[7], betNum, int.Parse(openArr[4]));
                }
                else if (playName == "任选二")
                {
                    winCount = LianMa(2, openArr, betArr);
                }
                else if (playName == "任选二组")
                {
                    winCount = RenXuanErZu(openArr, betArr);
                }
                else if (playName == "任选三")
                {
                    winCount = LianMa(3, openArr, betArr);
                }
                else if (playName == "任选四")
                {
                    winCount = LianMa(4, openArr, betArr);
                }
                else if (playName == "任选五")
                {
                    winCount = LianMa(5, openArr, betArr);
                }
            }

            #endregion

            #region 广东11X5

            if (lType == 15 || lType == 16)
            {
                if (string.IsNullOrEmpty(playName))
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[4]);

                    int he = int.Parse(openArr[0]) + int.Parse(openArr[1]) + int.Parse(openArr[2]) +
                             int.Parse(openArr[3]) + int.Parse(openArr[4]);



                    if (betNum == "总和大")
                    {
                        if (he >= 31) winCount++;
                    }
                    else if (betNum == "总和小")
                    {
                        if (he <= 29) winCount++;
                    }
                    else if (betNum == "总和单")
                    {
                        if (he % 2 != 0) winCount++;
                    }
                    else if (betNum == "总和双")
                    {
                        if (he % 2 == 0) winCount++;
                    }
                    else if (betNum == "总和30")
                    {
                        if (he == 30) winCount++;
                    }
                    else if (betNum == "龙")
                    {
                        if (a > b) winCount++;
                    }
                    else if (betNum == "虎")
                    {
                        if (a < b) winCount++;
                    }
                }
                else if (playName == "第一球")
                {
                    int a = int.Parse(openArr[0]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第二球")
                {
                    int a = int.Parse(openArr[1]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第三球")
                {
                    int a = int.Parse(openArr[2]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第四球")
                {
                    int a = int.Parse(openArr[3]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "第五球")
                {
                    int a = int.Parse(openArr[4]);
                    winCount = DaXiaoDanShuang(a, betNum);
                }
                else if (playName == "前三")
                {
                    int a = int.Parse(openArr[0]);
                    int b = int.Parse(openArr[1]);
                    int c = int.Parse(openArr[2]);

                    winCount = QianZhongHou11X5(betNum, a, b, c);
                }
                else if (playName == "中三")
                {
                    int a = int.Parse(openArr[1]);
                    int b = int.Parse(openArr[2]);
                    int c = int.Parse(openArr[3]);

                    winCount = QianZhongHou11X5(betNum, a, b, c);
                }
                else if (playName == "后三")
                {
                    int a = int.Parse(openArr[2]);
                    int b = int.Parse(openArr[3]);
                    int c = int.Parse(openArr[4]);

                    winCount = QianZhongHou11X5(betNum, a, b, c);
                }

            }

            #endregion

            #region 加拿大28

            if (lType == 23 || lType == 63 || lType == 24 || lType == 64 || lType == 84)
            {

                int a = int.Parse(openArr[0]);
                int b = int.Parse(openArr[1]);
                int c = int.Parse(openArr[2]);

                if (string.IsNullOrEmpty(playName) || playName == "混合")
                {
                    int sum = a + b + c;

                    sum28 = sum;

                    if (betNum == "总和大" || betNum == "大")
                    {
                        if (sum >= 14)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和小" || betNum == "小")
                    {
                        if (sum < 14)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和单" || betNum == "单")
                    {
                        if (sum % 2 != 0)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "总和双" || betNum == "双")
                    {
                        if (sum % 2 == 0)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "大单")
                    {
                        int[] arr = { 15, 17, 19, 21, 23, 25, 27 };

                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "大双")
                    {
                        int[] arr = { 14, 16, 18, 20, 22, 24, 26 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "小单")
                    {
                        int[] arr = { 1, 3, 5, 7, 9, 11, 13 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "小双")
                    {
                        int[] arr = { 0, 2, 4, 6, 8, 10, 12 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "红波")
                    {
                        int[] arr = { 1, 4, 7, 10, 15, 18, 21, 24 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "绿波")
                    {
                        int[] arr = { 3, 6, 9, 12, 17, 20, 23, 26 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "蓝波")
                    {
                        int[] arr = { 2, 5, 8, 11, 16, 19, 22, 25 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "极大")
                    {
                        int[] arr = { 22, 23, 24, 25, 26, 27 };
                        if (arr.Contains(sum))
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "极小")
                    {
                        if (sum < 6)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "豹子")
                    {
                        if (a == b && b == c && a == c)
                        {
                            winCount++;
                        }
                    }
                    else if (betNum == "顺子")
                    {
                        if (Util.IsShunZi(a, b, c)) winCount++;
                    }
                    else if (betNum == "对子")
                    {
                        if (Util.IsDuiZi(a, b, c)) winCount++;
                    }

                }
                else if (playName == "特码")
                {
                    if (int.Parse(betNum) == (a + b + c)) winCount++;
                }
            }

            #endregion


            #endregion



            //根据winCount做总处理****************************************

            decimal winMoney = 0;
            int winState = 1;
            decimal peilv = br.Peilv;

            UserInfo user = null;
            string sql = "";

            if (br.UserId == 0)
            {
                user = (UserInfo)CacheHelper.GetCache(br.TryId);     //试玩用户
            }
            else
            {
                user = Util.GetEntityByIdForFenZhan<UserInfo>(fenzhan, br.UserId);  //真实玩家
            }


            //处理开奖
            if (winCount == -1)
            {
                winState = 5;
            }
            else if (winCount == 0)
            {
                //没中奖
                winState = 4;
            }
            else
            {
                //LogHelper.WriteLog("---------------WINsTATE = 3 ----------");

                //中奖了
                winState = 3;

                winMoney = winCount * peilv * br.UnitMoney;





                #region 赔率特殊情况

                if (lType == 3 || lType == 4)
                {
                    if (playName == "三中二" || playName == "二中特")
                    {

                        decimal peilv1 = 0;
                        decimal peilv2 = 0;

                        if (playName == "三中二")
                        {
                            peilv1 = decimal.Parse(br.Peilv.ToString().Substring(0, 3));
                            peilv2 = decimal.Parse(br.Peilv.ToString().Substring(3, 2));
                        }
                        else if (playName == "二中特")
                        {
                            peilv1 = decimal.Parse(br.Peilv.ToString().Substring(0, 2));
                            peilv2 = decimal.Parse(br.Peilv.ToString().Substring(2, 2));
                        }

                        winMoney = winCount1 * peilv1 * br.UnitMoney + winCount2 * peilv2 * br.UnitMoney;
                    }

                }

                #endregion

                //派奖
                //Common.UpdateLotteryMoney(user, winMoney);

                if (Common.IsTryUser(user))
                {
                    //user.Money += winMoney;
                    //Common.UpdateCacheUser(user);
                }
                else
                {
                    sql += "update UserInfo set Money+=" + winMoney + " where Id=" + user.Id;
                }
            }

            //更新单子状态
            sql += "update BettingRecord set WinState = " + winState + ",WinCount = " + winCount + ",WinMoney = " + winMoney + " where Id = " + br.Id;

            decimal tuishui = 0;

            //特殊情况-六合彩特码B退水
            if (playName == "特码B")
            {
                //退水
                string seaSql = "select Value from Setting where [Key] ='TuiShuiForLHC'";
                decimal tuishuiRate = Convert.ToDecimal(SqlHelper.ExecuteScalar(seaSql));

                tuishui += (decimal)(br.BetCount * br.UnitMoney * tuishuiRate);
                sql += "update BettingRecord set TuiShui = " + tuishui + " where Id = " + br.Id;


                //更新彩票余额
                //Common.UpdateLotteryMoney(user, tuishui);


                if (Common.IsTryUser(user))
                {
                    user.Money += tuishui;
                    Common.UpdateCacheUser(user);
                }
                else
                {
                    sql += "update UserInfo set Money+=" + tuishui + " where Id=" + user.Id;
                }


            }


            //特殊情况-打和
            if (winCount == -1)
            {
                //退还本金
                decimal betMoney = (decimal)(br.BetCount * br.UnitMoney);

                tuishui += betMoney;

                //更新彩票余额
                //Common.UpdateLotteryMoney(user, betMoney);

                if (Common.IsTryUser(user))
                {
                    user.Money += betMoney;
                    Common.UpdateCacheUser(user);
                }
                else
                {
                    sql += "update UserInfo set Money+=" + betMoney + " where Id=" + user.Id;
                }

                sql += "update BettingRecord set TuiShui=" + betMoney + " where Id = " + br.Id;

            }

            decimal tempMoney = winMoney + tuishui;

            return tempMoney;


        }


        //-------------------------公共方法-----------------

        //49连码
        public static int SanZhongEr(string[] betArr, string openNum)
        {
            int count = 0;
            foreach (string s in betArr)
            {
                if (openNum.Contains(s))
                {
                    count++;
                }
            }
            return count;
        }


        public static bool ErZhongTe(string betNum, string[] openArr)
        {
            string tema = openArr[6];

            if (betNum.Contains(tema))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static int RenXuanErZu(string[] openArr, string[] betArr)
        {
            int winCount = 0;

            for (int i = 0; i < openArr.Length - 1; i++)
            {
                if (betArr.Contains(openArr[i]) && betArr.Contains(openArr[i + 1]))
                {
                    winCount++;
                }
            }

            return winCount;
        }


        ////广东快乐十分 幸运农场------ 连码
        public static int LianMa(int target, string[] openArr, string[] betArr)
        {
            int winCount = 0;
            int count = 0;

            foreach (string s in betArr)
            {
                if (openArr.Contains(s))
                {
                    count++;
                }
            }

            if (count >= target)
            {
                winCount = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
            }


            return winCount;
        }


        //广东快乐十分 幸运农场------ 大小单双
        public static int DaXiaoDanShuangForKuaiLe10Fen(string num, string betNum, int num2)
        {
            int winCount = 0;

            int he = 0;
            int wei = 0;
            if (num.Length == 2)
            {
                he = int.Parse(num.Substring(0, 1)) + int.Parse(num.Substring(1, 1));
                wei = int.Parse(num.Substring(1, 1));
            }
            else
            {
                he = int.Parse(num);
                wei = he;
            }


            int a = int.Parse(num);

            //string[] temp = {};           //东南西北中发白


            if (betNum == "大")
            {
                if (a >= 11) winCount++;
            }
            else if (betNum == "小")
            {
                if (a <= 10) winCount++;
            }
            else if (betNum == "单")
            {
                if (a % 2 != 0) winCount++;
            }
            else if (betNum == "双")
            {
                if (a % 2 == 0) winCount++;
            }
            else if (betNum == "合单")
            {
                if (he % 2 != 0) winCount++;
            }
            else if (betNum == "合双")
            {
                if (he % 2 == 0) winCount++;
            }
            else if (betNum == "尾大")
            {
                if (wei >= 5) winCount++;
            }
            else if (betNum == "尾小")
            {
                if (wei <= 4) winCount++;
            }
            else if (betNum == "龙")
            {
                if (a > num2) winCount++;
            }
            else if (betNum == "虎")
            {
                if (a < num2) winCount++;
            }
            else if (betNum == "东")
            {
                int[] temp = { 1, 5, 9, 13, 17 };
                if (temp.Contains(a)) winCount++;
            }
            else if (betNum == "南")
            {
                int[] temp = { 2, 6, 10, 14, 18 };
                if (temp.Contains(a)) winCount++;
            }
            else if (betNum == "西")
            {
                int[] temp = { 3, 7, 11, 15, 19 };
                if (temp.Contains(a)) winCount++;
            }
            else if (betNum == "北")
            {
                int[] temp = { 4, 8, 12, 16, 20 };
                if (temp.Contains(a)) winCount++;
            }
            else if (betNum == "中")
            {
                int[] temp = { 1, 2, 3, 4, 5, 6, 7 };
                if (temp.Contains(a)) winCount++;
            }
            else if (betNum == "发")
            {
                int[] temp = { 8, 9, 10, 11, 12, 13, 14 };
                if (temp.Contains(a)) winCount++;
            }
            else if (betNum == "白")
            {
                int[] temp = { 15, 16, 17, 18, 19, 20 };
                if (temp.Contains(a)) winCount++;
            }
            else
            {
                if (betNum == num) winCount++;
            }

            return winCount;
        }



        //11X5前中后
        public static int QianZhongHou11X5(string betNum, int a, int b, int c)
        {
            int winCount = 0;

            if (betNum == "顺子")
            {
                if (ShunZiFor11X5(a, b, c)) winCount++;
            }
            else if (betNum == "半顺")
            {
                if (BanShunFor11X5(a, b, c)) winCount++;
            }
            else if (betNum == "杂六")
            {
                if (ZaLiuFor11X5(a, b, c)) winCount++;
            }

            return winCount;
        }



        //11X5顺子
        public static bool ShunZiFor11X5(int a, int b, int c)
        {
            List<int> list = new List<int>();

            list.Add(a);
            list.Add(b);
            list.Add(c);

            list.Sort();

            a = list[0];
            b = list[1];
            c = list[2];

            string temp = "" + a + b + c;

            if ((c - b == 1 && b - a == 1) || temp == "10111" || temp == "1112")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //11X5半顺
        public static bool BanShunFor11X5(int a, int b, int c)
        {
            if (Util.BanShun(a, b, c) && !ShunZiFor11X5(a, b, c) && !Util.IsDuiZi(a, b, c))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //11X5杂六
        public static bool ZaLiuFor11X5(int a, int b, int c)
        {
            if (!ShunZiFor11X5(a, b, c) && !BanShunFor11X5(a, b, c))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //PK10 幸运飞艇 广东11X5 大小单双
        public static int DaXiaoDanShuang(int num, string betNum)
        {
            int winCount = 0;

            if (betNum == "大")
            {
                if (num >= 6) winCount++;
            }
            else if (betNum == "小")
            {
                if (num <= 5) winCount++;
            }
            else if (betNum == "单")
            {
                if (num % 2 != 0) winCount++;
            }
            else if (betNum == "双")
            {
                if (num % 2 == 0) winCount++;
            }
            else
            {
                if (betNum == num + "") winCount++;
            }

            return winCount;
        }

        //连尾
        public static int LianWei(int target, string[] betArr, string openNum, bool isZhong)
        {
            int winCount = 0;
            int count = 0;

            string[] arr = openNum.Split(',');

            openNum = arr[0].Substring(1, 1) + "," +
                             arr[1].Substring(1, 1) + "," +
                             arr[2].Substring(1, 1) + "," +
                             arr[3].Substring(1, 1) + "," +
                             arr[4].Substring(1, 1) + "," +
                             arr[5].Substring(1, 1) + "," +
                             arr[6].Substring(1, 1);


            foreach (string s in betArr)
            {
                if (isZhong)
                {
                    if (openNum.Contains(s.Substring(0, 1)))
                    {
                        count++;
                    }
                }
                else
                {
                    if (!openNum.Contains(s.Substring(0, 1)))
                    {
                        count++;
                    }
                }
            }


            if (count >= target)
            {
                winCount = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
            }

            return winCount;
        }

        //连肖
        public static int LianXiao(int target, string[] betArr, string openNum, bool isZhong)
        {
            int winCount = 0;
            int count = 0;


            string[] arr = openNum.Split(',');

            openNum = Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[6]));




            foreach (string s in betArr)
            {
                if (isZhong)
                {
                    if (openNum.Contains(s))
                    {
                        count++;
                    }
                }
                else
                {
                    if (!openNum.Contains(s))
                    {
                        count++;
                    }
                }
            }

            if (count >= target)
            {
                winCount = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
            }

            return winCount;
        }

        //不中
        public static int BuZhong(int target, string[] betArr, string openNum)
        {
            int winCount = 0;
            int count = 0;

            foreach (string s in betArr)
            {
                if (!openNum.Contains(s))
                {
                    count++;
                }
            }

            if (count >= target)
            {
                winCount = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
            }

            return winCount;
        }

        //阶乘
        public static int JieCheng(int num)
        {
            int result = 1;
            for (int i = 1; i <= num; i++)
            {
                result *= i;
            }
            return result;
        }

        //1-6龙虎
        public static int JudgeLongHu(string betNum, int a, int b)
        {
            int winCount = 0;

            if (betNum == "龙")
            {
                if (a > b) winCount++;
            }
            else if (betNum == "虎")
            {
                if (a < b) winCount++;
            }
            return winCount;
        }



        //判断正码1-6是否中奖
        public static int JudgeZM1To6IsWin(string betNum, string zm)
        {
            int winCount = 0;

            int a = int.Parse(zm);
            int he = int.Parse(zm.Substring(0, 1)) + int.Parse(zm.Substring(1, 1));
            int wei = int.Parse(zm.Substring(1, 1));


            if (a == 49 && (betNum == "大" || betNum == "小" || betNum == "单" || betNum == "双" || betNum == "合单" || betNum == "合双" || betNum == "尾大" || betNum == "尾小"))
            {
                winCount = -1;  //和局
            }
            else if (betNum == "大")
            {
                if (a >= 25) winCount++;
            }
            else if (betNum == "小")
            {
                if (a <= 24) winCount++;
            }
            else if (betNum == "单")
            {
                if (a % 2 != 0) winCount++;
            }
            else if (betNum == "双")
            {
                if (a % 2 == 0) winCount++;
            }
            else if (betNum == "合单")
            {
                if (he % 2 != 0) winCount++;
            }
            else if (betNum == "合双")
            {
                if (he % 2 == 0) winCount++;
            }
            else if (betNum == "尾大")
            {
                if (wei > 4) winCount++;
            }
            else if (betNum == "尾小")
            {
                if (wei <= 4) winCount++;
            }
            else if (betNum == "红波")
            {
                if (Util.GetColor(zm) == "red") winCount++;
            }
            else if (betNum == "绿波")
            {
                if (Util.GetColor(zm) == "green") winCount++;
            }
            else if (betNum == "蓝波")
            {
                if (Util.GetColor(zm) == "blue") winCount++;
            }
            else
            {
                if (betNum == zm) winCount++;
            }

            return winCount;
        }

        //判断一肖是否中奖
        public static int JudegeYiXiaoisWin(string betNum, string[] openArr)
        {
            int winCount = 0;

            string temp = Util.GetDigitByShengxiao(betNum);
            foreach (string s in openArr)
            {
                if (temp.Contains(s))
                {
                    winCount++;
                    break;
                }
            }

            return winCount;
        }

        //判断一肖是否中奖
        public static int JudegeTeXiaoisWin(string betNum, string tema)
        {
            int winCount = 0;

            string temp = Util.GetDigitByShengxiao(betNum);
            if (temp.Contains(tema)) winCount++;

            return winCount;
        }
    }
}
