using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProsolOnline.Controllers
{
    public class ServiceReleaseController : Controller
    {
        // GET: ServiceRelease
        public int CheckAccess(string pageName)
        {
            string pages = Convert.ToString(Session["access"]);
            if (!string.IsNullOrEmpty(pages))
            {
                String[] stringArray = pages.Split(',');
                if (Array.IndexOf(stringArray, pageName) > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    return 1;
                else return 0;

            }
            else return -1;

        }
        public ActionResult Index()
        {
            if (CheckAccess("Service Release") == 1)
                return View();
            else if (CheckAccess("Service Release") == 0)
                return View("Denied");
            else return View("Login");
            //return View();
        }
    }
}