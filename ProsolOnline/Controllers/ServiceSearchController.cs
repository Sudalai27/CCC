using Prosol.Common;
using Prosol.Core.Interface;
using Prosol.Core.Model;

using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Prosol.Core;

namespace ProsolOnline.Controllers
{
    public class ServiceSearchController : Controller
    {
        private readonly IServiceSearch _ServiceSearchService;
        public ServiceSearchController(IServiceSearch ServiceSearchService)
        {
            _ServiceSearchService = ServiceSearchService;
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

        // GET: ServiceSearch
        public ActionResult ServiceSearch()
        {

            if (CheckAccess("Service Search") == 1)
                return View("ServiceSearch");
            else if (CheckAccess("Service Search") == 0)
                return View("Denied");
            else return View("Login");
            //return View("Search");
        }



        public ActionResult ServiceDashboard()
        {

            if (CheckAccess("Service Dashboard") == 1)
                return View("ServiceDashboard");
            else if (CheckAccess("Service Dashboard") == 0)
                return View("Denied");
            else return View("Login");

        }
        public JsonResult getcategory()
        {
            var result = _ServiceSearchService.getcategory();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getgroupp(string ServiceCategorycode)
        {
            var strloc = _ServiceSearchService.getgroupp( ServiceCategorycode);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getUOM()
        {
            var result = _ServiceSearchService.getUOM();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getServiceCode(string cat, string grp, string uom)
        {
            var result = _ServiceSearchService.getServiceCode(cat, grp, uom);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gettabledetails(string ServiceCode)
        {
            var result = _ServiceSearchService.gettabledetails(ServiceCode);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getdetailsforcode(string code)
        {
            var result = _ServiceSearchService.getdetailsforcode(code);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ServiceMasterSearch()
        {
            //var obj = Request.Form["ngmodel"]; JsonConvert.DeserializeObject<ServiceCategory>(obj);
            Prosol_RequestService prs = new Prosol_RequestService();
            //var ServiceCategoryCode = Request.Form["ServiceCategoryCode"];
            //var ServiceGroupCode = Request.Form["ServiceGroupCode"];
            //var UomCode = Request.Form["UomCode"];
            //var ServiceCode = Request.Form["ServiceCode"];
            //var ShortDesc = Request.Form["ShortDesc"];
            //var LongDesc = Request.Form["LongDesc"];

            //prs.ServiceCategoryCode = JsonConvert.DeserializeObject<string>(ServiceCategoryCode);
            //prs.ServiceCategoryCode = ServiceCategoryCode != "undefined" ? JsonConvert.DeserializeObject<string>(ServiceCategoryCode): "undefined";
            //prs.ServiceGroupCode = ServiceGroupCode != "undefined"? JsonConvert.DeserializeObject<string>(ServiceGroupCode):"undefined";
            //prs.UomCode = UomCode != "undefined" ? JsonConvert.DeserializeObject<string>(UomCode) : "undefined";
            //prs.ServiceCode = ServiceCode != "undefined" ? JsonConvert.DeserializeObject<string>(ServiceCode) : "undefined";
            //prs.ShortDesc = ShortDesc != "undefined" ? JsonConvert.DeserializeObject<string>(ShortDesc) : "undefined";
            //prs.LongDesc = LongDesc != "undefined" ? JsonConvert.DeserializeObject<string>(LongDesc) : "undefined";
            //prs.LongDesc = LongDesc != "undefined" ? JsonConvert.DeserializeObject<string>(LongDesc) : "undefined";
            //prs1.LongDesc = LongDesc != "undefined" ? JsonConvert.DeserializeObject<string>(LongDesc) : "undefined";

            var search = Request.Form["search"];
            var searchkey = Request.Form["searchkey"];
            search = search != "undefined" ? JsonConvert.DeserializeObject<string>(search) : "undefined";
            searchkey = searchkey != "undefined" ? JsonConvert.DeserializeObject<string>(searchkey) : "undefined";

            var result = _ServiceSearchService.ServiceMasterSearch(search, searchkey);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult getdetailsforsd(string sd)
        {
            var result = _ServiceSearchService.getdetailsforsd(sd);
            return this.Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult getdetailsforld(string ld)
        {
            var result = _ServiceSearchService.getdetailsforld(ld);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }



        //srvicedashboard
        public JsonResult BindTotalItemService()
        {

            // var usrInfo = _ServiceSearchService.getimage(Convert.ToString(Session["UserId"]));
            var ListDash = _ServiceSearchService.BindTotalItem();

            return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        }
        //category
        public JsonResult BindTotalItemServiceCategory()
        {


            var ListDash = _ServiceSearchService.BindTotalItemcategory();

            return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        }

        //group
        public JsonResult BindTotalItemServiceGroup()
        {

            var ListDash = _ServiceSearchService.BindTotalItemcategoryGroup();

            return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        }




    }
}