using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using Prosol.Common;
using System.Net;
using MongoDB.Bson;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using OfficeOpenXml;



namespace ProsolOnline.Controllers
{
    public class FARController : Controller
    {
        private readonly I_Asset _AssetService;
        private readonly IUserCreate _UserCreateService;
        private readonly ICatalogue _CatalogueService;
        private readonly ICharateristics _CharateristicService;

        public FARController(I_Asset AssetService, IUserCreate UserCreateService, ICatalogue CatalogueService, ICharateristics CharaService)
        {
            _AssetService = AssetService;
            _UserCreateService = UserCreateService;
            _CatalogueService = CatalogueService;
            _CharateristicService = CharaService;

        }
        // GET: FAR
        public ActionResult Index()
        {
            return View();
        }
      
        [Authorize]
        public ActionResult AssetRegistry()
        {
           return View();
           

        }
        [Authorize]
        public ActionResult FARoverview()
        {
            return View();
        }
        [Authorize]
        public ActionResult CustomerPage()
        {
            return View();


        }
        public ActionResult Excelview()
        {
            return View();


        }
        public ActionResult CustomerPage1()
        {
            return View();


        }
        [Authorize]
        public ActionResult PowerBI()
        {
            return View();


        }
        [Authorize]
        public ActionResult AssetDashboard()
        {
            return View();


        }
        [Authorize]
        public ActionResult AssetAssignwork()
        {
            //return View();
            //if (CheckAccess("Asset Assign") == 1)
                return View();
            //else if (CheckAccess("Asset Assign") == 0)
            //    return View("Denied");
            //else return View("Login");

        }
        [Authorize]
        public ActionResult AssetSearch()
        {
            return View();


        }
        [Authorize]
        //public ActionResult AssetReport()
        //{

        //    if (CheckAccess("Asset Report") == 1)
        //        return View();
        //    else if (CheckAccess("Asset Report") == 0)
        //        return View("Denied");
        //    else return View("Login");


        //}
        public ActionResult AssetReport()
        {
            if (CheckAccess("Asset Report") == 1)
                //return Redirect("https://adportsgroup-report.netlify.app/");
                return View();
            else if (CheckAccess("Asset Report") == 0)
                return View("Denied");
            else
                return View("Login");
        }

