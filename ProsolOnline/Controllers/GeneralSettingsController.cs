using Newtonsoft.Json;
using ProsolOnline.Models;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Prosol.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace ProsolOnline.Controllers
{
    public class GeneralSettingsController : Controller
    {

        private readonly IGeneralSettings _GeneralSettingService;       
        public GeneralSettingsController(IGeneralSettings UOMservice)
        {
            _GeneralSettingService = UOMservice;
           
        }
        // GET: GeneralSettings
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("General Settings") == 1)
                return View();
            else if (CheckAccess("General Settings") == 0)
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
        public ActionResult Vendor()
        {

            if (CheckAccess("Vendor Details") == 1)
                return View();
            else if (CheckAccess("Vendor Details") == 0)
                return View("Denied");
            else return View("Login");           

        }
        [Authorize]
        public ActionResult Vendor1()
        {

            if (CheckAccess("Vendor Details") == 1)
                return View();
            else if (CheckAccess("Vendor Details") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Logics()
        {
            if (CheckAccess("Add Logics") == 1)
                return View();
            else if (CheckAccess("Add Logics") == 0)
                return View("Denied");
            else return View("Login");            

        }

        [Authorize]
        public ActionResult Bigdata()
        {
            if (CheckAccess("General Settings") == 1)
                return View();
            else if (CheckAccess("General Settings") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult AssetBigdata()
        {
            if (CheckAccess("General Settings") == 1)
                return View();
            else if (CheckAccess("General Settings") == 0)
                return View("Denied");
            else return View("Login");

        }

        [Authorize]
        public ActionResult UNSPSC()
        {
            if (CheckAccess("UNSPSC Master") == 1)
                return View();
            else if (CheckAccess("UNSPSC Master") == 0)
                return View("Denied");
            else return View("Login");         

        }
        //Group codes
        [Authorize]
        [HttpPost]
        public JsonResult InsertGroupcode()
        {
            var grp = Request.Form["grp"];
            var Model = JsonConvert.DeserializeObject<GroupcodeModel>(grp);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new GroupcodeModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var grpcde = new Prosol_GroupCodes();
                    grpcde._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    grpcde.code = Model.code;
                    grpcde.title = Model.title;

                    res = _GeneralSettingService.CreateGroupcode(grpcde);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]

        public JsonResult GetGroupcodeList1(int currentPage, int maxRows)
        {
            var groupList = _GeneralSettingService.GetGroupcodeList();

            var lst = new List<GroupcodeModel>();
            PaingGroup pageList = new PaingGroup();
            pageList.totItem = groupList.ToList().Count;
            var lstTmp = (from prsl in groupList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_GroupCodes mdl in lstTmp)
            {
                var grp = new GroupcodeModel();
                grp._id = mdl._id.ToString();
                grp.code = mdl.code;
                grp.title = mdl.title;
                //UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.GroupList = lst;
            double pageCount = (double)((decimal)groupList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        

        public JsonResult GetGroupListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var groupList = _GeneralSettingService.GetGroupcodeList(srchtxt);

            var lst = new List<GroupcodeModel>();
            PaingGroup pageList = new PaingGroup();
            pageList.totItem = groupList.ToList().Count;
            var lstTmp = (from prsl in groupList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_GroupCodes mdl in lstTmp)
            {
                var grp = new GroupcodeModel();
                grp._id = mdl._id.ToString();
                grp.code = mdl.code;
                grp.title = mdl.title;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.GroupList = lst;
            double pageCount = (double)((decimal)groupList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetGroupcodeList1()
        //{

        //    var grpList = _GeneralSettingService.GetGroupcodeList();
        //    var lst = new List<GroupcodeModel>();
        //    foreach (Prosol_GroupCodes mdl in grpList)
        //    {
        //        var grp = new GroupcodeModel();
        //        grp._id = mdl._id.ToString();
        //        grp.code = mdl.code;
        //        grp.title = mdl.title;
        //        lst.Add(grp);
        //    }
        //    return this.Json(lst, JsonRequestBehavior.AllowGet);

        //}



        public JsonResult GetGroupcodeList()
        {

            var grpList = _GeneralSettingService.GetGroupcodeList();
            var lst = new List<GroupcodeModel>();
            foreach (Prosol_GroupCodes mdl in grpList)
            {
                var grp = new GroupcodeModel();
                grp._id = mdl._id.ToString();
                grp.code = mdl.code;
                grp.title = mdl.title + " ( " + mdl.code + " )";
                lst.Add(grp);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult DelGroupcode(string id)
        {

            var res = _GeneralSettingService.DeleteGroupcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //Sub Group Code
        [HttpPost]
        [Authorize]
        public JsonResult InsertSubGroupcode()
        {
            var grp = Request.Form["subgrp"];
            var Model = JsonConvert.DeserializeObject<SubGroupcodeModel>(grp);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new SubGroupcodeModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var grpcde = new Prosol_SubGroupCodes();
                    grpcde._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    grpcde.code = Model.code;
                    //  var formattedName = Regex.Replace(Model.groupTitle, @"", "");
                    grpcde.title = Model.title;

                    grpcde.groupCode = Model.groupCode;
                    int spot = Model.groupTitle.LastIndexOf('(');
                    // Model.
                    grpcde.groupTitle = Model.groupTitle.Remove(spot - 1, (Model.groupTitle.Count() - spot + 1));//.Substring(0, Model.groupTitle.Length - 7);

                    res = _GeneralSettingService.CreateSubGroupcode(grpcde);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetSubGroupcodeList(int currentPage, int maxRows)
        {
            var subgroupList = _GeneralSettingService.GetSubGroupcodeList();

            var lst = new List<SubGroupcodeModel>();
            PaingsSubGroup pageList = new PaingsSubGroup();
            pageList.totItem = subgroupList.ToList().Count;
            var lstTmp = (from prsl in subgroupList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_SubGroupCodes mdl in lstTmp)
            {
                var grp = new SubGroupcodeModel();
                grp._id = mdl._id.ToString();
                grp.code = mdl.code;
                grp.title = mdl.title;

                grp.groupCode = mdl.groupCode;
                grp.groupTitle = mdl.groupTitle;
                //grp.code = mdl.code;
                //grp.title = mdl.title;
                //UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.SubGroupList = lst;
            double pageCount = (double)((decimal)subgroupList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSubGroupListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var groupList = _GeneralSettingService.GetSubGroupcodeList1(srchtxt);

            var lst = new List<SubGroupcodeModel>();
            PaingsSubGroup pageList = new PaingsSubGroup();
            pageList.totItem = groupList.ToList().Count;
            var lstTmp = (from prsl in groupList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_SubGroupCodes mdl in lstTmp)
            {
                var grp = new SubGroupcodeModel();
                grp._id = mdl._id.ToString();
                grp.code = mdl.code;
                grp.title = mdl.title;

                grp.groupCode = mdl.groupCode;
                grp.groupTitle = mdl.groupTitle;
                // grp.code = mdl.code;
                //grp.title = mdl.title;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.SubGroupList = lst;
            double pageCount = (double)((decimal)groupList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetSubGroupcodeList()
        //{

        //    var grpList = _GeneralSettingService.GetSubGroupcodeList();
        //    var lst = new List<SubGroupcodeModel>();
        //    foreach (Prosol_SubGroupCodes mdl in grpList)
        //    {
        //        var grp = new SubGroupcodeModel();
        //        grp._id = mdl._id.ToString();
        //        grp.code = mdl.code;
        //        grp.title = mdl.title;

        //        grp.groupCode = mdl.groupCode;
        //        grp.groupTitle = mdl.groupTitle;

        //        lst.Add(grp);
        //    }
        //    return this.Json(lst, JsonRequestBehavior.AllowGet);

        //}
        [Authorize]

        public JsonResult DelSubGroupcode(string id)
        {

            var res = _GeneralSettingService.DeleteSubGroupcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        //sub sub group codes

        public JsonResult GetSubGroupcodeList1(string MainGroupCode)
        {

            var grpList = _GeneralSettingService.GetSubGroupcodeList(MainGroupCode);
            var lst = new List<SubGroupcodeModel>();
            foreach (Prosol_SubGroupCodes mdl in grpList)
            {
                var grp = new SubGroupcodeModel();
                grp._id = mdl._id.ToString();
                grp.code = mdl.code;
                grp.title = mdl.title + " ( " + mdl.code + " )"; ;

                grp.groupCode = mdl.groupCode;
                grp.groupTitle = mdl.groupTitle;///// + " ( " + mdl.groupCode + " )";

                lst.Add(grp);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSubsubGroupcodeList(string SubGroupCode)
        {

            var grpList = _GeneralSettingService.GetSubsubGroupcodeList(SubGroupCode);
            var lst = new List<SubSubGroupCodeModel>();
            foreach (Prosol_SubSubGroupCode mdl in grpList)
            {
                var grp = new SubSubGroupCodeModel();
                grp._id = mdl._id.ToString();
                grp.SubSubGroupCode = mdl.SubSubGroupCode;
                grp.SubSubGroupTitle = mdl.SubSubGroupTitle + " ( " + mdl.SubSubGroupCode + " )"; 
                lst.Add(grp);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public bool InsertSubSubgroup()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            SubSubGroupCodeModel Model = JsonConvert.DeserializeObject<SubSubGroupCodeModel>(obj);
            Prosol_SubSubGroupCode mdl = new Prosol_SubSubGroupCode();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            mdl.SubSubGroupCode = Model.SubSubGroupCode;
            mdl.SubSubGroupTitle = Model.SubSubGroupTitle;
            mdl.MainGroupCode = Model.MainGroupCode;
            int spot = Model.MainGroupTitle.LastIndexOf('(');
            // Model.
            mdl.MainGroupTitle = Model.MainGroupTitle.Remove(spot - 1, (Model.MainGroupTitle.Count() - spot + 1));
            //mdl.MainGroupTitle = Model.MainGroupTitle;
            mdl.SubGroupCode = Model.SubGroupCode.ToUpper();
            int spot1 = Model.SubGroupTitle.LastIndexOf('(');
            // Model.
            mdl.SubGroupTitle = Model.SubGroupTitle.Remove(spot1 - 1, (Model.SubGroupTitle.Count() - spot1 + 1));
            // mdl.SubGroupTitle = Model.SubGroupTitle.ToUpper();
            // mdl.Islive = true;
            var getresult = _GeneralSettingService.InsertSubSubgroup(mdl, update);
            return getresult;

        }
        public JsonResult ListofSubSubUser(int currentPage, int maxRows)
        {
            var subsubgroupList = _GeneralSettingService.ListofSubSubUser();

            var lst = new List<SubSubGroupCodeModel>();
            PaingsSubSubGroup pageList = new PaingsSubSubGroup();
            pageList.totItem = subsubgroupList.ToList().Count;
            var lstTmp = (from prsl in subsubgroupList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_SubSubGroupCode mdl in lstTmp)
            {
                var grp = new SubSubGroupCodeModel();
                grp._id = mdl._id.ToString();
                grp.MainGroupTitle = mdl.MainGroupTitle;
                grp.SubGroupTitle = mdl.SubGroupTitle;
                grp.MainGroupCode = mdl.MainGroupCode;
                grp.SubGroupCode = mdl.SubGroupCode;
                grp.SubSubGroupCode = mdl.SubSubGroupCode;
                grp.SubSubGroupTitle = mdl.SubSubGroupTitle;
                //grp.code = mdl.code;
                //grp.title = mdl.title;

                //grp.groupCode = mdl.groupCode;
                //grp.groupTitle = mdl.groupTitle;
                //grp.code = mdl.code;
                //grp.title = mdl.title;
                //UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.SubSubGroupList = lst;
            double pageCount = (double)((decimal)subsubgroupList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSubSubGroupListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var subsubgroupList = _GeneralSettingService.GetSubSubGroupListSearch(srchtxt);

            var lst = new List<SubSubGroupCodeModel>();
            PaingsSubSubGroup pageList = new PaingsSubSubGroup();
            pageList.totItem = subsubgroupList.ToList().Count;
            var lstTmp = (from prsl in subsubgroupList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_SubSubGroupCode mdl in lstTmp)
            {
                var grp = new SubSubGroupCodeModel();
                grp._id = mdl._id.ToString();
                grp.MainGroupTitle = mdl.MainGroupTitle;
                grp.SubGroupTitle = mdl.SubGroupTitle;
                grp.MainGroupCode = mdl.MainGroupCode;
                grp.SubGroupCode = mdl.SubGroupCode;
                grp.SubSubGroupCode = mdl.SubSubGroupCode;
                grp.SubSubGroupTitle = mdl.SubSubGroupTitle;
                //grp.code = mdl.code;
                //grp.title = mdl.title;

                //grp.groupCode = mdl.groupCode;
                //grp.groupTitle = mdl.groupTitle;
                // grp.code = mdl.code;
                //grp.title = mdl.title;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.SubSubGroupList = lst;
            double pageCount = (double)((decimal)subsubgroupList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult ListofSubSubUser()
        //{
        //    var objList = _GeneralSettingService.ListofSubSubUser();

        //    var lst = new List<SubSubGroupCodeModel>();
        //    foreach (Prosol_SubSubGroupCode mdl in objList)
        //    {
        //        var obj = new SubSubGroupCodeModel();
        //        obj._id = mdl._id.ToString();
        //        obj.MainGroupTitle = mdl.MainGroupTitle;
        //        obj.SubGroupTitle = mdl.SubGroupTitle;
        //        obj.MainGroupCode = mdl.MainGroupCode;
        //        obj.SubGroupCode = mdl.SubGroupCode;
        //        obj.SubSubGroupCode = mdl.SubSubGroupCode;
        //        obj.SubSubGroupTitle = mdl.SubSubGroupTitle;

        //        //obj.Islive = mdl.Islive;
        //        lst.Add(obj);
        //    }
        //    var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;

        //  //  return this.Json(lst, JsonRequestBehavior.AllowGet);

        //}
        public JsonResult DelsubsubGroupcode(string id)
        {

            var res = _GeneralSettingService.DeleteSubsubGroupcode(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        // UOM
        [HttpPost]
        [Authorize]
        public JsonResult InsertUOM()
        {
            //int update = 0;
            //var uom = Request.Form["uom"];
            //UOMModel Model = JsonConvert.DeserializeObject<UOMModel>(uom);

            //Prosol_UOM mdl = new Prosol_UOM();
            //if (Model._id != null && Model._id != "undefined")
            //{
            //    update = 1;
            //    mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            //}
            
            //mdl.UOMname = Model.UOMname;
            //mdl.Unitname = Model.Unitname;
            //mdl.UpdatedOn = DateTime.Now;
            ////mdl.Islive = true;
            //var getresult = _GeneralSettingService.CreateUOM(mdl, update);
            //return this.Json(getresult, JsonRequestBehavior.AllowGet);
            // return getresult;

            var uom = Request.Form["uom"];
            var Model = JsonConvert.DeserializeObject<UOMModel>(uom);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new UOMModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var UOM = new Prosol_UOM();
                    UOM._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    UOM.UOMname = Model.UOMname;
                    UOM.Unitname = Model.Unitname;
                    // UOM.Attribute = Model.Attribute;

                    res = _GeneralSettingService.CreateUOM(UOM);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetUOMList()
        {

            var uomList = _GeneralSettingService.GetUOMList();
            var lst = new List<UOMModel>();
            foreach (Prosol_UOM mdl in uomList)
            {
                var UOM = new UOMModel();
                UOM._id = mdl._id.ToString();
                UOM.UOMname = mdl.UOMname;
                UOM.Unitname = mdl.Unitname;
               // UOM.Attribute = mdl.Attribute;
                lst.Add(UOM);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetUOM(string label)
        {

            var uomList = _GeneralSettingService.GetUOM(label);           
            return this.Json(uomList, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult Deluom(string id)
        {

            var res = _GeneralSettingService.DeleteUOM(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult UOM_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _GeneralSettingService.BulkUOM(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUOMList1(int currentPage, int maxRows)
        {
            var uomList = _GeneralSettingService.GetUOMList();

            var lst = new List<UOMModel>();
            PaingUOM pageList = new PaingUOM();
            pageList.totItem = uomList.ToList().Count;
            var lstTmp = (from prsl in uomList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
           
            foreach (Prosol_UOM mdl in lstTmp)
            {
                var UOM = new UOMModel();
                UOM._id = mdl._id.ToString();
                UOM.UOMname = mdl.UOMname;
                UOM.Unitname = mdl.Unitname;
                //UOM.Attribute = mdl.Attribute;
                lst.Add(UOM);
            }   
            
            pageList.UOMList = lst;
            double pageCount = (double)((decimal)uomList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUOMListSearch(string srchtxt, int currentPage, int maxRows)
        {          
            var uomList = _GeneralSettingService.GetUOMList(srchtxt);

            var lst = new List<UOMModel>();
            PaingUOM pageList = new PaingUOM();
            pageList.totItem = uomList.ToList().Count;
            var lstTmp = (from prsl in uomList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_UOM mdl in lstTmp)
            {
                var UOM = new UOMModel();
                UOM._id = mdl._id.ToString();
                UOM.UOMname = mdl.UOMname;
                UOM.Unitname = mdl.Unitname;
               // UOM.Attribute = mdl.Attribute;
                lst.Add(UOM);
            }

            pageList.UOMList = lst;
            double pageCount = (double)((decimal)uomList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        //uom1
        public JsonResult Insert_UOM1()
        {

            int update = 0;
            var uom = Request.Form["uom"];
            UOM_MODEL Model = JsonConvert.DeserializeObject<UOM_MODEL>(uom);
            Prosol_UOMMODEL mdl = new Prosol_UOMMODEL();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }
            mdl.UOMNAME = Model.UOMNAME.ToUpper();

            //mdl.Islive = true;
            var getresult = _GeneralSettingService.InsertData(mdl, update);
           
            return this.Json(getresult, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getlistuom()
        {
            var objList = _GeneralSettingService.getlistuom();


            var lst = new List<UOM_MODEL>();
            foreach (Prosol_UOMMODEL mdl in objList)
            {
                var obj = new UOM_MODEL();
                obj._id = mdl._id.ToString();
                obj.UOMNAME = mdl.UOMNAME;

                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Deluom1(string id)
        {

            var res = _GeneralSettingService.DeleteUOM1(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetUOMListSearchhhh(string srchtxt, int currentPage, int maxRows)
        //{
        //    var uomList = _GeneralSettingService.GetUOMList1(srchtxt);

        //    var lst = new List<UOM_MODEL>();
        //    PaingUOM1 pageList = new PaingUOM1();
        //    pageList.totItem = uomList.ToList().Count;
        //    var lstTmp = (from prsl in uomList
        //                  select prsl)
        //                .OrderBy(prsl => prsl._id)
        //                .Skip((currentPage - 1) * maxRows)
        //                .Take(maxRows).ToList();

        //    foreach (Prosol_UOMMODEL mdl in lstTmp)
        //    {
        //        var UOM = new UOM_MODEL();
        //        UOM._id = mdl._id.ToString();
        //        UOM.UOMNAME = mdl.UOMNAME;
        //        //UOM.Unitname = mdl.Unitname;
        //       // UOM.Attribute = mdl.Attribute;
        //        lst.Add(UOM);
        //    }

        //    pageList.UOMList = lst;
        //    double pageCount = (double)((decimal)uomList.Count() / Convert.ToDecimal(maxRows));
        //    pageList.PageCount = (int)Math.Ceiling(pageCount);
        //    pageList.CurrentPageIndex = currentPage;
        //    return this.Json(pageList, JsonRequestBehavior.AllowGet);

        //}
        //public JsonResult GetUOMList2(int currentPage, int maxRows)
        //{
        //    var uomList = _GeneralSettingService.GetUOMList1();

        //    var lst = new List<UOM_MODEL>();
        //    PaingUOM1 pageList = new PaingUOM1();
        //    pageList.totItem = uomList.ToList().Count;
        //    var lstTmp = (from prsl in uomList
        //                  select prsl)
        //                .OrderBy(prsl => prsl._id)
        //                .Skip((currentPage - 1) * maxRows)
        //                .Take(maxRows).ToList();

        //    foreach (Prosol_UOMMODEL mdl in lstTmp)
        //    {
        //        var UOM = new UOM_MODEL();
        //        UOM._id = mdl._id.ToString();
        //        UOM.UOMNAME = mdl.UOMNAME;
        //        //UOM.Unitname = mdl.Unitname;
        //        //UOM.Attribute = mdl.Attribute;
        //        lst.Add(UOM);
        //    }

        //    pageList.UOMList = lst;
        //    double pageCount = (double)((decimal)uomList.Count() / Convert.ToDecimal(maxRows));
        //    pageList.PageCount = (int)Math.Ceiling(pageCount);
        //    pageList.CurrentPageIndex = currentPage;
        //    return this.Json(pageList, JsonRequestBehavior.AllowGet);

        //}

        //Vendor

        [HttpPost]
        [Authorize]
        public JsonResult InsertVendor()
        {
            var ven = Request.Form["vendor"];
            var Model = JsonConvert.DeserializeObject<VendorModel>(ven);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new VendorModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var vendor = new Prosol_Vendor();
                    vendor._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    vendor.Code = Model.Code== null ? generatCode(): Model.Code;
                    vendor.ShortDescName = Model.ShortDescName;
                    vendor.Name = Model.Name;
                    vendor.Name2 = Model.Name2;
                    vendor.Name3 = Model.Name3;
                    vendor.Name4 = Model.Name4;
                    vendor.Address = Model.Address;
                    vendor.Address2 = Model.Address2;
                    vendor.Address3 = Model.Address3;
                    vendor.Address4 = Model.Address4;
                    vendor.City = Model.City;
                    vendor.Region = Model.Region;
                    vendor.Country = Model.Country;
                    vendor.Postal = Model.Postal;
                    vendor.Phone = Model.Phone;
                    vendor.Mobile = Model.Mobile;
                    vendor.Fax = Model.Fax;
                    vendor.Email = Model.Email;
                    vendor.Website = Model.Website;
                    vendor.Acquiredby = Model.Acquiredby;
                    vendor.Enabled =true;
                    vendor.AcquiredCompanyName = Model.AcquiredCompanyName;

                    res = _GeneralSettingService.CreateVendor(vendor);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        private string generatCode()
        {
            string rollno = "";
            string str = _GeneralSettingService.getNextCode();
            if (str != "")
            {
                int cde = Convert.ToInt32(str);
                cde++;
                switch (cde.ToString().Length)
                {
                    case 1:
                        rollno = "0000" + cde;
                        break;
                    case 2:
                        rollno = "000" + cde;
                        break;
                    case 3:
                        rollno = "00" + cde;
                        break;
                    case 4:
                        rollno = "0" + cde;
                        break;
                    case 5:
                        rollno =  cde.ToString();
                        break;
                }

                return rollno;
            }
            else return rollno = "00001";           
        }
        [Authorize]
        public JsonResult GetVendorList()
        {

            var uomList = _GeneralSettingService.GetVendorList();
            var lst = new List<VendorModel>();
            foreach (Prosol_Vendor mdl in uomList)
            {
                var vendor = new VendorModel();
                vendor._id = mdl._id.ToString();
                vendor.Code = mdl.Code;
                vendor.ShortDescName = mdl.ShortDescName;
                vendor.Name = mdl.Name;
                vendor.Name2 = mdl.Name2;
                vendor.Name3 = mdl.Name3;
                vendor.Name4 = mdl.Name4;
                vendor.Address = mdl.Address;
                vendor.Address2 = mdl.Address2;
                vendor.Address3 = mdl.Address3;
                vendor.Address4 = mdl.Address4;
                vendor.City = mdl.City;
                vendor.Region = mdl.Region;
                vendor.Country = mdl.Country;
                vendor.Postal = mdl.Postal;
                vendor.Phone = mdl.Phone;
                vendor.Mobile = mdl.Mobile;
                vendor.Fax = mdl.Fax;
                vendor.Email = mdl.Email;
                vendor.Website = mdl.Website;
                vendor.Acquiredby = mdl.Acquiredby;
                vendor.AcquiredCompanyName = mdl.AcquiredCompanyName;
                vendor.Enabled = mdl.Enabled;
                lst.Add(vendor);
            }
            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
           // return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        [Authorize]

        public void DownloadMfr()
        {
            var res = _GeneralSettingService.GetVendorList();
            var strJson = JsonConvert.SerializeObject(res);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "VendorList.xls"));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;
            for (int i = 1; i < dt.Columns.Count - 1; i++)
            {
                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }

            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 1; j < dt.Columns.Count - 1; j++)
                {
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        [Authorize]
        public JsonResult DelVendor(string id)
        {

            var res = _GeneralSettingService.DeleteVendor(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult DisableVendor(string id,bool Enabled)
        {

            var res = _GeneralSettingService.DisableVendor(id,Enabled);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [Authorize]
        public JsonResult Vendor_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _GeneralSettingService.BulkVendor(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVendorList1(int currentPage, int maxRows)
        {

            var uomList = _GeneralSettingService.GetVendorList();
            var lst = new List<VendorModel>();     
            PaingVendor pageList = new PaingVendor();
            pageList.totItem = uomList.ToList().Count;
            var lstTmp = (from prsl in uomList
                                       select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            foreach (Prosol_Vendor mdl in lstTmp)
            {
                var vendor = new VendorModel();
                vendor._id = mdl._id.ToString();
                vendor.Code = mdl.Code;
                vendor.ShortDescName = mdl.ShortDescName;
                vendor.Name = mdl.Name;
                vendor.Name2 = mdl.Name2;
                vendor.Name3 = mdl.Name3;
                vendor.Name4 = mdl.Name4;
                vendor.Address = mdl.Address;
                vendor.Address2 = mdl.Address2;
                vendor.Address3 = mdl.Address3;
                vendor.Address4 = mdl.Address4;
                vendor.City = mdl.City;
                vendor.Region = mdl.Region;
                vendor.Country = mdl.Country;
                vendor.Postal = mdl.Postal;
                vendor.Phone = mdl.Phone;
                vendor.Mobile = mdl.Mobile;
                vendor.Fax = mdl.Fax;
                vendor.Email = mdl.Email;
                vendor.Website = mdl.Website;
                vendor.Acquiredby = mdl.Acquiredby;
                vendor.AcquiredCompanyName = mdl.AcquiredCompanyName;
                vendor.Enabled = mdl.Enabled;
                lst.Add(vendor);
            }
            pageList.VendorsList = lst;
            double pageCount = (double)((decimal)uomList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult GetVendorSearch(string srchtxt,int currentPage, int maxRows)
        {

            var uomList = _GeneralSettingService.GetVendorList(srchtxt);
            var lst = new List<VendorModel>();
            PaingVendor pageList = new PaingVendor();
            pageList.totItem = uomList.ToList().Count;
            var lstTmp = (from prsl in uomList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            foreach (Prosol_Vendor mdl in lstTmp)
            {
                var vendor = new VendorModel();
                vendor._id = mdl._id.ToString();
                vendor.Code = mdl.Code;
                vendor.ShortDescName = mdl.ShortDescName;
                vendor.Name = mdl.Name;
                vendor.Name2 = mdl.Name2;
                vendor.Name3 = mdl.Name3;
                vendor.Name4 = mdl.Name4;
                vendor.Address = mdl.Address;
                vendor.Address2 = mdl.Address2;
                vendor.Address3 = mdl.Address3;
                vendor.Address4 = mdl.Address4;
                vendor.City = mdl.City;
                vendor.Region = mdl.Region;
                vendor.Country = mdl.Country;
                vendor.Postal = mdl.Postal;
                vendor.Phone = mdl.Phone;
                vendor.Mobile = mdl.Mobile;
                vendor.Fax = mdl.Fax;
                vendor.Email = mdl.Email;
                vendor.Website = mdl.Website;
                vendor.Acquiredby = mdl.Acquiredby;
                vendor.AcquiredCompanyName = mdl.AcquiredCompanyName;
                vendor.Enabled = mdl.Enabled;
                lst.Add(vendor);
            }
            pageList.VendorsList = lst;
            double pageCount = (double)((decimal)uomList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult AutoCompleteVendor(string term)
        {

            var uomList = _GeneralSettingService.GetVendorLst(term);
           // var result = uomList.Select(i => new { i.Code, i.Name, i.Name2, i.Name3, i.Name4 }).Distinct().ToList();
            return this.Json(uomList, JsonRequestBehavior.AllowGet);

        }
        //Abbrevation
        [HttpPost]
        [Authorize]
        public JsonResult InsertAbbr()
        {
         
             var abbr = Request.Form["abbr"];
            
            var Model = JsonConvert.DeserializeObject<AbbrevateModel>(abbr);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AbbrevateModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var Abb = new Prosol_Abbrevate();
                    Abb._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    Abb.Value = Model.Value;
                    Abb.Abbrevated = Model.Abbrevated;
                    Abb.Equivalent = Model.Equivalent;
                    Abb.LikelyWords = Model.LikelyWords;
                    Abb.vunit = Model.vunit;
                    Abb.eunit = Model.eunit;
                    Abb.Approved = "Yes";
                    Abb.User = Session["username"].ToString();
                    Abb.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    res = _GeneralSettingService.CreateAbbr(Abb);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult approvevalue()
        {
            bool res = false;
            var Abbrs1 = Request.Form["Abbrs1"];
            var mdl = JsonConvert.DeserializeObject<List<AbbrevateModel>>(Abbrs1).ToList();
            foreach (AbbrevateModel Model in mdl)
            {
                var Abb = new Prosol_Abbrevate();
                Abb._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                Abb.Value = Model.Value;
                Abb.Abbrevated = Model.Abbrevated;
                Abb.Equivalent = Model.Equivalent;
                Abb.LikelyWords = Model.LikelyWords;
                Abb.vunit = Model.vunit;
                Abb.eunit = Model.eunit;
                Abb.Approved = "Yes";
                Abb.ApprovedBy = Session["username"].ToString();
                Abb.ApprovedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


                Abb.UpdatedOn = Model.UpdatedOn;
                res = _GeneralSettingService.CreateAbbr(Abb);

            }

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetAbbrList()
        {

            var abbrList = _GeneralSettingService.GetAbbrList();
            var flist = abbrList.Where(x => x.Approved == "No").ToList();
            //  var valuelist = _GeneralSettingService.getvaluelist();

            var lst = new List<AbbrevateModel>();
            foreach (Prosol_Abbrevate mdl in flist)
            {
                //int gan = 0;
                var abv = new AbbrevateModel();
                //foreach (Prosol_Charateristics val in valuelist)
                //{
                //    if (val.Values != null && val.Values.Contains(mdl._id.ToString()))
                //    {
                //        abv.idvalue = "Yes";
                //        gan = 1;
                //        break;

                //    }

                //}
                //if (gan == 0)
                //    abv.idvalue = "No";
                abv._id = mdl._id.ToString();
                abv.Value = mdl.Value;
                abv.Abbrevated = mdl.Abbrevated;
                abv.Equivalent = mdl.Equivalent;
                abv.LikelyWords = mdl.LikelyWords;
                abv.vunit = mdl.vunit;
                abv.eunit = mdl.eunit;
                abv.User = mdl.User;
                abv.Approved = mdl.Approved;
                lst.Add(abv);


            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult DelAbbr(string id,string val)
        {

            var res = _GeneralSettingService.DeleteAbbr(id,val);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult unAbbrDel(string id)
        {

            var res = _GeneralSettingService.unAbbrDel(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        
        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult Abbri_Upload()
        {
            int res = 0;
            string User = Session["username"].ToString();
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _GeneralSettingService.BulkAbbri(file,User);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAbbrList1(int currentPage, int maxRows)
        {
            var AbbrList = _GeneralSettingService.GetAbbrList();

            var lst = new List<AbbrevateModel>();
            PaingAbbr pageList = new PaingAbbr();
            pageList.totItem = AbbrList.ToList().Count;
            var lstTmp = (from prsl in AbbrList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_Abbrevate mdl in lstTmp)
            {
                if (mdl.Approved == "Yes")
                {
                    var abv = new AbbrevateModel();
                    abv._id = mdl._id.ToString();
                    abv.Value = mdl.Value;
                    abv.Abbrevated = mdl.Abbrevated;
                    abv.Equivalent = mdl.Equivalent;
                    abv.LikelyWords = mdl.LikelyWords;
                    abv.vunit = mdl.vunit;
                    abv.eunit = mdl.eunit;
                    abv.User = mdl.User;
                    lst.Add(abv);
                }
            }

            pageList.AbbrList = lst;
            double pageCount = (double)((decimal)AbbrList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAbbrListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var AbbrList = _GeneralSettingService.GetAbbrList(srchtxt);

            var lst = new List<AbbrevateModel>();
            PaingAbbr pageList = new PaingAbbr();
            pageList.totItem = AbbrList.ToList().Count;
            var lstTmp = (from prsl in AbbrList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_Abbrevate mdl in lstTmp)
            {

                //if (mdl.Approved == "Yes")
                //{
                    var abv = new AbbrevateModel();
                    abv._id = mdl._id.ToString();
                    abv.Value = mdl.Value;
                    abv.Abbrevated = mdl.Abbrevated;
                    abv.Equivalent = mdl.Equivalent;
                    abv.LikelyWords = mdl.LikelyWords;
                    abv.vunit = mdl.vunit;
                    abv.eunit = mdl.eunit;
                    abv.User = mdl.User;
                    lst.Add(abv);
                //}
            }

            pageList.AbbrList = lst;
            double pageCount = (double)((decimal)AbbrList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }


        public void DownloadValuemaster(string FrmDte, string ToDte)
        {
            var res = _GeneralSettingService.DownloadValuemaster(FrmDte, ToDte);

            var strJson = JsonConvert.SerializeObject(res);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            dt.Columns.RemoveAt(0);
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "tab1");


            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"ValueMaster.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {

                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ValueMaster.xls"));
            //Response.ContentType = "application/ms-excel";
            //string str = string.Empty;
            //for (int i = 1; i < dt.Columns.Count - 1; i++)
            //{
            //    Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            //}

            //Response.Write("\n");
            //foreach (DataRow dr in dt.Rows)
            //{
            //    str = "";
            //    for (int j = 1; j < dt.Columns.Count - 1; j++)
            //    {
            //        Response.Write(str + dr[j]);
            //        str = "\t";
            //    }
            //    Response.Write("\n");
            //}
            //Response.End();
        }
        //Vendor type
        [HttpPost]
        [Authorize]
        public JsonResult InsertVendortype()
        {
            var vendors = Request.Form["Vendors"];
            var Model = JsonConvert.DeserializeObject<VendortypeModel>(vendors);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new VendortypeModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var vendor = new Prosol_Vendortype();
                    vendor._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    vendor.Code = Model.Code;
                    vendor.Type = Model.Type;

                    res = _GeneralSettingService.CreateVendortype(vendor);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetVendortypeList()
        {

            var VendorList = _GeneralSettingService.GetVendortypeList();
            var lst = new List<VendortypeModel>();
            foreach (Prosol_Vendortype mdl in VendorList)
            {
                var vendor = new VendortypeModel();
                vendor._id = mdl._id.ToString();
                vendor.Code = mdl.Code;
                vendor.Type = mdl.Type;
                lst.Add(vendor);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult DelVendortype(string id)
        {

            var res = _GeneralSettingService.DeleteVendortype(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
       

        //Reference type
        [HttpPost]
        [Authorize]
        public JsonResult InsertReftype()
        {
            var Refs = Request.Form["Refs"];
            var Model = JsonConvert.DeserializeObject<ReftypeModel>(Refs);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new ReftypeModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var Ref = new Prosol_Reftype();
                    Ref._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    Ref.Code = Model.Code;
                    Ref.Type = Model.Type;

                    res = _GeneralSettingService.CreateReftype(Ref);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetReftypeList()
        {

            var refList = _GeneralSettingService.GetReftypeList();
            var lst = new List<ReftypeModel>();
            foreach (Prosol_Reftype mdl in refList)
            {
                var refs = new ReftypeModel();
                refs._id = mdl._id.ToString();
                refs.Code = mdl.Code;
                refs.Type = mdl.Type;
                refs.Islive = mdl.Islive;
                lst.Add(refs);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult DelReftype(string id)
        {

            var res = _GeneralSettingService.DeleteReftype(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }


        //Logics
        [Authorize]
        public JsonResult GetAttributesList()
        {

            var res = _GeneralSettingService.GetAttributesList();         
            var AttributesList = res.Select(i => new { i.Characteristic}).Distinct();
            return this.Json(AttributesList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [Authorize]
        public JsonResult addLogic()
        {
            var logic = Request.Form["Logic"];
            var Model = JsonConvert.DeserializeObject<LogicsModel>(logic);
            var attList = Request.Form["Attributes"];
            var ListCharas = JsonConvert.DeserializeObject<List<ValuesList>>(attList);

            bool res = false;
            try
            {
                //if (Model == null)
                //    Model = new LogicsModel();
                //TryUpdateModel(Model);
                //foreach (ValuesList chMdel in ListCharas)
                //{
                //    TryUpdateModel(chMdel);
                //}
                //if (ModelState.IsValid)
                //{  
                var Abb = new Prosol_Logics();
                Abb._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                Abb.Noun = Model.Noun;
                Abb.Modifier = Model.Modifier;
                Abb.Generalterm = Model.Generalterm;
                Abb.Partno = Model.Partno;
                Abb.Refno = Model.Refno;
                Abb.Manufacturer = Model.Manufacturer;

                var lstCharateristics = new List<ValueList>();
                foreach (ValuesList LstAtt in ListCharas)
                {
                    var AttrMdl = new ValueList();
                    AttrMdl.slno = LstAtt.slno;
                    AttrMdl.AttributeName = LstAtt.AttributeName;
                    AttrMdl.Values = LstAtt.Values;
                    lstCharateristics.Add(AttrMdl);

                }
                Abb.Attributes = lstCharateristics;

                res = _GeneralSettingService.CreateLogics(Abb);
                //}
                //else
                //{

                //    return Json(new
                //    {
                //        success = false,
                //        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                //                 .Select(m => m.ErrorMessage).ToArray()
                //    }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetLogic(string Noun, string Modifier)
        {
            var logicList = _GeneralSettingService.GetLogic(Noun, Modifier);
           
            if (logicList != null)
            {
                var Abb = new LogicsModel();
                Abb._id = logicList._id.ToString();
                Abb.Noun = logicList.Noun;
                Abb.Modifier = logicList.Modifier;
                Abb.Generalterm = logicList.Generalterm;
                Abb.Partno = logicList.Partno;
                Abb.Refno = logicList.Refno;
                Abb.Manufacturer = logicList.Manufacturer;
                var lstCharateristics = new List<ValuesList>();
                foreach (ValueList LstAtt in logicList.Attributes)
                {
                    var AttrMdl = new ValuesList();
                    AttrMdl.slno = LstAtt.slno;
                    AttrMdl.AttributeName = LstAtt.AttributeName;
                    AttrMdl.Values = LstAtt.Values;
                    lstCharateristics.Add(AttrMdl);

                }
                Abb.Attributes = lstCharateristics;
                return this.Json(Abb, JsonRequestBehavior.AllowGet);
            }
            else return this.Json(logicList, JsonRequestBehavior.AllowGet);



        }

        //UNSPSC
        [Authorize]
        public JsonResult UNSPSC_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _GeneralSettingService.BulkUNSPSC(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetUnspsc(string Noun,string Modifier)
        {
            var refList = _GeneralSettingService.GetUnspsc(Noun, Modifier);
            var lst = new List<UNSPSCModel>();
            if(refList != null)
            foreach (Prosol_UNSPSC mdl in refList)
            {
                var unspsc = new UNSPSCModel();
                unspsc._id = mdl._id.ToString();
                unspsc.Noun = mdl.Noun;
                unspsc.Modifier = mdl.Modifier;
                unspsc.value = mdl.value;
                unspsc.Segment = mdl.Segment;
                unspsc.SegmentTitle = mdl.SegmentTitle;
                unspsc.Family = mdl.Family;
                unspsc.FamilyTitle = mdl.FamilyTitle;
                unspsc.Class = mdl.Class;
                unspsc.ClassTitle = mdl.ClassTitle;
                unspsc.Commodity = mdl.Commodity;
                unspsc.CommodityTitle = mdl.CommodityTitle;
                lst.Add(unspsc);
            }
            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult GetUnspsc1()
        {
            var refList = _GeneralSettingService.GetUnspsc();
            var lst = new List<UNSPSCModel>();
            foreach (Prosol_UNSPSC mdl in refList)
            {
                var unspsc = new UNSPSCModel();
                unspsc._id = mdl._id.ToString();
                //unspsc.Noun = mdl.Noun;
                //unspsc.Modifier = mdl.Modifier;
                //unspsc.value = mdl.value;
                unspsc.Segment = mdl.Segment;
                unspsc.SegmentTitle = mdl.SegmentTitle;
                unspsc.Family = mdl.Family;
                unspsc.FamilyTitle = mdl.FamilyTitle;
                unspsc.Class = mdl.Class;
                unspsc.ClassTitle = mdl.ClassTitle;
                unspsc.Commodity = mdl.Commodity;
                unspsc.CommodityTitle = mdl.CommodityTitle;
                lst.Add(unspsc);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetUnspscc(int currentPage, int maxRows)
        {
            var unspscList = _GeneralSettingService.GetUnspscc();

            var lst = new List<UNSPSCModel>();
            PaingsUNSPSC pageList = new PaingsUNSPSC();
            pageList.totItem = unspscList.ToList().Count;
            var lstTmp = (from prsl in unspscList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_UNSPSC mdl in lstTmp)
            {
                var grp = new UNSPSCModel();
                grp._id = mdl._id.ToString();

                grp.Noun = mdl.Noun;
                grp.Modifier = mdl.Modifier;
                grp.Segment = mdl.Segment;
                grp.value = mdl.value;
                grp.SegmentTitle = mdl.SegmentTitle;
                grp.Family = mdl.Family;
                grp.FamilyTitle = mdl.FamilyTitle;
                grp.Class = mdl.Class;
                grp.ClassTitle = mdl.ClassTitle;
                grp.Commodity = mdl.Commodity;
                grp.CommodityTitle = mdl.CommodityTitle;
                grp.Version = mdl.Version;
                //grp.code = mdl.code;
                //grp.title = mdl.title;

                //grp.groupCode = mdl.groupCode;
                //grp.groupTitle = mdl.groupTitle;
                //grp.code = mdl.code;
                //grp.title = mdl.title;
                //UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.UNSPSCList = lst;
            double pageCount = (double)((decimal)unspscList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetUNSPSCListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var unspscList = _GeneralSettingService.GetUNSPSCListSearch(srchtxt);

            var lst = new List<UNSPSCModel>();
            PaingsUNSPSC pageList = new PaingsUNSPSC();
            pageList.totItem = unspscList.ToList().Count;
            var lstTmp = (from prsl in unspscList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_UNSPSC mdl in lstTmp)
            {
                var grp = new UNSPSCModel();
                grp._id = mdl._id.ToString();
                grp.Noun = mdl.Noun;
                grp.Modifier = mdl.Modifier;
                grp.value = mdl.value;
                grp.Segment = mdl.Segment;
                grp.SegmentTitle = mdl.SegmentTitle;
                grp.Family = mdl.Family;
                grp.FamilyTitle = mdl.FamilyTitle;
                grp.Class = mdl.Class;
                grp.ClassTitle = mdl.ClassTitle;
                grp.Commodity = mdl.Commodity;
                grp.CommodityTitle = mdl.CommodityTitle;
                grp.Version = mdl.Version;
                //grp.code = mdl.code;
                //grp.title = mdl.title;

                //grp.groupCode = mdl.groupCode;
                //grp.groupTitle = mdl.groupTitle;
                // grp.code = mdl.code;
                //grp.title = mdl.title;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(grp);
            }

            pageList.UNSPSCList = lst;
            double pageCount = (double)((decimal)unspscList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetUnspscc()
        //{
        //    var refList = _GeneralSettingService.GetUnspscc();
        //    var lst = new List<UNSPSCModel>();
        //    foreach (Prosol_UNSPSC mdl in refList)
        //    {
        //        var unspsc = new UNSPSCModel();
        //        unspsc._id = mdl._id.ToString();
        //        unspsc.Noun = mdl.Noun;
        //        unspsc.Modifier = mdl.Modifier;
        //        unspsc.Segment = mdl.Segment;
        //        unspsc.SegmentTitle = mdl.SegmentTitle;
        //        unspsc.Family = mdl.Family;
        //        unspsc.FamilyTitle = mdl.FamilyTitle;
        //        unspsc.Class = mdl.Class;
        //        unspsc.ClassTitle = mdl.ClassTitle;
        //        unspsc.Commodity = mdl.Commodity;
        //        unspsc.CommodityTitle = mdl.CommodityTitle;
        //        unspsc.Version = mdl.Version;
        //        lst.Add(unspsc);
        //    }
        //    return this.Json(lst, JsonRequestBehavior.AllowGet);

        //}

        [Authorize]
        public JsonResult Delunspsc(string id)
        {

            var res = _GeneralSettingService.Delunspsc(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //HSN
        [HttpPost]

        [Authorize]
        public JsonResult HSN_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _GeneralSettingService.BulkHSN(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetHsn(string Noun, string Modifier)
        {
            var hsn = _GeneralSettingService.GetHsn(Noun, Modifier);


            return this.Json(hsn, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetHSNListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var hsnList = _GeneralSettingService.GetHSNList(srchtxt);

            var lst = new List<HSNModel>();
            PaingHSN pageList = new PaingHSN();
            pageList.totItem = hsnList.ToList().Count;
            var lstTmp = (from prsl in hsnList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_HSNModel mdl in lstTmp)
            {
                var HSN = new HSNModel();
                HSN._id = mdl._id.ToString();
                HSN.Noun = mdl.Noun;
                HSN.Modifier = mdl.Modifier;
                HSN.HSNID = mdl.HSNID;
                HSN.Desc = mdl.Desc;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(HSN);
            }

            pageList.HSNList = lst;
            double pageCount = (double)((decimal)hsnList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetHSNList1(int currentPage, int maxRows)
        {
            var hsnList = _GeneralSettingService.GetHSNList();

            var lst = new List<HSNModel>();
            PaingHSN pageList = new PaingHSN();
            pageList.totItem = hsnList.ToList().Count;
            var lstTmp = (from prsl in hsnList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_HSNModel mdl in lstTmp)
            {
                var HSN = new HSNModel();
                HSN._id = mdl._id.ToString();
                HSN.Noun = mdl.Noun;
                HSN.Modifier = mdl.Modifier;
                HSN.HSNID = mdl.HSNID;
                HSN.Desc = mdl.Desc;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(HSN);
            }

            pageList.HSNList = lst;
            double pageCount = (double)((decimal)hsnList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult Delhsn(string id)
        {

            var res = _GeneralSettingService.DeleteHSN(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [Authorize]
        public JsonResult InsertHSN()
        {

            var HSN = Request.Form["HSN"];
            var Model = JsonConvert.DeserializeObject<HSNModel>(HSN);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new HSNModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {
                    var hsn = new Prosol_HSNModel();
                    hsn._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    hsn.HSNID = Model.HSNID;
                    hsn.Desc = Model.Desc;


                    res = _GeneralSettingService.CreateHSN(hsn);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Inserthsn(string Noun, string Modifier, string HSNID, string Desc)
        {

            var res = _GeneralSettingService.CreateHSN1(Noun, Modifier, HSNID, Desc);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }





        //Bulk Data upload

        [Authorize]
        public JsonResult Bulkdata_Upload()
        {
            //try
            //{
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _GeneralSettingService.BulkData(file);
                    }
                }

                if (res.Contains("successfully"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            //}
        } 


        //Bulk PV Data upload

        [Authorize]
        public JsonResult BulkPV_Upload()
        {
            //try
            //{
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _GeneralSettingService.BulkPVData(file);
                    }
                }

                if (res.Contains("successfully"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            //}
        }
        [Authorize]
        public JsonResult BulkCat_Upload()
        {
            //try
            //{
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _GeneralSettingService.BulkCatData(file);
                    }
                }

                if (res.Contains("successfully"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            //}
        }
        [Authorize]
        public JsonResult BulkQc_Upload()
        {
            //try
            //{
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _GeneralSettingService.BulkQcData(file);
                    }
                }

                if (res.Contains("successfully"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            //}
        }


        //[Authorize]
        //public JsonResult Bulkdata_Upload()
        //{
        //    int res = 0;
        //    var file = Request.Files.Count > 0 ? Request.Files[0] : null;
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
        //        {
        //            res = _GeneralSettingService.BulkData(file);
        //        }
        //    }
        //    return this.Json(res, JsonRequestBehavior.AllowGet);
        //}
        [Authorize]
        public void BulkAttribute()
        {
         
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    var res = _GeneralSettingService.BulkAttribute(file);

                

                    DataTable dt = new DataTable();

                    foreach (IDictionary<string, object> row in res)
                    {
                        foreach (KeyValuePair<string, object> entry in row)
                        {
                            if (!dt.Columns.Contains(entry.Key.ToString()))
                            {
                                dt.Columns.Add(entry.Key);
                            }
                        }
                        dt.Rows.Add(row.Values.ToArray());
                    }


                    ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                    wbook.Worksheets.Add(dt, "tab1");
                    // Prepare the response
                    HttpResponseBase httpResponse = Response;
                    httpResponse.Clear();
                    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Provide you file name here
                    httpResponse.AddHeader("content-disposition", "attachment;filename=\"Export.xlsx\"");

                    // Flush the workbook to the Response.OutputStream
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wbook.SaveAs(memoryStream);
                        memoryStream.WriteTo(httpResponse.OutputStream);
                        memoryStream.Close();
                    }

                    httpResponse.End();
                }
            }
           // return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public JsonResult Getunit()
        {
            var res = _GeneralSettingService.Getunit();
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getVendorAbbrForShortDesc(string mfr)
        {
            var result = _GeneralSettingService.getVendorAbbrForShortDesc(mfr);
            if (result != null)
                return this.Json(result, JsonRequestBehavior.AllowGet);
            else
                return this.Json(false, JsonRequestBehavior.AllowGet);
        }

        //NEW CODE FOR VENDOR MASTER
        public JsonResult FINDVENDORMASTER(string mfr)
        {
            var result = _GeneralSettingService.FINDVENDORMASTER(mfr);
            if (result != null)
                return this.Json(result, JsonRequestBehavior.AllowGet);
            else
                return this.Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Duplicatecheck(string id, bool Islive)
        {

            var res = _GeneralSettingService.Duplicatecheck(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        [Authorize]

        public JsonResult Rework()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _GeneralSettingService.BulkRework(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

    }


    // 

    public class PaingVendor
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<VendorModel> VendorsList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingUOM
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<UOMModel> UOMList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingGroup
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<GroupcodeModel> GroupList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingsSubGroup
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<SubGroupcodeModel> SubGroupList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingsSubSubGroup
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<SubSubGroupCodeModel> SubSubGroupList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingsUNSPSC
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<UNSPSCModel> UNSPSCList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingHSN
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<HSNModel> HSNList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
    public class PaingAbbr
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<AbbrevateModel> AbbrList { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }
        public int totItem { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }

 
}