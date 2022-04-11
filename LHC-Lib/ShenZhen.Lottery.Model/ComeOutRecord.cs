using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShenZhen.Lottery.Public;

namespace ShenZhen.Lottery.Model
{
    public partial class ComeOutRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int BankId { get; set; }
        public string OrderId { get; set; }
        public int Type { get; set; }           //1.充值 2.提现
        public int Money { get; set; }
        public int PayType { get; set; }
        public int State { get; set; }      //状态  1.处理中 2.失败 3.成功
        public DateTime SubTime { get; set; }
        public int ChuKuanBankId { get; set; }
        public DateTime? YSTime { get; set; }
        public DateTime? ChuKuanTime { get; set; }
        public UserInfo User { get; set; }
        public BankInfo Bank { get; set; }
        public BankInfo ChuKuanBank { get; set; }
        public int YSAdminId { get; set; }
        public int ChuKuanAdminId { get; set; }

        public string Line { get; set; }

        public string Remark { get; set; }   //手动入款 备注
        public bool IsChaLiuShui { get; set; }   //是否差流水

        public decimal FanShuiMoney { get; set; }          //投注额
        public decimal FanShuiRate { get; set; }  //返水比例

        public int? ActivityId { get; set; }                //活动ID

        public decimal ChuKuanSXF { get; set; }


        public Activity Activity { get; set; }

        public string FailReason { get; set; }



        //a.c8 需要的属性
        public int lType { get; set; }

        public UserInfo toppestUser { get; set; }


        //总代的占成数
        public string ShowZhanCheng2
        {
            get
            {
                if (toppestUser.PId > 0)
                {
                    return toppestUser.ShowZhanCheng;
                }

                return "";
            }
        }


        public string ShowDirectTopName2
        {
            get
            {
                toppestUser = Util.GetEntityById<UserInfo>(this.UserId);

                if (toppestUser.PId > 0)
                {
                    while (toppestUser.PId != 32652)            //--32652
                    {
                        toppestUser = Util.GetEntityById<UserInfo>(toppestUser.PId);
                    }


                    //----------------
                    if (toppestUser.RealName != null)
                    {
                        return toppestUser.RealName;
                    }
                    else
                    {
                        return toppestUser.UserName;
                    }
                }

                return "";
            }
        }





        //拓展属性

        //手续费
        public string ShowChuKuanSXF
        {
            get
            {
                if (ChuKuanSXF == 0)
                {
                    return "--";
                }
                else
                {
                    return ChuKuanSXF.ToString("0.00");
                }
            }
        }

        public string ShowYouHui
        {
            get
            {
                if (ActivityId == null)
                {
                    return "--";
                }
                else
                {
                    string sql = "select * from Activity where Id=" + ActivityId;

                    Activity = Util.ReaderToModel<Activity>(sql);

                    if (Activity != null)
                    {
                        return Activity.Name;
                    }

                    return "";
                }
            }
        }



        //赠送金额
        public string ZengSongMoney
        {
            get
            {
                if (Activity != null)
                {
                    decimal rate = Activity.Rate;
                    return (Money * rate).ToString("0.00");
                }

                return "0";
            }
        }



        public string ShowType
        {
            get
            {

                if (Type == 1)
                {
                    return "入款";
                }
                else if (Type == 2)
                {
                    return "出款";
                }
                else if (Type == 3)
                {
                    return "充值优惠";
                }
                else if (Type == 4)
                {
                    return "每日返水";
                }
                else if (Type == 5)
                {
                    return "代理返水";
                }

                return "";
            }
        }

        public string ShowMoney
        {
            get
            {
                if (Type == 2)
                {
                    return (Money - ChuKuanSXF).ToString("0.00");
                }
                return Money.ToString("0.00");
            }
        }

        public string ShowMoney2
        {
            get
            {
                return Money.ToString("0.00");
            }
        }


        public string ShowMoney3
        {
            get
            {
                if (this.Type == 3)
                {
                    return this.Money.ToString("0.000");
                }
                else
                {
                    return this.FanShuiMoney.ToString("0.000");
                }
            }
        }


        public string ShowMoneyForChuRuKuanRecord
        {
            get
            {
                if (this.Type == 1)
                {
                    return Money.ToString("0.00");
                }
                else
                {
                    return "<font color='red'>" + Money.ToString("0.00") + "</font>";
                }
            }
        }

