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
using System.Data;

namespace ProsolOnline.Controllers
{
    public class ServiceMasterController : Controller
    {
        private readonly IServiceMaster _ServiceMasterService;
        private readonly IServiceCreation _ServiceCreationService;
        private readonly IEmailSettings _Emailservc;
        private readonly IUserCreate _UserCreateService;
        private readonly IGeneralSettings _GeneralSettings;



        // private readonly IUserCreate _ServiceUser;
        public ServiceMasterController(IServiceMaster ServiceMasterService,
                                        IServiceCreation ServiceCreationService, IEmailSettings Emailservc,
                                        IUserCreate UserCreateService, IGeneralSettings GeneralSettings)

        {
            _ServiceMasterService = ServiceMasterService;
            _ServiceCreationService = ServiceCreationService;
            _GeneralSettings = GeneralSettings;
            _Emailservc = Emailservc;
            _UserCreateService = UserCreateService;

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
        // GET: ServiceMaster
        public ActionResult Create()
        {
            if (CheckAccess("Service Master") == 1)
                return View("servicemaster");
            else if (CheckAccess("Service Master") == 0)
                return View("Denied");
            else return View("Login");

            // return View("servicemaster");
        }
        // ServiceCreation
        public ActionResult ServiceCreation(string itemId)
        {
            if (CheckAccess("Service Creation") == 1)
                return View("servicecreation");
            else if (CheckAccess("Service Creation") == 0)
                return View("Denied");
            else return View("Login");
            // return View("servicecreation");
        }
        public ActionResult RequestService()
        {
            if (CheckAccess("Request Service") == 1)
                return View("requestservice");
            else if (CheckAccess("Request service") == 0)
                return View("Denied");
            else return View("Login");

            //return View("requestservice");
        }
        public ActionResult ApproveService()
        {
            if (CheckAccess("Approve Service") == 1)
                return View("ApproveService");
            else if (CheckAccess("ApproveService") == 0)
                return View("Denied");
            else return View("Login");

            //return View("requestservice");
        }
        public ActionResult ServiceMapping()
        {
            if (CheckAccess("Service Mapping") == 1)
                return View("servicemapping");
            else if (CheckAccess("Service Mapping") == 0)
                return View("Denied");
            else return View("Login");

            //return View("requestservice");
        }
        // public ActionResult 
        //servicecategory
        [HttpPost]
        public bool InsertDatasercat()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            ServiceCategory Model = JsonConvert.DeserializeObject<ServiceCategory>(obj);

            Prosol_ServiceCategory mdl = new Prosol_ServiceCategory();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            //mdl.SAPCategorycode = Model.SAPCategorycode;
            mdl.SeviceCategorycode = Model.SeviceCategorycode.ToUpper();
            mdl.SeviceCategoryname = Model.SeviceCategoryname.ToUpper();
            mdl.UpdatedOn = DateTime.Now;
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertDatasercat(mdl, update);
            return getresult;
        }
        public JsonResult DelRequest1(string ItemId)
        {
            var res = _ServiceMasterService.deleteRequest(ItemId);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getdatalistusing_id(string _id)
        {
            var result = _ServiceCreationService.getdatalistforupdate(_id);

            return this.Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult rejectitem(string _id, string rejectedas, string Remarks)
        {
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.UserId = Session["userid"].ToString();
            pu.Name = Session["username"].ToString();
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var result = _ServiceCreationService.rejectitem(_id, pu, rejectedas, Remarks);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadReviewer()
        {
            var result = _ServiceCreationService.LoadReviewer();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadReleaser()
        {
            var result = _ServiceCreationService.LoadReleaser();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult showall_Categoryuser()
        {
            var objList = _ServiceMasterService.showall_Categoryuser();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<ServiceCategory>();
            foreach (Prosol_ServiceCategory mdl in objList)
            {
                var obj = new ServiceCategory();
                obj._id = mdl._id.ToString();
                //obj.SAPCategorycode = mdl.SAPCategorycode;
                obj.SeviceCategorycode = mdl.SeviceCategorycode;
                obj.SeviceCategoryname = mdl.SeviceCategoryname;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Dservicecategory(string id, bool Islive)
        {

            var res = _ServiceMasterService.Dservicecategory(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelServicecode(string id)
        {

            var res = _ServiceMasterService.DelServicecode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //servicegroup
        public bool InsertData()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            ServiceGroup Model = JsonConvert.DeserializeObject<ServiceGroup>(obj);
            Prosol_ServiceGroup mdl = new Prosol_ServiceGroup();
            //mdl.SAPGroupcode = Model.SAPGroupcode;
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            mdl.ServiceGroupcode = Model.ServiceGroupcode.ToUpper();
            mdl.ServiceGroupname = Model.ServiceGroupname.ToUpper();
            mdl.SeviceCategorycode = Model.SeviceCategorycode;
            mdl.SeviceCategoryname = Model.SeviceCategoryname;
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertData(mdl, update);
            return getresult;

        }
        public JsonResult showall_groupuser()
        {
            var objList = _ServiceMasterService.showall_groupuser();

            //  var obj1 = _ServiceMasterService.showall_Categoryuser();
            var lst = new List<ServiceGroup>();
            foreach (Prosol_ServiceGroup mdl in objList)
            {
                var obj = new ServiceGroup();
                obj._id = mdl._id.ToString();
                // obj.ServiceActivitycode = mdl.ServiceGroupcode;

                obj.ServiceGroupcode = mdl.ServiceGroupcode;
                obj.ServiceGroupname = mdl.ServiceGroupname;

                obj.SeviceCategorycode = mdl.SeviceCategorycode;
                obj.SeviceCategoryname = mdl.SeviceCategoryname;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getgroupcodeforcatagory(string catagory)
        {
            var result = _ServiceCreationService.getgroupcodeforcatagory(catagory);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Dservicegroup(string id, bool Islive)
        {
            var res = _ServiceMasterService.Dservicegroup(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelGroupcode(string id)
        {

            var res = _ServiceMasterService.DelGroupcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        //activity
        //public bool InsertDataAct()
        //{
        //    var obj = Request.Form["obj"];
        //    Prosol_ServiceActivity Model = JsonConvert.DeserializeObject<Prosol_ServiceActivity>(obj);
        //    Prosol_ServiceActivity mdl = new Prosol_ServiceActivity();
        //    //mdl.SAPActivitycode = Model.SAPActivitycode;
        //    mdl.ServiceActivitycode = Model.ServiceActivitycode;
        //    mdl.ServiceActivityname = Model.ServiceActivityname;
        //    mdl.SeviceCategorycode = Model.SeviceCategorycode;
        //    mdl.SeviceCategoryname = Model.SeviceCategoryname;
        //    mdl.ServiceGroupcode = Model.ServiceGroupcode;
        //    mdl.ServiceGroupname = Model.ServiceGroupname;
        //    mdl.Islive = true;
        //    var getresult = _ServiceMasterService.InsertDataAct(mdl);
        //    return getresult;

        //}

        public JsonResult getgroup(string SeviceCategorycode)
        {

            var strloc = _ServiceMasterService.getgroup(SeviceCategorycode);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);


        }

        public JsonResult getCleanser()
        {
            var res = _ServiceMasterService.getCleanser();
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult showall_Activityuser()
        //{
        //    var objList = _ServiceMasterService.showall_Activityuser();

        //    //var obj1 = _ServiceMasterService.showall_user();
        //    var lst = new List<ServiceActivity>();
        //    foreach (Prosol_ServiceActivity mdl in objList)
        //    {
        //        var obj = new ServiceActivity();
        //        obj._id = mdl._id.ToString();
        //        obj.SeviceCategoryname = mdl.SeviceCategoryname;
        //        obj.ServiceGroupname = mdl.ServiceGroupname;
        //        //obj.SAPActivitycode = mdl.SAPActivitycode;
        //        obj.ServiceActivitycode = mdl.ServiceActivitycode;
        //        obj.ServiceActivityname = mdl.ServiceActivityname;

        //        obj.Islive = mdl.Islive;
        //        lst.Add(obj);
        //    }
        //    return this.Json(lst, JsonRequestBehavior.AllowGet);

        //}
        //public JsonResult Dserviceactivity(string id, bool Islive)
        //{
        //    var res = _ServiceMasterService.Dserviceactivity(id, Islive);
        //    return this.Json(res, JsonRequestBehavior.AllowGet);

        //}
        //serviceUom
        [HttpPost]
        public bool InsertDataUom()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            ServiceUom Model = JsonConvert.DeserializeObject<ServiceUom>(obj);
            Prosol_ServiceUom mdl = new Prosol_ServiceUom();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }

            mdl.ServiceUomcode = Model.ServiceUomcode.ToUpper();
            mdl.ServiceUomname = Model.ServiceUomname.ToUpper();
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertDataUom(mdl, update);
            return getresult;
        }

        public JsonResult showall_Uomuser()
        {
            var objList = _ServiceMasterService.showall_Uomuser();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<ServiceUom>();
            foreach (Prosol_ServiceUom mdl in objList)
            {
                var obj = new ServiceUom();
                obj._id = mdl._id.ToString();
                obj.ServiceUomcode = mdl.ServiceUomcode;
                obj.ServiceUomname = mdl.ServiceUomname;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DserviceUom(string id, bool Islive)
        {

            var res = _ServiceMasterService.DserviceUom(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelUOMcode(string id)
        {

            var res = _ServiceMasterService.DelUOMcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //valuation

        [HttpPost]
        public bool InsertDataValuation()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            ServiceValuation Model = JsonConvert.DeserializeObject<ServiceValuation>(obj);

            Prosol_ServiceValuation mdl = new Prosol_ServiceValuation();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }

            mdl.ServiceValuationcode = Model.ServiceValuationcode.ToUpper();
            mdl.ServiceValuationname = Model.ServiceValuationname.ToUpper();
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertDataValuation(mdl, update);
            return getresult;
        }
        public JsonResult showall_Valuationuser()
        {
            var objList = _ServiceMasterService.showall_Valuationuser();
            var lst = new List<ServiceValuation>();
            foreach (Prosol_ServiceValuation mdl in objList)
            {
                var obj = new ServiceValuation();
                obj._id = mdl._id.ToString();
                obj.ServiceValuationcode = mdl.ServiceValuationcode;
                obj.ServiceValuationname = mdl.ServiceValuationname;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Dservicevaluation(string id, bool Islive)
        {

            var res = _ServiceMasterService.Dservicevaluation(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelValuationcode(string id)
        {

            var res = _ServiceMasterService.DelValuationcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        //coading logics
        //maincode
        public bool InsertDataMainCode()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            SMainCode Model = JsonConvert.DeserializeObject<SMainCode>(obj);

            Prosol_SMainCode mdl = new Prosol_SMainCode();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            //mdl.SAPCategorycode = Model.SAPCategorycode;
            mdl.SeviceCategorycode = Model.SeviceCategorycode;
            mdl.SeviceCategoryname = Model.SeviceCategoryname;
            mdl.MainCode = Model.MainCode.ToUpper();
            mdl.MainDiscription = Model.MainDiscription.ToUpper();
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertDataMainCode(mdl, update);
            return getresult;
        }
        public JsonResult showall_MainCode1(string catagory)
        {
            var objList = _ServiceMasterService.showall_MainCode(catagory);

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<SMainCode>();
            foreach (Prosol_SMainCode mdl in objList)
            {
                var obj = new SMainCode();
                obj._id = mdl._id.ToString();
                //obj.SAPCategorycode = mdl.SAPCategorycode;
                obj.SeviceCategoryname = mdl.SeviceCategoryname;
                obj.SeviceCategorycode = mdl.SeviceCategorycode;
                obj.MainCode = mdl.MainCode;
                obj.MainDiscription = mdl.MainDiscription;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult showall_MainCode()
        {
            var objList = _ServiceMasterService.showall_MainCode();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<SMainCode>();
            foreach (Prosol_SMainCode mdl in objList)
            {
                var obj = new SMainCode();
                obj._id = mdl._id.ToString();
                //obj.SAPCategorycode = mdl.SAPCategorycode;
                obj.SeviceCategoryname = mdl.SeviceCategoryname;
                obj.MainCode = mdl.MainCode;
                obj.MainDiscription = mdl.MainDiscription;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DMainCode(string id, bool Islive)
        {

            var res = _ServiceMasterService.DMainCode(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DelMaincode(string id)
        {

            var res = _ServiceMasterService.DelMaincode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //subcode
        public bool InsertDataSubCode()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            SSubCode Model = JsonConvert.DeserializeObject<SSubCode>(obj);
            Prosol_SSubCode mdl = new Prosol_SSubCode();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            //mdl.SAPGroupcode = Model.SAPGroupcode;
            mdl.SubCode = Model.SubCode.ToUpper();
            mdl.SubDiscription = Model.SubDiscription.ToUpper();
            mdl.MainCode = Model.MainCode;
            mdl.MainDiscription = Model.MainDiscription;
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertDataSubCode(mdl, update);
            return getresult;

        }
        public JsonResult showall_SubCodeUser()
        {
            var objList = _ServiceMasterService.showall_SubCodeUser();

            var obj1 = _ServiceMasterService.showall_MainCode();
            var lst = new List<SSubCode>();
            foreach (Prosol_SSubCode mdl in objList)
            {
                var obj = new SSubCode();
                obj._id = mdl._id.ToString();
                // obj.ServiceActivitycode = mdl.ServiceGroupcode;

                obj.SubCode = mdl.SubCode;
                obj.SubDiscription = mdl.SubDiscription;

                obj.MainCode = mdl.MainCode;
                obj.MainDiscription = mdl.MainDiscription;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DSubCode(string id, bool Islive)
        {

            var res = _ServiceMasterService.DSubCode(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DelSubcode(string id)
        {

            var res = _ServiceMasterService.DelSubcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //subsub
        public bool InsertDataSubSub()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            SSubSub Model = JsonConvert.DeserializeObject<SSubSub>(obj);
            Prosol_SSubSubCode mdl = new Prosol_SSubSubCode();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            mdl.SubSubCode = Model.SubSubCode;
            mdl.SubSubDiscription = Model.SubSubDiscription;
            mdl.MainCode = Model.MainCode;
            mdl.MainDiscription = Model.MainDiscription;
            mdl.SubCode = Model.SubCode.ToUpper();
            mdl.SubDiscription = Model.SubDiscription.ToUpper();
            mdl.Islive = true;
            var getresult = _ServiceMasterService.InsertDataSubSub(mdl, update);
            return getresult;

        }

        public bool Insertvalabbr()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            ServiceCharacteristicValue Model = JsonConvert.DeserializeObject<ServiceCharacteristicValue>(obj);
            Prosol_ServiceCharacteristicValue mdl = new Prosol_ServiceCharacteristicValue();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            mdl.CharacteristicValue = Model.CharacteristicValue.ToUpper();
            mdl.ValueAbbreviation = Model.ValueAbbreviation.ToUpper();
            mdl.Islive = true;
            var getresult = _ServiceMasterService.Insertvalabbr(mdl, update);
            return getresult;
        }


        public JsonResult ListValAbbr()
        {
            var objList = _ServiceMasterService.ListValAbbr();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<ServiceCharacteristicValue>();
            foreach (Prosol_ServiceCharacteristicValue mdl in objList)
            {
                var obj = new ServiceCharacteristicValue();
                obj._id = mdl._id.ToString();
                //obj.SAPCategorycode = mdl.SAPCategorycode;
                obj.CharacteristicValue = mdl.CharacteristicValue;
                obj.ValueAbbreviation = mdl.ValueAbbreviation;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Delvalabbrcode(string id)
        {

            var res = _ServiceMasterService.Delvalabbrcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DValAbbr(string id, bool Islive)
        {

            var res = _ServiceMasterService.DValAbbr(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }


        //public JsonResult getSubList(string MainCode)
        //{
        //    {
        //        var strloc = _ServiceMasterService.getSubList(MainCode);
        //        return this.Json(strloc, JsonRequestBehavior.AllowGet);
        //    }

        //}
        public JsonResult getSubList(string MainCode)
        {

            var strloc = _ServiceMasterService.getSubList(MainCode);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);


        }
        public JsonResult getSubList1(string MainCode)
        {

            var strloc = _ServiceMasterService.getSubList1(MainCode);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);


        }
        public JsonResult getsubcodeT(string MainCode)
        {

            var grpList = _ServiceMasterService.getSubList(MainCode);
            var lst = new List<SSubCode>();
            foreach (Prosol_SSubCode mdl in grpList)
            {
                var grp = new SSubCode();
                grp._id = mdl._id.ToString();
                grp.MainCode = mdl.MainCode;
                grp.MainDiscription = mdl.MainDiscription;

                grp.SubCode = mdl.SubCode;
                grp.SubDiscription = mdl.SubDiscription;///// + " ( " + mdl.groupCode + " )";
                grp.Islive = mdl.Islive;

                lst.Add(grp);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }


        public JsonResult showall_SubSubUser()
        {
            var objList = _ServiceMasterService.showall_SubSubUser();

            //var obj1 = _ServiceMasterService.showall_user();
            var lst = new List<SSubSub>();
            foreach (Prosol_SSubSubCode mdl in objList)
            {
                var obj = new SSubSub();
                obj._id = mdl._id.ToString();
                obj.MainCode = mdl.MainCode;

                obj.MainDiscription = mdl.MainDiscription;

                obj.SubCode = mdl.SubCode;
                obj.SubDiscription = mdl.SubDiscription;
                //obj.SAPActivitycode = mdl.SAPActivitycode;
                obj.SubSubCode = mdl.SubSubCode;
                obj.SubSubDiscription = mdl.SubSubDiscription;

                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DSubSub(string id, bool Islive)
        {
            var res = _ServiceMasterService.DSubSub(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DelSubSubcode(string id)
        {

            var res = _ServiceMasterService.DelSubSubcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

       public JsonResult addMS()        {
            //mainsub
            var Noun = Request.Form["Noun"];            var Modifier = Request.Form["Modifier"];            var Noun1 = JsonConvert.DeserializeObject<String>(Noun);            var Modifier1 = JsonConvert.DeserializeObject<String>(Modifier);
            //   var Noun1 = JsonConvert.DeserializeObject(Noun);
            //  var Modifier1 = JsonConvert.DeserializeObject(Modifier);

            //var model2 = JsonConvert.DeserializeObject<List<MS_Attribute>>(actvity);
            //var Model = JsonConvert.DeserializeObject<MS_Attribute>(obj);
            //Prosol_SSubCode mdl = new Prosol_SSubCode();


            // var getresult = _ServiceMasterService.addMS(mdl);
            //attributes
            var cha = Request.Form["CHA"];            var model1 = JsonConvert.DeserializeObject<List<MS_Attribute>>(cha);            var listchar = new List<Prosol_MSAttribute>();            foreach (MS_Attribute pat in model1)            {                var mdl1 = new Prosol_MSAttribute();                mdl1._id = pat._id != null ? new MongoDB.Bson.ObjectId(pat._id) : new MongoDB.Bson.ObjectId();
                // mdl1.MainCode = Model.MainCode;
                // mdl1.MainDiscription = Model.MainDiscription;
                //  mdl1.SubCode = Model.SubCode;
                // mdl1.SubDiscription = Model.SubDiscription;
                //  mdl1.Service = pat.Service;
                mdl1.Noun = Noun1;                mdl1.Modifier = Modifier1;                mdl1.Attributes = pat.Attributes.ToUpper();                mdl1.Values = pat.Values;                mdl1.Sequence = pat.Sequence;                mdl1.UpdatedOn = DateTime.Now;                listchar.Add(mdl1);            }            var resultss = _ServiceMasterService.attribute(listchar);            return this.Json(true, JsonRequestBehavior.AllowGet);        }
        //public JsonResult getAttribute(string Activity)
        //{
        //    {
        //        var strloc = _ServiceMasterService.getAttribute(Activity).ToList();
        //        // if(strloc != null)
        //        if (strloc.Count > 0)
        //        {
        //            var ListViewModelObj = new List<MS_Attribute>();
        //            foreach (Prosol_MSAttribute mdl in strloc)
        //            {
        //                var ViewModelObj = new MS_Attribute();
        //                ViewModelObj._id = mdl._id.ToString();
        //                ViewModelObj.Attributes = mdl.Attributes;
        //                ViewModelObj.Sequence = mdl.Sequence;
        //                ViewModelObj.Values = mdl.Values;
        //                ViewModelObj.MainCode = mdl.MainCode;
        //                ViewModelObj.SubCode = mdl.SubCode;
        //                ViewModelObj.MainDiscription = mdl.MainDiscription;
        //                ViewModelObj.SubDiscription = mdl.SubDiscription;
        //                ViewModelObj.Activity = mdl.Activity;
        //                ListViewModelObj.Add(ViewModelObj);
        //            }

        //            var strloc1 = _ServiceMasterService.getAttribute().ToList();
        //            if (strloc1 != null && strloc1.Count > 0)
        //            {
        //                foreach (Prosol_ServiceDefaultAttr dmdl in strloc1)
        //                {
        //                    var tmpLst = ListViewModelObj.Where(x => x.Attributes == dmdl.Attributes).ToList();
        //                    if (tmpLst == null || tmpLst.Count == 0)
        //                    {
        //                        var VieObj = new MS_Attribute();
        //                        VieObj.Attributes = dmdl.Attributes;
        //                        ListViewModelObj.Add(VieObj);
        //                    }


        //                }
        //            }

        //            return this.Json(ListViewModelObj, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            var strloc1 = _ServiceMasterService.getAttribute().ToList();
        //            var ListViewModelObj = new List<ServiceDefaultAttr>();
        //            foreach (Prosol_ServiceDefaultAttr mdl in strloc1)
        //            {
        //                var ViewModelObj = new ServiceDefaultAttr();
        //                // ViewModelObj._id = mdl._id.ToString();
        //                ViewModelObj.Attributes = mdl.Attributes;

        //                ListViewModelObj.Add(ViewModelObj);
        //            }
        //            return this.Json(ListViewModelObj, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //}
        //public JsonResult getAttribute1()
        //{
        //    {
        //        var strloc = _ServiceMasterService.getAttribute().ToList();
        //        var ListViewModelObj = new List<ServiceDefaultAttr>();
        //        foreach (Prosol_ServiceDefaultAttr mdl in strloc)
        //        {
        //            var ViewModelObj = new ServiceDefaultAttr();
        //            ViewModelObj._id = mdl._id.ToString();
        //            ViewModelObj.Attributes = mdl.Attributes;

        //            ListViewModelObj.Add(ViewModelObj);
        //        }
        //        return this.Json(ListViewModelObj, JsonRequestBehavior.AllowGet);
        //    }

        //}
        //attributeeeee
        public JsonResult GetValueListforcreate_temp(int currentPage, int maxRows, string Name, string Activity)
        {

            var ValueList1 = _ServiceMasterService.GetAbbrList().ToList();


            var AttributeObj = _ServiceMasterService.GetAttributeDetail(Name);

            if (AttributeObj != null)
            {

                if (AttributeObj.ValueList.Count() > 0)
                {
                    List<Prosol_Abbrevate> first = new List<Prosol_Abbrevate>();
                    foreach (string str in AttributeObj.ValueList)
                    {
                        foreach (Prosol_Abbrevate pa in ValueList1.ToList())
                        {
                            if (str == pa._id.ToString())
                            {
                                first.Add(pa);
                                ValueList1.Remove(pa);
                                goto label1;
                            }
                        }
                        label1: { }
                    }
                    var ValueList = new List<Prosol_Abbrevate>();

                    ValueList = first.ToList();

                    var lst = new List<AbbrevateModel>();
                    PaingGroup pageList = new PaingGroup();
                    pageList.totItem = ValueList.ToList().Count;
                    var lstTmp = (from prsl in ValueList
                                  select prsl)
                                .Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();


                    foreach (Prosol_Abbrevate mdl in lstTmp)
                    {
                        var midMdl = new AbbrevateModel();
                        midMdl._id = mdl._id.ToString();
                        midMdl.Value = mdl.Value;
                        midMdl.vunit = "  " + mdl.vunit;
                        lst.Add(midMdl);

                    }

                    pageList.Abbrevatedvalues = lst;
                    double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                    pageList.PageCount = (int)Math.Ceiling(pageCount);
                    pageList.CurrentPageIndex = currentPage;
                    //  resulted.Abbrevatedvalues = pageList;
                    return this.Json(pageList, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    var characteristicvalueObj = _ServiceMasterService.GetCharacteristicvalues(Name, Activity);
                    ///////

                    if (characteristicvalueObj != null)
                    {

                        if (characteristicvalueObj.Values != null)
                        {
                            List<Prosol_Abbrevate> first = new List<Prosol_Abbrevate>();
                            //  foreach (string str in characteristicvalueObj.Values)
                            // {
                            // foreach (Prosol_Abbrevate pa in ValueList1.ToList())
                            // {
                            //  int indx = ValueList1.FindIndex(f => (f._id.ToString()).Equals(str));
                            //   var tmp = new Prosol_Abbrevate();
                            //   tmp._id = new ObjectId(str);
                            // int indx =;
                            //  var match = ValueList1.Any(x => x._id.ToString().Equals(str));
                            // if (match)
                            //if (str == pa._id.ToString())
                            //  {
                            // first.Add(ValueList1[indx]);
                            // ValueList1.Remove(ValueList1[indx]);
                            // goto label1;
                            // }
                            // }
                            // label1: { }
                            // }
                            var ValueList = new List<Prosol_Abbrevate>();

                            if (first.Count > 0)
                            {
                                ValueList = first.Concat(ValueList1).ToList();
                            }
                            else
                            {
                                ValueList = ValueList1;

                            }

                            // ValueList = first.ToList();

                            var lst = new List<AbbrevateModel>();
                            PaingGroup pageList = new PaingGroup();
                            pageList.totItem = ValueList.ToList().Count;
                            var lstTmp = (from prsl in ValueList
                                          select prsl)
                                        .Skip((currentPage - 1) * maxRows)
                                        .Take(maxRows).ToList();


                            foreach (Prosol_Abbrevate mdl in lstTmp)
                            {
                                var midMdl = new AbbrevateModel();
                                midMdl._id = mdl._id.ToString();
                                midMdl.Value = mdl.Value;
                                midMdl.vunit = "  " + mdl.vunit;
                                lst.Add(midMdl);

                            }

                            pageList.Characteristicvalues = lst;
                            double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                            pageList.PageCount = (int)Math.Ceiling(pageCount);
                            pageList.CurrentPageIndex = currentPage;
                            return this.Json(pageList, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var ValueList = ValueList1;


                            var lst = new List<AbbrevateModel>();
                            PaingGroup pageList = new PaingGroup();
                            pageList.totItem = ValueList.ToList().Count;
                            var lstTmp = (from prsl in ValueList
                                          select prsl)
                                        .Skip((currentPage - 1) * maxRows)
                                        .Take(maxRows).ToList();


                            foreach (Prosol_Abbrevate mdl in lstTmp)
                            {
                                var midMdl = new AbbrevateModel();
                                midMdl._id = mdl._id.ToString();
                                midMdl.Value = mdl.Value;
                                midMdl.vunit = "  " + mdl.vunit;
                                lst.Add(midMdl);

                            }

                            pageList.Characteristicvalues = lst;
                            double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                            pageList.PageCount = (int)Math.Ceiling(pageCount);
                            pageList.CurrentPageIndex = currentPage;
                            return this.Json(pageList, JsonRequestBehavior.AllowGet);
                        }
                        ///////////
                    }
                    else
                    {
                        var ValueList = ValueList1;


                        var lst = new List<AbbrevateModel>();
                        PaingGroup pageList = new PaingGroup();
                        pageList.totItem = ValueList.ToList().Count;
                        var lstTmp = (from prsl in ValueList
                                      select prsl)
                                    .Skip((currentPage - 1) * maxRows)
                                    .Take(maxRows).ToList();


                        foreach (Prosol_Abbrevate mdl in lstTmp)
                        {
                            var midMdl = new AbbrevateModel();
                            midMdl._id = mdl._id.ToString();
                            midMdl.Value = mdl.Value;
                            midMdl.vunit = "  " + mdl.vunit;
                            lst.Add(midMdl);

                        }

                        pageList.Characteristicvalues = lst;
                        double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                        pageList.PageCount = (int)Math.Ceiling(pageCount);
                        pageList.CurrentPageIndex = currentPage;
                        return this.Json(pageList, JsonRequestBehavior.AllowGet);
                    }


                }
            }
            else
            {
                var ValueList = ValueList1;


                var lst = new List<AbbrevateModel>();
                PaingGroup pageList = new PaingGroup();
                pageList.totItem = ValueList.ToList().Count;
                var lstTmp = (from prsl in ValueList
                              select prsl)
                            .Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();


                foreach (Prosol_Abbrevate mdl in lstTmp)
                {
                    var midMdl = new AbbrevateModel();
                    midMdl._id = mdl._id.ToString();
                    midMdl.Value = mdl.Value;
                    midMdl.vunit = "  " + mdl.vunit;
                    lst.Add(midMdl);

                }

                pageList.Characteristicvalues = lst;
                double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                pageList.PageCount = (int)Math.Ceiling(pageCount);
                pageList.CurrentPageIndex = currentPage;
                return this.Json(pageList, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult GetAttributesDetail(string Name)
        {
            var AttributeObj = _ServiceMasterService.GetAttributeDetail(Name);
            var LocObj = new AttributeModel();
            if (AttributeObj != null)
            {
                LocObj._id = AttributeObj._id.ToString();
                LocObj.Attribute = AttributeObj.Attribute;
                LocObj.Validation = AttributeObj.Validation;
                LocObj.ValueList = AttributeObj.ValueList;
                LocObj.UOMList = AttributeObj.UOMList;
            }
            return Json(LocObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAttributesDetailfromcharacteristicvalues(string Name, string Activity)
        {
            var AttributeObj = _ServiceMasterService.GetCharacteristicvalues(Name, Activity);
            var LocObj = new AttributeModel();
            if (AttributeObj != null)
            {
                LocObj._id = AttributeObj._id.ToString();
                // LocObj.Attribute = AttributeObj.Attribute;
                // LocObj.Validation = AttributeObj.Validation;
                LocObj.ValueList = AttributeObj.Values;
                // LocObj.UOMList = AttributeObj.UOMList;
            }
            return Json(LocObj, JsonRequestBehavior.AllowGet);
        }

        //  return this.Json(true, JsonRequestBehavior.AllowGet);

        // }
        public JsonResult GetValueListforcreate_tempsearch(int currentPage, int maxRows, string Name, string Activity, string srchtxt)
        {

            var ValueList1 = _ServiceMasterService.GetAbbrList(srchtxt).ToList();


            var AttributeObj = _ServiceMasterService.GetAttributeDetail(Name);

            if (AttributeObj != null)
            {

                if (AttributeObj.ValueList.Count() > 0)
                {
                    List<Prosol_Abbrevate> first = new List<Prosol_Abbrevate>();
                    foreach (string str in AttributeObj.ValueList)
                    {
                        foreach (Prosol_Abbrevate pa in ValueList1.ToList())
                        {
                            if (str == pa._id.ToString())
                            {
                                first.Add(pa);
                                ValueList1.Remove(pa);
                                goto label1;
                            }
                        }
                        label1: { }
                    }
                    var ValueList = new List<Prosol_Abbrevate>();

                    ValueList = first.ToList();

                    var lst = new List<AbbrevateModel>();
                    PaingGroup pageList = new PaingGroup();
                    pageList.totItem = ValueList.ToList().Count;
                    var lstTmp = (from prsl in ValueList
                                  select prsl)
                                .Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();


                    foreach (Prosol_Abbrevate mdl in lstTmp)
                    {
                        var midMdl = new AbbrevateModel();
                        midMdl._id = mdl._id.ToString();
                        midMdl.Value = mdl.Value;
                        midMdl.vunit = "  " + mdl.vunit;
                        lst.Add(midMdl);

                    }

                    pageList.Abbrevatedvalues = lst;
                    double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                    pageList.PageCount = (int)Math.Ceiling(pageCount);
                    pageList.CurrentPageIndex = currentPage;
                    //  resulted.Abbrevatedvalues = pageList;
                    return this.Json(pageList, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    var characteristicvalueObj = _ServiceMasterService.GetCharacteristicvalues(Name, Activity);
                    ///////

                    if (characteristicvalueObj != null)
                    {

                        if (characteristicvalueObj.Values != null)
                        {
                            List<Prosol_Abbrevate> first = new List<Prosol_Abbrevate>();
                            foreach (string str in characteristicvalueObj.Values)
                            {
                                foreach (Prosol_Abbrevate pa in ValueList1.ToList())
                                {
                                    if (str == pa._id.ToString())
                                    {
                                        first.Add(pa);
                                        ValueList1.Remove(pa);
                                        goto label1;
                                    }
                                }
                                label1: { }
                            }
                            var ValueList = new List<Prosol_Abbrevate>();

                            if (first.Count > 0)
                            {
                                ValueList = first.Concat(ValueList1).ToList();
                            }
                            else
                            {
                                ValueList = ValueList1;
                            }

                            // ValueList = first.ToList();

                            var lst = new List<AbbrevateModel>();
                            PaingGroup pageList = new PaingGroup();
                            pageList.totItem = ValueList.ToList().Count;
                            var lstTmp = (from prsl in ValueList
                                          select prsl)
                                        .Skip((currentPage - 1) * maxRows)
                                        .Take(maxRows).ToList();


                            foreach (Prosol_Abbrevate mdl in lstTmp)
                            {
                                var midMdl = new AbbrevateModel();
                                midMdl._id = mdl._id.ToString();
                                midMdl.Value = mdl.Value;
                                midMdl.vunit = "  " + mdl.vunit;
                                lst.Add(midMdl);

                            }

                            pageList.Characteristicvalues = lst;
                            double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                            pageList.PageCount = (int)Math.Ceiling(pageCount);
                            pageList.CurrentPageIndex = currentPage;
                            return this.Json(pageList, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var ValueList = ValueList1;


                            var lst = new List<AbbrevateModel>();
                            PaingGroup pageList = new PaingGroup();
                            pageList.totItem = ValueList.ToList().Count;
                            var lstTmp = (from prsl in ValueList
                                          select prsl)
                                        .Skip((currentPage - 1) * maxRows)
                                        .Take(maxRows).ToList();


                            foreach (Prosol_Abbrevate mdl in lstTmp)
                            {
                                var midMdl = new AbbrevateModel();
                                midMdl._id = mdl._id.ToString();
                                midMdl.Value = mdl.Value;
                                midMdl.vunit = "  " + mdl.vunit;
                                lst.Add(midMdl);

                            }

                            pageList.Characteristicvalues = lst;
                            double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                            pageList.PageCount = (int)Math.Ceiling(pageCount);
                            pageList.CurrentPageIndex = currentPage;
                            return this.Json(pageList, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var ValueList = ValueList1;


                        var lst = new List<AbbrevateModel>();
                        PaingGroup pageList = new PaingGroup();
                        pageList.totItem = ValueList.ToList().Count;
                        var lstTmp = (from prsl in ValueList
                                      select prsl)
                                    .Skip((currentPage - 1) * maxRows)
                                    .Take(maxRows).ToList();


                        foreach (Prosol_Abbrevate mdl in lstTmp)
                        {
                            var midMdl = new AbbrevateModel();
                            midMdl._id = mdl._id.ToString();
                            midMdl.Value = mdl.Value;
                            midMdl.vunit = "  " + mdl.vunit;
                            lst.Add(midMdl);

                        }

                        pageList.Characteristicvalues = lst;
                        double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                        pageList.PageCount = (int)Math.Ceiling(pageCount);
                        pageList.CurrentPageIndex = currentPage;
                        return this.Json(pageList, JsonRequestBehavior.AllowGet);
                    }


                }
            }
            else
            {
                var ValueList = ValueList1;


                var lst = new List<AbbrevateModel>();
                PaingGroup pageList = new PaingGroup();
                pageList.totItem = ValueList.ToList().Count;
                var lstTmp = (from prsl in ValueList
                              select prsl)
                            .Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();


                foreach (Prosol_Abbrevate mdl in lstTmp)
                {
                    var midMdl = new AbbrevateModel();
                    midMdl._id = mdl._id.ToString();
                    midMdl.Value = mdl.Value;
                    midMdl.vunit = "  " + mdl.vunit;
                    lst.Add(midMdl);

                }

                pageList.Characteristicvalues = lst;
                double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                pageList.PageCount = (int)Math.Ceiling(pageCount);
                pageList.CurrentPageIndex = currentPage;
                return this.Json(pageList, JsonRequestBehavior.AllowGet);

            }
        }

        //getAttributeUniqueList
        public JsonResult getAttributeUniqueList()
        {
            var AttributeResultList = _ServiceMasterService.getAttributeUniqueList();
            var ResultUniqueList = AttributeResultList.Select(i => new { i.Attributes }).Distinct();
            return this.Json(ResultUniqueList, JsonRequestBehavior.AllowGet);

        }
        //attributelist
        public JsonResult GetAttributesList()
        {
            var res = _ServiceMasterService.GetAttributes().ToList();
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        //requestservice
        public JsonResult get_approvercodename()
        {
            var approver = _ServiceMasterService.get_approvercodename();
            return this.Json(approver, JsonRequestBehavior.AllowGet);
        }

        // newrequestservice
        public JsonResult newRequestService()
        {
            //var mul_req_values = Request.Form["Single_request"];
            //string date1 = DateTime.Now.ToString("MM/dd/yyyy");
            //date1 = date1.Replace(@"-", "");
            //date1 = date1.Replace(@"/", "");
            //date1 = date1.Replace(@" ", "");
            //int reqid = _ServiceMasterService.getlast_request_R_no1(date1);
            //var m_mul_req_values = JsonConvert.DeserializeObject<List<Prosol_RequestService>>(mul_req_values);
            //var app_list = m_mul_req_values.Select(m => new { m.approver }).Distinct().ToList();

            //foreach (Prosol_RequestService pr2 in m_mul_req_values)
            //{
            //    pr2.requester = Session["userid"].ToString();
            //}

            //foreach (var item in app_list)
            //{
            //    string str1 = "<html><body> REQUEST ID : " + date1 + " <table style='border: 1px solid black;border-collapse:collapse'><tr><th style='border:1px solid black'>S.No</th><th style='border:1px solid black'>ServiceCategory</th><th style='border:1px solid black'>ServiceGroup</th><th style='border:1px solid black'>UOM</th><th style='border:1px solid black'>ServiceDescription</th></tr>";
            //    string str2 = "";

            //    string app = item.approver;
            //    int i = 0;
            //    foreach (Prosol_RequestService pr1 in m_mul_req_values)
            //    {
            //        if (app == pr1.approver)
            //        {
            //            str2 = "<tr><td style='border:1px solid black'>" + ++i + "</td><td style='border:1px solid black'>" + pr1.ServiceCategory + "</td><td style='border:1px solid black'>" + pr1.ServiceGroup + "</td><td style='border:1px solid black'>" + pr1.unitofmeasurement + "</td><td style='border:1px solid black'>" + pr1.ServiceDiscription + "</td></tr>";
            //            str1 = str1 + str2;
            //        }
            //    }

            //    string str3 = "</table></body ></html > ";
            //    str1 = str1 + str3;
            //    try
            //    {
            //        var user_deteails = _ServiceMasterService.get_approvercode_name_using_approverid1(app).ToList();

            //        string to_mail = user_deteails[0].EmailId;
            //        // string userid = Data[0].Userid;
            //        email email1 = new email();
            //        email1.email_to = to_mail;
            //        email1.email_from = "codasol.madras@gmail.com";
            //        if (Session["username"] != null)
            //            email1.subject = "New Request from " + Session["username"].ToString();
            //        email1.body = str1;
            //        email1.IsBodyHtml = true;
            //        email1.host = "smtp.gmail.com";
            //        email1.enablessl = true;
            //        email1.UseDefaultCredentials = true;
            //        email1.Port = 587;
            //        email1.password = "codasolwestmambalam";

            //        emailservice es = new emailservice();

            //        bool val = es.sendmail(email1);
            //    }
            //    catch
            //    {
            //        string mul_resilt1 = "Error Sending Mail";
            //        return this.Json(mul_resilt1, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //bool mul_resilt = _ServiceMasterService.insert_multiplerequest1(m_mul_req_values, date1);
            //if (mul_resilt == true)
            //{
            //    return this.Json(true, JsonRequestBehavior.AllowGet);

            //}
            //else
            return this.Json(false, JsonRequestBehavior.AllowGet);

        }

        //bulk matirialrequest
        public JsonResult bulk_material_request_SM()
        {
            var mul_req_values = Request.Form["Mul_request"];
            // string date1 = DateTime.Now.ToString("MM/dd/yyyy");
            string date1 = DateTime.Now.ToString("yyyy/MM/dd");
            date1 = date1.Replace(@"-", "");
            date1 = date1.Replace(@"/", "");
            date1 = date1.Replace(@" ", "");
            int reqid = _ServiceMasterService.getlast_request_R_no1(date1);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<Prosol_RequestService>>(mul_req_values);
            var ms = JsonConvert.DeserializeObject<List<ItemRequestService>>(mul_req_values);
            var app_list = m_mul_req_values.Select(m => new { m.Cleanser }).Distinct().ToList();
            date1 = date1 + reqid.ToString();
            foreach (Prosol_RequestService pr2 in m_mul_req_values)
            {
                var Requester = new Prosol_UpdatedBy();
                Requester.UserId = Session["userid"].ToString();
                Requester.Name = Session["username"].ToString();
                Requester.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pr2.requester = Requester;
                pr2.Last_updatedBy = Requester;
                //var Cleanser = new Prosol_UpdatedBy();
                //Cleanser.UserId = pr2.Cleanser;
                //Cleanser.Name = _ServiceMasterService.getusernameforrequest(pr2.Cleanser);
                //Cleanser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //pr2.Cleanserr = Cleanser;
            }
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Requester") tmpStr = ent.TargetId;
            }
            foreach (Prosol_RequestService pr2 in m_mul_req_values)
            {
                pr2.requester.UserId = Session["userid"].ToString();
                pr2.approver = tmpStr;
            }

            var tbl = new DataTable();
            tbl.Columns.Add("PlantName");
            tbl.Columns.Add("Category");
            tbl.Columns.Add("Group");
            tbl.Columns.Add("UOM");
            tbl.Columns.Add("Legacy");
         
          
            foreach (Prosol_RequestService pr1 in m_mul_req_values)
            {
                var row = tbl.NewRow();
                row["PlantName"] = pr1.PlantName;
                row["Category"] = pr1.ServiceCategoryName;
                row["Group"] = pr1.ServiceGroupName;
                row["UOM"] = pr1.UomName;
                row["Legacy"] = pr1.Legacy;
               
                tbl.Rows.Add(row);



            }
            //foreach (var item in app_list)
            //{
            //string str1 = "<html><body> REQUEST ID : " + date1 + " <table style='border: 1px solid black;border-collapse:collapse'><tr><th style='border:1px solid black'>S.No</th><th style='border:1px solid black'>ServiceCategory</th><th style='border:1px solid black'>ServiceGroup</th><th style='border:1px solid black'>UOM</th><th style='border:1px solid black'>ServiceDescription</th></tr>";
            //string str2 = "";

            string app = m_mul_req_values[0].approver;
            int i = 0;
            //foreach (Prosol_RequestService pr1 in m_mul_req_values)
            //{
            //    //if (app == pr1.Cleanser)
            //    //{
            //    str2 = "<tr><td style='border:1px solid black'>" + ++i + "</td><td style='border:1px solid black'>" + pr1.ServiceCategoryName + "</td><td style='border:1px solid black'>" + pr1.ServiceGroupName + "</td><td style='border:1px solid black'>" + pr1.UomName + "</td><td style='border:1px solid black'>" + pr1.Legacy + "</td></tr>";
            //    str1 = str1 + str2;
            //    //}
            //}

            //string str3 = "</table></body ></html > ";
            //str1 = str1 + str3;
            try
            {
                var user_deteails = _ServiceMasterService.get_approvercode_name_using_approverid1(app).ToList();

                string to_mail = user_deteails[0].EmailId;
                string subject = "";
                // string userid = Data[0].Userid;
                // email email1 = new email();
                // email1.email_to = to_mail;
                // email1.email_from = "codasol.madras@gmail.com";
                if (Session["username"] != null)
                    subject = "New Request from " + Session["username"].ToString() + " REQUEST ID : " + date1;
                //string body = str1;
                //email1.IsBodyHtml = true;
                // email1.host = "smtp.gmail.com";
                // email1.enablessl = true;
                //email1.UseDefaultCredentials = true;
                // email1.Port = 587;
                // email1.password = "codasolwestmambalam";

                // EmailSettingService ems = new EmailSettingService();

                bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));
            }
            catch
            {
                string mul_resilt1 = "Error Sending Mail";
                return this.Json(mul_resilt1, JsonRequestBehavior.AllowGet);
            }
            //}
            bool mul_resilt = _ServiceMasterService.insert_multiplerequest1(m_mul_req_values, date1);
            if (mul_resilt == true)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);

            }
            else
                return this.Json(false, JsonRequestBehavior.AllowGet);

        }

        //servicecreation
        //public JsonResult getActivitySC(string ServiceGroupcode)
        //{
        //    {
        //        var strloc = _ServiceCreationService.getActivitySC(ServiceGroupcode);
        //        return this.Json(strloc, JsonRequestBehavior.AllowGet);
        //    }

        //}



        //servicecreation
        //tablr for main nd subcode

        public JsonResult GetMainSubAttributeTable(string SubCode)
        {
            {
                var strloc = _ServiceCreationService.GetMainSubAttributeTable(SubCode).ToList();
                if (strloc.Count > 0)
                {
                    var ListViewModelObj = new List<MS_Attribute>();
                    foreach (Prosol_MSAttribute mdl in strloc)
                    {
                        var ViewModelObj = new MS_Attribute();
                        ViewModelObj._id = mdl._id.ToString();
                        ViewModelObj.Attributes = mdl.Attributes;
                        // ViewModelObj.Values = mdl.Values;
                        ViewModelObj.Sequence = mdl.Sequence;
                        ViewModelObj.MainCode = mdl.MainCode;
                        ViewModelObj.SubCode = mdl.SubCode;
                        ListViewModelObj.Add(ViewModelObj);
                    }
                    return this.Json(ListViewModelObj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json(false, JsonRequestBehavior.AllowGet);
                }
            }

        }
        //subsublist
        public JsonResult GetSubSubList(string MainCode, string SubCode)
        {
            var strloc = _ServiceCreationService.GetSubSubList(MainCode, SubCode);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);

        }

        // defaultattribute

        public JsonResult Defaultattribute()
        {
            var strloc = _ServiceCreationService.Defaultattribute();
            return this.Json(strloc, JsonRequestBehavior.AllowGet);

        }
        public JsonResult checkDuplicate()
        {
            var obj = Request.Form["obj"];
            var attributes = Request.Form["Attributess"];
            //var obj1 = Request.Form["obj1"];
            Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
            
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            List<string> mylist = createshortlong1(m_mul_req_values, prs);
            var res = _ServiceCreationService.checkDuplicate(mylist[0], prs.ItemId);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult createshortlongsl()
        {

            var obj = Request.Form["obj"];
            var attributes = Request.Form["Attributess"];
            //var rev = Request.Form["obj1"];
            Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
           // Service_Request rev1 = JsonConvert.DeserializeObject<Service_Request>(rev);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            List<string> mylist = createshortlong1(m_mul_req_values,prs);

            //if (prs.ShortDesc != null && prs.LongDesc != null)
            //{
            List<Prosol_ServiceAttributeList> sal = JsonConvert.DeserializeObject<List<Prosol_ServiceAttributeList>>(attributes);
            prs.Characteristics = sal;
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            pub.Name = Session["username"].ToString();
            pub.UserId = Session["userid"].ToString();
            pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            //Prosol_UpdatedBy revie = JsonConvert.DeserializeObject<Prosol_UpdatedBy>(rev);
            //revie.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            //  var data = _ServiceCreationService.getdatalistforupdate(prs._id);

            //  Prosol_RequestService prrs = data;
            var prrs = new Prosol_RequestService();
            //  prrs._id = prs._id != null && prs._id != "" ? new MongoDB.Bson.ObjectId(prs._id) : new MongoDB.Bson.ObjectId();
            prrs.ServiceCategoryName = prs.ServiceCategoryName;
            prrs.ServiceCategoryCode = prs.ServiceCategoryCode;
            prrs.ServiceGroupName = prs.ServiceGroupName;
            prrs.ServiceGroupCode = prs.ServiceGroupCode;
            prrs.Noun = prs.Noun;
            prrs.Modifier = prs.Modifier;
            prrs.UomName = prs.UomName;
            prrs.UomCode = prs.UomCode;
            prrs.Noun = prs.Noun;
            prrs.Modifier = prs.Modifier;
            prrs.PlantCode = prs.PlantCode;
            prrs.PlantName = prs.PlantName;
            if (prs.ValuationName != "Select Valuation")
                prrs.ValuationName = prs.ValuationName;
            else prrs.ValuationName = "";
            prrs.Valuationcode = prs.Valuationcode;

            prrs.MainCode = prs.MainCode;
            if (prs.MainCodeName != "Select Main Code")
                prrs.MainCodeName = prs.MainCodeName;
            else prrs.MainCodeName = "";
            //prrs.MainCodeName = prs.MainCodeName;

            prrs.SubCode = prs.SubCode;
            if (prs.SubCodeName != "Select Sub Code")
                prrs.SubCodeName = prs.SubCodeName;
            else prrs.SubCodeName = "";
            // prrs.SubCodeName = prs.SubCodeName;

            prrs.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                prrs.SubSubCodeName = prs.SubSubCodeName;
            else prrs.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;

            prrs.Remarks = prs.Remarks;
            prrs.Characteristics = prs.Characteristics;
            prrs.Class = prs.Class;
            prrs.ClassTitle = prs.ClassTitle;
            prrs.Commodity = prs.Unspsc;
            prrs.CommodityTitle = prs.CommodityTitle;
            prrs.parent = prs.parent;
            // prrs.ShortDesc = prs.ShortDesc;
            prrs.ShortDesc = mylist[0].ToString();
            // prrs.LongDesc = prs.LongDesc;
            prrs.LongDesc = mylist[1].ToString();
            prrs.UpdatedOn = DateTime.Now;
            prrs.Cleanser = pub.UserId;
            prrs.Last_updatedBy = pub;
            prrs.ServiceStatus = "Cleansed";
            prrs.RequestStatus = "Cleanse";
            //  prrs.Reviewer = revie;
            prrs.Approvedon = prs.Approvedon;
            prrs.approver = prs.approver;
            prrs.ItemId = prs.ItemId;
            prrs.RequestId = prs.RequestId;
            prrs.ServiceCode = prs.ServiceCode;
            prrs.Legacy = prs.Legacy;
            //prrs.Approvedon=prs.
            if (prrs.ItemId == null || prrs.ItemId == null)
            {
                //var itemcode = _ServiceMasterService.getitemcode();
                string date1 = DateTime.Now.ToString("yyyy/MM/dd");
                date1 = date1.Replace(@"-", "");
                date1 = date1.Replace(@"/", "");
                date1 = date1.Replace(@" ", "");
                int reqid = _ServiceMasterService.getlast_request_R_no1(date1);
                date1 = date1 + reqid.ToString();
                prrs.ItemId = date1;
            }
            //string rn = "";
            //if (string.IsNullOrEmpty(prs.ServiceCode))
            //{
            //    var clcheck = _ServiceCreationService.getclchk();
            //    if (clcheck == "Customized Code")
            //    {
            //        rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //        prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //    }
            //    else if (clcheck == "UNSPSC Code")
            //        if (string.IsNullOrEmpty(prs.ServiceCode) || (prs.ServiceCode != null && prs.SubCode != prs.ServiceCode.Substring(0, 8)))
            //            prrs.ServiceCode = generateServicecodeunspsc(prrs.Commodity, 0);

            //        else

            //            prrs.ServiceCode = getservicecodeforitemcode(prrs.ItemId);

            //}
            var requesterid = _ServiceCreationService.GetSingleItemser(prrs.ItemId);

            prrs.requester = requesterid.requester;
            prrs._id = requesterid._id;
            var res = _ServiceCreationService.InsertServiceCreation(prrs);


            return this.Json(mylist, JsonRequestBehavior.AllowGet);




        }
        //private string generateServicecodeunspsc(string SubCode)
        //{
        //    // string itmCode = "";
        //    // string indicator = Convert.ToString(indc);
        //    var code = _ServiceCreationService.generateServicecodeunspsc(SubCode);
        //    return code;
        //}
        private string generateServicecodeunspsc(string LogicCode, int indc)
        {
            string itmCode = "";
            string indicator = Convert.ToString(indc);
            var code = _ServiceCreationService.generateServicecodeunspsc(LogicCode);

            if (code != "")
            {
                long serialinr = 0;
                if (indc == 1)
                {
                    serialinr = Convert.ToInt64(code.Substring(6, 4));
                }
                else serialinr = Convert.ToInt64(code.Substring(8, 4));

                serialinr++;
                string addincr = "";
                switch (serialinr.ToString().Length)
                {
                    case 1:
                        addincr = "000" + serialinr;
                        break;
                    case 2:
                        addincr = "00" + serialinr;
                        break;
                    case 3:
                        addincr = "0" + serialinr;
                        break;
                    case 4:
                        addincr = serialinr.ToString();
                        break;
                }
                itmCode = LogicCode + addincr;

            }
            else
            {
                itmCode = LogicCode + "0001";
            }

            return itmCode;
        }
        private string getservicecodeforitemcode(string itemcode)
        {
            // string itmCode = "";
            // string indicator = Convert.ToString(indc);
            itemcode = itemcode.Replace(@"-", "");
            // var code = _ServiceCreationService.generateServicecodeitem(itemcode);
            return itemcode;
        }
        [Authorize]
        public List<string> createshortlong1(List<AttrubutesSL> att, Service_Request prs)
        {
            List<AttrubutesSL> m_mul_req_values = att;
            string shortdesc = "";
            string ab = "";
            for (int o = 1; o <= m_mul_req_values.Count; o++)
                foreach (AttrubutesSL ats in m_mul_req_values)
                {
                    if (ats.Sequence == o)
                    {
                        if (shortdesc == "")
                        {
                            if (ats.Value != null && ats.Value != "" && ats.Value != "undefined") //if (ats.Value != null)
                            {
                                ab = _ServiceCreationService.getabbr(ats.Value.ToString().ToUpper());

                                if (!string.IsNullOrEmpty(ab) && ab.Length <= 40)
                                    shortdesc = ab;
                                else shortdesc = ats.Value;
                            }
                        }
                        else
                        {
                            if (ats.Value != null && ats.Value != "" && ats.Value != "undefined")
                            {
                                ab = _ServiceCreationService.getabbr(ats.Value.ToString().ToUpper());
                                if (ab != null && ab != " " && ab != "")
                                {
                                    if ((shortdesc + ab).ToString().Length <= 39)
                                    {
                                        shortdesc = shortdesc + "," + ab;
                                    }
                                }
                                else
                                {
                                    if ((shortdesc + ats.Value.ToString()).ToString().Length <= 39)
                                    {
                                        shortdesc = shortdesc + "," + ats.Value;
                                    }
                                }



                            }
                        }
                    }
                }
            shortdesc = shortdesc.TrimEnd(',');

            // Long desc
            string longdesc = prs.Noun+"," + prs.Modifier;


            //var prrs1 = new Prosol_RequestService();

            //prrs1. ServiceCategoryName = prs.ServiceCategoryName;
            //prrs1.ServiceGroupName = prs.ServiceGroupName;
            //var res = _ServiceCreationService.InsertServiceCreation(prrs1);
            //int count = m_mul_req_values.Count;
            //int ct = 1;
            //var ctt = _ServiceMasterService.ShwDefaultAttr().ToList();
            //int maxatt = 0;
            //if (ctt.Count > 0)
            //{
            //    maxatt = ctt.Count;
            //}
            //else
            //{
            //    maxatt = 0;
            //}



            foreach (AttrubutesSL atsl in m_mul_req_values.OrderBy(x => x.Sequence))
            {

                {
                    if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                        longdesc = longdesc + "," + atsl.Attributes + ": " + atsl.Value;
                  
                }

                //if (longdesc == "")
                //{
                //    if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //        longdesc = atsl.Value;

                //    ct++;
                //}
                //else
                //{
                //    //if(ct > maxatt >0)                                                                                        
                //    //if (ct > maxatt && ct != count)
                //    //{
                //    //    if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //    //        longdesc = longdesc + "," + atsl.Attributes + ": " + atsl.Value;
                //    //    ct++;
                //    //}
                //    //else 
                //    if (ct == count)
                //    {
                //        if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //            longdesc = longdesc + "," + atsl.Value;
                //    }

                //    else
                //    {
                //        if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //            longdesc = longdesc + "," + atsl.Value;
                //        ct++;
                //    }
                //}
            }
                // string res = "";
                List<string> mylist = new List<string>(new string[] { shortdesc, longdesc.TrimEnd().ToUpper() });
            return mylist;
        }
        public List<string> createshortlong(List<AttrubutesSL> att)
        {
            List<AttrubutesSL> m_mul_req_values = att;
            string shortdesc = "";
            string ab = "";
            for (int o = 1; o <= m_mul_req_values.Count; o++)
                foreach (AttrubutesSL ats in m_mul_req_values)
                {
                    if (ats.Sequence == o)
                    {
                        if (shortdesc == "")
                        {
                            if (ats.Value != null && ats.Value != "" && ats.Value != "undefined") //if (ats.Value != null)
                            {
                                ab = _ServiceCreationService.getabbr(ats.Value.ToString().ToUpper());

                                if (!string.IsNullOrEmpty(ab) && ab.Length <= 40)
                                    shortdesc = ab;
                                else shortdesc = ats.Value;
                            }
                        }
                        else
                        {
                            if (ats.Value != null && ats.Value != "" && ats.Value != "undefined")
                            {
                                ab = _ServiceCreationService.getabbr(ats.Value.ToString().ToUpper());
                                if (ab != null && ab != " " && ab != "")
                                {
                                    if ((shortdesc + ab).ToString().Length <= 39)
                                    {
                                        shortdesc = shortdesc + "," + ab;
                                    }
                                }
                                else
                                {
                                    if ((shortdesc + ats.Value.ToString()).ToString().Length <= 39)
                                    {
                                        shortdesc = shortdesc + "," + ats.Value;
                                    }
                                }



                            }
                        }
                    }
                }
            shortdesc = shortdesc.TrimEnd(',');

            // Long desc
            string longdesc = "";
            //int count = m_mul_req_values.Count;
            //int ct = 1;
            //var ctt = _ServiceMasterService.ShwDefaultAttr().ToList();
            //int maxatt = 0;
            //if (ctt.Count > 0)
            //{
            //    maxatt = ctt.Count;
            //}
            //else
            //{
            //    maxatt = 0;
            //}



            foreach (AttrubutesSL atsl in m_mul_req_values.OrderBy(x => x.Sequence))
            {

                {
                    if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                        longdesc = longdesc + "," + atsl.Attributes + ": " + atsl.Value;

                }

                //if (longdesc == "")
                //{
                //    if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //        longdesc = atsl.Value;

                //    ct++;
                //}
                //else
                //{
                //    //if(ct > maxatt >0)                                                                                        
                //    //if (ct > maxatt && ct != count)
                //    //{
                //    //    if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //    //        longdesc = longdesc + "," + atsl.Attributes + ": " + atsl.Value;
                //    //    ct++;
                //    //}
                //    //else 
                //    if (ct == count)
                //    {
                //        if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //            longdesc = longdesc + "," + atsl.Value;
                //    }

                //    else
                //    {
                //        if (atsl.Value != null && atsl.Value != "" && atsl.Value != "undefined")
                //            longdesc = longdesc + "," + atsl.Value;
                //        ct++;
                //    }
                //}
            }
            // string res = "";
            List<string> mylist = new List<string>(new string[] { shortdesc, longdesc.TrimEnd().ToUpper() });
            return mylist;
        }
        [Authorize]
        public JsonResult InsertServiceCreation(string ItemId)
        {

            string uid = Convert.ToString(Session["UserId"]);
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Cataloguer") tmpStr = ent.TargetId;
            }
            var relUname = _UserCreateService.getimage(tmpStr);
            string uID = tmpStr;
            string uName = relUname.UserName;


            //  var obj = Request.Form["obj"];
            //  var attributes = Request.Form["Attributess"];
            //  var rev = Request.Form["obj1"];
            //  Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
            //var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            //List<string> mylist = createshortlong(m_mul_req_values);

            ////if (prs.ShortDesc != null && prs.LongDesc != null)
            ////{
            //List<Prosol_ServiceAttributeList> sal = JsonConvert.DeserializeObject<List<Prosol_ServiceAttributeList>>(attributes);
            //prs.Characteristics = sal;
            //Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            //pub.Name = Session["username"].ToString();
            //pub.UserId = Session["userid"].ToString();
            //pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            //Prosol_UpdatedBy revie = JsonConvert.DeserializeObject<Prosol_UpdatedBy>(rev);
            //revie.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            //var data = _ServiceCreationService.getdatalistforupdate(prs._id);
            var data = _ServiceCreationService.GetSingleItemser(ItemId);
            data.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            //prrs.ServiceCategoryName = prs.ServiceCategoryName;
            //prrs.ServiceCategoryCode = prs.ServiceCategoryCode;
            //prrs.ServiceGroupName = prs.ServiceGroupName;
            //prrs.ServiceGroupCode = prs.ServiceGroupCode;
            //prrs.UomName = prs.UomName;
            //prrs.UomCode = prs.UomCode;


            //if (prs.ValuationName != "Select Valuation")
            //    prrs.ValuationName = prs.ValuationName;
            //else prrs.ValuationName = "";
            //prrs.Valuationcode = prs.Valuationcode;

            //prrs.MainCode = prs.MainCode;
            //if (prs.MainCodeName != "Select Main Code")
            //    prrs.MainCodeName = prs.MainCodeName;
            //else prrs.MainCodeName = "";
            ////prrs.MainCodeName = prs.MainCodeName;

            //prrs.SubCode = prs.SubCode;
            //if (prs.SubCodeName != "Select Sub Code")
            //    prrs.SubCodeName = prs.SubCodeName;
            //else prrs.SubCodeName = "";
            //// prrs.SubCodeName = prs.SubCodeName;

            //prrs.SubSubCode = prs.SubSubCode;
            //if (prs.SubSubCodeName != "Select Sub-Sub Code")
            //    prrs.SubSubCodeName = prs.SubSubCodeName;
            //else prrs.SubSubCodeName = "";
            //// prrs.SubSubCodeName = prs.SubSubCodeName;
            //prrs.Remarks = prs.Remarks;
            //prrs.Characteristics = prs.Characteristics;

            //prrs.Class = prs.Class;
            //prrs.ClassTitle = prs.ClassTitle;
            //prrs.Commodity = prs.Unspsc;
            //prrs.CommodityTitle = prs.CommodityTitle;

            //// prrs.ShortDesc = prs.ShortDesc;
            //prrs.ShortDesc = mylist[0].ToString();
            //// prrs.LongDesc = prs.LongDesc;
            //prrs.LongDesc = mylist[1].ToString();
            //prrs.UpdatedOn = DateTime.Now;
            //prrs.Cleanserr = pub;
            //prrs.Last_updatedBy = pub;
            //prrs.ServiceStatus = "Review";
            //prrs.RequestStatus = "Review";
            //prrs.Reviewer = revie;
            //prrs.ServiceCode = prrs.ServiceCode;
            //prrs.parent = prrs.parent;
            //string rn = "";

            //Review 
            var reviewer = new Prosol_UpdatedBy();
            reviewer.UserId = uID;
            reviewer.Name = uName;
            reviewer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            data.Reviewer = reviewer;
            data.ServiceStatus = "Review";
            data.RequestStatus = "Review";
            //if (prrs.ServiceCode == null)
            //{
            //    rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //    prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //}
            var res = _ServiceCreationService.InsertServiceCreation(data);
            //  mylist[2] = "true";
            return this.Json(res, JsonRequestBehavior.AllowGet);
            //  }
            //else
            //{
            //    return this.Json("shortlongempty", JsonRequestBehavior.AllowGet);
            //}
        }
        public JsonResult getcommlist(string sKey)
        {
            var srch = _ServiceCreationService.getCOMMList(sKey);
            var hsn = new List<Prosol_UNSPSC>();
            foreach (Prosol_UNSPSC C in srch)
            {
                var pro = new Prosol_UNSPSC();

                pro.Class = C.Class;
                pro.ClassTitle = C.ClassTitle;
                pro.Commodity = C.Commodity;
                pro.CommodityTitle = C.CommodityTitle;


                hsn.Add(pro);
            }

            var jsonResult = Json(hsn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult getcommcommlist(string sKey)
        {
            var srch = _ServiceCreationService.getCOMMCOMMList(sKey);
            var hsn = new List<Prosol_UNSPSC>();
            foreach (Prosol_UNSPSC C in srch)
            {
                var pro = new Prosol_UNSPSC();

                pro.Class = C.Class;
                pro.ClassTitle = C.ClassTitle;
                pro.Commodity = C.Commodity;
                pro.CommodityTitle = C.CommodityTitle;


                hsn.Add(pro);
            }

            var jsonResult = Json(hsn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult SaveServiceReview()
        {

            var obj = Request.Form["obj"];
            var attributes = Request.Form["Attributess"];
            var rev = Request.Form["obj1"];
            Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            List<string> mylist = createshortlong1(m_mul_req_values,prs);
            mylist.Add("");
            mylist.Add("");
            mylist.Add("false");


            //if (prs.ShortDesc != null && prs.LongDesc != null)
            //{
            List<Prosol_ServiceAttributeList> sal = JsonConvert.DeserializeObject<List<Prosol_ServiceAttributeList>>(attributes);
            prs.Characteristics = sal;
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            pub.Name = Session["username"].ToString();
            pub.UserId = Session["userid"].ToString();
            pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            Prosol_UpdatedBy release = JsonConvert.DeserializeObject<Prosol_UpdatedBy>(rev);
            release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            var data = _ServiceCreationService.getdatalistforupdate(prs._id);
            Prosol_RequestService prrs = data;


            prrs.PlantCode = prs.PlantCode;
            prrs.PlantName = prs.PlantName;
            prrs.ServiceCategoryName = prs.ServiceCategoryName;
            prrs.ServiceCategoryCode = prs.ServiceCategoryCode;
            prrs.ServiceGroupName = prs.ServiceGroupName;
            prrs.ServiceGroupCode = prs.ServiceGroupCode;
            prrs.UomName = prs.UomName;
            prrs.UomCode = prs.UomCode;
            prrs.Noun = prs.Noun;
            prrs.Modifier = prs.Modifier;
            if (prs.ValuationName != "Select Valuation")
                prrs.ValuationName = prs.ValuationName;
            else prrs.ValuationName = "";
            prrs.Valuationcode = prs.Valuationcode;

            prrs.MainCode = prs.MainCode;
            if (prs.MainCodeName != "Select Main Code")
                prrs.MainCodeName = prs.MainCodeName;
            else prrs.MainCodeName = "";
            //prrs.MainCodeName = prs.MainCodeName;

            prrs.SubCode = prs.SubCode;
            if (prs.SubCodeName != "Select Sub Code")
                prrs.SubCodeName = prs.SubCodeName;
            else prrs.SubCodeName = "";
            // prrs.SubCodeName = prs.SubCodeName;

            prrs.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                prrs.SubSubCodeName = prs.SubSubCodeName;
            else prrs.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;

            prrs.Remarks = prs.Remarks;
            prrs.Characteristics = prs.Characteristics;

            prrs.Class = prs.Class;
            prrs.ClassTitle = prs.ClassTitle;
            prrs.Commodity = prs.Unspsc;
            prrs.CommodityTitle = prs.CommodityTitle;

            prrs.ShortDesc = mylist[0].ToString();
            prrs.LongDesc = mylist[1].ToString();
            prrs.UpdatedOn = DateTime.Now;
            //  prrs.Cleanserr = pub;
            prrs.Last_updatedBy = pub;
            prrs.ServiceStatus = "QC";
            prrs.RequestStatus = "Review";
            prrs.Releaser = release;
            string rn = "";

            //if (prrs.ServiceCode == null && (prs.ServiceCode == ""))
            //{
            //var clcheck = _ServiceCreationService.getclchk();
            //if (clcheck == "Customized Code")
            //{
            //    if (prrs.ServiceCode == null || (prs.ServiceCode != null && prs.MainCode + prs.SubCode + prs.SubSubCode != prs.ServiceCode.Substring(0, 9)))
            //    {
            //        rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //        prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //    }
            //    else
            //    {
            //        prrs.ServiceCode = prs.ServiceCode;
            //    }
            //}
            //else if (clcheck == "UNSPSC Code")
            //{
            //    if (prs.ServiceCode != null && prs.SubCode != prs.ServiceCode.Substring(0, 8))
            //        prrs.ServiceCode = generateServicecodeunspsc(prrs.SubCode, 0);
            //}

            //else

            //    prrs.ServiceCode = getservicecodeforitemcode(prrs.ItemId);

            // }
            //if (prrs.ServiceCode == null || prrs.ServiceCode != null)
            //{
            //    rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //    prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //}
            var res = _ServiceCreationService.updateServiceReview(prrs);
            mylist[2] = "true";
            return this.Json(mylist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult updateServiceReview()
        {
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Reviewer") tmpStr = ent.TargetId;
            }
            var relUname = _UserCreateService.getimage(tmpStr);
            string uID = tmpStr;
            string uName = relUname.UserName;


            var obj = Request.Form["obj"];
            var attributes = Request.Form["Attributess"];
            var rev = Request.Form["obj1"];
            Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            List<string> mylist = createshortlong1(m_mul_req_values, prs);
            mylist.Add("");
            mylist.Add("");
            mylist.Add("false");

            //if (prs.ShortDesc != null && prs.LongDesc != null)
            //{
            List<Prosol_ServiceAttributeList> sal = JsonConvert.DeserializeObject<List<Prosol_ServiceAttributeList>>(attributes);
            prs.Characteristics = sal;
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            pub.Name = Session["username"].ToString();
            pub.UserId = Session["userid"].ToString();
            pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            Prosol_UpdatedBy release = JsonConvert.DeserializeObject<Prosol_UpdatedBy>(rev);
            release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            var data = _ServiceCreationService.getdatalistforupdate(prs._id);
            Prosol_RequestService prrs = data;


            prrs.PlantCode = prs.PlantCode;
            prrs.PlantName = prs.PlantName;
            prrs.ServiceCategoryName = prs.ServiceCategoryName;
            prrs.ServiceCategoryCode = prs.ServiceCategoryCode;
            prrs.ServiceGroupName = prs.ServiceGroupName;
            prrs.ServiceGroupCode = prs.ServiceGroupCode;
            prrs.UomName = prs.UomName;
            prrs.UomCode = prs.UomCode;
            prrs.Noun = prs.Noun;
            prrs.Modifier = prs.Modifier;
            prrs.ValuationName = prs.ValuationName;
            if (prs.ValuationName != "Select Valuation")
                prrs.ValuationName = prs.ValuationName;
            else prrs.ValuationName = "";
            prrs.Valuationcode = prs.Valuationcode;

            prrs.MainCode = prs.MainCode;
            if (prs.MainCodeName != "Select Main Code")
                prrs.MainCodeName = prs.MainCodeName;
            else prrs.MainCodeName = "";
            //prrs.MainCodeName = prs.MainCodeName;

            prrs.SubCode = prs.SubCode;
            if (prs.SubCodeName != "Select Sub Code")
                prrs.SubCodeName = prs.SubCodeName;
            else prrs.SubCodeName = "";
            // prrs.SubCodeName = prs.SubCodeName;

            prrs.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                prrs.SubSubCodeName = prs.SubSubCodeName;
            else prrs.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;

            prrs.Remarks = prs.Remarks;

            prrs.Characteristics = prs.Characteristics;

            prrs.Class = prs.Class;
            prrs.ClassTitle = prs.ClassTitle;
            prrs.Commodity = prs.Unspsc;
            prrs.CommodityTitle = prs.CommodityTitle;

            prrs.ShortDesc = mylist[0].ToString();
            prrs.LongDesc = mylist[1].ToString();
            prrs.UpdatedOn = DateTime.Now;
            //  prrs.Cleanserr = pub;
            prrs.Last_updatedBy = pub;
            prrs.ServiceStatus = "Release";
            prrs.RequestStatus = "Release";
            prrs.Releaser = release;

            prrs.ServiceCode = prrs.ServiceCode;

            var releaser = new Prosol_UpdatedBy();
            releaser.UserId = uID;
            releaser.Name = uName;
            releaser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            prrs.Releaser = releaser;

            //    string rn = "";
            //if (prrs.ServiceCode == null)
            //{
            //    rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //    prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //}
            var res = _ServiceCreationService.updateServiceReview(prrs);
            mylist[2] = "true";
            return this.Json(mylist, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return this.Json("shortlongempty", JsonRequestBehavior.AllowGet);
            //}
        }
        //saveservicerelse
        public JsonResult SaveServiceRelease()
        {
            var obj = Request.Form["obj"];
            var obj1 = Request.Form["obj1"];
            var attributes = Request.Form["Attributess"];
            var rev = Request.Form["obj1"];
            Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
            Service_Request prs1 = JsonConvert.DeserializeObject<Service_Request>(obj1);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            List<string> mylist = createshortlong1(m_mul_req_values,prs);
            mylist.Add("");
            mylist.Add("");
            mylist.Add("false");

            //if (prs.ShortDesc != null && prs.LongDesc != null)
            //{
            List<Prosol_ServiceAttributeList> sal = JsonConvert.DeserializeObject<List<Prosol_ServiceAttributeList>>(attributes);
            prs.Characteristics = sal;
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            pub.Name = Session["username"].ToString();
            pub.UserId = Session["userid"].ToString();
            pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            //Prosol_UpdatedBy release = JsonConvert.DeserializeObject<Prosol_UpdatedBy>(rev);
            //release.UpdatedOn = DateTime.Now;


            var data = _ServiceCreationService.getdatalistforupdate(prs._id);
            Prosol_RequestService prrs = data;
            prrs.PlantCode = prs.PlantCode;
            prrs.PlantName = prs.PlantName;
            prrs.ServiceCategoryName = prs1.ServiceCategoryName;
            prrs.ServiceCategoryCode = prs.ServiceCategoryCode;
            prrs.ServiceGroupName = prs1.ServiceGroupName;
            prrs.ServiceGroupCode = prs.ServiceGroupCode;
            prrs.UomName = prs.UomName;
            prrs.UomCode = prs.UomCode;
            prrs.Noun = prs.Noun;
            prrs.Modifier = prs.Modifier;
            if (prs.ValuationName != "Select Valuation")
                prrs.ValuationName = prs.ValuationName;
            else prrs.ValuationName = "";
            prrs.Valuationcode = prs.Valuationcode;

            prrs.MainCode = prs.MainCode;
            if (prs.MainCodeName != "Select")
                prrs.MainCodeName = prs.MainCodeName;
            else prrs.MainCodeName = "";
            //prrs.MainCodeName = prs.MainCodeName;

            prrs.SubCode = prs.SubCode;
            if (prs.SubCodeName != "Select")
                prrs.SubCodeName = prs.SubCodeName;
            else prrs.SubCodeName = "";
            // prrs.SubCodeName = prs.SubCodeName;

            prrs.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select")
                prrs.SubSubCodeName = prs.SubSubCodeName;
            else prrs.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;


            prrs.Remarks = prs.Remarks;
            prrs.Characteristics = prs.Characteristics;

            prrs.Class = prs.Class;
            prrs.ClassTitle = prs.ClassTitle;
            prrs.Commodity = prs.Unspsc;
            prrs.CommodityTitle = prs.CommodityTitle;

            prrs.ShortDesc = mylist[0];
            prrs.LongDesc = mylist[1];
            prrs.UpdatedOn = DateTime.Now;
            prrs.ServiceStatus = "QA";
            prrs.RequestStatus = "Release";
            prrs.Last_updatedBy = pub;
            string rn = "";

            //if (prrs.ServiceCode == null || prs.ServiceCode != null)
            //{
            //    var clcheck = _ServiceCreationService.getclchk();
            //    if (clcheck == "Customized Code")
            //    {
            //        if (prrs.ServiceCode == null || (prs.ServiceCode != null && prs.MainCode + prs.SubCode + prs.SubSubCode != prs.ServiceCode.Substring(0, 9)))
            //        {
            //            rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //            prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //        }
            //        else
            //        {
            //            prrs.ServiceCode = prs.ServiceCode;
            //        }
            //    }
            //    else if (clcheck == "UNSPSC Code")

            //        prrs.ServiceCode = generateServicecodeunspsc(prrs.Commodity);

            //    else

            //        prrs.ServiceCode = getservicecodeforitemcode(prrs.ItemId);

            //    }
            //if (prrs.ServiceCode == null || prrs.ServiceCode != null)
            //{
            //    rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //    prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //}
           
            var res = _ServiceCreationService.updateServiceRelease(prrs);
            mylist[2] = "true";
            return this.Json(mylist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateServiceRelease()
        {
            var obj = Request.Form["obj"];
            var attributes = Request.Form["Attributess"];
            var rev = Request.Form["obj1"];
            Service_Request prs = JsonConvert.DeserializeObject<Service_Request>(obj);
            Service_Request prs1 = JsonConvert.DeserializeObject<Service_Request>(rev);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(attributes);
            List<string> mylist = createshortlong1(m_mul_req_values, prs);
            mylist.Add("");
            mylist.Add("");
            mylist.Add("false");

            //if (prs.ShortDesc != null && prs.LongDesc != null)
            //{
            List<Prosol_ServiceAttributeList> sal = JsonConvert.DeserializeObject<List<Prosol_ServiceAttributeList>>(attributes);
            prs.Characteristics = sal;
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            pub.Name = Session["username"].ToString();
            pub.UserId = Session["userid"].ToString();
            pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            //Prosol_UpdatedBy release = JsonConvert.DeserializeObject<Prosol_UpdatedBy>(rev);
            //release.UpdatedOn = DateTime.Now;


            var data = _ServiceCreationService.getdatalistforupdate(prs._id);
            Prosol_RequestService prrs = data;
            prrs.PlantCode = prs.PlantCode;
            prrs.PlantName = prs.PlantName;
            prrs.ServiceCategoryName = prs1.ServiceCategoryName;
            prrs.ServiceCategoryCode = prs.ServiceCategoryCode;
            prrs.ServiceGroupName = prs1.ServiceGroupName;
            prrs.ServiceGroupCode = prs.ServiceGroupCode;
            prrs.UomName = prs.UomName;
            prrs.UomCode = prs.UomCode;
            prrs.Noun = prs.Noun;
            prrs.Modifier = prs.Modifier;
            if (prs.ValuationName != "Select Valuation")
                prrs.ValuationName = prs.ValuationName;
            else prrs.ValuationName = "";
            prrs.Valuationcode = prs.Valuationcode;

            prrs.MainCode = prs.MainCode;
            if (prs.MainCodeName != "Select Main Code")
                prrs.MainCodeName = prs.MainCodeName;
            else prrs.MainCodeName = "";
            //prrs.MainCodeName = prs.MainCodeName;

            prrs.SubCode = prs.SubCode;
            if (prs.SubCodeName != "Select Sub Code")
                prrs.SubCodeName = prs.SubCodeName;
            else prrs.SubCodeName = "";
            // prrs.SubCodeName = prs.SubCodeName;

            prrs.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                prrs.SubSubCodeName = prs.SubSubCodeName;
            else prrs.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;
            prrs.Remarks = prs.Remarks;
            prrs.Characteristics = prs.Characteristics;

            prrs.Class = prs.Class;
            prrs.ClassTitle = prs.ClassTitle;
            prrs.Commodity = prs.Unspsc;
            prrs.CommodityTitle = prs.CommodityTitle;

            prrs.ShortDesc = mylist[0];
            prrs.LongDesc = mylist[1];
            prrs.UpdatedOn = DateTime.Now;
            prrs.ServiceStatus = "Completed";
            prrs.RequestStatus = "Release";
            prrs.Last_updatedBy = pub;
            var rn = "";

            var clcheck = _ServiceCreationService.getclchk();
            if (clcheck == "Customized Code")
            {
                if (prrs.ServiceCode == null || (prs.ServiceCode != null && prs.MainCode + prs.SubCode + prs.SubSubCode != prs.ServiceCode.Substring(0, 9)))
                {
                    rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
                    prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
                }
                else
                {
                    prrs.ServiceCode = prs.ServiceCode;
                }
            }
            else if (clcheck == "UNSPSC Code")
            {
                if (prs.ServiceCode != null && prs.SubCode != prs.ServiceCode.Substring(0, 8))
                {
                    prrs.ServiceCode = generateServicecodeunspsc(prrs.SubCode, 0);
                }
                else
                {
                    prrs.ServiceCode = generateServicecodeunspsc(prrs.Commodity, 0);
                }
            }

            else

                prrs.ServiceCode = getservicecodeforitemcode(prrs.ItemId);
            //}
            //else
            //{

            //}
            //   string rn = "";
            //if (prrs.ServiceCode == null)
            //{
            //    rn = _ServiceCreationService.getRunningNo(prs.MainCode, prs.SubCode, prs.SubSubCode);
            //    prrs.ServiceCode = prrs.MainCode + prrs.SubCode + prrs.SubSubCode + rn;
            //}
          



           

            var res = _ServiceCreationService.updateServiceRelease(prrs);
                mylist[2] = "true";
            
            return this.Json(mylist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult senttoReview(string _id, string Remarks)
        {
            var res = _ServiceCreationService.senttoReview(_id, Remarks);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult senttocleanser(string _id, string Remarks)
        {
            var res = _ServiceCreationService.senttocleanser(_id, Remarks);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getdatalistforCleanser()
        {
            if (Session["userid"].ToString() != null)
            {
                string Cleanser = Session["userid"].ToString();
                var cleans = _ServiceCreationService.getdatalistforCleanser(Cleanser);

                List<Service_Request> SR = new List<Service_Request>();
                foreach (Prosol_RequestService prs in cleans)
                {
                    Service_Request sr1 = new Service_Request();
                    sr1._id = prs._id.ToString();
                    sr1.RequestId = prs.RequestId;
                    sr1.ItemId = prs.ItemId;
                    sr1.ServiceCode = prs.ServiceCode;
                    sr1.PlantName = prs.PlantName;
                    sr1.PlantCode = prs.PlantCode;
                    sr1.Legacy = prs.Legacy;
                    sr1.ServiceCategoryName = prs.ServiceCategoryName;
                    sr1.ServiceCategoryCode = prs.ServiceCategoryCode;
                    sr1.ServiceGroupName = prs.ServiceGroupName;
                    sr1.ServiceGroupCode = prs.ServiceGroupCode;
                    sr1.UomName = prs.UomName;
                    sr1.UomCode = prs.UomCode;

                    sr1.Noun = prs.Noun;
                    sr1.Modifier = prs.Modifier;
                    sr1.Valuationcode = prs.Valuationcode;
                    sr1.ValuationName = prs.ValuationName;
                    sr1.MainCode = prs.MainCode;
                    sr1.MainCodeName = prs.MainCodeName;
                    sr1.SubCode = prs.SubCode;
                    sr1.SubCodeName = prs.SubCodeName;
                    sr1.SubSubCode = prs.SubSubCode;
                    sr1.SubSubCodeName = prs.SubSubCodeName;
                    sr1.Characteristics = prs.Characteristics;
                    sr1.ShortDesc = prs.ShortDesc;
                    sr1.LongDesc = prs.LongDesc;
                    sr1.UpdatedOn = prs.UpdatedOn;
                    sr1.requester = prs.requester;
                    //sr1.Cleanserr = prs.Cleanserr;
                    sr1.Reviewer = prs.Reviewer;
                    sr1.Releaser = prs.Releaser;
                    sr1.ServiceStatus = prs.ServiceStatus;
                    sr1.RequestStatus = prs.RequestStatus;
                    sr1.RejectedBy = prs.RejectedBy;
                    sr1.Reject_reason = prs.Reject_reason;
                    var cleansername = _UserCreateService.getcleansername(prs.Cleanser);
                    sr1.Cleanser = cleansername.UserName;
                    sr1.Approvedon = prs.Approvedon;
                    sr1.approver = prs.approver;
                    sr1.Last_updatedBy = prs.Last_updatedBy;

                    sr1.Class = prs.Class;
                    sr1.ClassTitle = prs.ClassTitle;
                    sr1.Unspsc = prs.Commodity;
                    sr1.CommodityTitle = prs.CommodityTitle;
                    
                    //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    // DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(sr1.Last_updatedBy.UpdatedOn, INDIAN_ZONE);
                    //  DateTime indianTime = DateTime.SpecifyKind(sr1.Last_updatedBy.UpdatedOn, DateTimeKind.Utc);
                    // sr1.last_updated_date = indianTime.ToString();
                    sr1.last_updated_date = sr1.Last_updatedBy.UpdatedOn.ToString();

                    SR.Add(sr1);
                }
                var newList = SR.OrderBy(x => x.ServiceStatus).ToList();

                return this.Json(newList, JsonRequestBehavior.AllowGet);

                // return this.Json(SR, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetclarifyCode1(string ItemId, string Remarks, string usr, string Noun, string Modifier)
        {
            var objitem = Request.Form["obj"];
            var Attributeitem = Request.Form["Attributess"];


            var prs = JsonConvert.DeserializeObject<Service_Request>(objitem);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(Attributeitem);

            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();


            var data = _ServiceCreationService.GetSingleItemser(prs.ItemId);
            data.RequestStatus = "Clarification";
            data.ServiceStatus = "Cleanse";

            data.Clarification_On = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            data.Noun = prs.Noun;
            data.Modifier = prs.Modifier;
            data.UomName = prs.UomName;
            if (prs.ValuationName != "Select Valuation")
                data.ValuationName = prs.ValuationName;
            else data.ValuationName = "";
            data.Valuationcode = prs.Valuationcode;

            //data.Clarification_On = prs.Clarification_On;
            data.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                data.SubSubCodeName = prs.SubSubCodeName;
            else data.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;

            data.Remarks = prs.Remarks;
            data.Characteristics = prs.Characteristics;
            data.Class = prs.Class;
            data.ClassTitle = prs.ClassTitle;
            data.Commodity = prs.Unspsc;
            data.CommodityTitle = prs.CommodityTitle;
            data.parent = prs.parent;
            // prrs.ShortDesc = prs.ShortDesc;


            data.ItemId = prs.ItemId;
            data.RequestId = prs.RequestId;
            data.ServiceCode = prs.ServiceCode;
            data.Legacy = prs.Legacy;
            var result = _ServiceCreationService.InsertServiceCreation(data);
            if (result)
            {
                var tbl = new DataTable();
                tbl.Columns.Add("Item Code");
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Clarification Remarks");


                var row = tbl.NewRow();
                row["Item Code"] = prs.ItemId;
                row["Legacy"] = prs.Legacy;
                row["Clarification Remarks"] = prs.Remarks;
                tbl.Rows.Add(row);
                var usrinfo = _UserCreateService.Getreqinfo(data.requester.UserId);
                string tomail = usrinfo.EmailId;
                string subjectt = "Item Need Clarification";
                var x = _Emailservc.sendmail(tomail, subjectt, _Emailservc.getmailbody(tbl));

            }
            //}
            //        var sr1 = new Prosol_RequestService();
            //        sr1.Noun = prs1.Noun;
            //        sr1.Modifier = prs1.Modifier;
            //        sr1.Valuationcode = prs.Valuationcode;
            //        sr1.ValuationName = prs.ValuationName;
            //    }
            //}
            //foreach (AttrubutesSL prs1 in m_mul_req_values)
            //{
            //    var sr1 = new Prosol_MSAttribute();
            //    sr1.Noun = prs1.Noun;
            //    sr1.Modifier = prs1.Modifier;
            //    sr1.Valuationcode = prs.Valuationcode;
            //    sr1.ValuationName = prs.ValuationName;
            //}
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetclarifyCode(string ItemId, string Remarks, string usr, string Noun, string Modifier)
        {
            var objitem = Request.Form["obj"];
            var Attributeitem = Request.Form["Attributess"];


            var prs = JsonConvert.DeserializeObject<Service_Request>(objitem);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(Attributeitem);

            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();

            
            var data = _ServiceCreationService.GetSingleItemser(prs.ItemId);
           // data.RequestStatus = "Clarification";
            data.ServiceStatus= "Clarification";

            data.Clarification_On = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            data.Noun = prs.Noun;
            data.Modifier = prs.Modifier;
            data.UomName = prs.UomName;
            if (prs.ValuationName != "Select Valuation")
                data.ValuationName = prs.ValuationName;
            else data.ValuationName = "";
            data.Valuationcode = prs.Valuationcode;
           
            //data.Clarification_On = prs.Clarification_On;
            data.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                data.SubSubCodeName = prs.SubSubCodeName;
            else data.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;

            data.Remarks = prs.Remarks;
            data.Characteristics = prs.Characteristics;
            data.Class = prs.Class;
            data.ClassTitle = prs.ClassTitle;
            data.Commodity = prs.Unspsc;
            data.CommodityTitle = prs.CommodityTitle;
            data.parent = prs.parent;
            // prrs.ShortDesc = prs.ShortDesc;

           


            data.ItemId = prs.ItemId;
            data.RequestId = prs.RequestId;
            data.ServiceCode = prs.ServiceCode;
            data.Legacy = prs.Legacy;
            var result = _ServiceCreationService.InsertServiceCreation(data);

            if (result)
            {
                var tbl = new DataTable();
                tbl.Columns.Add("Item Code");
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Clarification Remarks");


                 var row = tbl.NewRow();
                row["Item Code"] = prs.ItemId;
                row["Legacy"] = prs.Legacy;
                row["Clarification Remarks"] = prs.Remarks;
                tbl.Rows.Add(row);
                var usrinfo = _UserCreateService.Getreqinfo(data.requester.UserId);
                string tomail = usrinfo.EmailId;
                string subjectt = "Item Need Clarification";
                var x = _Emailservc.sendmail(tomail, subjectt, _Emailservc.getmailbody(tbl));

            }

            //}
            //        var sr1 = new Prosol_RequestService();
            //        sr1.Noun = prs1.Noun;
            //        sr1.Modifier = prs1.Modifier;
            //        sr1.Valuationcode = prs.Valuationcode;
            //        sr1.ValuationName = prs.ValuationName;
            //    }
            //}
            //foreach (AttrubutesSL prs1 in m_mul_req_values)
            //{
            //    var sr1 = new Prosol_MSAttribute();
            //    sr1.Noun = prs1.Noun;
            //    sr1.Modifier = prs1.Modifier;
            //    sr1.Valuationcode = prs.Valuationcode;
            //    sr1.ValuationName = prs.ValuationName;
            //}
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetclarifyCode2(string ItemId, string Remarks, string usr, string Noun, string Modifier)
        {
            var objitem = Request.Form["obj"];
            var Attributeitem = Request.Form["Attributess"];


            var prs = JsonConvert.DeserializeObject<Service_Request>(objitem);
            var m_mul_req_values = JsonConvert.DeserializeObject<List<AttrubutesSL>>(Attributeitem);

            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();


            var data = _ServiceCreationService.GetSingleItemser(prs.ItemId);
            data.RequestStatus = "Clarification";
            data.ServiceStatus = "Cleanse";

            data.Clarification_On = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            data.Noun = prs.Noun;
            data.Modifier = prs.Modifier;
            data.UomName = prs.UomName;
            if (prs.ValuationName != "Select Valuation")
                data.ValuationName = prs.ValuationName;
            else data.ValuationName = "";
            data.Valuationcode = prs.Valuationcode;

            //data.Clarification_On = prs.Clarification_On;
            data.SubSubCode = prs.SubSubCode;
            if (prs.SubSubCodeName != "Select Sub-Sub Code")
                data.SubSubCodeName = prs.SubSubCodeName;
            else data.SubSubCodeName = "";
            // prrs.SubSubCodeName = prs.SubSubCodeName;

            data.Remarks = prs.Remarks;
            data.Characteristics = prs.Characteristics;
            data.Class = prs.Class;
            data.ClassTitle = prs.ClassTitle;
            data.Commodity = prs.Unspsc;
            data.CommodityTitle = prs.CommodityTitle;
            data.parent = prs.parent;
            // prrs.ShortDesc = prs.ShortDesc;


            data.ItemId = prs.ItemId;
            data.RequestId = prs.RequestId;
            data.ServiceCode = prs.ServiceCode;
            data.Legacy = prs.Legacy;
            var result = _ServiceCreationService.InsertServiceCreation(data);
            if (result)
            {
                var tbl = new DataTable();
                tbl.Columns.Add("Item Code");
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Clarification Remarks");


                var row = tbl.NewRow();
                row["Item Code"] = prs.ItemId;
                row["Legacy"] = prs.Legacy;
                row["Clarification Remarks"] = prs.Remarks;
                tbl.Rows.Add(row);
                var usrinfo = _UserCreateService.Getreqinfo(data.requester.UserId);
                string tomail = usrinfo.EmailId;
                string subjectt = "Item Need Clarification";
                var x = _Emailservc.sendmail(tomail, subjectt, _Emailservc.getmailbody(tbl));

            }
            //}
            //        var sr1 = new Prosol_RequestService();
            //        sr1.Noun = prs1.Noun;
            //        sr1.Modifier = prs1.Modifier;
            //        sr1.Valuationcode = prs.Valuationcode;
            //        sr1.ValuationName = prs.ValuationName;
            //    }
            //}
            //foreach (AttrubutesSL prs1 in m_mul_req_values)
            //{
            //    var sr1 = new Prosol_MSAttribute();
            //    sr1.Noun = prs1.Noun;
            //    sr1.Modifier = prs1.Modifier;
            //    sr1.Valuationcode = prs.Valuationcode;
            //    sr1.ValuationName = prs.ValuationName;
            //}
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
      
        public JsonResult getdatalistforReviewer()
        {
            if (Session["userid"].ToString() != null)
            {
                string Cleanser = Session["userid"].ToString();
                var cleans = _ServiceCreationService.getdatalistforReviewer(Cleanser);
                List<Service_Request> SR = new List<Service_Request>();
                foreach (Prosol_RequestService prs in cleans)
                {
                    Service_Request sr1 = new Service_Request();
                    sr1._id = prs._id.ToString();
                    sr1.RequestId = prs.RequestId;
                    sr1.ItemId = prs.ItemId;
                    sr1.ServiceCode = prs.ServiceCode;
                    sr1.PlantName = prs.PlantName;
                    sr1.ServiceCode = prs.ServiceCode;
                    sr1.Legacy = prs.Legacy;
                    sr1.ServiceCategoryName = prs.ServiceCategoryName;
                    sr1.ServiceCategoryCode = prs.ServiceCategoryCode;
                    sr1.ServiceGroupName = prs.ServiceGroupName;
                    sr1.ServiceGroupCode = prs.ServiceGroupCode;
                    sr1.UomName = prs.UomName;
                    sr1.UomCode = prs.UomCode;
                    sr1.Noun = prs.Noun;
                    sr1.Modifier = prs.Modifier;
                    sr1.Valuationcode = prs.Valuationcode;
                    sr1.ValuationName = prs.ValuationName;
                    sr1.MainCode = prs.MainCode;
                    sr1.MainCodeName = prs.MainCodeName;
                    sr1.SubCode = prs.SubCode;
                    sr1.SubCodeName = prs.SubCodeName;
                    sr1.SubSubCode = prs.SubSubCode;
                    sr1.SubSubCodeName = prs.SubSubCodeName;
                    sr1.Characteristics = prs.Characteristics;
                    sr1.ShortDesc = prs.ShortDesc;
                    sr1.LongDesc = prs.LongDesc;
                    sr1.UpdatedOn = prs.UpdatedOn;
                    sr1.requester = prs.requester;
                    sr1.Cleanserr = prs.Cleanserr;
                    sr1.Reviewer = prs.Reviewer;
                    sr1.Releaser = prs.Releaser;
                    sr1.ServiceStatus = prs.ServiceStatus;

                    sr1.RequestStatus = prs.RequestStatus;
                    sr1.RejectedBy = prs.RejectedBy;
                    sr1.Reject_reason = prs.Reject_reason;

                    var cleansername = _UserCreateService.getcleansername(prs.Cleanser);
                    sr1.Cleanser = cleansername.UserName;

                    sr1.Class = prs.Class;
                    sr1.ClassTitle = prs.ClassTitle;
                    sr1.Unspsc = prs.Commodity;
                    sr1.CommodityTitle = prs.CommodityTitle;

                    //sr1.Last_updatedBy = prs.Last_updatedBy;
                    sr1.Last_updatedBy = prs.Last_updatedBy;
                    // TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    //  DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(sr1.Last_updatedBy.UpdatedOn, INDIAN_ZONE);
                    sr1.last_updated_date = sr1.Last_updatedBy.UpdatedOn.ToString();
                    SR.Add(sr1);

                }
                var newList = SR.OrderBy(x => x.ServiceStatus).ToList();

                return this.Json(newList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getdatalistforReleaser()
        {
            if (Session["userid"].ToString() != null)
            {
                string Cleanser = Session["userid"].ToString();
                var cleans = _ServiceCreationService.getdatalistforReleaser(Cleanser);
                List<Service_Request> SR = new List<Service_Request>();
                foreach (Prosol_RequestService prs in cleans)
                {
                    Service_Request sr1 = new Service_Request();
                    sr1._id = prs._id.ToString();
                    sr1.RequestId = prs.RequestId;
                    sr1.ItemId = prs.ItemId;
                    sr1.ServiceCode = prs.ServiceCode;
                    sr1.ServiceCode = prs.ServiceCode;
                    sr1.PlantName = prs.PlantName;
                    sr1.Legacy = prs.Legacy;
                    sr1.ServiceCategoryName = prs.ServiceCategoryName;
                    sr1.ServiceCategoryCode = prs.ServiceCategoryCode;
                    sr1.ServiceGroupName = prs.ServiceGroupName;
                    sr1.ServiceGroupCode = prs.ServiceGroupCode;
                    sr1.UomName = prs.UomName;
                    sr1.UomCode = prs.UomCode;
                    sr1.Noun = prs.Noun;
                    sr1.Modifier = prs.Modifier;
                    sr1.Valuationcode = prs.Valuationcode;
                    sr1.ValuationName = prs.ValuationName;
                    sr1.MainCode = prs.MainCode;
                    sr1.MainCodeName = prs.MainCodeName;
                    sr1.SubCode = prs.SubCode;
                    sr1.SubCodeName = prs.SubCodeName;
                    sr1.SubSubCode = prs.SubSubCode;
                    sr1.SubSubCodeName = prs.SubSubCodeName;
                    sr1.Characteristics = prs.Characteristics;
                    sr1.ShortDesc = prs.ShortDesc;
                    sr1.LongDesc = prs.LongDesc;
                    sr1.UpdatedOn = prs.UpdatedOn;
                    sr1.requester = prs.requester;
                    sr1.Cleanserr = prs.Cleanserr;
                    sr1.Reviewer = prs.Reviewer;
                    sr1.Releaser = prs.Releaser;
                    sr1.ServiceStatus = prs.ServiceStatus;
                    sr1.RequestStatus = prs.RequestStatus;
                    sr1.RejectedBy = prs.RejectedBy;
                    sr1.Reject_reason = prs.Reject_reason;
                    var cleansername = _UserCreateService.getcleansername(prs.Cleanser);
                    sr1.Cleanser = cleansername.UserName;

                    sr1.Class = prs.Class;
                    sr1.ClassTitle = prs.ClassTitle;
                    sr1.Unspsc = prs.Commodity;
                    sr1.CommodityTitle = prs.CommodityTitle;

                    //sr1.Last_updatedBy = prs.Last_updatedBy;
                    sr1.Last_updatedBy = prs.Last_updatedBy;
                    //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    // DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(sr1.Last_updatedBy.UpdatedOn, INDIAN_ZONE);
                    sr1.last_updated_date = sr1.Last_updatedBy.UpdatedOn.ToString();
                    SR.Add(sr1);
                }
                var newList = SR.OrderBy(x => x.ServiceStatus).ToList();

                return this.Json(newList, JsonRequestBehavior.AllowGet);

                //return this.Json(SR, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        //bulkuploadfor servicerequest

        public JsonResult Load_Data_for_bulkupload()
        {
            DataTable dtload = new DataTable();
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    var loaddata = _ServiceMasterService.loaddata1(file);
                    return Json(loaddata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Check File Format", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Uploaded File is Empty", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult bulkupload_service_request()
        {

            var mul_req_values1 = Request.Form["sResultbulk1"];
            // string date1 = DateTime.Now.ToString("MM/dd/yyyy");
            string date1 = DateTime.Now.ToString("yyyy/MM/dd");
            date1 = date1.Replace(@"-", "");
            date1 = date1.Replace(@"/", "");
            date1 = date1.Replace(@" ", "");

            int reqid = _ServiceMasterService.getlast_request_R_no1(date1);
            date1 = date1 + reqid.ToString();


            // var 

            var m_mul_req_values = JsonConvert.DeserializeObject<List<Prosol_RequestService>>(mul_req_values1);

            //   var app_list = m_mul_req_values.Select(m => new { m.Cleanser }).Distinct().ToList();
            //  var result = arrStr.Select(i => new { i.Noun, i.NounDefinition }).Distinct();
            //  var app_list = m_mul_req_values.Select(m => new { m.approver }).Distinct().ToList();
            var planttt = _ServiceMasterService.getplantCode_Name();
            var sl = _GeneralSettings.GetUnspsc("Service", "Service");
            var grp_b = _ServiceMasterService.getgroupCode_Name();
            var sgcode = _ServiceMasterService.getsuom_Name();
            //
            foreach (Prosol_RequestService pr2 in m_mul_req_values)
            {
                int plantil = 0;
                int slil = 0;
                int gril = 0;
                int sgil = 0;
                foreach (Prosol_Plants pp in planttt)
                {
                    if (pr2.PlantName == pp.Plantname)
                    {
                        plantil = 1;
                        pr2.PlantCode = pp.Plantcode;
                        pr2.PlantName = pp.Plantname;
                        break;
                    }
                }
                if (plantil == 0)
                {
                    return this.Json("Invalid Plant " + pr2.PlantName + " found", JsonRequestBehavior.AllowGet);
                }

                foreach (Prosol_UNSPSC pm in sl)
                {
                    //if (pr2.plant == pm.Plantcode && pr2.storage_Location == pm.Title)
                    if (pr2.ServiceCategoryName == pm.SegmentTitle)
                    {
                        slil = 1;
                        //pr2.ServiceCategoryName = pm.SeviceCategorycode;
                        pr2.ServiceCategoryCode = pm.Segment;
                        pr2.ServiceCategoryName = pm.SegmentTitle;
                        break;
                    }
                }
                if (slil == 0)
                {
                    return this.Json("Invalid ServiceCategoryName " + pr2.ServiceCategoryName + " found", JsonRequestBehavior.AllowGet);
                }

                foreach (Prosol_UNSPSC pm in sl)
                {
                    if (pr2.ServiceCategoryCode == pm.Segment && pr2.ServiceGroupName == pm.FamilyTitle)
                    {
                        sgil = 1;
                        // pr2.ServiceGroupName = pm.ServiceGroupcode;
                        pr2.ServiceGroupCode = pm.Family;
                        pr2.ServiceGroupName = pm.FamilyTitle;
                        break;
                    }
                }
                if (sgil == 0)
                {
                    return this.Json("Invalid Group " + pr2.ServiceGroupName + " found", JsonRequestBehavior.AllowGet);
                }

                foreach (Prosol_UOMMODEL pp in sgcode)
                {
                    if (pr2.UomName == pp.UOMNAME)
                    {
                        gril = 1;
                        pr2.UomName = pp.UOMNAME;
                        pr2.UomCode = pp.UOMNAME;
                        break;
                    }
                }
                if (gril == 0)
                {
                    return this.Json("Invalid UomName " + pr2.UomName + " found", JsonRequestBehavior.AllowGet);
                }

            }
            ///
            foreach (Prosol_RequestService pr2 in m_mul_req_values)
            {
                var Requester = new Prosol_UpdatedBy();
                Requester.UserId = Session["userid"].ToString();
                Requester.Name = Session["username"].ToString();
                Requester.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pr2.requester = Requester;
                pr2.Last_updatedBy = Requester;

                var Cleanser = new Prosol_UpdatedBy();
                Cleanser.UserId = _ServiceMasterService.getuserIDforrequest(pr2.Cleanser);
                Cleanser.Name = pr2.Cleanser;
                Cleanser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pr2.Cleanserr = Cleanser;
                pr2.Cleanser = Cleanser.UserId;
                //Cleanser.Name = pr2.Cleanser;
                //Cleanser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //pr2.Cleanserr = Cleanser;
            }
            var app_list = m_mul_req_values.Select(m => new { m.Cleanser }).Distinct().ToList();
            foreach (var item in app_list)
            {
                //string str1 = "<html><body> REQUEST ID : " + date1 + " <table style='border: 1px solid black;border-collapse:collapse'><tr><th style='border:1px solid black'>S.No</th><th style='border:1px solid black'>ServiceCategory</th><th style='border:1px solid black'>ServiceGroup</th><th style='border:1px solid black'>UOM</th><th style='border:1px solid black'>ServiceDescription</th></tr>";
                //string str2 = "";

                string app = item.Cleanser;
                //int i = 0;
                //foreach (Prosol_RequestService pr1 in m_mul_req_values)
                //{
                //    if (app == pr1.Cleanser)
                //    {
                //        str2 = "<tr><td style='border:1px solid black'>" + ++i + "</td><td style='border:1px solid black'>" + pr1.ServiceCategoryName + "</td><td style='border:1px solid black'>" + pr1.ServiceGroupName + "</td><td style='border:1px solid black'>" + pr1.UomName + "</td><td style='border:1px solid black'>" + pr1.Legacy + "</td></tr>";
                //        str1 = str1 + str2;
                //    }
                //}

                //string str3 = "</table></body ></html > ";
                //str1 = str1 + str3;
                try
                {
                    var user_deteails = _ServiceMasterService.get_approvercode_name_using_approverid1(app).ToList();

                    string to_mail = user_deteails[0].EmailId;
                    string subject = "";
                    // string userid = Data[0].Userid;
                    // email email1 = new email();
                    // email1.email_to = to_mail;
                    // email1.email_from = "codasol.madras@gmail.com";
                    if (Session["username"] != null)
                        subject = "New Request from " + Session["username"].ToString() + " REQUEST ID : " + date1;
                  //  string body = str1;
                    //email1.IsBodyHtml = true;
                    // email1.host = "smtp.gmail.com";
                    // email1.enablessl = true;
                    //email1.UseDefaultCredentials = true;
                    // email1.Port = 587;
                    // email1.password = "codasolwestmambalam";

                    // EmailSettingService ems = new EmailSettingService();
                    var tbl = new DataTable();
                    tbl.Columns.Add("PlantName");
                    tbl.Columns.Add("Category");
                    tbl.Columns.Add("Group");
                    tbl.Columns.Add("UOM");
                    tbl.Columns.Add("Legacy");


                    foreach (Prosol_RequestService pr1 in m_mul_req_values)
                    {
                        var row = tbl.NewRow();
                        row["PlantName"] = pr1.PlantName;
                        row["Category"] = pr1.ServiceCategoryName;
                        row["Group"] = pr1.ServiceGroupName;
                        row["UOM"] = pr1.UomName;
                        row["Legacy"] = pr1.Legacy;

                        tbl.Rows.Add(row);



                    }
                    bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));
                }
                catch
                {
                    string mul_resilt1 = "Error Sending Mail";
                    return this.Json(mul_resilt1, JsonRequestBehavior.AllowGet);
                }
            }
            bool mul_resilt = _ServiceMasterService.insert_multiplerequest1(m_mul_req_values, date1);
            if (mul_resilt == true)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);

            }
            else
                return this.Json(false, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getuomlist()
        {
            var result = _ServiceMasterService.getuomlist();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetValues(string Noun, string Modifier, string Attribute)
        {
            var arrStr = _ServiceMasterService.GetValues(Noun, Modifier, Attribute).ToList();
            //string[] str = new string[arrStr.Count*4];
            //if (arrStr.Count > 0)
            //{

            //    int i = 0;
            //    foreach(Prosol_Logic pl in arrStr)
            //    {
            //        if(pl.AttributeName1 == Attribute)
            //        {
            //            str[i] = pl.Value1;
            //            i++;
            //        }
            //        if (pl.AttributeName2 == Attribute)
            //        {
            //            str[i] = pl.Value2;
            //            i++;
            //        }
            //        if (pl.AttributeName3 == Attribute)
            //        {
            //            str[i] = pl.Value3;
            //            i++;
            //        }
            //        if (pl.AttributeName4 == Attribute)
            //        {
            //            str[i] = pl.Value4;
            //            i++;
            //        }
            //    }
            //}
            //var sttr = str.Distinct().ToArray();

            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

        public bool InsertDefaultAttr()
        {
            var obj = Request.Form["CharaRw"];
            ServiceDefaultAttr Model = JsonConvert.DeserializeObject<ServiceDefaultAttr>(obj);

            Prosol_ServiceDefaultAttr mdl = new Prosol_ServiceDefaultAttr();
            mdl.Attributes = Model.Attributes;
            var getresult = _ServiceMasterService.InsertDefaultAttr(mdl);
            return getresult;
        }

        public JsonResult ShwDefaultAttr()
        {
            var objList = _ServiceMasterService.ShwDefaultAttr();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<ServiceDefaultAttr>();
            foreach (Prosol_ServiceDefaultAttr mdl in objList)
            {
                var obj = new ServiceDefaultAttr();
                obj._id = mdl._id.ToString();
                obj.Attributes = mdl.Attributes;

                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteDefAttr(string id)
        {

            var res = _ServiceMasterService.DeleteDefAttr(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        //checkvalue
        public string CheckValue(string Noun, string Modifier, string Attribute, string Value)
        {
            var res = _ServiceCreationService.CheckValue(Noun, Modifier, Attribute, Value);

            return res;
        }
        //addvalue
        public bool AddValue1(string Noun,string Modifier, string Attribute, string Value, string abb)
        {
            var res = _ServiceCreationService.AddValue1(Noun,Modifier, Attribute, Value, abb);

            return res;
        }

        //
        public bool CodeSaveforservice()
        {

            var obj = Request.Form["obj"];
            Servicecodelogic Model = JsonConvert.DeserializeObject<Servicecodelogic>(obj);

            Prosol_Servicecodelogic mdl = new Prosol_Servicecodelogic();
            if (Model.SERVICECODELOGIC == "UNSPSC Code")
            {
                mdl.SERVICECODELOGIC = Model.SERVICECODELOGIC;
                mdl.Version = Model.Version;
            }
            else
            {
                mdl.SERVICECODELOGIC = Model.SERVICECODELOGIC;
                mdl.Version = Model.Version;
            }

            var getresult = _ServiceCreationService.codesaveforlogic(mdl);
            return getresult;
        }

        public JsonResult loadversionforservice()
        {
            var objList = _ServiceCreationService.loadversionforservice();

            return this.Json(objList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Showdataservice()
        {
            var objList = _ServiceCreationService.Showdataservice();
            if (objList != null)
            {
                var obj = new Servicecodelogic();

                obj._id = objList._id.ToString();

                obj.SERVICECODELOGIC = objList.SERVICECODELOGIC;
                obj.Version = objList.Version;
                return this.Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("UNSPC Code", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Showdatacodelogic()
        {
            var objList = _ServiceCreationService.Showdatacodelogic();


            return this.Json(objList.SERVICECODELOGIC, JsonRequestBehavior.AllowGet);



        }

        public JsonResult showall_user()
        {

            var userlist = _ServiceCreationService.showall_user().ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetcatList()
        {
            var dataList = _ServiceCreationService.GetcatList();

            var res = Json(dataList, JsonRequestBehavior.AllowGet);
            res.MaxJsonLength = int.MaxValue;
            return res;

        }
        //csi
        public ActionResult getItemser(string itemId)
        {

            var ser = _ServiceCreationService.GetSingleItemser(itemId);


            var proCat = new Prosol_RequestService();
            if (ser != null)
            {
                var lstCharateristics = new List<Prosol_ServiceAttributeList>();
                if (ser.Characteristics != null)
                {
                    foreach (Prosol_ServiceAttributeList pattri in ser.Characteristics)
                    {
                        var AttrMdl = new Prosol_ServiceAttributeList();
                        AttrMdl.Attributes = pattri.Attributes;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.Sequence = pattri.Sequence;

                        lstCharateristics.Add(AttrMdl);
                    }
                }


                // proCat._id = cat._id.ToString();
                //  proCat.Itemcode = cat.Itemcode;
                proCat.ServiceStatus = "Cleanse";
                //  proCat.Legacy = cat.Legacy;
                //  proCat.Legacy2 = cat.Legacy2;
                proCat.PlantCode = ser.PlantCode;
                proCat.PlantName = ser.PlantName;
                proCat.ServiceCategoryName = ser.ServiceCategoryName;
                proCat.ServiceCategoryCode = ser.ServiceCategoryCode;
                proCat.ServiceGroupName = ser.ServiceGroupName;
                proCat.ServiceGroupCode = ser.ServiceGroupCode;

                proCat.UomName = ser.UomName;
                proCat.UomCode = ser.UomCode;
                proCat.Noun = ser.Noun;
                proCat.Modifier = ser.Modifier;
                proCat.Valuationcode = ser.Valuationcode;
                proCat.ValuationName = ser.ValuationName;

                proCat.MainCode = ser.MainCode;
                proCat.MainCodeName = ser.MainCodeName;
                proCat.SubCode = ser.SubCode;
                proCat.SubCodeName = ser.SubCodeName;
                proCat.SubSubCode = ser.SubSubCode;
                proCat.SubSubCodeName = ser.SubSubCodeName;
                proCat.Remarks = ser.Remarks;
                proCat.Commodity = ser.Commodity;
                proCat.CommodityTitle = ser.CommodityTitle;
                proCat.Class = ser.Class;
                proCat.ClassTitle = ser.ClassTitle;
                proCat.Characteristics = lstCharateristics;

            }

            return this.Json(proCat, JsonRequestBehavior.AllowGet);


        }
        //public JsonResult GetNounModifier(string Noun, string Modifier)
        //{
        //    if (Noun != "null" && Modifier != "null" && Noun != null && Modifier != null)
        //    {
        //        var arrStr = _NounModifiService.GetNounModifier(Noun, Modifier);
        //        if (arrStr != null)
        //        {
        //            var AttributeList = _CharateristicService.GetAttributes().ToList();
        //            var NM = new NounModifierModel();
        //            NM._id = (arrStr != null) ? arrStr._id.ToString() : null;
        //            NM.Noun = arrStr.Noun;
        //            NM.Modifier = arrStr.Modifier;
        //            NM.Nounabv = arrStr.Nounabv;
        //            NM.NounDefinition = arrStr.NounDefinition;
        //            NM.Modifierabv = arrStr.Modifierabv;
        //            NM.ModifierDefinition = arrStr.ModifierDefinition;
        //            NM.Formatted = arrStr.Formatted;
        //            NM.FileData = arrStr.FileData;
        //            NM.uomlist = arrStr.uomlist;

        //            var NMC_VM = new NounModifierVM();
        //            NMC_VM.One_NounModifier = NM;

        //            var arrChar = _CharateristicService.GetCharateristic(Noun, Modifier);
        //            var lstChar = new List<NM_AttributesModel>();
        //            foreach (Prosol_Charateristics nm_Char in arrChar)
        //            {
        //                var Chara = new NM_AttributesModel();
        //                Chara.Noun = nm_Char.Noun;
        //                Chara.Modifier = nm_Char.Modifier;
        //                Chara.Characteristic = nm_Char.Characteristic;
        //                //  Chara.Abbrivation = nm_Char.Abbrivation;
        //                Chara.Squence = nm_Char.Squence;
        //                Chara.ShortSquence = nm_Char.ShortSquence;
        //                Chara.Mandatory = nm_Char.Mandatory;
        //                Chara.Definition = nm_Char.Definition;
        //                Chara.Values = nm_Char.Values;
        //                Chara.Values = nm_Char.Values;
        //                Chara.Uom = nm_Char.Uom;
        //                Chara.UomMandatory = nm_Char.UomMandatory;

        //                var sObj = (from obj in AttributeList where obj.Attribute.Equals(nm_Char.Characteristic, StringComparison.OrdinalIgnoreCase) select obj).FirstOrDefault();
        //                if (sObj != null)
        //                    //  Chara.Validation = sObj.Validation == 1 ? @"^[0-9-./]+$" : "";
        //                    Chara.Validation = sObj.Validation == 1 ? @"^[0-9](.*[0-9])?$" : "";
        //                else Chara.Validation = "";
        //                Chara.Remove = 0;
        //                lstChar.Add(Chara);
        //            }
        //            NMC_VM.ALL_NM_Attributes = lstChar;
        //            return this.Json(NMC_VM, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //            return this.Json(null, JsonRequestBehavior.AllowGet);

        //    }
        //    else
        //        return this.Json(null, JsonRequestBehavior.AllowGet);


        //}


        public JsonResult searchMaster1()
        {

            string sCode = Request.Form["sCode"];
            string iCode = Request.Form["iCode"];
            string sSource = Request.Form["sSource"];
            string sCategory = Request.Form["sCategory"];
            string sShort = Request.Form["sShort"];
            string sLong = Request.Form["sLong"];
            string sGroup = Request.Form["sGroup"];
            string sUser = Request.Form["sUser"];
            string sStatus = Request.Form["sStatus"];
            //string sType = Request.Form["sType"];
            var dataList = _ServiceCreationService.searchMaster1(sCode, iCode, sSource, sShort, sLong, sCategory, sGroup, sUser, sStatus);
            var locObjList = new List<Service_Request>();
            if (dataList != null && dataList.Count > 0)
            {
                foreach (Prosol_RequestService mdl in dataList)
                {
                    var locMdl = new Service_Request();
                    locMdl._id = mdl._id.ToString();
                    locMdl.ItemId = mdl.ItemId;
                    locMdl.ServiceCode = mdl.ServiceCode;
                    locMdl.Legacy = mdl.Legacy;
                    locMdl.ShortDesc = mdl.ShortDesc;
                    locMdl.LongDesc = mdl.LongDesc;
                    locMdl.ServiceCategoryName = mdl.ServiceCategoryName;
                    locMdl.ServiceGroupName = mdl.ServiceGroupName;
                    locMdl.ServiceStatus = mdl.ServiceStatus;
                    // locMdl.requester = mdl.requester;
                    //locMdl.Reworkcat = mdl.Reworkcat;
                    //locMdl.Rework = mdl.Rework;
                    //locMdl.Junk = mdl.Junk;
                    if (mdl.requester != null)
                    {
                        var updted = new Prosol_UpdatedBy();
                        updted.Name = mdl.requester.Name;
                        locMdl.requester = updted;
                    }
                    if (mdl.Cleanserr != null)
                    {
                        var updted = new Prosol_UpdatedBy();
                        updted.Name = mdl.Cleanserr.Name;
                        locMdl.Cleanserr = updted;
                    }

                    if (mdl.Reviewer != null)
                    {
                        var updted = new Prosol_UpdatedBy();
                        updted.Name = mdl.Reviewer.Name;
                        locMdl.Reviewer = updted;
                    }
                    if (mdl.Releaser != null)
                    {
                        var updted = new Prosol_UpdatedBy();
                        updted.Name = mdl.Releaser.Name;
                        locMdl.Releaser = updted;
                    }

                    locObjList.Add(locMdl);

                }
            }

            // { "Itemcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release" };

            var jsonResult = Json(locObjList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public class PaingGroup
        {
            ///<summary>
            /// Gets or sets Customers.
            ///</summary>
            public List<AbbrevateModel> ValueList { get; set; }

            ///<summary>
            /// Gets or sets CurrentPageIndex.
            ///</summary>
            public int CurrentPageIndex { get; set; }
            public int totItem { get; set; }
            ///<summary>
            /// Gets or sets PageCount.
            ///</summary>
            public int PageCount { get; set; }
            public List<AbbrevateModel> Characteristicvalues { get; set; }

            public List<AbbrevateModel> Abbrevatedvalues { get; set; }

        }




        // SERVICE MAPPING

        [Authorize]
        public JsonResult Loaditem()
        {
            var srchList = _ServiceMasterService.frstmat();
            var lstCatalogue = new List<Service_Request>();
            foreach (Prosol_RequestService cat in srchList)
            {
                var proCat = new Service_Request();
                proCat.ServiceCode = cat.ServiceCode;
                proCat.Legacy = cat.Legacy;
                proCat.MainCode = cat.MainCode;
                proCat.SubCode = cat.SubCode;
                proCat.ShortDesc = cat.ShortDesc;
                proCat.LongDesc = cat.LongDesc;
                proCat.UomCode = cat.UomCode;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.parent = cat.parent;
                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        [Authorize]
        public JsonResult searching1(string sKey, string ServiceCode)
        {
            var srchList = _ServiceMasterService.searching1(sKey);
            var lstCatalogue = new List<Service_Request>();
            foreach (Prosol_RequestService cat in srchList)
            {
                if (ServiceCode != cat.ServiceCode)
                {
                    var proCat = new Service_Request();
                    proCat.ServiceCode = cat.ServiceCode;
                    proCat.Legacy = cat.Legacy;
                    proCat.MainCode = cat.MainCode;
                    proCat.SubCode = cat.SubCode;
                    proCat.ShortDesc = cat.ShortDesc;
                    proCat.LongDesc = cat.LongDesc;
                    proCat.UomCode = cat.UomCode;
                    proCat.Noun = cat.Noun;
                    proCat.Modifier = cat.Modifier;
                    proCat.parent = cat.parent;
                    lstCatalogue.Add(proCat);
                }

            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        [Authorize]
        public JsonResult searching(string sKey)
        {
            var srchList = _ServiceMasterService.searching(sKey);
            var lstCatalogue = new List<Service_Request>();
            foreach (Prosol_RequestService cat in srchList)
            {
                var proCat = new Service_Request();
                proCat.ServiceCode = cat.ServiceCode;
                proCat.Legacy = cat.Legacy;
                proCat.MainCode = cat.MainCode;
                proCat.SubCode = cat.SubCode;
                proCat.ShortDesc = cat.ShortDesc;
                proCat.LongDesc = cat.LongDesc;
                proCat.UomCode = cat.UomCode;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult srchchild(string sKey)
        {
            var bom = _ServiceMasterService.srchchild(sKey);
            var lst = new List<Service_Request>();
            if (bom[0].Child != null)
            {
                foreach (string BM in bom[0].Child)
                {
                    var b = _ServiceMasterService.srchchild(BM);
                    var Bom = new Service_Request();
                    Bom.ServiceCode = b[0].ServiceCode;
                    Bom.Legacy = b[0].Legacy;
                    Bom.LongDesc = b[0].LongDesc;
                    Bom.ShortDesc = b[0].ShortDesc;
                    Bom.UomCode = b[0].UomCode;
                    Bom.parent = b[0].parent;

                    lst.Add(Bom);
                }

            }
            else
            {
                lst = null;
            }

            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }
        [Authorize]
        public JsonResult CreateChild()
        {
            // int res = 0;            
            var item1 = Request.Form["request"];
            string item2 = JsonConvert.DeserializeObject<string>(Request.Form["ServiceCode"]);
            var ser = JsonConvert.DeserializeObject<List<Service_Request>>(item1);
            List<string> lst = new List<string>();

            foreach (Service_Request EBM1 in ser)
            {
                lst.Add(EBM1.ServiceCode);
            }
            int res = _ServiceMasterService.InsertChildData(lst.ToArray(), item2);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getRequested_Records()
        {
            var req_records = _ServiceMasterService.get_itemsToApprove(Session["userid"].ToString());
            foreach (Prosol_RequestService prm in req_records)
            {
                // string str = prm.requestedOn.ToLongDateString();
                string str = DateTime.Parse(prm.requester.UpdatedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " +str.Substring(str.Length-4);
                prm.RequestStatus = str;
            }

            return this.Json(req_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getsingle_requested_record(string abcsony)
        {
            var singlerecord = _ServiceMasterService.getsingle_requested_record(abcsony);
             //var app_name = _ServiceMasterService.getcleansername(singlerecord[0].Cleanser);
            return this.Json(singlerecord, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult getApproved_Records()
        {
            var app_records = _ServiceMasterService.getappApproved_Records(Session["userid"].ToString());
            foreach (Prosol_RequestService prm in app_records)
            {
                //  string str = prm.approvedOn.ToLongDateString();
                string str = DateTime.Parse(prm.Approvedon.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                prm.RequestStatus = str;


            }

            return this.Json(app_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getApproved_Records1()
        {
            var app_records = _ServiceMasterService.getreqApproved_Records(Session["userid"].ToString());
            foreach (Prosol_RequestService prm in app_records)
            {
                //  string str = prm.approvedOn.ToLongDateString();
                string str = DateTime.Parse(prm.Approvedon.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                prm.RequestStatus = str;


            }

            return this.Json(app_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getRejected_Records()
        {
            var rej_records = _ServiceMasterService.getRejected_Records(Session["userid"].ToString());
            foreach (Prosol_RequestService prm in rej_records)
            {
                int index = prm.Rejectedon.ToString().IndexOf(" ");

                string str = prm.Rejectedon != null ? prm.Rejectedon.ToString().Substring(0, index) : null;
                prm.RequestStatus = str;
                //prm.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //prm.requestStatus = prm.rejectedOn.ToString().Substring(0, 10);
                //string str = prm.rejectedOn.ToLongDateString();
                //string str = DateTime.Parse(prm.rejectedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                //prm.requestStatus = str;
            }

            return this.Json(rej_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getRejected_Records1()
        {
            var rej_records = _ServiceMasterService.getreqRejected_Records(Session["userid"].ToString());
            foreach (Prosol_RequestService prm in rej_records)
            {
                int index = prm.Rejectedon.ToString().IndexOf(" ");

                string str = prm.Rejectedon != null ? prm.Rejectedon.ToString().Substring(0, index) : null;
                prm.RequestStatus = str;
                //prm.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //prm.requestStatus = prm.rejectedOn.ToString().Substring(0, 10);
                //string str = prm.rejectedOn.ToLongDateString();
                //string str = DateTime.Parse(prm.rejectedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                //prm.requestStatus = str;
            }

            return this.Json(rej_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getclarification_Records()
        {
            var clar_records = _ServiceMasterService.getClarification_Records(Session["userid"].ToString());

            foreach (Prosol_RequestService prm in clar_records)
            {
                int index = prm.Clarification_On.ToString().IndexOf(" ");

                string str = prm.Clarification_On != null ? prm.Clarification_On.ToString().Substring(0, index) : null;
                prm.RequestStatus = str;
               
                //prm.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //prm.requestStatus = prm.rejectedOn.ToString().Substring(0, 10);
                //string str = prm.rejectedOn.ToLongDateString();
                //string str = DateTime.Parse(prm.rejectedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                //prm.requestStatus = str;
            }

            return this.Json(clar_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult ReRequest()
        {

            var rerequestitem = Request.Form["Rerequest"];

            var files = Request.Files.Count > 0 ? Request.Files : null;

            var deReRequest = JsonConvert.DeserializeObject<List<Prosol_RequestService>>(rerequestitem);

        
          
                bool mul_resilt = _ServiceMasterService.insert_Rerequest(deReRequest);
                return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);
            
        }
        [Authorize]
        public JsonResult submitservice_approval()
        {
            var form_data = Request.Form["values"];

            var pro_req = JsonConvert.DeserializeObject<Prosol_RequestService>(form_data);
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Approver") tmpStr = ent.TargetId;
            }
            pro_req.Cleanser = tmpStr;
            var catname = _ServiceMasterService.getcatname(pro_req.Cleanser);

            // pro_req.Cleanserr.Name = catname.UserName.ToString();
            var Cleanserr = new Prosol_UpdatedBy();
            Cleanserr.Name = catname.UserName;
            //pro_req.Cleanserr.Name = Cleanserr.Name;
            if (pro_req.RequestStatus == "Service approved")
            {


                pro_req.Approvedon = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                bool result = _ServiceMasterService.submit_servcapproval(pro_req, pub, catname.UserName);
                //var user_deteails = _ServiceMasterService.get_cataloguer_emailid(pro_req.cataloguer).ToList();
                //pub.UserId = user_deteails[0].Userid;
                //pub.Name = user_deteails[0].UserName;
                //pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                // string to_mail = user_deteails[0].EmailId;
                //string subject = "";
                //// string userid = Data[0].Userid;
                //// email email1 = new email();
                //// email1.email_to = to_mail;
                ////  email1.email_from = "codasol.madras@gmail.com";
                //if (Session["username"].ToString() != null)
                //    subject = "New Request from " + Session["username"].ToString();
                //else
                //    subject = "New Request from Prosol";
                //string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Item Code</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.ItemId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Source</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.Legacy + "</td></tr></table></body></html>";
                //bool val = _Emailservc.sendmail(to_mail, subject, body);
                // email1.IsBodyHtml = true;
                // email1.host = "smtp.gmail.com";
                // email1.enablessl = true;
                // email1.UseDefaultCredentials = true;
                //email1.Port = 587;
                // email1.password = "codasolwestmambalam";

                // EmailSettingService ems = new EmailSettingService();
                string to_mail = catname.EmailId;
                string subject = "";
                if (Session["username"].ToString() != null)
                    subject = "New Request approved by " + Session["username"].ToString() + " REQUEST ID : " + pro_req.RequestId;
                else
                    subject = "New Request approved from Prosol";
                var tbl = new DataTable();
                tbl.Columns.Add("PlantName");
                tbl.Columns.Add("Category");
                tbl.Columns.Add("Group");
                tbl.Columns.Add("UOM");
                tbl.Columns.Add("Legacy");


               
                    var row = tbl.NewRow();
                    row["PlantName"] = pro_req.PlantName;
                    row["Category"] = pro_req.ServiceCategoryName;
                    row["Group"] = pro_req.ServiceGroupName;
                    row["UOM"] = pro_req.UomName;
                    row["Legacy"] = pro_req.Legacy;

                    tbl.Rows.Add(row);



        
                bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));

                if (result == true)
                {
                    return this.Json("Service approved", JsonRequestBehavior.AllowGet);
                }
                else
                    return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {

                pro_req.Rejectedon = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //foreach (TargetExn ent in usrInfo.Roles)
                //{
                //    if (ent.Name == "Approver") tmpStr = ent.TargetId;
                //}
                //pro_req.Cleanser = tmpStr;
                bool result = _ServiceMasterService.submit_servcapproval(pro_req, pub, catname.UserName);
                string to_mail = catname.EmailId;
                string subject = "";
                if (Session["username"].ToString() != null)
                    subject = "Request rejected by " + Session["username"].ToString() + " REQUEST ID : " + pro_req.RequestId;
                else
                    subject = "Request rejected from Prosol";
                var tbl = new DataTable();
                tbl.Columns.Add("PlantName");
                tbl.Columns.Add("Category");
                tbl.Columns.Add("Group");
              
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Reason Rejection");


                var row = tbl.NewRow();
                row["PlantName"] = pro_req.PlantName;
                row["Category"] = pro_req.ServiceCategoryName;
                row["Group"] = pro_req.ServiceGroupName;
            
                row["Legacy"] = pro_req.Legacy;
                row["Reason Rejection"] = pro_req.Reject_reason;
                tbl.Rows.Add(row);




                bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));
                if (result == true)
                {
                    // var user_deteails = _ServiceMasterService.get_req(pro_req.requester).ToList();
                    return this.Json("Service rejected", JsonRequestBehavior.AllowGet);
                }
                else

                    return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getAttribute(string Noun, string Modifier)
        {
            {
                var strloc = _ServiceMasterService.getAttribute(Noun, Modifier).ToList();
                // if(strloc != null)
                if (strloc.Count > 0)
                {
                    var ListViewModelObj = new List<MS_Attribute>();
                    foreach (Prosol_MSAttribute mdl in strloc)
                    {
                        var ViewModelObj = new MS_Attribute();
                        ViewModelObj._id = mdl._id.ToString();
                        ViewModelObj.Attributes = mdl.Attributes;
                        ViewModelObj.Sequence = mdl.Sequence;
                        ViewModelObj.Values = mdl.Values;
                        ViewModelObj.Noun = mdl.Noun;
                        ViewModelObj.Modifier = mdl.Modifier;
                        //ViewModelObj.MainCode = mdl.MainCode;
                        //ViewModelObj.SubCode = mdl.SubCode;
                        //ViewModelObj.MainDiscription = mdl.MainDiscription;
                        //ViewModelObj.SubDiscription = mdl.SubDiscription;
                        //  ViewModelObj.Activity = mdl.Activity;
                        ListViewModelObj.Add(ViewModelObj);
                    }

                    // var strloc1 = _ServiceMasterService.getAttribute().ToList();
                    //if(strloc1!=null && strloc1.Count > 0)
                    // {
                    //     foreach (Prosol_ServiceDefaultAttr  dmdl in strloc1)
                    //     {
                    //         var tmpLst = ListViewModelObj.Where(x => x.Attributes == dmdl.Attributes).ToList();
                    //         if(tmpLst==null || tmpLst.Count == 0)
                    //         {
                    //             var VieObj = new MS_Attribute();
                    //             VieObj.Attributes = dmdl.Attributes;
                    //             ListViewModelObj.Add(VieObj);
                    //         }


                    //     }
                    // }

                    return this.Json(ListViewModelObj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json(null, JsonRequestBehavior.AllowGet);
                }
            //{
            //    var strloc1 = _ServiceMasterService.getAttribute().ToList();
            //    var ListViewModelObj = new List<ServiceDefaultAttr>();
            //    foreach (Prosol_ServiceDefaultAttr mdl in strloc1)
            //    {
            //        var ViewModelObj = new ServiceDefaultAttr();
            //        // ViewModelObj._id = mdl._id.ToString();
            //        ViewModelObj.Attributes = mdl.Attributes;

            //        ListViewModelObj.Add(ViewModelObj);
            //    }
            //    return this.Json(ListViewModelObj, JsonRequestBehavior.AllowGet);
            //}
        }

    }

        public JsonResult AutoCompleteNoun(string term)
        {
            var arrStr = _ServiceMasterService.AutoSearchNoun(term);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetNounDetail(string Noun)
        {

            var arrStr = _ServiceMasterService.GetNounDetail(Noun);
            var NM = new MS_Attribute();
            NM._id = arrStr._id.ToString();
            NM.Noun = arrStr.Noun;
            //     NM.Nounabv = arrStr.Nounabv;
            //   NM.NounDefinition = arrStr.NounDefinition;
            return this.Json(NM, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetModifier(string Noun)
        {
            var arrStr = _ServiceMasterService.GetModifierList(Noun);
            var result = arrStr.Select(i => new { i.Modifier }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}






