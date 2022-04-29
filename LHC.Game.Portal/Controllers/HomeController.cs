using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHC.Game.Portal.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(int id = 3)
        {
            ViewBag.lType = id;

            string sql = "select sum(betcount*unitmoney) from BettingRecord where WinState = 1 and userid = " + LoginUser.Id;

            ViewBag.weijieMoney = Util.GetCountByDataBase(sql); //未结金额


            //今日输赢

            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time1 = date + " 0:0:0";
            string time2 = date + " 23:59:59";

            sql = "select sum( winMoney + tuishui5 -   betcount*unitmoney) from BettingRecord where WinState > 1 and  SubTime > '" + time1 + "' and  SubTime <'" + time2 + "' and userid = " + LoginUser.Id;

            ViewBag.todaySY = Util.GetCountByDataBase3(sql);



            //公告

            sql = "select top(1)* from  Notice order by Id desc";
            Notice n = Util.ReaderToModel<Notice>(sql);
            ViewBag.notice = n.Content;



            return View();
        }



        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            //LogHelper.WriteLog(Request.Headers["User-Agent"]);




            //真实用户
            string sql = "select top(1)* from UserInfo where UserName=@UserName and Password=@Password";
            SqlParameter[] pms =
            {
                new SqlParameter("@UserName",username),
                new SqlParameter("@Password",password),
            };


            UserInfo userInfo = Util.ReaderToModel<UserInfo>(sql, pms);

            if (userInfo == null)
            {
                return Content("error");
            }
            else if (userInfo.IsDJ != null && userInfo.IsDJ == true)
            {
                return Content("dongjie");
            }
            else
            {
                string guid = Guid.NewGuid().ToString();

                HttpCookie hc = new HttpCookie("UserId", guid);
                Response.Cookies.Add(hc); //mm + cookie 保存登陆状态   关闭浏览器cookie消失

                string userId = userInfo.Id + "";

                //CacheHelper.SetCache(guid, userId, DateTime.Now.AddMinutes(20));
                SetCache(guid, userId);




                //更新UserInfo
                string time = DateTime.Now.ToString();

                string ip = GetIp();


                string updateUserLoginInfoSql = "update UserInfo set LastLoginTime='" + time + "',GUID= '" + guid + "',LoginCount=LoginCount+1,LastLoginIp = '" + ip + "' where Id=" + userInfo.Id;
                SqlHelper.ExecuteNonQuery(updateUserLoginInfoSql);


                //登陆日志
                Util.SaveLoginLog(userInfo.Id, ip, Request.Headers["User-Agent"], false);



                return Content("ok");
            }
        }


        public ActionResult LoginOut()
        {
            HttpCookie hc = Request.Cookies["UserId"];
            string guid = hc.Value;

            //清除Cookie
            hc.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(hc);

            //清缓存
            CacheManager.Remove(guid);

            //
            string sql = "update UserInfo set GUID = null where Id =" + LoginUser.Id;
            SqlHelper.ExecuteNonQuery(sql);


            return Content("ok");
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPwd()
        {

            return View();
        }


        [HttpPost]
        public ActionResult ModifyPwd(string pwd, string pwd2)
        {

            if (pwd != LoginUser.PassWord)
            {
                return Content("原密码错误");
            }
            else
            {
                string sql = "update UserInfo set PassWord=@NewPwd where Id=" + LoginUser.Id;

                SqlParameter[] pms = { new SqlParameter("@NewPwd", pwd2), };
                SqlHelper.ExecuteNonQuery(sql, pms);

                return Content("ok");
            }

        }

    }
}
