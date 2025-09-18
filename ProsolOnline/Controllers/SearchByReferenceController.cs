using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Prosol.Core;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using Newtonsoft.Json;

namespace ProsolOnline.Controllers
{
    public class SearchByReferenceController : Controller
    {

        private readonly ISearchByReference _SearchByReferenceService;  
        public SearchByReferenceController(ISearchByReference SearchByReferenceService)
        {
            _SearchByReferenceService = SearchByReferenceService;
        }

        // GET: SearchByReference
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("Search by REF") == 1)
                return View("Searchbyref");
            else if (CheckAccess("Search by REF") == 0)
                return View("Denied");
            else return View("Login");

           
        }
        public ActionResult Searchbyref()
        {
            if (CheckAccess("Search by REF") == 1)
                return View();
            else if (CheckAccess("Search by REF") == 0)
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
        public JsonResult chkandshowresults(string Noun, string Modifier, string Industry_Sector, string Material_Type, string Inspection_Type, string Plant, string Profit_Center, string Storage_Location, string Storage_Bin, string Valuation_Class, string Price_Control, string MRP_Type, string MRP_Controller, string Procurement_Type)
        {
           
                var result = _SearchByReferenceService.getresultsforrest(Noun, Modifier, Industry_Sector, Material_Type, Inspection_Type, Plant, Profit_Center, Storage_Location, Storage_Bin, Valuation_Class, Price_Control, MRP_Type, MRP_Controller, Procurement_Type);
           


            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetPlants()
        {
            var result = _SearchByReferenceService.GetPlants();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetStorageLocations(string Plant)
        {
            var result = _SearchByReferenceService.GetStorageLocations(Plant);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetStorageBin(string Plant, string sl)
        {
            var result = _SearchByReferenceService.GetStorageBin(Plant,sl);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetResultForCode(string code)
        {
            var result = _SearchByReferenceService.GetResultForCode(code);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetResultFornoun_modifier(string noun, string modifier)
        {
            var result = _SearchByReferenceService.GetResultFornoun_modifier(noun,modifier);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}