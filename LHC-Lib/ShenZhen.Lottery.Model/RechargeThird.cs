using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class RechargeThird
    {
        public int Id { get; set; }
        public string ThirdName { get; set; }
        public string RechargeLine { get; set; }
        public int RechargeType { get; set; }
        public int Money1 { get; set; }
        public int Money2 { get; set; }
        public bool Status { get; set; }
        public DateTime SubTime { get; set; }

        public string GroupId { get; set; }



        //扩展

        //用户组
        public string ShowGroup
        {
            get
            {
                if (GroupId != null)
                {
                    string result = "";

                    string[] arr = GroupId.Split(',');

                    foreach (string s in arr)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            result += Util.GetEntityById<UserGroup>(int.Parse(s)).Name + " ";
                        }


                    }

                    return result;
                }

                return "";
            }
        }


        public string ShowRechargeType
        {
            get
            {

                //APP  1 3 6
                //扫码  2 4 7
                //网银  8

                string result = "";
                if (RechargeType == 1)
                {
                    result = "微信APP支付";
                }
                else if (RechargeType == 2)
                {
                    result = "微信扫码支付";
                }
                else if (RechargeType == 3)
                {
                    result = "QQ钱包APP支付";
                }
                else if (RechargeType == 4)
                {
                    result = "QQ钱包扫码支付";
                }
                else if (RechargeType == 6)
                {
                    result = "支付宝APP支付";
                }
                else if (RechargeType == 7)
                {
                    result = "支付宝扫码支付";
                }
                else if (RechargeType == 8)
                {
                    result = "网银";
                }
                else if (RechargeType == 9)
                {
                    result = "银联H5";
                }
                else if (RechargeType == 10)
                {
                    result = "银联扫码";
                }

                return result;
            }
        }


    }
}
