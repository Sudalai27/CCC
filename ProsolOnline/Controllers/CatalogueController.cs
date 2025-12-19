using Newtonsoft.Json;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nest;
using Elasticsearch.Net;
using ProsolOnline.ViewModel;
using System.IO;
using System.Data;
using System.Net;

namespace ProsolOnline.Controllers
{
    [ValidateInput(false)]
    public class CatalogueController : Controller
    {
        private readonly ICatalogue _CatalogueService;
        private readonly ICharateristics _CharateristicService;
        private readonly IUserCreate _UserCreateService;
        private readonly INounModifier _Nounmodifier;
        private readonly ILogic _LogicService;
        private readonly IGeneralSettings _GeneralSettingService;

        public CatalogueController( ICharateristics CharaService,
            ICatalogue catalogueService, IUserCreate UserCreateService,INounModifier nounmodifier, ILogic LogicService,
            IGeneralSettings GeneralSettingService)
        {         
            _CharateristicService = CharaService;
            _CatalogueService = catalogueService;
            _UserCreateService = UserCreateService;
            _Nounmodifier = nounmodifier;
            _LogicService = LogicService;
            _GeneralSettingService = GeneralSettingService;

        }
        // GET: Catalogue
        [Authorize]
        public ActionResult Index(string itemId)
        {
           
                if (CheckAccess("Catalogue") == 1)
                    return View();
                else if (CheckAccess("Catalogue") == 0)
                    return View("Denied");
                else return View("Login");
           

        }
       
