using ShenZhen.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHC.Game.Portal.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {

            string issue = Util5.GetCurrentIssue(3);

            string  time1 = Util5.GetRemainingTime(3);

            string time2 = Util5.GetOpenRemainingTime(3);

            string result = issue + "----" + time1 + "----" + time2;

            return Content(result);

            return View();
        }

    }
}
