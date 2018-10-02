using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? check)
        {
            if (check==1)
            {
                Session["PerdoruesiID"] = null;
                return RedirectToAction("Login", "Account");
            }
            try
            {

                int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                int? RoliID = int.Parse(Session["RoliID"].ToString());
                ViewBag.RoliID = RoliID;
                return View();

            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}