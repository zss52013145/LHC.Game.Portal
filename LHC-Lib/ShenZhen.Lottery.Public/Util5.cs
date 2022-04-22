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


       

        public static void AddTuiShui(object userId, int lType, double kouchu, string pankou)
        {
            string txt = "";

            if (lType == 1)
            {
                txt = "特码A\t100000\t100000\t13.2\t14.5\t15.5\t15.5|特码B\t100000\t100000\t3\t4.5\t5.5\t5.5|特码单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码家禽野禽\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码尾大尾小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-红波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-蓝波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-绿波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码大单小单\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码大双小双\t50000\t100000\t1.4\t1.5\t2.5\t2.5|正码A\t100000\t100000\t12.5\t11.5\t11.5\t11.5|正码B\t10000\t100000\t3.5\t3.5\t3.5\t3.5|总和单双\t100000\t100000\t1.4\t2.5\t2.5\t2.5|总和大小\t100000\t100000\t1.4\t2.5\t2.5\t2.5|正特\t10000\t60000\t13.2\t13.5\t13.5\t13.5|正特单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特家禽野禽\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特尾大尾小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-红波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-蓝波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-绿波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特大单小单\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特大双小双\t50000\t100000\t1.4\t3.5\t3.5\t3.5|一肖中\t50000\t200000\t1.4\t1\t0.5\t0.5|一肖不中\t50000\t200000\t1.4\t1\t0.5\t0.5|尾数中\t50000\t200000\t1.4\t0.5\t0.5\t0.5|尾数不中\t50000\t200000\t1.4\t0\t0\t0|二肖连中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|三肖连中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|四肖连中\t10000\t100000\t1.4\t1.5\t1.5\t1.5|五肖连中\t3000\t100000\t1.4\t1.5\t1.5\t1.5|二肖连不中\t10000\t100000\t1.4\t1.5\t1.5\t1.5|三肖连不中\t80000\t100000\t1.4\t1.5\t1.5\t1.5|四肖连不中\t50000\t100000\t1.4\t1.5\t1.5\t1.5|五肖连不中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|二尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|四尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|五尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|二尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|四尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|五尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三全中\t999\t3000\t15.5\t15.5\t15.5\t15.5|三中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二全中\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|特串\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|四不中\t100000\t100000\t1\t1.5\t1.5\t0.5|五不中\t100000\t100000\t1\t1.5\t1.5\t0.5|六不中\t100000\t100000\t1\t1.5\t1.5\t0.5|七不中\t100000\t100000\t1\t1.5\t1.5\t0.5|八不中\t100000\t100000\t1\t1.5\t1.5\t0.5|九不中\t100000\t100000\t1\t1.5\t1.5\t0.5|十不中\t100000\t100000\t1\t1.5\t1.5\t0.5|十一不中\t50000\t100000\t1\t2.5\t2.5\t2.5|十二不中\t50000\t100000\t1\t2.5\t2.5\t2.5|总肖\t100000\t100000\t1.4\t1\t2\t3|总肖单双\t100000\t100000\t1.4\t1\t2\t3|特肖\t100000\t100000\t1.5\t1.5\t1.5\t1.5|二肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|三肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|四肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|五肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|六肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|一粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|二粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|三粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|五粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|五中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|六中一\t10000\t10000\t0.5\t0.5\t0.5\t0.5|七中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|八中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|九中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|十中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|半波\t10000\t500000\t2.5\t2.5\t2.5\t2.5|特尾\t10000\t500000\t0.5\t0.5\t0.5\t0.5|头数\t20000\t100000\t0.5\t0.5\t0.5\t0.5|五行\t20000\t100000\t0.5\t0.5\t0.5\t0.5";
            }
            else if (lType == 3)
            {
                txt = "特码A\t100000\t100000\t13.2\t14.5\t15.5\t15.5|特码B\t100000\t100000\t3\t4.5\t5.5\t5.5|特码单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码家禽野禽\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码尾大尾小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-红波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-蓝波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-绿波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大单小单\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大双小双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|正码A\t10000\t100000\t11.5\t11.5\t11.5\t11.5|正码B\t10000\t100000\t3.2\t3.5\t3.5\t3.5|总和单双\t100000\t100000\t1.4\t2.5\t2.5\t2.5|总和大小\t100000\t100000\t1.4\t2.5\t2.5\t2.5|正特\t100000\t100000\t13.2\t13.5\t13.5\t13.5|正特单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特家禽野禽\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特尾大尾小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-红波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-蓝波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-绿波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大单小单\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大双小双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|一肖中\t100000\t200000\t1.4\t1\t0.5\t0.5|一肖不中\t100000\t100000\t1.4\t1\t0.5\t0.5|尾数中\t100000\t200000\t1\t0.5\t0.5\t0.5|尾数不中\t100000\t200000\t1\t0\t0\t0|二肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|三肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|四肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|五肖连中\t3000\t100000\t1.5\t1.5\t1.5\t1.5|二肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|三肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|四肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|五肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|二尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|四尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|五尾连中\t50000\t50000000\t0.5\t1.5\t1.5\t0.5|二尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|四尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|五尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三全中\t1000\t3000\t15.5\t15.5\t15.5\t15.5|三中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二全中\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|特串\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|四不中\t50000\t100000\t1\t1.5\t1.5\t0.5|五不中\t100000\t100000\t1\t1.5\t1.5\t0.5|六不中\t50000\t100000\t1\t1.5\t1.5\t0.5|七不中\t50000\t100000\t1\t1.5\t1.5\t0.5|八不中\t50000\t100000\t1\t1.5\t1.5\t0.5|九不中\t50000\t100000\t1\t1.5\t1.5\t0.5|十不中\t50000\t100000\t1\t1.5\t1.5\t0.5|十一不中\t50000\t100000\t1\t2.5\t2.5\t2.5|十二不中\t50000\t100000\t1\t2.5\t2.5\t2.5|总肖\t30000\t50000\t1.4\t1\t2\t3|总肖单双\t30000\t50000\t1.4\t1\t2\t3|特肖\t10000\t50000\t1.5\t1.5\t1.5\t1.5|二肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|三肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|四肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|五肖\t30000\t50000\t2.5\t2.5\t2.5\t2.5|六肖\t30000\t100000\t1.4\t2.5\t2.5\t2.5|一粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|二粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|三粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|五粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|五中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|六中一\t10000\t10000\t0.5\t0.5\t0.5\t0.5|七中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|八中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|九中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|十中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|半波\t10000\t500000\t2.5\t2.5\t2.5\t2.5|特尾\t10000\t500000\t0.5\t0.5\t0.5\t0.5|头数\t20000\t100000\t0.5\t0.5\t0.5\t0.5|五行\t20000\t100000\t0.5\t0.5\t0.5\t0.5";
            }


            string[] arr = txt.Split('|');

            double a = 0;
            double b = 0;
            double c = 0;
            double d = 0;


            string[] arr2 = null;

            string playName = "";
            string max1 = "";
            string max2 = "";

            foreach (string s in arr)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    arr2 = s.Split('\t');

                    playName = arr2[0];
                    max1 = arr2[1];
                    max2 = arr2[2];

                    a = double.Parse(arr2[3]) - kouchu;
                    b = double.Parse(arr2[4]) - kouchu;
                    c = double.Parse(arr2[5]) - kouchu;
                    d = double.Parse(arr2[6]) - kouchu;

                    #region 排除负数

                    if (a < 0) a = 0;
                    if (b < 0) b = 0;
                    if (c < 0) c = 0;
                    if (d < 0) d = 0;

                    #endregion


                    #region 会员只有一个盘口

                    if (pankou == "A")
                    {

                        b = 0;
                        c = 0;
                        d = 0;
                    }
                    else if (pankou == "B")
                    {

                        a = 0;
                        c = 0;
                        d = 0;
                    }
                    else if (pankou == "C")
                    {

                        a = 0;
                        b = 0;
                        d = 0;
                    }
                    else if (pankou == "D")
                    {

                        a = 0;
                        b = 0;
                        c = 0;
                    }


                    #endregion

                    //不退水
                    if (kouchu == 100)
                    {

                        a = 0;
                        b = 0;
                        c = 0;
                        d = 0;
                    }



                    string sql = "insert into tuishuiinfo(userId,lType,playname,max1,max2,A,B,C,D)  values(@userId,@lType,@playname,@max1,@max2,@A,@B,@C,@D)";

                    SqlParameter[] pms =
                    {
                        new SqlParameter("@userId",userId),
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@playname",playName),
                        new SqlParameter("@max1",max1),
                        new SqlParameter("@max2",max2),
                        new SqlParameter("@A",a),
                        new SqlParameter("@B",b),
                        new SqlParameter("@C",c),
                        new SqlParameter("@D",d),
                        
                    };

                    SqlHelper.ExecuteNonQuery(sql, pms);

                }
            }



        }

        public static void AddTuiShui(object userId, int lType, double kouchu)
        {
            string txt = "";

            if (lType == 1)
            {
                txt = "特码A\t100000\t100000\t13.2\t14.5\t15.5\t15.5|特码B\t100000\t100000\t3\t4.5\t5.5\t5.5|特码单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码家禽野禽\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码尾大尾小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-红波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-蓝波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-绿波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码大单小单\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码大双小双\t50000\t100000\t1.4\t1.5\t2.5\t2.5|正码A\t100000\t100000\t12.5\t11.5\t11.5\t11.5|正码B\t10000\t100000\t3.5\t3.5\t3.5\t3.5|总和单双\t100000\t100000\t1.4\t2.5\t2.5\t2.5|总和大小\t100000\t100000\t1.4\t2.5\t2.5\t2.5|正特\t10000\t60000\t13.2\t13.5\t13.5\t13.5|正特单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特家禽野禽\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特尾大尾小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-红波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-蓝波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-绿波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特大单小单\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特大双小双\t50000\t100000\t1.4\t3.5\t3.5\t3.5|一肖中\t50000\t200000\t1.4\t1\t0.5\t0.5|一肖不中\t50000\t200000\t1.4\t1\t0.5\t0.5|尾数中\t50000\t200000\t1.4\t0.5\t0.5\t0.5|尾数不中\t50000\t200000\t1.4\t0\t0\t0|二肖连中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|三肖连中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|四肖连中\t10000\t100000\t1.4\t1.5\t1.5\t1.5|五肖连中\t3000\t100000\t1.4\t1.5\t1.5\t1.5|二肖连不中\t10000\t100000\t1.4\t1.5\t1.5\t1.5|三肖连不中\t80000\t100000\t1.4\t1.5\t1.5\t1.5|四肖连不中\t50000\t100000\t1.4\t1.5\t1.5\t1.5|五肖连不中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|二尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|四尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|五尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|二尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|四尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|五尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三全中\t999\t3000\t15.5\t15.5\t15.5\t15.5|三中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二全中\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|特串\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|四不中\t100000\t100000\t1\t1.5\t1.5\t0.5|五不中\t100000\t100000\t1\t1.5\t1.5\t0.5|六不中\t100000\t100000\t1\t1.5\t1.5\t0.5|七不中\t100000\t100000\t1\t1.5\t1.5\t0.5|八不中\t100000\t100000\t1\t1.5\t1.5\t0.5|九不中\t100000\t100000\t1\t1.5\t1.5\t0.5|十不中\t100000\t100000\t1\t1.5\t1.5\t0.5|十一不中\t50000\t100000\t1\t2.5\t2.5\t2.5|十二不中\t50000\t100000\t1\t2.5\t2.5\t2.5|总肖\t100000\t100000\t1.4\t1\t2\t3|总肖单双\t100000\t100000\t1.4\t1\t2\t3|特肖\t100000\t100000\t1.5\t1.5\t1.5\t1.5|二肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|三肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|四肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|五肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|六肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|一粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|二粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|三粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|五粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|五中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|六中一\t10000\t10000\t0.5\t0.5\t0.5\t0.5|七中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|八中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|九中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|十中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|半波\t10000\t500000\t2.5\t2.5\t2.5\t2.5|特尾\t10000\t500000\t0.5\t0.5\t0.5\t0.5|头数\t20000\t100000\t0.5\t0.5\t0.5\t0.5|五行\t20000\t100000\t0.5\t0.5\t0.5\t0.5";
            }
            else if (lType == 3)
            {
                txt = "特码A\t100000\t100000\t13.2\t14.5\t15.5\t15.5|特码B\t100000\t100000\t3\t4.5\t5.5\t5.5|特码单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码家禽野禽\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码尾大尾小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-红波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-蓝波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-绿波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大单小单\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大双小双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|正码A\t10000\t100000\t11.5\t11.5\t11.5\t11.5|正码B\t10000\t100000\t3.2\t3.5\t3.5\t3.5|总和单双\t100000\t100000\t1.4\t2.5\t2.5\t2.5|总和大小\t100000\t100000\t1.4\t2.5\t2.5\t2.5|正特\t100000\t100000\t13.2\t13.5\t13.5\t13.5|正特单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特家禽野禽\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特尾大尾小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-红波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-蓝波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-绿波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大单小单\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大双小双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|一肖中\t100000\t200000\t1.4\t1\t0.5\t0.5|一肖不中\t100000\t100000\t1.4\t1\t0.5\t0.5|尾数中\t100000\t200000\t1\t0.5\t0.5\t0.5|尾数不中\t100000\t200000\t1\t0\t0\t0|二肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|三肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|四肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|五肖连中\t3000\t100000\t1.5\t1.5\t1.5\t1.5|二肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|三肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|四肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|五肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|二尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|四尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|五尾连中\t50000\t50000000\t0.5\t1.5\t1.5\t0.5|二尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|四尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|五尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三全中\t1000\t3000\t15.5\t15.5\t15.5\t15.5|三中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二全中\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|特串\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|四不中\t50000\t100000\t1\t1.5\t1.5\t0.5|五不中\t100000\t100000\t1\t1.5\t1.5\t0.5|六不中\t50000\t100000\t1\t1.5\t1.5\t0.5|七不中\t50000\t100000\t1\t1.5\t1.5\t0.5|八不中\t50000\t100000\t1\t1.5\t1.5\t0.5|九不中\t50000\t100000\t1\t1.5\t1.5\t0.5|十不中\t50000\t100000\t1\t1.5\t1.5\t0.5|十一不中\t50000\t100000\t1\t2.5\t2.5\t2.5|十二不中\t50000\t100000\t1\t2.5\t2.5\t2.5|总肖\t30000\t50000\t1.4\t1\t2\t3|总肖单双\t30000\t50000\t1.4\t1\t2\t3|特肖\t10000\t50000\t1.5\t1.5\t1.5\t1.5|二肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|三肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|四肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|五肖\t30000\t50000\t2.5\t2.5\t2.5\t2.5|六肖\t30000\t100000\t1.4\t2.5\t2.5\t2.5|一粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|二粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|三粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|五粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|五中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|六中一\t10000\t10000\t0.5\t0.5\t0.5\t0.5|七中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|八中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|九中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|十中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|半波\t10000\t500000\t2.5\t2.5\t2.5\t2.5|特尾\t10000\t500000\t0.5\t0.5\t0.5\t0.5|头数\t20000\t100000\t0.5\t0.5\t0.5\t0.5|五行\t20000\t100000\t0.5\t0.5\t0.5\t0.5";
            }


            string[] arr = txt.Split('|');

            double a = 0;
            double b = 0;
            double c = 0;
            double d = 0;


            string[] arr2 = null;

            string playName = "";
            string max1 = "";
            string max2 = "";

            foreach (string s in arr)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    arr2 = s.Split('\t');

                    playName = arr2[0];
                    max1 = arr2[1];
                    max2 = arr2[2];

                    a = double.Parse(arr2[3]) - kouchu;
                    b = double.Parse(arr2[4]) - kouchu;
                    c = double.Parse(arr2[5]) - kouchu;
                    d = double.Parse(arr2[6]) - kouchu;


                    string sql = "insert into tuishuiinfo(userId,lType,playname,max1,max2,A,B,C,D)  values(@userId,@lType,@playname,@max1,@max2,@A,@B,@C,@D)";

                    SqlParameter[] pms =
                    {
                        new SqlParameter("@userId",userId),
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@playname",playName),
                        new SqlParameter("@max1",max1),
                        new SqlParameter("@max2",max2),
                        new SqlParameter("@A",a),
                        new SqlParameter("@B",b),
                        new SqlParameter("@C",c),
                        new SqlParameter("@D",d),
                        
                    };

                    SqlHelper.ExecuteNonQuery(sql, pms);

                }
            }



        }

        public static void AddTuiShui(int userId, int lType)
        {
            string txt = "";

            if (lType == 1)
            {
                txt = "特码A\t100000\t100000\t13.2\t14.5\t15.5\t15.5|特码B\t100000\t100000\t3\t4.5\t5.5\t5.5|特码单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码家禽野禽\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码尾大尾小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-红波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-蓝波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-绿波\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码大单小单\t50000\t100000\t1.4\t1.5\t2.5\t2.5|特码大双小双\t50000\t100000\t1.4\t1.5\t2.5\t2.5|正码A\t100000\t100000\t12.5\t11.5\t11.5\t11.5|正码B\t10000\t100000\t3.5\t3.5\t3.5\t3.5|总和单双\t100000\t100000\t1.4\t2.5\t2.5\t2.5|总和大小\t100000\t100000\t1.4\t2.5\t2.5\t2.5|正特\t10000\t60000\t13.2\t13.5\t13.5\t13.5|正特单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特家禽野禽\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特尾大尾小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-红波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-蓝波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-绿波\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特大单小单\t50000\t100000\t1.4\t3.5\t3.5\t3.5|正特大双小双\t50000\t100000\t1.4\t3.5\t3.5\t3.5|一肖中\t50000\t200000\t1.4\t1\t0.5\t0.5|一肖不中\t50000\t200000\t1.4\t1\t0.5\t0.5|尾数中\t50000\t200000\t1.4\t0.5\t0.5\t0.5|尾数不中\t50000\t200000\t1.4\t0\t0\t0|二肖连中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|三肖连中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|四肖连中\t10000\t100000\t1.4\t1.5\t1.5\t1.5|五肖连中\t3000\t100000\t1.4\t1.5\t1.5\t1.5|二肖连不中\t10000\t100000\t1.4\t1.5\t1.5\t1.5|三肖连不中\t80000\t100000\t1.4\t1.5\t1.5\t1.5|四肖连不中\t50000\t100000\t1.4\t1.5\t1.5\t1.5|五肖连不中\t30000\t100000\t1.4\t1.5\t1.5\t1.5|二尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|四尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|五尾连中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|二尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|四尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|五尾连不中\t50000\t100000\t1.4\t1.5\t1.5\t0.5|三全中\t999\t3000\t15.5\t15.5\t15.5\t15.5|三中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二全中\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|特串\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|四不中\t100000\t100000\t1\t1.5\t1.5\t0.5|五不中\t100000\t100000\t1\t1.5\t1.5\t0.5|六不中\t100000\t100000\t1\t1.5\t1.5\t0.5|七不中\t100000\t100000\t1\t1.5\t1.5\t0.5|八不中\t100000\t100000\t1\t1.5\t1.5\t0.5|九不中\t100000\t100000\t1\t1.5\t1.5\t0.5|十不中\t100000\t100000\t1\t1.5\t1.5\t0.5|十一不中\t50000\t100000\t1\t2.5\t2.5\t2.5|十二不中\t50000\t100000\t1\t2.5\t2.5\t2.5|总肖\t100000\t100000\t1.4\t1\t2\t3|总肖单双\t100000\t100000\t1.4\t1\t2\t3|特肖\t100000\t100000\t1.5\t1.5\t1.5\t1.5|二肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|三肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|四肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|五肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|六肖\t100000\t100000\t2.5\t2.5\t2.5\t2.5|一粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|二粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|三粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|五粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|五中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|六中一\t10000\t10000\t0.5\t0.5\t0.5\t0.5|七中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|八中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|九中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|十中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|半波\t10000\t500000\t2.5\t2.5\t2.5\t2.5|特尾\t10000\t500000\t0.5\t0.5\t0.5\t0.5|头数\t20000\t100000\t0.5\t0.5\t0.5\t0.5|五行\t20000\t100000\t0.5\t0.5\t0.5\t0.5";
            }
            else if (lType == 3)
            {
                txt = "特码A\t100000\t100000\t13.2\t14.5\t15.5\t15.5|特码B\t100000\t100000\t3\t4.5\t5.5\t5.5|特码单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数单双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码合数大小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码家禽野禽\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码尾大尾小\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-红波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-蓝波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码色波-绿波\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大单小单\t100000\t100000\t1.4\t1.5\t2.5\t2.5|特码大双小双\t100000\t100000\t1.4\t1.5\t2.5\t2.5|正码A\t10000\t100000\t11.5\t11.5\t11.5\t11.5|正码B\t10000\t100000\t3.2\t3.5\t3.5\t3.5|总和单双\t100000\t100000\t1.4\t2.5\t2.5\t2.5|总和大小\t100000\t100000\t1.4\t2.5\t2.5\t2.5|正特\t100000\t100000\t13.2\t13.5\t13.5\t13.5|正特单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数单双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特合数大小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特家禽野禽\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特尾大尾小\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-红波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-蓝波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特色波-绿波\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大单小单\t100000\t100000\t1.4\t3.5\t3.5\t3.5|正特大双小双\t100000\t100000\t1.4\t3.5\t3.5\t3.5|一肖中\t100000\t200000\t1.4\t1\t0.5\t0.5|一肖不中\t100000\t100000\t1.4\t1\t0.5\t0.5|尾数中\t100000\t200000\t1\t0.5\t0.5\t0.5|尾数不中\t100000\t200000\t1\t0\t0\t0|二肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|三肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|四肖连中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|五肖连中\t3000\t100000\t1.5\t1.5\t1.5\t1.5|二肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|三肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|四肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|五肖连不中\t50000\t100000\t1.5\t1.5\t1.5\t1.5|二尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|四尾连中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|五尾连中\t50000\t50000000\t0.5\t1.5\t1.5\t0.5|二尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|四尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|五尾连不中\t50000\t100000\t0.5\t1.5\t1.5\t0.5|三全中\t1000\t3000\t15.5\t15.5\t15.5\t15.5|三中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二全中\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X\t2000\t6000\t15.5\t15.5\t15.5\t15.5|特串\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|三中二X之中三\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|二中特X之中二\t2000\t6000\t15.5\t15.5\t15.5\t15.5|四不中\t50000\t100000\t1\t1.5\t1.5\t0.5|五不中\t100000\t100000\t1\t1.5\t1.5\t0.5|六不中\t50000\t100000\t1\t1.5\t1.5\t0.5|七不中\t50000\t100000\t1\t1.5\t1.5\t0.5|八不中\t50000\t100000\t1\t1.5\t1.5\t0.5|九不中\t50000\t100000\t1\t1.5\t1.5\t0.5|十不中\t50000\t100000\t1\t1.5\t1.5\t0.5|十一不中\t50000\t100000\t1\t2.5\t2.5\t2.5|十二不中\t50000\t100000\t1\t2.5\t2.5\t2.5|总肖\t30000\t50000\t1.4\t1\t2\t3|总肖单双\t30000\t50000\t1.4\t1\t2\t3|特肖\t10000\t50000\t1.5\t1.5\t1.5\t1.5|二肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|三肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|四肖\t10000\t50000\t2.5\t2.5\t2.5\t2.5|五肖\t30000\t50000\t2.5\t2.5\t2.5\t2.5|六肖\t30000\t100000\t1.4\t2.5\t2.5\t2.5|一粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|二粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|三粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|五粒任中\t10000\t100000\t1.5\t1.5\t1.5\t1.5|四中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|五中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|六中一\t10000\t10000\t0.5\t0.5\t0.5\t0.5|七中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|八中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|九中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|十中一\t10000\t100000\t0.5\t0.5\t0.5\t0.5|半波\t10000\t500000\t2.5\t2.5\t2.5\t2.5|特尾\t10000\t500000\t0.5\t0.5\t0.5\t0.5|头数\t20000\t100000\t0.5\t0.5\t0.5\t0.5|五行\t20000\t100000\t0.5\t0.5\t0.5\t0.5";
            }


            string[] arr = txt.Split('|');



            foreach (string s in arr)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    string[] arr2 = s.Split('\t');

                    string playName = arr2[0];
                    string max1 = arr2[1];
                    string max2 = arr2[2];
                    string a = arr2[3];
                    string b = arr2[4];
                    string c = arr2[5];
                    string d = arr2[6];


                    string sql = "insert into tuishuiinfo(userId,lType,playname,max1,max2,A,B,C,D)  values(@userId,@lType,@playname,@max1,@max2,@A,@B,@C,@D)";


                    SqlParameter[] pms =
                    {
                        new SqlParameter("@userId",userId),
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@playname",playName),
                        new SqlParameter("@max1",max1),
                        new SqlParameter("@max2",max2),
                        new SqlParameter("@A",a),
                        new SqlParameter("@B",b),
                        new SqlParameter("@C",c),
                        new SqlParameter("@D",d),
                        
                    };

                    SqlHelper.ExecuteNonQuery(sql, pms);

                }
            }



        }







        public static decimal FormatDigit(decimal d)
        {

            string s = d + "";

            while (s.EndsWith("0") && s.Contains("."))
            {

                s = s.TrimEnd('0');
            }

            if (s.EndsWith("."))
            {
                s = s.TrimEnd('.');
            }

            return decimal.Parse(s);
        }



        //盈亏记录sql
        public static string GetProfitLossSql(int userId, int type, decimal money, decimal currentMoney, string mark)
        {
            //string subTime = DateTime.Now.ToString();


            string sql = "insert into ProfitLoss(UserId,Money,CurrentMoney,Type,Mark) values(" + userId + "," + money + "," + currentMoney + "," + type + ",'" + mark + "')";

            return sql;
        }

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

            sql = "select peilv from  peilvinfo  where  lType = @lType and PanKou=@PanKou and PlayBigType=@PlayBigType and PlaySmallType=@PlaySmallType";

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


            if (bigPlayName.Contains("连"))
            {
                if (playName != "三全中" && playName != "二全中" && playName != "特串")
                {
                    //return 1.134;

                    arr = o.ToString().Split('/');

                    return double.Parse(arr[0]);
                }
            }



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


        #region 判断单子的正确性

        public static bool JudgeBetCorrect(int lType, string bigPlayName, string smallPlayName, string betInfo)
        {
            string[] arr = null;   // betInfo.Split('|');
            string betNum = "";   //arr[2];


            string[] betArr = betInfo.Split(',');

            ////特殊情况
            //if (betNum.StartsWith("+") || betNum.StartsWith("-"))
            //{
            //    return false;
            //}


            #region 六合彩

            #region 特码 正码 正特

            if (bigPlayName.Contains("特码") || bigPlayName.Contains("正码") || bigPlayName.Contains("正特"))
            {
                arr = betInfo.Split('|'); //多注

                foreach (string s in arr)
                {
                    string[] arr2 = s.Split('-');
                    betNum = arr2[0];

                    if (Util.IsNumberic(betNum)) //如果是数字
                    {
                        betArr = betNum.Split(',');

                        //01-49限制
                        if (!JudgeNumCorrect(betArr))
                        {
                            return false;
                        }
                    }
                }
            }

            #endregion

            #region 不中

            else if (bigPlayName == "自选不中" && smallPlayName.Contains("不中"))
            {

                int len = 5;

                if (smallPlayName.Contains("六")) len = 6;
                else if (smallPlayName.Contains("七")) len = 7;
                else if (smallPlayName.Contains("八")) len = 8;
                else if (smallPlayName.Contains("九")) len = 9;
                else if (smallPlayName.Contains("十")) len = 10;



                //长度限制
                if (!JudgeBetNumLength(betArr, len, 20))
                {
                    return false;
                }

                //01-49限制
                if (!JudgeNumCorrect(betArr))
                {
                    return false;
                }

                //重复号码限制
                if (JudgeBetNumHasSame(betArr))
                {
                    return false;
                }
            }



            #endregion

            #region 连肖

            else if (smallPlayName.Contains("肖连"))
            {


                int len = 2;

                if (smallPlayName.Contains("三")) len = 3;
                else if (smallPlayName.Contains("四")) len = 4;
                else if (smallPlayName.Contains("五")) len = 5;

                //长度限制
                if (!JudgeBetNumLength(betArr, len, 12))
                {
                    return false;
                }

                //生肖限制
                if (!JudgeNumCorrectForSX(betArr))
                {
                    return false;
                }

                //重复号码限制
                if (JudgeBetNumHasSame(betArr))
                {
                    return false;
                }
            }






            #endregion

            #region 连尾

            else if (smallPlayName.Contains("尾连"))
            {

                int len = 2;

                if (smallPlayName.Contains("三")) len = 3;
                else if (smallPlayName.Contains("四")) len = 4;
                else if (smallPlayName.Contains("五")) len = 5;

                //长度限制
                if (!JudgeBetNumLength(betArr, len, 8))
                {
                    return false;
                }

                //连尾限制
                if (!JudgeNumCorrectForLW(betArr))
                {
                    return false;
                }

                //重复号码限制
                if (JudgeBetNumHasSame(betArr))
                {
                    return false;
                }
            }





            #endregion

            #region 连码

            else if (smallPlayName.Contains("三全中") || smallPlayName.Contains("三中二"))
            {
                //长度限制
                if (!JudgeBetNumLength(betArr, 3, 10))
                {
                    return false;
                }

                //01-49限制
                if (!JudgeNumCorrect(betArr))
                {
                    return false;
                }

                //重复号码限制
                if (JudgeBetNumHasSame(betArr))
                {
                    return false;
                }
            }
            else if (betInfo.Contains("二全中") || betInfo.Contains("二中特") || betInfo.Contains("特串"))
            {
                //长度限制
                if (!JudgeBetNumLength(betArr, 2, 10))
                {
                    return false;
                }

                //01-49限制
                if (!JudgeNumCorrect(betArr))
                {
                    return false;
                }

                //重复号码限制
                if (JudgeBetNumHasSame(betArr))
                {
                    return false;
                }
            }

            #endregion



            #endregion




            return true;
        }



        public static bool JudgeBetNumHasSame(string[] betArr)
        {

            for (int i = 0; i < betArr.Length; i++)
            {
                for (int j = i + 1; j < betArr.Length; j++)
                {
                    if (betArr[i] == betArr[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public static bool JudgeBetNumLength(string[] betArr, int len1, int len2)
        {
            int len = betArr.Length;

            if (len < len1 || len > len2)
            {
                return false;
            }

            return true;
        }


        public static bool JudgeIsAllNumFor6(string[] betArr)
        {
            int num = -1;

            foreach (string s in betArr)
            {
                if (!Util.IsNumberic(s))
                {
                    return false;
                }
            }

            return true;
        }


        public static bool JudgeNumCorrect(string[] betArr)
        {
            int num = -1;

            foreach (string s in betArr)
            {


                //必须在 01-49之间
                num = int.Parse(s);

                if (num > 49 || num < 1)
                {
                    return false;
                }

            }


            return true;
        }


        public static bool JudgeNumCorrectForSX(string[] betArr)
        {
            string shengxiao = "鼠,牛,虎,兔,龙,蛇,马,羊,猴,鸡,狗,猪";
            string[] shengxiaoArr = shengxiao.Split(',');

            foreach (string s in betArr)
            {
                if (!shengxiaoArr.Contains(s))
                {
                    return false;
                }
            }

            return true;
        }


        public static bool JudgeNumCorrectForLW(string[] betArr)
        {
            string lianwei = "0尾，1尾，2尾，3尾，4尾，5尾，6尾，7尾，8尾，9尾";

            foreach (string s in betArr)
            {
                if (!lianwei.Contains(s))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

    }
}