        [Authorize]
        public ActionResult CatAssetAssignwork()
        {
            return View();


        }
        [Authorize]
        public ActionResult Asset_BulkUpload()
        {
            return View();


        }
        [Authorize]
        public ActionResult AssetDictionary()
        {
            return View();


        }
        [Authorize]
        public ActionResult AssetQR()
        {
            return View();


        }
        [Authorize]
        public ActionResult AssetTools()
        {
            return View();
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

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult InsertAssetData()
        {
            var files = Request.Files.Count > 0 ? Request.Files : null;
            var assetvals = Request.Form["Asset"];
            assetvals = assetvals.Replace("Images:", "AssetImages:");
            var Catasset = Request.Form["Cat"];
            var Charas = Request.Form["attri"];
            var ListCharas = JsonConvert.DeserializeObject<List<AssetAttributes>>(Charas);
            var assetatt = JsonConvert.DeserializeObject<AssetAttributes>(Catasset);

            var assetvalues = JsonConvert.DeserializeObject<CatAssetModel>(assetvals);

            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

                var catasset = _AssetService.getasset(assetvalues.UniqueId);

            var AssetBom = Request.Form["AssetBOM"];
            var AssetBomLst = JsonConvert.DeserializeObject<List<AssetBomModel>>(AssetBom);

            if (files != null && files.Count > 0)
            {
                for (int J = 0; J < files.Count; J++)
                {

                    assetvalues.Attachment = assetvalues.Attachment + files[J].FileName + ',';

                }
                assetvalues.Attachment = assetvalues.Attachment.TrimEnd(',');
            }

            //Technical 

            catasset.AssetNo = assetvalues.AssetNo;
            catasset.EquipmentNo = assetvalues.EquipmentNo;
            catasset.pvAssetNo = assetvalues.pvAssetNo;
            catasset.UniqueId = assetvalues.UniqueId;
            catasset.Exchk = assetvalues.Exchk;
            catasset.Attachment = assetvalues.Attachment;
            catasset.Rework = "";
            catasset.Catalogue = assetvalues.Catalogue;
            catasset.Equipment_Short = assetvalues.Equipment_Short;
            catasset.Equipment_Long = assetvalues.Equipment_Long;
            catasset.EnrichedValue = assetvalues.EnrichedValue;
            catasset.MissingValue = assetvalues.MissingValue;
            catasset.RepeatedValue = assetvalues.RepeatedValue;
            catasset.Remarks = assetvalues.Remarks;
            catasset.Rework_Remarks = assetvalues.Rework_Remarks;
            catasset.Cat_Remarks = assetvalues.Cat_Remarks;
            catasset.Soureurl = assetvalues.Soureurl;
            catasset.Idle_Operational = assetvalues.Idle_Operational;
            catasset.assetConditionRemarks = assetvalues.assetConditionRemarks;
            catasset.Remarks = assetvalues.Remarks;
            catasset.Rework_Remarks = assetvalues.Rework_Remarks;
            catasset.AdditionalNotes = assetvalues.AdditionalNotes;
            catasset.NewTagNo = assetvalues.NewTagNo;
            catasset.OldTagNo = assetvalues.OldTagNo;
            catasset.VirtualTagNo = assetvalues.VirtualTagNo;
            catasset.Doc_Availability = assetvalues.Doc_Availability;
            catasset.Status = assetvalues.Status;

            if (catasset.ItemStatus == 4)
            {
                catasset.ItemStatus = 5;
                var Reviewer = new Prosol_UpdatedBy();

                Reviewer.UserId = usrInfo.Userid;
                Reviewer.Name = usrInfo.UserName;
                Reviewer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                catasset.Review = Reviewer;

            }

            if (catasset.ItemStatus == 2)
            {
                catasset.ItemStatus = 3;
                var cataloguer = new Prosol_UpdatedBy();

                cataloguer.UserId = usrInfo.Userid;
                cataloguer.Name = usrInfo.UserName;
                cataloguer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                catasset.Catalogue = cataloguer;
            }

            if (assetvalues.AssetImages != null)
            {
                var Asset_imgs = new AssetImage();
                if (assetvalues.AssetImages.AssetImage != null && assetvalues.AssetImages.AssetImage.Length > 0)
                    Asset_imgs.AssetImgs = assetvalues.AssetImages.AssetImage;
                if (assetvalues.AssetImages.MatImgs != null && assetvalues.AssetImages.MatImgs.Length > 0)
                    Asset_imgs.MatImgs = assetvalues.AssetImages.MatImgs;
                if (assetvalues.AssetImages.NamePlateImge != null && assetvalues.AssetImages.NamePlateImge.Length > 0)
                    Asset_imgs.NamePlateImge = assetvalues.AssetImages.NamePlateImge;
                if (assetvalues.AssetImages.NamePlateImgeTwo != null && assetvalues.AssetImages.NamePlateImgeTwo.Length > 0)
                    Asset_imgs.NamePlateImgeTwo = assetvalues.AssetImages.NamePlateImgeTwo;
                if (assetvalues.AssetImages.NamePlateText != null && assetvalues.AssetImages.NamePlateText.Length > 0)
                    Asset_imgs.NamePlateText = assetvalues.AssetImages.NamePlateText;
                if (assetvalues.AssetImages.NamePlateTextTwo != null && assetvalues.AssetImages.NamePlateTextTwo.Length > 0)
                    Asset_imgs.NamePlateTextTwo = assetvalues.AssetImages.NamePlateTextTwo;
                if (assetvalues.AssetImages.NameplateImgs != null && assetvalues.AssetImages.NameplateImgs.Length > 0)
                    Asset_imgs.NameplateImgs = assetvalues.AssetImages.NameplateImgs;
                if (assetvalues.AssetImages.NewTagImage != null && assetvalues.AssetImages.NewTagImage.Length > 0)
                    Asset_imgs.NewTagImage = assetvalues.AssetImages.NewTagImage;
                if (assetvalues.AssetImages.VirtualTagImage != null && assetvalues.AssetImages.VirtualTagImage.Length > 0)
                    Asset_imgs.VirtualTagImage = assetvalues.AssetImages.VirtualTagImage;
                if (assetvalues.AssetImages.OldTagImage != null && assetvalues.AssetImages.OldTagImage.Length > 0)
                    Asset_imgs.OldTagImage = assetvalues.AssetImages.OldTagImage;


                catasset.AssetImages = Asset_imgs;
            }

            if (assetvalues.GIS != null)
            {
                var gis = new Prosol_AssetGIS();
                gis.LattitudeStart = assetvalues.GIS.LattitudeStart;
                gis.LattitudeEnd = assetvalues.GIS.LattitudeEnd;
                gis.LongitudeStart = assetvalues.GIS.LongitudeStart;
                gis.LongitudeEnd = assetvalues.GIS.LongitudeEnd;
                gis.Lat_LongLength = assetvalues.GIS.Lat_LongLength;
                catasset.GIS = gis;
            }
            if (assetvalues.AssetCondition != null)
            {
                var Asset_Cndt = new Prosol_AssetCondition();

                Asset_Cndt.Corrosion = assetvalues.AssetCondition.Corrosion;
                Asset_Cndt.Damage = assetvalues.AssetCondition.Damage;
                Asset_Cndt.Leakage = assetvalues.AssetCondition.Leakage;
                Asset_Cndt.Vibration = assetvalues.AssetCondition.Vibration;
                Asset_Cndt.Temparature = assetvalues.AssetCondition.Temparature;
                Asset_Cndt.Smell = assetvalues.AssetCondition.Smell;
                Asset_Cndt.Noise = assetvalues.AssetCondition.Noise;
                Asset_Cndt.Rank = assetvalues.AssetCondition.Rank;
                Asset_Cndt.Asset_Condition = assetvalues.AssetCondition.Asset_Condition;
                Asset_Cndt.CorrosionImage = assetvalues.AssetCondition.CorrosionImage;
                Asset_Cndt.DamageImage = assetvalues.AssetCondition.DamageImage;
                Asset_Cndt.LeakageImage = assetvalues.AssetCondition.LeakageImage;
                catasset.AssetCondition = Asset_Cndt;
            }

            bool mul_resilt = false;
            var assetattr = new Prosol_AssetAttributes();
            var assetattrr = new Prosol_AssetAttributes();
            assetattr.UniqueId = assetvalues.UniqueId;
            assetattr.Noun = assetatt.Noun;
            assetattr.Modifier = assetatt.Modifier;
            catasset.Noun = assetatt.Noun;
            catasset.Modifier = assetatt.Modifier;

            var nmDic = _CharateristicService.GetCharateristic(assetatt.Noun, assetatt.Modifier).ToList();
            if (nmDic.Count > 0)
            {
                var lstCharateristics = new List<Asset_AttributeList>();

                foreach (AssetAttributes LstAtt in ListCharas)
                {
                    foreach (var dicAtt in nmDic)
                    {
                        if (dicAtt.Characteristic == LstAtt.Characteristic)
                        {
                            var AttrMdl = new Asset_AttributeList();
                            AttrMdl.Characteristic = LstAtt.Characteristic;
                            if (LstAtt.Value != null && LstAtt.Value != "")
                            {
                                AttrMdl.Value = LstAtt.Value.Trim().ToUpper();
                                _AssetService.AddValue(LstAtt.Noun, LstAtt.Modifier, LstAtt.Characteristic, LstAtt.Value, LstAtt.Value, null,null);
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
                            break;
                        }
                    }
                }
                assetattr.Characterisitics = lstCharateristics;
            }
            //Attribute
            var lstCharateristics1 = new List<Asset_AttributeList>();
            if (ListCharas != null)
            {

                foreach (AssetAttributes LstAtt in ListCharas)
                {
                    var AttrMdl = new Asset_AttributeList();
                    AttrMdl.Characteristic = LstAtt.Characteristic;
                    AttrMdl.Value = LstAtt.Value;
                    AttrMdl.UOM = LstAtt.UOM;
                    AttrMdl.Squence = LstAtt.Squence;
                    AttrMdl.ShortSquence = LstAtt.ShortSquence;
                    AttrMdl.Source = LstAtt.Source;
                    lstCharateristics1.Add(AttrMdl);

                }
            }
            catasset.Characteristics = lstCharateristics1;
            var slRes = _AssetService.GenerateShortLong(catasset);
            catasset.Equipment_Short = slRes[0];
            catasset.Equipment_Long = slRes[1];
            catasset.Characteristics = null;


            //ERP

            catasset.Plant = assetvalues.Plant;
            catasset.AssetNo = assetvalues.AssetNo;
            catasset.AssetQRCode = assetvalues.AssetQRCode;
            catasset.Description = assetvalues.Description;
            catasset.Func_Location = assetvalues.Func_Location;
            catasset.Equ_Category = assetvalues.Equ_Category;
            catasset.TechIdentNo = assetvalues.TechIdentNo;
            catasset.Parent = assetvalues.Parent;
            catasset.Org_Code = assetvalues.Org_Code;
            catasset.CostCenter = assetvalues.CostCenter;
            catasset.CostCenter_Desc = assetvalues.CostCenter_Desc;
            catasset.ABC_Indicator = assetvalues.ABC_Indicator;
            catasset.Equ_Category = assetvalues.Equ_Category;
            catasset.MainWorkCenter = assetvalues.MainWorkCenter;
            catasset.ObjType = assetvalues.ObjType;
            catasset.System = assetvalues.System;
            catasset.PartNo = assetvalues.PartNo;
            catasset.SerialNo = assetvalues.SerialNo;
            catasset.ModelNo = assetvalues.ModelNo;
            catasset.ModelYear = assetvalues.ModelYear;
            catasset.Manufacturer = assetvalues.Manufacturer;
            catasset.MfrCountry = assetvalues.MfrCountry;
            catasset.MfrYear = assetvalues.MfrYear;
            catasset.AssCondition = assetvalues.AssCondition;
            catasset.PID_Number = assetvalues.PID_Number;
            catasset.PID_Desc = assetvalues.PID_Desc;
            catasset.Section_Number = assetvalues.Section_Number;
            catasset.Section_Desc = assetvalues.Section_Desc;
            catasset.Discipline = assetvalues.Discipline;
            catasset.AdditionalInfo = assetvalues.AdditionalInfo;
            mul_resilt = _AssetService.Insertasset(catasset, assetattr, files);

            //Asset BOM
            var ListdbBOm = new List<Prosol_AssetBOM>();
            if (AssetBomLst.Count > 0)
            {
                foreach (var mdl in AssetBomLst)
                {

                    var tmpBom = new Prosol_AssetBOM();

                     tmpBom._id = string.IsNullOrEmpty(mdl._id)?new MongoDB.Bson.ObjectId(): new MongoDB.Bson.ObjectId(mdl._id);
                    tmpBom.UniqueId = mdl.UniqueId;
                    tmpBom.BOMName = mdl.BOMName;
                    tmpBom.Description = mdl.Description;
                    tmpBom.Qty = mdl.Qty;
                    tmpBom.UOM = mdl.UOM;
                    ListdbBOm.Add(tmpBom);
                }
                _AssetService.UpdateAssetBom(ListdbBOm);
            }


            return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);


        }
        //public JsonResult GetFARInfo()
        //{
        //   var resFar= _AssetService.GetFARInfo();
        //    if (resFar.Count > 0)
        //    {
        //        int estimate = 0, plan = 0, actual = 0;
        //        decimal planper = 0, actualper = 0;
        //        foreach(var d in resFar)
        //        {
        //            d.UpdatedOn =Convert.ToDateTime( DateTime.Parse(Convert.ToString(d.UpdatedOn)).ToString("MM/dd/yyyy HH:MM"));
        //            estimate = estimate + Convert.ToInt32(d.Estimated);
        //            plan = plan + Convert.ToInt32(d.plannedComplete);
        //            actual = actual + Convert.ToInt32(d.ActualComplete);
        //            //   planper = planper + Convert.ToDecimal(d.PlannedPerc);
        //            // actualper = actualper + Convert.ToDecimal(d.ActualPerc);
        //            d.Estimated = Convert.ToInt32(d.Estimated).ToString("#,##0");
        //            d.plannedComplete = Convert.ToInt32( d.plannedComplete).ToString("#,##0");
        //            d.ActualComplete = Convert.ToInt32(d.ActualComplete).ToString("#,##0");

        //        }
        //        var mdl = new prosol_FARdashboard();
        //        mdl.Business = "Total";
        //        mdl.Category = "Total";
        //        mdl.Estimated = estimate.ToString("#,##0");
        //        mdl.plannedComplete = plan.ToString("#,##0");
        //        mdl.ActualComplete = actual.ToString("#,##0");

        //        var p = ((Convert.ToDecimal(plan) / Convert.ToDecimal(estimate)) * 100);
        //        mdl.PlannedPerc = Convert.ToDecimal(p).ToString("F");
              

        //        var a = ((Convert.ToDecimal(actual) / Convert.ToDecimal(estimate)) * 100);
        //        mdl.ActualPerc = Convert.ToDecimal(a).ToString("F");
                
        //        resFar.Add(mdl);


        //    }
        //    return this.Json(resFar, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult getCityList(string City)
        {
            var objList = _AssetService.getAllCities(City);
            
            var Citylst = new List<AssetCity>();

            foreach (Prosol_City mdl in objList)
            {
                var obj = new AssetCity();
                obj.Region_Id = mdl.Region_Id;
                obj.City = mdl.City;
                obj.id = mdl._id.ToString();


                Citylst.Add(obj);

            }
          
            return this.Json(Citylst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFarMaster()
        {
            var masterLst = _AssetService.GetFarMaster();
            var farLst = new List<Prosol_FARRepository>();
            foreach(var mas in masterLst)
            {
                var far = new Prosol_FARRepository();
                far.FARId = mas.FARId;
                far.Region = mas.Region;
                far.AssetDesc = mas.AssetDesc;
                far.Islive = mas.Islive;
                farLst.Add(far);
            }
            var jsonResult = Json(farLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetSiteMaster()
        {
            var masterLst = _AssetService.GetSiteMaster();
            var siteLst = new List<Prosol_SiteMaster>();
            foreach(var mas in masterLst)
            {
                var site = new Prosol_SiteMaster();
                site.SiteId = mas.SiteId;
                site.Cluster = mas.Cluster;
                site.HighLevelLocation = mas.HighLevelLocation;
                site.Islive = mas.Islive;
                siteLst.Add(site);
            }
            var jsonResult = Json(siteLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetLocMaster()
        {
            var masterLst = _AssetService.GetLocationMaster();
            var locLst = new List<Prosol_Location>();
            foreach(var mas in masterLst)
            {
                var loc = new Prosol_Location();
                loc.Location = mas.Location;
                loc.LocationHierarchy = mas.LocationHierarchy;
                loc.Islive = mas.Islive;
                locLst.Add(loc);
            }
            var jsonResult = Json(locLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetAssetTypeMaster()
        {
            var masterLst = _AssetService.GetAssetTypeMaster();
            var atLst = new List<Prosol_AssetTypeMaster>();
            foreach(var mas in masterLst)
            {
                var at = new Prosol_AssetTypeMaster();
                at.AssetType = mas.AssetType;
                at.ClassificationHierarchyDesc = mas.ClassificationHierarchyDesc;
                at.FailureCode = mas.FailureCode;
                at.Islive = mas.Islive;
                atLst.Add(at);
            }
            var jsonResult = Json(atLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetTarMaster()
        {
            var masterLst = _AssetService.GetTarMaster();
            var jsonResult = Json(masterLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetMfr(string Label,string Term)
        {
            var masterLst = _AssetService.GetMfrMaster().Where(i => i.Label == Label && (i.Code.Contains(Term)||i.Title.Contains(Term))).ToList();
            var jsonResult = Json(masterLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetFuncLoc()
        {
            var masterLst = _AssetService.GetFuncLoc();
            var jsonResult = Json(masterLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult InsertMfr()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_FARMaster>(obj);
            var masterLst = _AssetService.InsertMfr(Model);
            var jsonResult = Json(masterLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetBusiness()
        {
            var objList = _AssetService.getAllCommonMaster();
            
            var lst = new List<AssetBusiness>();
            var newlst = new assetCommon();
            foreach (Prosol_Business mdl in objList.Businesses)
            {
                var obj = new AssetBusiness();
                obj.id = mdl._id.ToString();
                obj.BusinessName = mdl.BusinessName;

                lst.Add(obj);
               
            }
                newlst.Businesses = lst;

            var Majorlst = new List<AssetMajorClass>();

            foreach (Prosol_MajorClass mdl in objList.MajorClasses)
            {
                var obj = new AssetMajorClass();
                obj.Business_id = mdl.Business_id;
                obj.id = mdl._id.ToString();
                obj.MajorClass = mdl.MajorClass;

                Majorlst.Add(obj);

            }
            newlst.MajorClasses = Majorlst;

            var Minorlst = new List<AssetMinorClass>();

            //foreach (Prosol_MinorClass mdl in objList.MinorClasses)
            //{
            //    var obj = new AssetMinorClass();
            //    obj.Major_id = mdl.Major_id;
            //    obj.id = mdl._id.ToString();
            //    obj.MinorClass = mdl.MinorClass;

            //    Minorlst.Add(obj);

            //}
            newlst.MinorClasses = Minorlst;

            var Regionlst = new List<AssetRegion>();

            foreach (Prosol_Region mdl in objList.Regions)
            {
                var obj = new AssetRegion();
                obj.Region = mdl.Region;
                obj.id = mdl._id.ToString();
             

                Regionlst.Add(obj);

            }
            newlst.Regions = Regionlst;

            var Citylst = new List<AssetCity>();

            foreach (Prosol_City mdl in objList.Cities)
            {
                var obj = new AssetCity();
                obj.Region_Id = mdl.Region_Id;
                obj.City = mdl.City;
                obj.id = mdl._id.ToString();


                Citylst.Add(obj);

            }
            newlst.Cities = Citylst;

            var Arealst = new List<AssetArea>();

            foreach (Prosol_Area mdl in objList.Areas)
            {
                var obj = new AssetArea();
                obj.City_Id = mdl.City_Id;
                obj.Area= mdl.Area;
                obj.id = mdl._id.ToString();


                Arealst.Add(obj);

            }
            newlst.Areas = Arealst;

            var SubArealst = new List<AssetsubArea>();

            foreach (Prosol_SubArea mdl in objList.SubAreas)
            {
                var obj = new AssetsubArea();
                obj.Area_Id = mdl.Area_Id;
                obj.SubArea = mdl.SubArea;
                obj.id = mdl._id.ToString();


                SubArealst.Add(obj);

            }
            newlst.SubAreas = SubArealst;


            var EquipClasslst = new List<AssetEquipClass>();

            foreach (Prosol_EquipmentClass mdl in objList.EquipmentClasses)
            {
                var obj = new AssetEquipClass();
                obj.EquipmentClass = mdl.EquipmentClass;
                obj.id = mdl._id.ToString();
                


                EquipClasslst.Add(obj);

            }
            newlst.EquipmentClasses = EquipClasslst;

            var Equiptypelst = new List<AssetEquipType>();
            foreach (Prosol_EquipmentType mdl in objList.EquipmentTypes)
            {
                var obj = new AssetEquipType();
                obj.EquClass_Id = mdl.EquClass_Id;
                obj.EquipmentType = mdl.EquipmentType;
                obj.id = mdl._id.ToString();
                Equiptypelst.Add(obj);

            }
            newlst.EquipmentTypes = Equiptypelst;


            var Identilst = new List<AssetIdentifier>();
            foreach (Prosol_Identifier mdl in objList.Identifiers)
            {
                var obj = new AssetIdentifier();
                obj.fClass_Id = mdl.fClass_Id;
                obj.Identifier = mdl.Identifier;
                obj.id = mdl._id.ToString();
                Identilst.Add(obj);

            }
            newlst.Identifiers = Identilst;
           // return this.Json(newlst, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(newlst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
       
        [Authorize]
        public JsonResult GetAssetDataList()
        {

            var Assetlist = _AssetService.GetAssetDataList(Convert.ToString(Session["userid"])).ToList();
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            var lstCatalogue = new List<CatAssetModel>();

            var AllMaster = _AssetService.getAllCommonMaster();

            foreach (Prosol_AssetMaster assetvalues in Assetlist)
            {
                var catasset = new CatAssetModel();

                catasset.ItemStatus = assetvalues.ItemStatus;

                catasset.UniqueId = assetvalues.UniqueId;
                catasset.AssetNo = assetvalues.AssetNo;
                catasset.Rework = assetvalues.Rework;

                catasset.PVuser = new Prosol_UpdatedBy();
                if (assetvalues.PVuser != null)
                {
                    catasset.PVuser.Name = assetvalues.PVuser.Name;
                    catasset.PVuser.UpdatedOn = assetvalues.PVuser.UpdatedOn;
                }

                catasset.AssetNo = assetvalues.AssetNo;
                catasset.Description = assetvalues.Description;
                catasset.Parent = assetvalues.Parent;
                catasset.Status = assetvalues.Status;
                catasset.SerialNo = assetvalues.SerialNo;
                catasset.ModelNo = assetvalues.ModelNo;
                catasset.ModelYear = assetvalues.ModelYear;
                catasset.CostCenter = assetvalues.CostCenter;
                catasset.MaximoAssetInfo = assetvalues.MaximoAssetInfo;
                catasset.Remarks = assetvalues.Remarks;
                catasset.AssetQRCode = assetvalues.AssetQRCode;
                catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                catasset.NewTagNo = assetvalues.NewTagNo;


                if (assetvalues.AssetImages != null)
                {
                    var Asset_imgs = new AssetImages();
                    if (assetvalues.AssetImages.AssetImgs != null && assetvalues.AssetImages.AssetImgs.Length > 0)
                        Asset_imgs.AssetImage = assetvalues.AssetImages.AssetImgs;
                    if (assetvalues.AssetImages.MatImgs != null && assetvalues.AssetImages.MatImgs.Length > 0)
                        Asset_imgs.MatImgs = assetvalues.AssetImages.MatImgs;
                    if (assetvalues.AssetImages.NamePlateImge != null && assetvalues.AssetImages.NamePlateImge.Length > 0)
                        Asset_imgs.NamePlateImge = assetvalues.AssetImages.NamePlateImge;
                    if (assetvalues.AssetImages.NamePlateImgeTwo != null && assetvalues.AssetImages.NamePlateImgeTwo.Length > 0)
                        Asset_imgs.NamePlateImgeTwo = assetvalues.AssetImages.NamePlateImgeTwo;
                    if (assetvalues.AssetImages.NamePlateText != null && assetvalues.AssetImages.NamePlateText.Length > 0)
                        Asset_imgs.NamePlateText = assetvalues.AssetImages.NamePlateText;
                    if (assetvalues.AssetImages.NamePlateTextTwo != null && assetvalues.AssetImages.NamePlateTextTwo.Length > 0)
                        Asset_imgs.NamePlateTextTwo = assetvalues.AssetImages.NamePlateTextTwo;
                    if (assetvalues.AssetImages.NameplateImgs != null && assetvalues.AssetImages.NameplateImgs.Length > 0)
                        Asset_imgs.NameplateImgs = assetvalues.AssetImages.NameplateImgs;
                    if (assetvalues.AssetImages.NewTagImage != null && assetvalues.AssetImages.NewTagImage.Length > 0)
                        Asset_imgs.NewTagImage = assetvalues.AssetImages.NewTagImage;
                    if (assetvalues.AssetImages.VirtualTagImage != null && assetvalues.AssetImages.VirtualTagImage.Length > 0)
                        Asset_imgs.VirtualTagImage = assetvalues.AssetImages.VirtualTagImage;
                    if (assetvalues.AssetImages.OldTagImage != null && assetvalues .AssetImages.OldTagImage.Length > 0)
                        Asset_imgs.OldTagImage = assetvalues.AssetImages.OldTagImage;


                    catasset.AssetImages = Asset_imgs;
                }

                if (assetvalues.GIS != null)
                {
                    var gis = new AssetGIS();
                    gis.LattitudeStart = assetvalues.GIS.LattitudeStart;
                    gis.LattitudeEnd = assetvalues.GIS.LattitudeEnd;
                    gis.LongitudeStart = assetvalues.GIS.LongitudeStart;
                    gis.LongitudeEnd = assetvalues.GIS.LongitudeEnd;
                    gis.Lat_LongLength = assetvalues.GIS.Lat_LongLength;
                    catasset.GIS = gis;
                }
                if (assetvalues.AssetCondition != null)
                {
                    var Asset_Cndt = new AssetCondition();

                    Asset_Cndt.Corrosion = assetvalues.AssetCondition.Corrosion;
                    Asset_Cndt.Damage = assetvalues.AssetCondition.Damage;
                    Asset_Cndt.Leakage = assetvalues.AssetCondition.Leakage;
                    Asset_Cndt.Vibration = assetvalues.AssetCondition.Vibration;
                    Asset_Cndt.Temparature = assetvalues.AssetCondition.Temparature;
                    Asset_Cndt.Smell = assetvalues.AssetCondition.Smell;
                    Asset_Cndt.Noise = assetvalues.AssetCondition.Noise;
                    Asset_Cndt.Rank = assetvalues.AssetCondition.Rank;
                    Asset_Cndt.Asset_Condition = assetvalues.AssetCondition.Asset_Condition;
                    Asset_Cndt.CorrosionImage = assetvalues.AssetCondition.CorrosionImage;
                    Asset_Cndt.DamageImage = assetvalues.AssetCondition.DamageImage;
                    Asset_Cndt.LeakageImage = assetvalues.AssetCondition.LeakageImage;
                    catasset.AssetCondition = Asset_Cndt;
                }

                catasset.Catalogue = assetvalues.Catalogue;


                var catname = new Prosol_UpdatedBy();
                catname.Name = assetvalues.Catalogue.Name;
                catasset.Catalogue = catname;

                catasset.Review = new Prosol_UpdatedBy();
                catasset.Review.Name = assetvalues.Review != null ? assetvalues.Review.Name : "";
                catasset.Plant = assetvalues.Plant;
                catasset.AssetNo = assetvalues.AssetNo;
                catasset.AssetQRCode = assetvalues.AssetQRCode;
                catasset.Description = assetvalues.Description;
                catasset.Func_Location = assetvalues.Func_Location;
                catasset.Equ_Category = assetvalues.Equ_Category;
                catasset.TechIdentNo = assetvalues.TechIdentNo;
                catasset.Parent = assetvalues.Parent;
                catasset.Org_Code = assetvalues.Org_Code;
                catasset.CostCenter = assetvalues.CostCenter;
                catasset.CostCenter_Desc = assetvalues.CostCenter_Desc;
                catasset.ABC_Indicator = assetvalues.ABC_Indicator;
                catasset.Equ_Category = assetvalues.Equ_Category;
                catasset.MainWorkCenter = assetvalues.MainWorkCenter;
                catasset.ObjType = assetvalues.ObjType;
                catasset.System = assetvalues.System;
                catasset.PartNo = assetvalues.PartNo;
                catasset.SerialNo = assetvalues.SerialNo;
                catasset.ModelNo = assetvalues.ModelNo;
                catasset.ModelYear = assetvalues.ModelYear;
                catasset.Manufacturer = assetvalues.Manufacturer;
                catasset.MfrCountry = assetvalues.MfrCountry;
                catasset.MfrYear = assetvalues.MfrYear;
                catasset.AssCondition = assetvalues.AssCondition;
                catasset.PID_Number = assetvalues.PID_Number;
                catasset.PID_Desc = assetvalues.PID_Desc;
                catasset.Section_Number = assetvalues.Section_Number;
                catasset.Section_Desc = assetvalues.Section_Desc;
                catasset.Discipline = assetvalues.Discipline;

                lstCatalogue.Add(catasset);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        [Authorize]
        public JsonResult getAssetBom(string id)
        {
            //var Lst = _AssetService.getAllAssetBom(equipmentId);
            var Lst = _AssetService.GetBOM(id);
            var lstbomUI = new List<AssetBomModel>();
            if (Lst.Count > 0)
            {

                foreach (var mdl in Lst)
                {
                    var uibom = new AssetBomModel();
                    uibom._id = mdl._id.ToString();

                    uibom.BOMId = mdl.BOMId;
                    uibom.BOMDesc = mdl.BOMDesc;
                    uibom.BOMLongDesc = mdl.BOMLongDesc;
                    uibom.AssemblyId = mdl.AssemblyId;
                    uibom.AssemblyDesc = mdl.AssemblyDesc;
                    uibom.AssemblyLongDesc = mdl.AssemblyLongDesc;
                    uibom.ComponentId = mdl.ComponentId;
                    uibom.ComponentDesc = mdl.ComponentDesc;
                    uibom.ComponentLongDesc = mdl.ComponentLongDesc;
                    uibom.UOM = mdl.UOM;
                    uibom.Sequence = mdl.Sequence;
                    uibom.Quantity = mdl.Quantity;
                    uibom.Category = mdl.Category;
                    uibom.Func_Location = mdl.Func_Location;
                    uibom.TechIdentNo = mdl.TechIdentNo;

                    uibom.UniqueId = mdl.UniqueId;
                    uibom.EquipmentId = mdl.EquipmentId;
                    uibom.TagNo = mdl.TagNo;
                    uibom.Barcode = mdl.Barcode;
                    uibom.OldTag = mdl.OldTag;
                    uibom.BOMName = mdl.BOMName;
                    uibom.Description = mdl.Description;
                    uibom.Qty = mdl.Qty;
                    uibom.UOM = mdl.UOM;
                    uibom.NamePlateImg = mdl.NamePlateImg;
                    uibom.OldTagImg = mdl.OldTagImg;
                    uibom.BOMImg = mdl.BOMImg;
                    uibom.BarCodeImg = mdl.BarCodeImg;
                    lstbomUI.Add(uibom);


                }

            }

            return this.Json(lstbomUI, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getFLBom(string id)
        {
            var Lst = _AssetService.getFLBom(id);

            return this.Json(Lst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult getNestedFL(string id)
        {
            var Lst = _AssetService.GetFL(id);

            //var lstFL = Lst.FindAll(l => l.FunctLocation.StartsWith(id)).Select(s => s.FunctLocation).ToList();
            var lstFL = Lst.FindAll(l => l.FunctLocation.StartsWith(id)).ToList();

            return this.Json(lstFL, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetAttributeinfo(string UniqueId)
        {
            var assetvalues = _AssetService.GetAssetInfo(UniqueId);
            var attributevalues = _AssetService.GetAttributeInfo(UniqueId);
            var lstCharateristics = new List<CatAsset_AttributeList>();
            var exlstCharateristics = new List<CatAsset_AttributeList>();
            var assetat = new AssetAttributes();
            if (attributevalues != null && attributevalues.Characterisitics != null)
            {
                foreach (Asset_AttributeList pattri in attributevalues.Characterisitics)
                {
                    var AttrMdl = new CatAsset_AttributeList();
                    AttrMdl.Characteristic = pattri.Characteristic;
                    AttrMdl.Value = pattri.Value;
                    AttrMdl.UOM = pattri.UOM;
                    AttrMdl.Squence = pattri.Squence;
                    AttrMdl.ShortSquence = pattri.ShortSquence;
                    AttrMdl.Source = pattri.Source;
                    AttrMdl.SourceUrl = pattri.SourceUrl;
                    if (assetvalues.ItemStatus > 3 && AttrMdl.Value != "" && AttrMdl.Value != null)
                    {
                        string abbrVal = _CatalogueService.CheckValue(attributevalues.Noun, attributevalues.Modifier, pattri.Characteristic, pattri.Value);
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
                assetat.Characterisitics = lstCharateristics;
                assetat.Noun = attributevalues.Noun;
                assetat.Modifier = attributevalues.Modifier;

            }
            if (attributevalues != null && attributevalues.exCharacterisitics != null)
            {
                foreach (Asset_AttributeList pattri in attributevalues.exCharacterisitics)
                {
                    var AttrMdl = new CatAsset_AttributeList();
                    AttrMdl.Characteristic = pattri.Characteristic;
                    AttrMdl.Value = pattri.Value;
                    AttrMdl.UOM = pattri.UOM;
                    AttrMdl.Squence = pattri.Squence;
                    AttrMdl.ShortSquence = pattri.ShortSquence;
                    AttrMdl.Source = pattri.Source;
                    AttrMdl.SourceUrl = pattri.SourceUrl;
                    if (assetvalues.ItemStatus > 3 && AttrMdl.Value != "" && AttrMdl.Value != null)
                    {
                        string abbrVal = _CatalogueService.CheckValue(attributevalues.Noun, attributevalues.Modifier, pattri.Characteristic, pattri.Value);
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
                    exlstCharateristics.Add(AttrMdl);
                }
                assetat.exCharacterisitics = exlstCharateristics;
                assetat.exNoun = attributevalues.exNoun;
                assetat.exModifier = attributevalues.exModifier;

            }

            return this.Json(assetat, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetAssetinfo(string UniqueId)
            {
            var assetvalues = _AssetService.GetAssetInfo(UniqueId);
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            var catasset = new CatAssetModel();
            if (catasset != null)
            {

                catasset.ItemStatus = assetvalues.ItemStatus;
                catasset.Exchk = assetvalues.Exchk;
                    catasset.Attachment = assetvalues.Attachment;
                var gisinfo = new AssetGIS();
                if (assetvalues.GIS != null)
                {
                    gisinfo.LattitudeStart = assetvalues.GIS.LattitudeStart;
                    gisinfo.LattitudeEnd = assetvalues.GIS.LattitudeEnd;
                    gisinfo.LongitudeStart = assetvalues.GIS.LongitudeStart;
                    gisinfo.LongitudeEnd = assetvalues.GIS.LongitudeEnd;
                    gisinfo.Lat_LongLength = assetvalues.GIS.Lat_LongLength;
                    catasset.GIS = gisinfo;
                }

                var building = new Building();

                var Img = new AssetImages();
                if (assetvalues.AssetImages != null)
                {
                    Img.AssetImage = assetvalues.AssetImages.AssetImgs;
                    Img.MatImgs = assetvalues.AssetImages.MatImgs;
                    Img.OldTagImage = assetvalues.AssetImages.OldTagImage;
                    Img.NewTagImage = assetvalues.AssetImages.NewTagImage;
                    Img.VirtualTagImage = assetvalues.AssetImages.VirtualTagImage;
                    Img.NamePlateImge = assetvalues.AssetImages.NamePlateImge;
                    Img.NamePlateImgeTwo = assetvalues.AssetImages.NamePlateImgeTwo;
                    Img.NamePlateText = assetvalues.AssetImages.NamePlateText;
                    Img.NamePlateTextTwo = assetvalues.AssetImages.NamePlateTextTwo;
                    //Img.AssetImage = assetvalues.AssetImages.AssetImage;
                    catasset.AssetImages = Img;
                }

                var condition = new AssetCondition();
                if (assetvalues.AssetCondition != null)
                {
                    condition.Corrosion = assetvalues.AssetCondition.Corrosion;
                    condition.CorrosionImage = assetvalues.AssetCondition.CorrosionImage;
                    condition.Damage = assetvalues.AssetCondition.Damage;
                    condition.DamageImage = assetvalues.AssetCondition.DamageImage;

                    condition.Leakage = assetvalues.AssetCondition.Leakage;
                    condition.LeakageImage = assetvalues.AssetCondition.LeakageImage;

                    condition.Noise = assetvalues.AssetCondition.Noise;
                    condition.Rank = assetvalues.AssetCondition.Rank;
                    condition.Smell = assetvalues.AssetCondition.Smell;
                    condition.Temparature = assetvalues.AssetCondition.Temparature;
                    condition.Vibration = assetvalues.AssetCondition.Vibration;
                    condition.Asset_Condition = assetvalues.AssetCondition.Asset_Condition;

                    catasset.AssetCondition = condition;
                }
                catasset.UniqueId = assetvalues.UniqueId;
                catasset.AssetNo = assetvalues.AssetNo;
                catasset.Rework_Remarks = assetvalues.Rework_Remarks;
                catasset.Cat_Remarks = assetvalues.Cat_Remarks;
                catasset.Rework = assetvalues.Rework;

                catasset.PVuser = assetvalues.PVuser;

                catasset.Status = assetvalues.Status;

                //FAR&TAR

                catasset.AssetNo = assetvalues.AssetNo;
                catasset.EquipmentNo = assetvalues.EquipmentNo;
                catasset.pvAssetNo = assetvalues.pvAssetNo;
                catasset.AssetQRCode = assetvalues.AssetQRCode;
                catasset.Description = assetvalues.Description;
                catasset.SerialNo = assetvalues.SerialNo;
                catasset.ModelNo = assetvalues.ModelNo;
                catasset.ModelYear = assetvalues.ModelYear;
                catasset.Catalogue = assetvalues.Catalogue;
                catasset.MaximoAssetInfo = assetvalues.MaximoAssetInfo;
                catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                catasset.Remarks = assetvalues.Remarks;
                catasset.Catalogue = assetvalues.Catalogue;
                catasset.OldTagNo = assetvalues.OldTagNo;
                catasset.NewTagNo = assetvalues.NewTagNo;
                catasset.Equipment_Short = assetvalues.Equipment_Short;
                catasset.Equipment_Long = assetvalues.Equipment_Long;
                catasset.EnrichedValue = assetvalues.EnrichedValue;
                catasset.MissingValue = assetvalues.MissingValue;
                catasset.RepeatedValue = assetvalues.RepeatedValue;
                catasset.Idle_Operational = assetvalues.Idle_Operational;
                catasset.Remarks = assetvalues.Remarks;
                catasset.Rework_Remarks = assetvalues.Rework_Remarks;
                catasset.Soureurl = assetvalues.Soureurl;
                catasset.Review = assetvalues.Review;
                catasset.Remarks = assetvalues.Remarks;
                catasset.Rework_Remarks = assetvalues.Rework_Remarks;
                catasset.assetConditionRemarks = assetvalues.assetConditionRemarks;
                catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                catasset.Description_ = assetvalues.Description_;
                catasset.Doc_Availability = assetvalues.Doc_Availability;
                catasset.VirtualTagNo = assetvalues.VirtualTagNo;

                catasset.Plant = assetvalues.Plant;
                catasset.AssetNo = assetvalues.AssetNo;
                catasset.AssetQRCode = assetvalues.AssetQRCode;
                catasset.Description = assetvalues.Description;
                catasset.Func_Location = assetvalues.Func_Location;
                catasset.Equ_Category = assetvalues.Equ_Category;
                catasset.TechIdentNo = assetvalues.TechIdentNo;
                catasset.Parent = assetvalues.Parent;
                catasset.Org_Code = assetvalues.Org_Code;
                catasset.CostCenter = assetvalues.CostCenter;
                catasset.CostCenter_Desc = assetvalues.CostCenter_Desc;
                catasset.ABC_Indicator = assetvalues.ABC_Indicator;
                catasset.Equ_Category = assetvalues.Equ_Category;
                catasset.MainWorkCenter = assetvalues.MainWorkCenter;
                catasset.ObjType = assetvalues.ObjType;
                catasset.System = assetvalues.System;
                catasset.PartNo = assetvalues.PartNo;
                catasset.SerialNo = assetvalues.SerialNo;
                catasset.ModelNo = assetvalues.ModelNo;
                catasset.ModelYear = assetvalues.ModelYear;
                catasset.Manufacturer = assetvalues.Manufacturer;
                catasset.MfrCountry = assetvalues.MfrCountry;
                catasset.MfrYear = assetvalues.MfrYear;
                catasset.AssCondition = assetvalues.AssCondition;
                catasset.PID_Number = assetvalues.PID_Number;
                catasset.PID_Desc = assetvalues.PID_Desc;
                catasset.Section_Number = assetvalues.Section_Number;
                catasset.Section_Desc = assetvalues.Section_Desc;
                catasset.Discipline = assetvalues.Discipline;
                catasset.AdditionalInfo = assetvalues.AdditionalInfo;

                //BOM
                var bom = new BOM();
                if (assetvalues.Bom != null)
                {
                    bom.BOMId = assetvalues.Bom.BOMId;
                    bom.BOMDescription = assetvalues.Bom.BOMDescription;
                    if (assetvalues.Bom.Assembly != null)
                    {
                        var asmbLst = new List<ASSEMBLY>();
                        foreach (var asm in assetvalues.Bom.Assembly)
                        {
                            var asmb = new ASSEMBLY();
                            asmb.AssemblyId = asm.AssemblyId;
                            asmb.AssemblyDescription = asm.AssemblyDescription;
                            asmb.Quantity = asm.Quantity;
                            asmb.UOM = asm.UOM;
                            asmbLst.Add(asmb);
                        }
                        bom.Assembly = asmbLst;
                    }
                    if (assetvalues.Bom.Assembly != null)
                    {
                        var mtlLst = new List<MAT>();
                        foreach (var asm in assetvalues.Bom.Mat)
                        {
                            var mtl = new MAT();
                            mtl.Materialcode = asm.Materialcode;
                            mtl.MaterialDescription = asm.MaterialDescription;
                            mtl.Quantity = asm.Quantity;
                            mtl.UOM = asm.UOM;
                            mtl.Status = asm.Status;
                            mtlLst.Add(mtl);
                        }
                        bom.Mat = mtlLst;
                    }
                }

                catasset.Bom = bom;

            }
            return this.Json(catasset, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult InsertAssetDataList()
        {
            string uid = Convert.ToString(Session["UserId"]);
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            
           
            var catDatalist = Request.Unvalidated["AssetdataList"];
            var catModelList = JsonConvert.DeserializeObject<List<CatAssetModel>>(catDatalist);

        


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_AssetMaster>();
              
                foreach (CatAssetModel mdl in catModelList)
                {
                    var proCat = new Prosol_AssetMaster();

                    proCat.ItemStatus = mdl.ItemStatus;
                    proCat.Exchk = mdl.Exchk;
                    //proCat.ItemStatus = mdl.ItemStatus;
                    //proCat.EquipmentClass = mdl.EquipmentClass;

                    //proCat.EquipmentType = mdl.EquipmentType;
                    //proCat.FLOC_Code = mdl.FLOC_Code;
                    //proCat.FuncLocDesc = mdl.FuncLocDesc;

                    //proCat.Function = mdl.Function;
                    //proCat.Identifier = mdl.Identifier;
                    //proCat.OldFunLoc = mdl.OldFunLoc;
                    //proCat.EquipmentDesc = mdl.EquipmentDesc;
                    //proCat.Equipment_Short = mdl.Equipment_Short;
                    //proCat.Equipment_Long = mdl.Equipment_Long;

                    //proCat.Height = mdl.Height;

                    //var building = new Building();
                    //if (mdl.AssetBuilding != null)
                    //{
                    //    building.BuildingId = mdl.AssetBuilding.BuildingId;
                    //    building.BuildingName = mdl.AssetBuilding.BuildingName;
                    //    building.Length = mdl.AssetBuilding.Length;
                    //    building.Width = mdl.AssetBuilding.Width;
                    //    building.Height = mdl.AssetBuilding.Height;
                    //    building.Location = mdl.AssetBuilding.Location;
                    //    building.BuildingImage = mdl.AssetBuilding.BuildingImage;
                    //    proCat.building = building;
                    //}

                    if (mdl.AssetImages != null)
                    {
                        var Asset_imgs = new AssetImage();
                        if (mdl.AssetImages.AssetImage != null && mdl.AssetImages.AssetImage.Length > 0)
                            Asset_imgs.AssetImgs = mdl.AssetImages.AssetImage;
                        if (mdl.AssetImages.MatImgs != null && mdl.AssetImages.MatImgs.Length > 0)
                            Asset_imgs.MatImgs = mdl.AssetImages.MatImgs;
                        if (mdl.AssetImages.NamePlateImge != null && mdl.AssetImages.NamePlateImge.Length > 0)
                            Asset_imgs.NamePlateImge = mdl.AssetImages.NamePlateImge;
                        if (mdl.AssetImages.NamePlateImgeTwo != null && mdl.AssetImages.NamePlateImgeTwo.Length > 0)
                            Asset_imgs.NamePlateImgeTwo = mdl.AssetImages.NamePlateImgeTwo;
                        if (mdl.AssetImages.NamePlateText != null && mdl.AssetImages.NamePlateText.Length > 0)
                            Asset_imgs.NamePlateText = mdl.AssetImages.NamePlateText;
                        if (mdl.AssetImages.NamePlateTextTwo != null && mdl.AssetImages.NamePlateTextTwo.Length > 0)
                            Asset_imgs.NamePlateTextTwo = mdl.AssetImages.NamePlateTextTwo;
                        if (mdl.AssetImages.NameplateImgs != null && mdl.AssetImages.NameplateImgs.Length > 0)
                            Asset_imgs.NameplateImgs = mdl.AssetImages.NameplateImgs;
                        if (mdl.AssetImages.NewTagImage != null && mdl.AssetImages.NewTagImage.Length > 0)
                            Asset_imgs.NewTagImage = mdl.AssetImages.NewTagImage;
                        if (mdl.AssetImages.VirtualTagImage != null && mdl.AssetImages.VirtualTagImage.Length > 0)
                            Asset_imgs.VirtualTagImage = mdl.AssetImages.VirtualTagImage;
                        if (mdl.AssetImages.OldTagImage != null && mdl.AssetImages.OldTagImage.Length > 0)
                            Asset_imgs.OldTagImage = mdl.AssetImages.OldTagImage;


                        proCat.AssetImages = Asset_imgs;
                    }

                    if (mdl.GIS != null)
                    {
                        var gis = new Prosol_AssetGIS();
                        gis.LattitudeStart = mdl.GIS.LattitudeStart;
                        gis.LattitudeEnd = mdl.GIS.LattitudeEnd;
                        gis.LongitudeStart = mdl.GIS.LongitudeStart;
                        gis.LongitudeEnd = mdl.GIS.LongitudeEnd;
                        gis.Lat_LongLength = mdl.GIS.Lat_LongLength;
                        proCat.GIS = gis;
                    }
                    if (mdl.AssetCondition != null)
                    {
                        var Asset_Cndt = new Prosol_AssetCondition();

                        Asset_Cndt.Corrosion = mdl.AssetCondition.Corrosion;
                        Asset_Cndt.Damage = mdl.AssetCondition.Damage;
                        Asset_Cndt.Leakage = mdl.AssetCondition.Leakage;
                        Asset_Cndt.Vibration = mdl.AssetCondition.Vibration;
                        Asset_Cndt.Temparature = mdl.AssetCondition.Temparature;
                        Asset_Cndt.Smell = mdl.AssetCondition.Smell;
                        Asset_Cndt.Noise = mdl.AssetCondition.Noise;
                        Asset_Cndt.Rank = mdl.AssetCondition.Rank;
                        Asset_Cndt.Asset_Condition = mdl.AssetCondition.Asset_Condition;
                        Asset_Cndt.CorrosionImage = mdl.AssetCondition.CorrosionImage;
                        Asset_Cndt.DamageImage = mdl.AssetCondition.DamageImage;
                        Asset_Cndt.LeakageImage = mdl.AssetCondition.LeakageImage;
                        proCat.AssetCondition = Asset_Cndt;
                    }

                    //proCat.Length = mdl.Length;
                    //proCat.Location = mdl.Location;

                    //proCat.MajorClass = mdl.MajorClass;

                    //proCat.MinorClass = mdl.MinorClass;


                    //proCat.Identifier = mdl.Identifier;
                    //proCat.OldFunLoc = mdl.OldFunLoc;
                    //proCat.Quantity = mdl.Quantity;

                    //proCat.Region = mdl.Region;
                    //proCat.SubArea = mdl.SubArea;
                    //proCat.Area = mdl.Area;
                    //proCat.Remarks = mdl.Remarks;
                    //proCat.AdditionalInfo = mdl.AdditionalInfo;
                    proCat.AssetNo = mdl.AssetNo;
                    //proCat.UOM = mdl.UOM;
                    proCat.Rework_Remarks = mdl.Rework_Remarks;
                    proCat.Rework = mdl.Rework;
                    // proCat.Width = mdl.Width;
                    //proCat.YearOfInstall = mdl.YearOfInstall;
                    //proCat.YearOfMfr = mdl.YearOfMfr;

                    //proCat.Plant = mdl.Plant;
                    proCat.PVuser = mdl.PVuser;

                    //proCat.OldTagNo = mdl.OldTagNo;
                    //proCat.NewTagNo = mdl.NewTagNo;
                    //proCat.BOM = mdl.BOM;
                    //proCat.Idle_Operational = mdl.Idle_Operational;

                    //proCat.SiteName = mdl.SiteName;
                    //proCat.SiteType = mdl.SiteType;
                    //proCat.CompanyCode = mdl.CompanyCode;
                    //proCat.ObjectId = mdl.ObjectId;
                    //proCat.ObjectType = mdl.ObjectType;
                    //proCat.SAP_Equipment = mdl.SAP_Equipment;
                    //proCat.AssetNo = mdl.AssetNo;
                    //proCat.AssetSubNo = mdl.AssetSubNo;
                    //proCat.FuncLocDesc = mdl.FuncLocDesc;
                    //proCat.Location = mdl.Location;
                    //proCat.Barcode = mdl.Barcode;
                    //proCat.City = mdl.City;
                    //proCat.BuildingId = mdl.AssetBuilding.BuildingId;
                    //proCat.BuildingName = mdl.AssetBuilding.BuildingName;

                    //finance
                    //proCat.ABC_Indicator = mdl.ABC_Indicator;
                    //proCat.Accumulated_Depreciation = mdl.Accumulated_Depreciation;
                    //proCat.Addition = mdl.Addition;

                    //proCat.CostCenter = mdl.CostCenter;
                    //proCat.Depreciation_Startdate = mdl.Depreciation_Startdate != null ? DateTime.Parse(Convert.ToString(mdl.Depreciation_Startdate)).ToString("MM/dd/yyyy") : "";
                    //proCat.Depreciation_Year = mdl.Depreciation_Year;
                    //proCat.Department = mdl.Department;
                    //proCat.MaintanancePlant = mdl.MaintanancePlant;
                    //proCat.MainWork_Center = mdl.MainWork_Center;

                    //proCat.NetBook_Value = mdl.NetBook_Value;
                    //proCat.OpeningBalance = mdl.OpeningBalance;
                    //proCat.PlannerGroup = mdl.PlannerGroup;

                    //proCat.PlanningPlant = mdl.PlanningPlant;
                    //proCat.PlantSection = mdl.PlantSection;
                    //proCat.Retirement = mdl.Retirement;
                    //proCat.Salvage_ResidualValue = mdl.Salvage_ResidualValue;
                    //proCat.Status = mdl.Status;

                    //proCat.Transaction_date = mdl.Transaction_date != null ? DateTime.Parse(Convert.ToString(mdl.Transaction_date)).ToString("MM/dd/yyyy") : "";
                    //proCat.Warranty_ExpiryDate = mdl.Warranty_ExpiryDate != null ? DateTime.Parse(Convert.ToString(mdl.Warranty_ExpiryDate)).ToString("MM/dd/yyyy") : "";
                    //proCat.Business = mdl.Business;
                    //proCat.CreatedOn = mdl.CreatedOn;

                    // proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    //FAR&TAR

                    proCat.AssetNo = mdl.AssetNo;
                    proCat.EquipmentNo = mdl.EquipmentNo;
                    proCat.UniqueId = mdl.UniqueId;
                    proCat.pvAssetNo = mdl.pvAssetNo;
                    proCat.Description = mdl.Description;
                    proCat.Parent = mdl.Parent;
                    proCat.SerialNo = mdl.SerialNo;
                    proCat.ModelNo = mdl.ModelNo;
                    proCat.ModelYear = mdl.ModelYear;
                    proCat.Manufacturer = mdl.Manufacturer;
                    proCat.Catalogue = mdl.Catalogue;
                    proCat.NewTagNo = mdl.NewTagNo;
                    proCat.OldTagNo = mdl.OldTagNo;
                    proCat.AdditionalNotes = mdl.AdditionalNotes;
                    proCat.Remarks = mdl.Rework;
                    proCat.Rework_Remarks = mdl.Rework_Remarks;
                    proCat.Cat_Remarks = mdl.Cat_Remarks;
                    if (mdl.ItemStatus == 3)
                    {
                        proCat.ItemStatus = 4;
                    }
                    if (mdl.ItemStatus == 5)
                    {
                        proCat.ItemStatus = 6;
                    }

                    //  proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                    //  proCat.ItemStatus = 4;

                    //proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    proCat.Equipment_Short = mdl.Equipment_Short;
                    proCat.Equipment_Long = mdl.Equipment_Long;
                    proCat.EnrichedValue = mdl.EnrichedValue;
                    proCat.MissingValue = mdl.MissingValue;
                    proCat.RepeatedValue = mdl.RepeatedValue;
                    proCat.Soureurl = mdl.Soureurl;
                    proCat.assetConditionRemarks = mdl.assetConditionRemarks;
                    proCat.Remarks = mdl.Remarks;
                    proCat.Rework_Remarks = mdl.Rework_Remarks;
                    proCat.AdditionalNotes = mdl.AdditionalNotes;
                    proCat.NewTagNo = mdl.NewTagNo;
                    proCat.OldTagNo = mdl.OldTagNo;
                    proCat.Doc_Availability = mdl.Doc_Availability;
                    proCat.VirtualTagNo = mdl.VirtualTagNo;

                    proCat.Plant = mdl.Plant;
                    proCat.AssetNo = mdl.AssetNo;
                    proCat.AssetQRCode = mdl.AssetQRCode;
                    proCat.Description = mdl.Description;
                    proCat.Func_Location = mdl.Func_Location;
                    proCat.Equ_Category = mdl.Equ_Category;
                    proCat.TechIdentNo = mdl.TechIdentNo;
                    proCat.Parent = mdl.Parent;
                    proCat.Org_Code = mdl.Org_Code;
                    proCat.CostCenter = mdl.CostCenter;
                    proCat.CostCenter_Desc = mdl.CostCenter_Desc;
                    proCat.ABC_Indicator = mdl.ABC_Indicator;
                    proCat.Equ_Category = mdl.Equ_Category;
                    proCat.MainWorkCenter = mdl.MainWorkCenter;
                    proCat.ObjType = mdl.ObjType;
                    proCat.System = mdl.System;
                    proCat.PartNo = mdl.PartNo;
                    proCat.SerialNo = mdl.SerialNo;
                    proCat.ModelNo = mdl.ModelNo;
                    proCat.ModelYear = mdl.ModelYear;
                    proCat.Manufacturer = mdl.Manufacturer;
                    proCat.MfrCountry = mdl.MfrCountry;
                    proCat.MfrYear = mdl.MfrYear;
                    proCat.AssCondition = mdl.AssCondition;
                    proCat.PID_Number = mdl.PID_Number;
                    proCat.PID_Desc = mdl.PID_Desc;
                    proCat.Section_Number = mdl.Section_Number;
                    proCat.Section_Desc = mdl.Section_Desc;
                    proCat.Discipline = mdl.Discipline;
                    proCat.AdditionalInfo = mdl.AdditionalInfo;

                    listdt.Add(proCat);
                }


                res = _AssetService.submitasset(listdt);
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


        //Load Hierarchy

        [Authorize]

        public JsonResult getHierarchy(string Bis, string Major, string FunStr)
        {
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
         

            var ListItems = new List<Prosol_AssetMaster>();
            if (string.IsNullOrEmpty(FunStr))
            {
                if (usrInfo.Departmentname == "ELECTRICITY")
                    ListItems = _AssetService.getAssetDataByFun("ELECTRICITY", "").ToList();

                if (usrInfo.Departmentname == "WATER TRANSMISSION AND DISTRIBUTION")
                    ListItems = _AssetService.getAssetDataByFun("WATER TRANSMISSION AND DISTRIBUTION", "").ToList();

                if (usrInfo.Departmentname == "WATER PRODUCTION")
                    ListItems = _AssetService.getAssetDataByFun("WATER PRODUCTION", "").ToList();
            }
            else
            {                
                    ListItems = _AssetService.getAssetDataByFun(Bis,Major, FunStr).ToList();

            }


            var ListEquipments = new List<CatAssetModel>();

            var AllMaster = _AssetService.getAllCommonMaster();

            foreach (Prosol_AssetMaster assetvalues in ListItems)
            {
                var catasset = new CatAssetModel();


                //var regname = AllMaster.Regions.Where(x => x._id.ToString() == assetvalues.Region).ToList();
                //catasset.Region = regname.Count > 0 ? regname[0].Region : "";

                //var Cityname = AllMaster.Cities.Where(x => x._id.ToString() == assetvalues.City).ToList();
                //catasset.City = Cityname.Count > 0 ? Cityname[0].City : "";

                //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
                //catasset.Area = Areaname.Count > 0 ? Areaname[0].Area : "";

                //var SubAreaname = AllMaster.SubAreas.Where(x => x._id.ToString() == assetvalues.SubArea).ToList();
                //catasset.SubArea = SubAreaname.Count > 0 ? SubAreaname[0].SubArea : "";


                //var Majorcls = AllMaster.MajorClasses.Where(x => x._id.ToString() == assetvalues.MajorClass).ToList();
                //catasset.MajorClass = Majorcls.Count > 0 ? Majorcls[0].MajorClass : "";

                //var Funcname = AllMaster.MinorClasses.Where(x => x._id.ToString() == assetvalues.MinorClass).ToList();
                //catasset.MinorClass = Funcname.Count > 0 ? Funcname[0].MinorClass : "";

                //catasset.Business = assetvalues.Business;
                //if (string.IsNullOrEmpty(FunStr))
                //{
                //    catasset.Quantity = Convert.ToInt32(assetvalues.Quantity).ToString("#,##0");
                //    catasset.SerialNo = Convert.ToInt32(assetvalues.SerialNo).ToString("#,##0");
                //    catasset.Make = Convert.ToInt32(assetvalues.Make).ToString("#,##0");
                //    catasset.ModelNo = Convert.ToInt32(assetvalues.ModelNo).ToString("#,##0");
                //}
                    
                catasset.ItemStatus = assetvalues.ItemStatus;
                
              
                //if (!string.IsNullOrEmpty(FunStr))
                //{
                    catasset.UniqueId = assetvalues.AssetNo;
                //    catasset.Barcode = assetvalues.Barcode;
                //    catasset.NewTagNo = assetvalues.NewTagNo;
                //    catasset.SiteId = assetvalues.SiteId;
                //    catasset.SiteName = assetvalues.SiteName;
                //    catasset.SiteType = assetvalues.SiteType;
                //    catasset.CompanyCode = assetvalues.CompanyCode;
                //    catasset.SAP_Equipment = assetvalues.SAP_Equipment;
                //    catasset.OldTagNo = assetvalues.UniqueId;
                //    catasset.AssetNo = assetvalues.AssetNo;
                //    catasset.AssetSubNo = assetvalues.AssetSubNo;
                //    catasset.ObjectType = assetvalues.ObjectType;
                //    catasset.Location = assetvalues.Location;
                //    catasset.EquipmentDesc = assetvalues.EquipmentDesc;
                //    catasset.FLOC_Code = assetvalues.FLOC_Code;
                //    catasset.FuncLocDesc = assetvalues.FuncLocDesc;
                  
                //    var business = AllMaster.Businesses.Where(x => x._id.ToString() == assetvalues.Business).ToList();
                //    catasset.Business = business.Count > 0 ? business[0].BusinessName : "";

                //    var Identiname = AllMaster.Identifiers.Where(x => x._id.ToString() == assetvalues.Identifier).ToList();
                //    catasset.Identifier = Identiname.Count > 0 ? Identiname[0].Identifier : "";

                //    var EquipCls = AllMaster.EquipmentClasses.Where(x => x._id.ToString() == assetvalues.EquipmentClass).ToList();
                //    catasset.EquipmentClass = EquipCls.Count > 0 ? EquipCls[0].EquipmentClass : "";

                //    var EquipTye = AllMaster.EquipmentTypes.Where(x => x._id.ToString() == assetvalues.EquipmentType).ToList();
                //    catasset.EquipmentType = EquipTye.Count > 0 ? EquipTye[0].EquipmentType : "";

                //    catasset.BOM = _AssetService.getAllAssetBom(assetvalues.UniqueId).ToList().Count.ToString();
                //    if(assetvalues.CLF_Remarks!=null)
                //    {
                //        catasset.CLF_Remarks = new List<CLF_Remarks>();
                //        foreach (var clf in assetvalues.CLF_Remarks)
                //        {
                //            var md = new CLF_Remarks();
                //            md.UserId = clf.UserId;
                //            md.UserName = clf.UserName;
                //            md.Remark = clf.Remark;
                //            md.CreatedOn = clf.CreatedOn;
                //            catasset.CLF_Remarks.Add(md);
                //        }
                //    }
                    
                //}
                ListEquipments.Add(catasset);
                
            }      
            

            var jsonResult = Json(ListEquipments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //asset report
        [Authorize]
        public JsonResult getBomList(string UniqueId)
        {
            var ListItems = _AssetService.getAllAssetBom(UniqueId);
            var jsonResult = Json(ListItems, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
     
        [Authorize]
        [HttpGet]
        public JsonResult GetPVusers()
        {

            var userlist = _AssetService.getpvuser().ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        //Get the region


        // Track load Data
        [HttpPost]
        public JsonResult DownloadTrackCodes()
        {
            var assetno = Request.Form["assetno"];
            Session["assetno"] = assetno;

            string[] codestr;
            List<Prosol_AssetMaster> get_assigndata = new List<Prosol_AssetMaster>();
            List<Prosol_AssetMaster> gd_data = new List<Prosol_AssetMaster>();
            //List<multiplecodelist> getcode = new List<multiplecodelist>();
            List<string> code_split = new List<string>();
            codestr = assetno.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }
            foreach (string cdn in code_split)
            {
                gd_data = _AssetService.trackmulticodelist(cdn).ToList();
                get_assigndata.AddRange(gd_data);

            }
            var cunt = gd_data.Count;
            return Json(cunt, JsonRequestBehavior.AllowGet);

        }

        public void TrackloadAssetCodes()
        {
            var assetno = Session["assetno"].ToString();
            string[] codestr;
            List<Prosol_Datamaster> get_assigndata = new List<Prosol_Datamaster>();
            List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
            //List<multiplecodelist> getcode = new List<multiplecodelist>();
            List<string> code_split = new List<string>();
            codestr = assetno.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }

            var res1 = _AssetService.Downloaddatacodes(codestr).ToList();
            var res2 = _AssetService.Downloadvendordatacodes(codestr).ToList();

            DataTable dt = new DataTable();
            foreach (IDictionary<string, object> row in res1)
            {
                var rw = dt.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt.Rows.Add(rw);
            }
            DataTable dt1 = new DataTable();
            foreach (IDictionary<string, object> row in res2)
            {
                var rw = dt1.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt1.Columns.Contains(entry.Key.ToString()))
                    {
                        dt1.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt1.Rows.Add(rw);
            }
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "ITEM");
            wbook.Worksheets.Add(dt1, "VENDOR");
            string fileName = "Items";
            //if (id != "QA")
            //{
            //  Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");
            httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}.xlsx\"");
            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();

        }
        //public ActionResult TrackloadAssetCodes()
        //{
        //    try
        //    {
        //        var assetno = Request.Form["assetno"];
        //        if (string.IsNullOrEmpty(assetno))
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Asset number is required.");
        //        }

        //        string[] codestr = assetno.Split(',');
        //        List<string> code_split = new List<string>();

        //        foreach (string n in codestr)
        //        {
        //            string trimmedCode = n.Trim();
        //            if (!code_split.Contains(trimmedCode))
        //            {
        //                code_split.Add(trimmedCode);
        //            }
        //        }


        //        DataTable dt1 = BuildDataTable(res1);
        //        DataTable dt2 = BuildDataTable(res2);

        //        using (ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook())
        //        {
        //            wbook.Worksheets.Add(dt1, "ITEM");
        //            wbook.Worksheets.Add(dt2, "VENDOR");

        //            string fileName = "AssetReport.xlsx";

        //            // Prepare the response
        //            HttpResponseBase httpResponse = Response;
        //            httpResponse.Clear();
        //            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");

        //            // Write workbook to response stream
        //            using (MemoryStream memoryStream = new MemoryStream())
        //            {
        //                wbook.SaveAs(memoryStream);
        //                memoryStream.WriteTo(httpResponse.OutputStream);
        //            }

        //            httpResponse.End();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (ex.Message)
        //        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
        //    }

        //    return new EmptyResult();
        //}

        private DataTable BuildDataTable(List<Dictionary<string, object>> data)
        {
            DataTable dt = new DataTable();

            foreach (IDictionary<string, object> row in data)
            {
                var newRow = dt.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    newRow[entry.Key] = entry.Value;
                }
                dt.Rows.Add(newRow);
            }

            return dt;
        }



        [Authorize]
        [HttpPost]
        public JsonResult TrackloadAsset()
         {
            string FARId = Request.Form["FARId"];
            //string Major = Request.Form["MajorClass"];
            //string Minor = Request.Form["MinorClass"];
            //string SubArea = Request.Form["SubArea"];
            string Role = Request.Form["Role"];
            string User = Request.Form["User"];
            string Status = Request.Form["Status"];
            string fromdate = Request.Form["From"];
            string todate = Request.Form["To"];

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();



           
                rows = _AssetService.getallassetdata(FARId, Role, User,Status, fromdate, todate).ToList();

                var AllMaster = _AssetService.getAllCommonMaster();

            var jsonResult = Json(rows, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(rows, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult TrackloadAsset()
        // {
        //    string FARId = Request.Form["FARId"];
        //    //string Major = Request.Form["MajorClass"];
        //    //string Minor = Request.Form["MinorClass"];
        //    //string SubArea = Request.Form["SubArea"];
        //    string Role = Request.Form["Role"];
        //    string User = Request.Form["User"];
        //    string Status = Request.Form["Status"];
        //    string fromdate = Request.Form["From"];
        //    string todate = Request.Form["To"];

        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();



           
        //        var rows = _AssetService.getallassetdata(FARId, Role, User,Status, fromdate, todate).ToList();

        //        var AllMaster = _AssetService.getAllCommonMaster();
        //        foreach (var cde in Assetlist)
        //        {

        //            row = new Dictionary<string, object>();

        //        row.Add("Unique ID", cde.UniqueId);
        //        if (cde.FixedAssetNo != null)
        //            row.Add("Fixedassetnum(FAR ID)", cde.FixedAssetNo);
        //        else
        //            row.Add("Fixedassetnum(FAR ID)", "");
        //        row.Add("SiteId", cde.SiteId);
        //        if (/*cde.AssetNo.Contains("-")*/cde.AssetNo != null)
        //            row.Add("Asset Number", cde.AssetNo);
        //        else
        //            row.Add("Asset Number", "");
        //        if (cde.pvAssetNo != null && cde.pvAssetNo != cde.AssetNo)
        //        row.Add("PV Asset Number", cde.pvAssetNo);
        //        else
        //            row.Add("PV Asset Number", "");
        //        if (cde.OldTagNo != null)
        //            row.Add("Old Asset Barcode", cde.OldTagNo);
        //        else
        //            row.Add("Old Asset Barcode", "");
        //        if (cde.AssetQRCode != null)
        //        row.Add("Asset QR Code", cde.AssetQRCode);
        //        else
        //            row.Add("Asset QR Code", "");
        //        if (cde.Description != null)
        //            row.Add("Asset Description", cde.Description);
        //        else
        //            row.Add("Asset Description", "");
        //        if (cde.Parent != null)
        //            row.Add("Parent", cde.Parent);
        //        else
        //            row.Add("Parent", "");
        //        if (cde.Location != null)
        //            row.Add("Asset Location", cde.Location);
        //        else
        //            row.Add("Asset Location ", "");
        //        if (cde.LocationHierarchy != null)
        //            row.Add("Location Hierarchy", cde.LocationHierarchy);
        //        else
        //            row.Add("Location Hierarchy", "");
        //        if (cde.ClassificationHierarchyDesc != null)
        //            row.Add("Classificationhierarchy", cde.ClassificationHierarchyDesc);
        //        else
        //            row.Add("Classificationhierarchy", "");
        //        if (cde.AggregatedClassSpecAttr != null)
        //            row.Add("Aggregatedclassspecattributes", cde.AggregatedClassSpecAttr);
        //        else
        //            row.Add("Aggregatedclassspecattributes", "");
        //        if (cde.Status != null)
        //            row.Add("Asset Status", cde.Status);
        //        else
        //            row.Add("Asset Status", "");
        //        if (cde.AssetType != null)
        //            row.Add("Assettype", cde.AssetType);
        //        else
        //            row.Add("Assettype", "");
        //        if (cde.FailureCode != null)
        //            row.Add("FailureCode", cde.FailureCode);
        //        else
        //            row.Add("FailureCode", "");
        //        if (cde.Priority != null)
        //            row.Add("Priority", cde.Priority);
        //        else
        //            row.Add("Priority", "");
        //        if (cde.MaintaineBy != null)
        //            row.Add("Maintained By", cde.MaintaineBy);
        //        else
        //            row.Add("Maintained By", "");
        //        if (cde.WarrentyExpDate != null)
        //            row.Add("Warrantyexpdate", cde.WarrentyExpDate);
        //        else
        //            row.Add("Warrantyexpdate", "");
        //        if (cde.Name != null)
        //            row.Add("Name", cde.Name);
        //        else
        //            row.Add("Name", "");
        //        if (cde.SerialNo != null)
        //            row.Add("Serial No", cde.SerialNo);
        //        else
        //            row.Add("Serial No", "");
        //        if (cde.ModelNo != null)
        //            row.Add("Model No", cde.ModelNo);
        //        else
        //            row.Add("Model No", "");
        //        if (cde.ModelYear != null)
        //            row.Add("Model Year", cde.ModelYear);
        //        else
        //            row.Add("Model Year", "");
        //        if (cde.AssCondition != null && cde.AssCondition != "Excellent" )
        //            row.Add("Asset Condition", cde.AssCondition);
        //        else
        //            row.Add("Asset Condition", "");
        //        if (cde.OwnedBySite != null)
        //            row.Add("OwnedBySite", cde.OwnedBySite);
        //        else
        //            row.Add("OwnedBySite", "");
        //        if(cde.Manufacturer != null )
        //        row.Add("Manufacturer", cde.Manufacturer);
        //        else
        //        row.Add("Manufacturer", "");
        //        if (cde.ReportGroup != null)
        //            row.Add("ReportGroup", cde.ReportGroup);
        //        else
        //            row.Add("ReportGroup", "");
        //        if (cde.Vendor != null )
        //        row.Add("Vendor", cde.Vendor);
        //        else
        //        row.Add("Vendor", "");
        //        if(cde.PurchasePrice != null )
        //        row.Add("PurchasePrice", cde.PurchasePrice);
        //        else
        //        row.Add("PurchasePrice", "");
        //        if(cde.InstallDate != null )
        //        row.Add("InstallDate", cde.InstallDate);
        //        else
        //        row.Add("InstallDate", "");
        //        if(cde.PO_Contract != null )
        //        row.Add("PO Contract", cde.PO_Contract);
        //        else
        //        row.Add("PO Contract", "");
        //        if(cde.LoadCertExpDate != null )
        //        row.Add("Loadcertexpdate", cde.LoadCertExpDate);
        //        else
        //        row.Add("Loadcertexpdate", "");
        //        if(cde.CalCertExpDate != null )
        //        row.Add("Calcertexpdate", cde.CalCertExpDate);
        //        else
        //        row.Add("Calcertexpdate", "");
        //        if(cde.CertExpDate != null )
        //        row.Add("Certexpdate", cde.CertExpDate);
        //        else
        //        row.Add("Certexpdate", "");
        //        if(cde.TrafficCertExpDate != null )
        //        row.Add("Trafficcertexpdate", cde.TrafficCertExpDate);
        //        else
        //        row.Add("Trafficcertexpdate", "");
        //        if(cde.Ownedby != null )
        //        row.Add("Ownedby", cde.Ownedby);
        //        else
        //        row.Add("Ownedby", "");
        //        if(cde.Maintainer != null )
        //        row.Add("Maintaineby", cde.Maintainer);
        //        else
        //        row.Add("Maintaineby", "");
        //        if(cde.PresentLocation != null )
        //        row.Add("Present Location", cde.PresentLocation);
        //        else
        //        row.Add("Present Location", "");
        //        if(cde.Operatedby != null )
        //        row.Add("Operatedby", cde.Operatedby);
        //        else
        //        row.Add("Operatedby", "");
        //        if (cde.BarcodeNumber != null)
        //            row.Add("Existing BarCode", cde.BarcodeNumber);
        //        else
        //            row.Add("Existing BarCode", "");
        //        if (cde.AdditionalNotes != null )
        //        row.Add("Additional Notes", cde.AdditionalNotes);
        //        else
        //        row.Add("Additional Notes", "");
        //        if(cde.GIS != null && cde.GIS.LongitudeStart != null )
        //        row.Add("Start Point", cde.GIS.LongitudeStart.Replace('W', 'E'));
        //        else
        //        row.Add("Start Point", "");
        //        if(cde.GIS != null && cde.GIS.LattitudeStart != null)
        //        row.Add("End Point", cde.GIS.LattitudeStart);
        //        else
        //        row.Add("End Point", "");
        //        if(cde.AssetCondition != null)
        //        {
        //            string pvCond = "";
        //            if (cde.AssetCondition.Rank == "7")
        //            {
        //                pvCond = "Excellent";
        //            }
        //            if (cde.AssetCondition.Rank == "6")
        //            {
        //                pvCond = "Very Good";
        //                if(cde.AssetCondition.Damage == "Medium")
        //                    pvCond = "Very Good";
        //            }
        //            if (cde.AssetCondition.Rank == "4" || cde.AssetCondition.Rank == "5")
        //            {
        //                pvCond = "Good";
        //            }
        //            if (cde.AssetCondition.Rank == "2" || cde.AssetCondition.Rank == "3")
        //            {
        //                pvCond = "Poor";
        //            }
        //            if (cde.AssetCondition.Rank == "1")
        //            {
        //                pvCond = "Very Poor";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium")
        //            {
        //                pvCond = "Good";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium"&&cde.AssetCondition.Vibration == "Medium"&&cde.AssetCondition.Temparature == "Medium")
        //            {
        //                pvCond = "Good";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium" && cde.AssetCondition.Vibration == "Medium" && cde.AssetCondition.Temparature == "Medium" && cde.AssetCondition.Damage == "Medium" && cde.AssetCondition.Smell == "Medium")
        //            {
        //                pvCond = "Poor";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium" && cde.AssetCondition.Vibration == "Medium" && cde.AssetCondition.Temparature == "Medium" && cde.AssetCondition.Damage == "Medium" && cde.AssetCondition.Smell == "Medium" && cde.AssetCondition.Noise == "Medium")
        //            {
        //                pvCond = "Very Poor";
        //            }
        //            if (cde.AssetCondition.Damage == "High" && cde.AssetCondition.Corrosion == "High")
        //            {
        //                pvCond = "Very Poor";
        //            }
        //            if (cde.AssetCondition.Corrosion == "No" &&cde.AssetCondition.Leakage == "No" && cde.AssetCondition.Vibration == "No" && cde.AssetCondition.Temparature == "No" && cde.AssetCondition.Damage == "No" && cde.AssetCondition.Smell == "No" && cde.AssetCondition.Noise == "No")
        //            {
        //                pvCond = "Excellent";
        //            }
        //            if (pvCond != "")
        //                row.Add("PV Asset Condition", pvCond);
        //            else
        //            {
        //                row.Add("PV Asset Condition", "");
        //            }
        //        }
        //        else
        //        {
        //            row.Add("PV Asset Condition", "");
        //        }
        //        if (cde.AssetImages != null)
        //        {
        //            if (cde.AssetImages.NamePlateText != null)
        //            {
        //                if (cde.AssetImages.NamePlateText.Length >= 1)
        //                {
        //                    row.Add("Nameplate Text", cde.AssetImages.NamePlateText[0]);
        //                }
        //                else
        //                {
        //                    row.Add("Nameplate Text", "");
        //                }
        //            }
        //            else
        //            {
        //                row.Add("Nameplate Text", "");
        //            }
        //        }
        //        else
        //        {
        //            row.Add("Nameplate Text", "");
        //        }

        //        var classification = _AssetService.GetAttributeInfo(cde.UniqueId);

        //        if (classification != null)
        //        {
        //            if (classification.Noun != null && classification.Modifier != null)
        //            {
        //                if (classification.Noun != null && classification.Modifier == null || classification.Modifier == "--")
        //                {
        //                    row.Add("Classificationid", classification.Noun);
        //                }
        //                else
        //                {
        //                    row.Add("Classificationid", classification.Noun + "," + classification.Modifier);
        //                }
        //            }
        //            else
        //            {
        //                row.Add("Classificationid", "");
        //            }
        //            //var Cha = new List<Asset_AttributeList>();
        //            //var exCha = new List<Asset_AttributeList>();
        //            //if (classification.Characterisitics != null)
        //            //    Cha = classification.Characterisitics;
        //            //if (classification.exCharacterisitics != null)
        //            //    exCha = classification.exCharacterisitics;
        //            //int chaCnt = Cha.Count();
        //            //int filledCha = Cha.Count(g => g.Value != "");
        //            //double chaFilledRatePer = (double)filledCha / chaCnt * 100;
        //            //string chaFilledRate = chaFilledRatePer.ToString("F2") + " %";

        //            //int exchaCnt = exCha.Count();
        //            //int filledexCha = exCha.Count(g => g.Value != "");
        //            //double exchaFilledRatePer = (double)filledexCha / exchaCnt * 100;
        //            //string exchaFilledRate = exchaFilledRatePer.ToString("F2") + " %";


        //            //var mandCharc = new List<Asset_AttributeList>();
        //            //foreach (var cha in classification.Characterisitics)
        //            //{
        //            //    var mand = new Asset_AttributeList();
        //            //    var dic = _CharateristicService.GetCharacteristicvalues(cha.Characteristic, classification.Noun, classification.Modifier);
        //            //    mand.Characteristic = cha.Characteristic;
        //            //    mand.Value = cha.Value;
        //            //    mand.Abbrevated = cha.Abbrevated;
        //            //    mand.UomMandatory = cha.UomMandatory;
        //            //    mandCharc.Add(mand);
        //            //}

        //            //int mandCnt = mandCharc.Count();
        //            //int MandCha = mandCharc.Count(g => g.UomMandatory == "Yes");
        //            //int filledMandCha = mandCharc.Count(g => g.UomMandatory == "Yes" && g.Value != "");
        //            //double mandFilledRatePer = (double)filledMandCha / MandCha * 100;
        //            //string mandFilledRate = mandFilledRatePer.ToString("F2") + " %";

        //            //if (exchaFilledRate == "NaN %")
        //            //{
        //            //    exchaFilledRate = "0.00 %";
        //            //}
        //            //if (chaFilledRate == "NaN %")
        //            //{
        //            //    chaFilledRate = "0.00 %";
        //            //}
        //            //if (mandFilledRate == "NaN %")
        //            //{
        //            //    mandFilledRate = "0.00 %";
        //            //}

        //            //if (!string.IsNullOrEmpty(exchaFilledRate))
        //            //    row.Add("Existing Filled Rate", exchaFilledRate);
        //            //else
        //            //    row.Add("Existing Filled Rate", "");
        //            //if (!string.IsNullOrEmpty(mandFilledRate))
        //            //    row.Add("Mandatory Filled Rate", mandFilledRate);
        //            //else
        //            //    row.Add("Mandatory Filled Rate", "");
        //            //if (!string.IsNullOrEmpty(chaFilledRate))
        //            //    row.Add("New Filled Rate", chaFilledRate);
        //            //else
        //            //    row.Add("New Filled Rate", "");
        //        }
        //        else
        //        {
        //            row.Add("Classificationid", "");
        //            //row.Add("Existing Filled Rate", "");
        //            //row.Add("Mandatory Filled Rate", "");
        //            //row.Add("New Filled Rate", "");
        //        }

        //        if (cde.Equipment_Short != null )
        //        row.Add("Short Desc", cde.Equipment_Short);
        //        else
        //        row.Add("Short Desc", "");
        //        if(cde.Equipment_Long != null )
        //        row.Add("Long Desc", cde.Equipment_Long);
        //        else
        //        row.Add("Long Desc", "");
        //        if(cde.PVuser != null && cde.PVuser.Name != null)
        //        row.Add("PV", cde.PVuser.Name);
        //        else
        //        row.Add("PV", "");
        //        if (cde.PVstatus == "Completed" && cde.PVuser != null && cde.PVuser.UpdatedOn != null)
        //        {
        //            DateTime date1 = DateTime.Parse(Convert.ToString(cde.PVuser.UpdatedOn));
        //            row.Add("PV Completed Date", date1.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //            row.Add("PV Completed Date", "");
        //        if(cde.Catalogue != null && cde.Catalogue.Name != null)
        //        row.Add("Catalogue", cde.Catalogue.Name);
        //        else
        //        row.Add("Catalogue", "");
        //        if (cde.ItemStatus > 3  && cde.Catalogue != null && cde.Catalogue.UpdatedOn != null)
        //        {
        //            DateTime date2 = DateTime.Parse(Convert.ToString(cde.Catalogue.UpdatedOn));
        //            row.Add("Catalogue Completed Date", date2.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //            row.Add("Catalogue Completed Date", "");
        //        if (cde.Review != null && cde.Review.Name != null)
        //        row.Add("QC", cde.Review.Name);
        //        else
        //        row.Add("QC", "");
        //        if (cde.ItemStatus > 5 && cde.Review != null && cde.Review.UpdatedOn != null)
        //        {
        //            DateTime date3 = DateTime.Parse(Convert.ToString(cde.Review.UpdatedOn));
        //            row.Add("QC Completed Date", date3.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //            row.Add("QC Completed Date", "");
        //        if (cde.Soureurl != null)
        //        {
        //            row.Add("URL", cde.Soureurl);
        //        }
        //        else
        //            row.Add("URL", "");
        //        if (cde.Remarks != null)
        //        {
        //            row.Add("PV Remarks", cde.Remarks);
        //        }
        //        else
        //            row.Add("PV Remarks", "");
        //        if (cde.Remarks != null)
        //        {
        //            row.Add("Rework Remarks", cde.Rework_Remarks);
        //        }
        //        else
        //            row.Add("Rework Remarks", "");
        //        if (cde.ItemStatus == 0)
        //        {
        //            row.Add("Status", "PV Not Assigned");
        //        }
        //        else if (cde.ItemStatus == 1)
        //        {
        //            row.Add("Status", "PV Pending");
        //        }
        //        else if (cde.ItemStatus == 2)
        //        {
        //            row.Add("Status", "Catalogue Pending");
        //        }
        //        else if (cde.ItemStatus == 3)
        //        {
        //            row.Add("Status", "Catalogue Saved");
        //        }
        //        else if (cde.ItemStatus == 4)
        //        {
        //            row.Add("Status", "QC Pending");
        //        }
        //        else if (cde.ItemStatus == 5)
        //        {
        //            row.Add("Status", "QC Saved");
        //        }
        //        else if (cde.ItemStatus >= 6)
        //        {
        //            row.Add("Status", "Released");
        //        }
        //        rows.Add(row);

        //        }

        //    var jsonResult = Json(rows, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //    //return Json(rows, JsonRequestBehavior.AllowGet);
        //}

        // Track Download Data


        //public void DownloadAssetMulticode(string FARId, string Role, string User, string Status, string Fromdate, string Todate, int Attr)
        //{

        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    List<Dictionary<string, object>> attRows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> aRow;
        //    var Assetlist = _AssetService.getallassetdata(FARId, Role, User, Status, Fromdate, Todate).ToList();
        //    var AllMaster = _AssetService.getAllCommonMaster();
        //    foreach (var cde in Assetlist)
        //    {

        //        row = new Dictionary<string, object>();

        //        row.Add("Unique ID", cde.UniqueId);
        //        if (cde.FixedAssetNo != null)
        //            row.Add("Fixedassetnum(FAR ID)", cde.FixedAssetNo);
        //        else
        //            row.Add("Fixedassetnum(FAR ID)", "");
        //        row.Add("SiteId", cde.SiteId);
        //        if (/*cde.AssetNo.Contains("-")*/cde.AssetNo != null)
        //            row.Add("Asset Number", cde.AssetNo);
        //        else
        //            row.Add("Asset Number", "");
        //        if (cde.pvAssetNo != null && cde.pvAssetNo != cde.AssetNo)
        //            row.Add("PV Asset Number", cde.pvAssetNo);
        //        else
        //            row.Add("PV Asset Number", "");
        //        if (cde.OldTagNo != null)
        //            row.Add("Old Asset Barcode", cde.OldTagNo);
        //        else
        //            row.Add("Old Asset Barcode", "");
        //        if (cde.AssetQRCode != null)
        //            row.Add("Asset QR Code", cde.AssetQRCode);
        //        else
        //            row.Add("Asset QR Code", "");
        //        if (cde.Description != null)
        //            row.Add("Asset Description", cde.Description);
        //        else
        //            row.Add("Asset Description", "");
        //        if (cde.Parent != null)
        //            row.Add("Parent", cde.Parent);
        //        else
        //            row.Add("Parent", "");
        //        if (cde.Location != null)
        //            row.Add("Asset Location", cde.Location);
        //        else
        //            row.Add("Asset Location ", "");
        //        if (cde.LocationHierarchy != null)
        //            row.Add("Location Hierarchy", cde.LocationHierarchy);
        //        else
        //            row.Add("Location Hierarchy", "");
        //        if (cde.ClassificationHierarchyDesc != null)
        //            row.Add("Classificationhierarchy", cde.ClassificationHierarchyDesc);
        //        else
        //            row.Add("Classificationhierarchy", "");
        //        if (cde.AggregatedClassSpecAttr != null)
        //            row.Add("Aggregatedclassspecattributes", cde.AggregatedClassSpecAttr);
        //        else
        //            row.Add("Aggregatedclassspecattributes", "");
        //        if (cde.Status != null)
        //            row.Add("Asset Status", cde.Status);
        //        else
        //            row.Add("Asset Status", "");
        //        if (cde.AssetType != null)
        //            row.Add("Assettype", cde.AssetType);
        //        else
        //            row.Add("Assettype", "");
        //        if (cde.FailureCode != null)
        //            row.Add("FailureCode", cde.FailureCode);
        //        else
        //            row.Add("FailureCode", "");
        //        if (cde.Priority != null)
        //            row.Add("Priority", cde.Priority);
        //        else
        //            row.Add("Priority", "");
        //        if (cde.MaintaineBy != null)
        //            row.Add("Maintained By", cde.MaintaineBy);
        //        else
        //            row.Add("Maintained By", "");
        //        if (cde.WarrentyExpDate != null)
        //            row.Add("Warrantyexpdate", cde.WarrentyExpDate);
        //        else
        //            row.Add("Warrantyexpdate", "");
        //        if (cde.Name != null)
        //            row.Add("Name", cde.Name);
        //        else
        //            row.Add("Name", "");
        //        if (cde.SerialNo != null)
        //            row.Add("Serial No", cde.SerialNo);
        //        else
        //            row.Add("Serial No", "");
        //        if (cde.ModelNo != null)
        //            row.Add("Model No", cde.ModelNo);
        //        else
        //            row.Add("Model No", "");
        //        if (cde.ModelYear != null)
        //            row.Add("Model Year", cde.ModelYear);
        //        else
        //            row.Add("Model Year", "");
        //        if (cde.AssCondition != null && cde.AssCondition != "Excellent")
        //            row.Add("Asset Condition", cde.AssCondition);
        //        else
        //            row.Add("Asset Condition", "");
        //        if (cde.OwnedBySite != null)
        //            row.Add("OwnedBySite", cde.OwnedBySite);
        //        else
        //            row.Add("OwnedBySite", "");
        //        if (cde.Manufacturer != null)
        //            row.Add("Manufacturer", cde.Manufacturer);
        //        else
        //            row.Add("Manufacturer", "");
        //        if (cde.ReportGroup != null)
        //            row.Add("ReportGroup", cde.ReportGroup);
        //        else
        //            row.Add("ReportGroup", "");
        //        if (cde.Vendor != null)
        //            row.Add("Vendor", cde.Vendor);
        //        else
        //            row.Add("Vendor", "");
        //        if (cde.PurchasePrice != null)
        //            row.Add("PurchasePrice", cde.PurchasePrice);
        //        else
        //            row.Add("PurchasePrice", "");
        //        if (cde.InstallDate != null)
        //            row.Add("InstallDate", cde.InstallDate);
        //        else
        //            row.Add("InstallDate", "");
        //        if (cde.PO_Contract != null)
        //            row.Add("PO Contract", cde.PO_Contract);
        //        else
        //            row.Add("PO Contract", "");
        //        if (cde.LoadCertExpDate != null)
        //            row.Add("Loadcertexpdate", cde.LoadCertExpDate);
        //        else
        //            row.Add("Loadcertexpdate", "");
        //        if (cde.CalCertExpDate != null)
        //            row.Add("Calcertexpdate", cde.CalCertExpDate);
        //        else
        //            row.Add("Calcertexpdate", "");
        //        if (cde.CertExpDate != null)
        //            row.Add("Certexpdate", cde.CertExpDate);
        //        else
        //            row.Add("Certexpdate", "");
        //        if (cde.TrafficCertExpDate != null)
        //            row.Add("Trafficcertexpdate", cde.TrafficCertExpDate);
        //        else
        //            row.Add("Trafficcertexpdate", "");
        //        if (cde.Ownedby != null)
        //            row.Add("Ownedby", cde.Ownedby);
        //        else
        //            row.Add("Ownedby", "");
        //        if (cde.Maintainer != null)
        //            row.Add("Maintaineby", cde.Maintainer);
        //        else
        //            row.Add("Maintaineby", "");
        //        if (cde.PresentLocation != null)
        //            row.Add("Present Location", cde.PresentLocation);
        //        else
        //            row.Add("Present Location", "");
        //        if (cde.Operatedby != null)
        //            row.Add("Operatedby", cde.Operatedby);
        //        else
        //            row.Add("Operatedby", "");
        //        if (cde.BarcodeNumber != null)
        //            row.Add("Existing BarCode", cde.BarcodeNumber);
        //        else
        //            row.Add("Existing BarCode", "");
        //        if (cde.AdditionalNotes != null)
        //            row.Add("Additional Notes", cde.AdditionalNotes);
        //        else
        //            row.Add("Additional Notes", "");
        //        if (cde.GIS != null && cde.GIS.LongitudeStart != null)
        //            row.Add("Start Point", cde.GIS.LongitudeStart.Replace('W', 'E'));
        //        else
        //            row.Add("Start Point", "");
        //        if (cde.GIS != null && cde.GIS.LattitudeStart != null)
        //            row.Add("End Point", cde.GIS.LattitudeStart);
        //        else
        //            row.Add("End Point", "");
        //        if (cde.AssetCondition != null)
        //        {
        //            string pvCond = "";
        //            if (cde.AssetCondition.Rank == "7")
        //            {
        //                pvCond = "Excellent";
        //            }
        //            if (cde.AssetCondition.Rank == "6")
        //            {
        //                pvCond = "Very Good";
        //                if (cde.AssetCondition.Damage == "Medium")
        //                    pvCond = "Very Good";
        //            }
        //            if (cde.AssetCondition.Rank == "4" || cde.AssetCondition.Rank == "5")
        //            {
        //                pvCond = "Good";
        //            }
        //            if (cde.AssetCondition.Rank == "2" || cde.AssetCondition.Rank == "3")
        //            {
        //                pvCond = "Poor";
        //            }
        //            if (cde.AssetCondition.Rank == "1")
        //            {
        //                pvCond = "Very Poor";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium")
        //            {
        //                pvCond = "Good";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium" && cde.AssetCondition.Vibration == "Medium" && cde.AssetCondition.Temparature == "Medium")
        //            {
        //                pvCond = "Good";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium" && cde.AssetCondition.Vibration == "Medium" && cde.AssetCondition.Temparature == "Medium" && cde.AssetCondition.Damage == "Medium" && cde.AssetCondition.Smell == "Medium")
        //            {
        //                pvCond = "Poor";
        //            }
        //            if (cde.AssetCondition.Leakage == "Medium" && cde.AssetCondition.Vibration == "Medium" && cde.AssetCondition.Temparature == "Medium" && cde.AssetCondition.Damage == "Medium" && cde.AssetCondition.Smell == "Medium" && cde.AssetCondition.Noise == "Medium")
        //            {
        //                pvCond = "Very Poor";
        //            }
        //            if (cde.AssetCondition.Damage == "High" && cde.AssetCondition.Corrosion == "High")
        //            {
        //                pvCond = "Very Poor";
        //            }
        //            if (cde.AssetCondition.Corrosion == "No" && cde.AssetCondition.Leakage == "No" && cde.AssetCondition.Vibration == "No" && cde.AssetCondition.Temparature == "No" && cde.AssetCondition.Damage == "No" && cde.AssetCondition.Smell == "No" && cde.AssetCondition.Noise == "No")
        //            {
        //                pvCond = "Excellent";
        //            }
        //            if (pvCond != "")
        //            row.Add("PV Asset Condition", pvCond);
        //            else
        //            {
        //                row.Add("PV Asset Condition", "");
        //            }
        //        }
        //        else
        //        {
        //            row.Add("PV Asset Condition", "");
        //        }
        //        if (cde.AssetImages != null)
        //        {
        //            if (cde.AssetImages.NamePlateText != null)
        //            {
        //                if (cde.AssetImages.NamePlateText.Length >= 1)
        //                {
        //                    row.Add("Nameplate Text", cde.AssetImages.NamePlateText[0]);
        //                }
        //                else
        //                {
        //                    row.Add("Nameplate Text", "");
        //                }
        //            }
        //            else
        //            {
        //                row.Add("Nameplate Text", "");
        //            }
        //        }
        //        else
        //        {
        //            row.Add("Nameplate Text", "");
        //        }
        //        var classification = _AssetService.GetAttributeInfo(cde.UniqueId);

        //        if (classification != null)
        //        {
        //            if (classification.Noun != null && classification.Modifier != null)
        //            {
        //                if (classification.Noun != null && classification.Modifier == null || classification.Modifier == "--")
        //                {
        //                    row.Add("Classificationid", classification.Noun);
        //                }
        //                else
        //                {
        //                    row.Add("Classificationid", classification.Noun + "," + classification.Modifier);
        //                }
        //            }
        //            else
        //            {
        //                row.Add("Classificationid", "");
        //            }


        //            //var Cha = new List<Asset_AttributeList>();
        //            //var exCha = new List<Asset_AttributeList>();
        //            //if (classification.Characterisitics != null)
        //            //    Cha = classification.Characterisitics;
        //            //if (classification.exCharacterisitics != null)
        //            //    exCha = classification.exCharacterisitics;
        //            //int chaCnt = Cha.Count();
        //            //int filledCha = Cha.Count(g => g.Value != "");
        //            //double chaFilledRatePer = (double)filledCha / chaCnt * 100;
        //            //string chaFilledRate = chaFilledRatePer.ToString("F2") + " %";

        //            //int exchaCnt = exCha.Count();
        //            //int filledexCha = exCha.Count(g => g.Value != "");
        //            //double exchaFilledRatePer = (double)filledexCha / exchaCnt * 100;
        //            //string exchaFilledRate = exchaFilledRatePer.ToString("F2") + " %";

        //            //var mandCharc = new List<Asset_AttributeList>();
        //            //foreach (var cha in classification.Characterisitics)
        //            //{
        //            //    var mand = new Asset_AttributeList();
        //            //    var dic = _CharateristicService.GetCharacteristicvalues(cha.Characteristic, classification.Noun, classification.Modifier);
        //            //    mand.Characteristic = cha.Characteristic;
        //            //    mand.Value = cha.Value;
        //            //    mand.Abbrevated = cha.Abbrevated;
        //            //    mand.UomMandatory = cha.UomMandatory;
        //            //    mandCharc.Add(mand);
        //            //}

        //            //int mandCnt = mandCharc.Count();
        //            //int MandCha = mandCharc.Count(g => g.UomMandatory == "Yes");
        //            //int filledMandCha = mandCharc.Count(g => g.UomMandatory == "Yes" && g.Value != "");
        //            //double mandFilledRatePer = (double)filledMandCha / MandCha * 100;
        //            //string mandFilledRate = mandFilledRatePer.ToString("F2") + " %";

        //            //if (exchaFilledRate == "NaN %")
        //            //{
        //            //    exchaFilledRate = "0.00 %";
        //            //}
        //            //if(chaFilledRate == "NaN %")
        //            //{
        //            //    chaFilledRate = "0.00 %";
        //            //}
        //            //if (mandFilledRate == "NaN %")
        //            //{
        //            //    mandFilledRate = "0.00 %";
        //            //}

        //            //if (!string.IsNullOrEmpty(exchaFilledRate))
        //            //    row.Add("Existing Filled Rate", exchaFilledRate);
        //            //else
        //            //    row.Add("Existing Filled Rate", "");
        //            //if (!string.IsNullOrEmpty(mandFilledRate))
        //            //    row.Add("Mandatory Filled Rate", mandFilledRate);
        //            //else
        //            //    row.Add("Mandatory Filled Rate", "");
        //            //if (!string.IsNullOrEmpty(chaFilledRate))
        //            //    row.Add("New Filled Rate", chaFilledRate);
        //            //else
        //            //    row.Add("New Filled Rate", "");
        //        }
        //        else
        //        {
        //            row.Add("Classificationid", "");
        //            //row.Add("Existing Filled Rate", "");
        //            //row.Add("Mandatory Filled Rate", "");
        //            //row.Add("New Filled Rate", "");
        //        }


        //        if (cde.Equipment_Short != null)
        //            row.Add("Short Desc", cde.Equipment_Short);
        //        else
        //            row.Add("Short Desc", "");
        //        if (cde.Equipment_Long != null)
        //            row.Add("Long Desc", cde.Equipment_Long);
        //        else
        //            row.Add("Long Desc", "");
        //        if (cde.PVuser != null && cde.PVuser.Name != null)
        //            row.Add("PV", cde.PVuser.Name);
        //        else
        //            row.Add("PV", "");
        //        if (cde.PVstatus == "Completed" && cde.PVuser != null && cde.PVuser.UpdatedOn != null)
        //        {
        //            DateTime date1 = DateTime.Parse(Convert.ToString(cde.PVuser.UpdatedOn));
        //            row.Add("PV Completed Date", date1.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //            row.Add("PV Completed Date", "");
        //        if (cde.Catalogue != null && cde.Catalogue.Name != null)
        //            row.Add("Catalogue", cde.Catalogue.Name);
        //        else
        //            row.Add("Catalogue", "");
        //        if (cde.ItemStatus > 3 && cde.Catalogue != null && cde.Catalogue.UpdatedOn != null)
        //        {
        //            DateTime date2 = DateTime.Parse(Convert.ToString(cde.Catalogue.UpdatedOn));
        //            row.Add("Catalogue Completed Date", date2.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //            row.Add("Catalogue Completed Date", "");
        //        if (cde.Review != null && cde.Review.Name != null)
        //            row.Add("QC", cde.Review.Name);
        //        else
        //            row.Add("QC", "");
        //        if (cde.ItemStatus > 5 && cde.Review != null && cde.Review.UpdatedOn != null)
        //        {
        //            DateTime date3 = DateTime.Parse(Convert.ToString(cde.Review.UpdatedOn));
        //            row.Add("QC Completed Date", date3.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //            row.Add("QC Completed Date", "");
        //        if (cde.Soureurl != null)
        //        {
        //            row.Add("URL", cde.Soureurl);
        //        }
        //        else
        //            row.Add("URL", "");
        //        if (cde.Remarks != null)
        //        {
        //            row.Add("PV Remarks", cde.Remarks);
        //        }
        //        else
        //            row.Add("PV Remarks", "");
        //        if (cde.Remarks != null)
        //        {
        //            row.Add("Rework Remarks", cde.Rework_Remarks);
        //        }
        //        else
        //            row.Add("Rework Remarks", "");
        //        if (cde.ItemStatus == 0)
        //        {
        //            row.Add("Status", "PV Not Assigned");
        //        }
        //        else if (cde.ItemStatus == 1)
        //        {
        //            row.Add("Status", "PV Pending");
        //        }
        //        else if (cde.ItemStatus == 2 )
        //        {
        //            row.Add("Status", "Catalogue Pending");
        //        }
        //        else if ( cde.ItemStatus == 3)
        //        {
        //            row.Add("Status", "Catalogue Saved");
        //        }
        //        else if (cde.ItemStatus == 4 )
        //        {
        //            row.Add("Status", "QC Pending");
        //        }
        //        else if (cde.ItemStatus == 5)
        //        {
        //            row.Add("Status", "QC Saved");
        //        }
        //        else if (cde.ItemStatus >= 6)
        //        {
        //            row.Add("Status", "Released");
        //        }
        //        rows.Add(row);
        //    }
        //    if (Attr == 1)
        //    {
        //        var codeLst = Assetlist.GroupBy(i => i.UniqueId).Select(g => g.Key).ToList();
        //        var attrLst = _AssetService.getallattrdata(codeLst).ToList();
        //        if (attrLst != null)
        //        {
        //            foreach (var mrg in attrLst)
        //            {
        //                if (mrg != null)
        //                {

        //                    row = new Dictionary<string, object>();
        //                    row.Add("Unique Id", mrg.UniqueId);
        //                    row.Add("Noun", mrg.Noun);
        //                    row.Add("Modifier", mrg.Modifier);
        //                    for (int i = 1; i <= 30; i++)
        //                    {
        //                        if (mrg.Characterisitics != null && i <= mrg.Characterisitics.Count)
        //                        {
        //                            var attr = mrg.Characterisitics[i - 1];
        //                            if (attr.Characteristic != null)
        //                            {
        //                                row.Add("ATTRIBUTE NAME " + i, attr.Characteristic);
        //                            }
        //                            else
        //                            {
        //                                row.Add("ATTRIBUTE NAME " + i, "");
        //                            }

        //                            if (attr.Value != null)
        //                            {
        //                                row.Add("ATTRIBUTE VALUE " + i, attr.Value);
        //                            }
        //                            else
        //                            {
        //                                row.Add("ATTRIBUTE VALUE " + i, "");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            row.Add("ATTRIBUTE NAME " + i, "");
        //                            row.Add("ATTRIBUTE VALUE " + i, "");
        //                        }
        //                    }
        //                    attRows.Add(row);
        //                }
        //            }
        //        }
        //    }
        //    DataTable dt = new DataTable();

        //    foreach (IDictionary<string, object> row1 in rows)
        //    {
        //        var rw = dt.NewRow();

        //        foreach (KeyValuePair<string, object> entry in row1)
        //        {
        //            if (!dt.Columns.Contains(entry.Key.ToString()))
        //            {
        //                dt.Columns.Add(entry.Key);
        //            }
        //            rw[entry.Key] = entry.Value;

        //        }
        //        dt.Rows.Add(rw);
        //        // dt.Rows.Add(row.Values.ToArray());
        //    }
        //    DataTable dt1 = new DataTable();

        //    foreach (IDictionary<string, object> row1 in attRows)
        //    {
        //        var rw = dt1.NewRow();

        //        foreach (KeyValuePair<string, object> entry in row1)
        //        {
        //            if (!dt1.Columns.Contains(entry.Key.ToString()))
        //            {
        //                dt1.Columns.Add(entry.Key);
        //            }
        //            rw[entry.Key] = entry.Value;

        //        }
        //        dt1.Rows.Add(rw);
        //        // dt.Rows.Add(row.Values.ToArray());
        //    }         


        //    ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        //    wbook.Worksheets.Add(dt, "Equipments");
        //    if(dt1.Rows.Count>0)
        //    wbook.Worksheets.Add(dt1, "Attributes");
        //    // Prepare the response
        //    HttpResponseBase httpResponse = Response;
        //    httpResponse.Clear();
        //    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    //Provide you file name here
        //    httpResponse.AddHeader("content-disposition", "attachment;filename=\"AssetReport.xlsx\"");

        //    // Flush the workbook to the Response.OutputStream
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        wbook.SaveAs(memoryStream);
        //        memoryStream.WriteTo(httpResponse.OutputStream);
        //        memoryStream.Close();
        //    }

        //    httpResponse.End();

        //}


        public void DownloadAssetMulticode(string FARId, string Role, string User, string Status, string Fromdate, string Todate, int Attr)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            List<Dictionary<string, object>> attRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> aRow;

            rows = _AssetService.getallassetdata(FARId, Role, User, Status, Fromdate, Todate).ToList();
            var AllMaster = _AssetService.getAllCommonMaster();

            // Process attributes if Attr == 1
            if (Attr == 1)
            {
                attRows = _AssetService.getallattrdata(FARId, Role, User, Status, Fromdate, Todate).ToList();
               
            }

            DataTable dt = new DataTable();

            foreach (IDictionary<string, object> row in rows)
            {
                var rw = dt.NewRow();

                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;

                }
                dt.Rows.Add(rw);
                // dt.Rows.Add(row.Values.ToArray());
            }
            DataTable dt1 = new DataTable();

            foreach (IDictionary<string, object> row1 in attRows)
            {
                var rw = dt1.NewRow();

                foreach (KeyValuePair<string, object> entry in row1)
                {
                    if (!dt1.Columns.Contains(entry.Key.ToString()))
                    {
                        dt1.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;

                }
                dt1.Rows.Add(rw);
                // dt.Rows.Add(row.Values.ToArray());
            }


            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "Equipments");
            if (dt1.Rows.Count > 0)
                wbook.Worksheets.Add(dt1, "Attributes");
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"AssetReport.xlsx\"");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }



        //asset assign





        //PVUSER
        public JsonResult getuserassignpv()
        {
            string role = "PV User";
            var userlist = _AssetService.getuserpv(role).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        //ReassignCodeSearch

        [HttpPost]
        [Authorize]
        public ActionResult reassignsearch_code(string Region, string City, string Area, string SubArea)
        {

            var gd_data = _AssetService.reloaddata1(Region, City, Area, SubArea).ToList();
            var AllMaster = _AssetService.getAllCommonMaster();
            var lstassign = new List<CatAssetModel>();

            if (gd_data.Count > 0)
            {
                foreach (Prosol_AssetMaster assetvalues in gd_data)
                {
                    var catasset = new CatAssetModel();
                  
                    //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
                    //catasset.Area = Areaname.Count > 0 ? Areaname[0].Area : "";
                    catasset.UniqueId = assetvalues.AssetNo;
                    //catasset.Barcode = assetvalues.Barcode;

                    //var Cityname = AllMaster.Cities.Where(x => x._id.ToString() == assetvalues.City).ToList();
                    //catasset.City = Cityname.Count > 0 ? Cityname[0].City : "";

                    //var Businame = AllMaster.Businesses.Where(x => x._id.ToString() == assetvalues.Business).ToList();
                    //catasset.Business = Businame.Count > 0 ? Businame[0].BusinessName : "";

                    //var EquipClass = AllMaster.EquipmentClasses.Where(x => x._id.ToString() == assetvalues.EquipmentClass).ToList();
                    //catasset.EquipmentClass = EquipClass.Count > 0 ? EquipClass[0].EquipmentClass : "";


                    ////  catasset.EquipmentClass = assetvalues.EquipmentClass;

                    //var EquipType = AllMaster.EquipmentTypes.Where(x => x._id.ToString() == assetvalues.EquipmentType).ToList();
                    //catasset.EquipmentType = EquipType.Count > 0 ? EquipType[0].EquipmentType : "";

                    //catasset.FLOC_Code = assetvalues.FLOC_Code;

                    //var Funcname = AllMaster.MinorClasses.Where(x => x._id.ToString() == assetvalues.MinorClass).ToList();
                    //catasset.MinorClass = Funcname.Count > 0 ? Funcname[0].MinorCode : "";

                    //var majorname = AllMaster.MajorClasses.Where(x => x._id.ToString() == assetvalues.MajorClass).ToList();
                    //catasset.MajorClass = majorname.Count > 0 ? majorname[0].MajorClass : "";

                    ////catasset.MajorClass = assetvalues.MajorClass;

                    //catasset.EquipmentDesc = assetvalues.EquipmentDesc;

                    //var Identiname = AllMaster.Identifiers.Where(x => x._id.ToString() == assetvalues.Identifier).ToList();
                    //catasset.Identifier = Identiname.Count > 0 ? Identiname[0].IdentifierCode : "";

                    //catasset.OldFunLoc = assetvalues.OldFunLoc;
                    //catasset.Quantity = assetvalues.Quantity;

                    //var regname = AllMaster.Regions.Where(x => x._id.ToString() == assetvalues.Region).ToList();
                    //catasset.Region = regname.Count > 0 ? regname[0].Region : "";


                    //catasset.ItemStatus = assetvalues.ItemStatus;

                    //var SubAreaname = AllMaster.SubAreas.Where(x => x._id.ToString() == assetvalues.SubArea).ToList();
                    //catasset.SubArea = SubAreaname.Count > 0 ? SubAreaname[0].SubArea : "";
                    //lstassign.Add(catasset);

                }

            }

            var jsonResult = Json(lstassign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
        
        //ReassignCodeSearch

        [HttpPost]
        [Authorize]
        public ActionResult reassignsearch_code1(string farId)
        {

            string user = Convert.ToString(Session["Userid"]);
            var gd_data = _AssetService.reloaddataa(farId,user).ToList();
            var AllMaster = _AssetService.getAllCommonMaster();
            var lstassign = new List<CatAssetModel>();

            if (gd_data.Count > 0)
            {
                foreach (Prosol_AssetMaster assetvalues in gd_data)
                {
                    var catasset = new CatAssetModel();

                    catasset.UniqueId = assetvalues.UniqueId;
                    catasset.AssetNo = assetvalues.AssetNo;
                    catasset.Description = assetvalues.Description;
                    catasset.Parent = assetvalues.Parent;
                    catasset.Status = assetvalues.Status;
                    catasset.SerialNo = assetvalues.SerialNo;
                    catasset.ModelNo = assetvalues.ModelNo;
                    catasset.ModelYear = assetvalues.ModelYear;
                    catasset.Manufacturer = assetvalues.Manufacturer;
                    catasset.Catalogue = assetvalues.Catalogue;
                    catasset.ItemStatus = assetvalues.ItemStatus;

                    catasset.Plant = assetvalues.Plant;
                    catasset.AssetNo = assetvalues.AssetNo;
                    catasset.AssetQRCode = assetvalues.AssetQRCode;
                    catasset.Description = assetvalues.Description;
                    catasset.Func_Location = assetvalues.Func_Location;
                    catasset.Equ_Category = assetvalues.Equ_Category;
                    catasset.TechIdentNo = assetvalues.TechIdentNo;
                    catasset.Parent = assetvalues.Parent;
                    catasset.Org_Code = assetvalues.Org_Code;
                    catasset.CostCenter = assetvalues.CostCenter;
                    catasset.CostCenter_Desc = assetvalues.CostCenter_Desc;
                    catasset.ABC_Indicator = assetvalues.ABC_Indicator;
                    catasset.Equ_Category = assetvalues.Equ_Category;
                    catasset.MainWorkCenter = assetvalues.MainWorkCenter;
                    catasset.ObjType = assetvalues.ObjType;
                    catasset.System = assetvalues.System;
                    catasset.PartNo = assetvalues.PartNo;
                    catasset.SerialNo = assetvalues.SerialNo;
                    catasset.ModelNo = assetvalues.ModelNo;
                    catasset.ModelYear = assetvalues.ModelYear;
                    catasset.Manufacturer = assetvalues.Manufacturer;
                    catasset.MfrCountry = assetvalues.MfrCountry;
                    catasset.MfrYear = assetvalues.MfrYear;
                    catasset.AssCondition = assetvalues.AssCondition;
                    catasset.PID_Number = assetvalues.PID_Number;
                    catasset.PID_Desc = assetvalues.PID_Desc;
                    catasset.Section_Number = assetvalues.Section_Number;
                    catasset.Section_Desc = assetvalues.Section_Desc;
                    catasset.Discipline = assetvalues.Discipline;

                    lstassign.Add(catasset);

                }

            }

            var jsonResult = Json(lstassign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
     
        //ReassignLoadData

        [HttpGet]
        [Authorize]
        public JsonResult get_reassigndata(string business, string Major, string Region,string City, string Area, string SubArea)
        {
            var get_reassigndata = _AssetService.reloaddata1(Region,City, Area, SubArea);
            var jsonResult = Json(get_reassigndata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //AssignToPV
        [Authorize]
        public int reAssignwrk_submit()
        {
            var i = 0;
            var selection = Request.Form["selecteditem"];
            Session["selection"] = selection.ToString();
            string FirstName = Request.Form["PVuser"];

            string Role = Request.Form["Business"];
            Session["Role"] = Role.ToString();
            var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
            string term = FirstName;
            var userid = _AssetService.GetCatId(FirstName);

            if (selecteditem.Count > 0)
            {
                Prosol_AssetMaster reasgn = new Prosol_AssetMaster();
                Prosol_UpdatedBy reuserobj = new Prosol_UpdatedBy();
                reuserobj.Name = userid.UserName;
                reuserobj.UserId = userid.Userid;

                reuserobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                reasgn.PVuser = reuserobj;

                foreach (string item in selecteditem)
                {
                    var asgndata = _AssetService.reassign_submit1(Convert.ToString(item), Role, reasgn);
                    if (asgndata)
                    {
                        i++;
                    }
                }
                
                return i;
            }
            else
            {
                return i;
            }
        }
        //cat assign
        //Table Load 
        public JsonResult get_assigndata1()
        {
            var get_assigndata = _AssetService.catloaddata1().ToList();
            var AllMaster = _AssetService.getAllCommonMaster();
            var lstassign = new List<CatAssetModel>();

            if (get_assigndata.Count>0)
            {
                foreach (Prosol_AssetMaster assetvalues in get_assigndata)
                {
                    var catasset = new CatAssetModel();
                    var pvname = new Prosol_UpdatedBy();
                    if (assetvalues.PVuser != null)
                    {
                        pvname.Name = assetvalues.PVuser.Name;
                        catasset.PVuser = pvname;
                    }

                    //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
                    //catasset.Area = Areaname.Count > 0 ? Areaname[0].Area : "";
                    catasset.UniqueId = assetvalues.AssetNo;
                    //catasset.Barcode = assetvalues.Barcode;

                    //var Cityname = AllMaster.Cities.Where(x => x._id.ToString() == assetvalues.City).ToList();
                    //catasset.City = Cityname.Count > 0 ? Cityname[0].City : "";

                    //var Businame = AllMaster.Businesses.Where(x => x._id.ToString() == assetvalues.Business).ToList();
                    //catasset.Business = Businame.Count > 0 ? Businame[0].BusinessName : "";

                    //var EquipClass = AllMaster.EquipmentClasses.Where(x => x._id.ToString() == assetvalues.EquipmentClass).ToList();
                    //catasset.EquipmentClass = EquipClass.Count > 0 ? EquipClass[0].EquipmentClass : "";


                    ////  catasset.EquipmentClass = assetvalues.EquipmentClass;

                    //var EquipType = AllMaster.EquipmentTypes.Where(x => x._id.ToString() == assetvalues.EquipmentType).ToList();
                    //catasset.EquipmentType = EquipType.Count > 0 ? EquipType[0].EquipmentType : "";

                    //catasset.FLOC_Code = assetvalues.FLOC_Code;

                    //var Funcname = AllMaster.MinorClasses.Where(x => x._id.ToString() == assetvalues.MinorClass).ToList();
                    //catasset.MinorClass = Funcname.Count > 0 ? Funcname[0].MinorCode : "";

                    //var majorname = AllMaster.MajorClasses.Where(x => x._id.ToString() == assetvalues.MajorClass).ToList();
                    //catasset.MajorClass = majorname.Count > 0 ? majorname[0].MajorClass : "";

                    ////catasset.MajorClass = assetvalues.MajorClass;

                    //catasset.EquipmentDesc = assetvalues.EquipmentDesc;

                    //var Identiname = AllMaster.Identifiers.Where(x => x._id.ToString() == assetvalues.Identifier).ToList();
                    //catasset.Identifier = Identiname.Count > 0 ? Identiname[0].IdentifierCode : "";

                    //catasset.OldFunLoc = assetvalues.OldFunLoc;
                    //catasset.Quantity = assetvalues.Quantity;

                    //var regname = AllMaster.Regions.Where(x => x._id.ToString() == assetvalues.Region).ToList();
                    //catasset.Region = regname.Count > 0 ? regname[0].Region : "";


                    //catasset.ItemStatus = assetvalues.ItemStatus;

                    //var SubAreaname = AllMaster.SubAreas.Where(x => x._id.ToString() == assetvalues.SubArea).ToList();
                    //catasset.SubArea = SubAreaname.Count > 0 ? SubAreaname[0].SubArea : "";
                    lstassign.Add(catasset);

                }
            }
                var jsonResult = Json(lstassign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        [Authorize]
        public JsonResult getuserassign()
        {
          
            var userlist = _AssetService.getcatuser().ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize]
        public JsonResult getcatuser()
        {

            var userlist = _AssetService.getuser().ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public int catAssignwrk_submit()
        {
            var i = 0;
            var selection = Request.Form["selecteditem"];

            string FirstName = Request.Form["FirstName"];
            string Plantcde = Request.Form["PlantCode"];

            string Role = Request.Form["PlantName"];
            Session["Role"] = Role.ToString();
            var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
           
            var userid = _AssetService.GetCatId(FirstName);

            if (selecteditem.Count > 0)
            {
                Prosol_AssetMaster reasgn = new Prosol_AssetMaster();
                Prosol_UpdatedBy reuserobj = new Prosol_UpdatedBy();
                reuserobj.UserId = userid.Userid;
                reuserobj.Name = FirstName;
                reuserobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //reasgn.Catalogue = UserName;
                //reasgn.PVuser = FirstName;
                reasgn.Catalogue = reuserobj;

                foreach (string item in selecteditem)
                {
                    var asgndata = _AssetService.catreassign_submit(Convert.ToString(item), Role, reasgn);
                    if (asgndata)
                    {
                        i++;
                    }
                }
                return i;
            }
            else
            {
                return i;
            }
        }
        //Bulk user assign

        [Authorize]
        public JsonResult AssetCatBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.AssetCatBulk_Upload(file);
                    }
                }

                if (res.Contains("successfully"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public JsonResult AssetBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    string user = Convert.ToString(Session["Userid"]);
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.AssetBulk_Upload(file, user);
                    }
                }

                if (res.Contains("Success"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public JsonResult AssetProxyBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    string user = Convert.ToString(Session["Userid"]);
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.AssetProxyBulk_Upload(file, user);
                    }
                }

                if (res.Contains("Success"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public JsonResult BOMBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.BOMBulk_Upload(file);
                    }
                }

                if (res.Contains("Success"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AttriBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.AttriBulk_Upload(file);
                    }
                }

                if (res.Contains("Success"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult VendorBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.VendorBulk_Upload(file);
                    }
                }

                if (res.Contains("Success"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        //Asset Masters
        public JsonResult GetAssetMaster()
        {
            var objList = _AssetService.getAllCommonMasterforTools();

            var lst = new List<AssetBusiness>();
            var newlst = new assetCommon();
            foreach (Prosol_Business mdl in objList.Businesses)
            {
                var obj = new AssetBusiness();
                obj.id = mdl._id.ToString();
                obj.BusinessCode = mdl.BusinessCode;
                obj.BusinessName = mdl.BusinessName;
                obj.IsActive = mdl.IsActive;
                lst.Add(obj);

            }
            newlst.Businesses = lst;

            var Majorlst = new List<AssetMajorClass>();

            foreach (Prosol_MajorClass mdl in objList.MajorClasses)
            {
                var obj = new AssetMajorClass();
                var busname = objList.Businesses.Where(x => x._id.ToString() == mdl.Business_id).ToList();
                obj.Business_id = busname.Count > 0 ? busname[0].BusinessName : "";
               // obj.Business_id = mdl.Business_id;
                obj.id = mdl._id.ToString();
                obj.MajorCode = mdl.MajorCode;
                obj.MajorClass = mdl.MajorClass;
                obj.IsActive = mdl.IsActive;
                Majorlst.Add(obj);

            }

            newlst.MajorClasses = Majorlst;

            var Minorlst = new List<AssetMinorClass>();

            foreach (Prosol_MinorClass mdl in objList.MinorClasses)
            {
                var obj = new AssetMinorClass();
              

                var mrjname = objList.MajorClasses.Where(x => x._id.ToString() == mdl.Major_id).ToList();
                obj.Major_id = mrjname.Count > 0 ? mrjname[0].MajorClass : "";
                if(mrjname.Count>0){
                    var busname = objList.Businesses.Where(x => x._id.ToString() == mrjname[0].Business_id).ToList();
                    var businame = busname.Count > 0 ? busname[0].BusinessName : "";
                    obj.Business_Id = businame;
                }
                    obj.id = mdl._id.ToString();

              
                obj.MinorCode = mdl.MinorCode;
                obj.MinorClass = mdl.MinorClass;
                obj.IsActive = mdl.IsActive;
                Minorlst.Add(obj);

            }
            newlst.MinorClasses = Minorlst;

            var Identilst = new List<AssetIdentifier>();
            //foreach (Prosol_Identifier mdl in objList.Identifiers)
            //{
            //    var obj = new AssetIdentifier();
            //    var mnrname = objList.MinorClasses.Where(x => x._id.ToString() == mdl.fClass_Id).ToList();
            //    obj.fClass_Id = mnrname.Count > 0 ? mnrname[0].MinorClass : "";

            //    var mrjname = objList.MajorClasses.Where(x => x._id.ToString() == mnrname[0].Major_id).ToList();
            //    var Majorname = mrjname.Count > 0 ? mrjname[0].MajorClass : "";
            //    //obj.bu = mrjname.Count > 0 ? mrjname[0].MajorClass : "";

            //    if (mrjname.Count > 0)
            //    {
            //        var busname = objList.Businesses.Where(x => x._id.ToString() == mrjname[0].Business_id).ToList();
            //        var businame = busname.Count > 0 ? busname[0].BusinessName : "";
            //        obj.Business_Id = businame;
            //    }

            //    obj.id = mdl._id.ToString();
            //    obj.Identifier = mdl.Identifier;
            //    obj.IdentifierCode = mdl.IdentifierCode;
            //    obj.IsActive = mdl.IsActive;
             
            //    obj.Major_Id = Majorname;
            //    Identilst.Add(obj);

            //}
            //newlst.Identifiers = Identilst;

            var Regionlst = new List<AssetRegion>();

            foreach (Prosol_Region mdl in objList.Regions)
            {
                var obj = new AssetRegion();
                obj.Region = mdl.Region;
                obj.id = mdl._id.ToString();
                obj.RegionCode = mdl.RegionCode;
                obj.IsActive = mdl.IsActive;


                Regionlst.Add(obj);

            }
            newlst.Regions = Regionlst;

            var Citylst = new List<AssetCity>();

            foreach (Prosol_City mdl in objList.Cities)
            {
                var obj = new AssetCity();
                var regname = objList.Regions.Where(x => x._id.ToString() == mdl.Region_Id).ToList();
               obj.Region_Id = regname.Count > 0 ? regname[0].Region : "";

               //  obj.Region_Id = mdl.Region_Id;
                obj.City = mdl.City;
                obj.CityCode = mdl.CityCode;
                obj.IsActive = mdl.IsActive;
                obj.id = mdl._id.ToString();


                Citylst.Add(obj);

            }
            newlst.Cities = Citylst;

            var Arealst = new List<AssetArea>();

            foreach (Prosol_Area mdl in objList.Areas)
            {
                var obj = new AssetArea();
                var cityname = objList.Cities.Where(x => x._id.ToString() == mdl.City_Id).ToList();
                obj.City_Id = cityname.Count > 0 ? cityname[0].City : "";

                var regname = objList.Regions.Where(x => x._id.ToString() == cityname[0].Region_Id).ToList();
                var Regioname= regname.Count > 0 ? regname[0].Region : "";
                //   obj.City_Id = mdl.City_Id;
                obj.Region_Id = Regioname;
                obj.Area = mdl.Area;
                obj.AreaCode = mdl.AreaCode;
                obj.id = mdl._id.ToString();
                obj.IsActive = mdl.IsActive;

                Arealst.Add(obj);

            }
            newlst.Areas = Arealst;

            var SubArealst = new List<AssetsubArea>();

            foreach (Prosol_SubArea mdl in objList.SubAreas)
            {
                var obj = new AssetsubArea();
                var areaname = objList.Areas.Where(x => x._id.ToString() == mdl.Area_Id).ToList();
                obj.Area_Id = areaname.Count > 0 ? areaname[0].Area : "";

                var cityname = objList.Cities.Where(x => x._id.ToString() == areaname[0].City_Id).ToList();
                var CitName= cityname.Count > 0 ? cityname[0].City : "";

                var regname = objList.Regions.Where(x => x._id.ToString() == cityname[0].Region_Id).ToList();
                var Regioname = regname.Count > 0 ? regname[0].Region : "";
                //obj.Area_Id = mdl.Area_Id;
                obj.Region_Id = Regioname;
                obj.City_Id = CitName;
                obj.SubArea = mdl.SubArea;
                obj.SubAreaCode = mdl.SubAreaCode;
                obj.id = mdl._id.ToString();
                obj.IsActive = mdl.IsActive;

                SubArealst.Add(obj);

            }
            newlst.SubAreas = SubArealst;


            var EquipClasslst = new List<AssetEquipClass>();

            foreach (Prosol_EquipmentClass mdl in objList.EquipmentClasses)
            {
                var obj = new AssetEquipClass();
                obj.EquipmentClass = mdl.EquipmentClass;
                obj.id = mdl._id.ToString();
                obj.EquipmentCode = mdl.EquipmentCode;
                obj.IsActive = mdl.IsActive;



                EquipClasslst.Add(obj);

            }
            newlst.EquipmentClasses = EquipClasslst;

            var Equiptypelst = new List<AssetEquipType>();
            foreach (Prosol_EquipmentType mdl in objList.EquipmentTypes)
            {
                var obj = new AssetEquipType();

                var equpclassname = objList.EquipmentClasses.Where(x => x._id.ToString() == mdl.EquClass_Id).ToList();
                obj.EquClass_Id = equpclassname.Count > 0 ? equpclassname[0].EquipmentClass : "";
                // obj.EquClass_Id = mdl.EquClass_Id;
                obj.EquipmentType = mdl.EquipmentType;
                obj.EquTypeCode = mdl.EquTypeCode;
                obj.IsActive = mdl.IsActive;
                obj.id = mdl._id.ToString();
                Equiptypelst.Add(obj);

            }
            newlst.EquipmentTypes = Equiptypelst;

            var Locatnlst = new List<AssetLocation>();
            foreach (Prosol_Location mdl in objList.Locations)
            {
                var obj = new AssetLocation();
                var areaname = objList.Areas.Where(x => x._id.ToString() == mdl.Area_Id).ToList();
                obj.Area_Id = areaname.Count > 0 ? areaname[0].Area : "";
                obj.Location = mdl.Location;
                obj.LocationCode = mdl.LocationCode;
                obj.IsActive = mdl.IsActive;
                obj.id = mdl._id.ToString();
                Locatnlst.Add(obj);

            }
            newlst.Locations = Locatnlst;

            var jsonResult = Json(newlst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return this.Json(newlst, JsonRequestBehavior.AllowGet);

        }
        public void DownloadIdentifier()
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> attRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> aRow;
            var objList = _AssetService.getAllCommonMasterforTools();
            var Identilst = new List<AssetIdentifier>();
            foreach (Prosol_Identifier mdl in objList.Identifiers)
            {
                var obj = new AssetIdentifier();
                var mnrname = objList.MinorClasses.Where(x => x._id.ToString() == mdl.fClass_Id).ToList();
                obj.fClass_Id = mnrname.Count > 0 ? mnrname[0].MinorClass : "";

                var mrjname = objList.MajorClasses.Where(x => x._id.ToString() == mnrname[0].Major_id).ToList();
                var Majorname = mrjname.Count > 0 ? mrjname[0].MajorClass : "";
                //obj.bu = mrjname.Count > 0 ? mrjname[0].MajorClass : "";

                if (mrjname.Count > 0)
                {
                    var busname = objList.Businesses.Where(x => x._id.ToString() == mrjname[0].Business_id).ToList();
                    var businame = busname.Count > 0 ? busname[0].BusinessName : "";
                    obj.Business_Id = businame;
                }

                obj.id = mdl._id.ToString();
                obj.Identifier = mdl.Identifier;
                obj.IdentifierCode = mdl.IdentifierCode;
                obj.IsActive = mdl.IsActive;

                obj.Major_Id = Majorname;
                Identilst.Add(obj);

            }
            foreach (var cde in Identilst)
            {

                row = new Dictionary<string, object>();
                row.Add("Business", cde.Business_Id);
                row.Add("Majorclass", cde.Major_Id);
                row.Add("Minorclass", cde.fClass_Id);
                row.Add("IdentifierCode", cde.IdentifierCode);
                row.Add("Identifier", cde.Identifier);
                rows.Add(row);
            }
            DataTable dt = new DataTable();

            foreach (IDictionary<string, object> row1 in rows)
            {
                var rw = dt.NewRow();

                foreach (KeyValuePair<string, object> entry in row1)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;

                }
                dt.Rows.Add(rw);
                // dt.Rows.Add(row.Values.ToArray());
            }
           


            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "Identifiers");
          
            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"IdentifierMaster.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        //Master for dropdown
        public JsonResult dropdownasset()
        {
            var objList = _AssetService.getAllCommonMasterforTools();

            var lst = new List<AssetBusiness>();
            var newlst = new assetCommon();
            foreach (Prosol_Business mdl in objList.Businesses)
            {
                var obj = new AssetBusiness();
                obj.id = mdl._id.ToString();
                obj.BusinessCode = mdl.BusinessCode;
                obj.BusinessName = mdl.BusinessName;
                obj.IsActive = mdl.IsActive;
                lst.Add(obj);

            }
            newlst.Businesses = lst;

            var Majorlst = new List<AssetMajorClass>();

            foreach (Prosol_MajorClass mdl in objList.MajorClasses)
            {
                var obj = new AssetMajorClass();
                var busname = objList.Businesses.Where(x => x._id.ToString() == mdl.Business_id).ToList();
              //  obj.Business_id = busname.Count > 0 ? busname[0].BusinessName : "";
                obj.Business_id = mdl.Business_id;
                obj.id = mdl._id.ToString();
                obj.MajorCode = mdl.MajorCode;
                obj.MajorClass = mdl.MajorClass;
                obj.IsActive = mdl.IsActive;
                Majorlst.Add(obj);

            }

            newlst.MajorClasses = Majorlst;

            var Minorlst = new List<AssetMinorClass>();

            foreach (Prosol_MinorClass mdl in objList.MinorClasses)
            {
                var obj = new AssetMinorClass();


                var mrjname = objList.MajorClasses.Where(x => x._id.ToString() == mdl.Major_id).ToList();
              //  obj.Major_id = mrjname.Count > 0 ? mrjname[0].MajorClass : "";
                //obj.bu = mrjname.Count > 0 ? mrjname[0].MajorClass : "";
               obj.Major_id = mdl.Major_id;
                var busname = objList.Businesses.Where(x => x._id.ToString() == mdl.Business_id).ToList();
                var businame = busname.Count > 0 ? busname[0].BusinessName : "";
                //obj.Major_id = mdl.Major_id;
                obj.id = mdl._id.ToString();

                obj.Business_Id = businame;
                obj.MinorCode = mdl.MinorCode;
                obj.MinorClass = mdl.MinorClass;
                obj.IsActive = mdl.IsActive;
                Minorlst.Add(obj);

            }
            newlst.MinorClasses = Minorlst;

            var Identilst = new List<AssetIdentifier>();
            //foreach (Prosol_Identifier mdl in objList.Identifiers)
            //{
            //    var obj = new AssetIdentifier();
            //    var mnrname = objList.MinorClasses.Where(x => x._id.ToString() == mdl.fClass_Id).ToList();
            //    obj.fClass_Id = mnrname.Count > 0 ? mnrname[0].MinorClass : "";

            //    var mrjname = objList.MajorClasses.Where(x => x._id.ToString() == mdl.MajorClass_Id).ToList();
            //    var Majorname = mrjname.Count > 0 ? mrjname[0].MajorClass : "";
            //    //obj.bu = mrjname.Count > 0 ? mrjname[0].MajorClass : "";

            //    var busname = objList.Businesses.Where(x => x._id.ToString() == mdl.Business_Id).ToList();
            //    var businame = busname.Count > 0 ? busname[0].BusinessName : "";
            //    //obj.fClass_Id = mdl.fClass_Id;

            //    obj.id = mdl._id.ToString();
            //    obj.Identifier = mdl.Identifier;
            //    obj.IdentifierCode = mdl.IdentifierCode;
            //    obj.IsActive = mdl.IsActive;
            //    obj.Business_Id = businame;
            //    obj.Major_Id = Majorname;
            //    Identilst.Add(obj);

            //}
            //newlst.Identifiers = Identilst;

            var Regionlst = new List<AssetRegion>();

            foreach (Prosol_Region mdl in objList.Regions)
            {
                var obj = new AssetRegion();
                obj.Region = mdl.Region;
                obj.id = mdl._id.ToString();
                obj.RegionCode = mdl.RegionCode;
                obj.IsActive = mdl.IsActive;


                Regionlst.Add(obj);

            }
            newlst.Regions = Regionlst;

            var Citylst = new List<AssetCity>();

            foreach (Prosol_City mdl in objList.Cities)
            {
                var obj = new AssetCity();
                var regname = objList.Regions.Where(x => x._id.ToString() == mdl.Region_Id).ToList();
                //obj.Region_Id = regname.Count > 0 ? regname[0].Region : "";

                 obj.Region_Id = mdl.Region_Id;
                obj.City = mdl.City;
                obj.CityCode = mdl.CityCode;
                obj.IsActive = mdl.IsActive;
                obj.id = mdl._id.ToString();


                Citylst.Add(obj);

            }
            newlst.Cities = Citylst;

            var Arealst = new List<AssetArea>();

            foreach (Prosol_Area mdl in objList.Areas)
            {
                var obj = new AssetArea();
                var cityname = objList.Cities.Where(x => x._id.ToString() == mdl.City_Id).ToList();
           //     obj.City_Id = cityname.Count > 0 ? cityname[0].City : "";

                var regname = objList.Regions.Where(x => x._id.ToString() == cityname[0].Region_Id).ToList();
                var Regioname = regname.Count > 0 ? regname[0].Region : "";
                  obj.City_Id = mdl.City_Id;
                obj.Region_Id = Regioname;
                obj.Area = mdl.Area;
                obj.AreaCode = mdl.AreaCode;
                obj.id = mdl._id.ToString();
                obj.IsActive = mdl.IsActive;

                Arealst.Add(obj);

            }
            newlst.Areas = Arealst;

            var SubArealst = new List<AssetsubArea>();

            foreach (Prosol_SubArea mdl in objList.SubAreas)
            {
                var obj = new AssetsubArea();
                var areaname = objList.Areas.Where(x => x._id.ToString() == mdl.Area_Id).ToList();
                obj.Area_Id = areaname.Count > 0 ? areaname[0].Area : "";

                var cityname = objList.Cities.Where(x => x._id.ToString() == areaname[0].City_Id).ToList();
                var CitName = cityname.Count > 0 ? cityname[0].City : "";

                var regname = objList.Regions.Where(x => x._id.ToString() == cityname[0].Region_Id).ToList();
                var Regioname = regname.Count > 0 ? regname[0].Region : "";
                //obj.Area_Id = mdl.Area_Id;
                obj.Region_Id = Regioname;
                obj.City_Id = CitName;
                obj.SubArea = mdl.SubArea;
                obj.SubAreaCode = mdl.SubAreaCode;
                obj.id = mdl._id.ToString();
                obj.IsActive = mdl.IsActive;

                SubArealst.Add(obj);

            }
            newlst.SubAreas = SubArealst;


            var EquipClasslst = new List<AssetEquipClass>();

            foreach (Prosol_EquipmentClass mdl in objList.EquipmentClasses)
            {
                var obj = new AssetEquipClass();
                obj.EquipmentClass = mdl.EquipmentClass;
                obj.id = mdl._id.ToString();
                obj.EquipmentCode = mdl.EquipmentCode;
                obj.IsActive = mdl.IsActive;



                EquipClasslst.Add(obj);

            }
            newlst.EquipmentClasses = EquipClasslst;

            var Equiptypelst = new List<AssetEquipType>();
            foreach (Prosol_EquipmentType mdl in objList.EquipmentTypes)
            {
                var obj = new AssetEquipType();

                var equpclassname = objList.EquipmentClasses.Where(x => x._id.ToString() == mdl.EquClass_Id).ToList();
                obj.EquClass_Id = equpclassname.Count > 0 ? equpclassname[0].EquipmentClass : "";
                // obj.EquClass_Id = mdl.EquClass_Id;
                obj.EquipmentType = mdl.EquipmentType;
                obj.EquTypeCode = mdl.EquTypeCode;
                obj.IsActive = mdl.IsActive;
                obj.id = mdl._id.ToString();
                Equiptypelst.Add(obj);

            }
            newlst.EquipmentTypes = Equiptypelst;

            var Locatnlst = new List<AssetLocation>();
            foreach (Prosol_Location mdl in objList.Locations)
            {
                var obj = new AssetLocation();
                var areaname = objList.Areas.Where(x => x._id.ToString() == mdl.Area_Id).ToList();
                obj.Area_Id = areaname.Count > 0 ? areaname[0].Area : "";
                obj.Location = mdl.Location;
                obj.LocationCode = mdl.LocationCode;
                obj.IsActive = mdl.IsActive;
                obj.id = mdl._id.ToString();
                Locatnlst.Add(obj);

            }
            newlst.Locations = Locatnlst;
            return this.Json(newlst, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult InsertFuncLoc()
        {
            var obj = Request.Form["Data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_Funloc>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new Prosol_Funloc();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Funloc();
                    //mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Level1 = Model.Level1;
                    mdl.Level2 = Model.Level2;
                    mdl.Level3 = Model.Level3;
                    mdl.Level4 = Model.Level4;
                    mdl.Level5 = Model.Level5;
                    mdl.Level6 = Model.Level6;
                    mdl.Level7 = Model.Level7;
                    mdl.Equipment = Model.Equipment;
                    mdl.PrimaryEquipment = Model.PrimaryEquipment;
                    mdl.SubEquipment1 = Model.SubEquipment1;
                    mdl.SubEquipment2 = Model.SubEquipment2;
                    mdl.SubEquipment3 = Model.SubEquipment3;
                    mdl.FunctLocation = Model.FunctLocation;
                    mdl.SuperiorLocation = Model.SuperiorLocation;
                    mdl.SectionNo = Model.SectionNo;
                    mdl.Sequence = Model.Sequence;
                    mdl.UniqueId = Model.UniqueId;
                    mdl.Islive = true;


                    res = _AssetService.InsertDataFL(mdl);
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
        [HttpPost]
        public JsonResult UpdateFuncLoc()
        {
            var obj = Request.Form["Data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_Funloc>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new Prosol_Funloc();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Funloc();
                    //mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Level1 = Model.Level1;
                    mdl.Level2 = Model.Level2;
                    mdl.Level3 = Model.Level3;
                    mdl.Level4 = Model.Level4;
                    mdl.Level5 = Model.Level5;
                    mdl.Level6 = Model.Level6;
                    mdl.Level7 = Model.Level7;
                    mdl.Equipment = Model.Equipment;
                    mdl.PrimaryEquipment = Model.PrimaryEquipment;
                    mdl.SubEquipment1 = Model.SubEquipment1;
                    mdl.SubEquipment2 = Model.SubEquipment2;
                    mdl.SubEquipment3 = Model.SubEquipment3;
                    mdl.FunctLocation = Model.FunctLocation;
                    mdl.SuperiorLocation = Model.SuperiorLocation;
                    mdl.SectionNo = Model.SectionNo;
                    mdl.Sequence = Model.Sequence;
                    mdl.UniqueId = Model.UniqueId;
                    mdl.Islive = true;


                    res = _AssetService.UpdateDataFL(mdl);
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

        [HttpPost]
        public JsonResult InsertDataRegion()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetRegion>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetRegion();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Region();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.Region = Model.Region;
                    mdl.RegionCode = Model.RegionCode;


                    res = _AssetService.InsertDataRegn(mdl);
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




        [HttpPost]
        public JsonResult InsertDataCity()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetCity>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetCity();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_City();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.City = Model.City;
                    mdl.CityCode = Model.CityCode;
                    mdl.Region_Id = Model.Region_Id;



                    res = _AssetService.InsertDataCity(mdl);
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

        //Area
        [HttpPost]
        public JsonResult InsertDataArea()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetArea>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetArea();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Area();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.Area = Model.Area;
                    mdl.AreaCode = Model.AreaCode;
                    mdl.City_Id = Model.City_Id;
                    mdl.Region_Id = Model.Region_Id;


                    res = _AssetService.InsertDataAre(mdl);
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
        [HttpPost]
        public JsonResult InsertDataSubArea()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetsubArea>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetsubArea();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_SubArea();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.SubArea = Model.SubArea;
                    mdl.SubAreaCode = Model.SubAreaCode;
                    mdl.Area_Id = Model.Area_Id;
                    mdl.Region_Id = Model.Region_Id;
                    mdl.City_Id = Model.City_Id;


                    res = _AssetService.InsertDataSubAre(mdl);
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
        [HttpPost]
        public JsonResult InsertDataLoc()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetLocation>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetLocation();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Location();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.Location = Model.Location;
                    mdl.LocationCode = Model.LocationCode;
                    mdl.Area_Id = Model.Area_Id;


                    res = _AssetService.InsertDataLoc(mdl);
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
        [HttpPost]
        public JsonResult InsertDataEquipClass()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetEquipClass>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetEquipClass();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_EquipmentClass();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.EquipmentClass = Model.EquipmentClass;
                    mdl.EquipmentCode = Model.EquipmentCode;


                    res = _AssetService.InsertDataEquipClass(mdl);
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
        [HttpPost]
        public JsonResult InsertDataEquipType()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetEquipType>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetEquipType();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_EquipmentType();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.EquipmentType = Model.EquipmentType;
                    mdl.EquTypeCode = Model.EquTypeCode;
                    mdl.EquClass_Id = Model.EquClass_Id;


                    res = _AssetService.InsertDataEquipType(mdl);
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


        public JsonResult DisableReg(string id, bool Islive)
        {

            var res = _AssetService.DisableReg(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableCity(string id, bool Islive)
        {

            var res = _AssetService.DisableCity(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableArea(string id, bool Islive)
        {

            var res = _AssetService.DisableArea(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableSubArea(string id, bool Islive)
        {

            var res = _AssetService.DisableSubArea(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableLoc(string id, bool Islive)
        {

            var res = _AssetService.DisableLoc(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableEquipClass(string id, bool Islive)
        {

            var res = _AssetService.DisableEquipClass(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableEquipType(string id, bool Islive)
        {

            var res = _AssetService.DisableEquipType(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertDataBusiness()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetFARMaster>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetFARMaster();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_FARMaster();

                    mdl.Code = Model.Code;
                    mdl.Title = Model.Title;
                    mdl.Label = Model.Label;
                    mdl.Islive = true;

                    res = _AssetService.InsertDataBusiness(mdl);
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
        [HttpPost]
        public JsonResult UpdateDataBusiness()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetFARMaster>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetFARMaster();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_FARMaster();

                    mdl.Code = Model.Code;
                    mdl.Title = Model.Title;
                    mdl.Label = Model.Label;
                    mdl.Islive = true;

                    res = _AssetService.UpdateDataBusiness(mdl);
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

        public JsonResult InsertDataFar()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_FARRepository>(obj);
            bool res = false;
            try
            {
                var mdl = new Prosol_FARRepository();
                mdl.FARId = Model.FARId;
                mdl.Region = Model.Region;
                mdl.AssetDesc = Model.AssetDesc;
                res = _AssetService.InsertDataFar(mdl);
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertDataSite()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_SiteMaster>(obj);
            bool res = false;
            try
            {
                var mdl = new Prosol_SiteMaster();
                mdl.SiteId = Model.SiteId;
                mdl.Cluster = Model.Cluster;
                mdl.HighLevelLocation = Model.HighLevelLocation;
                res = _AssetService.InsertDataSite(mdl);
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertDataLoc1()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_Location>(obj);
            bool res = false;
            try
            {
                var mdl = new Prosol_Location();
                mdl.Location = Model.Location;
                mdl.LocationHierarchy = Model.LocationHierarchy;
                res = _AssetService.InsertDataLoc1(mdl);
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertDataAT()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_AssetTypeMaster>(obj);
            bool res = false;
            try
            {
                var mdl = new Prosol_AssetTypeMaster();
                mdl.AssetType = Model.AssetType;
                mdl.ClassificationHierarchyDesc = Model.ClassificationHierarchyDesc;
                mdl.FailureCode = Model.FailureCode;
                res = _AssetService.InsertDataAT(mdl);
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveMfr(string id, bool IsActive, string flg)
        {

            var res = _AssetService.RemoveMfr(id, IsActive, flg);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableFunLoc(string section, string id, bool sts)
        {

            var res = _AssetService.DisableFunLoc(section, id, sts);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableBun(string id, bool IsActive)
        {

            var res = _AssetService.DisableBus(id, IsActive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DisableNotes(string id, bool IsActive)
        {

            var res = _AssetService.DisableNotes(id, IsActive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult InsertData()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetMajorClass>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetMajorClass();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_MajorClass();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.Business_id = Model.Business_id;
                    mdl.MajorCode = Model.MajorCode;
                    mdl.MajorClass = Model.MajorClass;
                    mdl.IsActive = true;
                    res = _AssetService.InsertData(mdl);
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

        public JsonResult Disablemjr(string id, bool IsActive)
        {

            var res = _AssetService.Disablemjr(id, IsActive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult InsertData1()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetMinorClass>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetMinorClass();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_MinorClass();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.Major_id = Model.Major_id;
                    mdl.MinorCode = Model.MinorCode;
                    mdl.MinorClass = Model.MinorClass;
                    mdl.Business_id = Model.Business_Id;
                    mdl.IsActive = true;
                    res = _AssetService.InsertData1(mdl);
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

        public JsonResult Disablemnr(string id, bool IsActive)
        {

            var res = _AssetService.Disablemnr(id, IsActive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }




        public JsonResult InsertDataIdent()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetIdentifier>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new AssetIdentifier();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_Identifier();
                    mdl._id = Model.id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model.id);
                    mdl.fClass_Id = Model.fClass_Id;
                    mdl.IdentifierCode = Model.IdentifierCode;
                    mdl.Identifier = Model.Identifier;
                    mdl.Business_Id = Model.Business_Id;
                    mdl.MajorClass_Id = Model.Major_Id;
                    mdl.IsActive = true;
                    res = _AssetService.InsertDataIdent(mdl);
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
        public JsonResult Disableidnt(string id, bool IsActive)
        {

            var res = _AssetService.Disableidnt(id, IsActive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult IdentifierBulk_Upload()
        {
            try
            {
                string res = "";
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        res = _AssetService.IdentifierBulk_Upload(file);
                    }
                }
                if (res.Contains("Success"))
                    return this.Json(res, JsonRequestBehavior.AllowGet);
                else return this.Json(res = "Error : " + res, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return this.Json(e.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        //ItemCode Search
        [Authorize]
        public JsonResult searchMaster()
         {
            string sCode = Request.Form["sCode"];
            string sSource = Request.Form["sSource"];
            string sUser = Request.Form["sUser"];
            string sStatus = Request.Form["sStatus"];
            string sQR = Request.Form["sQR"];
            var AllMaster = _AssetService.getAllCommonMaster();
            var dataList = _AssetService.searchItemCode(sCode, sSource, sUser, sStatus, sQR);
            var locObjList = new List<CatAssetModel>();
            if (dataList != null && dataList.Count > 0)
            {

                foreach (Prosol_AssetMaster assetvalues in dataList)
                {
                    var catasset = new CatAssetModel();
                    catasset.ItemStatus = assetvalues.ItemStatus;
                    catasset.UniqueId = assetvalues.UniqueId;
                    catasset.AssetNo = assetvalues.AssetNo;
                    catasset.Func_Location = assetvalues.Func_Location;
                    catasset.Description = assetvalues.Description;
                    catasset.Description_ = assetvalues.Description_;
                    catasset.TechIdentNo = assetvalues.TechIdentNo;
                    catasset.AssetQRCode = assetvalues.AssetQRCode;
                    catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                    catasset.NewTagNo = assetvalues.NewTagNo;

                    catasset.PVuser = new Prosol_UpdatedBy();
                    if (assetvalues.PVuser != null)
                    {
                        catasset.PVuser.Name = assetvalues.PVuser.Name;
                        catasset.PVuser.UpdatedOn = assetvalues.PVuser.UpdatedOn;
                    }
                  

                    var catname = new Prosol_UpdatedBy();
                    if (assetvalues.Catalogue != null)
                    {
                        catname.Name = assetvalues.Catalogue.Name;
                        catasset.Catalogue = catname;
                    }
                    catasset.Review = new Prosol_UpdatedBy();
                    if (assetvalues.Review != null)
                    {
                        catasset.Review.Name = assetvalues.Review != null ? assetvalues.Review.Name : "";
                    }
                    locObjList.Add(catasset);
                }
               
            }           
            var jsonResult = Json(locObjList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [Authorize]
        public bool PVreAssignwrk_submit()
        {
            var selection = Request.Form["selecteditem"];
            string FirstName = Request.Form["FirstName"];
            string username = Request.Form["Username"];
            Session["Username"] = username.ToString();
            string Role = Request.Form["Role"];
            Session["Role"] = Role.ToString();
            //  Session["Role"] = Role.ToString();
            var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
            string term = FirstName;
            var userid = _AssetService.AutoSearchUserName(term).ToList();
            if (selecteditem.Count > 0)
            {
                Prosol_AssetMaster reasgn = new Prosol_AssetMaster();
                Prosol_UpdatedBy reuserobj = new Prosol_UpdatedBy();
                reuserobj.UserId = userid[0].Userid;
                reuserobj.Name = FirstName;
                reuserobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                if (Role == "Cataloguer")
                    reasgn.Catalogue = reuserobj;
                if (Role == "PV User")
                    reasgn.PVuser = reuserobj;
                //else
                //    reasgn.Release = reuserobj;

                foreach (string item in selecteditem)
                {
                    var asgndata = _AssetService.PVreassign_submit(Convert.ToString(item), Role, reasgn);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Authorize]
        public JsonResult getuser(string role)
        {

            var userlist = _AssetService.getuseronly(role).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        [Authorize]
        public ActionResult pv_reassign(string PVUsername, string Role)
        {

            Session["Username"] = PVUsername;
            Session["Role"] = Role;
            var gd_data = _AssetService.PV_Reassign(PVUsername, Role).ToList();
            var AllMaster = _AssetService.getAllCommonMaster();
            var lstassign = new List<CatAssetModel>();

            if (gd_data.Count > 0)
            {
                foreach (Prosol_AssetMaster assetvalues in gd_data)
                {
                    var catasset = new CatAssetModel();

                    //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
                    //catasset.Area = Areaname.Count > 0 ? Areaname[0].Area : "";
                    catasset.UniqueId = assetvalues.UniqueId;
                    catasset.AssetNo = assetvalues.AssetNo;
                    //catasset.Barcode = assetvalues.Barcode;

                    //var Cityname = AllMaster.Cities.Where(x => x._id.ToString() == assetvalues.City).ToList();
                    //catasset.City = Cityname.Count > 0 ? Cityname[0].City : "";

                    //var Businame = AllMaster.Businesses.Where(x => x._id.ToString() == assetvalues.Business).ToList();
                    //catasset.Business = Businame.Count > 0 ? Businame[0].BusinessName : "";

                    //var EquipClass = AllMaster.EquipmentClasses.Where(x => x._id.ToString() == assetvalues.EquipmentClass).ToList();
                    //catasset.EquipmentClass = EquipClass.Count > 0 ? EquipClass[0].EquipmentClass : "";


                    ////  catasset.EquipmentClass = assetvalues.EquipmentClass;

                    //var EquipType = AllMaster.EquipmentTypes.Where(x => x._id.ToString() == assetvalues.EquipmentType).ToList();
                    //catasset.EquipmentType = EquipType.Count > 0 ? EquipType[0].EquipmentType : "";

                    //catasset.FLOC_Code = assetvalues.FLOC_Code;

                    //var Funcname = AllMaster.MinorClasses.Where(x => x._id.ToString() == assetvalues.MinorClass).ToList();
                    //catasset.MinorClass = Funcname.Count > 0 ? Funcname[0].MinorCode : "";

                    //var majorname = AllMaster.MajorClasses.Where(x => x._id.ToString() == assetvalues.MajorClass).ToList();
                    //catasset.MajorClass = majorname.Count > 0 ? majorname[0].MajorClass : "";

                    ////catasset.MajorClass = assetvalues.MajorClass;

                    //catasset.EquipmentDesc = assetvalues.EquipmentDesc;

                    //var Identiname = AllMaster.Identifiers.Where(x => x._id.ToString() == assetvalues.Identifier).ToList();
                    //catasset.Identifier = Identiname.Count > 0 ? Identiname[0].IdentifierCode : "";

                    //catasset.OldFunLoc = assetvalues.OldFunLoc;
                    //catasset.Quantity = assetvalues.Quantity;

                    //var regname = AllMaster.Regions.Where(x => x._id.ToString() == assetvalues.Region).ToList();
                    //catasset.Region = regname.Count > 0 ? regname[0].Region : "";


                    //catasset.ItemStatus = assetvalues.ItemStatus;

                    //var SubAreaname = AllMaster.SubAreas.Where(x => x._id.ToString() == assetvalues.SubArea).ToList();
                    //catasset.SubArea = SubAreaname.Count > 0 ? SubAreaname[0].SubArea : "";
                    catasset.AssetNo = assetvalues.AssetNo;
                    catasset.Description = assetvalues.Description;
                    catasset.Parent = assetvalues.Parent;
                    catasset.Status = assetvalues.Status;
                    catasset.SerialNo = assetvalues.SerialNo;
                    catasset.ModelNo = assetvalues.ModelNo;
                    catasset.ModelYear = assetvalues.ModelYear;
                    catasset.Manufacturer = assetvalues.Manufacturer;
                    catasset.Catalogue = assetvalues.Catalogue;

                    catasset.Plant = assetvalues.Plant;
                    catasset.AssetNo = assetvalues.AssetNo;
                    catasset.AssetQRCode = assetvalues.AssetQRCode;
                    catasset.Description = assetvalues.Description;
                    catasset.Func_Location = assetvalues.Func_Location;
                    catasset.Equ_Category = assetvalues.Equ_Category;
                    catasset.TechIdentNo = assetvalues.TechIdentNo;
                    catasset.Parent = assetvalues.Parent;
                    catasset.Org_Code = assetvalues.Org_Code;
                    catasset.CostCenter = assetvalues.CostCenter;
                    catasset.CostCenter_Desc = assetvalues.CostCenter_Desc;
                    catasset.ABC_Indicator = assetvalues.ABC_Indicator;
                    catasset.Equ_Category = assetvalues.Equ_Category;
                    catasset.MainWorkCenter = assetvalues.MainWorkCenter;
                    catasset.ObjType = assetvalues.ObjType;
                    catasset.System = assetvalues.System;
                    catasset.PartNo = assetvalues.PartNo;
                    catasset.SerialNo = assetvalues.SerialNo;
                    catasset.ModelNo = assetvalues.ModelNo;
                    catasset.ModelYear = assetvalues.ModelYear;
                    catasset.Manufacturer = assetvalues.Manufacturer;
                    catasset.MfrCountry = assetvalues.MfrCountry;
                    catasset.MfrYear = assetvalues.MfrYear;
                    catasset.AssCondition = assetvalues.AssCondition;
                    catasset.PID_Number = assetvalues.PID_Number;
                    catasset.PID_Desc = assetvalues.PID_Desc;
                    catasset.Section_Number = assetvalues.Section_Number;
                    catasset.Section_Desc = assetvalues.Section_Desc;
                    catasset.Discipline = assetvalues.Discipline;
                    lstassign.Add(catasset);

                }

            }

            var jsonResult = Json(lstassign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
        public JsonResult getnameplateImges()
        {
            string folderPath = Server.MapPath("~/Content/images/NamePlateImgs/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        [Authorize]
        public JsonResult UploadnameplatImg()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _AssetService.GetAssetInfo(EquipId);
            string imgName ="";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (E_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (E_Obj.AssetImages != null )
                    {
                        if (E_Obj.AssetImages.NamePlateImge != null)
                        {
                            var NPimgs = E_Obj.AssetImages.NamePlateImge;
                            if (NPimgs.Length > 0)
                            {
                                imgName = EquipId + "NP-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = EquipId + "NP-" + 1 + ".jpg";

                            var imgList = E_Obj.AssetImages.NamePlateImge.ToList();
                            imgList.Add(apiurl + imgName);
                            E_Obj.AssetImages.NamePlateImge = imgList.ToArray();
                        }
                        else
                        {
                            imgName = EquipId + "NP-" + 1 + ".jpg";
                            E_Obj.AssetImages.NamePlateImge = new string[] { apiurl + imgName };

                        }

                    }
                    else
                    {
                        imgName = EquipId + "NP-" + 1 + ".jpg";
                        //E_Obj.AssetImages = new Prosol_AssetImages();

                        E_Obj.AssetImages.NamePlateImge = new string[] { apiurl+imgName };

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
                    _AssetService.insertImages(E_Obj);
                  

                    return this.Json("Success: "+imgName+" Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: "+EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadnameplatImg2()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _AssetService.GetAssetInfo(EquipId);
            string imgName ="";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (E_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (E_Obj.AssetImages != null )
                    {
                        if (E_Obj.AssetImages.NamePlateImgeTwo != null)
                        {
                            var NPimgs = E_Obj.AssetImages.NamePlateImgeTwo;
                            if (NPimgs.Length > 0)
                            {
                                imgName = EquipId + "NP2-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = EquipId + "NP2-" + 1 + ".jpg";

                            var imgList = E_Obj.AssetImages.NamePlateImgeTwo.ToList();
                            imgList.Add(apiurl + imgName);
                            E_Obj.AssetImages.NamePlateImgeTwo = imgList.ToArray();
                        }
                        else
                        {
                            imgName = EquipId + "NP2-" + 1 + ".jpg";
                            E_Obj.AssetImages.NamePlateImgeTwo = new string[] { apiurl + imgName };

                        }

                    }
                    else
                    {
                        imgName = EquipId + "NP2-" + 1 + ".jpg";
                        //E_Obj.AssetImages = new Prosol_AssetImages();

                        E_Obj.AssetImages.NamePlateImgeTwo = new string[] { apiurl+imgName };

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
                    _AssetService.insertImages(E_Obj);
                  

                    return this.Json("Success: "+imgName+" Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: "+EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
       
        public JsonResult DeleteNPImg(string FileName, string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _AssetService.GetAssetInfo(Uid);
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.NamePlateImge != null)
                        {
                            var NPimgs = E_Obj.AssetImages.NamePlateImge;
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
                                E_Obj.AssetImages.NamePlateImge = strLst.ToArray();
                                _AssetService.insertImages(E_Obj);
                            }
                        }

                    }
                    //WebRequest request = WebRequest.Create(delURL);
                    //request.Method = "DELETE";
                    //request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
       
        public JsonResult DeleteNPImg2(string FileName, string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _AssetService.GetAssetInfo(Uid);
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.NamePlateImgeTwo != null)
                        {
                            var NPimgs = E_Obj.AssetImages.NamePlateImgeTwo;
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
                                E_Obj.AssetImages.NamePlateImgeTwo = strLst.ToArray();
                                _AssetService.insertImages(E_Obj);
                            }
                        }

                    }
                    //WebRequest request = WebRequest.Create(delURL);
                    //request.Method = "DELETE";
                    //request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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

        //asset images
        public JsonResult getAssetImg()
        {
            string folderPath = Server.MapPath("~/Content/images/AssetImgs/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        [Authorize]
        public JsonResult UploadMaximoAssetImg()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _AssetService.GetAssetInfo(EquipId);
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
                                imgName = E_Obj.AssetNo +"_"+ EquipId + "MAXI-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = E_Obj.AssetNo + "_" + EquipId + "MAXI-" + 1 + ".jpg";

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
                        imgName = E_Obj.AssetNo + "_" + EquipId + "MAXI-" + 1 + ".jpg";
                        E_Obj.AssetImages = new AssetImage();

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
                    _AssetService.insertImages(E_Obj);


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
            var E_Obj = _AssetService.GetAssetInfo(EquipId);
            string imgName = "";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
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
                                imgName = E_Obj.AssetNo +"_"+ EquipId + "AI-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = E_Obj.AssetNo + "_" + EquipId + "AI-" + 1 + ".jpg";

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
                        imgName = E_Obj.AssetNo + "_" + EquipId + "AI-" + 1 + ".jpg";
                        E_Obj.AssetImages = new AssetImage();

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
                    _AssetService.insertImages(E_Obj);


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult DeleteMaximoAssetImg(string FileName,string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _AssetService.GetAssetInfo(Uid);
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
                                _AssetService.insertImages(E_Obj);
                            }
                        }

                    }
                    //WebRequest request = WebRequest.Create(delURL);
                    //request.Method = "DELETE";
                    //request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
        public JsonResult DeleteAssetImg(string FileName,string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _AssetService.GetAssetInfo(Uid);
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
                                _AssetService.insertImages(E_Obj);
                            }
                        }

                    }
                    //WebRequest request = WebRequest.Create(delURL);
                    //request.Method = "DELETE";
                    //request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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

        //OldTag images
        public JsonResult getOldTagImg()
        {
            string folderPath = Server.MapPath("~/Content/images/OldTagImgs/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        [Authorize]
        public JsonResult UploadOldTagImg()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _AssetService.GetAssetInfo(EquipId);
            string imgName = "";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (E_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.OldTagImage != null)
                        {
                            var NPimgs = E_Obj.AssetImages.OldTagImage;
                            if (NPimgs.Length > 0)
                            {
                                imgName = EquipId + "OT-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = EquipId + "OT-" + 1 + ".jpg";

                            var imgList = E_Obj.AssetImages.OldTagImage.ToList();
                            imgList.Add(apiurl + imgName);
                            E_Obj.AssetImages.OldTagImage = imgList.ToArray();
                        }
                        else
                        {
                            imgName = EquipId + "OT-" + 1 + ".jpg";                         

                            E_Obj.AssetImages.OldTagImage = new string[] { apiurl + imgName };

                        }

                    }
                    else
                    {
                        imgName = EquipId + "OT-" + 1 + ".jpg";
                        //E_Obj.AssetImages = new Prosol_AssetImages();

                        E_Obj.AssetImages.OldTagImage = new string[] { apiurl + imgName };

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
                    _AssetService.insertImages(E_Obj);


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult DeleteOldTagImg(string FileName, string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _AssetService.GetAssetInfo(Uid);
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.OldTagImage != null)
                        {
                            var NPimgs = E_Obj.AssetImages.OldTagImage;
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
                                E_Obj.AssetImages.OldTagImage = strLst.ToArray();
                                _AssetService.insertImages(E_Obj);
                            }
                        }

                    }
                    //WebRequest request = WebRequest.Create(delURL);
                    //request.Method = "DELETE";
                    //request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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

        //NewTag images
        public JsonResult getNewTagImg()
        {
            string folderPath = Server.MapPath("~/Content/images/NewTagImgs/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        [Authorize]
        public JsonResult UploadNewTagImg()
        {
            var EquipId = Request.Form["UniqueId"];
            var E_Obj = _AssetService.GetAssetInfo(EquipId);
            string imgName = "";
            string apiurl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (E_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.NewTagImage != null)
                        {
                            var NPimgs = E_Obj.AssetImages.NewTagImage;
                            if (NPimgs.Length > 0)
                            {
                                imgName = EquipId + "NT-" + (NPimgs.Length + 1) + ".jpg";
                            }
                            else
                                imgName = EquipId + "NT-" + 1 + ".jpg";

                            var imgList = E_Obj.AssetImages.NewTagImage.ToList();
                            imgList.Add(apiurl + imgName);
                            E_Obj.AssetImages.NewTagImage = imgList.ToArray();
                        }
                        else
                        {
                            imgName = EquipId + "NT-" + 1 + ".jpg";

                            E_Obj.AssetImages.NewTagImage = new string[] { apiurl + imgName };

                        }

                    }
                    else
                    {
                        imgName = EquipId + "OT-" + 1 + ".jpg";
                        //E_Obj.AssetImages = new Prosol_AssetImages();

                        E_Obj.AssetImages.NewTagImage = new string[] { apiurl + imgName };

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
                    _AssetService.insertImages(E_Obj);


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + EquipId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult DeleteNewTagImg(string FileName, string Uid)
        {
            if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(Uid))
            {
                var inxd = FileName.LastIndexOf('/');
                string fName = FileName.Substring(inxd);

                string delURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/" + fName;


                try
                {
                    var E_Obj = _AssetService.GetAssetInfo(Uid);
                    if (E_Obj.AssetImages != null)
                    {
                        if (E_Obj.AssetImages.NewTagImage != null)
                        {
                            var NPimgs = E_Obj.AssetImages.NewTagImage;
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
                                E_Obj.AssetImages.NewTagImage = strLst.ToArray();
                                _AssetService.insertImages(E_Obj);
                            }
                        }

                    }
                    //WebRequest request = WebRequest.Create(delURL);
                    //request.Method = "DELETE";
                    //request.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
        //BOM

        [Authorize]
        public JsonResult UploadBomOldTagImg()
        {
            var BomId = Request.Form["UniqueId"];
            var B_Obj = _AssetService.getBomInfo(BomId);
            string imgName = "";
            string apiurl = "https://dwd86npks63nx.cloudfront.net/";
            if (B_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (string.IsNullOrEmpty(B_Obj.OldTagImg))
                    {
                        imgName = BomId + "OT-1.jpg";                     
                        B_Obj.OldTagImg = apiurl + imgName;
                        
                        string PostURL = "https://dgmdr92vab.execute-api.ap-south-1.amazonaws.com/dev/upload";

                        try
                        {
                            int contentLength = file.ContentLength;
                            byte[] data = new byte[contentLength];
                            file.InputStream.Read(data, 0, contentLength);

                            // Prepare web request...
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                            webRequest.Method = "POST";
                            webRequest.Headers.Add("file-name", imgName);
                            webRequest.Headers.Add("x-api-key", "2WdNu6vMl16CXxpHaK2Av9WB3nU1qcRc52K1T35i");
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
                        _AssetService.insertBomImages(B_Obj);
                    }


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + BomId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult UploadBomNamePlateImg()
        {
            var BomId = Request.Form["UniqueId"];
            var B_Obj = _AssetService.getBomInfo(BomId);
            string imgName = "";
            string apiurl = "https://dwd86npks63nx.cloudfront.net/";
            if (B_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (string.IsNullOrEmpty(B_Obj.NamePlateImg))
                    {
                        imgName = BomId + "NP-1.jpg";
                        B_Obj.NamePlateImg = apiurl + imgName;

                        string PostURL = "https://dgmdr92vab.execute-api.ap-south-1.amazonaws.com/dev/upload";

                        try
                        {
                            int contentLength = file.ContentLength;
                            byte[] data = new byte[contentLength];
                            file.InputStream.Read(data, 0, contentLength);

                            // Prepare web request...
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                            webRequest.Method = "POST";
                            webRequest.Headers.Add("file-name", imgName);
                            webRequest.Headers.Add("x-api-key", "2WdNu6vMl16CXxpHaK2Av9WB3nU1qcRc52K1T35i");
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
                        _AssetService.insertBomImages(B_Obj);
                    }


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + BomId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult UploadBomImg()
        {
            var BomId = Request.Form["UniqueId"];
            var B_Obj = _AssetService.getBomInfo(BomId);
            string imgName = "";
            string apiurl = "https://dwd86npks63nx.cloudfront.net/";
            if (B_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (string.IsNullOrEmpty(B_Obj.BOMImg))
                    {
                        imgName = BomId + "BI-1.jpg";
                        B_Obj.BOMImg = apiurl + imgName;

                        string PostURL = "https://dgmdr92vab.execute-api.ap-south-1.amazonaws.com/dev/upload";

                        try
                        {
                            int contentLength = file.ContentLength;
                            byte[] data = new byte[contentLength];
                            file.InputStream.Read(data, 0, contentLength);

                            // Prepare web request...
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                            webRequest.Method = "POST";
                            webRequest.Headers.Add("file-name", imgName);
                            webRequest.Headers.Add("x-api-key", "2WdNu6vMl16CXxpHaK2Av9WB3nU1qcRc52K1T35i");
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
                        _AssetService.insertBomImages(B_Obj);
                    }


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + BomId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult UploadBomBarCodeImg()
        {
            var BomId = Request.Form["UniqueId"];
            var B_Obj = _AssetService.getBomInfo(BomId);
            string imgName = "";
            string apiurl = "https://dwd86npks63nx.cloudfront.net/";
            if (B_Obj != null)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (string.IsNullOrEmpty(B_Obj.BarCodeImg))
                    {
                        imgName = BomId + "BC-1.jpg";
                        B_Obj.BarCodeImg = apiurl + imgName;

                        string PostURL = "https://dgmdr92vab.execute-api.ap-south-1.amazonaws.com/dev/upload";

                        try
                        {
                            int contentLength = file.ContentLength;
                            byte[] data = new byte[contentLength];
                            file.InputStream.Read(data, 0, contentLength);

                            // Prepare web request...
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                            webRequest.Method = "POST";
                            webRequest.Headers.Add("file-name", imgName);
                            webRequest.Headers.Add("x-api-key", "2WdNu6vMl16CXxpHaK2Av9WB3nU1qcRc52K1T35i");
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
                        _AssetService.insertBomImages(B_Obj);
                    }


                    return this.Json("Success: " + imgName + " Uploaded successfully", JsonRequestBehavior.AllowGet);
                }
                return this.Json("Error : Please upload image", JsonRequestBehavior.AllowGet);

            }
            else return this.Json("Error: " + BomId + "Equipment not found", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult AssetRework(string UniqueId, string RevRemarks, string CondRemarks, string ImageRemarks, string AddRemarks)
        {
            bool res = false;
            int sts = 2;
            try
            {
                res = _AssetService.AssetRework(UniqueId, RevRemarks, CondRemarks, ImageRemarks, AddRemarks, sts);
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
        public JsonResult PVRework(string UniqueId, string RevRemarks, string CondRemarks, string ImageRemarks, string AddRemarks)
        {
            bool res = false;
            int sts = 1;
            try
            {
                res = _AssetService.AssetRework(UniqueId, RevRemarks, CondRemarks, ImageRemarks, AddRemarks, sts);
            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult InsertFields()
        {

            var fields = Request.Form["disFields"];
           
            var ListFields = JsonConvert.DeserializeObject<List<Grid_Settings>>(fields);
           
            var flds = new List<Prosol_GridSettings>();
            foreach (var fld in ListFields)
            {
                var grid = new Prosol_GridSettings();
                grid.UserId = Convert.ToString(Session["UserId"]);
                grid.ColName = fld.ColName;
                grid.Display = fld.Display;
                grid.Visible = fld.Visible;

                grid._id = string.IsNullOrEmpty(fld._id) ? new ObjectId() : new ObjectId(fld._id);
                flds.Add(grid);
              
            }

            var mul_resilt = _AssetService.InserGridflds(flds);




            return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);


        }
        [Authorize]
        public JsonResult getFieldsList()
        {
            var Userid = Convert.ToString(Session["UserId"]); 
            var ListFields = _AssetService.getAllFields(Userid);
            var grids= new List<Grid_Settings>();
            foreach (var flds in ListFields)
            {
                var gridflds = new Grid_Settings();
                gridflds._id = flds._id.ToString();
                gridflds.ColName = flds.ColName;
                gridflds.Display = flds.Display;
                gridflds.Visible = flds.Visible;
                grids.Add(gridflds);


            }
            var jsonResult = Json(grids, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult ApproveAll()
        {

            var fields = Request.Form["ApprovedItems"];

            var AppRemark = Request.Form["ApproveRemark"];
            var clfRem = Request.Form["CLFremark"];
           // var CLFRemarkslst = JsonConvert.DeserializeObject<List<CLF_Remarks>>(clfRem);

            var ListFields = JsonConvert.DeserializeObject<List<CatAssetModel>>(fields);

            var flds = new List<Prosol_AssetMaster>();
            //var CLF= new List<CLF_Remark>();

            //if (clfRem != null)
            //{
            //    var cl = new CLF_Remark();
            //    cl.Remark = clfRem;
            //    cl.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    cl.UserId = Convert.ToString(Session["UserId"]);
            //    cl.UserName = Convert.ToString(Session["username"]);
            //    CLF.Add(cl);

            //}
            //if (AppRemark != null)
            //{
            //    var cl = new CLF_Remark();
            //    cl.Remark = AppRemark;
            //    cl.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    cl.UserId = Convert.ToString(Session["UserId"]);
            //    cl.UserName = Convert.ToString(Session["username"]);
            //    CLF.Add(cl);

            //}
            foreach (var fld in ListFields)
            {
                var grid = new Prosol_AssetMaster();

                if (fld.SerialNo == "true")
                {
                    //grid.UniqueId = fld.UniqueId;

                    var rel = new Prosol_UpdatedBy();
                    rel.UserId = Convert.ToString(Session["UserId"]);
                    rel.Name = Convert.ToString(Session["username"]);
                    rel.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    grid.Release = rel;
                    grid.ItemStatus = string.IsNullOrEmpty(clfRem) ? 8 : 9;
                    //if (CLF != null)
                    //{
                        //grid.CLF_Remarks = CLF;
                    //}

                    flds.Add(grid);
                }

            }

            var appcnt = _AssetService.ApproveFAR(flds);
            
            return this.Json(appcnt, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        [Authorize]
        public ActionResult catreloaddata1(string Business, string Major, string Region, string City, string Area, string SubArea)
        {

            var gd_data = _AssetService.catreloaddata1(Business,Major,Region, City, Area, SubArea).ToList();
            var AllMaster = _AssetService.getAllCommonMaster();
            var lstassign = new List<CatAssetModel>();

            if (gd_data.Count > 0)
            {
                foreach (Prosol_AssetMaster assetvalues in gd_data)
                {
                    var catasset = new CatAssetModel();
                    var pvname = new Prosol_UpdatedBy();
                    if (assetvalues.PVuser != null)
                    {
                        pvname.Name = assetvalues.PVuser.Name;
                        catasset.PVuser = pvname;
                    }

                    //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
                    //catasset.Area = Areaname.Count > 0 ? Areaname[0].Area : "";
                    catasset.UniqueId = assetvalues.AssetNo;
                    //catasset.Barcode = assetvalues.Barcode;

                    //var Cityname = AllMaster.Cities.Where(x => x._id.ToString() == assetvalues.City).ToList();
                    //catasset.City = Cityname.Count > 0 ? Cityname[0].City : "";

                    //var Businame = AllMaster.Businesses.Where(x => x._id.ToString() == assetvalues.Business).ToList();
                    //catasset.Business = Businame.Count > 0 ? Businame[0].BusinessName : "";

                    //var EquipClass = AllMaster.EquipmentClasses.Where(x => x._id.ToString() == assetvalues.EquipmentClass).ToList();
                    //catasset.EquipmentClass = EquipClass.Count > 0 ? EquipClass[0].EquipmentClass : "";


                    ////  catasset.EquipmentClass = assetvalues.EquipmentClass;

                    //var EquipType = AllMaster.EquipmentTypes.Where(x => x._id.ToString() == assetvalues.EquipmentType).ToList();
                    //catasset.EquipmentType = EquipType.Count > 0 ? EquipType[0].EquipmentType : "";

                    //catasset.FLOC_Code = assetvalues.FLOC_Code;

                    //var Funcname = AllMaster.MinorClasses.Where(x => x._id.ToString() == assetvalues.MinorClass).ToList();
                    //catasset.MinorClass = Funcname.Count > 0 ? Funcname[0].MinorCode : "";

                    //var majorname = AllMaster.MajorClasses.Where(x => x._id.ToString() == assetvalues.MajorClass).ToList();
                    //catasset.MajorClass = majorname.Count > 0 ? majorname[0].MajorClass : "";

                    ////catasset.MajorClass = assetvalues.MajorClass;

                    //catasset.EquipmentDesc = assetvalues.EquipmentDesc;

                    //var Identiname = AllMaster.Identifiers.Where(x => x._id.ToString() == assetvalues.Identifier).ToList();
                    //catasset.Identifier = Identiname.Count > 0 ? Identiname[0].IdentifierCode : "";

                    //catasset.OldFunLoc = assetvalues.OldFunLoc;
                    //catasset.Quantity = assetvalues.Quantity;

                    //var regname = AllMaster.Regions.Where(x => x._id.ToString() == assetvalues.Region).ToList();
                    //catasset.Region = regname.Count > 0 ? regname[0].Region : "";


                    //catasset.ItemStatus = assetvalues.ItemStatus;

                    //var SubAreaname = AllMaster.SubAreas.Where(x => x._id.ToString() == assetvalues.SubArea).ToList();
                    //catasset.SubArea = SubAreaname.Count > 0 ? SubAreaname[0].SubArea : "";
                    lstassign.Add(catasset);

                }




            }

            var jsonResult = Json(lstassign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }


        //[HttpPost]
        //[Authorize]
        public ActionResult catreloaddataa(string farId)
        {

            var gd_data = _AssetService.catreloaddataa(farId).ToList();
            var lstassign = new List<CatAssetModel>();

            if (gd_data.Count > 0)
            {
                foreach (Prosol_AssetMaster assetvalues in gd_data)
                {
                    var catasset = new CatAssetModel();
                    var pvname = new Prosol_UpdatedBy();
                    if (assetvalues.PVuser != null)
                    {
                        pvname.Name = assetvalues.PVuser.Name;
                        catasset.PVuser = pvname;
                    }

                    //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
                    //catasset.Area = Areaname.Count > 0 ? Areaname[0].Area : "";
                    //catasset.UniqueId = assetvalues.AssetNo;
                    catasset.UniqueId = assetvalues.UniqueId;
                    catasset.AssetNo = assetvalues.AssetNo;
                    catasset.Description = assetvalues.Description;
                    catasset.Parent = assetvalues.Parent;
                    catasset.Status = assetvalues.Status;
                    catasset.SerialNo = assetvalues.SerialNo;
                    catasset.ModelNo = assetvalues.ModelNo;
                    catasset.ModelYear = assetvalues.ModelYear;
                    catasset.Manufacturer = assetvalues.Manufacturer;
                    catasset.Catalogue = assetvalues.Catalogue;
                    lstassign.Add(catasset);

                }




            }

            var jsonResult = Json(lstassign, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }


        public void DownloadAssetAssign()
        {
            var username = Session["Username"].ToString();
            var Role = Session["Role"].ToString();
            var datalist = _AssetService.PV_Reassign(username, Role).ToList();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            foreach (var cde in datalist)
            {

                row = new Dictionary<string, object>();

                row.Add("Item ID", cde.UniqueId);


                if (!string.IsNullOrEmpty(cde.AssetNo))
                    row.Add("Asset Number", cde.AssetNo);
                else
                    row.Add("Asset Number", "");

                if (!string.IsNullOrEmpty(cde.Description))
                    row.Add("Description", cde.Description);
                else
                    row.Add("Description", "");

                if (!string.IsNullOrEmpty(cde.PVuser.Name))
                    row.Add("Username", cde.PVuser.Name);
                else
                    row.Add("Username", "");

                rows.Add(row);

            }

            var strJson = JsonConvert.SerializeObject(rows);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));




            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "tab1");
            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"AssignWorks.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();


        }


        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult GenerateShortLong()
        {
            var cat1 = Request.Form["cat"];
            var cat = JsonConvert.DeserializeObject<CatAssetModel>(cat1);
            var Charas = Request.Form["attri"];
            var ListCharas = JsonConvert.DeserializeObject<List<AssetAttributes>>(Charas);




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
            var lstCharateristics = new List<Asset_AttributeList>();
            if (ListCharas != null)
            {

                foreach (AssetAttributes LstAtt in ListCharas)
                {
                    var AttrMdl = new Asset_AttributeList();
                    AttrMdl.Characteristic = LstAtt.Characteristic;
                    AttrMdl.Value = LstAtt.Value;
                    AttrMdl.UOM = LstAtt.UOM;
                    AttrMdl.Squence = LstAtt.Squence;
                    AttrMdl.ShortSquence = LstAtt.ShortSquence;
                    AttrMdl.Source = LstAtt.Source;
                    lstCharateristics.Add(AttrMdl);

                }
            }
            var proCat = new Prosol_AssetMaster();
            proCat._id = cat._id != null ? new MongoDB.Bson.ObjectId(cat._id) : new MongoDB.Bson.ObjectId();
            proCat.UniqueId = cat.UniqueId != null ? cat.UniqueId : "";
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            proCat.Description = cat.Description;
            proCat.Characteristics = lstCharateristics;
            proCat.ModelNo = cat.ModelNo;
            proCat.PartNo = cat.PartNo;
            proCat.Manufacturer = cat.Manufacturer;
            proCat.SerialNo = cat.SerialNo;
            proCat.MfrYear = cat.MfrYear;
            proCat.MfrCountry = cat.MfrCountry;
            proCat.AdditionalInfo = cat.AdditionalInfo;
            proCat.TechIdentNo = cat.TechIdentNo;
            //new 2



            var res = _AssetService.GenerateShortLong(proCat);
            //newcode
            var thi = res[1];
            res[4] = RepeatedValue(thi);
            var jsonResult = Json(res, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        // RepeatedValue
        private string RepeatedValue(string cat)
        {
            //BEARING,BALL,TYPE: SELF - ALIGNING,DESIGNATION: 2311 K,INSIDE DIAMETER:55 MM,OUTSIDE DIAMETER:120 MM,


            string constr = "", tmpStr = "";
            string[] tmpArr = cat.Split(',');
            int inc = 0;
            foreach (string str1 in tmpArr)
            {
                if (str1.Contains(':'))
                {
                    string[] strplt = str1.Split(':');
                    tmpArr[inc] = strplt[1];
                }
                if (inc < (tmpArr.Length - 1))
                    tmpStr = tmpStr + tmpArr[inc] + ",";

                inc++;
            }

            string[] repp = tmpStr.Split(' ', ',', ';');
            foreach (string str in repp)
            {

                int flg = 0;
                foreach (string str1 in repp)
                {
                    if (str == str1)
                    {
                        if (flg == 1)
                        {
                            string[] forCheck = constr.Split(',');
                            if (!forCheck.Contains(str))
                                constr = constr + str + ",";
                            //  constr = constr.TrimStart(',').TrimEnd(':');
                        }
                        flg = 1;
                    }

                }
            }

            return constr;
        }

        [Authorize]
        public JsonResult GetAttachment(string itemcode)
        {
            var LstAttch = _CatalogueService.GetAttachment(itemcode);
            var attachMdlList = new List<AttachmentModel>();
            foreach (Prosol_Attachment atm in LstAttch)
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


        //Dashboard



        public JsonResult getAllOpenReqs()
        {
            List<string> reqLst = new List<string> { "pending"/*, "approved"*/ };
            List<int> masLst = new List<int> { 0, -1, -2, -3, 1, 2, 3, 4, 5, 6 };
            //var reqItms = _AssetService.getReqItems();
            var masItms = _AssetService.getMasterItems();
            var result = new List<object>();
            var req = "";
            var itmsCount = "";
            var purgAndItmsCount = new
            {
                req = "",
                itmsCount = ""
            };
            //foreach (var re in reqLst)
            //{
            //    var filteredItems = reqItms.Where(item => item.itemStatus == re);
            //    itmsCount = filteredItems.Count().ToString();
            //    if (re == "pending")
            //    {
            //        req = "In Approval";
            //    }
            //    //else if (re == "L1-Approved")
            //    //{
            //    //    req = "In Approval";
            //    //}
            //    //else if (re == "L1-Approved")
            //    //{
            //    //    req = "In Approval";
            //    //}
            //    //else if (re == "L2-Approved")
            //    //{
            //    //    req = "Cataloguing";
            //    //}

            //    purgAndItmsCount = new
            //    {
            //        req,
            //        itmsCount
            //    };

            //    result.Add(purgAndItmsCount);
            //}
            foreach (var mas in masLst)
            {
                var filteredItems = masItms.Where(item => item.ItemStatus == mas);
                itmsCount = filteredItems.Count().ToString();
                if (mas == 0)
                {
                    req = "Pending";
                }
                else if (mas == 1)
                {
                    req = "In PV";
                }
                else if (mas == 2 || mas == 3)
                {
                    req = "In Catalogue";
                }
                else if (mas == 4 || mas == 5)
                {
                    req = "In QC";
                }
                else if ( mas == 6)
                {
                    req = "Released";
                }

                purgAndItmsCount = new
                {
                    req,
                    itmsCount
                };

                result.Add(purgAndItmsCount);
            }
            //var skuItms = _AssetService.getSKUItems();
            //req = "Total";
            //itmsCount = skuItms.Count().ToString();
            purgAndItmsCount = new
            {
                req,
                itmsCount
            };
            result.Add(purgAndItmsCount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult getTotalReqs(int tyear)
        //{
        //    List<string> monthLst = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        //    //List<string> reqLst = new List<strin  g> { "Replenishment", "NonReplenishment" };
        //    var reqItms = _AssetService.getReqItems();
        //    var data = new List<object>();
        //    var yearsList = new List<object>();
        //    var rep = "";
        //    var month = "";
        //    var year = "";
        //    var nonrep = 0;
        //    float avg = 0;
        //    foreach (var mth in monthLst)
        //    {
        //        var yearlyItems = reqItms.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Year == tyear);
        //        var filteredItems = yearlyItems.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Month == monthLst.IndexOf(month) + 1);
        //        month = mth;
        //        rep = filteredItems.Count().ToString();
        //        var purgAndItmsCount = new
        //        {
        //            rep,
        //            //avg,
        //            month
        //        };

        //        data.Add(purgAndItmsCount);
        //        //}

        //    };

        //    foreach (var req in reqItms)
        //    {
        //        if (req.CreatedOn != null)
        //        {
        //            year = req.CreatedOn.Value.Year.ToString();
        //        }
        //        var years = new
        //        {
        //            year
        //        };
        //        yearsList.Add(years);
        //    }
        //    var result = new { data, yearsList };
        //    //result.Add(purgAndItmsCount);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetPendItems(int year)
        {
            List<string> monthLst = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            //var reqItms = _DashboardService.getReqItems();
            var masItms = _AssetService.getMasterItems();
            var data = new List<object>();
            var yearsList = new List<object>();
            var fyear = "";

            foreach (var month in monthLst)
            {
                //var yearlyItems = totalItms.Where(item => item.requestedOn.HasValue && item.requestedOn.Value.Year == year);
                //var filteredItems = yearlyItems.Where(item => item.requestedOn.HasValue && item.requestedOn.Value.Month == monthLst.IndexOf(month) + 1);
                //var itmsCount = filteredItems.Count().ToString();
                //var yearlyReqItems = reqItms.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Year == year);
                var yearlyMasItems = masItms.Where(item => item.User.UpdatedOn.HasValue && item.User.UpdatedOn.Value.Year == year);
                //var filteredReqItems = yearlyReqItems.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Month == monthLst.IndexOf(month) + 1);
                var filteredMasItems = yearlyMasItems.Where(item => item.User.UpdatedOn.HasValue && item.User.UpdatedOn.Value.Month == monthLst.IndexOf(month) + 1);
                //var l1 = filteredReqItems.Where(item => item.ItemStatus == 0).Count();
                //var l2 = filteredReqItems.Where(item => item.itemStatus == "approved").Count();
                var pen = filteredMasItems.Count(item => item.ItemStatus == 0);
                var app = filteredMasItems.Count(item => item.ItemStatus == 1 || item.ItemStatus == 2);
                var fin = filteredMasItems.Count(item => item.ItemStatus == 3 || item.ItemStatus == 4);
                var clf = filteredMasItems.Count(item => item.ItemStatus == -1 || item.ItemStatus == -2 || item.ItemStatus == -3);
                var qc = filteredMasItems.Count(item => item.ItemStatus == 5 || item.ItemStatus == 6);

                var itmsCount = /*l1 + l2 +*/ fin + qc + pen + app + clf;

                var purgAndItmsCount = new
                {
                    fyear,
                    month,
                    itmsCount
                };

                data.Add(purgAndItmsCount);
            }
            foreach (var req in masItms)
            {
                if (req.User.UpdatedOn != null)
                {
                    fyear = req.User.UpdatedOn.Value.Year.ToString();
                }
                var years = new
                {
                    fyear
                };
                yearsList.Add(years);
            }
            var result = new { data, yearsList };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTotalReqs(int tyear)
        {
            List<string> monthLst = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            //List<string> reqLst = new List<string> { "Replenishment", "NonReplenishment" };
            var reqItms = _AssetService.getReqItems();
            var data = new List<object>();
            var yearsList = new List<object>();
            var rep = "";
            var month = "";
            var year = "";
            var nonrep = 0;
            float avg = 0;
            foreach (var mth in monthLst)
            {
                var yearlyItems = reqItms.Where(item => item.User.UpdatedOn.HasValue && item.User.UpdatedOn.Value.Year == tyear);
                var filteredItems = yearlyItems.Where(item => item.User.UpdatedOn.HasValue && item.User.UpdatedOn.Value.Month == monthLst.IndexOf(month) + 1);
                month = mth;
                rep = filteredItems.Count().ToString();
                var purgAndItmsCount = new
                {
                    rep,
                    //avg,
                    month
                };

                data.Add(purgAndItmsCount);
                //}

            };

            foreach (var req in reqItms)
            {
                if (req.User.UpdatedOn != null)
                {
                    year = req.User.UpdatedOn.Value.Year.ToString();
                }
                var years = new
                {
                    year
                };
                yearsList.Add(years);
            }
            var result = new { data, yearsList };
            //result.Add(purgAndItmsCount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult ShortLong()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkShortLong(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]

        public JsonResult BulkShortLong()
        {
            int res = 0;
            res = _AssetService.BulkAutoShortLong();
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public void Catdownload(string id)
        {
            var usr = Convert.ToString(Session["username"]);
            var res1 = _AssetService.Downloaddata(usr, id).ToList();
            DataTable dt = new DataTable();
            foreach (IDictionary<string, object> row in res1)
            {
                var rw = dt.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt.Rows.Add(rw);
            }
            DataTable dt1 = new DataTable();
            if (id != "All")
            {
                var res2 = _AssetService.Downloadvendordata(Convert.ToString(Session["username"]), id).ToList();
                foreach (IDictionary<string, object> row in res2)
                {
                    var rw = dt1.NewRow();
                    foreach (KeyValuePair<string, object> entry in row)
                    {
                        if (!dt1.Columns.Contains(entry.Key.ToString()))
                        {
                            dt1.Columns.Add(entry.Key);
                        }
                        rw[entry.Key] = entry.Value;
                    }
                    dt1.Rows.Add(rw);
                }
            }
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "ITEM");
            if (id != "All")
            {
                wbook.Worksheets.Add(dt1, "MANUFACTURER");
            }
            string fileName = "Items";

            //if (id == "cat")
            //{
            //    fileName = "Cat_Items";
            //}
            //else if (id == "qc")
            //{
            //    fileName = "QC_Items";
            //}
            //if (id != "QA")
            //{
            //  Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");
            httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}.xlsx\"");
            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();

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
                    res = _AssetService.BulkCatData(file);
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
                    res = _AssetService.BulkQcData(file);
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


        //Asset Bulk Data upload

        [Authorize]
        public JsonResult AssetBulkdata_Upload()
        {
            //try
            //{
            string res = "";
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.AssetBulkData(file);
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

        public JsonResult Rework()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkRework(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult BulkDashboard()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkDashboard(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult BulkSwap()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkSwap(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult MfrBulkUpload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.MfrBulkUpload(file);
                }
            }
                return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //[Authorize]

        //public JsonResult BulkLocation()
        //{
        //    int res = 0;
        //    var file = Request.Files.Count > 0 ? Request.Files[0] : null;
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
        //        {
        //            res = _AssetService.BulkLocation(file);
        //        }
        //    }
        //    return this.Json(res, JsonRequestBehavior.AllowGet);
        //}
        [Authorize]

        public JsonResult BulkParent()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkParent(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult BulkObject()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkObject(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult BulkUNSPSC()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkUNSPSC(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult BulkAssetNo()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkAssetNo(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkCost()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkCost(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkDiscipline()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkDiscipline(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkWorkC()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkWorkC(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkAdditional()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkAdditional(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkURL()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkURL(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkTag()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkTag(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkLegacy()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _AssetService.BulkLegacy(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public JsonResult GetDashboard()
        {
            var res = new List<Prosol_Dashboard>();
            res = _AssetService.getDashboard().ToList();
            //if(mdl != null)
            //{
            //    var obj = new Prosol_Dashboard();
            //    obj.Cluster = mdl.C
            //}
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        //QR Generation

        //OLD
        //[HttpPost]
        //public JsonResult GenerateQRCode(QRRequestModel request)
        //{
        //    string AssetNo = request.AssetNo;
        //    string uniqueId = request.UniqueId;
        //    string fileName = request.fileName;
        //    string TagNo = "";
        //    int UpdatedRunNo = _AssetService.GetLastRunning("VT") + 1;
        //    TagNo = "3" + UpdatedRunNo.ToString("D7");

        //    //if (string.IsNullOrWhiteSpace(TagNo))
        //    //{
        //    //    TagNo = "Default QR Code Text";
        //    //}

        //    //string fileName = $"QRCode_{Guid.NewGuid()}.png";
        //    if (string.IsNullOrEmpty(fileName))
        //        fileName = AssetNo + "_VT_" + DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString("yyyyMMddHHmmss") + ".png";
        //    string folderPath = Server.MapPath("~/QRImages/");
        //    string fullPath = Path.Combine(folderPath, fileName);

        //    var E_Obj = _AssetService.GetAssetInfo(AssetNo);

        //    if (E_Obj.AssetImages != null)
        //    {
        //        if (E_Obj.AssetImages.VirtualTagImage == null || E_Obj.AssetImages.VirtualTagImage.Length == 0)
        //        {
        //            E_Obj.AssetImages.VirtualTagImage = new string[] { fileName };
        //        }
        //        else
        //        {
        //            E_Obj.AssetImages.VirtualTagImage[0] = fileName;
        //        }
        //    }
        //    _AssetService.insertImages(E_Obj);
        //    _AssetService.UpdateRunning("VT", UpdatedRunNo);

        //    try
        //    {
        //        if (!Directory.Exists(folderPath))
        //            Directory.CreateDirectory(folderPath);

        //        using (var qrGenerator = new QRCodeGenerator())
        //        using (var qrCodeData = qrGenerator.CreateQrCode(TagNo, QRCodeGenerator.ECCLevel.Q))
        //        using (var qrCode = new QRCode(qrCodeData))
        //        using (var qrImage = qrCode.GetGraphic(20))
        //        {
        //            string logoPath = Server.MapPath("~/Images/Ad-logo.png");
        //            Bitmap originalLogo = new Bitmap(logoPath);

        //            int finalSize = 400;
        //            int padding = 1;

        //            int headerHeight = 80;
        //            Bitmap resizedLogo = new Bitmap(finalSize - 2 * padding, headerHeight);
        //            using (Graphics gLogo = Graphics.FromImage(resizedLogo))
        //            {
        //                gLogo.Clear(Color.White);
        //                gLogo.DrawImage(originalLogo, 0, 0, resizedLogo.Width, resizedLogo.Height);
        //            }

        //            int qrSize = 250;
        //            Bitmap resizedQRImage = new Bitmap(qrSize, qrSize);
        //            using (Graphics gQR = Graphics.FromImage(resizedQRImage))
        //            {
        //                gQR.Clear(Color.White);
        //                gQR.DrawImage(qrImage, 0, 0, qrSize, qrSize);
        //            }

        //            int footerHeight = 50;

        //            using (Bitmap finalImage = new Bitmap(finalSize, finalSize))
        //            using (Graphics g = Graphics.FromImage(finalImage))
        //            {
        //                g.Clear(Color.White);

        //                int headerX = padding;
        //                int headerY = padding;
        //                g.DrawImage(resizedLogo, headerX, headerY);

        //                int qrX = (finalSize - qrSize) / 2;
        //                int qrY = headerY + resizedLogo.Height + padding;
        //                g.DrawImage(resizedQRImage, qrX, qrY);

        //                // Footer text
        //                int footerY = qrY + qrSize + padding;
        //                using (Font font = new Font("Arial", 36, FontStyle.Bold))
        //                //using (Font font = new Font("Arial", 20))
        //                using (Brush brush = new SolidBrush(Color.Black))
        //                using (StringFormat format = new StringFormat()
        //                {
        //                    Alignment = StringAlignment.Center,
        //                    LineAlignment = StringAlignment.Center
        //                })
        //                {
        //                    RectangleF footerRect = new RectangleF(0, footerY, finalSize, footerHeight);
        //                    g.DrawString(TagNo, font, brush, footerRect, format);
        //                }

        //                finalImage.Save(fullPath, ImageFormat.Png);
        //            }

        //            originalLogo.Dispose();
        //            resizedLogo.Dispose();
        //            resizedQRImage.Dispose();
        //        }
        //        return Json(new
        //        {
        //            success = true,
        //            imageUrl = Url.Content("~/QRImages/" + fileName)
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            error = ex.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //NEW
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GenerateQRCode(QRRequestModel request)
        {
            try
            {
                string tagNo;
                string fileName = _AssetService.GenerateQRCode(request, out tagNo);

                return Json(new
                {
                    success = true,
                    tagNo,
                    imageUrl = Url.Content("~/QRImages/" + fileName)
                });
            }
            catch (Exception ex)
            {
                // Log exception here
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }


        //OLD
        //[Authorize]
        //public JsonResult BulkQR()
        //{
        //    var res = new List<object>();
        //    var AssetNo = "";
        //    var TagNo = "";
        //    var file = Request.Files.Count > 0 ? Request.Files[0] : null;
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        if (file != null && file.FileName.EndsWith(".xlsx"))
        //        {
        //            using (var package = new ExcelPackage(file.InputStream))
        //            {
        //                var assetLst = _AssetService.AssetQRCheck(file);
        //                foreach (var drw in assetLst)
        //                {
        //                    AssetNo = drw.AssetNo;
        //                    TagNo = drw.TagNo;
        //                    string fileName = AssetNo + "_VT_" + DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString("yyyyMMddHHmmss") + ".png";
        //                    var req = new QRRequestModel();
        //                    req.AssetNo = AssetNo;
        //                    req.fileName = fileName;
        //                    GenerateQRCode(req);
        //                    var lst = new
        //                    {
        //                        assetNo = AssetNo,
        //                        tagNo = TagNo,
        //                        tagURL = fileName
        //                    };
        //                    if( !string.IsNullOrEmpty(lst.assetNo) )
        //                    res.Add(lst);
        //                }
        //            }
        //        }

        //    }
        //    return this.Json(res, JsonRequestBehavior.AllowGet);
        //}

        //NEW
        [HttpPost]
        [Authorize]
        public JsonResult BulkQR()
        {
            var resultList = new List<object>();
            string logPath = Server.MapPath("~/Logs/BulkQRError.txt");

            try
            {
                if (Request.Files.Count == 0)
                {
                    return Json(new { success = false, message = "No file uploaded." });
                }

                var file = Request.Files[0];

                if (file == null || file.ContentLength == 0)
                {
                    return Json(new { success = false, message = "Uploaded file is empty." });
                }

                if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = false, message = "Only .xlsx files are allowed." });
                }

                // AssetQRCheck may throw, so wrap it in try-catch
                IEnumerable<QRObj> assetList;
                try
                {
                    assetList = _AssetService.AssetQRCheck(file);
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(logPath, "AssetQRCheck Failed:\n" + ex.ToString());
                    return Json(new { success = false, message = "Error processing Excel file: " + ex.Message });
                }

                foreach (var drw in assetList)
                {
                    try
                    {
                        string assetNo = drw.AssetNo;
                        string tagNo;

                        var request = new QRRequestModel
                        {
                            AssetNo = assetNo,
                            fileName = assetNo + "_VT_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".png"
                        };

                        string generatedFileName = _AssetService.GenerateQRCode(request, out tagNo);

                        resultList.Add(new
                        {
                            assetNo = assetNo,
                            tagNo = tagNo,
                            tagURL = generatedFileName
                        });
                    }
                    catch (Exception ex)
                    {
                        resultList.Add(new
                        {
                            assetNo = drw.AssetNo,
                            error = "Failed to generate QR: " + ex.Message
                        });

                        // Optionally log each item-level failure
                        System.IO.File.AppendAllText(logPath, $"Asset: {drw.AssetNo} failed. Error: {ex}\n");
                    }
                }

                return Json(new
                {
                    success = true,
                    data = resultList
                });
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(logPath, "BulkQR Global Error:\n" + ex.ToString());

                return Json(new
                {
                    success = false,
                    message = "Bulk QR processing failed: " + ex.Message
                });
            }
        }


        public JsonResult GetDataList(string label)
        {
            var lst = _AssetService.GetDataList(label);
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllDataList()
        {
            var lst = _AssetService.GetDataList();
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public void Mfrdownload(string id)
        {
            var res1 = _AssetService.DownloadMFR(id).ToList();
            DataTable dt = new DataTable();
            foreach (IDictionary<string, object> row in res1)
            {
                var rw = dt.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt.Rows.Add(rw);
            }
            var fileName = id;
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, id);
            //  Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");
            httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}.xlsx\"");
            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();
        }
        public JsonResult GetAssetValues(string Noun, string Modifier, string Attribute)
        {


            var arrStr = _AssetService.GetAssetValues(Noun, Modifier, Attribute);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);



        }
        public JsonResult AddAssetValue(string Noun, string Modifier, string Attribute, string Value,string abb)
        {
            string user = Session["UserName"].ToString();
            string role = Session["Role"].ToString();

            var arrStr = _AssetService.AddValue(Noun, Modifier, Attribute, Value, abb, user, role);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);



        }
        public JsonResult GetUsersCounts(string role)
        {

            var result = new List<UsersResult>();
            var dataList = _AssetService.BindAll().ToList();
            var usersList = _AssetService.BindUsersByRole(role).ToList();

            foreach(var user in usersList)
            {
                var obj = new UsersResult();

                obj.Name = user.UserName;

                List<Prosol_AssetMaster> filterData;
                string pendCount = "";
                string savedCount = "";
                string allCount = "";

                switch (role)
                {
                    case "Reviewer":
                        filterData = dataList.Where(d => d.Review != null && d.Review.UserId == user.Userid).ToList();
                        pendCount = filterData.Where(p => p.ItemStatus == 4).ToList().Count().ToString();
                        savedCount = filterData.Where(c => c.ItemStatus == 5).ToList().Count().ToString();
                        allCount = (Convert.ToInt32(pendCount) + Convert.ToInt32(savedCount)).ToString();
                        break;
                    case "Cataloguer":
                        filterData = dataList.Where(d => d.Catalogue != null && d.Catalogue.UserId == user.Userid).ToList();
                        pendCount = filterData.Where(p => p.ItemStatus == 2).ToList().Count().ToString();
                        savedCount = filterData.Where(c => c.ItemStatus == 3).ToList().Count().ToString();
                        allCount = (Convert.ToInt32(pendCount) + Convert.ToInt32(savedCount)).ToString();
                        break;
                    case "PV User":
                        filterData = dataList.Where(d => d.PVuser != null && d.PVuser.UserId == user.Userid).ToList();
                        pendCount = filterData.Where(p => p.ItemStatus == 1).ToList().Count().ToString();
                        savedCount = "-";
                        allCount = pendCount;
                        break;
                    default:
                        filterData = new List<Prosol_AssetMaster>(); 
                        break;
                }

                obj.oPending = pendCount;
                obj.oCompleted = savedCount;
                obj.Overall = allCount;

                result.Add(obj);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetCounts(string role)
        //{

        //    var titleList = new List<string> { "MAXIMO ASSETS", "PV COMPLETED", "PV YET TO COMPLETE", "PV COMPLETED - NEW ASSETS", "TOTAL PV COMPLETED", "OVERALL ASSETS", "UNIQUE ASSETS", "SUBMITTED ASSET" };

        //    var result = new List<Overall>();
        //    var dataList = _AssetService.BindAll().ToList();
        //    var usersList = _AssetService.BindUsersByRole(role).ToList();

        //    foreach(var title in titleList)
        //    {
        //        var obj = new Overall();

        //        obj.Title = title;

        //        List<Prosol_AssetMaster> filterData;
        //        string count = "";

        //        switch (title)
        //        {
        //            case "Reviewer":
        //                filterData = dataList.Where(d => d.Review != null && d.Review.UserId == user.Userid).ToList();
        //                count = filterData.Where(p => p.ItemStatus == 4).ToList().Count().ToString();
        //                break;
        //            case "Cataloguer":
        //                filterData = dataList.Where(d => d.Catalogue != null && d.Catalogue.UserId == user.Userid).ToList();
        //                break;
        //            case "PV User":
        //                filterData = dataList.Where(d => d.PVuser != null && d.PVuser.UserId == user.Userid).ToList();
        //                break;
        //            default:
        //                filterData = new List<Prosol_AssetMaster>(); 
        //                break;
        //        }

        //        obj.oPending = pendCount;
        //        obj.oCompleted = savedCount;
        //        obj.Overall = allCount;

        //        result.Add(obj);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        [Authorize]
        public JsonResult Deletefile(string uniqueId,string fileName)
        {
            var res = _AssetService.Deletefile(uniqueId, fileName);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

    }

    public class UsersResult
    {
        public string Name { get; set; }
        public string Overall { get; set; }
        public string oCompleted { get; set; }
        public string oPending { get; set; }
    }
    public class Overall
    {
        public string Title { get; set; }
        public string Count { get; set; }
    }
}