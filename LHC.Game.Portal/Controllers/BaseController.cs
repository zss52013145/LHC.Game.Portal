using ShenZhen.Lottery.Model;
using ShenZhen.Lottery.Public;
using ShenZhen.Lottery.Public.Cache;
using ShenZhen.Lottery.Public.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHC.Game.Portal.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public UserInfo LoginUser;

        public static readonly ICacheManager CacheManager = new RedisCache();



        #region 登陆校验

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //ViewBag.siteName = siteName;
            //ViewBag.siteTitle = siteTitle;
            //ViewBag.siteUrl = siteUrl;
            //ViewBag.kefu = kefu;
            //ViewBag.pcUrl = pcUrl;
            //ViewBag.copyright = copyright;
            //ViewBag.appleUrl = appleUrl;
            //ViewBag.vivoShopUrl = vivoShopUrl;



           
            string controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string action = filterContext.RouteData.Values["action"].ToString().ToLower();



            if (controller == "base" ||

                 controller == "weihu" ||

                (controller == "home" && (action == "login" || action == "register" || action == "getvcode")) ||

                (controller == "lottery" && (action == "getlastopenreocrd" || action == "getlastopenreocrd2" || action == "checklastissue" || action == "getremainingtime" || action == "getcurrentissueandtime" || action == "getremainopentimebytype" || action == "getopenremainingtime" || action == "getcurrentissue"))

                )
            {

            }
            else
            {
                HttpCookie hc = filterContext.HttpContext.Request.Cookies["UserId"];

                if (hc == null)
                {
                    filterContext.Result = new RedirectResult("/home/login");
                }
                else
                {
                    string guid = hc.Value;
                    int userId = GetLoginId(guid);

                    if (userId == -1)
                    {
                        filterContext.Result = new RedirectResult("/home/login");
                    }
                    else
                    {

                        LoginUser = Util.GetEntityById<UserInfo>(userId);

                        if (LoginUser.IsDJ == true)
                        {
                            filterContext.Result = new RedirectResult("/home/login");
                        }
                        else
                        {
                            if (action != "getlotterymoney") //特殊情况
                            {
                                SetCache(guid, userId);         //滑动 缓存时间
                            }
                        }

                        ViewBag.user = LoginUser;

                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }


        #endregion


        #region Util


        //2022-4-19
        public int GetLoginId(string guid)
        {
            try
            {
                string userId = CacheManager.Get<string>(guid);

                return int.Parse(userId);

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
            }

            return -1;
        }

        public void SetCache(string key, object value)
        {
            try
            {
                CacheManager.Set(key, value, 60 * 24 * 7);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
            }
        }

        //特殊情况 判断登录
        public void JudeIsLogin()
        {
            //判断是否登录
            HttpCookie hc = Request.Cookies["UserId"];
            if (hc != null)
            {
                LoginUser = Util.GetEntityById<UserInfo>(GetLoginId(hc.Value));
            }
        }

        public string GetIp()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(ip))
            {
                ip = Request.UserHostAddress;
            }

            return ip;
        }

        public void ClearCookie(string name)
        {
            HttpCookie hc = Response.Cookies[name];
            hc.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(hc);
        }


        public void AddCookie(string name)
        {
            HttpCookie hc = new HttpCookie(name, "1");
            Response.Cookies.Add(hc);
        }


        #endregion


    }
}
