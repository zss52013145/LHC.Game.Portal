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
using ShenZhen.Lottery.Public.Cache;
using ShenZhen.Lottery.Public.Cache.Redis;

namespace ShenZhen.Lottery.Common
{
    public class DealOpen
    {

        public static readonly ICacheManager CacheManager = new RedisCache();

        //处理当前下注的结算
        public static void HandCurrentBetting(int fenzhan, int lType, string currentIssue, string openNum)//, int skipCount, int handCount)
        {
            #region 普通下注的


            string sql = "select * from BettingRecord where Issue='" + currentIssue + "' and WinState = 1 and lType =" + lType;


            List<BettingRecord> data = null;

            try
            {
                data = Util.ReaderToListForFenZhan<BettingRecord>(fenzhan, sql);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(fenzhan + "----" + sql);

                LogHelper.WriteLog(ex.Message + ex.StackTrace);

                ////添加记录
                //sql = "insert into OpenFail(lType,Issue,SubTime) values(" + lType + ",'" + currentIssue + "','" + DateTime.Now.ToString() + "')";
                //SqlHelper.ExecuteNonQuery(sql);
            }

            //int count = data.Count();

            //LogHelper.WriteLog(fenzhan + "------------>" + lType + "------------>" + currentIssue + "-------->" + data.Count() + "-------->" + openNum);

            if (data != null && data.Count > 0)
            {

                LogHelper.WriteLog(currentIssue + "----->" + openNum + "------>" + data.Count);

                #region 遍历处理每个单子

                foreach (BettingRecord br in data)
                {
                    try
                    {
                        HandBetting(fenzhan, br, openNum);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                    }
                }


                #endregion
            }


            #endregion
        }

        //具体开奖算法
        public static void HandBetting(int fenzhan, BettingRecord br, string openNum)
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



            #region 六合彩

