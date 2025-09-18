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
    public class ValueStandardisationController : Controller
    {
        private readonly IValuestandardization _ValuestandardisationService;
        private readonly ICatalogue _CatalogueService;
        public ValueStandardisationController(IValuestandardization ValuestandardisationService,ICatalogue catalougeService)
        {
            _ValuestandardisationService = ValuestandardisationService;
            _CatalogueService = catalougeService;
        }
        // GET: ValueStandardisation
        [Authorize]
        public ActionResult Index()
        {

            if (CheckAccess("Value Standardization") == 1)
                return View("valuestandardisation");
            else if (CheckAccess("Value Standardization") == 0)
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
        public JsonResult GetNoun()
        {
            var Nounlist = _ValuestandardisationService.GetNoun();

            var result = Nounlist.Select(i => new { i.Noun}).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetModifier(string noun)
        {
            var Modifierlist = _ValuestandardisationService.GetModifier(noun);
            var result = Modifierlist.Select(i => new { i.Modifier }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        //GetAttributes
        [Authorize]
        public JsonResult GetAttributes(string noun,string modifier)
        {
            var Characteristiclist = _ValuestandardisationService.GetAttributes(noun,modifier);
            var result = Characteristiclist.Select(i => new { i.Characteristic }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult load_values(string noun, string modifier, string attribute)
        {
            var valuelist = _ValuestandardisationService.load_values(noun, modifier, attribute);
            var charac = new List<Prosol_AttributeList>();

            foreach (Prosol_Datamaster dm in valuelist)
            {

                if (dm.Characteristics != null)
                {
                    foreach (Prosol_AttributeList al in dm.Characteristics)
                    {
                        var AttrMdl = new Prosol_AttributeList();
                        if (al.Characteristic == attribute && al.Value != null && al.Value != "")
                        {
                            AttrMdl.Characteristic = al.Characteristic;
                            AttrMdl.Value = al.Value;
                            AttrMdl.UOM = al.UOM != "" ? al.UOM : null;
                            AttrMdl.Squence = al.Squence;
                            AttrMdl.ShortSquence = al.ShortSquence;
                            var res = _CatalogueService.CheckValue(noun, modifier, al.Characteristic, al.Value);
                           
                            if (res != "false")
                            {
                                AttrMdl.Source = res;
                            }

                            charac.Add(AttrMdl);

                        }
                    }
                }
            }
            var result = charac.Select(i => new { i.Value, i.UOM, i.Source }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        //  modifier
        
             [Authorize]
        public JsonResult mandatory(string noun, string modifier, string attribute)
        {
            var result = false;
            var list = _ValuestandardisationService.GetAttributes(noun, modifier);
            var YN = list.Single(x => x.Characteristic == attribute).Mandatory;
            if(YN == "Yes")
            {
                result = true;
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public int update_values(string noun, string modifier,string attribute, string value, string newvalue,string UOM,string newUOM)
        {
            int  updated_list = _ValuestandardisationService.update_values(noun,modifier,attribute,value,newvalue,UOM, newUOM);

            return updated_list;

        }
        [Authorize]
        public int Delete_values(string noun, string modifier, string attribute, string value)
        {
            int updated_list = _ValuestandardisationService.Delete_values(noun, modifier, attribute, value);

            return updated_list;

        }
    }
}