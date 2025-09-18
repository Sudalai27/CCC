using Prosol.Common;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProsolOnline.Controllers
{
    public class MasterController : Controller
    {
        // GET: Salesandothers
        private readonly IMaster _masterService;
        private readonly IUserAccess _useraccessservice;

        public MasterController(IMaster masterService, IUserAccess Accessservice)
        {
            _masterService = masterService;
            _useraccessservice = Accessservice;
        }
        public ActionResult General()
        {
            if (CheckAccess("General") == 1)
                return View();
            else if (CheckAccess("General") == 0)
                return View("Denied");
            else return View("Login");           
        }
        //public ActionResult Index()
        //{
        //    if (Session["userid"] == null)
        //        return View("Login");
        //    else
        //    {
        //        if (_useraccessservice.pagelimit(Convert.ToString(Session["userid"]), "Sales & Others"))
        //            return View();
        //        else
        //            return View("Denied");
        //    }
        //}
        public ActionResult Plant()
        {
            if (CheckAccess("Plant") == 1)
                return View();
            else if (CheckAccess("Plant") == 0)
                return View("Denied");
            else return View("Login");
           
        }
        public ActionResult Sales()
        {
            if (CheckAccess("Sales & Others") == 1)
                return View();
            else if (CheckAccess("Sales & Others") == 0)
                return View("Denied");
            else return View("Login");           
        }
        public ActionResult MRP()
        {
            if (CheckAccess("Mrp data") == 1)
                return View();
            else if (CheckAccess("Mrp data") == 0)
                return View("Denied");
            else return View("Login");          
        }

        private int CheckAccess(string pageName)
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

        [HttpPost]
        public JsonResult InsertData()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterModel>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new MasterModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Master();
                    mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Label = Model.Label;
                    mdl.Code = Model.Code;
                    mdl.Title = Model.Title;
                    mdl.Plantcode = Model.Plantcode;
                    mdl.Storagelocationcode = Model.Storagelocationcode;
                    res = _masterService.InsertData(mdl);
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
        public JsonResult GetDataList(string label)
        {
            var objList = _masterService.GetDataList(label);
            var lst = new List<MasterModel>();
            foreach (Prosol_Master mdl in objList)
            {
                var obj = new MasterModel();
                obj._id = mdl._id.ToString();
                obj.Label = mdl.Label;
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Plantcode = mdl.Plantcode;              
                obj.Storagelocationcode = mdl.Storagelocationcode;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        //sb
        public JsonResult getbincode(string label, string StorageLocation)
        {

            var grpList = _masterService.getbincode(label, StorageLocation);
            var lst = new List<MasterModel>();
            foreach (Prosol_Master mdl in grpList)
            {
                var obj = new MasterModel();
                obj._id = mdl._id.ToString();
                obj.Label = mdl.Label;
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Plantcode = mdl.Plantcode;
                obj.Storagelocationcode = mdl.Storagelocationcode;
                obj.Islive = mdl.Islive;
                lst.Add(obj);


            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetMaster()
        {
            var objList = _masterService.GetMaster();
          //  var objList = _masterService.GetMaster().ToList();
            var lst = new List<MasterModel>();
            // if (objList != null && objList.Count>0)
            if (objList != null)
            {
                foreach (Prosol_Master mdl in objList)
                {
                    var obj = new MasterModel();
                    obj._id = mdl._id.ToString();
                    obj.Label = mdl.Label;
                    obj.Code = mdl.Code;
                    obj.Title = mdl.Title;
                    obj.Plantcode = mdl.Plantcode;
                    obj.Storagelocationcode = mdl.Storagelocationcode;

                    lst.Add(obj);
                }
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelData(string id)
        {

            var res = _masterService.DelData(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableData(string id, bool Islive)
        {

            var res = _masterService.DisableData(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //Plant
        [HttpPost]
        public JsonResult InsertDataplnt()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<PlantModel>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new PlantModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Plants();
                    mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Plantname = Model.Plantname;
                    mdl.Plantcode = Model.Plantcode;


                    res = _masterService.InsertDataplnt(mdl);
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
        public JsonResult GetDataListplnt()
        {
            var objList = _masterService.GetDataListplnt();
            var lst = new List<PlantModel>();
            foreach (Prosol_Plants mdl in objList)
            {
                var obj = new PlantModel();
                obj._id = mdl._id.ToString();
                obj.Plantcode = mdl.Plantcode;
                obj.Plantname = mdl.Plantname;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetMasterplant()
        {
            var objList = _masterService.GetMasterplnt();
            var lst = new List<PlantModel>();
            foreach (Prosol_Plants mdl in objList)
            {
                var obj = new PlantModel();
                obj._id = mdl._id.ToString();
                obj.Plantname = mdl.Plantname;
                obj.Plantcode = mdl.Plantcode;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelDataplnt(string id)
        {

            var res = _masterService.DelDataplnt(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisablePlant(string id, bool Islive)
        {

            var res = _masterService.DisablePlant(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        //Department

        [HttpPost]
        public JsonResult InsertDatawithdept()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentModel>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new DepartmentModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Departments();
                    mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Departmentname = Model.Departmentname;
                   


                    res = _masterService.InsertDatawithdept(mdl);
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
        public JsonResult GetDataListdept()
        {
            var objList = _masterService.GetDataListdept();
            var lst = new List<DepartmentModel>();
            foreach (Prosol_Departments mdl in objList)
            {
                var obj = new DepartmentModel();
                obj._id = mdl._id.ToString();
                obj.Departmentname = mdl.Departmentname;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetMasterdept()
        {
            var objList = _masterService.GetMasterdept();
            var lst = new List<DepartmentModel>();
            foreach (Prosol_Departments mdl in objList)
            {
                var obj = new DepartmentModel();
                obj._id = mdl._id.ToString();
                obj.Departmentname = mdl.Departmentname;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelDatadept(string id)
        {
            var res = _masterService.DelDatadept(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DisableDept(string id, bool Islive)
        {

            var res = _masterService.DisableDept(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult getstoragelocation(string plant)
        {
            var strloc = _masterService.getstoragelocation(plant);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertDatawithstorage()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterModel>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new MasterModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Master();
                    mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Code = Model.Code;
                    mdl.Title = Model.Title;
                    mdl.Label = Model.Label;
                    mdl.Plantcode = Model.Plantcode;
                    mdl.Storagelocationcode = Model.Storagelocationcode;
                    res = _masterService.InsertDatawithstorage(mdl);
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

        public JsonResult GetDataListstorage(string label)
        {
            var objList = _masterService.GetDataListstorage(label);
            var lst = new List<MasterModel>();
            foreach (Prosol_Master mdl in objList)
            {
                var obj = new MasterModel();
                obj._id = mdl._id.ToString();
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Plantcode = mdl.Plantcode;
                obj.Label = mdl.Label;
                obj.Storagelocationcode = mdl.Storagelocationcode;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMasterstorage()
        {
            var objList = _masterService.GetMasterstorage();
            var lst = new List<MasterModel>();
            foreach (Prosol_Master mdl in objList)
            {
                var obj = new MasterModel();
                obj._id = mdl._id.ToString();
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Plantcode = mdl.Plantcode;
                obj.Label = mdl.Label;
                obj.Storagelocationcode = mdl.Storagelocationcode;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelDatastorage(string id)
        {
            var res = _masterService.DelDatadept(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult InsertDatawithplant()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterModel>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new MasterModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Master();
                    mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Label = Model.Label;
                    mdl.Code = Model.Code;
                    mdl.Title = Model.Title;
                    mdl.Plantcode = Model.Plantcode;

                    res = _masterService.InsertDatawithplant(mdl);
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
        public JsonResult Getcategory()
        {
            var objList1 = _masterService.Getcategory();
            //  var objList = _masterService.GetMaster().ToList();
            var lst = new List<ServiceCategory>();
            // if (objList != null && objList.Count>0)
            if (objList1 != null)
            {
                foreach (Prosol_ServiceCategory mdl in objList1)
                {
                    var obj = new ServiceCategory();
                    obj._id = mdl._id.ToString();
                    obj.SeviceCategorycode = mdl.SeviceCategorycode;
                    obj.SeviceCategoryname = mdl.SeviceCategoryname;


                    lst.Add(obj);
                }
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

    }
}