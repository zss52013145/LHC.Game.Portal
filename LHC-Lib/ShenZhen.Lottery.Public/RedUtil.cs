using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Public
{
    public static class RedUtil
    {
        public static List<double> FenBao(double money, int count)
        {
            //double money = 100;

            //int count = 5;         //包数


            double[] arr = new double[count];
            double[] arr2 = new double[count];



            for (int i = 0; i < count - 1; i++)
            {
                double t = RandomANum();

                if (t > 0)
                {
                    arr[i] = t;
                }
            }

            Array.Sort(arr);

            double sum = arr.Sum();

            int index = count - 1;

            while (sum >= 1)
            {

                if (arr[index] != 0 && arr[index] > 0.01)
                {
                    arr[index] = arr[index] / 2;

                    sum = arr.Sum();
                }

                index--;

                //重复
                if (index == -1)
                {
                    index = count - 1;
                }
            }

            arr[0] = 1 - sum;



            for (int i = 0; i < count; i++)
            {
                double m1 = arr[i] * money;
                m1 = SaveTwoDot(m1);

                arr2[i] = m1;
            }


            //如果 特殊情况
            double t8 = money / count;

            if (t8 >= 0.01 && t8 <= 0.1)
            {
                for (int i = 0; i < count; i++)
                {
                    arr2[i] = SaveTwoDot(t8);
                }
            }


            arr2[0] = 0;
            sum = arr2.Sum();
            arr2[0] = money - sum;
            arr2[0] = Convert.ToDouble(arr2[0].ToString("0.00"));


            List<double> list = Util.ListRandom(arr2.ToList());


            return list;

        }


        //随机一个数字
        public static double RandomANum()
        {

            byte[] buffer = Guid.NewGuid().ToByteArray();

            int iSeed = BitConverter.ToInt32(buffer, 0);

            //long tick = DateTime.Now.Ticks;
            //Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            Random ran = new Random(iSeed);

            double temp = ran.NextDouble();

            return temp;

        }

        public static double SaveTwoDot(double d)
        {

            string t = d + "";

            if (!t.Contains("."))
            {
                t = t + ".0";
            }

            string[] arr = t.Split('.');

            string t2 = "";
            if (arr[1].Length == 1)
            {
                t2 = arr[0] + "." + arr[1].Substring(0, 1);
            }
            else
            {
                t2 = arr[0] + "." + arr[1].Substring(0, 2);
            }

            return double.Parse(t2);

            //return d;

        }


    }
}