            if (lType == 1 || lType == 3)
            {
                if (playName == "特码A" || playName == "特码B")
                {

                    #region 123


                    int tema = int.Parse(openArr[6]);
                    int temahe = int.Parse(openArr[6].Substring(0, 1)) + int.Parse(openArr[6].Substring(1, 1));
                    int wei = int.Parse(openArr[6].Substring(1, 1));


                    if (Util.IsNumberic(betNum))
                    {
                        if (tema == int.Parse(betNum)) winCount++;
                    }
                    else if (tema == 49 && (betNum == "大" || betNum == "小" || betNum == "单" || betNum == "双" || betNum == "合单" || betNum == "合双" || betNum == "合大" || betNum == "合小" || betNum == "大单" || betNum == "小单" || betNum == "大双" || betNum == "小双" || betNum == "尾大" || betNum == "尾小"))
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
                    else if (betNum == "合大")
                    {
                        if (temahe >= 7) winCount++;
                    }
                    else if (betNum == "合小")
                    {
                        if (temahe <= 6) winCount++;
                    }
                    else if (betNum == "大单")
                    {
                        if (tema % 2 != 0 && tema >= 25) winCount++;
                    }
                    else if (betNum == "小单")
                    {
                        if (tema % 2 != 0 && tema <= 24) winCount++;
                    }
                    else if (betNum == "大双")
                    {
                        if (tema % 2 == 0 && tema >= 25) winCount++;
                    }
                    else if (betNum == "小双")
                    {
                        if (tema % 2 == 0 && tema <= 24) winCount++;
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

                    #endregion


                    //else
                    //{
                    //    //数字01-49
                    //    if (betNum == openArr[6])
                    //    {
                    //        winCount++;
                    //    }
                    //}
                }
                else if (playName == "正码A" || playName == "正码B")
                {

                    #region 123

                    int zhengmahe = int.Parse(openArr[0]) + int.Parse(openArr[1]) + int.Parse(openArr[2]) +
                                    int.Parse(openArr[3]) + int.Parse(openArr[4]) + int.Parse(openArr[5]) + int.Parse(openArr[6]);


                    string qianliu = openArr[0] + "," + openArr[1] + "," + openArr[2] + "," + openArr[3] + "," + openArr[4] + "," + openArr[5];

                    string zmh = zhengmahe.ToString();

                    int wei = int.Parse(zmh.Substring(zmh.Length - 1, 1));


                    if (betNum == "大")
                    {
                        if (zhengmahe >= 175) winCount++;
                    }
                    else if (betNum == "小")
                    {
                        if (zhengmahe <= 174) winCount++;
                    }
                    else if (betNum == "单")
                    {
                        if (zhengmahe % 2 != 0) winCount++;
                    }
                    else if (betNum == "双")
                    {
                        if (zhengmahe % 2 == 0) winCount++;
                    }

                    else if (Util.IsNumberic(betNum))
                    {
                        //数字01-49
                        if (betNum.Length == 1)
                        {
                            betNum = "0" + betNum;
                        }

                        if (qianliu.Contains(betNum)) winCount++;
                    }

                    #endregion


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
                else if (playName == "一肖中")
                {
                    winCount = JudegeYiXiaoisWin(betNum, openArr);
                }
                else if (playName == "尾数中")
                {
                    winCount = JudegeWeiShuisWin(betNum, openArr);
                }
                else if (playName == "尾数不中")
                {
                    winCount = JudegeWeiShuisNotWin(betNum, openArr);
                }
                else if (playName == "特肖")
                {
                    winCount = JudegeTeXiaoisWin(betNum, openArr[6]);
                }
                else if (playName == "六肖")
                {
                    int tema = int.Parse(openArr[6]);

                    if (tema == 49)
                    {
                        winCount = -1;              //和局
                    }
                    else
                    {
                        string shengxiao = Util.GetShengxiaoByDigit(tema);
                        //if (betNum.Contains(shengxiao)) winCount++;

                        if (betNum.Contains(shengxiao)) {
                        
                           //从剩下的号码中取出5个号码 组成一注

                            int len = betNum.Split(',').Length - 1;

                            winCount = JieCheng(len) / (JieCheng(len - 5) * JieCheng(5));

                        }

                    }
                }
                else if (playName == "一粒任中")
                {
                    string t = "";

                    foreach (string s in betArr)
                    {
                        t = s;

                        if (s.Length == 1)
                        {
                            t = "0" + s;
                        }

                        if (openNum.Contains(t))
                        {
                            winCount++;
                        }
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

                    #region 123

                    betNum = HandbetNum(betNum);


                    if (betNum.Contains('拖'))
                    {
                        betArr = betNum.Split('拖');


                        bool isContais = true;

                        //1.判断胆码是否全包含
                        string openNum2 = openNum.Substring(0, openNum.Length - 3);

                        foreach (string s in betArr[0].Split(','))
                        {
                            if (!openNum2.Contains(s))
                            {
                                isContais = false;
                            }
                        }

                        if (!isContais)
                        {
                            winCount = 0;
                        }
                        else
                        {
                            int tuoCount = 0;

                            //胆码通过
                            foreach (string s in betArr[1].Split(','))
                            {
                                if (openNum2.Contains(s))
                                {
                                    tuoCount++;
                                }
                            }

                            winCount = tuoCount;
                        }

                    }
                    else
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

                    #endregion

                }
                else if (playName == "三中二") //特殊情况
                {

                    #region 123

                    betNum = HandbetNum(betNum);


                    if (betNum.Contains('拖'))
                    {
                        betArr = betNum.Split('拖');


                        bool isContais = true;

                        //1.判断胆码是否全包含
                        string openNum2 = openNum.Substring(0, openNum.Length - 3);

                        foreach (string s in betArr[0].Split(','))
                        {
                            if (!openNum2.Contains(s))
                            {
                                isContais = false;
                            }
                        }

                        if (!isContais)
                        {
                            winCount = 0;
                        }
                        else
                        {

                            //胆码通过
                            foreach (string s in betArr[1].Split(','))
                            {
                                if (openNum2.Contains(s))
                                {
                                    winCount2++;
                                }
                                else
                                {
                                    winCount1++;
                                }
                            }

                            winCount = winCount1 + winCount2;
                        }

                    }
                    else
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



                    #endregion

                }
                else if (playName == "二全中")
                {

                    #region 123

                    betNum = HandbetNum(betNum);


                    if (betNum.Contains('拖'))
                    {
                        betArr = betNum.Split('拖');


                        bool isContais = true;

                        //1.判断胆码是否全包含
                        string openNum2 = openNum.Substring(0, openNum.Length - 3);

                        foreach (string s in betArr[0].Split(','))
                        {
                            if (!openNum2.Contains(s))
                            {
                                isContais = false;
                            }
                        }

                        if (!isContais)
                        {
                            winCount = 0;
                        }
                        else
                        {
                            int tuoCount = 0;

                            //胆码通过
                            foreach (string s in betArr[1].Split(','))
                            {
                                if (openNum2.Contains(s))
                                {
                                    tuoCount++;
                                }
                            }

                            winCount = tuoCount;
                        }

                    }
                    else
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

                    #endregion

                }
                else if (playName == "二中特")
                {
                    #region 123

                    betNum = HandbetNum(betNum);


                    if (betNum.Contains('拖'))
                    {
                        betArr = betNum.Split('拖');


                        bool isContais = true;

                        //1.判断胆码是否全包含
                        string openNum2 = openNum.Substring(0, openNum.Length - 3);

                        foreach (string s in betArr[0].Split(','))
                        {
                            if (!openNum.Contains(s))
                            {
                                isContais = false;
                            }
                        }

                        if (!isContais)
                        {
                            winCount = 0;
                        }
                        else
                        {

                            string tuo = "";

                            //胆码通过
                            foreach (string s in betArr[1].Split(','))
                            {
                                if (openNum.Contains(s))
                                {
                                    tuo += s + " ";
                                }

                            }


                            tuo = tuo.TrimEnd();

                            if (betArr[0] == openArr[6])//胆码是特码
                            {
                                winCount2 = tuo.Split(' ').Length;
                            }
                            else
                            {
                                if (tuo.Contains(openArr[6])) //拖里面包含特码
                                {
                                    winCount2 = 1;

                                    winCount1 = tuo.Split(' ').Length - 1;
                                }
                                else
                                {
                                    winCount1 = tuo.Split(' ').Length;
                                }
                            }

                            winCount = winCount1 + winCount2;
                        }

                    }
                    else
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



                    #endregion

                }
                else if (playName == "特串")
                {
                    #region 123

                    betNum = HandbetNum(betNum);


                    if (betNum.Contains('拖'))
                    {
                        betArr = betNum.Split('拖');


                        bool isContais = true;

                        //1.判断胆码是否全包含
                        string openNum2 = openNum.Substring(0, openNum.Length - 3);

                        foreach (string s in betArr[0].Split(','))
                        {
                            if (!openNum.Contains(s))
                            {
                                isContais = false;
                            }
                        }

                        if (!isContais)
                        {
                            winCount = 0;
                        }
                        else
                        {

                            string tuo = "";

                            //胆码通过
                            foreach (string s in betArr[1].Split(','))
                            {
                                if (openNum.Contains(s))
                                {
                                    tuo += s + " ";
                                }

                            }


                            tuo = tuo.TrimEnd();

                            if (betArr[0] == openArr[6])//胆码是特码
                            {
                                winCount2 = tuo.Split(' ').Length;
                            }
                            else
                            {
                                if (tuo.Contains(openArr[6])) //拖里面包含特码
                                {
                                    winCount2 = 1;

                                    //winCount1 = tuo.Split(' ').Length - 1;
                                }
                            }

                            winCount = winCount1 + winCount2;
                        }

                    }
                    else
                    {
                        if (betNum.Contains(openArr[6])) //包含特码
                        {
                            //包含一个正码
                            for (int i = 0; i < openArr.Length - 1; i++)
                            {
                                if (betNum.Contains(openArr[i]))
                                {
                                    winCount++;
                                }
                            }
                        }

                    }



                    #endregion

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
                else if (playName == "总肖单双")
                {
                    if (betNum == "总肖单")
                    {
                        winCount = ZongXiaoDanShuang(openNum, 1);
                    }
                    else if (betNum == "总肖双")
                    {
                        winCount = ZongXiaoDanShuang(openNum, 2);
                    }

                }
                else if (playName.Contains("中一"))
                {
                    int target = 5;
                    if (playName == "六中一") target = 6;
                    else if (playName == "七中一") target = 7;
                    else if (playName == "八中一") target = 8;
                    else if (playName == "九中一") target = 9;
                    else if (playName == "十中一") target = 10;

                    winCount = ZhongYi(target, betNum, openNum);

                }
                else if (playName.Contains("肖连中"))
                {

                    //

                    #region 123

                    int target = 2;

                    if (playName == "三肖连中") target = 3;
                    else if (playName == "四肖连中") target = 4;
                    else if (playName == "五肖连中") target = 5;


                    //
                    string[] countArr = null;

                    if (betNum.Contains("拖"))
                    {
                        countArr = LianXiaoZhongForDan(target, betNum, openNum).Split('|');
                    }
                    else
                    {
                        countArr = LianXiaoZhong(target, betNum, openNum).Split('|');
                    }



                    winCount1 = int.Parse(countArr[0]);
                    winCount2 = int.Parse(countArr[1]);
                    winCount = winCount1 + winCount2;
                    #endregion
                }
                else if (playName.Contains("肖连不中"))
                {
                    #region 123


                    int target = 2;

                    if (playName == "三肖连不中") target = 3;
                    else if (playName == "四肖连不中") target = 4;
                    else if (playName == "五肖连不中") target = 5;


                    //winCount = LianXiaoBuZhong(target, betArr, openNum);


                    string[] countArr = null;

                    if (betNum.Contains("拖"))
                    {
                        countArr = LianXiaoBuZhongForDan(target, betNum, openNum).Split('|');
                    }
                    else
                    {
                        countArr = LianXiaoBuZhong(target, betNum, openNum).Split('|');
                    }


                    winCount1 = int.Parse(countArr[0]);
                    winCount2 = int.Parse(countArr[1]);
                    winCount = winCount1 + winCount2;

                    #endregion
                }
                else if (playName.Contains("尾连中"))
                {
                    #region 123

                    int target = 2;

                    if (playName == "三尾连中") target = 3;
                    else if (playName == "四尾连中") target = 4;
                    else if (playName == "五尾连中") target = 5;


                    //
                    string[] countArr = null;

                    if (betNum.Contains("拖"))
                    {
                        countArr = LianWeiForDan(target, betNum, openNum).Split('|');
                    }
                    else
                    {
                        countArr = LianWei(target, betNum, openNum).Split('|');
                    }



                    winCount1 = int.Parse(countArr[0]);
                    winCount2 = int.Parse(countArr[1]);
                    winCount = winCount1 + winCount2;
                    #endregion
                }
                else if (playName.Contains("尾连不中"))
                {


                    #region 123

                    int target = 2;

                    if (playName == "三尾连不中") target = 3;
                    else if (playName == "四尾连不中") target = 4;
                    else if (playName == "五尾连不中") target = 5;


                    //
                    string[] countArr = null;

                    if (betNum.Contains("拖"))
                    {
                        countArr = LianWeiBuZhongForDan(target, betNum, openNum).Split('|');
                    }
                    else
                    {
                        countArr = LianWeiBuZhong(target, betNum, openNum).Split('|');
                    }



                    winCount1 = int.Parse(countArr[0]);
                    winCount2 = int.Parse(countArr[1]);
                    winCount = winCount1 + winCount2;
                    #endregion
                }


            }

            #endregion




            #endregion



            //根据winCount做总处理****************************************

            decimal winMoney = 0;
            int winState = 1;
            decimal peilv = br.Peilv;

            UserInfo user = Util.GetEntityByIdForFenZhan<UserInfo>(fenzhan, br.UserId);  //真实玩家
            string sql = "";


            //-----------------------------防止意外------------
            if (user == null)
            {
                //LogHelper.WriteLog("-------------------------------->缓存为空");
                return;
            }
            //-----------------------------End防止意外------------


            //string time = DateTime.Now.ToString();
            //sql = "insert into OpenRecord(BId,SubTime) values(" + br.Id + ",'" + time + "')";


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
                //中奖了
                winState = 3;

                winMoney = winCount * peilv * br.UnitMoney;


                #region 赔率特殊情况


                decimal peilv1 = 0;
                decimal peilv2 = 0;


                if (playName.Contains("肖连") || playName.Contains("尾连") || playName == "三中二" || playName == "二中特")
                {

                    string peilvSql = "select peilv from playinfo2 where  lType = " + br.lType + " and  Pankou = '" + user.PanKou + "' and playSmallType ='" + playName + "'";

                    object o = SqlHelper.ExecuteScalar(peilvSql);

                    string[] peilvArr = o.ToString().Split('/');

                    peilv1 = decimal.Parse(peilvArr[0]);
                    peilv2 = decimal.Parse(peilvArr[1]);

                    winMoney = winCount1 * peilv1 * br.UnitMoney + winCount2 * peilv2 * br.UnitMoney;

                    if (playName.Contains("肖连不中") || playName.Contains("尾连中") || playName == "三中二" )
                    {

                        winMoney = winCount1 * peilv2 * br.UnitMoney + winCount2 * peilv1 * br.UnitMoney;
                    }


                }

                //if (playName == "三中二" || playName == "二中特" || playName.Contains("连肖(中)") || playName.Contains("尾连(不中)"))
                //{



                //    if (playName == "三中二")
                //    {
                //        peilv1 = decimal.Parse(br.Peilv.ToString().Substring(0, 3));
                //        peilv2 = decimal.Parse(br.Peilv.ToString().Substring(3, 2));
                //    }
                //    else if (playName == "二中特")
                //    {
                //        peilv1 = decimal.Parse(br.Peilv.ToString().Substring(0, 2));
                //        peilv2 = decimal.Parse(br.Peilv.ToString().Substring(2, 2));
                //    }


                //    winMoney = winCount1 * peilv1 * br.UnitMoney + winCount2 * peilv2 * br.UnitMoney;
                //}



                #endregion

                //派奖
                //Common.UpdateLotteryMoney(user, winMoney);

                sql += "update UserInfo set Money+=" + winMoney + " where Id=" + user.Id;

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

            if (tempMoney > 0 && user.Id > 0)
            {
                decimal currentMoney = user.Money + tempMoney;
                sql += Util.GetProfitLossSql(user.Id, 2, tempMoney, currentMoney);
            }


            SqlHelper.ExecuteTransactionForFenZhan(fenzhan, sql);



            //再次检查 单子是否已开
            //string searchSql = "select WinState from BettingRecord where Id =" + br.Id;
            //int state = (int)SqlHelper.ExecuteScalarForFenZhan(fenzhan, searchSql);

            //if (state == 1)
            //{
            //    SqlHelper.ExecuteTransactionForFenZhan(fenzhan, sql);
            //}


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


            if (betNum == "大" || betNum == "小" || betNum == "单" || betNum == "双")
            {
                if (num == 11)
                {
                    winCount = -1;
                }
            }

            return winCount;
        }

        //连尾
        public static string LianWei(int target, string betNum, string openNum)
        {

            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;


            string[] betArr = betNum.Split(',');

            string[] arr = openNum.Split(',');

            openNum = arr[0].Substring(1, 1) + "," +
                             arr[1].Substring(1, 1) + "," +
                             arr[2].Substring(1, 1) + "," +
                             arr[3].Substring(1, 1) + "," +
                             arr[4].Substring(1, 1) + "," +
                             arr[5].Substring(1, 1) + "," +
                             arr[6].Substring(1, 1);


            List<string> list = new List<string>();

            string wei = "";

            foreach (string s in betArr)
            {
                wei = s.Substring(0, 1);

                if (openNum.Contains(wei))
                {
                    list.Add(wei);
                    count++;
                }
            }


            if (count >= target)
            {
                //winCount = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));


                if (list.Contains("0"))
                {
                    //二连肖 三连肖 四连肖


                    //1.从count - 1 里面 取 target - 1个 与 当前生肖组成一组

                    winCount1 = JieCheng(count - 1) / (JieCheng(target - 1) * JieCheng(count - 1 - (target - 1)));

                    if (count - 1 >= target)
                    {
                        //2.从count-1 里面 取 target  个 组成一注
                        winCount2 = JieCheng(count - 1) / (JieCheng(target) * JieCheng(count - 1 - target));
                    }
                }
                else
                {
                    winCount2 = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
                }


            }

            return winCount1 + "|" + winCount2;

        }


        //连尾 胆码
        public static string LianWeiForDan(int target, string betNum, string openNum)
        {

            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;


            string[] betArr = betNum.Split('拖');

            string[] danmaArr = betArr[0].Split(',');

            string[] arr = openNum.Split(',');

            openNum = arr[0].Substring(1, 1) + "," +
                             arr[1].Substring(1, 1) + "," +
                             arr[2].Substring(1, 1) + "," +
                             arr[3].Substring(1, 1) + "," +
                             arr[4].Substring(1, 1) + "," +
                             arr[5].Substring(1, 1) + "," +
                             arr[6].Substring(1, 1);


            #region 判断包含胆码

            bool isContainsDanMa = true;

            foreach (string s in danmaArr)
            {
                if (!openNum.Contains(s.Substring(0, 1)))
                {
                    isContainsDanMa = false;
                }
            }

            if (!isContainsDanMa)
            {
                return "0|0";
            }

            #endregion


            List<string> list = new List<string>();

            string wei = "";

            foreach (string s in betArr[1].Split(','))
            {
                wei = s.Substring(0, 1);

                if (openNum.Contains(wei))
                {
                    list.Add(wei);
                    count++;
                }
            }


            if (count >= 1)
            {

                if (danmaArr.Contains("0尾"))
                {
                    winCount1 = count;
                }
                else
                {
                    if (list.Contains("0"))
                    {
                        winCount1 = 1;
                        winCount2 = count - 1;
                    }
                    else
                    {
                        winCount2 = count;
                    }
                }


            }

            return winCount1 + "|" + winCount2;

        }


        //连尾不中
        public static string LianWeiBuZhong(int target, string betNum, string openNum)
        {

            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;


            string[] betArr = betNum.Split(',');

            string[] arr = openNum.Split(',');

            openNum = arr[0].Substring(1, 1) + "," +
                             arr[1].Substring(1, 1) + "," +
                             arr[2].Substring(1, 1) + "," +
                             arr[3].Substring(1, 1) + "," +
                             arr[4].Substring(1, 1) + "," +
                             arr[5].Substring(1, 1) + "," +
                             arr[6].Substring(1, 1);


            List<string> list = new List<string>();

            string wei = "";

            foreach (string s in betArr)
            {
                wei = s.Substring(0, 1);

                if (!openNum.Contains(wei))
                {
                    list.Add(wei);
                    count++;
                }
            }


            if (count >= target)
            {
                //winCount = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));


                if (list.Contains("0"))
                {
                    //二连肖 三连肖 四连肖


                    //1.从count - 1 里面 取 target - 1个 与 当前生肖组成一组

                    winCount1 = JieCheng(count - 1) / (JieCheng(target - 1) * JieCheng(count - 1 - (target - 1)));

                    if (count - 1 >= target)
                    {
                        //2.从count-1 里面 取 target  个 组成一注
                        winCount2 = JieCheng(count - 1) / (JieCheng(target) * JieCheng(count - 1 - target));
                    }
                }
                else
                {
                    winCount2 = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
                }


            }

            return winCount1 + "|" + winCount2;

        }


        //连尾不中胆码
        public static string LianWeiBuZhongForDan(int target, string betNum, string openNum)
        {

            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;


            string[] betArr = betNum.Split('拖');

            string[] danmaArr = betArr[0].Split(',');

            string[] arr = openNum.Split(',');

            openNum = arr[0].Substring(1, 1) + "," +
                             arr[1].Substring(1, 1) + "," +
                             arr[2].Substring(1, 1) + "," +
                             arr[3].Substring(1, 1) + "," +
                             arr[4].Substring(1, 1) + "," +
                             arr[5].Substring(1, 1) + "," +
                             arr[6].Substring(1, 1);


            #region 判断包含胆码

            bool isContainsDanMa = false;

            foreach (string s in danmaArr)
            {
                if (openNum.Contains(s.Substring(0, 1)))
                {
                    isContainsDanMa = true;
                }
            }

            if (isContainsDanMa)
            {
                return "0|0";
            }

            #endregion


            List<string> list = new List<string>();

            string wei = "";

            foreach (string s in betArr[1].Split(','))
            {
                wei = s.Substring(0, 1);

                if (!openNum.Contains(wei))
                {
                    list.Add(wei);
                    count++;
                }
            }


            if (count >= 1)
            {

                if (danmaArr.Contains("0尾"))
                {
                    winCount1 = count;
                }
                else
                {
                    if (list.Contains("0"))
                    {
                        winCount1 = 1;
                        winCount2 = count - 1;
                    }
                    else
                    {
                        winCount2 = count;
                    }
                }


            }

            return winCount1 + "|" + winCount2;

        }

        //连尾不中
        public static string LianWeiBuZhong2(int target, string[] betArr, string openNum)
        {
            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;

            string[] arr = openNum.Split(',');

            openNum = arr[0].Substring(1, 1) + "," +
                             arr[1].Substring(1, 1) + "," +
                             arr[2].Substring(1, 1) + "," +
                             arr[3].Substring(1, 1) + "," +
                             arr[4].Substring(1, 1) + "," +
                             arr[5].Substring(1, 1) + "," +
                             arr[6].Substring(1, 1);


            List<string> list = new List<string>();

            foreach (string s in betArr)
            {
                if (!openNum.Contains(s.Substring(0, 1)))
                {
                    count++;
                    list.Add(s.Substring(0, 1));
                }
            }


            if (count >= target)
            {
                if (list.Contains("0"))
                {
                    //1.从count - 1 里面 取 target - 1个 与 当前生肖组成一组

                    winCount1 = JieCheng(count - 1) / (JieCheng(target - 1) * JieCheng(count - 1 - (target - 1)));

                    //2.从count-1 里面 取 target  个 组成一注
                    if (count - 1 >= target)
                    {
                        winCount2 = JieCheng(count - 1) / (JieCheng(target) * JieCheng(count - 1 - target));
                    }
                }
                else
                {
                    winCount2 = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
                }

            }

            return winCount1 + "|" + winCount2;
        }


        //连肖不中
        public static string LianXiaoBuZhong(int target, string betNum, string openNum)
        {
            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;



            string[] betArr = betNum.Split(',');

            string[] arr = openNum.Split(',');

            openNum = Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[6]));


            List<string> list = new List<string>();

            foreach (string s in betArr)
            {
                if (!openNum.Contains(s))
                {
                    count++;
                    list.Add(s);
                }
            }

            if (count >= target)
            {
                string shengxiao = Util.GetCurrentShengXiao();
                if (list.Contains(shengxiao))
                {
                    //二连肖 三连肖 四连肖


                    //1.从count - 1 里面 取 target - 1个 与 当前生肖组成一组

                    winCount1 = JieCheng(count - 1) / (JieCheng(target - 1) * JieCheng(count - 1 - (target - 1)));

                    if (count - 1 >= target)
                    {
                        //2.从count-1 里面 取 target  个 组成一注
                        winCount2 = JieCheng(count - 1) / (JieCheng(target) * JieCheng(count - 1 - target));
                    }
                }
                else
                {
                    winCount2 = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
                }


            }

            return winCount1 + "|" + winCount2;
        }


        //连肖不中胆码
        public static string LianXiaoBuZhongForDan(int target, string betNum, string openNum)
        {
            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;

            string[] betArr = betNum.Split('拖');

            string[] danmaArr = betArr[0].Split(',');

            string[] arr = openNum.Split(',');

            openNum = Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[6]));