        [Authorize]
        public ActionResult Review()
        {
            if (CheckAccess("Review") == 1)
                return View();
            else if (CheckAccess("Review") == 0)
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
        public ActionResult Release()
        {
            if (CheckAccess("Release") == 1)
                return View();
            else if (CheckAccess("Release") == 0)
                return View("Denied");
            else return View("Login");          

        }
        [Authorize]
        public ActionResult Bulk()
        {
            if (CheckAccess("BulkUpload") == 1)
                return View();
            else if (CheckAccess("BulkUpload") == 0)
                return View("Denied");
            else return View("Login");

        }
        public ActionResult Pvdata()
        {
            // return View();
            if (CheckAccess("PV Data") == 1)
                return View();
            else if (CheckAccess("PV Data") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Catview()
        {
            return View();

        }
        [Authorize]
        public ActionResult Excelview()
        {         
                return View();
          
        }
        [Authorize]
        public ActionResult Releaseview()
        {          
            return View();
           
        }
        
        [Authorize]
        public JsonResult GetNounModifier(string Noun, string Modifier)
        {

            if (Noun != "null" && Modifier != "null" && Noun != null && Modifier != null)
            {
                var arrStr = _Nounmodifier.GetNounModifier(Noun, Modifier);
                if (arrStr != null)
                {
                    var AttributeList = _CharateristicService.GetAttributes().ToList();
                    var NM = new NounModifierModel();
                    NM._id = (arrStr != null) ? arrStr._id.ToString() : null;
                    NM.Noun = arrStr.Noun;
                    NM.Modifier = arrStr.Modifier;
                    NM.Nounabv = arrStr.Nounabv;
                    NM.NounDefinition = arrStr.NounDefinition;
                    NM.Modifierabv = arrStr.Modifierabv;
                    NM.ModifierDefinition = arrStr.ModifierDefinition;
                    NM.Formatted = arrStr.Formatted;
                    NM.FileData = arrStr.FileData;
                    var NMC_VM = new NounModifierVM();
                    NMC_VM.One_NounModifier = NM;

                    var arrChar = _CharateristicService.GetCharateristic(Noun, Modifier);
                    var lstChar = new List<NM_AttributesModel>();
                    foreach (Prosol_Charateristics nm_Char in arrChar)
                    {
                        var Chara = new NM_AttributesModel();
                        Chara.Noun = nm_Char.Noun;
                        Chara.Modifier = nm_Char.Modifier;
                        Chara.Characteristic = nm_Char.Characteristic;
                       // Chara.Abbrivation = nm_Char.Abbrivation;
                        Chara.Squence = nm_Char.Squence;
                        Chara.ShortSquence = nm_Char.ShortSquence;
                        Chara.Mandatory = nm_Char.Mandatory;
                        Chara.Definition = nm_Char.Definition;
                      //  Chara.Source = nm_Char.Source;
                      //  Chara.Values = nm_Char.Values;
                       // Chara.Values = nm_Char.Values;
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
        [Authorize]
        public JsonResult AutoCompleteVendor(string term)
        {

            var uomList = _CatalogueService.GetVendorList(term);
            var result = uomList.Select(i => new { i.Code, i.Name,i.Name2, i.Name3, i.Name4,i.AcquiredCompanyName,i.Acquiredby }).Distinct().ToList();           
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getcommcommlist(string sKey)
        {
            var srch = _CatalogueService.getCOMMCOMMList(sKey);
            var hsn = new List<Prosol_UNSPSC>();
            foreach (Prosol_UNSPSC C in srch)
            {
                var pro = new Prosol_UNSPSC();

                pro.Class = C.Class;
                pro.ClassTitle = C.ClassTitle;
                pro.Commodity = C.Commodity;
                pro.CommodityTitle = C.CommodityTitle;
                pro.SegmentTitle = C.SegmentTitle;
                pro.Segment = C.Segment;
                pro.FamilyTitle = C.FamilyTitle;
                pro.Family = C.Family;
                    

                hsn.Add(pro);
            }

            var jsonResult = Json(hsn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult InsertData()
        {

            var files = Request.Files.Count > 0 ? Request.Files : null;
            var itemsts = Request.Form["itemsts"];
            var pvsts = Request.Form["PVStatus"];
            //var type1 = Request.Form["type"];
            //var Type2 = JsonConvert.DeserializeObject(type1).ToString();

            var cat = Request.Form["cat"];
            var Equipment = Request.Form["Equ"];
            var catModel = JsonConvert.DeserializeObject<CatalogueModel>(cat);
            var EquModel = JsonConvert.DeserializeObject<Equipment>(Equipment);         
            var Charas = Request.Form["attri"];
            var ListCharas = JsonConvert.DeserializeObject<List<AttributeList>>(Charas);

            var ERPInfo = Request.Form["ERP"];
            var mdl = JsonConvert.DeserializeObject<ERPInfoModel>(ERPInfo);


            //var generalinfo = Request.Form["Generalinfo"];
            //var Generalinfomdl = JsonConvert.DeserializeObject<GeneralinfoModel>(generalinfo);

            //var plantinfo = Request.Form["Plantinfo"];
            //var Plantinfomdl = JsonConvert.DeserializeObject<PlantinfoModel>(plantinfo);

            //var MRPdata = Request.Form["MRPdata"];
            //var MRPdatamdl = JsonConvert.DeserializeObject<MRPdataModel>(MRPdata);

            //var Salesdata = Request.Form["Salesinfo"];
            //var salesmdl = JsonConvert.DeserializeObject<SalesinfoModel>(Salesdata);

            var vendorsuppliers = Request.Form["vendorsupplier"];
            var vendorsuppliersList = JsonConvert.DeserializeObject<List<Vendorsupplier>>(vendorsuppliers);

            var FileList = Request.Form["Attachments"];

          //  FileList.SaveAs(System.IO.Path.Combine("D:/enventure/images", cat.Itemcode.ToString() + imgid.ToString()));
            var attList = JsonConvert.DeserializeObject<List<AttachmentModel>>(FileList);

            string stus = Request.Form["sts"];
            string HSNID = Request.Form["HSNID"];
            
            int res = 0;
            try
            {
                //catModel.Itemcode = "10021";
                if (catModel == null)
                    catModel = new CatalogueModel();
                TryUpdateModel(catModel);

                if (ModelState.IsValid)
                {

                    
                    res = CreateData(catModel, ListCharas, EquModel, vendorsuppliersList, mdl, attList, files, stus, itemsts, pvsts, HSNID);
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
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                return Json(new
                {
                    success = false,
                    errors = e.ToString()
                }, JsonRequestBehavior.AllowGet);
               // res =-1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        protected virtual int CreateData(CatalogueModel cat, List<AttributeList> ListAttributes,
            Equipment EquModel, List<Vendorsupplier> vendorsuppliersList,
            ERPInfoModel mdl, List<AttachmentModel> attList, HttpFileCollectionBase files, string stus, string itemsts, string pvsts, string HSNID)
        {
            int res = 0;
            var proCat = new Prosol_Datamaster();

            cat.Itemcode = cat.Itemcode != null ? cat.Itemcode : generateRequest();
            cat.Materialcode = cat.Materialcode != null ? cat.Materialcode : null;
            proCat.exMaterialcode = cat.exMaterialcode;

            //Attribute
            proCat.Type = Convert.ToString(cat.Exchk);
            var lstCharateristics = new List<Prosol_AttributeList>();
            foreach (AttributeList LstAtt in ListAttributes)
            {
                var AttrMdl = new Prosol_AttributeList();
                AttrMdl.Characteristic = LstAtt.Characteristic;

                if (LstAtt.Value != null && LstAtt.Value != "")
                {
                   
                   
                    AttrMdl.Value = LstAtt.Value.Trim().ToUpper();
                    //if (itemsts == "0" || itemsts == "1")
                    //{
                    //    string returnVal = _CatalogueService.CheckValue1(cat.Noun, cat.Modifier, LstAtt.Characteristic, LstAtt.Value);
                    //    if (returnVal == "false")
                    //    {
                    //        res = 9;
                    //        break;
                    //    }

                    //}     
                    if (itemsts == "0" || itemsts == "1")
                    {
                        string returnVal = _CatalogueService.CheckValue1(cat.Noun, cat.Modifier, LstAtt.Characteristic, LstAtt.Value);
                        if (returnVal == "false")
                        {
                            AddValue(cat.Noun, cat.Modifier, LstAtt.Characteristic, LstAtt.Value, LstAtt.Value,null);
                        }

                    }
                    if (Convert.ToInt16(itemsts) >1 )
                    {
                        string returnVal = _CatalogueService.CheckValApprove(LstAtt.Value);
                        if (returnVal == "false" || returnVal=="")
                        {
                            AddValue(cat.Noun, cat.Modifier, LstAtt.Characteristic, LstAtt.Value, LstAtt.Value, null);
                            //res = 10;
                            //break;
                        }
                    }
                }
                else
                {
                    AttrMdl.Value = "";
                }

                AttrMdl.UOM = LstAtt.UOM;
                AttrMdl.Squence = LstAtt.Squence;
                AttrMdl.ShortSquence = LstAtt.ShortSquence;

                AttrMdl.Source = LstAtt.Source;
                AttrMdl.SourceUrl = LstAtt.SourceUrl;

                lstCharateristics.Add(AttrMdl);

            }
            //Vendorsuppliers
            var LstVendors = new List<Vendorsuppliers>();
            foreach (Vendorsupplier LstAtt in vendorsuppliersList)
            {
                if ((LstAtt.Name != null && LstAtt.Name != "") || (LstAtt.RefNo != "" && LstAtt.RefNo != null))
                {
                    var vndMdl = new Vendorsuppliers();
                    vndMdl.slno = LstAtt.slno;
                    vndMdl.Code = LstAtt.Code;
                    vndMdl.Name = LstAtt.Name;

                    if (Convert.ToInt16(itemsts) > -1 && LstAtt.Name!=null && LstAtt.Name != "")
                    {
                        var sObj = _GeneralSettingService.FINDVENDORMASTER(LstAtt.Name);
                        if (sObj == null)
                        {
                            res = 8;
                            break;
                        }
                    }

                    vndMdl.Type = LstAtt.Type;
                    vndMdl.RefNo = LstAtt.RefNo;
                    vndMdl.Refflag = LstAtt.Refflag;
                    vndMdl.s = LstAtt.s;
                    vndMdl.l = LstAtt.l;
                    vndMdl.shortmfr = LstAtt.shortmfr;

                    LstVendors.Add(vndMdl);
                }

            }


            //Images

            if (cat.AssetImages != null)
            {
                var Asset_imgs = new MatImage();
                if (cat.AssetImages.AssetImgs != null && cat.AssetImages.AssetImgs.Length > 0)
                    Asset_imgs.AssetImgs = cat.AssetImages.AssetImgs;
                if (cat.AssetImages.MatImgs != null && cat.AssetImages.MatImgs.Length > 0)
                    Asset_imgs.MatImgs = cat.AssetImages.MatImgs;
                if (cat.AssetImages.NamePlateImge != null && cat.AssetImages.NamePlateImge.Length > 0)
                    Asset_imgs.NamePlateImge = cat.AssetImages.NamePlateImge;
                if (cat.AssetImages.NamePlateImgeTwo != null && cat.AssetImages.NamePlateImgeTwo.Length > 0)
                    Asset_imgs.NamePlateImgeTwo = cat.AssetImages.NamePlateImgeTwo;
                if (cat.AssetImages.NamePlateText != null && cat.AssetImages.NamePlateText.Length > 0)
                    Asset_imgs.NamePlateText = cat.AssetImages.NamePlateText;
                if (cat.AssetImages.NamePlateTextTwo != null && cat.AssetImages.NamePlateTextTwo.Length > 0)
                    Asset_imgs.NamePlateTextTwo = cat.AssetImages.NamePlateTextTwo;
                if (cat.AssetImages.NameplateImgs != null && cat.AssetImages.NameplateImgs.Length > 0)
                    Asset_imgs.NameplateImgs = cat.AssetImages.NameplateImgs;
                if (cat.AssetImages.NewTagImage != null && cat.AssetImages.NewTagImage.Length > 0)
                    Asset_imgs.NewTagImage = cat.AssetImages.NewTagImage;
                if (cat.AssetImages.OldTagImage != null && cat.AssetImages.OldTagImage.Length > 0)
                    Asset_imgs.OldTagImage = cat.AssetImages.OldTagImage;


                proCat.AssetImages = Asset_imgs;

            }


            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

            //PV
            if (itemsts == "11")
            {
                res = 0;
                proCat.PVstatus = "Pending";
                proCat.Reworkcat = "PV";
                //var PV = new Prosol_UpdatedBy();
                //PV.UserId = usrInfo.Userid;
                //PV.Name = usrInfo.UserName;
                //PV.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //proCat.PVuser = PV;
            }

            if (res < 8)
            {
                //Plant
                var erpMdl = new Prosol_ERPInfo();
                if (mdl != null)
                {
                   
                    erpMdl._id = mdl._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(mdl._id);
                   
                    erpMdl.Itemcode = cat.Itemcode;

                    //General
                    erpMdl.Industrysector = mdl.Industrysector;
                    erpMdl.Materialtype = mdl.Materialtype;
                    erpMdl.BaseUOP = mdl.BaseUOP;
                    erpMdl.Unit_issue = mdl.Unit_issue;
                    erpMdl.AlternateUOM = mdl.AlternateUOM;
                    erpMdl.Inspectiontype = mdl.Inspectiontype;
                    erpMdl.Inspectioncode = mdl.Inspectioncode;
                    erpMdl.Division = mdl.Division;
                    erpMdl.Salesunit = mdl.Salesunit;


                    erpMdl.Industrysector_ = mdl.Industrysector_;
                    erpMdl.Materialtype_ = mdl.Materialtype_;
                    erpMdl.BaseUOP_ = mdl.BaseUOP_;
                    erpMdl.Unit_issue_ = mdl.Unit_issue_;
                    erpMdl.AlternateUOM_ = mdl.AlternateUOM_;
                    erpMdl.Inspectiontype_ = mdl.Inspectiontype_;
                    erpMdl.Inspectioncode_ = mdl.Inspectioncode_;
                    erpMdl.Division_ = mdl.Division_;
                    erpMdl.Salesunit_ = mdl.Salesunit_;

                    erpMdl.Numerator_ = mdl.Numerator_;
                    erpMdl.Denominator_ = mdl.Denominator_;
                    erpMdl.Oldmaterialno_ = mdl.Oldmaterialno_;

                    //Plant

                    erpMdl.Plant = mdl.Plant;
                    erpMdl.ProfitCenter = mdl.ProfitCenter;
                    erpMdl.StorageLocation = mdl.StorageLocation;
                    erpMdl.StorageBin = mdl.StorageBin;
                    erpMdl.ValuationClass = mdl.ValuationClass;
                    erpMdl.PriceControl = mdl.PriceControl;
                    erpMdl.ValuationCategory = mdl.ValuationCategory;
                    erpMdl.VarianceKey = mdl.VarianceKey;
                    erpMdl.BatchManagement = mdl.BatchManagement;
                    erpMdl.SnProfile = mdl.SnProfile;
                    erpMdl.Plant_ = mdl.Plant_;
                    erpMdl.ProfitCenter_ = mdl.ProfitCenter_;
                    erpMdl.StorageLocation_ = mdl.StorageLocation_;
                    erpMdl.StorageBin_ = mdl.StorageBin;
                    erpMdl.ValuationClass_ = mdl.ValuationClass_;
                    erpMdl.PriceControl_ = mdl.PriceControl_;
                    erpMdl.ValuationCategory_ = mdl.ValuationCategory_;
                    erpMdl.VarianceKey_ = mdl.VarianceKey_;

                    erpMdl.ValuationType_ = mdl.ValuationType_;
                    erpMdl.StandardPrice_ = mdl.StandardPrice_;
                    erpMdl.MovingAvgprice_ = mdl.MovingAvgprice_;

                    //Mrp data

                    erpMdl.MRPType = mdl.MRPType;
                    erpMdl.MRPController = mdl.MRPController;
                    erpMdl.LOTSize = mdl.LOTSize;
                    erpMdl.ProcurementType = mdl.ProcurementType;
                    erpMdl.PlanningStrgyGrp = mdl.PlanningStrgyGrp;
                    erpMdl.AvailCheck = mdl.AvailCheck;
                    erpMdl.ScheduleMargin = mdl.ScheduleMargin;


                    erpMdl.MRPType_ = mdl.MRPType_;
                    erpMdl.MRPController_ = mdl.MRPController_;
                    erpMdl.LOTSize_ = mdl.LOTSize_;
                    erpMdl.ProcurementType_ = mdl.ProcurementType_;
                    erpMdl.PlanningStrgyGrp_ = mdl.PlanningStrgyGrp_;
                    erpMdl.AvailCheck_ = mdl.AvailCheck_;
                    erpMdl.ScheduleMargin_ = mdl.ScheduleMargin_;

                    erpMdl.SafetyStock_ = mdl.SafetyStock_;
                    erpMdl.ReOrderPoint_ = mdl.ReOrderPoint_;
                    erpMdl.MaxStockLevel_ = mdl.MaxStockLevel_;
                    erpMdl.MinStockLevel_ = mdl.MinStockLevel_;

                    // Sales & Others
                    erpMdl.AccAsignmtCategory = mdl.AccAsignmtCategory;
                    erpMdl.TaxClassificationGroup = mdl.TaxClassificationGroup;
                    erpMdl.TaxClassificationGroup1 = mdl.TaxClassificationGroup1;
                    erpMdl.ItemCategoryGroup = mdl.ItemCategoryGroup;
                    erpMdl.SalesOrganization = mdl.SalesOrganization;
                    erpMdl.DistributionChannel = mdl.DistributionChannel;
                    erpMdl.MaterialStrategicGroup = mdl.MaterialStrategicGroup;
                    erpMdl.PurchasingGroup = mdl.PurchasingGroup;
                    erpMdl.PurchasingValueKey = mdl.PurchasingValueKey;

                    erpMdl.AccAsignmtCategory_ = mdl.AccAsignmtCategory_;
                    erpMdl.TaxClassificationGroup_ = mdl.TaxClassificationGroup_;
                    erpMdl.ItemCategoryGroup_ = mdl.ItemCategoryGroup_;
                    erpMdl.SalesOrganization_ = mdl.SalesOrganization_;
                    erpMdl.DistributionChannel_ = mdl.DistributionChannel_;
                    erpMdl.MaterialStrategicGroup_ = mdl.MaterialStrategicGroup_;
                    erpMdl.PurchasingGroup_ = mdl.PurchasingGroup_;
                    erpMdl.PurchasingValueKey_ = mdl.PurchasingValueKey_;

                    erpMdl.GoodsReceptprocessingTime_ = mdl.GoodsReceptprocessingTime_;
                    erpMdl.Quantity_ = mdl.Quantity_;

                    //new fields
                    erpMdl.DeliveringPlant = mdl.DeliveringPlant;
                      erpMdl.TaxClassificationDep = mdl.TaxClassificationDep;
                    erpMdl.TaxClassificationType = mdl.TaxClassificationType;
                    erpMdl.TaxClassificationDep1 = mdl.TaxClassificationDep1;
                    erpMdl.TaxClassificationType1 = mdl.TaxClassificationType1;
                    erpMdl.TransportationGroup = mdl.TransportationGroup;
                    erpMdl.LoadingGroup = mdl.LoadingGroup;
                    erpMdl.SalesText = mdl.SalesText;
                    erpMdl.OrderUnit = mdl.OrderUnit;
                    erpMdl.AutomaticPO = mdl.AutomaticPO;
                    erpMdl.PurchaseOrderText = mdl.PurchaseOrderText;
                    erpMdl.PlannedDeliveryTime = mdl.PlannedDeliveryTime;


                    erpMdl.DeliveringPlant_ = mdl.DeliveringPlant_;
               
                    erpMdl.TransportationGroup_ = mdl.TransportationGroup_;
                    erpMdl.LoadingGroup_ = mdl.LoadingGroup_;
                    erpMdl.SalesText_ = mdl.SalesText_;
                    erpMdl.OrderUnit_ = mdl.OrderUnit_;
                    erpMdl.AutomaticPO_ = mdl.AutomaticPO_;
                    erpMdl.PurchaseOrderText_ = mdl.PurchaseOrderText_;
                    erpMdl.PlannedDeliveryTime_ = mdl.PlannedDeliveryTime_;


                    erpMdl.Plant = mdl.Plant;
                    erpMdl.ProfitCenter = mdl.ProfitCenter;

                    erpMdl.Materialtype = mdl.Materialtype;
                    erpMdl.Materialtype_ = mdl.Materialtype_;
                    erpMdl.MaterialStrategicGroup = mdl.MaterialStrategicGroup;
                    erpMdl.MaterialStrategicGroup_ = mdl.MaterialStrategicGroup_;
                    erpMdl.MRPType = mdl.MRPType;
                    erpMdl.MRPType_ = mdl.MRPType_;

                    erpMdl.SafetyStock_ = mdl.SafetyStock_;
                    erpMdl.ReOrderPoint_ = mdl.ReOrderPoint_;
                    erpMdl.MaxStockLevel_ = mdl.MaxStockLevel_;
                    erpMdl.MinStockLevel_ = mdl.MinStockLevel_;

                    erpMdl.StorageBin = mdl.StorageBin;
                    erpMdl.StorageBin2 = mdl.StorageBin2;
                    erpMdl.StorageBin3 = mdl.StorageBin3;
                    erpMdl.StorageBin4 = mdl.StorageBin4;
                    erpMdl.StorageBin5 = mdl.StorageBin5;
                    erpMdl.Price_Unit = mdl.Price_Unit;
                    erpMdl.Price_Unit2 = mdl.Price_Unit2;
                    erpMdl.Price_Unit3 = mdl.Price_Unit3;
                    erpMdl.Price_Unit4 = mdl.Price_Unit4;
                    erpMdl.Price_Unit5 = mdl.Price_Unit5;
                    erpMdl.Quantity_ = mdl.Quantity_;
                    erpMdl.Quantity2_ = mdl.Quantity2_;
                    erpMdl.Quantity3_ = mdl.Quantity3_;
                    erpMdl.Quantity4_ = mdl.Quantity4_;
                    erpMdl.Quantity5_ = mdl.Quantity5_;
                    erpMdl.Currency = mdl.Currency;
                    erpMdl.Currency2 = mdl.Currency2;
                    erpMdl.Currency3 = mdl.Currency3;
                    erpMdl.Currency4 = mdl.Currency4;
                    erpMdl.Currency5 = mdl.Currency5;
                    erpMdl.StorageLocation = mdl.StorageLocation;
                    erpMdl.StorageLocation_ = mdl.StorageLocation_;
                    erpMdl.StorageLocation2 = mdl.StorageLocation2;
                    erpMdl.StorageLocation2_ = mdl.StorageLocation2_;
                    erpMdl.StorageLocation3 = mdl.StorageLocation3;
                    erpMdl.StorageLocation3_ = mdl.StorageLocation3_;
                    erpMdl.StorageLocation4 = mdl.StorageLocation4;
                    erpMdl.StorageLocation4_ = mdl.StorageLocation4_;
                    erpMdl.StorageLocation5 = mdl.StorageLocation5;
                    erpMdl.StorageLocation5_ = mdl.StorageLocation5_;

                    _CatalogueService.WriteERPInfo(erpMdl);
                }



                //Equipment
                var Equi_mdl = new Equipments();
                if (EquModel != null)
                {
                    Equi_mdl.Name = EquModel.Name;
                    Equi_mdl.Manufacturer = EquModel.Manufacturer;
                    Equi_mdl.Modelno = EquModel.Modelno;
                    Equi_mdl.Tagno = EquModel.Tagno;
                    Equi_mdl.Serialno = EquModel.Serialno;
                    Equi_mdl.Additionalinfo = EquModel.Additionalinfo;
                    Equi_mdl.SuperiorEquipment = EquModel.SuperiorEquipment;
                    Equi_mdl.EMS = EquModel.EMS;
                    Equi_mdl.ENS = EquModel.ENS;

                    proCat.Equipment = Equi_mdl;
                }

               


                //Attachments 
                var lstAtt = new List<Prosol_Attachment>();
                if (attList != null)
                {
                    foreach (AttachmentModel atlst in attList)
                    {
                        if (atlst.FileName != null && atlst.FileName != "")
                        {
                            var attlst = new Prosol_Attachment();

                            attlst.Itemcode = cat.Itemcode;
                            attlst.Title = atlst.Title;
                           // attlst.UserId = Convert.ToString(Session["UserId"]);
                            attlst.FileName = atlst.FileName;
                            attlst.FileSize = atlst.FileSize;
                            attlst.FileId = atlst.FileId;
                            attlst.Date_ts = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                            lstAtt.Add(attlst);
                        }
                    }


                    //  FileList.SaveAs(System.IO.Path.Combine(Server.MapPath("D:/enventure/images"), cat.Itemcode.ToString() + imgid.ToString()));
                }



                _CatalogueService.WriteAttachment(lstAtt, files);

                var autormrk = "TOOL-REMARK: PROVIDE ";
                if (proCat.Type == "True")
                {
                    var arrChar = _CharateristicService.GetCharateristic(cat.Noun, cat.Modifier);
                    var lstChar = new List<NM_AttributesModel>();
                    foreach (Prosol_Charateristics nm_Char in arrChar)
                    {
                        var Chara = new NM_AttributesModel();
                        Chara.Characteristic = nm_Char.Characteristic;
                        Chara.Mandatory = nm_Char.Mandatory;
                        Chara.Values = nm_Char.Values;
                        lstChar.Add(Chara);
                    }
                  
                    foreach (NM_AttributesModel x in lstChar)
                    {
                        foreach (Prosol_AttributeList y in lstCharateristics)
                        {
                            if (x.Characteristic == y.Characteristic && x.Mandatory == "Yes" && (y.Value == null || y.Value == ""))
                            {
                                autormrk += x.Characteristic + ",";
                            }

                        }
                    }
                }

                files = null;

                proCat.Vendorsuppliers = LstVendors;

                proCat._id = cat._id != null ? new MongoDB.Bson.ObjectId(cat._id) : new MongoDB.Bson.ObjectId();

                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = Convert.ToInt16(itemsts);
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;
                proCat.Noun = cat.Noun;
                proCat.exNoun = cat.exNoun;
                proCat.Modifier = cat.Modifier;
                proCat.exModifier = cat.exModifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Shortdesc_ = cat.Shortdesc_;
                proCat.Longdesc = cat.Longdesc;
                proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Soureurl = cat.Soureurl;
                proCat.Junk = cat.Junk;
                proCat.Manufacturercode = cat.Manufacturercode;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;
                proCat.Application = cat.Application;
                proCat.Drawingno = cat.Drawingno;
                proCat.Referenceno = cat.Referenceno;
                if(cat.Exchk)
                proCat.RevRemarks = autormrk+ " CATALOGUER-REMARK:" + cat.Remarks;
                else proCat.Remarks = cat.Remarks;

                proCat.RevRemarks = cat.RevRemarks;
                proCat.RelRemarks = cat.RelRemarks;
                proCat.Unspsc = cat.Unspsc;
                proCat.UOM = cat.UOM;
                proCat.Maincode = cat.Maincode;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;
                proCat.MissingValue = cat.MissingValue;
                proCat.EnrichedValue = cat.EnrichedValue;
                proCat.exMissingValue = cat.exMissingValue;
                proCat.Unmap = cat.Unmap;
                proCat.HSNID = cat.HSNID;
                proCat.exManufacturer = cat.exManufacturer;
                proCat.exPartno = cat.exPartno;
                proCat.Stock_Status = cat.Stock_Status;
                proCat.PVRemarks = cat.PVRemarks;

                proCat.Characteristics = lstCharateristics;
                var lstCharateristics1 = new List<Prosol_AttributeList>();
                if (cat.exCharacteristics != null)
                {
                    foreach (AttributeList pattri in cat.exCharacteristics)
                    {
                        var AttrMdl = new Prosol_AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        AttrMdl.Source = pattri.Source;
                        AttrMdl.SourceUrl = pattri.SourceUrl;
                        lstCharateristics1.Add(AttrMdl);
                    }
                }
                proCat.exCharacteristics = lstCharateristics1;
                //pvdata
                proCat.System_Balance = cat.System_Balance;
                proCat.Quantity_as_on_Date = cat.Quantity_as_on_Date;
                // DateTime DATE = cat.Quantity_as_on_Date.ToString("yyyy/MM/dd");
                //  proCat.Quantity_as_on_Date = DATE;
                //proCat.Quantity_as_on_Date = DateTime.Parse(cat.Quantity_as_on_Date.ToString()).ToString("MMM dd yyyy hh:mm tt");

                proCat.Stock_Quantity = cat.Stock_Quantity;

                proCat.No_of_Item_Aginst_PV_Obs = cat.No_of_Item_Aginst_PV_Obs;
                proCat.Physical_Observation = cat.Physical_Observation;
                proCat.Expired_Date = cat.Expired_Date;
                proCat.Storage_Bin1 = cat.StorageBin1;
                proCat.Storage_Location1 = cat.StorageLocation1;
                proCat.GR_Date = cat.GR_Date;
                proCat.No_of_Items_Expired = cat.No_of_Items_Expired;
                proCat.Bin_Updation_Miss_Placed = cat.Bin_Updation_Miss_Placed;
                proCat.Shelf_Life = cat.Shelf_Life;
                proCat.PVstatus = pvsts;
                proCat.Specification = cat.Specification;
                proCat.RelRemarks = cat.RelRemarks;

                proCat.Observations = new List<PhyObs>();
                if (cat.Observations != null && cat.Observations.Count() > 0)
                {
                    foreach (var obs in cat.Observations)
                    {
                        var obj = new PhyObs();
                        obj.Observation = obs.Observation;
                        obj.Qty = obs.Qty;
                        proCat.Observations.Add(obj);
                    }
                }
                proCat.DataCollection = new List<StorageBin>();
                if (cat.DataCollection != null && cat.DataCollection.Count() > 0)
                {
                    foreach (var obs in cat.DataCollection)
                    {
                        var obj = new StorageBin();
                        obj.Observation = obs.Observation;
                        obj.Qty = obs.Qty;
                        obj.sQty = obs.sQty;
                        obj.dQty = obs.dQty;
                        proCat.DataCollection.Add(obj);
                    }
                }
                proCat.StorageLocations = new List<StorageLoc>();
                if (cat.StorageLocations != null && cat.StorageLocations.Count() > 0)
                {

                    foreach (var sl in cat.StorageLocations)
                    {
                        var obj = new StorageLoc();
                        obj.StorageLocation = sl.StorageLocation;
                        obj.StorageLocation_ = sl.StorageLocation_;
                        obj.DataCollection = new List<StorageBin>();
                        if (sl.DataCollection != null && sl.DataCollection.Count() > 0)
                        {
                            foreach (var dc in sl.DataCollection)
                            {
                                var dObj = new StorageBin();
                                dObj.Observation = dc.Observation;
                                dObj.sQty = dc.sQty;
                                dObj.dQty = dc.dQty;
                                obj.DataCollection.Add(dObj);
                            }
                        }
                        proCat.StorageLocations.Add(obj);
                    }
                }
                proCat.barcode_status = cat.barcode_status;
                proCat.barcode_rm = cat.barcode_rm;
                proCat.des_status = cat.des_status;
                proCat.des_remark = cat.des_remark;
                proCat.Subsubcode = cat.Subsubcode;

                if (itemsts == "1" || itemsts == "3")
                {
                    if (cat.PVuser != null)
                    {
                        var pv = new Prosol_UpdatedBy();
                        pv.UserId = cat.PVuser.UserId;
                        pv.Name = cat.PVuser.Name;
                        pv.UpdatedOn = cat.PVuser.UpdatedOn;
                        proCat.PVuser = pv;
                    }
                    proCat.PVstatus = cat.PVstatus;
                }

                var pvLogs = new List<Prosol_UpdatedBy>();
                if (cat.PVLog != null && cat.PVLog.Count() > 0)
                {
                    foreach (var pl in cat.PVLog)
                    {
                        var log = new Prosol_UpdatedBy();
                        log.UserId = pl.UserId;
                        log.Name = pl.Name;
                        log.UpdatedOn = pl.UpdatedOn;
                        pvLogs.Add(log);
                    }
                    proCat.PVLog = pvLogs;
                }

                string tmpStr = "";
                foreach (TargetExn ent in usrInfo.Roles)
                {
                    if (ent.Name == "Cataloguer")
                    {
                        tmpStr = ent.TargetId;

                        var relUname = _UserCreateService.getimage(tmpStr);
                        string uID = tmpStr;
                        string uName = relUname.UserName;

                        var cataloguer = new Prosol_UpdatedBy();
                        cataloguer.UserId = uID;
                        cataloguer.Name = uName;
                        cataloguer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        proCat.Catalogue = cataloguer;

                    }
                }
              

                return _CatalogueService.WriteData(proCat, stus);
            }
            else return res;

          
        }
        private string generateRequest()
        {
            string itmCode = "";
            var ICode =_CatalogueService.getItem();
        
            if (ICode!="")
            {

              
               long serialinr = Convert.ToInt64(ICode);
                serialinr++;
                string addincr = "";
                switch (serialinr.ToString().Length)
                {
                    case 1:
                        addincr = "0000" + serialinr;
                        break;
                    case 2:
                        addincr = "000" + serialinr;
                        break;
                    case 3:
                        addincr = "00" + serialinr;
                        break;
                    case 4:
                        addincr = "0" + serialinr;
                        break;
                    default:
                        addincr = serialinr.ToString();
                        break;

                }
                itmCode = addincr;

            }
            else
            {
                itmCode = "00001";
            }
            return itmCode;
        }
        [Authorize]
        public JsonResult GetSingleItem(string itemcode)
        {
           // int sts = 0;
           // int sts1 = 1;
            var cat = _CatalogueService.GetSingleItem(itemcode);
           

            var proCat = new CatalogueModel();
            if (cat != null)
            {
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        AttrMdl.Source = pattri.Source;
                        AttrMdl.SourceUrl = pattri.SourceUrl;
                        if (cat.ItemStatus > 1 && AttrMdl.Value != "" && AttrMdl.Value != null)
                        {
                            string abbrVal = _CatalogueService.CheckValue(cat.Noun, cat.Modifier, pattri.Characteristic, pattri.Value);
                            if (abbrVal != null)
                                AttrMdl.Abbrevated = abbrVal;
                            else AttrMdl.Abbrevated = "false";


                            string returnVal = _CatalogueService.CheckValApprove(pattri.Value);
                            if (returnVal == "false")
                            {
                                AttrMdl.Approve = true;
                            }
                        }
                        else
                        {
                            AttrMdl.Abbrevated = "false";
                            AttrMdl.Approve = false;
                        }
                        lstCharateristics.Add(AttrMdl);
                    }
                }
                var lstCharateristics1 = new List<AttributeList>();
                if (cat.exCharacteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.exCharacteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        AttrMdl.Source = pattri.Source;
                        AttrMdl.SourceUrl = pattri.SourceUrl;
                        if (cat.ItemStatus > 1 && AttrMdl.Value != "" && AttrMdl.Value != null)
                        {
                            string abbrVal = _CatalogueService.CheckValue(cat.Noun, cat.Modifier, pattri.Characteristic, pattri.Value);
                            if (abbrVal != null)
                                AttrMdl.Abbrevated = abbrVal;
                            else AttrMdl.Abbrevated = "false";


                            string returnVal = _CatalogueService.CheckValApprove(pattri.Value);
                            if (returnVal == "false")
                            {
                                AttrMdl.Approve = true;
                            }
                        }
                        else
                        {
                            AttrMdl.Abbrevated = "false";
                            AttrMdl.Approve = false;
                        }
                        lstCharateristics1.Add(AttrMdl);
                    }
                }

                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;


                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                    Equi_mdl.SuperiorEquipment = cat.Equipment.SuperiorEquipment;
                    Equi_mdl.EMS = cat.Equipment.EMS;
                    Equi_mdl.ENS = cat.Equipment.ENS;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;

                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        vndMdl.s = vndrs.s;
                        vndMdl.l = vndrs.l;
                        vndMdl.shortmfr = vndrs.shortmfr;
                        LstVendors.Add(vndMdl);
                    }

                }

                proCat.Vendorsuppliers = LstVendors;
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.exMaterialcode = cat.exMaterialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;

                proCat.Noun = cat.Noun;
                proCat.exNoun = cat.exNoun;
                proCat.Modifier = cat.Modifier;
                proCat.exModifier = cat.exModifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Shortdesc_ = cat.Shortdesc_;
                proCat.Longdesc = cat.Longdesc;
                //new
                if (cat.Longdesc != null)
                {
                    proCat.RepeatedValue = RepeatedValue(proCat.Longdesc);
                }
                proCat.Additionalinfo = cat.Additionalinfo;
                proCat.exAdditionalinfo = cat.exAdditionalinfo;
                proCat.Soureurl = cat.Soureurl;
                proCat.Junk = cat.Junk;
                proCat.Manufacturercode = cat.Manufacturercode;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.exManufacturer = cat.exManufacturer;
                proCat.Partno = cat.Partno;
                proCat.exPartno = cat.exPartno;
                proCat.Partnodup = cat.Partnodup;
                proCat.Application = cat.Application;
                proCat.Drawingno = cat.Drawingno;
                proCat.Referenceno = cat.Referenceno;
                proCat.ItemStatus = cat.ItemStatus;
                if (cat.Remarks != null && cat.Remarks.Contains(" CATALOGUER-REMARK:"))
                {
                    proCat.Remarks = cat.Remarks.Substring(cat.Remarks.IndexOf(" CATALOGUER-REMARK:") + 19);
                    //   proCat.Junk = cat.Remarks.Substring(cat.Remarks.IndexOf("AutoGenerated:"));
                    proCat.Junk = cat.Remarks.Substring(0, cat.Remarks.IndexOf(" CATALOGUER-REMARK:"));
                    proCat.Junk = proCat.Junk.TrimEnd(',');
                }
                else proCat.Remarks = cat.Remarks;



                proCat.RevRemarks = cat.RevRemarks;
                proCat.RelRemarks = cat.RelRemarks;
                proCat.PVRemarks = cat.PVRemarks;
                proCat.Unspsc = cat.Unspsc;
                proCat.UOM = cat.UOM;
                proCat.Maincode = cat.Maincode;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;
                proCat.MissingValue = cat.MissingValue;
                proCat.EnrichedValue = cat.EnrichedValue;
                proCat.exMissingValue = cat.exMissingValue;
                proCat.Unmap = cat.Unmap;
                proCat.Stock_Status = cat.Stock_Status;
                //proCat.Exchk = Convert.ToBoolean(cat.Type);

                //pv data
                proCat.System_Balance = cat.System_Balance;

                proCat.Quantity_as_on_Date = cat.Quantity_as_on_Date;


                proCat.Stock_Quantity = cat.Stock_Quantity;

                proCat.No_of_Item_Aginst_PV_Obs = cat.No_of_Item_Aginst_PV_Obs;
                proCat.Physical_Observation = cat.Physical_Observation;
                proCat.Expired_Date = cat.Expired_Date;
                proCat.StorageBin1 = cat.Storage_Bin1;
                proCat.StorageLocation1 = cat.Storage_Location1;
                proCat.GR_Date = cat.GR_Date;
                proCat.No_of_Items_Expired = cat.No_of_Items_Expired;
                proCat.Bin_Updation_Miss_Placed = cat.Bin_Updation_Miss_Placed;
                proCat.Shelf_Life = cat.Shelf_Life;
                proCat.PVstatus = cat.PVstatus;
                proCat.Specification = cat.Specification;


                proCat.Observations = new List<VPhyObs>();
                if(cat.Observations != null && cat.Observations.Count() > 0)
                {
                    foreach (var obs in cat.Observations)
                    {
                        var obj = new VPhyObs();
                        obj.Observation = obs.Observation;
                        obj.Qty = obs.Qty;
                        proCat.Observations.Add(obj);
                    }
                }
                proCat.DataCollection = new List<VStorageBin>();
                if(cat.DataCollection != null && cat.DataCollection.Count() > 0)
                {
                    foreach (var obs in cat.DataCollection)
                    {
                        var obj = new VStorageBin();
                        obj.Observation = obs.Observation;
                        obj.Qty = obs.Qty;
                        obj.sQty = obs.sQty;
                        obj.dQty = obs.dQty;
                        proCat.DataCollection.Add(obj);
                    }
                }

                proCat.StorageLocations = new List<VStorageLoc>();
                if (cat.StorageLocations != null && cat.StorageLocations.Count() > 0)
                {

                    foreach (var sl in cat.StorageLocations)
                    {
                        var obj = new VStorageLoc();
                        obj.StorageLocation = sl.StorageLocation;
                        obj.StorageLocation_ = sl.StorageLocation_;
                        obj.DataCollection = new List<VStorageBin>();
                        if (sl.DataCollection != null && sl.DataCollection.Count() > 0)
                        {
                            foreach (var dc in sl.DataCollection)
                            {
                                var dObj = new VStorageBin();
                                dObj.Observation = dc.Observation;
                                dObj.sQty = dc.sQty;
                                dObj.dQty = dc.dQty;
                                obj.DataCollection.Add(dObj);
                            }
                        }
                        proCat.StorageLocations.Add(obj);
                    }
                }
                proCat.barcode_status = cat.barcode_status;
                proCat.barcode_rm = cat.barcode_rm;
                proCat.des_status = cat.des_status;
                proCat.des_remark = cat.des_remark;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;
                proCat.PVRemarks = cat.PVRemarks;

                if (cat.Catalogue != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Catalogue.Name;
                    proCat.Catalogue = updted;
                }

                if (cat.Review != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Review.Name;
                    proCat.Review = updted;
                }
                if (cat.Release != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Release.Name;
                    proCat.Release = updted;
                }
                if (cat.PVuser != null)
                {
                    var updted = new UpdatedBy();
                    updted.UserId = cat.PVuser.UserId;
                    updted.Name = cat.PVuser.Name;
                    updted.UpdatedOn = cat.PVuser.UpdatedOn;
                    proCat.PVuser = updted;
                }

                var pvLogs = new List<UpdatedBy>();
                if (cat.PVLog != null && cat.PVLog.Count() > 0)
                {
                    foreach (var pl in cat.PVLog)
                    {
                        var log = new UpdatedBy();
                        log.UserId = pl.UserId;
                        log.Name = pl.Name;
                        log.UpdatedOn = pl.UpdatedOn;
                        pvLogs.Add(log);
                    }
                    proCat.PVLog = pvLogs;
                }

                proCat.PVstatus = cat.PVstatus;
                proCat.Characteristics = lstCharateristics;
                proCat.exCharacteristics = lstCharateristics1;
                proCat.exManufacturer = cat.exManufacturer;
                proCat.exPartno = cat.exPartno;

                if (cat.AssetImages != null)
                {
                    var Asset_imgs = new MatImages();
                    if (cat.AssetImages.AssetImgs != null && cat.AssetImages.AssetImgs.Length > 0)
                        Asset_imgs.AssetImgs = cat.AssetImages.AssetImgs;
                    if (cat.AssetImages.MatImgs != null && cat.AssetImages.MatImgs.Length > 0)
                        Asset_imgs.MatImgs = cat.AssetImages.MatImgs;
                    if (cat.AssetImages.NamePlateImge != null && cat.AssetImages.NamePlateImge.Length > 0)
                        Asset_imgs.NamePlateImge = cat.AssetImages.NamePlateImge;
                    if (cat.AssetImages.NamePlateImgeTwo != null && cat.AssetImages.NamePlateImgeTwo.Length > 0)
                        Asset_imgs.NamePlateImgeTwo = cat.AssetImages.NamePlateImgeTwo;
                    if (cat.AssetImages.NamePlateText != null && cat.AssetImages.NamePlateText.Length > 0)
                        Asset_imgs.NamePlateText = cat.AssetImages.NamePlateText;
                    if (cat.AssetImages.NamePlateTextTwo != null && cat.AssetImages.NamePlateTextTwo.Length > 0)
                        Asset_imgs.NamePlateTextTwo = cat.AssetImages.NamePlateTextTwo;
                    if (cat.AssetImages.NameplateImgs != null && cat.AssetImages.NameplateImgs.Length > 0)
                        Asset_imgs.NameplateImgs = cat.AssetImages.NameplateImgs;
                    if (cat.AssetImages.NewTagImage != null && cat.AssetImages.NewTagImage.Length > 0)
                        Asset_imgs.NewTagImage = cat.AssetImages.NewTagImage;
                    if (cat.AssetImages.OldTagImage != null && cat.AssetImages.OldTagImage.Length > 0)
                        Asset_imgs.OldTagImage = cat.AssetImages.OldTagImage;


                    proCat.AssetImages = Asset_imgs;

                }
            }

            return this.Json(proCat, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetERPInfo(string itemcode)
        {
            var mdl = _CatalogueService.GetERPInfo(itemcode);

            var erpMdl = new ERPInfoModel();
            if (mdl != null)
            {
                erpMdl._id = mdl._id.ToString();
               // erpMdl.Itemcode = mdl.Itemcode;

                //General
                erpMdl.Industrysector = mdl.Industrysector;
                erpMdl.Materialtype = mdl.Materialtype;
                erpMdl.BaseUOP = mdl.BaseUOP;
                erpMdl.Unit_issue = mdl.Unit_issue;
                erpMdl.AlternateUOM = mdl.AlternateUOM;               
                erpMdl.Inspectiontype = mdl.Inspectiontype;
                erpMdl.Inspectioncode = mdl.Inspectioncode;              
                erpMdl.Division = mdl.Division;
                erpMdl.Salesunit = mdl.Salesunit;

              
                erpMdl.Numerator_ = mdl.Numerator_;
                erpMdl.Denominator_ = mdl.Denominator_;              
                erpMdl.Oldmaterialno_ = mdl.Oldmaterialno_;

                //Plant

                erpMdl.ValuationClass = mdl.ValuationClass;              
                erpMdl.PriceControl = mdl.PriceControl;               
                erpMdl.ValuationCategory = mdl.ValuationCategory;
                erpMdl.VarianceKey = mdl.VarianceKey;
                erpMdl.BatchManagement = mdl.BatchManagement;
                erpMdl.SnProfile = mdl.SnProfile;

                erpMdl.ValuationType_ = mdl.ValuationType_;              
                erpMdl.StandardPrice_ = mdl.StandardPrice_;
                erpMdl.MovingAvgprice_ = mdl.MovingAvgprice_;

                //Mrp data

                erpMdl.MRPController = mdl.MRPController;
                erpMdl.LOTSize = mdl.LOTSize;              
                erpMdl.ProcurementType = mdl.ProcurementType;                          
                erpMdl.PlanningStrgyGrp = mdl.PlanningStrgyGrp;
                erpMdl.AvailCheck = mdl.AvailCheck;
                erpMdl.ScheduleMargin = mdl.ScheduleMargin;

                // Sales & Others
                erpMdl.AccAsignmtCategory = mdl.AccAsignmtCategory;
                erpMdl.TaxClassificationGroup = mdl.TaxClassificationGroup;
                erpMdl.TaxClassificationGroup1 = mdl.TaxClassificationGroup1;
                erpMdl.ItemCategoryGroup = mdl.ItemCategoryGroup;
                erpMdl.SalesOrganization = mdl.SalesOrganization;
                erpMdl.DistributionChannel = mdl.DistributionChannel;
                erpMdl.MaterialStrategicGroup = mdl.MaterialStrategicGroup;
                erpMdl.PurchasingGroup = mdl.PurchasingGroup;
                erpMdl.PurchasingValueKey = mdl.PurchasingValueKey;
                erpMdl.GoodsReceptprocessingTime_ = mdl.GoodsReceptprocessingTime_;
                erpMdl.Quantity_ = mdl.Quantity_;
                //new fields

                erpMdl.DeliveringPlant = mdl.DeliveringPlant;
                erpMdl.TaxClassificationDep = mdl.TaxClassificationDep;
                erpMdl.TaxClassificationType = mdl.TaxClassificationType;
                erpMdl.TaxClassificationDep1 = mdl.TaxClassificationDep1;
                erpMdl.TaxClassificationType1 = mdl.TaxClassificationType1;
                erpMdl.TransportationGroup = mdl.TransportationGroup;
                erpMdl.LoadingGroup = mdl.LoadingGroup;
                erpMdl.OrderUnit = mdl.OrderUnit;
                erpMdl.AutomaticPO = mdl.AutomaticPO;
                erpMdl.SalesText = mdl.SalesText;
                erpMdl.PurchaseOrderText = mdl.PurchaseOrderText;
                erpMdl.PlannedDeliveryTime = mdl.PlannedDeliveryTime;


                erpMdl.DeliveringPlant_ = mdl.DeliveringPlant_;
             
                erpMdl.LoadingGroup_ = mdl.LoadingGroup_;
                erpMdl.OrderUnit_ = mdl.OrderUnit_;
                erpMdl.AutomaticPO_ = mdl.AutomaticPO_;


                erpMdl.Plant = mdl.Plant;
                erpMdl.ProfitCenter = mdl.ProfitCenter;

                erpMdl.Materialtype = mdl.Materialtype;
                erpMdl.Materialtype_ = mdl.Materialtype_;
                erpMdl.MaterialStrategicGroup = mdl.MaterialStrategicGroup;
                erpMdl.MaterialStrategicGroup_ = mdl.MaterialStrategicGroup_;
                erpMdl.MRPType = mdl.MRPType;
                erpMdl.MRPType_ = mdl.MRPType_;

                erpMdl.SafetyStock_ = mdl.SafetyStock_;
                erpMdl.ReOrderPoint_ = mdl.ReOrderPoint_;
                erpMdl.MaxStockLevel_ = mdl.MaxStockLevel_;
                erpMdl.MinStockLevel_ = mdl.MinStockLevel_;

                erpMdl.StorageBin = mdl.StorageBin;
                erpMdl.StorageBin2 = mdl.StorageBin2;
                erpMdl.StorageBin3 = mdl.StorageBin3;
                erpMdl.StorageBin4 = mdl.StorageBin4;
                erpMdl.StorageBin5 = mdl.StorageBin5;
                erpMdl.Price_Unit = mdl.Price_Unit;
                erpMdl.Price_Unit2 = mdl.Price_Unit2;
                erpMdl.Price_Unit3 = mdl.Price_Unit3;
                erpMdl.Price_Unit4 = mdl.Price_Unit4;
                erpMdl.Price_Unit5 = mdl.Price_Unit5;
                erpMdl.Quantity_ = mdl.Quantity_;
                erpMdl.Quantity2_ = mdl.Quantity2_;
                erpMdl.Quantity3_ = mdl.Quantity3_;
                erpMdl.Quantity4_ = mdl.Quantity4_;
                erpMdl.Quantity5_ = mdl.Quantity5_;
                erpMdl.Currency = mdl.Currency;
                erpMdl.Currency2 = mdl.Currency2;
                erpMdl.Currency3 = mdl.Currency3;
                erpMdl.Currency4 = mdl.Currency4;
                erpMdl.Currency5 = mdl.Currency5;
                erpMdl.StorageLocation = mdl.StorageLocation;
                erpMdl.StorageLocation_ = mdl.StorageLocation_;
                erpMdl.StorageLocation2 = mdl.StorageLocation2;
                erpMdl.StorageLocation2_ = mdl.StorageLocation2_;
                erpMdl.StorageLocation3 = mdl.StorageLocation3;
                erpMdl.StorageLocation3_ = mdl.StorageLocation3_;
                erpMdl.StorageLocation4 = mdl.StorageLocation4;
                erpMdl.StorageLocation4_ = mdl.StorageLocation4_;
                erpMdl.StorageLocation5 = mdl.StorageLocation5;
                erpMdl.StorageLocation5_ = mdl.StorageLocation5_;


            }
            return this.Json(erpMdl, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetGeneralinfo(string itemcode)
        //{
        //    var gen = _CatalogueService.GetGeneralinfo(itemcode);

        //    var genMdl = new GeneralinfoModel();
        //    if (gen != null)
        //    {
        //        genMdl._id = gen._id.ToString();
        //        genMdl.Itemcode = gen.Itemcode;
        //        genMdl.Industrysector = gen.Industrysector;
        //        genMdl.Materialtype = gen.Materialtype;
        //        genMdl.BaseUOP = gen.BaseUOP;
        //        genMdl.Unit_issue = gen.Unit_issue;
        //        genMdl.AlternateUOM = gen.AlternateUOM;
        //        genMdl.Numerator = gen.Numerator;
        //        genMdl.Denominator = gen.Denominator;
        //        genMdl.Inspectiontype = gen.Inspectiontype;
        //        genMdl.Inspectioncode = gen.Inspectioncode;
        //        genMdl.Oldmaterialno = gen.Oldmaterialno;
        //        genMdl.Division = gen.Division;
        //        genMdl.Salesunit = gen.Salesunit;               

        //    }
        //    return this.Json(genMdl, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetPlantinfo(string itemcode)
        //{
        //    var gen = _CatalogueService.GetPlantinfo(itemcode);

        //    var genMdl = new PlantinfoModel();
        //    if (gen != null)
        //    {
        //        genMdl._id = gen._id.ToString();
        //        genMdl.Itemcode = gen.Itemcode;
        //        genMdl.Plant = gen.Plant;
        //        genMdl.ProfitCenter = gen.ProfitCenter;
        //        genMdl.StorageLocation = gen.StorageLocation;
        //        genMdl.StorageBin = gen.StorageBin;
        //        genMdl.ValuationClass = gen.ValuationClass;
        //        genMdl.ValuationType = gen.ValuationType;
        //        genMdl.PriceControl = gen.PriceControl;
        //        genMdl.StandardPrice = gen.StandardPrice;
        //        genMdl.MovingAvgprice = gen.MovingAvgprice;
        //        genMdl.ValuationCategory = gen.ValuationCategory;
        //        genMdl.VarianceKey = gen.VarianceKey;

        //    }
        //    return this.Json(genMdl, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetMRPdata(string itemcode)
        //{
        //    var gen = _CatalogueService.GetMRPdata(itemcode);

        //    var mrpMdl = new MRPdataModel();
        //    if (gen != null)
        //    {
        //        mrpMdl._id = gen._id.ToString();
        //        mrpMdl.Itemcode = gen.Itemcode;
        //        mrpMdl.MRPType = gen.MRPType;
        //        mrpMdl.MRPController = gen.MRPController;
        //        mrpMdl.LOTSize = gen.LOTSize;
        //        mrpMdl.ReOrderPoint = gen.ReOrderPoint;
        //        mrpMdl.ProcurementType = gen.ProcurementType;
        //        mrpMdl.SafetyStock = gen.SafetyStock;
        //        mrpMdl.MaxStockLevel = gen.MaxStockLevel;
        //        mrpMdl.MinStockLevel = gen.MinStockLevel;
        //        mrpMdl.PlanningStrgyGrp = gen.PlanningStrgyGrp;
        //        mrpMdl.AvailCheck = gen.AvailCheck;
        //        mrpMdl.ScheduleMargin = gen.ScheduleMargin;

        //    }
        //    return this.Json(mrpMdl, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetSalesinfo(string itemcode)
        //{
        //    var gen = _CatalogueService.GetSalesinfo(itemcode);

        //    var salesMdl = new SalesinfoModel();
        //    if (gen != null)
        //    {
        //        salesMdl._id = gen._id.ToString();
        //        salesMdl.Itemcode = gen.Itemcode;
        //        salesMdl.AccAsignmtCategory = gen.AccAsignmtCategory;
        //        salesMdl.TaxClassificationGroup = gen.TaxClassificationGroup;
        //        salesMdl.ItemCategoryGroup = gen.ItemCategoryGroup;
        //        salesMdl.SalesOrganization = gen.SalesOrganization;
        //        salesMdl.DistributionChannel = gen.DistributionChannel;
        //        salesMdl.MaterialStrategicGroup = gen.MaterialStrategicGroup;
        //        salesMdl.PurchasingGroup = gen.PurchasingGroup;
        //        salesMdl.PurchasingValueKey = gen.PurchasingValueKey;
        //        salesMdl.GoodsReceptprocessingTime = gen.GoodsReceptprocessingTime;
        //        salesMdl.Quantity = gen.Quantity;

        //    }
        //    return this.Json(salesMdl, JsonRequestBehavior.AllowGet);
        //}
        [Authorize]
        public JsonResult GetEquip(string EName)
        {
            var LstE= _CatalogueService.GetEquip(EName);
            
            var attachMdlList = new List<Equipment>();
            foreach (Prosol_Datamaster atm in LstE)
            {
                var lMdl = new Equipment();
               if (atm.Equipment != null)
                {
                  if(atm.Equipment.Name != null || atm.Equipment.Name != " ")
                    {
                        lMdl.Name = atm.Equipment.Name;
                        attachMdlList.Add(lMdl);
                    }  
                   

                }
              
               

            }
           
            var distinctList = attachMdlList
                        .Select(m => new { m.Name })
                        .Distinct()
                        .ToList();
           
          
            return this.Json(distinctList, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetAttachment(string itemcode)
        {
            var LstAttch = _CatalogueService.GetAttachment(itemcode);
            var attachMdlList = new List<AttachmentModel>();
            foreach(Prosol_Attachment atm in LstAttch)
            {
                var lMdl = new AttachmentModel();
                lMdl._id = atm._id.ToString();               
                lMdl.Itemcode = atm.Itemcode;
                lMdl.Title = atm.Title;
                lMdl.FileName = atm.FileName;
                lMdl.FileSize = atm.FileSize;
                lMdl.FileId = atm.FileId;
                attachMdlList.Add(lMdl);

            }
            return this.Json(attachMdlList, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetDataList()
        {
            int sts = 0;
            int sts1 = 1;
            var dataList = _CatalogueService.GetDataList(sts, sts1,Convert.ToString(Session["userid"])).ToList();          

            var lstCatalogue = new List<CatalogueModel>();
        //    int saved = 0;
          //  int bal = 0;
            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();
                //var lstCharateristics = new List<AttributeList>();
                //if (cat.Characteristics != null)
                //{
                //    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                //    {
                //        var AttrMdl = new AttributeList();
                //        AttrMdl.Characteristic = pattri.Characteristic;
                //        AttrMdl.Value = pattri.Value;
                //        AttrMdl.UOM = pattri.UOM;
                //        AttrMdl.Squence = pattri.Squence;
                //        AttrMdl.ShortSquence = pattri.ShortSquence;
                //        lstCharateristics.Add(AttrMdl);
                //    }
                //}

                //var Equi_mdl = new Equipment();
                //if (cat.Equipment != null)
                //{
                //    Equi_mdl.Name = cat.Equipment.Name;
                //    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                //    Equi_mdl.Modelno = cat.Equipment.Modelno;
                //    Equi_mdl.Tagno = cat.Equipment.Tagno;
                //    Equi_mdl.Serialno = cat.Equipment.Serialno;
                //    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

                //    proCat.Equipment = Equi_mdl;
                //}

                //var LstVendors = new List<Vendorsupplier>();
                //if (cat.Vendorsuppliers != null)
                //{
                //    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                //    {
                //        var vndMdl = new Vendorsupplier();
                //        vndMdl.slno = vndrs.slno;
                //        vndMdl.Code = vndrs.Code;
                //        vndMdl.Name = vndrs.Name;
                //        vndMdl.Type = vndrs.Type;
                //        vndMdl.RefNo = vndrs.RefNo;
                //        LstVendors.Add(vndMdl);
                //    }
                //}

                //proCat.Vendorsuppliers = LstVendors;
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;            
            
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                if(proCat.Longdesc!="" && proCat.Longdesc != null)
                {
                    proCat.RepeatedValue = RepeatedValue(proCat.Longdesc);
                }
              
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;
                if (cat.PVuser != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.PVuser.Name;
                    updted.UpdatedOn = cat.PVuser.UpdatedOn;
                    proCat.PVuser = updted;
                }
                if (cat.Catalogue != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Catalogue.Name;
                    proCat.Catalogue = updted;
                }

                if (cat.Review != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Review.Name;
                    proCat.Review = updted;
                }
                if (cat.Release != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Release.Name;
                    proCat.Release = updted;
                }

                //proCat.Shortdesc = cat.Shortdesc;
                //proCat.Longdesc = cat.Longdesc;
                //proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Junk = cat.Junk;
                proCat.Unmap = cat.Unmap;
                proCat.Stock_Status = cat.Stock_Status;

                //proCat.Manufacturercode = cat.Manufacturercode;
                //proCat.Manufacturer = cat.Manufacturer;
                //proCat.Partno = cat.Partno;              
                //proCat.Application = cat.Application;
                //proCat.Drawingno = cat.Drawingno;
                //proCat.Referenceno = cat.Referenceno;
                //proCat.ItemStatus = cat.ItemStatus;
                //proCat.Remarks = cat.Remarks;
                //proCat.Characteristics = lstCharateristics;


                //if (cat.ItemStatus == 0)
                //    bal++;
                //else
                //    saved++;

                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;            
            return jsonResult;

            //return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetDataListpv(int page, int pageSize)
        {
            int sts = 0;
            int sts1 = 1;
            var dataList = _CatalogueService.GetDataListpv(page, pageSize,Convert.ToString(Session["userid"])).ToList();          

            var lstCatalogue = new List<CatalogueModel>();
        //    int saved = 0;
          //  int bal = 0;
            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();
                //var lstCharateristics = new List<AttributeList>();
                //if (cat.Characteristics != null)
                //{
                //    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                //    {
                //        var AttrMdl = new AttributeList();
                //        AttrMdl.Characteristic = pattri.Characteristic;
                //        AttrMdl.Value = pattri.Value;
                //        AttrMdl.UOM = pattri.UOM;
                //        AttrMdl.Squence = pattri.Squence;
                //        AttrMdl.ShortSquence = pattri.ShortSquence;
                //        lstCharateristics.Add(AttrMdl);
                //    }
                //}

                //var Equi_mdl = new Equipment();
                //if (cat.Equipment != null)
                //{
                //    Equi_mdl.Name = cat.Equipment.Name;
                //    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                //    Equi_mdl.Modelno = cat.Equipment.Modelno;
                //    Equi_mdl.Tagno = cat.Equipment.Tagno;
                //    Equi_mdl.Serialno = cat.Equipment.Serialno;
                //    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

                //    proCat.Equipment = Equi_mdl;
                //}

                //var LstVendors = new List<Vendorsupplier>();
                //if (cat.Vendorsuppliers != null)
                //{
                //    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                //    {
                //        var vndMdl = new Vendorsupplier();
                //        vndMdl.slno = vndrs.slno;
                //        vndMdl.Code = vndrs.Code;
                //        vndMdl.Name = vndrs.Name;
                //        vndMdl.Type = vndrs.Type;
                //        vndMdl.RefNo = vndrs.RefNo;
                //        LstVendors.Add(vndMdl);
                //    }
                //}

                //proCat.Vendorsuppliers = LstVendors;
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;            
            
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                if(proCat.Longdesc!="" && proCat.Longdesc != null)
                {
                    proCat.RepeatedValue = RepeatedValue(proCat.Longdesc);
                }
              
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;
                if (cat.PVuser != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.PVuser.Name;
                    updted.UpdatedOn = cat.PVuser.UpdatedOn;
                    proCat.PVuser = updted;
                }
                if (cat.Catalogue != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Catalogue.Name;
                    proCat.Catalogue = updted;
                }

                if (cat.Review != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Review.Name;
                    proCat.Review = updted;
                }
                if (cat.Release != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Release.Name;
                    proCat.Release = updted;
                }

                //proCat.Shortdesc = cat.Shortdesc;
                //proCat.Longdesc = cat.Longdesc;
                //proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Junk = cat.Junk;
                proCat.Unmap = cat.Unmap;
                proCat.Stock_Status = cat.Stock_Status;

                //proCat.Manufacturercode = cat.Manufacturercode;
                //proCat.Manufacturer = cat.Manufacturer;
                //proCat.Partno = cat.Partno;              
                //proCat.Application = cat.Application;
                //proCat.Drawingno = cat.Drawingno;
                //proCat.Referenceno = cat.Referenceno;
                //proCat.ItemStatus = cat.ItemStatus;
                //proCat.Remarks = cat.Remarks;
                //proCat.Characteristics = lstCharateristics;


                //if (cat.ItemStatus == 0)
                //    bal++;
                //else
                //    saved++;

                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;            
            return jsonResult;

            //return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult InsertDataList()
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
            var catDatalist = Request.Form["DataList"];
           // string uID = Request.Form["uId"];
           // string uName = Request.Form["uName"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);

         
            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();               
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {
                    if (mdl.ItemStatus == 1)
                    {
                        var proCat = new Prosol_Datamaster();
                        proCat._id = new MongoDB.Bson.ObjectId(mdl._id);                     
                        proCat.ItemStatus = 2;                                      
                        proCat.Itemcode = mdl.Itemcode;
                        proCat.Noun = mdl.Noun;
                        proCat.Modifier = mdl.Modifier;                       
                        proCat.UpdatedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                           

                        //Review 
                        var reviewer = new Prosol_UpdatedBy();
                        reviewer.UserId = uID;
                        reviewer.Name = uName;
                        reviewer.UpdatedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                        proCat.Review = reviewer;
                      
                        listdt.Add(proCat);
                    }
                }

                res = _CatalogueService.WriteDataList(listdt);
              //  runElasticSearch(listdt);
            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                res = 0;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetRejectCodee()
        {
            string username = Session["username"].ToString();

            var catDatalist = Request.Form["DataListt"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);

            var listdt = new List<Prosol_Datamaster>();
            foreach (CatalogueModel mdl in catModelList)
            {
                if (mdl.Catalogue.Name == username)
                {
                    var proCat = new Prosol_Datamaster();
                    proCat.Itemcode = mdl.Itemcode;

                    listdt.Add(proCat);
                }
                }

            if (listdt.Count > 0)
            {
                var result = _CatalogueService.GetCodeForRejectedItems(listdt);
                return this.Json(result.ToString() + " items has been deleted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Select items belongs to you", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRejectRevCodee()
        {
            string username = Session["username"].ToString();

            var catDatalist = Request.Form["DataListt"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);

            var listdt = new List<Prosol_Datamaster>();
            foreach (CatalogueModel mdl in catModelList)
            {
                if (mdl.Review.Name == username)
                {
                    var proCat = new Prosol_Datamaster();
                    proCat.Itemcode = mdl.Itemcode;                  
                    listdt.Add(proCat);
                }
            }

            if (listdt.Count > 0)
            {
                var result = _CatalogueService.GetCodeForRejectedItems(listdt);
                return this.Json(result.ToString() + " items has been deleted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Select items belongs to you", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRejectRelCodee()
        {
            string username = Session["username"].ToString();

            var catDatalist = Request.Form["DataListt"];
            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);

            var listdt = new List<Prosol_Datamaster>();
            foreach (CatalogueModel mdl in catModelList)
            {
                if (mdl.Release.Name == username)
                {
                    var proCat = new Prosol_Datamaster();
                    proCat.Itemcode = mdl.Itemcode;
                    listdt.Add(proCat);
                }
            }

            if (listdt.Count > 0)
            {
                var result = _CatalogueService.GetCodeForRejectedItems(listdt);
                return this.Json(result.ToString() + " items has been deleted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Select items belongs to you", JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public JsonResult Deletefile(string id, string imgId)
        {
            var res1 = _CatalogueService.GetDeletefile(id);
            string path = System.IO.Path.Combine(Server.MapPath("~/PDF_IMAGES/" + res1)); // , attlst.FileName); // Server.MapPath("files//" + file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }
            var res = _CatalogueService.Deletefile(id,imgId);
         
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        //public void Downloadfile(string fileName, string type, string imgId)
        //{

        //    byte[] bytearr= _CatalogueService.Downloadfile(imgId);

        //    Response.AddHeader("Content-type", type);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    Response.BinaryWrite(bytearr);
        //    Response.Flush();
        //    Response.End();
        //    // return this.Json("", JsonRequestBehavior.AllowGet);
        //}
        [Authorize]
        public void Downloadfile(string ItemId, string fName)
        {
            var ListItems = _CatalogueService.GetAttachment(ItemId).ToList();
            if (ListItems != null && ListItems.Count > 0)
            {
                foreach (var strNmae in ListItems)
                {
                    if (strNmae.FileName == fName)
                    {

                        byte[] bytearr = _CatalogueService.Downloadfile(strNmae.FileId);

                        Response.AddHeader("Content-type", strNmae.ContentType);
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + strNmae.FileName);
                        Response.BinaryWrite(bytearr);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            // return this.Json("", JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public void Downloadimages(string mat_code)
        {

            string[] split = mat_code.Split(new Char[] { ',', '\n','\t',' ' },
                                 StringSplitOptions.RemoveEmptyEntries);

            foreach (string x in split)
            {
                var ress = _CatalogueService.findattachmentss(x);

                if(ress.Count() > 0)
                {
                    foreach (Prosol_Attachment xx in ress)
                    {
                        if (xx.FileName.ToUpper().Contains(".PNG") || xx.FileName.ToUpper().Contains(".JPG") || xx.FileName.ToUpper().Contains(".JPEG"))
                        {
                            byte[] bytearr = _CatalogueService.Downloadfile(xx.FileId.ToString());

                          //  Response.AddHeader("Content-type", xx.ContentType);
                            //Response.AddHeader("Content-Disposition", "attachment; filename=" + xx.FileName);
                            Response.BinaryWrite(bytearr);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
         
            // return this.Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getReviewerList()       {

            var ReviewerList = _CatalogueService.getReviewerList().ToList();

            return this.Json(ReviewerList, JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        //public JsonResult checkPartno(string Partno, string icode)
        //{

        //   var res=_CatalogueService.checkPartno(Partno, icode);

        //    return this.Json(res, JsonRequestBehavior.AllowGet);        

        //}

        [Authorize]
        public JsonResult checkPartno(string Partno, string icode, string Flag)
        {
            if (Flag == "DRAWING ")
            {
                Flag = "DRAWING & POSITION NUMBER";
            }
            var res = _CatalogueService.checkPartno(Partno, icode, Flag);

            return this.Json(res, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult checkDuplicate()
        {

            var cat1 = Request.Form["cat"];
            var cat = JsonConvert.DeserializeObject<CatalogueModel>(cat1);
            var Charas = Request.Form["attri"];
            var ListCharas = JsonConvert.DeserializeObject<List<AttributeList>>(Charas);
           



            // vendor

            var vendorsuppliers = Request.Form["vendorsupplier"];

            var vendorsuppliersList = JsonConvert.DeserializeObject<List<Vendorsupplier>>(vendorsuppliers);


           
            var LstVendors = new List<Vendorsuppliers>();
            if (vendorsuppliersList != null && vendorsuppliersList.Count > 0)
            {
                foreach (Vendorsupplier LstAtt in vendorsuppliersList)
                {
                    if ((LstAtt.Name != null && LstAtt.Name != "") || (LstAtt.RefNo != "" && LstAtt.RefNo != null))
                    {

                        var vndMdl = new Vendorsuppliers();
                        vndMdl.slno = LstAtt.slno;
                        vndMdl.Code = LstAtt.Code;
                        vndMdl.Name = LstAtt.Name;
                        vndMdl.Type = LstAtt.Type;
                        vndMdl.RefNo = LstAtt.RefNo;
                        vndMdl.Refflag = LstAtt.Refflag;
                        vndMdl.s = LstAtt.s;
                        vndMdl.l = LstAtt.l;
                        vndMdl.shortmfr = LstAtt.shortmfr;

                        LstVendors.Add(vndMdl);
                    }


                }
            }


            //Attribute
            var lstCharateristics = new List<Prosol_AttributeList>();
            if (ListCharas != null)
            {

                foreach (AttributeList LstAtt in ListCharas)
                {
                    var AttrMdl = new Prosol_AttributeList();
                    AttrMdl.Characteristic = LstAtt.Characteristic;
                    AttrMdl.Value = LstAtt.Value;
                    AttrMdl.UOM = LstAtt.UOM;
                    AttrMdl.Squence = LstAtt.Squence;
                    AttrMdl.ShortSquence = LstAtt.ShortSquence;
                    AttrMdl.Source = LstAtt.Source;

                    lstCharateristics.Add(AttrMdl);

                }
            }

           

            var proCat = new Prosol_Datamaster();


            proCat._id = cat._id != null ? new MongoDB.Bson.ObjectId(cat._id) : new MongoDB.Bson.ObjectId();
            proCat.Itemcode = cat.Itemcode != null ? cat.Itemcode : "";
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            proCat.Partno = cat.Partno;
            proCat.Characteristics = lstCharateristics;
            proCat.Vendorsuppliers = LstVendors;

            var Equipment = Request.Form["Equ"];
            if (Equipment != null)
            {
                var EquModel = JsonConvert.DeserializeObject<Equipment>(Equipment);
                //Equipment
                var Equi_mdl = new Equipments();
                if (EquModel != null)
                {
                    Equi_mdl.Name = EquModel.Name;
                    Equi_mdl.Manufacturer = EquModel.Manufacturer;
                    Equi_mdl.Modelno = EquModel.Modelno;
                    Equi_mdl.Tagno = EquModel.Tagno;
                    Equi_mdl.Serialno = EquModel.Serialno;
                    Equi_mdl.Additionalinfo = EquModel.Additionalinfo;
                    Equi_mdl.EMS = EquModel.EMS;
                    Equi_mdl.ENS = EquModel.ENS;
                    proCat.Equipment = Equi_mdl;

                }
            }


            var res = _CatalogueService.checkDuplicate(proCat);
            var jsonResult = Json(res, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult GenerateShortLong()
        {
            var cat1 = Request.Form["cat"];
            var cat = JsonConvert.DeserializeObject<CatalogueModel>(cat1);
            var Charas = Request.Form["attri"];
            var ListCharas = JsonConvert.DeserializeObject<List<AttributeList>>(Charas);
            //var exCharas = Request.Form["exattri"];
            //var exListCharas = JsonConvert.DeserializeObject<List<AttributeList>>(exCharas);




            // vendor

            var vendorsuppliers = Request.Form["vendorsupplier"];

            var vendorsuppliersList = JsonConvert.DeserializeObject<List<Vendorsupplier>>(vendorsuppliers);



            var LstVendors = new List<Vendorsuppliers>();
            if (vendorsuppliersList != null && vendorsuppliersList.Count > 0)
            {
                foreach (Vendorsupplier LstAtt in vendorsuppliersList)
                {
                    if ((LstAtt.Name != null && LstAtt.Name != "") || (LstAtt.RefNo != "" && LstAtt.RefNo != null))
                    {

                        var vndMdl = new Vendorsuppliers();
                        vndMdl.slno = LstAtt.slno;
                        vndMdl.Code = LstAtt.Code;
                        vndMdl.Name = LstAtt.Name;
                        vndMdl.Type = LstAtt.Type;
                        vndMdl.RefNo = LstAtt.RefNo;
                        vndMdl.Refflag = LstAtt.Refflag;
                        vndMdl.s = LstAtt.s;
                        vndMdl.l = LstAtt.l;
                        if(!string.IsNullOrEmpty(LstAtt.Name))
                            vndMdl.shortmfr = LstAtt.shortmfr;
                        else
                            vndMdl.shortmfr = "";

                        LstVendors.Add(vndMdl);
                    }


                }
            }


            //Attribute
            var lstCharateristics = new List<Prosol_AttributeList>();
            if (ListCharas != null)
            {

                foreach (AttributeList LstAtt in ListCharas)
                {
                    var AttrMdl = new Prosol_AttributeList();
                    AttrMdl.Characteristic = LstAtt.Characteristic;
                    AttrMdl.Value = LstAtt.Value;
                    AttrMdl.UOM = LstAtt.UOM;
                    AttrMdl.Squence = LstAtt.Squence;
                    AttrMdl.ShortSquence = LstAtt.ShortSquence;
                    AttrMdl.Source = LstAtt.Source;
                    lstCharateristics.Add(AttrMdl);

                }
            }
            //ExAttribute
            var exlstCharateristics = new List<Prosol_AttributeList>();
            if (ListCharas != null)
            {

                foreach (AttributeList LstAtt in cat.exCharacteristics)
                {
                    var AttrMdl = new Prosol_AttributeList();
                    AttrMdl.Characteristic = LstAtt.Characteristic;
                    AttrMdl.Value = LstAtt.Value;
                    AttrMdl.UOM = LstAtt.UOM;
                    AttrMdl.Squence = LstAtt.Squence;
                    AttrMdl.ShortSquence = LstAtt.ShortSquence;
                    AttrMdl.Source = LstAtt.Source;
                    exlstCharateristics.Add(AttrMdl);

                }
            }
            var proCat = new Prosol_Datamaster();
            proCat._id = cat._id != null ? new MongoDB.Bson.ObjectId(cat._id) : new MongoDB.Bson.ObjectId();
            proCat.Itemcode = cat.Itemcode != null ? cat.Itemcode : "";
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            proCat.Partno = cat.Partno;
            proCat.Characteristics = lstCharateristics;
            proCat.exCharacteristics = exlstCharateristics;
            proCat.Vendorsuppliers = LstVendors;
            proCat.Additionalinfo = cat.Additionalinfo;
            proCat.exPartno = cat.exPartno;
            proCat.exManufacturer = cat.exManufacturer;
            //new 2
            proCat.Legacy = cat.Legacy;
            proCat.Legacy2 = cat.Legacy2;
            proCat.Longdesc = cat.Longdesc;
    
            //Equipment
            var Equipment = Request.Form["Equ"];
            if (Equipment != null)
            {
                var EquModel = JsonConvert.DeserializeObject<Equipment>(Equipment);
                var Equi_mdl = new Equipments();
                if (EquModel != null)
                {
                    Equi_mdl.Name = EquModel.Name;
                    Equi_mdl.Manufacturer = EquModel.Manufacturer;
                    Equi_mdl.Modelno = EquModel.Modelno;
                    Equi_mdl.Tagno = EquModel.Tagno;
                    Equi_mdl.Serialno = EquModel.Serialno;
                    Equi_mdl.Additionalinfo = EquModel.Additionalinfo;
                    Equi_mdl.EMS = EquModel.EMS;
                    Equi_mdl.ENS = EquModel.ENS;
                    Equi_mdl.SuperiorEquipment = EquModel.SuperiorEquipment;
                    proCat.Equipment = Equi_mdl;
                    proCat.Equipment = Equi_mdl;

                }
            }


            var  res = _CatalogueService.GenerateShortLong(proCat);
            //newcode
             var thi = res[1] ;
            res[6] = RepeatedValue(thi);
            var jsonResult = Json(res, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
       
        //Reviewer
        [Authorize]
        public JsonResult GetRSingleItem(string itemcode)
        {
           // int sts = 2;
           // int sts1 = 3;
            var cat = _CatalogueService.GetRSingleItem(itemcode);


            var proCat = new CatalogueModel();
            if (cat != null)
            {
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        AttrMdl.Source = pattri.Source;


                      

                        lstCharateristics.Add(AttrMdl);
                    }
                }

                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                    Equi_mdl.EMS = cat.Equipment.EMS;
                    Equi_mdl.ENS = cat.Equipment.ENS;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        vndMdl.s = vndrs.s;
                        vndMdl.l = vndrs.l;
                        vndMdl.shortmfr = vndrs.shortmfr;
                        LstVendors.Add(vndMdl);
                    }

                }

                proCat.Vendorsuppliers = LstVendors;
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;

                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Soureurl = cat.Soureurl;
                proCat.Junk = cat.Junk;
                proCat.Manufacturercode = cat.Manufacturercode;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;
                proCat.Partnodup = cat.Partnodup;
                proCat.Application = cat.Application;
                proCat.Drawingno = cat.Drawingno;
                proCat.Referenceno = cat.Referenceno;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Remarks = cat.Remarks;
                proCat.RevRemarks = cat.RevRemarks;
                proCat.RelRemarks = cat.RelRemarks;
                proCat.Unspsc = cat.Unspsc;

                proCat.UOM = cat.UOM;
                proCat.Maincode = cat.Maincode;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;

                proCat.Characteristics = lstCharateristics;
            }

            return this.Json(proCat, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult getReleaserList()
        {

            var ApproverList = _CatalogueService.getReleaserList().ToList();

            return this.Json(ApproverList, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetRDataList()
        {
            int sts = 2;
            int sts1 = 3;
            var dataList = _CatalogueService.GetRDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();

            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();
               
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;

                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Attachment = cat.Attachment;
                proCat.Rework = cat.Rework;
                proCat.Junk = cat.Junk;

                if (cat.Catalogue != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Catalogue.Name;
                    proCat.Catalogue = updted;
                }

                if (cat.Review != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Review.Name;
                    proCat.Review = updted;
                }
                if (cat.Release != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Release.Name;
                    proCat.Release = updted;
                }

                lstCatalogue.Add(proCat);
            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

           // return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult InsertRDataList()
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
            var catDatalist = Request.Form["DataList"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {
                    //if (mdl.ItemStatus == 3)
                    //{
                        var proCat = new Prosol_Datamaster();
                        proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                        proCat.ItemStatus = 4;
                        proCat.Itemcode = mdl.Itemcode;
                        proCat.Noun = mdl.Noun;
                        proCat.Modifier = mdl.Modifier;
                        proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


                        var releaser = new Prosol_UpdatedBy();
                        releaser.UserId = uID;
                        releaser.Name = uName;
                        releaser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                        proCat.Release = releaser;

                        listdt.Add(proCat);
                   // }
                }
                res = _CatalogueService.WriteRDataList(listdt);

            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                res = 0;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Reworkreview(string itemcode,string RevRemarks)
        {
            bool res =false;
            try
            {
                res = _CatalogueService.Reworkreview(itemcode, RevRemarks);
            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        //Releaser 
        [Authorize]
        public JsonResult GetReleaseSingleItem(string itemcode)
        {
           
            var cat = _CatalogueService.GetReleaseSingleItem(itemcode);


            var proCat = new CatalogueModel();
            if (cat != null)
            {
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        AttrMdl.Source = pattri.Source;
                        lstCharateristics.Add(AttrMdl);
                    }
                }

                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                    Equi_mdl.EMS = cat.Equipment.EMS;
                    Equi_mdl.ENS = cat.Equipment.ENS;
                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        vndMdl.s = vndrs.s;
                        vndMdl.l = vndrs.l;
                        vndMdl.shortmfr = vndrs.shortmfr;
                        LstVendors.Add(vndMdl);
                    }

                }

                proCat.Vendorsuppliers = LstVendors;
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;

                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Soureurl = cat.Soureurl;
                proCat.Junk = cat.Junk;
                proCat.Manufacturercode = cat.Manufacturercode;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;
                proCat.Partnodup = cat.Partnodup;
                proCat.Application = cat.Application;
                proCat.Drawingno = cat.Drawingno;
                proCat.Referenceno = cat.Referenceno;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Remarks = cat.Remarks;
                proCat.RevRemarks = cat.RevRemarks;
                proCat.RelRemarks = cat.RelRemarks;
                proCat.Unspsc = cat.Unspsc;

                proCat.UOM = cat.UOM;
                proCat.Maincode = cat.Maincode;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;

                proCat.Characteristics = lstCharateristics;
            }

            return this.Json(proCat, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetReleaseDataList()
        {
            int sts = 4;
            int sts1 = 5;
            var dataList = _CatalogueService.GetReleaseDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();

            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();

                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;
                proCat.UOM = cat.UOM;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;

                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Attachment = cat.Attachment;
                proCat.Junk = cat.Junk;
                if (cat.Catalogue != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Catalogue.Name;
                    proCat.Catalogue = updted;
                }

                if (cat.Review != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Review.Name;
                    proCat.Review = updted;
                }
                if (cat.Release != null)
                {
                    var updted = new UpdatedBy();
                    updted.Name = cat.Release.Name;
                    proCat.Release = updted;
                }

                lstCatalogue.Add(proCat);
            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

           // return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        [ValidateInput(false)]
        public JsonResult InsertReleaseDataList()
        {
            var catDatalist = Request.Form["DataList"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);
            string res = "";
            try
            {

                var listdt = new List<Prosol_Datamaster>();

                var codeLogic = _Nounmodifier.showcode();
                foreach (CatalogueModel mdl in catModelList)
                {
                    var proCat = _CatalogueService.GetReleaseSingleItem(mdl.Itemcode);

                    //if (codeLogic.CODELOGIC == "Customized Code")
                    //{
                    //    //  string Lcode = proCat.Maincode + proCat.Subcode + proCat.Subsubcode;
                    //    if (proCat.Materialcode == null || proCat.Materialcode == "")

                    //        proCat.Materialcode = generateMaterialcode(proCat.Subsubcode, 1);

                    //}
                    //else if (codeLogic.CODELOGIC == "UNSPSC Code")
                    //{
                    //    if (proCat.Materialcode == null || proCat.Materialcode == "" || proCat.Materialcode.Substring(0, 8) != mdl.Unspsc)
                    //        proCat.Materialcode = generateMaterialcode(proCat.Unspsc, 0);
                    //}
                    //else
                    //{
                        proCat.Materialcode = mdl.Materialcode;
                    //}




                    proCat.ItemStatus = 6;
                    proCat.Noun = mdl.Noun;
                    proCat.Modifier = mdl.Modifier;
                    proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    listdt.Add(proCat);
                    //  }
                }

                var tbl = new DataTable();
                tbl.Columns.Add("Item Code");
                tbl.Columns.Add("Material Code");
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Short Desc");
                tbl.Columns.Add("Long Desc");
                tbl.Columns.Add("UOM");

                //foreach (CatalogueModel pr1 in catModelList)
                //{
                    foreach (var pr2 in listdt)
                    {
                       // var slname = _ItemRequestService.getslname(pr1.storage_Location);
                        var row = tbl.NewRow();
                        row["Item Code"] = pr2.Itemcode;
                        row["Material Code"] = pr2.Materialcode;

                        row["Legacy"] = pr2.Legacy;
                        row["Short Desc"] = pr2.Shortdesc;
                        row["Long Desc"] = pr2.Longdesc;
                        row["UOM"] = pr2.UOM;
                        tbl.Rows.Add(row);

                    }

                //}
             
                res = _CatalogueService.WriteReleaseDataList(listdt,tbl);
                return this.Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //var log = new Prosol.Common.LogManager();
                //log.LogError(log.GetType(), e);
                //res = 0;
                return this.Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        public JsonResult Reworkrelease(string itemcode, string RelRemarks)
        {
            bool res = false;
            try
            {
                res = _CatalogueService.Reworkrelease(itemcode, RelRemarks);
            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        private string generateMaterialcode(string LogicCode, int indc)
        {
            string itmCode = "";
            string indicator = Convert.ToString(indc);
            var code = _CatalogueService.getMaterialCode(LogicCode);

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
        //Excel view

        [Authorize]
        public JsonResult GetCatItemList()
        {
            int sts = 0;
            int sts1 = 1;
            var dataList = _CatalogueService.GetDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();

            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        lstCharateristics.Add(AttrMdl);
                    }
                    // proCat.ItemStatus = 0;
                    proCat.Legacy = cat.Legacy;
                    proCat.Legacy2 = cat.Legacy2;
                    proCat.Itemcode = cat.Itemcode;
                    proCat.Noun = cat.Noun;
                    proCat.Modifier = cat.Modifier;

                    proCat.Characteristics = lstCharateristics;

                }
                else
                {
                    proCat.Legacy = cat.Legacy;
                    proCat.Legacy2 = cat.Legacy2;
                    proCat.Itemcode = cat.Itemcode;
                    proCat.Noun = cat.Noun;
                    proCat.Modifier = cat.Modifier;
                    proCat.Characteristics = new List<AttributeList>();
                }
                lstCatalogue.Add(proCat);
                // proCat._id = cat._id.ToString();              

            }
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            int flg = 0;
            foreach (CatalogueModel ct in lstCatalogue.OrderByDescending(s => s.Characteristics.Count))
            {
                if (flg == 0)
                {
                    row = new Dictionary<string, object>();
                    row.Add("ItemCode", "Item Code");
                    row.Add("Legacy", "Legacy");
                    row.Add("Legacy2", "PV data");
                    row.Add("Noun", "Noun");
                    row.Add("Modifier", "Modifier");
                    if (ct.Characteristics != null)
                    {
                        int i = 1;
                        foreach (AttributeList at in ct.Characteristics)
                        {
                            row.Add("Attribute" + i, "Attribute" + i);
                            row.Add("Value" + i, "Value" + i);
                            row.Add("UOM" + i, "UOM" + i);

                            i++;
                        }

                    }
                    rows.Add(row);
                    flg = 1;
                }

                row = new Dictionary<string, object>();
                row.Add("ItemCode", ct.Itemcode);
                row.Add("Legacy", ct.Legacy);
                row.Add("Legacy2", ct.Legacy2);
                row.Add("Noun", ct.Noun);
                row.Add("Modifier", ct.Modifier);
                if (ct.Characteristics != null)
                {
                    int i = 1;
                    foreach (AttributeList at in ct.Characteristics)
                    {
                        row.Add("Attribute" + i, at.Characteristic);
                        row.Add("Value" + i, at.Value);
                        row.Add("UOM" + i, at.UOM);

                        i++;
                    }

                }
                rows.Add(row);
            }

            return this.Json(rows, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult GetItemList()
        {
            int sts = 2;
            int sts1 = 3;
            var dataList = _CatalogueService.GetRDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();

            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        lstCharateristics.Add(AttrMdl);
                    }
                   // proCat.ItemStatus = 0;
                    proCat.Legacy = cat.Legacy;
                    proCat.Legacy2 = cat.Legacy2;
                    proCat.Itemcode = cat.Itemcode;
                    proCat.Noun = cat.Noun;
                    proCat.Modifier = cat.Modifier;

                    proCat.Characteristics = lstCharateristics;
                    lstCatalogue.Add(proCat);
                }
                // proCat._id = cat._id.ToString();              

            }
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            int flg = 0;
            foreach (CatalogueModel ct in lstCatalogue.OrderByDescending(s => s.Characteristics.Count))
            {
                if (flg == 0)
                {
                    row = new Dictionary<string, object>();
                    row.Add("ItemCode", "Item Code");
                    row.Add("Legacy", "Legacy");
                    row.Add("Legacy2", "PV data");
                    row.Add("Noun", "Noun");
                    row.Add("Modifier", "Modifier");
                    if (ct.Characteristics != null)
                    {
                        int i = 1;
                        foreach (AttributeList at in ct.Characteristics)
                        {
                            row.Add("Attribute" + i, "Attribute" + i);
                            row.Add("Value" + i, "Value" + i);
                            row.Add("UOM" + i, "UOM" + i);

                            i++;
                        }

                    }
                    rows.Add(row);
                    flg = 1;
                }

                row = new Dictionary<string, object>();
                row.Add("ItemCode", ct.Itemcode);
                row.Add("Legacy", ct.Legacy);
                row.Add("Legacy2", ct.Legacy2);
                row.Add("Noun", ct.Noun);
                row.Add("Modifier", ct.Modifier);
                if (ct.Characteristics != null)
                {
                    int i = 1;
                    foreach (AttributeList at in ct.Characteristics)
                    {
                        row.Add("Attribute" + i, at.Characteristic);
                        row.Add("Value" + i, at.Value);
                        row.Add("UOM" + i, at.UOM);

                        i++;
                    }

                }
                rows.Add(row);
            }  

            return this.Json(rows, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetReleaseItemList()
        {
            int sts = 4;
            int sts1 = 5;
            var dataList = _CatalogueService.GetReleaseDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();

            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        lstCharateristics.Add(AttrMdl);
                    }
                    // proCat.ItemStatus = 0;
                    proCat.Legacy = cat.Legacy;
                    proCat.Legacy2 = cat.Legacy2;
                    proCat.Itemcode = cat.Itemcode;
                    proCat.Materialcode = cat.Materialcode;
                    proCat.Noun = cat.Noun;
                    proCat.Modifier = cat.Modifier;

                    proCat.Characteristics = lstCharateristics;
                    lstCatalogue.Add(proCat);
                }
                // proCat._id = cat._id.ToString();              

            }
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            int flg = 0;
            foreach (CatalogueModel ct in lstCatalogue.OrderByDescending(s => s.Characteristics.Count))
            {
                if (flg == 0)
                {
                    row = new Dictionary<string, object>();
                    row.Add("ItemCode", "Item Code");
                    row.Add("Legacy", "Legacy");
                    row.Add("Legacy2", "PV data");
                    row.Add("Noun", "Noun");
                    row.Add("Modifier", "Modifier");
                    if (ct.Characteristics != null)
                    {
                        int i = 1;
                        foreach (AttributeList at in ct.Characteristics)
                        {
                            row.Add("Attribute" + i, "Attribute" + i);
                            row.Add("Value" + i, "Value" + i);
                            row.Add("UOM" + i, "UOM" + i);

                            i++;
                        }

                    }
                    rows.Add(row);
                    flg = 1;
                }

                row = new Dictionary<string, object>();
                row.Add("ItemCode", ct.Itemcode);
                row.Add("Legacy", ct.Legacy);
                row.Add("Legacy2", ct.Legacy2);
                row.Add("Noun", ct.Noun);
                row.Add("Modifier", ct.Modifier);
                if (ct.Characteristics != null)
                {
                    int i = 1;
                    foreach (AttributeList at in ct.Characteristics)
                    {
                        row.Add("Attribute" + i, at.Characteristic);
                        row.Add("Value" + i, at.Value);
                        row.Add("UOM" + i, at.UOM);

                        i++;
                    }

                }
                rows.Add(row);
            }

            return this.Json(rows, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult SaveItemsList(string data)
        {
            data = data.Replace("\"\"", "xxx");
            data = data.Replace("\",\"", "*****");
            data = data.Replace("\"],[\"", "****");
            data = data.Replace("[\"", "**");          
            data = data.Replace("\"]", "***");

            data = data.Replace("\",", "!!!");
            data = data.Replace(",\"", "$$$");


            data = data.Replace("\"", "");

            data = data.Replace("xxx", "\"\"");
            data = data.Replace("*****","\",\"");

            data = data.Replace("****", "\"],[\"");

            data = data.Replace("***", "\"]");

            data = data.Replace("**", "[\"");

            data = data.Replace("!!!", "\",");
            data = data.Replace("$$$", ",\"");


            bool res = false;

            var Lst = JsonConvert.DeserializeObject<List<string[]>>(data);
            var LstCat = new List<Prosol_Datamaster>();
            foreach (string[] arrStr in Lst)
            {
                int i = 0;
                var catmdl = new Prosol_Datamaster();
                catmdl.Itemcode = arrStr[i++];
                catmdl.Legacy = arrStr[i++];
                catmdl.Legacy2 = arrStr[i++];
                catmdl.Noun = arrStr[i++];
                catmdl.Modifier = arrStr[i++];
                var ListattMdl = new List<Prosol_AttributeList>();
                for (; i <= arrStr.Length - 3;)
                {
                    if (arrStr[i] != null)
                    {
                        var attMdl = new Prosol_AttributeList();
                        attMdl.Characteristic = arrStr[i++];
                        attMdl.Value = arrStr[i++];
                        attMdl.UOM = arrStr[i++];
                        ListattMdl.Add(attMdl);
                    }
                    else break;
                }
                catmdl.Characteristics = ListattMdl;

                LstCat.Add(catmdl);
            }
            try
            {
                res = _CatalogueService.UpdateData(LstCat);
            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetValuesList(string Noun, string Modifier, string Characteristic)
        {
            var arrStr = _CatalogueService.GetValuesList(Noun, Modifier, Characteristic);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUnits()
        {
            var arrStr = _CatalogueService.GetUnits();
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }
        public JsonResult checkValidate(string Attribute)
        {
            var arrStr = _CatalogueService.checkValidate(Attribute);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetsimItemsList(string Noun, string Modifier)
        {
            var arrStr = _CatalogueService.GetsimItemsList(Noun, Modifier); 
                      
            var jsonResult = Json(arrStr, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        
        public JsonResult getplantCode_Name()
        {
            var plantdetails = _CatalogueService.getplantCode_Name();
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

            List<Prosol_Plants> lst = new List<Prosol_Plants>();
            foreach (Prosol_Plants mdl in plantdetails)
            {
                if (usrInfo.Plantcode == null)
                {
                    lst.Add(mdl);
                }
                else
                {
                    foreach (string cd in usrInfo.Plantcode)
                    {
                        if (cd == mdl.Plantcode)
                        {
                            lst.Add(mdl);
                        }
                    }

                }
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getplantCode_Name1()
        {
            var plantdetails = _CatalogueService.getplantCode_Name();
            //  var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

            List<Prosol_Plants> lst = new List<Prosol_Plants>();
            foreach (Prosol_Plants mdl in plantdetails)
            {
                var pln = new Prosol_Plants();
                pln.Plantname = mdl.Plantname;
           //     pln.Plantcode = mdl.Plantcode;
                lst.Add(pln);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);
        }
        //uom
        public JsonResult getuomlist()
        {
            var result = _CatalogueService.getuomlist();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        //Attribute Logic
               
        public ActionResult getItem(string itemId)
        {
     
            var cat = _CatalogueService.GetSingleItem(itemId);


            var proCat = new CatalogueModel();
            if (cat != null)
            {
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                       // AttrMdl.Squence = pattri.Squence;
                       // AttrMdl.ShortSquence = pattri.ShortSquence;
                        AttrMdl.Source = pattri.Source;
                        lstCharateristics.Add(AttrMdl);
                    }
                }

                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                    Equi_mdl.EMS = cat.Equipment.EMS;
                    Equi_mdl.ENS = cat.Equipment.ENS;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        vndMdl.s = vndrs.s;
                        vndMdl.l = vndrs.l;
                        vndMdl.shortmfr = vndrs.shortmfr;
                        LstVendors.Add(vndMdl);
                    }

                }

                proCat.Vendorsuppliers = LstVendors;
               // proCat._id = cat._id.ToString();
              //  proCat.Itemcode = cat.Itemcode;
                proCat.ItemStatus = cat.ItemStatus;
              //  proCat.Legacy = cat.Legacy;
              //  proCat.Legacy2 = cat.Legacy2;

                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                //newcode
               proCat.RepeatedValue = RepeatedValue(proCat.Longdesc);
                //
                proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Soureurl = cat.Soureurl;
                proCat.Junk = cat.Junk;
                proCat.Manufacturercode = cat.Manufacturercode;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;
                proCat.Partnodup = cat.Partnodup;
                proCat.Application = cat.Application;
                proCat.Drawingno = cat.Drawingno;
                proCat.Referenceno = cat.Referenceno;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Remarks = cat.Remarks;
                proCat.RevRemarks = cat.RevRemarks;
                proCat.RelRemarks = cat.RelRemarks;

                

                proCat.Unspsc = cat.Unspsc;

                proCat.UOM = cat.UOM;
                proCat.Maincode = cat.Maincode;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;

                proCat.Characteristics = lstCharateristics;
               // var res = _CatalogueService.GenerateShortLong(proCat);
            }

           return this.Json(proCat, JsonRequestBehavior.AllowGet);

           
        }
        public string CheckValue(string Noun, string Modifier, string Attribute, string Value)
        {
          //  Value = Value.Replace("***", "&");
            var res = _CatalogueService.CheckValue(Noun, Modifier, Attribute, Value);

            return res;
        }
        public string CheckValue1(string Noun, string Modifier, string Attribute, string Value)
        {
          //  Value = Value.Replace("***", "&");
            var res = _CatalogueService.CheckValue1(Noun, Modifier, Attribute, Value);

            return res;
        }
         public string CheckValApprove(string Value)
        {
            //Value = Value.Replace("***", "&");

            var res = _CatalogueService.CheckValApprove(Value);

            return res;
        }
        public bool AddValue(string Noun, string Modifier, string Attribute, string Value,string abb, string role)
        {
           // Value = Value.Replace("***", "&");
           // abb = abb.Replace("***", "&");
            string user = Session["username"].ToString();
            var res = _CatalogueService.AddValue(Noun, Modifier, Attribute, Value,abb, user, role);

            return res;
        }
        public bool ApproveValue(string Noun, string Modifier, string Attribute, string Value, string abb)
        {
          //  Value = Value.Replace("***", "&");
           // abb = abb.Replace("***", "&");
            string user = Session["username"].ToString();
            var res = _CatalogueService.ApproveVal(Noun, Modifier, Attribute, Value, abb, user);

            return res;
        }
        public JsonResult FetchNMRelation(string Noun, string Modifier)
        {
            var LstNMRelation = _LogicService.FetchNMRelation(Noun, Modifier).ToList();
            var jsonResult = Json(LstNMRelation, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult FetchATTRelation(string Noun, string Modifier)
        {
            var LstATTRelation = _LogicService.FetchATTRelation(Noun, Modifier).ToList();
            var jsonResult = Json(LstATTRelation, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public string getunitforvalue(string value)
        {
            return _CatalogueService.getunitforvalue(value);
        }
        public JsonResult GetRejectCode(string Itemcode, string Remarks, string usr, string Noun,string Modifier)
        {
            var cat = Request.Form["cat"];
            var Equipment = Request.Form["Equ"];
            var Charas = Request.Form["attri"];
            var vendorsuppliers = Request.Form["vendorsupplier"];
            var vendorsuppliersList = JsonConvert.DeserializeObject<List<Vendorsupplier>>(vendorsuppliers);
            var catModel= JsonConvert.DeserializeObject<CatalogueModel>(cat);
            var EquModel = JsonConvert.DeserializeObject<Equipment>(Equipment);
            var ListCharas = JsonConvert.DeserializeObject<List<AttributeList>>(Charas);
            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();
            var autormrk = "TOOL-REMARK: PROVIDE ";

            var lstCharateristics = new List<Prosol_AttributeList>();
            if (ListCharas != null)
            {
                foreach (AttributeList LstAtt in ListCharas)
                {
                    var AttrMdl = new Prosol_AttributeList();
                    AttrMdl.Characteristic = LstAtt.Characteristic;

                    if (LstAtt.Value != null && LstAtt.Value != "")
                    {
                        AttrMdl.Value = LstAtt.Value.Trim().ToUpper();

                    }
                    else
                    {
                        AttrMdl.Value = "";
                    }

                    AttrMdl.UOM = LstAtt.UOM;
                    AttrMdl.Squence = LstAtt.Squence;
                    AttrMdl.ShortSquence = LstAtt.ShortSquence;

                    AttrMdl.Source = LstAtt.Source;
                    AttrMdl.SourceUrl = LstAtt.SourceUrl;

                    lstCharateristics.Add(AttrMdl);

                }
            }
            
            //Vendorsuppliers
            var LstVendors = new List<Vendorsuppliers>();
            foreach (Vendorsupplier LstAtt in vendorsuppliersList)
            {
                if ((LstAtt.Name != null && LstAtt.Name != "") || (LstAtt.RefNo != "" && LstAtt.RefNo != null))
                {
                    var vndMdl = new Vendorsuppliers();
                    vndMdl.slno = LstAtt.slno;
                    vndMdl.Code = LstAtt.Code;
                    vndMdl.Name = LstAtt.Name;
                    vndMdl.Type = LstAtt.Type;
                    vndMdl.RefNo = LstAtt.RefNo;
                    vndMdl.Refflag = LstAtt.Refflag;
                    vndMdl.s = LstAtt.s;
                    vndMdl.l = LstAtt.l;
                    vndMdl.shortmfr = LstAtt.shortmfr;

                    LstVendors.Add(vndMdl);
                }

            }
            if (catModel.Noun != null && catModel.Modifier != null)
            {
                var arrChar = _CharateristicService.GetCharateristic(catModel.Noun, catModel.Modifier);
                var lstChar = new List<NM_AttributesModel>();
                foreach (Prosol_Charateristics nm_Char in arrChar)
                {
                    var Chara = new NM_AttributesModel();
                    Chara.Characteristic = nm_Char.Characteristic;
                    Chara.Mandatory = nm_Char.Mandatory;
                    Chara.Values = nm_Char.Values;
                    lstChar.Add(Chara);
                }

                foreach (NM_AttributesModel x in lstChar)
                {
                    foreach (AttributeList y in ListCharas)
                    {
                        if (x.Characteristic == y.Characteristic && x.Mandatory == "Yes" && (y.Value == null || y.Value == ""))
                        {
                            autormrk += x.Characteristic + ",";
                        }

                    }
                }
                var arrStr = _Nounmodifier.GetNounModifier(catModel.Noun, catModel.Modifier);
            ///foreach (Vendorsupplier y in vendorsuppliersList)
           // {
               if(vendorsuppliersList[0].Name == null && (arrStr?.Formatted == 0 || arrStr?.Formatted == 2))
                {
                    autormrk += "MANUFACTURER,";
                }
                if (vendorsuppliersList[0].RefNo == null && (arrStr?.Formatted == 0 || arrStr?.Formatted == 2))
                {
                    if (vendorsuppliersList[0].Refflag != null)
                        autormrk += vendorsuppliersList[0].Refflag;
                    else
                        autormrk += "PART/MODEL NUMBER";
                }

        //    }
       
                if (EquModel == null && arrStr.Formatted == 0)
                {
                    autormrk += "EQUIPMENT DETAILS,";

                }
            }


            var procat = new Prosol_Datamaster();
            procat.Itemcode = catModel.Itemcode;
            procat.Materialcode = catModel.Materialcode;
            procat.Noun = catModel.Noun;
            procat.Modifier = catModel.Modifier;
            procat.UOM = catModel.UOM;
            procat.Characteristics = lstCharateristics;
            //procat.RelRemarks = autormrk + " CATALOGUER-REMARK:" + catModel.RevRemarks;
            procat.RevRemarks = catModel.RevRemarks;
            procat.Vendorsuppliers = LstVendors;
            procat.Additionalinfo = catModel.Additionalinfo;
            var Equi_mdl = new Equipments();
            if (EquModel != null)
            {
                Equi_mdl.Name = EquModel.Name;
                Equi_mdl.Manufacturer = EquModel.Manufacturer;
                Equi_mdl.Modelno = EquModel.Modelno;
                Equi_mdl.Tagno = EquModel.Tagno;
                Equi_mdl.Serialno = EquModel.Serialno;
                Equi_mdl.Additionalinfo = EquModel.Additionalinfo;
                Equi_mdl.EMS = EquModel.EMS;
                Equi_mdl.ENS = EquModel.ENS;

                procat.Equipment = Equi_mdl;
            }
            var tbl = new DataTable();
            tbl.Columns.Add("Item Code");
            tbl.Columns.Add("Legacy");
            tbl.Columns.Add("Clarification Remarks");




            var row = tbl.NewRow();
            row["Item Code"] = catModel.Itemcode;
            row["Legacy"] = catModel.Legacy;
            row["Clarification Remarks"] = catModel.Remarks;
            tbl.Rows.Add(row);




            //  var result = _CatalogueService.GetCodeForRejectedItems(catModel.Itemcode, catModel.Remarks, userid, username , usr, catModel.Noun, catModel.Modifier);
            var result = _CatalogueService.GetCodeForRejectedItems(procat, userid, username,tbl);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRejectCode1(string Itemcode, string RevRemarks)
        {
            var cat = Request.Form["cat"];
            var catModel = JsonConvert.DeserializeObject<CatalogueModel>(cat);
            Itemcode = catModel.Itemcode;
            RevRemarks = catModel.RevRemarks;
            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();
          

            var result = _CatalogueService.GetCodeForRejectedItems1(Itemcode, RevRemarks, userid, username);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRejectCode2(string Itemcode, string RelRemarks)
        {
            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();
            var result = _CatalogueService.GetCodeForRejectedItems2(Itemcode, RelRemarks, userid, username);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }


        // for over all search

        [Authorize]
        public JsonResult GetDBNounList()
        {
            var dataList = _Nounmodifier.GetNounModifierList();
            // var dataList = _CatalogueService.GetTotalItem().ToList();          
            //  var result = dataList.Select(i => new { i.Noun,i.Modifier }).Distinct().ToList();        
            var res = Json(dataList, JsonRequestBehavior.AllowGet);
            res.MaxJsonLength = int.MaxValue;
            return res;
          
        }
        [Authorize]
        public JsonResult searchMaster()
        {
            string sCode = Request.Form["sCode"];
            string sSource = Request.Form["sSource"];
            string sNoun = Request.Form["sNoun"];
            string sShort = Request.Form["sShort"];
            string sLong = Request.Form["sLong"];
            string sModifier = Request.Form["sModifier"];
            string sUser = Request.Form["sUser"];
            string sStatus = Request.Form["sStatus"];
            string sType = Request.Form["sType"];
            var dataList = _CatalogueService.searchMaster(sCode, sSource,sShort,sLong,sNoun, sModifier, sUser, sType, sStatus);
            var locObjList = new List<CatalogueModel>();
            if (dataList != null && dataList.Count > 0)
            {
                foreach (Prosol_Datamaster mdl in dataList)
                {
                    var locMdl = new CatalogueModel();
                    locMdl._id = mdl._id.ToString();
                    locMdl.Itemcode = mdl.Itemcode;
                    locMdl.Materialcode = mdl.Materialcode;
                    locMdl.Legacy = mdl.Legacy;
                    locMdl.Legacy2 = mdl.Legacy2;
                    locMdl.Shortdesc = mdl.Shortdesc;
                    locMdl.Longdesc = mdl.Longdesc;
                    locMdl.Noun = mdl.Noun;
                    locMdl.Modifier = mdl.Modifier;
                    locMdl.ItemStatus = mdl.ItemStatus;
                    locMdl.Reworkcat = mdl.Reworkcat;
                    locMdl.Rework = mdl.Rework;
                    locMdl.Junk = mdl.Junk;
                    locMdl.Stock_Status = mdl.Stock_Status;
                    if (mdl.PVuser != null)
                    {
                        var updted = new UpdatedBy();
                        updted.Name = mdl.PVuser.Name;
                        updted.UpdatedOn = mdl.PVuser.UpdatedOn;
                        locMdl.PVuser = updted;
                    }
                    if (mdl.Catalogue != null)
                    {
                        var updted = new UpdatedBy();
                        updted.Name = mdl.Catalogue.Name;
                        locMdl.Catalogue = updted;
                    }

                    if (mdl.Review != null)
                    {
                        var updted = new UpdatedBy();
                        updted.Name = mdl.Review.Name;
                        locMdl.Review = updted;
                    }
                    if (mdl.Release != null)
                    {
                        var updted = new UpdatedBy();
                        updted.Name = mdl.Release.Name;
                        locMdl.Release = updted;
                    }

                    locObjList.Add(locMdl);

                }
            }

            // { "Itemcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release" };

            var jsonResult = Json(locObjList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult searchmasterpv()
        {
            string sCode = Request.Form["sCode"];
            string Plant = Request.Form["sPlant"];

            string StorageLocation = Request.Form["StorageLocation"];
            string storagebin = Request.Form["storagebin"];
            string sNoun = Request.Form["sNoun"];
            string sModifier = Request.Form["sModifier"];

            var dataList = _CatalogueService.searchmasterpv(sCode, Plant, StorageLocation, storagebin, sNoun, sModifier);
            var locObjList = new List<CatalogueModel>();
            if (dataList != null && dataList.Count > 0)
            {
                foreach (Prosol_Datamaster mdl in dataList)
                {
                    var locMdl = new CatalogueModel();
                    locMdl._id = mdl._id.ToString();
                    locMdl.Itemcode = mdl.Itemcode;
                    locMdl.Materialcode = mdl.Materialcode;
                    locMdl.Legacy = mdl.Legacy;
                    locMdl.Shortdesc = mdl.Shortdesc;
                    locMdl.Longdesc = mdl.Longdesc;
                    locMdl.Noun = mdl.Noun;
                    locMdl.Modifier = mdl.Modifier;
                    locMdl.ItemStatus = mdl.ItemStatus;

                    locMdl.Reworkcat = mdl.Reworkcat;
                    locMdl.Rework = mdl.Rework;

                    locMdl.StorageLocation1 = mdl.Storage_Location1;
                    locMdl.StorageBin1 = mdl.Storage_Bin1;


                    locObjList.Add(locMdl);

                }
            }

            // { "Itemcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release" };

            var jsonResult = Json(locObjList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [Authorize]
        public JsonResult showall_user()
        {
           
                var userlist = _CatalogueService.showall_user().ToList();
                return Json(userlist, JsonRequestBehavior.AllowGet);
           
        }
        [Authorize]
        public JsonResult Bulkdata_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkData(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkSingleAttribute()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkSingleAttribute(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkAttribute()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkAttribute(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Vendordata_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkVendor(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Equipdata_Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkEquip(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]

        public JsonResult ShortLong()
        {
            string res = "";
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    Tuple<int, string> res1 = _CatalogueService.BulkShortLong(file);
                    if (res1.Item2 != null && res1.Item1 > 0)
                    {
                        res = res1.Item1 + " item(s) uploaded and Error in " + res1.Item2;
                    }
                    else if (res1.Item2 == null && res1.Item1 > 0)
                    {
                        res = res1.Item1 + " item(s) uploaded successfully";
                    }
                    else
                    {
                        res = "Item(s) not uploaded Error in " + res1.Item2;
                    }
                }
            }

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]

        public JsonResult BulkUrl()
        {
            string res = "";
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    Tuple<int, string> res1 = _CatalogueService.BulkUrl(file);
                    if (res1.Item2 != null && res1.Item1 > 0)
                    {
                        res = res1.Item1 + " item(s) uploaded and Error in " + res1.Item2;
                    }
                    else if (res1.Item2 == null && res1.Item1 > 0)
                    {
                        res = res1.Item1 + " item(s) uploaded successfully";
                    }
                    else
                    {
                        res = "Item(s) not uploaded Error in " + res1.Item2;
                    }
                }
            }

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        // RepeatedValue
        //private string RepeatedValue(string cat)
        //{
        //    //BEARING,BALL,TYPE: SELF - ALIGNING,DESIGNATION: 2311 K,INSIDE DIAMETER:55 MM,OUTSIDE DIAMETER:120 MM,


        //    string constr = "", tmpStr = "";
        //    string[] tmpArr = cat.Split(',');
        //    int inc = 0;
        //    foreach (string str1 in tmpArr)
        //    {
        //        if (str1.Contains(':'))
        //        {
        //            string[] strplt = str1.Split(':');
        //            tmpArr[inc] = strplt[1];
        //        }
        //        if (inc < (tmpArr.Length - 1))
        //            tmpStr = tmpStr + tmpArr[inc] + ",";

        //        inc++;
        //    }

        //    string[] repp = tmpStr.Split(' ', ',', ';');
        //    foreach (string str in repp)
        //    {

        //        int flg = 0;
        //        foreach (string str1 in repp)
        //        {
        //            if (str == str1 && str != "--"&& str1 != "--")
        //            {
        //                if (flg == 1)
        //                {
        //                    string[] forCheck = constr.Split(',');
        //                    if (!forCheck.Contains(str))
        //                        constr = constr + str + ",";
        //                    //  constr = constr.TrimStart(',').TrimEnd(':');
        //                }
        //                flg = 1;
        //            }

        //        }
        //    }
        //    if (constr.EndsWith(","))
        //    {
        //        constr = constr.Substring(0, constr.Length - 1);
        //    }
        //    return constr;
        //}

        private string RepeatedValue(string cat)
        {
            string constr = "";
            string tmpStr = "";

            string[] tmpArr = cat.Split(',');

            for (int i = 0; i < tmpArr.Length; i++)
            {
                if (tmpArr[i].Contains(':'))
                {
                    string[] strSplit = tmpArr[i].Split(':');
                    tmpArr[i] = strSplit.Length > 1 ? strSplit[1].Trim() : tmpArr[i];
                }

                tmpStr += tmpArr[i].Trim() + (i < tmpArr.Length - 1 ? "," : "");
            }

            string[] words = tmpStr.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            HashSet<string> seenWords = new HashSet<string>();
            HashSet<string> repeatedWords = new HashSet<string>();

            foreach (string word in words)
            {
                if (seenWords.Contains(word) && !repeatedWords.Contains(word) && word != "--")
                {
                    repeatedWords.Add(word);
                }
                seenWords.Add(word);
            }

            constr = string.Join(",", repeatedWords);

            return constr;
        }

        private void runElasticSearch(List<Prosol_Datamaster> datalst)
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;

            //Connection string for Elasticsearch
            /*connectionSettings = new ConnectionSettings(new Uri("http://localhost Jump :9200/")); //local PC
            elasticClient = new ElasticClient(connectionSettings);*/

            //Multiple node for fail over (cluster addresses)
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200"),
                //new Uri("Add server 2 address")   //Add cluster addresses here
                //new Uri("Add server 3 address")
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);
            elasticClient = new ElasticClient(connectionSettings);

            List<ESdata> esData = new List<ESdata>();
            foreach (Prosol_Datamaster mdl in datalst)
            {
                var sObject = _CatalogueService.GetSingleItem(mdl.Itemcode);
                ESdata esd = new ESdata();
                esd.id = sObject._id.ToString();
                esd.Itemcode = sObject.Itemcode;
                esd.Shortdesc = sObject.Shortdesc;
                esd.Longdesc = sObject.Longdesc;
                esData.Add(esd);

            }
            // Update by Partial Document
            foreach (ESdata mdl in esData)
            {
                var response = elasticClient.Update<ESdata>(mdl.id, d => d.Index("datamaster").Type("Short")
                .Doc(new ESdata
                {
                    Itemcode = mdl.Itemcode,
                    Shortdesc = mdl.Shortdesc,
                    Longdesc = mdl.Longdesc
                }));

                if (!response.IsValid)
                {
                    var res = elasticClient.Index(mdl, i => i.Index("datamaster").Type("Short").Id(mdl.id));


                }

            }

        }

        [Authorize]
        public JsonResult getHSNList(string sKey)
        {
            var srch = _CatalogueService.getHSNList(sKey);
            var hsn = new List<Prosol_HSNModel>();
            foreach (Prosol_HSNModel H in srch)
            {
                var pro = new Prosol_HSNModel();

                pro.HSNID = H.HSNID;
                pro.Desc = H.Desc;


                hsn.Add(pro);
            }

            var jsonResult = Json(hsn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        [Authorize]
        public JsonResult UploadMaximoAssetImg()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _CatalogueService.GetSingleItem(EquipId);
            string imgName = "";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (E_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.MatImgs != null)
                        {
                            var NPimgs = E_Obj.AssetImages.MatImgs;
                            if (NPimgs.Length > 0)
                            {
                                imgName = E_Obj.Materialcode + "_" + EquipId + "MAXI-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = E_Obj.Materialcode + "_" + EquipId + "MAXI-" + 1 + ".jpg";

                            var imgList = E_Obj.AssetImages.MatImgs.ToList();
                            imgList.Add(apiurl + imgName);
                            E_Obj.AssetImages.MatImgs = imgList.ToArray();
                        }
                        else
                        {
                            imgName = EquipId + "MAXI-" + 1 + ".jpg";
                            E_Obj.AssetImages.MatImgs = new string[] { apiurl + imgName };
                        }

                    }
                    else
                    {
                        imgName = E_Obj.Materialcode + "_" + EquipId + "MAXI-" + 1 + ".jpg";
                        E_Obj.AssetImages = new MatImage();

                        E_Obj.AssetImages.MatImgs = new string[] { apiurl + imgName };

                    }

                    string PostURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/upload";

                    try
                    {
                        int contentLength = file.ContentLength;
                        byte[] data = new byte[contentLength];
                        file.InputStream.Read(data, 0, contentLength);

                        // Prepare web request...
                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                        webRequest.Method = "POST";
                        webRequest.Headers.Add("file-name", imgName);
                        webRequest.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                        //   webRequest.ContentType = "multipart/form-data";
                        webRequest.ContentType = "image/jpeg";
                        webRequest.ContentLength = data.Length;
                        using (Stream postStream = webRequest.GetRequestStream())
                        {
                            // Send the data.
                            postStream.Write(data, 0, data.Length);
                            postStream.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        return this.Json(ex.ToString(), JsonRequestBehavior.AllowGet);
                    }
                    _CatalogueService.insertImages(E_Obj);


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadAssetImg()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _CatalogueService.GetSingleItem(EquipId);
            string imgName = "";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            //string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (E_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.AssetImgs != null)
                        {
                            var NPimgs = E_Obj.AssetImages.AssetImgs;
                            if (NPimgs.Length > 0)
                            {
                                imgName = E_Obj.Materialcode + "_" + EquipId + "AI-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = E_Obj.Materialcode + "_" + EquipId + "AI-" + 1 + ".jpg";

                            var imgList = E_Obj.AssetImages.AssetImgs.ToList();
                            imgList.Add(apiurl + imgName);
                            E_Obj.AssetImages.AssetImgs = imgList.ToArray();
                        }
                        else
                        {
                            imgName = EquipId + "AI-" + 1 + ".jpg";
                            E_Obj.AssetImages.AssetImgs = new string[] { apiurl + imgName };
                        }

                    }
                    else
                    {
                        imgName = E_Obj.Materialcode + "_" + EquipId + "AI-" + 1 + ".jpg";
                        E_Obj.AssetImages = new MatImage();

                        E_Obj.AssetImages.AssetImgs = new string[] { apiurl + imgName };

                    }

                    string PostURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/upload";

                    try
                    {
                        int contentLength = file.ContentLength;
                        byte[] data = new byte[contentLength];
                        file.InputStream.Read(data, 0, contentLength);

                        // Prepare web request...
                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                        webRequest.Method = "POST";
                        webRequest.Headers.Add("file-name", imgName);
                        webRequest.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                        //   webRequest.ContentType = "multipart/form-data";
                        webRequest.ContentType = "image/jpeg";
                        webRequest.ContentLength = data.Length;
                        using (Stream postStream = webRequest.GetRequestStream())
                        {
                            // Send the data.
                            postStream.Write(data, 0, data.Length);
                            postStream.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        return this.Json(ex.ToString(), JsonRequestBehavior.AllowGet);
                    }
                    _CatalogueService.insertImages(E_Obj);


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult DeleteMaximoAssetImg(string FileName, string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _CatalogueService.GetSingleItem(Uid);
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.MatImgs != null)
                        {
                            var NPimgs = E_Obj.AssetImages.MatImgs;
                            if (NPimgs.Length > 0)
                            {
                                var strLst = new List<string>();
                                foreach (string str in NPimgs)
                                {
                                    if (str != FileName)
                                    {
                                        strLst.Add(str);
                                    }
                                }
                                E_Obj.AssetImages.MatImgs = strLst.ToArray();
                                _CatalogueService.insertImages(E_Obj);
                            }
                        }

                    }
                    WebRequest request = WebRequest.Create(delURL);
                    request.Method = "DELETE";
                    request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    return this.Json("Success: Image deleted", JsonRequestBehavior.AllowGet);
                }
                catch (Exception we)
                {
                    //  wRespStatusCode = ((HttpWebResponse)we.Response).StatusCode;
                    return this.Json("Error : " + we.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else return this.Json("Error : Image/Unique Id Not found", JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult DeleteAssetImg(string FileName, string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _CatalogueService.GetSingleItem(Uid);
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.AssetImgs != null)
                        {
                            var NPimgs = E_Obj.AssetImages.AssetImgs;
                            if (NPimgs.Length > 0)
                            {
                                var strLst = new List<string>();
                                foreach (string str in NPimgs)
                                {
                                    if (str != FileName)
                                    {
                                        strLst.Add(str);
                                    }
                                }
                                E_Obj.AssetImages.AssetImgs = strLst.ToArray();
                                _CatalogueService.insertImages(E_Obj);
                            }
                        }

                    }
                    WebRequest request = WebRequest.Create(delURL);
                    request.Method = "DELETE";
                    request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    return this.Json("Success: Image deleted", JsonRequestBehavior.AllowGet);
                }
                catch (Exception we)
                {
                    //  wRespStatusCode = ((HttpWebResponse)we.Response).StatusCode;
                    return this.Json("Error : " + we.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else return this.Json("Error : Image/Unique Id Not found", JsonRequestBehavior.AllowGet);

        }
        public JsonResult BulkNM()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkNM(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
    }

   
}