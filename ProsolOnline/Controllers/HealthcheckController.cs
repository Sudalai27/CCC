using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProsolOnline.Controllers
{
    public class HealthcheckController : Controller
    {
        // GET: Healthcheck
        [Authorize]
        public ActionResult Index()
        {
            return View("Healthcheck");
        }

        [Authorize]
        public ActionResult Healthcheck()
        {
            return View();
        }

    }
}