        public string ShowTime
        {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ShowChuKuanTime
        {
            get
            {
                if (ChuKuanTime != null)
                {
                    return ((DateTime)ChuKuanTime).ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "";
            }
        }

        public string ShowYSTime
        {
            get
            {
                if (YSTime != null)
                {
                    return ((DateTime)YSTime).ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "";
            }
        }

        public string ShowState
        {
            get
            {
                if (State == 1)
                {
                    return "处理中";
                }
                else if (State == 2)
                {
                    return "失败";
                }
                else if (State == 3)
                {
                    return "成功";
                }
                else if (State == 4)
                {
                    return "审核通过";
                }

                return "";
            }
        }

        public string ShowUserName
        {
            get
            {
                User = Util.GetEntityById<UserInfo>(this.UserId);

                if (User != null)
                {
                    return User.UserName;
                }

                return "";
            }
        }

        public string ShowZhanCheng
        {
            get
            {
                User = Util.GetEntityById<UserInfo>(this.UserId);

                if (User != null)
                {
                    return User.ShowZhanCheng;
                }

                return "";
            }
        }


        public string ShowDirectTopName
        {
            get
            {
                User = Util.GetEntityById<UserInfo>(this.UserId);
                User = Util.GetEntityById<UserInfo>(User.PId);

                if (User != null)
                {
                    if (User.RealName != null)
                    {
                        return User.RealName;
                    }
                    else
                    {
                        return User.UserName;
                    }
                }

                return "";
            }
        }


        public string ShowRealName
        {
            get
            {
                if (User != null)
                {
                    return User.RealName;
                }

                return "";
            }
        }

        public string ShowPayType
        {
            get
            {
                //if (PayType == 1)
                //{
                //    return "网银转账";
                //}
                //else if (PayType == 2)
                //{
                //    return "柜台转账";
                //}
                //else if (PayType == 3)
                //{
                //    return "手机网银";
                //}
                //else if (PayType == 4)
                //{
                //    return "网银";
                //}
                //else if (PayType == 5)
                //{
                //    return "微信";
                //}
                //else if (PayType == 6)
                //{
                //    return "支付宝";
                //}

                if (PayType == 1)
                {
                    return "微信H5";
                }
                else if (PayType == 2)
                {
                    return "微信扫码";
                }
                else if (PayType == 3)
                {
                    return "QQ钱包H5";
                }
                else if (PayType == 4)
                {
                    return "QQ钱包扫码";
                }
                else if (PayType == 5)
                {
                    return "银行卡转账";
                }
                else if (PayType == 6)
                {
                    return "支付宝H5";
                }
                else if (PayType == 7)
                {
                    return "支付宝扫码";
                }
                else if (PayType == 8)
                {
                    return "网银";
                }



                return "";
            }
        }

        public string ShowBankName
        {
            get
            {
                string sql = "select * from BankInfo where Id=" + BankId;
                Bank = Util.ReaderToModel<BankInfo>(sql);

                if (Bank != null)
                {
                    return Bank.BankName;
                }

                return "";
            }
        }

        public string ShowChuKuanBankName
        {
            get
            {
                string sql = "select * from BankInfo where Id=" + ChuKuanBankId;
                ChuKuanBank = Util.ReaderToModel<BankInfo>(sql);

                if (ChuKuanBank != null)
                {
                    return ChuKuanBank.BankName;
                }

                return "";
            }
        }

        public string ShowBankRealName
        {
            get
            {

                if (Bank != null)
                {
                    return Bank.RealName;
                }

                return "";
            }
        }

        public string ShowChuKuanBankRealName
        {
            get
            {

                if (ChuKuanBank != null)
                {
                    return ChuKuanBank.RealName;
                }

                return "";
            }
        }

        public string ShowBankNum
        {
            get
            {

                if (Bank != null)
                {
                    return Bank.BankNum;
                }

                return "";
            }
        }

        public string ShowChuKuanBankNum
        {
            get
            {

                if (ChuKuanBank != null)
                {
                    return ChuKuanBank.BankNum;
                }

                return "";
            }
        }

        public string ShowZhiHangName
        {
            get
            {

                if (Bank != null)
                {
                    return Bank.ZhiHangName;
                }

                return "";
            }
        }

        public string ShowProvience
        {
            get
            {

                if (Bank != null)
                {
                    return Bank.Provience;
                }

                return "";
            }
        }

        public string ShowCity
        {
            get
            {

                if (Bank != null)
                {
                    return Bank.City;
                }

                return "";
            }
        }

        public string ShowYSAdmin
        {
            get
            {
                try
                {

                    //ChuKuanAdminId
                    Admin a = Util.GetEntityById<Admin>(YSAdminId);

                    return a.RealName;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        public string ShowChuKuanAdmin
        {
            get
            {
                if (Type == 1 && PayType != 5 && ChuKuanAdminId == 0)
                {
                    return "自动入款";
                }
                else
                {
                    //ChuKuanAdminId

                    Admin a = Util.GetEntityById<Admin>(this.ChuKuanAdminId);

                    if (a != null)
                    {
                        return a.RealName;
                    }
                }

                return "";
            }
        }

        public string ShowRuKuanState
        {
            get
            {
                if (Type == 1)
                {
                    if (State == 2)
                    {
                        return "入款失败";
                    }
                    else if (State == 3)
                    {
                        return "入款成功";
                    }
                }
                else if (Type == 2)
                {
                    if (State == 2)
                    {
                        return "出款失败";
                    }
                    else if (State == 3)
                    {
                        return "出款成功";
                    }
                }

                return "";
            }
        }



    }
}
