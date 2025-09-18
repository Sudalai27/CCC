using Prosol.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProsolOnline.Controllers
{
    public class ServiceReviewController : Controller
    {

        private readonly IServiceReport _ServiceReportService;
        
        
        public ServiceReviewController(IServiceReport ServiceReportService)
        {

            _ServiceReportService = ServiceReportService;




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



        // GET: ServiceReview

        public ActionResult Index()
        {


            if (CheckAccess("Service Review") == 1)
                return View();
            else if (CheckAccess("Service Review") == 0)
                return View("Denied");
            else return View("Login");
            //return View();
        }


        //bulkcategory
        public JsonResult bulkCategory_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.BulkServiceCategory(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        //bulkgroup
        public JsonResult bulkGroup_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.bulkGroupUpload(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        //bulkmaincode

        public JsonResult bulkServiceMainCode_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.bulkServiceMainCodeUpload(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //bulksubcode
        public JsonResult bulkServiceSubCode_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.bulkServiceSubCodeUpload(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //bulksubsubcode
        public JsonResult bulkServiceSubSubCode_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.bulkServiceSubSubCodeUpload(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        //bulkcharacteristic

        public JsonResult BulkCharacteristic_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.BulkCharacteristicUpload(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //bulkvalue
        public JsonResult Bulkvalue_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _ServiceReportService.BulkvalueUpload(file);
                }

            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}