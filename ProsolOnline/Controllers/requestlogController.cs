using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Prosol.Core;
using Prosol.Core.Model;
using ProsolOnline.Models;
using Newtonsoft.Json;
using Prosol.Core.Interface;
using System.Globalization;

namespace ProsolOnline.Controllers
{
    public class requestlogController : Controller
    {
        private readonly IItemRequestLog _ItemRequestLogService;        
        public requestlogController(IItemRequestLog ItemRequestLogService)
        {
            _ItemRequestLogService = ItemRequestLogService;
           
        }

        // GET: requestlog
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("Request Log") == 1)
                return View("req_log");
            else if (CheckAccess("Request Log") == 0)
                return View("Denied");
            else return View("Login");           

        }
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
        [Authorize]
        public JsonResult getRequested_Records()
        {
            var req_records = _ItemRequestLogService.get_itemsToApprove(Session["userid"].ToString());         
            foreach (Prosol_Request prm in req_records)
            {
                string str = DateTime.Parse(prm.requestedOn.ToString()).ToString("dd/MM/yyyy");
                //string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                prm.requestStatus = str;
            }

            return this.Json(req_records, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult getApproved_Records()
        {
            var app_records = _ItemRequestLogService.getApproved_Records(Session["userid"].ToString());

            // load approved date in request status
            foreach (Prosol_Request prm in app_records)
            {
                //string str = prm.approvedOn.ToLongDateString();
                string str = DateTime.Parse(prm.approvedOn.ToString()).ToString("dd/MM/yyyy");
                //  string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                prm.requestStatus = str;
            }

            return this.Json(app_records, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult getRejected_Records()
        {
            var rej_records = _ItemRequestLogService.getRejected_Records(Session["userid"].ToString());

            // load approved date in request status
            foreach (Prosol_Request prm in rej_records)
            {
                int index = prm.rejectedOn.ToString().IndexOf(" ");

                string str = prm.rejectedOn != null ? prm.rejectedOn.ToString().Substring(0, index) : null;
                prm.requestStatus = str;
                //prm.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //prm.requestStatus = prm.rejectedOn.ToString().Substring(0, 10);
                // string str = prm.rejectedOn.ToLongDateString();
                //string str = DateTime.Parse(prm.rejectedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                //prm.requestStatus = str;
            }

            return this.Json(rej_records, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult getClarification_Records()
        {
            var rej_records = _ItemRequestLogService.getClarification_Records(Session["userid"].ToString());

            // load approved date in request status
            foreach (Prosol_Request prm in rej_records)
            {
                int index = prm.rejectedOn.ToString().IndexOf(" ");

                string str = prm.rejectedOn != null ? prm.rejectedOn.ToString().Substring(0, index) : null;
                prm.requestStatus = str;
                //prm.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //prm.requestStatus = prm.rejectedOn.ToString().Substring(0, 10);
                // string str = prm.rejectedOn.ToLongDateString();
                //string str = DateTime.Parse(prm.rejectedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                //prm.requestStatus = str;
            }

            return this.Json(rej_records, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult getsingle_requested_record(string abcsony)
        {
            var singlerecord = _ItemRequestLogService.getsingle_requested_record(abcsony);

            return this.Json(singlerecord, JsonRequestBehavior.AllowGet);

        }


    }
}