            #region 判断包含胆码

            bool isContainsDanMa = false;

            foreach (string s in danmaArr)
            {
                if (openNum.Contains(s))
                {
                    isContainsDanMa = true;
                    break;
                }
            }

            if (isContainsDanMa)
            {
                return "0|0";
            }

            #endregion




            List<string> list = new List<string>();

            foreach (string s in betArr[1].Split(','))
            {
                if (!openNum.Contains(s))
                {
                    count++;
                    list.Add(s);
                }
            }


            if (count >= 1)
            {
                string shengxiao = Util.GetCurrentShengXiao();


                if (danmaArr.Contains(shengxiao))
                {
                    winCount1 = count;
                }
                else
                {
                    if (list.Contains(shengxiao))
                    {
                        winCount1 = 1;
                        winCount2 = count - 1;
                    }
                    else
                    {
                        winCount2 = count;
                    }
                }




            }

            return winCount1 + "|" + winCount2;
        }


        //连肖中
        public static string LianXiaoZhong(int target, string betNum, string openNum)
        {
            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;



            string[] betArr = betNum.Split(',');

            string[] arr = openNum.Split(',');

            openNum = Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[6]));


            List<string> list = new List<string>();

            foreach (string s in betArr)
            {
                if (openNum.Contains(s))
                {
                    count++;
                    list.Add(s);
                }
            }

            if (count >= target)
            {
                string shengxiao = Util.GetCurrentShengXiao();
                if (list.Contains(shengxiao))
                {
                    //二连肖 三连肖 四连肖


                    //1.从count - 1 里面 取 target - 1个 与 当前生肖组成一组

                    winCount1 = JieCheng(count - 1) / (JieCheng(target - 1) * JieCheng(count - 1 - (target - 1)));

                    if (count - 1 >= target)
                    {
                        //2.从count-1 里面 取 target  个 组成一注
                        winCount2 = JieCheng(count - 1) / (JieCheng(target) * JieCheng(count - 1 - target));
                    }
                }
                else
                {
                    winCount2 = JieCheng(count) / (JieCheng(target) * JieCheng(count - target));
                }


            }

            return winCount1 + "|" + winCount2;
        }


        //连肖 胆码
        public static string LianXiaoZhongForDan(int target, string betNum, string openNum)
        {
            int winCount1 = 0;
            int winCount2 = 0;
            int count = 0;

            string[] betArr = betNum.Split('拖');

            string[] danmaArr = betArr[0].Split(',');

            string[] arr = openNum.Split(',');

            openNum = Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[6]));


            //判断包含胆码

            #region 判断包含胆码

            bool isContainsDanMa = true;

            foreach (string s in danmaArr)
            {
                if (!openNum.Contains(s))
                {
                    isContainsDanMa = false;
                }
            }

            if (!isContainsDanMa)
            {
                return "0|0";
            }

            #endregion




            List<string> list = new List<string>();

            foreach (string s in betArr[1].Split(','))
            {
                if (openNum.Contains(s))
                {
                    count++;
                    list.Add(s);
                }
            }


            if (count >= 1)
            {
                string shengxiao = Util.GetCurrentShengXiao();


                if (danmaArr.Contains(shengxiao))
                {
                    winCount1 = count;
                }
                else
                {
                    if (list.Contains(shengxiao))
                    {
                        winCount1 = 1;
                        winCount2 = count - 1;
                    }
                    else
                    {
                        winCount2 = count;
                    }
                }




            }

            return winCount1 + "|" + winCount2;
        }


        //中一
        public static int ZhongYi(int target, string betNum, string openNum)
        {

            //挑选4-10个号码为一投注组合，若开奖号码的正码和特码有一个号码在其中视为中奖


            int winCount = 0;


            //1.查看投注号码中有几个是开出的7个号码

            string[] arr = openNum.Split(',');

            List<string> list = null;

            if (target == 5)
            {
                list = GetNum5(betNum);
            }
            else if (target == 6)
            {
                list = GetNum6(betNum);
            }
            else if (target == 7)
            {
                list = GetNum7(betNum);
            }
            else if (target == 8)
            {
                list = GetNum8(betNum);
            }
            else if (target == 9)
            {
                list = GetNum9(betNum);
            }
            else if (target == 10)
            {
                list = GetNum10(betNum);
            }


            foreach (string s in list)                  //投注内容
            {
                foreach (string s2 in arr)
                {
                    if (s.Contains(s2))
                    {
                        winCount++;
                        break;
                    }
                }
            }

            return winCount;
        }



        //根据投注号码获得组合号码
        public static List<string> GetNum5(string betNum)
        {
            List<string> list = new List<string>();

            string[] arr = betNum.Split(',');


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length == 1) arr[i] = "0" + arr[i];
            }

            string t = "";

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j + 1; k < arr.Length; k++)
                    {
                        for (int o = k + 1; o < arr.Length; o++)
                        {
                            for (int p = o + 1; p < arr.Length; p++)
                            {
                                t = arr[i] + "," + arr[j] + "," + arr[k] + "," + arr[o] + "," + arr[p];

                                list.Add(t);
                            }
                        }
                    }
                }
            }


            return list;

        }

        //根据投注号码获得组合号码
        public static List<string> GetNum6(string betNum)
        {
            List<string> list = new List<string>();

            string[] arr = betNum.Split(',');


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length == 1) arr[i] = "0" + arr[i];
            }

            string t = "";

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j + 1; k < arr.Length; k++)
                    {
                        for (int o = k + 1; o < arr.Length; o++)
                        {
                            for (int p = o + 1; p < arr.Length; p++)
                            {
                                for (int q = p + 1; q < arr.Length; q++)
                                {
                                    t = arr[i] + "," + arr[j] + "," + arr[k] + "," + arr[o] + "," + arr[p] + "," + arr[q];

                                    list.Add(t);
                                }

                            }
                        }
                    }
                }
            }


            return list;

        }

        //根据投注号码获得组合号码
        public static List<string> GetNum7(string betNum)
        {
            List<string> list = new List<string>();

            string[] arr = betNum.Split(',');


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length == 1) arr[i] = "0" + arr[i];
            }

            string t = "";

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j + 1; k < arr.Length; k++)
                    {
                        for (int o = k + 1; o < arr.Length; o++)
                        {
                            for (int p = o + 1; p < arr.Length; p++)
                            {
                                for (int q = p + 1; q < arr.Length; q++)
                                {

                                    for (int r = q + 1; r < arr.Length; r++)
                                    {
                                        t = arr[i] + "," + arr[j] + "," + arr[k] + "," + arr[o] + "," + arr[p] + "," + arr[q] + "," + arr[r];

                                        list.Add(t);
                                    }


                                }

                            }
                        }
                    }
                }
            }


            return list;

        }

        //根据投注号码获得组合号码
        public static List<string> GetNum8(string betNum)
        {
            List<string> list = new List<string>();

            string[] arr = betNum.Split(',');


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length == 1) arr[i] = "0" + arr[i];
            }

            string t = "";

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j + 1; k < arr.Length; k++)
                    {
                        for (int o = k + 1; o < arr.Length; o++)
                        {
                            for (int p = o + 1; p < arr.Length; p++)
                            {
                                for (int q = p + 1; q < arr.Length; q++)
                                {
                                    for (int r = q + 1; r < arr.Length; r++)
                                    {
                                        for (int s = r + 1; s < arr.Length; s++)
                                        {
                                            t = arr[i] + "," + arr[j] + "," + arr[k] + "," + arr[o] + "," + arr[p] + "," + arr[q] + "," + arr[r] + "," + arr[s];

                                            list.Add(t);
                                        }


                                    }


                                }

                            }
                        }
                    }
                }
            }


            return list;

        }

        //根据投注号码获得组合号码
        public static List<string> GetNum9(string betNum)
        {
            List<string> list = new List<string>();

            string[] arr = betNum.Split(',');


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length == 1) arr[i] = "0" + arr[i];
            }

            string t = "";

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j + 1; k < arr.Length; k++)
                    {
                        for (int o = k + 1; o < arr.Length; o++)
                        {
                            for (int p = o + 1; p < arr.Length; p++)
                            {
                                for (int q = p + 1; q < arr.Length; q++)
                                {
                                    for (int r = q + 1; r < arr.Length; r++)
                                    {
                                        for (int s = r + 1; s < arr.Length; s++)
                                        {

                                            for (int u = s + 1; u < arr.Length; u++)
                                            {
                                                t = arr[i] + "," + arr[j] + "," + arr[k] + "," + arr[o] + "," + arr[p] + "," + arr[q] + "," + arr[r] + "," + arr[s] + "," + arr[u];

                                                list.Add(t);
                                            }

                                        }


                                    }


                                }

                            }
                        }
                    }
                }
            }


            return list;

        }

        //根据投注号码获得组合号码
        public static List<string> GetNum10(string betNum)
        {
            List<string> list = new List<string>();

            string[] arr = betNum.Split(',');


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length == 1) arr[i] = "0" + arr[i];
            }

            string t = "";

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    for (int k = j + 1; k < arr.Length; k++)
                    {
                        for (int o = k + 1; o < arr.Length; o++)
                        {
                            for (int p = o + 1; p < arr.Length; p++)
                            {
                                for (int q = p + 1; q < arr.Length; q++)
                                {
                                    for (int r = q + 1; r < arr.Length; r++)
                                    {
                                        for (int s = r + 1; s < arr.Length; s++)
                                        {

                                            for (int u = s + 1; u < arr.Length; u++)
                                            {
                                                for (int v = u + 1; v < arr.Length; v++)
                                                {
                                                    t = arr[i] + "," + arr[j] + "," + arr[k] + "," + arr[o] + "," + arr[p] + "," + arr[q] + "," + arr[r] + "," + arr[s] + "," + arr[u] + "," + arr[v];

                                                    list.Add(t);
                                                }


                                            }

                                        }


                                    }


                                }

                            }
                        }
                    }
                }
            }


            return list;

        }

        public static int ZongXiaoDanShuang(string openNum, int type)
        {
            int winCount = 0;

            string[] arr = openNum.Split(',');

            openNum = Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                             Util.GetShengxiaoByDigit(int.Parse(arr[6]));

            List<string> list = new List<string>();

            foreach (string s in openNum.Split(','))
            {
                if (!list.Contains(s))
                {
                    list.Add(s);
                }
            }

            if (type == 1 && list.Count % 2 != 0)
            {

                winCount++;
            }
            else if (type == 2 && list.Count % 2 == 0)
            {
                winCount++;
            }

            return winCount;
        }

        //不中
        public static int BuZhong(int target, string[] betArr, string openNum)
        {
            int winCount = 0;
            int count = 0;

            string temp = "";

            foreach (string s in betArr)
            {

                if (s.Length == 1)
                {
                    temp = "0" + s;
                }
                else
                {
                    temp = s;
                }

                if (!openNum.Contains(temp))
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


            if (a == 49 && (betNum == "大" || betNum == "小" || betNum == "单" || betNum == "双" || betNum == "合单" || betNum == "合双" || betNum == "合大" || betNum == "合小" || betNum == "尾大" || betNum == "尾小" || betNum == "大单" || betNum == "小单" || betNum == "大双" || betNum == "小双"))
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
            else if (betNum == "合大")
            {
                if (he >= 7) winCount++;
            }
            else if (betNum == "合小")
            {
                if (he <= 6) winCount++;
            }
            else if (betNum == "大单")
            {
                if (a % 2 != 0 && a >= 25) winCount++;
            }
            else if (betNum == "小单")
            {
                if (a % 2 != 0 && a <= 24) winCount++;
            }
            else if (betNum == "大双")
            {
                if (a % 2 == 0 && a >= 25) winCount++;
            }
            else if (betNum == "小双")
            {
                if (a % 2 == 0 && a <= 24) winCount++;
            }
            else if (betNum == "尾大")
            {
                if (wei > 4) winCount++;
            }
            else if (betNum == "尾小")
            {
                if (wei <= 4) winCount++;
            }
            else if (betNum == "家禽")
            {
                string shengxiao = Util.GetShengxiaoByDigit(a);
                if (Util.IsJiaQin(shengxiao))
                {
                    winCount++;
                }
            }
            else if (betNum == "野兽")
            {
                string shengxiao = Util.GetShengxiaoByDigit(a);
                if (!Util.IsJiaQin(shengxiao))
                {
                    winCount++;
                }
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

            string temp = Util.GetDigitByShengxiao(betNum);     //生肖对应的号码

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



        //判断尾数是否中奖
        public static int JudegeWeiShuisWin(string betNum, string[] openArr)
        {
            int winCount = 0;

            string wei = betNum.Substring(0, 1);

            foreach (string s in openArr)
            {
                if (s.EndsWith(wei))
                {
                    winCount++;
                    break;
                }
            }

            return winCount;
        }



        //判断尾数不中
        public static int JudegeWeiShuisNotWin(string betNum, string[] openArr)
        {
            int winCount = 0;

            string wei = betNum.Substring(0, 1);

            foreach (string s in openArr)
            {
                if (s.EndsWith(wei))
                {
                    winCount++;
                    break;
                }
            }

            if (winCount > 0)
            {
                return 0;  //有尾数 就不中
            }
            else
            {
                return 1;
            }

            //return winCount;
        }

        //JudegeWeiShuisWin


        //判断一肖是否中奖
        public static int JudegeTeXiaoisWin(string betNum, string tema)
        {
            int winCount = 0;

            string temp = Util.GetDigitByShengxiao(betNum);
            if (temp.Contains(tema)) winCount++;

            return winCount;
        }



        public static string HandbetNum(string betNum)
        {

            if (betNum.Contains("拖"))
            {
                string[] arr = betNum.Split('拖');

                string result = "";

                foreach (string s in arr[0].Split(','))
                {
                    if (s.Length == 1)
                    {
                        result += "0" + s + ",";
                    }
                    else
                    {
                        result += s + ",";
                    }
                }


                result = result.TrimEnd(',') + "拖";

                foreach (string s in arr[1].Split(','))
                {
                    if (s.Length == 1)
                    {
                        result += "0" + s + ",";
                    }
                    else
                    {
                        result += s + ",";
                    }
                }


                result = result.TrimEnd(',');

                return result;



            }
            else
            {

                string[] arr = betNum.Split(',');

                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].Length == 1)
                    {
                        arr[i] = "0" + arr[i];
                    }
                }

                return string.Join(",", arr);

            }


        }
    }
}
