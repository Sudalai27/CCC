using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProsolOnline.Models;
using ProsolOnline.ViewModel;
using Prosol.Common;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Newtonsoft.Json;
using System.Data;
using MongoDB.Bson;
using System.IO;

namespace ProsolOnline.Controllers
{

    public class DictionaryController : Controller
    {
        private readonly INounModifier _NounModifiService;
        private readonly ICharateristics _CharateristicService;
        private readonly ICatalogue _CatalogueService;
        private readonly I_Asset _AssetService;
        private readonly IGeneralSettings _GeneralSettings;
        private readonly ILogic _LogicService;
        public DictionaryController(INounModifier NMservice,
            ICharateristics CharaService,
            ICatalogue catalogueService,
            I_Asset assetService,
            IGeneralSettings generalSettings,
            ILogic LogicService)
        {
            _NounModifiService = NMservice;
            _CharateristicService = CharaService;
            _CatalogueService = catalogueService;
            _AssetService = assetService;
            _GeneralSettings = generalSettings;
            _LogicService = LogicService;
        }

        // GET: Dictionary
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("Create") == 1)
                return View("Add");
            else if (CheckAccess("Create") == 0)
                return View("Denied");
            else return View("Login");

        }
        //coadinglogic

        public ActionResult CodeLogic()
        {
            if (CheckAccess("Create") == 1)
                return View();
            else if (CheckAccess("Create") == 0)
                return View("Denied");
            else return View("Login");
        }
        public ActionResult Logic()
        {
            if (CheckAccess("Logic") == 1)
                return View();
            else if (CheckAccess("Logic") == 0)
                return View("Denied");
            else return View("Login");
        }
        [Authorize]
        public ActionResult Characteristics()
        {
            if (CheckAccess("Characteristics") == 1)
                return View();
            else if (CheckAccess("Characteristics") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Values()
        {
            if (CheckAccess("ValueMaster") == 1)
                return View();
            else if (CheckAccess("ValueMaster") == 0)
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
        public ActionResult add()
        {
            if (CheckAccess("Create") == 1)
                return View();
            else if (CheckAccess("Create") == 0)
                return View("Denied");
            else return View("Login");
        }
        [Authorize]
        public ActionResult view()
        {
            if (CheckAccess("View") == 1)
                return View();
            else if (CheckAccess("View") == 0)
                return View("Denied");
            else return View("Login");

        }
        // public string        (NounModifierModel NM, HttpPostedFileBase[] files)  

        [HttpPost]
        [Authorize]
        // [ValidateAntiForgeryToken]
        public JsonResult addNM()
        {
            var NM = Request.Form["NM"];
            // NounModifierModel Model = new NounModifierModel();
            var Model = JsonConvert.DeserializeObject<NounModifierModel>(NM);


            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            var Charas = Request.Form["CHA"];
            var ListCharas = JsonConvert.DeserializeObject<List<NM_AttributesModel>>(Charas);
            string res = "";
            try
            {
                if (Model == null)
                    Model = new NounModifierModel();
                TryUpdateModel(Model);

                foreach (NM_AttributesModel chMdel in ListCharas)
                {
                    TryUpdateModel(chMdel);
                }
                if (ModelState.IsValid)
                {
                    //Noun Modifier DB write
                    var NounMod = new Prosol_NounModifiers();
                    if (CreateNounModifier(NounMod, Model, file))
                    {
                        //Charateristic DB write                   
                        if (CreateCharateristics(ListCharas, Model))
                        {
                            res = "Success";
                        }
                        else res = "Fail";
                    }
                    else res = "Fail";
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                    // ModelState.AddModelError("", "Noun and Modifier shouldn't be blank");
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = "Error";
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        // [ValidateAntiForgeryToken]
        public JsonResult AssetAddNM()
        {
            var NM = Request.Form["NM"];
            // NounModifierModel Model = new NounModifierModel();
            var Model = JsonConvert.DeserializeObject<NounModifierModel>(NM);


            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            var Charas = Request.Form["CHA"];
            var ListCharas = JsonConvert.DeserializeObject<List<NM_AttributesModel>>(Charas);
            string res = "";
            try
            {
                if (Model == null)
                    Model = new NounModifierModel();
                TryUpdateModel(Model);

                foreach (NM_AttributesModel chMdel in ListCharas)
                {
                    TryUpdateModel(chMdel);
                }
                if (ModelState.IsValid)
                {
                    //Noun Modifier DB write
                    var NounMod = new Prosol_NounModifiers();
                    if (AssetCreateNounModifier(NounMod, Model, file))
                    {
                        //Charateristic DB write                   
                        if (AssetCreateCharateristics(ListCharas, Model))
                        {
                            res = "Success";
                        }
                        else res = "Fail";
                    }
                    else res = "Fail";
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                    // ModelState.AddModelError("", "Noun and Modifier shouldn't be blank");
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = "Error";
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        protected virtual bool CreateCharateristics(List<NM_AttributesModel> ListAttributes, NounModifierModel NMmodel)
        {
            var lstCharateristics = new List<Prosol_Charateristics>();

            foreach (NM_AttributesModel LstAtt in ListAttributes)
            {
                var AttrMdl = new Prosol_Charateristics();
                AttrMdl.Noun = NMmodel.Noun;
                AttrMdl.Modifier = NMmodel.Modifier;
                AttrMdl.Characteristic = LstAtt.Characteristic;
                AttrMdl.Abbrivation = LstAtt.Abbrivation;
                AttrMdl.Squence = LstAtt.Squence;
                AttrMdl.ShortSquence = LstAtt.ShortSquence;
                AttrMdl.Mandatory = LstAtt.Mandatory;
                AttrMdl.Definition = LstAtt.Definition;
                AttrMdl.Values = LstAtt.Values;
                AttrMdl.Uom = LstAtt.Uom;
                AttrMdl.UomMandatory = LstAtt.UomMandatory;

                AttrMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                lstCharateristics.Add(AttrMdl);

            }
            return _CharateristicService.Create(lstCharateristics);
        }

        [NonAction]
        protected virtual bool AssetCreateCharateristics(List<NM_AttributesModel> ListAttributes, NounModifierModel NMmodel)
        {
            var lstCharateristics = new List<Prosol_Charateristics>();

            foreach (NM_AttributesModel LstAtt in ListAttributes)
            {
                var AttrMdl = new Prosol_Charateristics();
                AttrMdl.Noun = NMmodel.Noun;
                AttrMdl.Modifier = NMmodel.Modifier;
                AttrMdl.Characteristic = LstAtt.Characteristic;
                AttrMdl.Abbrivation = LstAtt.Abbrivation;
                AttrMdl.Squence = LstAtt.ShortSquence;
                AttrMdl.ShortSquence = LstAtt.ShortSquence;
                AttrMdl.Mandatory = LstAtt.Mandatory;
                //AttrMdl.Definition = LstAtt.Definition;
                AttrMdl.Definition = "Equ";
                AttrMdl.Values = LstAtt.Values;
                AttrMdl.Uom = LstAtt.Uom;
                AttrMdl.UomMandatory = LstAtt.UomMandatory;
                AttrMdl.ClassificationId = LstAtt.ClassificationId;
                AttrMdl.ClassLevel = LstAtt.ClassLevel;
                AttrMdl.HierarchyPath = LstAtt.HierarchyPath;
                AttrMdl.PDesc = LstAtt.PDesc;

                AttrMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                lstCharateristics.Add(AttrMdl);

            }
            return _CharateristicService.AssetCreate(lstCharateristics);
        }

        [NonAction]
        protected virtual bool CreateNounModifier(Prosol_NounModifiers NounMod, NounModifierModel Model, HttpPostedFileBase file)
        {
            try
            {
                var arrStr = _NounModifiService.GetNounModifier(Model.Noun, Model.Modifier);
                if (arrStr != null)
                {
                    NounMod._id = Model._id != null ? new ObjectId(Model._id) : new ObjectId();
                    NounMod.Noun = Model.Noun;
                    NounMod.NounEqu = Model.NounEqu;
                    NounMod.Nounabv = Model.Nounabv;
                    NounMod.NounDefinition = Model.NounDefinition;
                    NounMod.Modifier = Model.Modifier;
                    NounMod.ModifierEqu = Model.ModifierEqu;
                    NounMod.Modifierabv = Model.Modifierabv;
                    NounMod.ModifierDefinition = Model.ModifierDefinition;
                    NounMod.Similaritems = Model.Similaritems;
                    NounMod.Formatted = Model.Formatted;
                    NounMod.uomlist = Model.uomlist;
                    return _NounModifiService.Create(NounMod, file);
                }
                else
                {
                    NounMod._id = new ObjectId();
                    NounMod.Noun = Model.Noun;
                    NounMod.NounEqu = Model.NounEqu;
                    NounMod.Nounabv = Model.Nounabv;
                    NounMod.NounDefinition = Model.NounDefinition;
                    NounMod.Modifier = Model.Modifier;
                    NounMod.ModifierEqu = Model.ModifierEqu;
                    NounMod.Modifierabv = Model.Modifierabv;
                    NounMod.ModifierDefinition = Model.ModifierDefinition;
                    NounMod.Similaritems = Model.Similaritems;
                    NounMod.Formatted = Model.Formatted;
                    NounMod.uomlist = Model.uomlist;
                    return _NounModifiService.Create(NounMod, file);

                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                return false;
            }
        }
        [NonAction]
        protected virtual bool AssetCreateNounModifier(Prosol_NounModifiers NounMod, NounModifierModel Model, HttpPostedFileBase file)
        {
            try
            {
                var arrStr = _NounModifiService.GetNounModifier(Model.Noun, Model.Modifier);
                if (arrStr != null)
                {
                    NounMod._id = Model._id != null ? new ObjectId(Model._id) : new ObjectId();
                    NounMod.Noun = Model.Noun;
                    NounMod.NounEqu = Model.NounEqu;
                    NounMod.Nounabv = Model.Nounabv;
                    NounMod.NounDefinition = Model.NounDefinition;
                    NounMod.Modifier = Model.Modifier;
                    NounMod.ModifierEqu = Model.ModifierEqu;
                    NounMod.Modifierabv = Model.Modifierabv;
                    NounMod.ModifierDefinition = Model.ModifierDefinition;
                    NounMod.Similaritems = Model.Similaritems;
                    //NounMod.Formatted = Model.Formatted;
                    NounMod.Formatted = 1;
                    NounMod.uomlist = Model.uomlist;
                    return _NounModifiService.AssetCreate(NounMod, file);
                }
                else
                {
                    NounMod._id = new ObjectId();
                    NounMod.Noun = Model.Noun;
                    NounMod.NounEqu = Model.NounEqu;
                    NounMod.Nounabv = Model.Nounabv;
                    NounMod.NounDefinition = Model.NounDefinition;
                    NounMod.Modifier = Model.Modifier;
                    NounMod.ModifierEqu = Model.ModifierEqu;
                    NounMod.Modifierabv = Model.Modifierabv;
                    NounMod.ModifierDefinition = Model.ModifierDefinition;
                    NounMod.Similaritems = Model.Similaritems;
                    //NounMod.Formatted = Model.Formatted;
                    NounMod.Formatted = 1;
                    NounMod.uomlist = Model.uomlist;
                    return _NounModifiService.AssetCreate(NounMod, file);

                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                return false;
            }
        }
        [Authorize]
        public JsonResult AutoCompleteNoun(string term)
        {
            var arrStr = _NounModifiService.AutoSearchNoun(term);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult AutoCompleteAssetNoun(string term)
        {
            var arrStr = _NounModifiService.AutoSearchAssetNoun(term);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult AutoCompleteModifier(string term, string Noun)
        {

            var arrStr = _NounModifiService.AutoSearchModifier(term, Noun);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult AutoCompleteAssetModifier(string term, string Noun)
        {

            var arrStr = _NounModifiService.AutoSearchModifier(term, Noun);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [Authorize]
        public JsonResult GetNounModifier(string Noun, string Modifier)
        {
            if (Noun != "null" && Modifier != "null" && Noun != null && Modifier != null)
            {
                var arrStr = _NounModifiService.GetNounModifier(Noun, Modifier);
                if (arrStr != null)
                {
                    var AttributeList = _CharateristicService.GetAttributes().ToList();
                    var NM = new NounModifierModel();
                    NM._id = (arrStr != null) ? arrStr._id.ToString() : null;
                    NM.Noun = arrStr.Noun;
                    NM.Modifier = arrStr.Modifier;
                    NM.NounEqu = arrStr.NounEqu;
                    NM.Nounabv = arrStr.Nounabv;
                    NM.NounDefinition = arrStr.NounDefinition;
                    NM.ModifierEqu = arrStr.ModifierEqu;
                    NM.Modifierabv = arrStr.Modifierabv;
                    NM.ModifierDefinition = arrStr.ModifierDefinition;
                    NM.Similaritems = arrStr.Similaritems;
                    NM.Formatted = arrStr.Formatted;
                    NM.FileData = arrStr.FileData;
                    NM.uomlist = arrStr.uomlist;

                    var NMC_VM = new NounModifierVM();
                    NMC_VM.One_NounModifier = NM;

                    var arrChar = _CharateristicService.GetCharateristic(Noun, Modifier);
                    //var arrChar = _CharateristicService.GetCharateristics(Noun, Modifier, "Equ");
                    var lstChar = new List<NM_AttributesModel>();
                    foreach (Prosol_Charateristics nm_Char in arrChar)
                    {
                        var Chara = new NM_AttributesModel();
                        Chara.Noun = nm_Char.Noun;
                        Chara.Modifier = nm_Char.Modifier;
                        Chara.Characteristic = nm_Char.Characteristic;
                        Chara.Abbrivation = nm_Char.Abbrivation;
                        Chara.Squence = nm_Char.Squence;
                        Chara.ShortSquence = nm_Char.ShortSquence;
                        Chara.Mandatory = nm_Char.Mandatory;
                        Chara.Definition = nm_Char.Definition;
                        Chara.Values = nm_Char.Values;
                        Chara.Values = nm_Char.Values;
                        Chara.Uom = nm_Char.Uom;
                        Chara.UomMandatory = nm_Char.UomMandatory;

                        var sObj = (from obj in AttributeList where obj.Attribute.Equals(nm_Char.Characteristic, StringComparison.OrdinalIgnoreCase) select obj).FirstOrDefault();
                        if (sObj != null)
                            //  Chara.Validation = sObj.Validation == 1 ? @"^[0-9-./]+$" : "";
                            Chara.Validation = sObj.Validation == 1 ? @"^[0-9](.*[0-9])?$" : "";
                        else Chara.Validation = "";
                        Chara.Remove = 0;
                        lstChar.Add(Chara);
                    }
                    NMC_VM.ALL_NM_Attributes = lstChar;
                    return this.Json(NMC_VM, JsonRequestBehavior.AllowGet);
                }
                else
                    return this.Json(null, JsonRequestBehavior.AllowGet);

            }
            else
                return this.Json(null, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        [Authorize]
        public JsonResult GetNounModifier2(string Noun, string Modifier)
        {
            if (Noun != "null" && Modifier != "null" && Noun != null && Modifier != null)
            {
                var arrStr = _NounModifiService.GetNounModifier(Noun, Modifier);
                if (arrStr != null)
                {
                    var AttributeList = _CharateristicService.GetAttributes().ToList();
                    var NM = new NounModifierModel();
                    NM._id = (arrStr != null) ? arrStr._id.ToString() : null;
                    NM.Noun = arrStr.Noun;
                    NM.Modifier = arrStr.Modifier;
                    NM.NounEqu = arrStr.NounEqu;
                    NM.Nounabv = arrStr.Nounabv;
                    NM.NounDefinition = arrStr.NounDefinition;
                    NM.ModifierEqu = arrStr.ModifierEqu;
                    NM.Modifierabv = arrStr.Modifierabv;
                    NM.ModifierDefinition = arrStr.ModifierDefinition;
                    NM.Similaritems = arrStr.Similaritems;
                    NM.Formatted = arrStr.Formatted;
                    NM.FileData = arrStr.FileData;
                    NM.uomlist = arrStr.uomlist;

                    var NMC_VM = new NounModifierVM();
                    NMC_VM.One_NounModifier = NM;

                    var arrChar = _CharateristicService.GetCharateristics(Noun, Modifier, "MM");
                    var lstChar = new List<NM_AttributesModel>();
                    foreach (Prosol_Charateristics nm_Char in arrChar)
                    {
                        var Chara = new NM_AttributesModel();
                        Chara.Noun = nm_Char.Noun;
                        Chara.Modifier = nm_Char.Modifier;
                        Chara.Characteristic = nm_Char.Characteristic;
                        //  Chara.Abbrivation = nm_Char.Abbrivation;
                        Chara.Squence = nm_Char.Squence;
                        Chara.ShortSquence = nm_Char.ShortSquence;
                        Chara.Mandatory = nm_Char.Mandatory;
                        Chara.Definition = nm_Char.Definition;
                        Chara.Values = nm_Char.Values;
                        Chara.Values = nm_Char.Values;
                        Chara.Uom = nm_Char.Uom;
                        Chara.UomMandatory = nm_Char.UomMandatory;

                        var sObj = (from obj in AttributeList where obj.Attribute.Equals(nm_Char.Characteristic, StringComparison.OrdinalIgnoreCase) select obj).FirstOrDefault();
                        if (sObj != null)
                            //  Chara.Validation = sObj.Validation == 1 ? @"^[0-9-./]+$" : "";
                            Chara.Validation = sObj.Validation == 1 ? @"^[0-9](.*[0-9])?$" : "";
                        else Chara.Validation = "";
                        Chara.Remove = 0;
                        lstChar.Add(Chara);
                    }
                    NMC_VM.ALL_NM_Attributes = lstChar;
                    var jsonResult = Json(NMC_VM, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                else
                    return this.Json(null, JsonRequestBehavior.AllowGet);

            }
            else
                return this.Json(null, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        [Authorize]
        public JsonResult EquGetNounModifier(string Noun, string Modifier)
        {
            if (Noun != "null" && Modifier != "null" && Noun != null && Modifier != null)
            {
                var arrStr = _NounModifiService.GetNounModifier(Noun, Modifier);
                if (arrStr != null)
                {
                    var AttributeList = _CharateristicService.GetAttributes().ToList();
                    var NM = new NounModifierModel();
                    NM._id = (arrStr != null) ? arrStr._id.ToString() : null;
                    NM.Noun = arrStr.Noun;
                    NM.Modifier = arrStr.Modifier;
                    NM.NounEqu = arrStr.NounEqu;
                    NM.Nounabv = arrStr.Nounabv;
                    NM.NounDefinition = arrStr.NounDefinition;
                    NM.ModifierEqu = arrStr.ModifierEqu;
                    NM.Modifierabv = arrStr.Modifierabv;
                    NM.ModifierDefinition = arrStr.ModifierDefinition;
                    NM.Similaritems = arrStr.Similaritems;
                    NM.Formatted = arrStr.Formatted;
                    NM.FileData = arrStr.FileData;
                    NM.uomlist = arrStr.uomlist;

                    var NMC_VM = new NounModifierVM();
                    NMC_VM.One_NounModifier = NM;

                    var arrChar = _CharateristicService.GetCharateristics(Noun, Modifier, "Equ");
                    var lstChar = new List<NM_AttributesModel>();
                    foreach (Prosol_Charateristics nm_Char in arrChar)
                    {
                        var Chara = new NM_AttributesModel();
                        Chara.Noun = nm_Char.Noun;
                        Chara.Modifier = nm_Char.Modifier;
                        Chara.Characteristic = nm_Char.Characteristic;
                        Chara.Abbrivation = nm_Char.Abbrivation;
                        Chara.Squence = nm_Char.Squence;
                        Chara.ShortSquence = nm_Char.ShortSquence;
                        Chara.Mandatory = nm_Char.Mandatory;
                        Chara.Definition = nm_Char.Definition;
                        var values = _AssetService.GetAssetValues(Chara.Noun, Chara.Modifier, Chara.Characteristic);
                        Chara.Values = values.ToArray();
                        Chara.Uom = nm_Char.Uom;
                        Chara.UomMandatory = nm_Char.UomMandatory;
                        Chara.HierarchyPath = nm_Char.HierarchyPath;
                        Chara.ClassificationId = nm_Char.ClassificationId;
                        Chara.ClassLevel = nm_Char.ClassLevel;
                        Chara.PDesc = nm_Char.PDesc;

                        var sObj = (from obj in AttributeList where obj.Attribute.Equals(nm_Char.Characteristic, StringComparison.OrdinalIgnoreCase) select obj).FirstOrDefault();
                        if (sObj != null)
                            //  Chara.Validation = sObj.Validation == 1 ? @"^[0-9-./]+$" : "";
                            Chara.Validation = sObj.Validation == 1 ? @"^[0-9](.*[0-9])?$" : "";
                        else Chara.Validation = "";
                        Chara.Remove = 0;
                        lstChar.Add(Chara);
                    }
                    NMC_VM.ALL_NM_Attributes = lstChar;
                    return this.Json(NMC_VM, JsonRequestBehavior.AllowGet);
                }
                else
                    return this.Json(null, JsonRequestBehavior.AllowGet);

            }
            else
                return this.Json(null, JsonRequestBehavior.AllowGet);


        }


        [HttpGet]
        [Authorize]
        public JsonResult GetNounDetail(string Noun)
        {

            var arrStr = _NounModifiService.GetNounDetail(Noun);
            var NM = new NounModifierModel();
            NM._id = arrStr._id.ToString();
            NM.Noun = arrStr.Noun;
            //     NM.Nounabv = arrStr.Nounabv;
            //   NM.NounDefinition = arrStr.NounDefinition;
            return this.Json(NM, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult NM_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _NounModifiService.BulkNounModifier(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult CH_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CharateristicService.BulkCharateristic(file);
                }

            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //value
        [HttpPost]
        public JsonResult Value_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CharateristicService.BulkValue(file);
                }

            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Uom_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CharateristicService.BulkUom(file);
                }

            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public JsonResult GetNoun()
        {
            var arrStr = _NounModifiService.GetNounList();
            var result = arrStr.Select(i => new {i.Noun}).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult GetAssetNoun()
        {
            var arrStr = _NounModifiService.GetAssetNounList();
            var result = arrStr.Select(i => new {i.Noun}).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
       
        public JsonResult GetForamted(string Noun, string Modifier)
        {
            string value = "";
            var format = _NounModifiService.Getformat(Noun, Modifier);

            if (format[0].Formatted == 0)
            {
                value = "OEM";
            }
            else if (format[0].Formatted == 1)
            {
                value = "Generic";
            }
            else if (format[0].Formatted == 2)
            {
                value = "OPM";
            }
            return this.Json(value, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetModifier(string Noun)
        {
            var arrStr = _NounModifiService.GetModifierList(Noun);
            var result = arrStr.Select(i => new { i.Modifier, i.ModifierDefinition,i.NounEqu }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetAssetModifier(string Noun)
        {
            var arrStr = _NounModifiService.GetAssetModifierList(Noun);
            var result = arrStr.Select(i => new { i.Modifier, i.ModifierDefinition,i.NounEqu }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetRP(string Noun)
        {
            var arrStr = _NounModifiService.GetModifierList(Noun).ToList();
          
            return this.Json(arrStr[0].RP, JsonRequestBehavior.AllowGet);

        }

        [Authorize]

        public void Download()
        {
            var res = _CharateristicService.DownloadNM();

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
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"AssetDictionary.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();


            //var strJson = JsonConvert.SerializeObject(res);
            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));

            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "NounModifier.xls"));
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

        [Authorize]

        public void DownloadAsset()
        {
            var res = _CharateristicService.DownloadAssetNM();

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
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"AssetDictionary.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();


            //var strJson = JsonConvert.SerializeObject(res);
            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));

            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "NounModifier.xls"));
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


        [Authorize]
        public JsonResult GetValuesList(string Noun, string Modifier, string Characteristic)
        {
            var arrStr = _CatalogueService.GetValues(Noun, Modifier, Characteristic);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetUOMList()
        {

            var UOMList = new List<UOMModel>();
            var varUOMList = _GeneralSettings.GetUOMList();
            foreach (Prosol_UOM md in varUOMList)
            {
                var midMdl = new UOMModel();
                midMdl._id = md._id.ToString();
                midMdl.Unitname = md.Unitname;
                UOMList.Add(midMdl);

            }
            return this.Json(UOMList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetValueList(int currentPage, int maxRows, string Name)
        {

            var ValueList1 = _GeneralSettings.GetAbbrList().ToList();


            var AttributeObj = _CharateristicService.GetAttributeDetail(Name);
            if (AttributeObj != null)
            {

                if (AttributeObj.ValueList != null && AttributeObj.ValueList.Count() > 0)
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
                    if (first.Count > 0)
                    {
                        //foreach (Prosol_Abbrevate pat in first)
                        //{
                        //    ValueList.Add(pat);
                        //}
                        //foreach (Prosol_Abbrevate pat in ValueList1)
                        //{
                        //    ValueList.Add(pat);
                        //}
                        ValueList = first.Concat(ValueList1).ToList();
                    }
                    else
                    {
                        ValueList = ValueList1;
                    }
                    var lst = new List<AbbrevateModel>();
                    PaingGroup pageList = new PaingGroup();
                    pageList.totItem = ValueList.ToList().Count;
                    var lstTmp = (from prsl in ValueList
                                  select prsl)
                                .Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();

                    //var lst = new List<AbbrevateModel>();
                    //PaingGroup pageList = new PaingGroup();
                    //pageList.totItem = ValueList.ToList().Count;
                    //var lstTmp = (from prsl in ValueList
                    //              select prsl)
                    //            .OrderBy(prsl => prsl._id)
                    //            .Skip((currentPage - 1) * maxRows)
                    //            .Take(maxRows).ToList();

                    foreach (Prosol_Abbrevate mdl in lstTmp)
                    {
                        var midMdl = new AbbrevateModel();
                        midMdl._id = mdl._id.ToString();
                        midMdl.Value = mdl.Value;
                        midMdl.vunit = "  " + mdl.vunit;
                        lst.Add(midMdl);

                    }

                    pageList.ValueList = lst;
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


                    //var lst = new List<AbbrevateModel>();
                    //PaingGroup pageList = new PaingGroup();
                    //pageList.totItem = ValueList.ToList().Count;
                    //var lstTmp = (from prsl in ValueList
                    //              select prsl)
                    //            .OrderBy(prsl => prsl._id)
                    //            .Skip((currentPage - 1) * maxRows)
                    //            .Take(maxRows).ToList();

                    foreach (Prosol_Abbrevate mdl in lstTmp)
                    {
                        var midMdl = new AbbrevateModel();
                        midMdl._id = mdl._id.ToString();
                        midMdl.Value = mdl.Value;
                        midMdl.vunit = "  " + mdl.vunit;
                        lst.Add(midMdl);

                    }

                    pageList.ValueList = lst;
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


                //var lst = new List<AbbrevateModel>();
                //PaingGroup pageList = new PaingGroup();
                //pageList.totItem = ValueList.ToList().Count;
                //var lstTmp = (from prsl in ValueList
                //              select prsl)
                //            .OrderBy(prsl => prsl._id)
                //            .Skip((currentPage - 1) * maxRows)
                //            .Take(maxRows).ToList();

                foreach (Prosol_Abbrevate mdl in lstTmp)
                {
                    var midMdl = new AbbrevateModel();
                    midMdl._id = mdl._id.ToString();
                    midMdl.Value = mdl.Value;
                    midMdl.vunit = "  " + mdl.vunit;
                    lst.Add(midMdl);

                }

                pageList.ValueList = lst;
                double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                pageList.PageCount = (int)Math.Ceiling(pageCount);
                pageList.CurrentPageIndex = currentPage;
                return this.Json(pageList, JsonRequestBehavior.AllowGet);
            }

            //var lst = new List<AbbrevateModel>();
            //PaingGroup pageList = new PaingGroup();
            //pageList.totItem = ValueList.ToList().Count;
            //var lstTmp = (from prsl in ValueList
            //              select prsl)
            //            .OrderBy(prsl => prsl._id)
            //            .Skip((currentPage - 1) * maxRows)
            //            .Take(maxRows).ToList();

            //foreach (Prosol_Abbrevate mdl in lstTmp)
            //{
            //    var midMdl = new AbbrevateModel();
            //    midMdl._id = mdl._id.ToString();
            //    midMdl.Value = mdl.Value;
            //    midMdl.vunit = "  " + mdl.vunit;
            //    lst.Add(midMdl);

            //}

            //pageList.ValueList = lst;
            //double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
            //pageList.PageCount = (int)Math.Ceiling(pageCount);
            //pageList.CurrentPageIndex = currentPage;
            //return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        //public JsonResult GetValueList()
        //{
        //    var abbList = new List<AbbrevateModel>();
        //    var ValueList = _GeneralSettings.GetAbbrList();
        //    foreach (Prosol_Abbrevate md in ValueList)
        //    {
        //        var midMdl = new AbbrevateModel();
        //        midMdl._id = md._id.ToString();
        //        midMdl.Value = md.Value;
        //        midMdl.vunit = "  "+md.vunit;
        //        abbList.Add(midMdl);

        //    }
        //    return this.Json(abbList, JsonRequestBehavior.AllowGet);

        //}

        public JsonResult GetValueListSearch(string Name, string srchtxt, int currentPage, int maxRows)
        {
            // var abbList = new List<AbbrevateModel>();
            var ValueList1 = _GeneralSettings.GetAbbrList(srchtxt).ToList();

            //---------------

            var AttributeObj = _CharateristicService.GetAttributeDetail(Name);

            List<Prosol_Abbrevate> first = new List<Prosol_Abbrevate>();
            if (AttributeObj != null)
            {
                if (AttributeObj.ValueList != null && AttributeObj.ValueList.Count() > 0)
                {
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
                    if (first.Count > 0)
                    {
                        //foreach(Prosol_Abbrevate pat in first)
                        //{
                        //    ValueList.Add(pat);
                        //}
                        //foreach (Prosol_Abbrevate pat in ValueList1)
                        //{
                        //    ValueList.Add(pat);
                        //}
                        ValueList = first.Concat(ValueList1).ToList();
                    }
                    else
                    {
                        ValueList = ValueList1;
                    }




                    var lst = new List<AbbrevateModel>();
                    PaingGroup pageList = new PaingGroup();
                    pageList.totItem = ValueList.ToList().Count;
                    var lstTmp = (from prsl in ValueList
                                  select prsl)
                                .Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();

                    //var lst = new List<AbbrevateModel>();
                    //PaingGroup pageList = new PaingGroup();
                    //pageList.totItem = ValueList.ToList().Count;
                    //var lstTmp = (from prsl in ValueList
                    //              select prsl)
                    //            .OrderBy(prsl => prsl._id)
                    //            .Skip((currentPage - 1) * maxRows)
                    //            .Take(maxRows).ToList();

                    foreach (Prosol_Abbrevate mdl in lstTmp)
                    {
                        var midMdl = new AbbrevateModel();
                        midMdl._id = mdl._id.ToString();
                        midMdl.Value = mdl.Value;
                        midMdl.vunit = "  " + mdl.vunit;
                        lst.Add(midMdl);

                    }

                    pageList.ValueList = lst;
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

                    //var lst = new List<AbbrevateModel>();
                    //PaingGroup pageList = new PaingGroup();
                    //pageList.totItem = ValueList.ToList().Count;
                    //var lstTmp = (from prsl in ValueList
                    //              select prsl)
                    //            .OrderBy(prsl => prsl._id)
                    //            .Skip((currentPage - 1) * maxRows)
                    //            .Take(maxRows).ToList();

                    foreach (Prosol_Abbrevate mdl in lstTmp)
                    {
                        var midMdl = new AbbrevateModel();
                        midMdl._id = mdl._id.ToString();
                        midMdl.Value = mdl.Value;
                        midMdl.vunit = "  " + mdl.vunit;
                        lst.Add(midMdl);

                    }

                    pageList.ValueList = lst;
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

                //var lst = new List<AbbrevateModel>();
                //PaingGroup pageList = new PaingGroup();
                //pageList.totItem = ValueList.ToList().Count;
                //var lstTmp = (from prsl in ValueList
                //              select prsl)
                //            .OrderBy(prsl => prsl._id)
                //            .Skip((currentPage - 1) * maxRows)
                //            .Take(maxRows).ToList();

                foreach (Prosol_Abbrevate mdl in lstTmp)
                {
                    var midMdl = new AbbrevateModel();
                    midMdl._id = mdl._id.ToString();
                    midMdl.Value = mdl.Value;
                    midMdl.vunit = "  " + mdl.vunit;
                    lst.Add(midMdl);

                }

                pageList.ValueList = lst;
                double pageCount = (double)((decimal)ValueList.Count() / Convert.ToDecimal(maxRows));
                pageList.PageCount = (int)Math.Ceiling(pageCount);
                pageList.CurrentPageIndex = currentPage;
                return this.Json(pageList, JsonRequestBehavior.AllowGet);
            }

        }




        public JsonResult getuomlist1(string Noun, string Modifier)
        {

            var list = _NounModifiService.getuomlist(Noun, Modifier);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAttributesList()
        {
            var res = _CharateristicService.GetAttributes().ToList();
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public JsonResult InsertAttribute()
        {

            var obj = Request.Form["objAtt"];
            AttributeModel AttributeMdl = JsonConvert.DeserializeObject<AttributeModel>(obj);

            var ValueCheckbxLst = Request.Form["AttirbuteList"];
            List<string> Listvalue = new List<string>();
            if (ValueCheckbxLst != "null")
            {
                Listvalue = JsonConvert.DeserializeObject<List<string>>(ValueCheckbxLst);
                AttributeMdl.ValueList = Listvalue.ToArray();
            }
            var UOMCheckbxLst = Request.Form["UOMList"];
            List<string> ListUOM = new List<string>();
            if (UOMCheckbxLst != "null")
            {
                ListUOM = JsonConvert.DeserializeObject<List<string>>(UOMCheckbxLst);
                AttributeMdl.UOMList = ListUOM.ToArray();
            }
            var dbModel = new Prosol_Attribute();
            dbModel._id = AttributeMdl._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(AttributeMdl._id);
            dbModel.Attribute = AttributeMdl.Attribute;
            dbModel.Validation = AttributeMdl.Validation;
            dbModel.ValueList = AttributeMdl.ValueList;
            dbModel.UOMList = AttributeMdl.UOMList;
            var res = _CharateristicService.AddAttribute(dbModel);


            return Json(res, JsonRequestBehavior.AllowGet);

        }




        public JsonResult GetAttributesDetail(string Name)
        {
            var AttributeObj = _CharateristicService.GetAttributeDetail(Name);
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

        public JsonResult GetNMUOMList(string Noun, string Modifier, string Attribute)
        {
            var UOMList = new List<UOMModel>();
            var AttributeObj = _CharateristicService.GetAttributeDetail(Attribute);
            var LocObj = new AttributeModel();
            if (AttributeObj != null)
            {
               
                LocObj.UOMList = AttributeObj.UOMList;
                if(LocObj.UOMList!=null && LocObj.UOMList.Length > 0)
                {
                 
                    var varUOMList = _GeneralSettings.GetUOMList();
                    foreach (Prosol_UOM md in varUOMList)
                    {
                        if (LocObj.UOMList.Contains(md._id.ToString()))
                        {
                            var midMdl = new UOMModel();
                            midMdl._id = md._id.ToString();
                            midMdl.Unitname = md.Unitname;
                            UOMList.Add(midMdl);
                        }

                    }
                    if (UOMList != null && UOMList.Count > 0)
                    {
                        var uomCharacLst = _CharateristicService.GetCharacteristicvalues(Attribute, Noun, Modifier);
                        if (uomCharacLst != null)
                        {
                            foreach (UOMModel dlm in UOMList)
                            {
                                if (uomCharacLst.Uom != null && uomCharacLst.Uom.Contains(dlm._id))
                                {
                                    dlm.UOMname = "1";
                                }
                                else dlm.UOMname = "0";

                            }
                        }
                    }

                }
            }

           
          

            return Json(UOMList, JsonRequestBehavior.AllowGet);
        }

        public void DownloadAttributeMaster()
        {
            var res = _CharateristicService.DownloadAttributeMaster();
            var strJson = JsonConvert.SerializeObject(res);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "AttributeMaster.xls"));
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
        //codesave for code logic
        public bool CodeSave()
        {

            var obj = Request.Form["obj"];
            CodeLogic Model = JsonConvert.DeserializeObject<CodeLogic>(obj);

            Prosol_CodeLogic mdl = new Prosol_CodeLogic();
            if (Model.CODELOGIC == "UNSPSC Code")
            {
                mdl.CODELOGIC = Model.CODELOGIC;
                mdl.unspsc_Version = Model.Version;
            }
            else
            {
                mdl.CODELOGIC = Model.CODELOGIC;
                mdl.unspsc_Version = Model.Version;
            }

            var getresult = _NounModifiService.codesave(mdl);
            return getresult;
        }

        public JsonResult Showdata()
        {
            var objList = _NounModifiService.showcode();
            if (objList != null)
            {
                var obj = new CodeLogic();

                obj._id = objList._id.ToString();

                obj.CODELOGIC = objList.CODELOGIC;
                obj.Version = objList.unspsc_Version;
                return this.Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("UNSPC Code", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAttributeList()
        {
            var result = _LogicService.getattributelist();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getunit()
        {
            var result = _LogicService.getunit();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        //logic
        public bool insertdata()
        {
            int update = 0;
            var obj = Request.Form["obj"];
            Logic Model = JsonConvert.DeserializeObject<Logic>(obj);

            Prosol_Logic mdl = new Prosol_Logic();

            mdl.AttributeName1 = Model.AttributeName1;
            mdl.Value1 = Model.Value1;
            mdl.Unitname1 = Model.Unitname1;
            mdl.AttributeName2 = Model.AttributeName2;
            mdl.Value2 = Model.Value2;
            mdl.Unitname2 = Model.Unitname2;
            mdl.AttributeName3 = Model.AttributeName3;
            mdl.Value3 = Model.Value3;
            mdl.Unitname3 = Model.Unitname3;
            mdl.AttributeName4 = Model.AttributeName4;
            mdl.Value4 = Model.Value4;
            mdl.Unitname4 = Model.Unitname4;
            var getresult = _LogicService.insertdata(mdl, update);
            return getresult;
        }
        public bool updatedata()
        {
            int update = 1;
            var obj = Request.Form["obj"];
            Logic Model = JsonConvert.DeserializeObject<Logic>(obj);

            Prosol_Logic mdl = new Prosol_Logic();
            mdl._id = new ObjectId(Model._id);
            mdl.AttributeName1 = Model.AttributeName1;
            mdl.Value1 = Model.Value1;
            mdl.Unitname1 = Model.Unitname1;
            mdl.AttributeName2 = Model.AttributeName2;
            mdl.Value2 = Model.Value2;
            mdl.Unitname2 = Model.Unitname2;
            mdl.AttributeName3 = Model.AttributeName3;
            mdl.Value3 = Model.Value3;
            mdl.Unitname3 = Model.Unitname3;
            mdl.AttributeName4 = Model.AttributeName4;
            mdl.Value4 = Model.Value4;
            mdl.Unitname4 = Model.Unitname4;
            var getresult = _LogicService.insertdata(mdl, update);
            return getresult;
        }
        public JsonResult ATTRRELLIST()
        {
            var objList = _LogicService.ATTRRELLIST();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<Logic>();
            foreach (Prosol_Logic mdl in objList)
            {
                var obj = new Logic();
                obj._id = mdl._id.ToString();

                obj.AttributeName1 = mdl.AttributeName1;
                obj.Value1 = mdl.Value1;
                obj.Unitname1 = mdl.Unitname1;
                obj.AttributeName2 = mdl.AttributeName2;
                obj.Value2 = mdl.Value2;
                obj.Unitname2 = mdl.Unitname2;
                obj.AttributeName3 = mdl.AttributeName3;
                obj.Value3 = mdl.Value3;
                obj.Unitname3 = mdl.Unitname3;
                obj.AttributeName4 = mdl.AttributeName4;
                obj.Value4 = mdl.Value4;
                obj.Unitname4 = mdl.Unitname4;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteAttrRel(string id)
        {

            var res = _LogicService.deleteattribute(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getunitList(string AttributeName1)
        {

            var strloc = _LogicService.getunitname(AttributeName1);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);

        }
        //valuelist
        public JsonResult GetValueList1()
        {
            var res = _LogicService.GetValue1().ToList();
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetNounValue()
        {
            var result = _LogicService.getnoun();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModifierValue(string Noun)
        {

            var strloc = _LogicService.getmodifiers(Noun);
            return this.Json(strloc, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetNounModifier1(string Noun, string Modifier)
        {
            var arrChar = _CharateristicService.GetCharateristic(Noun, Modifier);
            var AttributeList = _CharateristicService.GetAttributes().ToList();
            var lstChar = new List<KeyAttribute>();
            foreach (Prosol_Charateristics nm_Char in arrChar)
            {
                var Chara = new KeyAttribute();
                // Chara.Noun = nm_Char.Noun;
                // Chara.Modifier = nm_Char.Modifier;
                Chara.Characteristic = nm_Char.Characteristic;
                //  Chara.Abbrivation = nm_Char.Abbrivation;
                Chara.Squence = nm_Char.Squence;
                Chara.ShortSquence = nm_Char.ShortSquence;
                // Chara.Mandatory = nm_Char.Mandatory;
                Chara.Definition = nm_Char.Definition;
                // Chara.Values = nm_Char.Values;
                Chara.check = false;
                //var sObj = (from obj in AttributeList where obj.Attribute.Equals(nm_Char.Characteristic, StringComparison.OrdinalIgnoreCase) select obj).FirstOrDefault();
                //if (sObj != null)
                //    Chara.Validation = sObj.Validation == 1 ? "[0-9]+(\\.[0-9]{1,4}?)?" : "";
                //else Chara.Validation = "";
                lstChar.Add(Chara);

            }
            return this.Json(lstChar, JsonRequestBehavior.AllowGet);

        }


        // NM Attribute Relation

        public bool SaveNMAttributeRelationship()
        {
            int update = 0;
            var catDatalist = Request.Form["obj"];
            var Characteristics = Request.Form["uo"];

            var attvalue = Request.Form["attvalue"];
            var att = Request.Form["att"];

            string att1 = JsonConvert.DeserializeObject<string>(att);
            string attvalue1 = JsonConvert.DeserializeObject<string>(attvalue);

            List<Prosol_KeyAttribute> charcc = JsonConvert.DeserializeObject<List<Prosol_KeyAttribute>>(Characteristics);

            Prosol_NMAttributeRelationship Model = JsonConvert.DeserializeObject<Prosol_NMAttributeRelationship>(catDatalist);
            // Model._id = new ObjectId(charcc._id);
            Model.Characteristics = charcc;
            Model.KeyAttribute = att1;
            Model.KeyValue = attvalue1;

            var getresult = _LogicService.savenmattribute(Model, update);

            return true;
        }
        public bool UpdateNMAttributeRelationship()
        {
            int update = 1;
            var catDatalist = Request.Form["obj"];
            var Characteristics = Request.Form["uo"];
            var _iid = Request.Form["uid"];

            var attvalue = Request.Form["attvalue"];
            var att = Request.Form["att"];

            string att1 = JsonConvert.DeserializeObject<string>(att);
            string attvalue1 = JsonConvert.DeserializeObject<string>(attvalue);
            string _id = JsonConvert.DeserializeObject<string>(_iid);

            List<Prosol_KeyAttribute> charcc = JsonConvert.DeserializeObject<List<Prosol_KeyAttribute>>(Characteristics);

            //Prosol_NMAttributeRelationship Model = JsonConvert.DeserializeObject<Prosol_NMAttributeRelationship>(catDatalist);

            NMAttributeRelationship Model1 = JsonConvert.DeserializeObject<NMAttributeRelationship>(catDatalist);

            Prosol_NMAttributeRelationship Model = new Prosol_NMAttributeRelationship();

            Model.Characteristics = charcc;
            Model.KeyAttribute = att1;
            Model.KeyValue = attvalue1;
            Model._id = new ObjectId(Model1._id);
            Model.Noun = Model1.Noun;
            Model.Modifier = Model1.Modifier;
            //  Model.Noun = 

            var getresult = _LogicService.savenmattribute(Model, update);

            return true;
        }


        public JsonResult getNMAttributeList()
        {
            var objList = _LogicService.getnmList();

            // var objList = _masterService.GetDataListplnt();
            var lst = new List<NMAttributeRelationship>();
            foreach (Prosol_NMAttributeRelationship mdl in objList)
            {
                var obj = new NMAttributeRelationship();
                obj._id = mdl._id.ToString();

                obj.Noun = mdl.Noun;
                obj.Modifier = mdl.Modifier;
                obj.KeyAttribute = mdl.KeyAttribute;
                obj.KeyValue = mdl.KeyValue;

                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteNMAttrRel(string id)
        {

            var res = _LogicService.DeleteNMAttrRel(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTableListforkeyatt(string Noun, string Modifier, string KeyAttribute, string KeyValue)

        {
            var arrStr = _LogicService.GetTableListforkeyatt(Noun, Modifier, KeyAttribute, KeyValue);
            var obj = new NMAttributeRelationship();
            //  Prosol_NMAttributeRelationship 
            NMAttributeRelationship nm = new NMAttributeRelationship();
            foreach (Prosol_NMAttributeRelationship mdl in arrStr)
            {

                obj._id = mdl._id.ToString();
                obj.Noun = mdl.Noun;
                obj.Modifier = mdl.Modifier;
                obj.KeyAttribute = mdl.KeyAttribute;
                obj.KeyValue = mdl.KeyValue;
                obj.Characteristics = mdl.Characteristics;


            }


            return this.Json(obj, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetLogicListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var LogicList = _LogicService.GetLogicList(srchtxt);

            var lst = new List<Logic>();
            PaingLogic pageList = new PaingLogic();
            pageList.totItem = LogicList.ToList().Count;
            var lstTmp = (from prsl in LogicList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_Logic mdl in lstTmp)
            {
                var Logic = new Logic();
                Logic._id = mdl._id.ToString();

                Logic.AttributeName1 = mdl.AttributeName1;
                Logic.Value1 = mdl.Value1;
                Logic.Unitname1 = mdl.Unitname1;
                Logic.AttributeName2 = mdl.AttributeName2;
                Logic.Value2 = mdl.Value2;
                Logic.Unitname2 = mdl.Unitname2;
                Logic.AttributeName3 = mdl.AttributeName3;
                Logic.Value3 = mdl.Value3;
                Logic.Unitname3 = mdl.Unitname3;
                Logic.AttributeName4 = mdl.AttributeName4;
                Logic.Value4 = mdl.Value4;
                Logic.Unitname4 = mdl.Unitname4;

                Logic.Noun = mdl.Noun;
                Logic.Modifier = mdl.Modifier;

                //UOM.UOMname = mdl.UOMname;
                //UOM.Unitname = mdl.Unitname;
                // UOM.Attribute = mdl.Attribute;
                lst.Add(Logic);
            }

            pageList.LOGICList = lst;
            double pageCount = (double)((decimal)LogicList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetLogicList1(int currentPage, int maxRows)
        {
            var LogicList = _LogicService.GetLogicList();

            var lst = new List<Logic>();
            PaingLogic pageList = new PaingLogic();
            pageList.totItem = LogicList.ToList().Count;
            var lstTmp = (from prsl in LogicList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_Logic mdl in lstTmp)
            {
                var Logic = new Logic();
                Logic._id = mdl._id.ToString();
                Logic.AttributeName1 = mdl.AttributeName1;
                Logic.Value1 = mdl.Value1;
                Logic.Unitname1 = mdl.Unitname1;
                Logic.AttributeName2 = mdl.AttributeName2;
                Logic.Value2 = mdl.Value2;
                Logic.Unitname2 = mdl.Unitname2;
                Logic.AttributeName3 = mdl.AttributeName3;
                Logic.Value3 = mdl.Value3;
                Logic.Unitname3 = mdl.Unitname3;
                Logic.AttributeName4 = mdl.AttributeName4;
                Logic.Value4 = mdl.Value4;
                Logic.Unitname4 = mdl.Unitname4;

                Logic.Noun = mdl.Noun;
                Logic.Modifier = mdl.Modifier;

                //UOM.UOMname = mdl.UOMname;
                // UOM.Unitname = mdl.Unitname;
                //UOM.Attribute = mdl.Attribute;
                lst.Add(Logic);
            }

            pageList.LOGICList = lst;
            double pageCount = (double)((decimal)LogicList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetNMListSearch(string srchtxt, int currentPage, int maxRows)
        {
            var nmList = _LogicService.GetNMList(srchtxt);

            var lst = new List<NMAttributeRelationship>();
            PaingNM pageList = new PaingNM();
            pageList.totItem = nmList.ToList().Count;
            var lstTmp = (from prsl in nmList
                          select prsl)
                        .OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            foreach (Prosol_NMAttributeRelationship mdl in lstTmp)
            {
                var Logic = new NMAttributeRelationship();
                Logic._id = mdl._id.ToString();

                Logic.Noun = mdl.Noun;
                Logic.Modifier = mdl.Modifier;
                Logic.KeyAttribute = mdl.KeyAttribute;
                Logic.KeyValue = mdl.KeyValue;
                //Logic.Value2 = mdl.Value2;

                lst.Add(Logic);
            }

            pageList.NMList = lst;
            double pageCount = (double)((decimal)nmList.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;
            return this.Json(pageList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetNMList1(int currentPage, int maxRows)
        {
            var nmList = _LogicService.GetNMList();

            if (nmList.Count() > 0)
            {

                var lst = new List<NMAttributeRelationship>();
                PaingNM pageList = new PaingNM();
                pageList.totItem = nmList.ToList().Count;
                var lstTmp = (from prsl in nmList
                              select prsl)
                            .OrderBy(prsl => prsl._id)
                            .Skip((currentPage - 1) * maxRows)
                            .Take(maxRows).ToList();

                foreach (Prosol_NMAttributeRelationship mdl in lstTmp)
                {
                    var Logic = new NMAttributeRelationship();
                    Logic._id = mdl._id.ToString();

                    Logic.Noun = mdl.Noun;
                    Logic.Modifier = mdl.Modifier;
                    Logic.KeyAttribute = mdl.KeyAttribute;
                    Logic.KeyValue = mdl.KeyValue;
                    lst.Add(Logic);
                }

                pageList.NMList = lst;
                double pageCount = (double)((decimal)nmList.Count() / Convert.ToDecimal(maxRows));
                pageList.PageCount = (int)Math.Ceiling(pageCount);
                pageList.CurrentPageIndex = currentPage;
                return this.Json(pageList, JsonRequestBehavior.AllowGet);
            }
            return this.Json(0, JsonRequestBehavior.AllowGet);

        }
        public JsonResult NMATTRIBUTE_Upload()

        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _LogicService.BulkNounModifierAttribute(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        public class PaingLogic
        {
            ///<summary>
            /// Gets or sets Customers.
            ///</summary>
            public List<Logic> LOGICList { get; set; }

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
        public class PaingNM
        {
            ///<summary>
            /// Gets or sets Customers.
            ///</summary>
            public List<NMAttributeRelationship> NMList { get; set; }

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
        public JsonResult loadversion()
        {
            var objList = _GeneralSettings.loadversion();

            return this.Json(objList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetValueListforcreate_temp(int currentPage, int maxRows, string Name, string Noun, string Modifier)
        {

            var ValueList1 = _GeneralSettings.GetAbbrList().ToList();


            var AttributeObj = _CharateristicService.GetAttributeDetail(Name);

            if (AttributeObj != null)
            {

                if (AttributeObj.ValueList != null && AttributeObj.ValueList.Count() > 0)
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


                    var characteristicvalueObj = _CharateristicService.GetCharacteristicvalues(Name, Noun, Modifier);
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

        public JsonResult GetValueListforcreate_tempsearch(int currentPage, int maxRows, string Name, string Noun, string Modifier, string srchtxt)
        {

            var ValueList1 = _GeneralSettings.GetAbbrList(srchtxt).ToList();


            var AttributeObj = _CharateristicService.GetAttributeDetail(Name);

            if (AttributeObj != null)
            {

                if (AttributeObj.ValueList != null && AttributeObj.ValueList.Count() > 0)
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


                    var characteristicvalueObj = _CharateristicService.GetCharacteristicvalues(Name, Noun, Modifier);
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
        public JsonResult GetAttributesDetailfromcharacteristicvalues(string Name, string Noun, string Modifier)
        {
            var AttributeObj = _CharateristicService.GetCharacteristicvalues(Name, Noun, Modifier);
            var LocObj = new AttributeModel();
            if (AttributeObj != null)
            {
                LocObj._id = AttributeObj._id.ToString();
                // LocObj.Attribute = AttributeObj.Attribute;
                // LocObj.Validation = AttributeObj.Validation;
                LocObj.ValueList = AttributeObj.Values;
                 LocObj.UOMList = AttributeObj.Uom;
            }
            return Json(LocObj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AutoCompleteAssetValues(string term)
        {
            var arrStr = _NounModifiService.AutoSearchAssetValues(term);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

    }

}