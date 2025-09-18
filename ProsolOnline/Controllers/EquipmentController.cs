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
using System.Globalization;

namespace ProsolOnline.Controllers
{
    [ValidateInput(false)]
    public class EquipmentController : Controller
    {
        private readonly IEquipment _CatalogueService;
        private readonly ICharateristics _CharateristicService;
        private readonly IUserCreate _UserCreateService;
        private readonly INounModifier _Nounmodifier;
        private readonly ILogic _LogicService;


        public EquipmentController(ICharateristics CharaService,
            IEquipment catalogueService, IUserCreate UserCreateService, INounModifier nounmodifier, ILogic LogicService)
        {
            _CharateristicService = CharaService;
            _CatalogueService = catalogueService;
            _UserCreateService = UserCreateService;
            _Nounmodifier = nounmodifier;
            _LogicService = LogicService;

        }

        public JsonResult searchmbccode()
        {
            if (Session["mbccode"] != null && Session["mbccode"].ToString() != "")
            {
                string stree = Session["mbccode"].ToString();
                Session["mbccode"] = null;
                return Json(stree, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
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
        public ActionResult Equipments()
        {
            if (CheckAccess("New Equipment") == 1)
                return View();
            else if (CheckAccess("New Equipment") == 0)
                return View("Denied");
            else return View("Login");

        }

        [Authorize]
        public ActionResult SrchEquipment()
        {
            if (CheckAccess("Search") == 1)
                return View();
            else if (CheckAccess("Search") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult EquipmentDictionary()
        {
            if (CheckAccess("Search") == 1)
                return View("EquipmentDictionary");
            else if (CheckAccess("Search") == 0)
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
            var result = uomList.Select(i => new { i.Code, i.Name, i.Name2, i.Name3, i.Name4, i.AcquiredCompanyName, i.Acquiredby }).Distinct().ToList();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult InsertData()
        {

            var files = Request.Files.Count > 0 ? Request.Files : null;
            var itemsts = Request.Form["itemsts"];
            var pvsts = Request.Form["PVStatus"];

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
            //var HSNID1 = Request.Form["HSNID"];

            // string HSNID = JsonConvert.DeserializeObject<string>(HSNID1);
            string HSNID = "";
            HSNID = JsonConvert.DeserializeObject<string>(Request.Form["HSNID"] != null ? Request.Form["HSNID"] : "");
            // var Desc1 = Request.Form["Desc"];
            // string Desc = JsonConvert.DeserializeObject<string>(Desc1);
            //  string Desc = "";
            //  Desc = JsonConvert.DeserializeObject<string>(Request.Form["Desc"] != null ? Request.Form["Desc"] : "");

            int res = 0;
            try
            {
                //catModel.Itemcode = "10021";
                if (catModel == null)
                    catModel = new CatalogueModel();
                TryUpdateModel(catModel);

                if (ModelState.IsValid)
                {
                    if (CreateData(catModel, ListCharas, EquModel, vendorsuppliersList, mdl, attList, files, stus, itemsts, HSNID, pvsts) == 0)
                    {
                        res = 0;
                    }
                    else if (CreateData(catModel, ListCharas, EquModel, vendorsuppliersList, mdl, attList, files, stus, itemsts, HSNID, pvsts) == 1)
                    {
                        res = 1;
                    }
                    else if (CreateData(catModel, ListCharas, EquModel, vendorsuppliersList, mdl, attList, files, stus, itemsts, HSNID, pvsts) == 2)
                    {
                        res = 2;
                    }
                    else if (CreateData(catModel, ListCharas, EquModel, vendorsuppliersList, mdl, attList, files, stus, itemsts, HSNID, pvsts) == 3)
                    {
                        res = 3;
                    }
                    else res = -1;
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
                res = 0;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        protected virtual int CreateData(CatalogueModel cat, List<AttributeList> ListAttributes,
            Equipment EquModel, List<Vendorsupplier> vendorsuppliersList,
            ERPInfoModel mdl, List<AttachmentModel> attList, HttpFileCollectionBase files, string stus, string itemsts, string HSNID, string pvsts)
        {
            var proCat = new Prosol_Datamaster();
            //  var user = _
            cat.Itemcode = cat.Itemcode != null ? cat.Itemcode : generateRequest();


            //Attribute
            var lstCharateristics = new List<Prosol_AttributeList>();
            foreach (AttributeList LstAtt in ListAttributes)
            {
                var AttrMdl = new Prosol_AttributeList();
                AttrMdl.Characteristic = LstAtt.Characteristic;

                if (LstAtt.Value != null && LstAtt.Value != "")
                {
                    // var value1 = _CatalogueService.getsm_value(LstAtt.Value);

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

                erpMdl.Plant_ = mdl.Plant_;
                erpMdl.ProfitCenter_ = mdl.ProfitCenter_;
                erpMdl.StorageLocation_ = mdl.StorageLocation_;
                erpMdl.StorageBin_ = mdl.StorageBin_;
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
                Equi_mdl.EMS = EquModel.EMS;
                Equi_mdl.ENS = EquModel.ENS;

                proCat.Equipment = Equi_mdl;
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
                        attlst.FileName = atlst.FileName;
                        attlst.FileSize = atlst.FileSize;
                        attlst.FileId = atlst.FileId;


                        lstAtt.Add(attlst);
                    }
                }
            }
            _CatalogueService.WriteAttachment(lstAtt, files);


            proCat.Vendorsuppliers = LstVendors;

            proCat._id = cat._id != null ? new MongoDB.Bson.ObjectId(cat._id) : new MongoDB.Bson.ObjectId();

            proCat.Itemcode = cat.Itemcode;
            proCat.Materialcode = cat.Materialcode;
            // proCat.ItemStatus = Convert.ToInt16(itemsts);
            if (cat.ItemStatus == 1 || cat.ItemStatus == 0)
                proCat.ItemStatus = 1;
            else proCat.ItemStatus = 3;


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
            proCat.Application = cat.Application;
            proCat.Drawingno = cat.Drawingno;
            proCat.Referenceno = cat.Referenceno;
            proCat.Remarks = cat.Remarks;
            proCat.RevRemarks = cat.RevRemarks;
            proCat.RelRemarks = cat.RelRemarks;
            proCat.Unspsc = cat.Unspsc;
            proCat.UOM = cat.UOM;
            proCat.HSNID = HSNID;
            //  proCat.HSNDesc = Desc;
            proCat.Maincode = cat.Maincode;
            proCat.Subcode = cat.Subcode;
            proCat.Subsubcode = cat.Subsubcode;
            var codeLogic = _Nounmodifier.showcode();
            //  proCat.Logic = codeLogic.CODELOGIC;

            proCat.Characteristics = lstCharateristics;
            //pvdata
            proCat.System_Balance = cat.System_Balance;
            proCat.Quantity_as_on_Date = cat.Quantity_as_on_Date;


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
            proCat.category = cat.category;
            if (cat.category == null || cat.category == "")
            {
                proCat.Equipments = new List<EquList>();
                var obj = new EquList();

                obj.Itemcode = cat.Equipments;
                obj.PartQty = cat.PartQty;
                proCat.Equipments.Add(obj);
            }

            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

            if (cat._id == null)
            {
                //var usrInfo1 = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
                //string tmpStr = "";
                //foreach (TargetExn ent in usrInfo1.Roles)
                //{
                //    if (ent.Name == "Cataloguer") tmpStr = ent.TargetId;
                //}
                //var relUname = _UserCreateService.getimage(tmpStr);
                //string uID = tmpStr;
                //string uName = relUname.UserName;

                var cataloguer = new Prosol_UpdatedBy();
                cataloguer.UserId = usrInfo.Userid;
                cataloguer.Name = usrInfo.UserName;
                cataloguer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);




                proCat.Catalogue = cataloguer;
            }

            proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            return _CatalogueService.WriteData(proCat, stus);
        }
        private string generateRequest()
        {
            string itmCode = "";
            var ICode = _CatalogueService.getItem();

            if (ICode != "")
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
                proCat.HSNID = cat.HSNID;
                // proCat.HSNDesc = cat.HSNDesc;
                proCat.UOM = cat.UOM;
                proCat.Maincode = cat.Maincode;
                proCat.Subcode = cat.Subcode;
                proCat.Subsubcode = cat.Subsubcode;
                proCat.category = cat.category;
                proCat.Equipments = cat.Itemcode;

                if (cat.Equipments != null && cat.Equipments.Count > 0)
                {
                    proCat.EquipLists = new List<EquipList>();
                    foreach (EquList mdl in cat.Equipments)
                    {
                        var ob = new EquipList();
                        ob.Itemcode = mdl.Itemcode;
                        ob.PartQty = mdl.PartQty;

                        proCat.EquipLists.Add(ob);
                    }
                }

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
                    updted.Name = cat.PVuser.Name;
                    proCat.PVuser = updted;
                }
                proCat.Characteristics = lstCharateristics;

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

                erpMdl.Plant = mdl.Plant;
                erpMdl.ProfitCenter = mdl.ProfitCenter;
                erpMdl.StorageLocation = mdl.StorageLocation;
                erpMdl.StorageBin = mdl.StorageBin;
                erpMdl.ValuationClass = mdl.ValuationClass;
                erpMdl.PriceControl = mdl.PriceControl;
                erpMdl.ValuationCategory = mdl.ValuationCategory;
                erpMdl.VarianceKey = mdl.VarianceKey;

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

                erpMdl.SafetyStock_ = mdl.SafetyStock_;
                erpMdl.ReOrderPoint_ = mdl.ReOrderPoint_;
                erpMdl.MaxStockLevel_ = mdl.MaxStockLevel_;
                erpMdl.MinStockLevel_ = mdl.MinStockLevel_;

                // Sales & Others
                erpMdl.AccAsignmtCategory = mdl.AccAsignmtCategory;
                erpMdl.TaxClassificationGroup = mdl.TaxClassificationGroup;
                erpMdl.ItemCategoryGroup = mdl.ItemCategoryGroup;
                erpMdl.SalesOrganization = mdl.SalesOrganization;
                erpMdl.DistributionChannel = mdl.DistributionChannel;
                erpMdl.MaterialStrategicGroup = mdl.MaterialStrategicGroup;
                erpMdl.PurchasingGroup = mdl.PurchasingGroup;
                erpMdl.PurchasingValueKey = mdl.PurchasingValueKey;

                erpMdl.GoodsReceptprocessingTime_ = mdl.GoodsReceptprocessingTime_;
                erpMdl.Quantity_ = mdl.Quantity_;


            }
            return this.Json(erpMdl, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public JsonResult GetEquip(string EName)
        {
            var LstE = _CatalogueService.GetEquip(EName);

            var attachMdlList = new List<Equipment>();
            foreach (Prosol_Datamaster atm in LstE)
            {
                var lMdl = new Equipment();
                if (atm.Equipment != null)
                {
                    if (atm.Equipment.Name != null || atm.Equipment.Name != " ")
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
        [Authorize]
        public JsonResult GetDataList()
        {
            int sts = 0;
            int sts1 = 1;
            var dataList = _CatalogueService.GetDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();
            int saved = 0;
            int bal = 0;
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
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;
                proCat.category = cat.category;
                proCat.Unspsc = cat.Unspsc;

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

                proCat.Junk = cat.Junk;
                lstCatalogue.Add(proCat);
            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetSpareList(string itemCode)
        {
            var SparesLst = _CatalogueService.GetSparesList(itemCode).ToList();

            var lstCatalogue = new List<CatalogueModel>();
            int saved = 0;
            int bal = 0;
            foreach (Prosol_Datamaster cat in SparesLst)
            {
                var proCat = new CatalogueModel();

                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Materialcode = cat.Materialcode;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;
                proCat.UOM = cat.UOM;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;
                proCat.category = cat.category;
                if (cat.Equipments != null)
                {
                    foreach (var pt in cat.Equipments)
                    {
                        if (pt.Itemcode == itemCode)
                        {
                            proCat.PartQty = pt.PartQty;
                            break;
                        }
                    }
                   
                }
              
                //Attribute
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {

                    foreach (Prosol_AttributeList LstAtt in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = LstAtt.Characteristic;
                        AttrMdl.Value = LstAtt.Value;
                        AttrMdl.UOM = LstAtt.UOM;
                        AttrMdl.Squence = LstAtt.Squence;
                        AttrMdl.ShortSquence = LstAtt.ShortSquence;
                        AttrMdl.Source = LstAtt.Source;

                        lstCharateristics.Add(AttrMdl);

                    }
                }
                proCat.Characteristics = lstCharateristics;

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

                proCat.Junk = cat.Junk;
                lstCatalogue.Add(proCat);
            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [Authorize]
        public JsonResult GetFunLoc(string srch)
        {
            var FLLst = _CatalogueService.GetFunLocList(srch).ToList();

            var jsonResult = Json(FLLst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult FetchExist(string Srchcode)
        {
            var SparesLst = _CatalogueService.FetchExist(Srchcode).ToList();

            var lstCatalogue = new List<CatalogueModel>();
            int saved = 0;
            int bal = 0;
            foreach (Prosol_Datamaster cat in SparesLst)
            {
                var proCat = new CatalogueModel();

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
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;
                proCat.category = cat.category;
                lstCatalogue.Add(proCat);
            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //
        public JsonResult AddexistSpares()
        {
            var Spares = Request.Form["SparesLst"];

            var SparesList = JsonConvert.DeserializeObject<List<CatalogueModel>>(Spares);

            var listdt = new List<Prosol_Datamaster>();
            foreach (CatalogueModel mdl in SparesList)
            {

                var proCat = new Prosol_Datamaster();

                proCat.Itemcode = mdl.Itemcode;
                proCat.Equipments = new List<EquList>();
                var obj = new EquList();

                obj.Itemcode = SparesList[0].Equipments;
                obj.PartQty = mdl.PartQty;
                proCat.Equipments.Add(obj);
                listdt.Add(proCat);

            }
            var cunt = _CatalogueService.AddexistSpares(listdt);
            return this.Json(cunt, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetEquDataList()
        {
            int sts = 0;
            int sts1 = 1;
            var dataList = _CatalogueService.GetDataList(sts, sts1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();
            int saved = 0;
            int bal = 0;
            foreach (Prosol_Datamaster cat in dataList)
            {
                var proCat = new CatalogueModel();

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
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;

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

                proCat.Junk = cat.Junk;

                lstCatalogue.Add(proCat);
            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        //pvdata

        public JsonResult GetDataListpv()
        {
            int sts = 12;
            // string sts = 'Pending';
            string pending1 = "Pending";
            var dataList = _CatalogueService.GetDataListpv(pending1, Convert.ToString(Session["userid"])).ToList();

            var lstCatalogue = new List<CatalogueModel>();
            int saved = 0;
            int bal = 0;
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
                proCat.Attachment = cat.Attachment;
                proCat.Reworkcat = cat.Reworkcat;
                proCat.PVstatus = cat.PVstatus;
                //  proCat.Specification = cat.Specification;


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
                    updted.Name = cat.PVuser.Name;
                    proCat.PVuser = updted;
                }

                //proCat.Shortdesc = cat.Shortdesc;
                //proCat.Longdesc = cat.Longdesc;
                //proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Junk = cat.Junk;
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

            var mdl = JsonConvert.DeserializeObject<CatalogueModel>(catDatalist);


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                if (mdl != null)
                {

                    var proCat = new Prosol_Datamaster();
                    proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                    if (mdl.ItemStatus == 1)
                        proCat.ItemStatus = 2;

                    proCat.Itemcode = mdl.Itemcode;
                    proCat.Noun = mdl.Noun;
                    proCat.Modifier = mdl.Modifier;
                    proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    proCat.Unspsc = mdl.Unspsc;
                    proCat.Junk = mdl.Junk;
                    var codeLogic = _Nounmodifier.showcode();
                    //    proCat.Logic = codeLogic.CODELOGIC;



                    //Review 
                    var reviewer = new Prosol_UpdatedBy();
                    reviewer.UserId = uID;
                    reviewer.Name = uName;
                    reviewer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    proCat.Review = reviewer;
                    if (mdl.ItemStatus == 3)
                    {
                        //{
                        // var proCat = _CatalogueService.GetReleaseSingleItem(mdl.Itemcode);

                        if (codeLogic.CODELOGIC == "Customized Code")
                        {
                            //  string Lcode = proCat.Maincode + proCat.Subcode + proCat.Subsubcode;
                            if (proCat.Materialcode == null || proCat.Materialcode == "")

                                proCat.Materialcode = generateMaterialcode(proCat.Maincode + proCat.Subcode + proCat.Subsubcode, 1);

                        }
                        else if (codeLogic.CODELOGIC == "UNSPSC Code")
                        {
                            if (proCat.Materialcode == null || proCat.Materialcode == "")
                                proCat.Materialcode = generateMaterialcode(proCat.Unspsc, 0);
                        }
                        else
                        {
                            proCat.Materialcode = proCat.Itemcode;
                        }

                        proCat.ItemStatus = 6;
                    }
                    listdt.Add(proCat);

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
        public JsonResult RemoveEquipment()
        {

            var catDatalist = Request.Form["DataList"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {

                    var proCat = new Prosol_Datamaster();
                    proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                    proCat.ItemStatus = 2;
                    proCat.Itemcode = mdl.Itemcode;
                    listdt.Add(proCat);

                }

                res = _CatalogueService.RemoveEquipment(listdt);
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
        public JsonResult PotentialEquip()
        {
            var catDatalist = Request.Form["DataList"];
            var catDatalist1 = Request.Form["DataList1"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);
            var catModelList1 = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist1);

            string dupList = "";
            int mainFlg = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {
                    dupList = dupList + "Duplicate codes for " + mdl.Itemcode + " : ";
                    foreach (CatalogueModel md in catModelList1)
                    {
                        if (mdl.Itemcode != md.Itemcode)
                        {
                            int skipFlg = 0;
                            if (mdl.Vendorsuppliers != null && mdl.Vendorsuppliers.Count > 0)
                            {
                                foreach (Vendorsupplier eq in mdl.Vendorsuppliers)
                                {

                                    if (md.Vendorsuppliers != null && md.Vendorsuppliers.Count > 0)
                                    {
                                        foreach (Vendorsupplier eq1 in md.Vendorsuppliers)
                                        {
                                            if (eq.Refflag == "MODEL NUMBER" && eq.RefNo == eq1.RefNo)
                                            {
                                                dupList = dupList + mdl.Itemcode + ",";
                                                skipFlg = 1;
                                                mainFlg = 1;
                                            }
                                        }

                                    }
                                }

                            }

                            if (skipFlg == 0)
                            {
                                if (mdl.Shortdesc == md.Shortdesc)
                                {
                                    dupList = dupList + mdl.Itemcode + ",";
                                    mainFlg = 1;
                                }
                            }
                        }

                    }


                }
                if (mainFlg == 0)
                    dupList = "";


            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                dupList = e.ToString();
            }
            return this.Json(dupList, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult CopyEquip()
        {
            var catDatalist = Request.Form["DataList"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {

                    var proCat = new Prosol_Datamaster();
                    proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                    proCat.ItemStatus = 2;
                    proCat.Itemcode = mdl.Itemcode;
                    listdt.Add(proCat);

                }

                res = _CatalogueService.CopyEquip(listdt);
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
        public JsonResult RemoveSpare()
        {

            var catDatalist = Request.Form["DataList"];
            string equi = Request.Form["Equip"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {

                    var proCat = new Prosol_Datamaster();
                    proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                    proCat.ItemStatus = 2;
                    proCat.Itemcode = mdl.Itemcode;
                    listdt.Add(proCat);

                }

                res = _CatalogueService.RemoveSpare(listdt, equi);
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
        public JsonResult PotentialSpare()
        {
            var catDatalist = Request.Form["DataList"];
            var catDatalist1 = Request.Form["DataList1"];

            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);
            var catModelList1 = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist1);

            string dupList = "";
            int mainFlg = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {
                    dupList = dupList + "Duplicate codes for " + mdl.Itemcode + " : ";
                    foreach (CatalogueModel md in catModelList1)
                    {
                        if (mdl.Itemcode != md.Itemcode)
                        {
                            int skipFlg = 0;
                            if (mdl.Vendorsuppliers != null && mdl.Vendorsuppliers.Count > 0)
                            {
                                foreach (Vendorsupplier eq in mdl.Vendorsuppliers)
                                {

                                    if (md.Vendorsuppliers != null && md.Vendorsuppliers.Count > 0)
                                    {
                                        foreach (Vendorsupplier eq1 in md.Vendorsuppliers)
                                        {
                                            if (eq.Refflag == "MODEL NUMBER" && eq.RefNo == eq1.RefNo)
                                            {
                                                dupList = dupList + mdl.Itemcode + ",";
                                                skipFlg = 1;
                                                mainFlg = 1;
                                            }
                                        }

                                    }
                                }

                            }

                            if (skipFlg == 0)
                            {
                                if (mdl.Shortdesc == md.Shortdesc)
                                {
                                    dupList = dupList + mdl.Itemcode + ",";
                                    mainFlg = 1;
                                }
                            }
                        }

                    }


                }
                if (mainFlg == 0)
                    dupList = "";


            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                dupList = e.ToString();
            }
            return this.Json(dupList, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult CopySpare()
        {
            var catDatalist = Request.Form["DataList"];
            string equi = Request.Form["Equip"];
            var catModelList = JsonConvert.DeserializeObject<List<CatalogueModel>>(catDatalist);


            int res = 0;
            try
            {
                var updatedby = new UpdatedBy();
                var listdt = new List<Prosol_Datamaster>();
                foreach (CatalogueModel mdl in catModelList)
                {

                    var proCat = new Prosol_Datamaster();
                    proCat._id = new MongoDB.Bson.ObjectId(mdl._id);
                    proCat.ItemStatus = 2;
                    proCat.Itemcode = mdl.Itemcode;
                    listdt.Add(proCat);

                }

                res = _CatalogueService.CopySpare(listdt, equi);
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
            var res = _CatalogueService.Deletefile(id, imgId);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public void Downloadfile(string fileName, string type, string imgId)
        {

            byte[] bytearr = _CatalogueService.Downloadfile(imgId);

            Response.AddHeader("Content-type", type);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytearr);
            Response.Flush();
            Response.End();
            // return this.Json("", JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public void Downloadimages(string mat_code)
        {

            string[] split = mat_code.Split(new Char[] { ',', '\n', '\t', ' ' },
                                 StringSplitOptions.RemoveEmptyEntries);

            foreach (string x in split)
            {
                var ress = _CatalogueService.findattachmentss(x);

                if (ress.Count() > 0)
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
        public JsonResult getReviewerList()
        {

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
            var HSNID = Request.Form["HSNID"];
            var Desc = Request.Form["Desc"];

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
            proCat.HSNID = HSNID;
            //  proCat.HSNDesc = Desc;

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
            var jsonResult = Json(null, JsonRequestBehavior.AllowGet);
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
            proCat.Additionalinfo = cat.Additionalinfo;
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
                    proCat.Equipment = Equi_mdl;

                }
            }


            var res = _CatalogueService.GenerateShortLong(proCat);
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
                    var codeLogic = _Nounmodifier.showcode();
                    //    proCat.Logic = codeLogic.CODELOGIC;
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
        public JsonResult Reworkreview(string itemcode)
        {
            bool res = false;
            try
            {
                res = _CatalogueService.Reworkreview(itemcode);
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
                var codeLogic = _Nounmodifier.showcode();
                proCat.Logic = codeLogic.CODELOGIC;
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
            int res = 0;
            try
            {

                var listdt = new List<Prosol_Datamaster>();

                var codeLogic = _Nounmodifier.showcode();
                foreach (CatalogueModel mdl in catModelList)
                {
                    //if (mdl.ItemStatus == 5)
                    //{
                    var proCat = _CatalogueService.GetReleaseSingleItem(mdl.Itemcode);

                    if (codeLogic.CODELOGIC == "Customized Code")
                    {
                        //  string Lcode = proCat.Maincode + proCat.Subcode + proCat.Subsubcode;
                        if (proCat.Materialcode == null || proCat.Materialcode == "")

                            proCat.Materialcode = generateMaterialcode(proCat.Maincode + proCat.Subcode + proCat.Subsubcode, 1);

                    }
                    else if (codeLogic.CODELOGIC == "UNSPSC Code")
                    {
                        if (proCat.Materialcode == null || proCat.Materialcode == "")
                            proCat.Materialcode = generateMaterialcode(proCat.Unspsc, 0);
                    }
                    else
                    {
                        proCat.Materialcode = proCat.Itemcode;
                    }
                    proCat.ItemStatus = 6;
                    proCat.Noun = mdl.Noun;
                    proCat.Modifier = mdl.Modifier;
                    proCat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    listdt.Add(proCat);
                    //  }
                }
                res = _CatalogueService.WriteReleaseDataList(listdt);

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
        public JsonResult Reworkrelease(string itemcode)
        {
            bool res = false;
            try
            {
                res = _CatalogueService.Reworkrelease(itemcode);
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
            // string itmCode = "";
            string indicator = Convert.ToString(indc);
            var code = _CatalogueService.getMaterialCode(LogicCode);

            //if (code != "")
            //{
            //    long serialinr = 0;
            //    if (indc == 1)
            //    {
            //        serialinr = Convert.ToInt64(code.Substring(LogicCode.Length));
            //    }
            //    else serialinr = Convert.ToInt64(code.Substring(8, 4));

            //    serialinr++;
            //    string addincr = "";
            //    if (LogicCode.Length == 1 && (LogicCode=="C"))
            //    {
            //        switch (serialinr.ToString().Length)
            //        {
            //            case 1:
            //                addincr = "00000000" + serialinr;
            //                break;
            //            case 2:
            //                addincr = "0000000" + serialinr;
            //                break;
            //            case 3:
            //                addincr = "000000" + serialinr;
            //                break;
            //            case 4:
            //                addincr = "00000" + serialinr;
            //                break;
            //            case 5:
            //                addincr = "0000" + serialinr;
            //                break;
            //            case 6:
            //                addincr = "000" + serialinr;
            //                break;
            //            case 7:
            //                addincr = "00" + serialinr;
            //                break;
            //            case 8:
            //                addincr = "0" + serialinr;
            //                break;
            //            case 9:
            //                addincr = serialinr.ToString();
            //                break;
            //        }
            //        itmCode = LogicCode + addincr;
            //    }
            //    if (LogicCode.Length == 1 && (LogicCode == "2" || LogicCode == "4"))
            //    {
            //        switch (serialinr.ToString().Length)
            //        {
            //            case 1:
            //                addincr = "0000000" + serialinr;
            //                break;
            //            case 2:
            //                addincr = "000000" + serialinr;
            //                break;
            //            case 3:
            //                addincr = "00000" + serialinr;
            //                break;
            //            case 4:
            //                addincr = "0000" + serialinr;
            //                break;
            //            case 5:
            //                addincr = "000" + serialinr;
            //                break;
            //            case 6:
            //                addincr = "00" + serialinr;
            //                break;
            //            case 7:
            //                addincr = "0" + serialinr;
            //                break;                       
            //            case 8:
            //                addincr = serialinr.ToString();
            //                break;
            //        }
            //        itmCode = LogicCode + addincr;
            //    }
            //    //if (LogicCode.Length == 2)
            //    //{
            //    //    switch (serialinr.ToString().Length)
            //    //    {
            //    //        case 1:
            //    //            addincr = "0000000" + serialinr;
            //    //            break;
            //    //        case 2:
            //    //            addincr = "000000" + serialinr;
            //    //            break;
            //    //        case 3:
            //    //            addincr = "00000" + serialinr;
            //    //            break;
            //    //        case 4:
            //    //            addincr = "0000" + serialinr;
            //    //            break;
            //    //        case 5:
            //    //            addincr = "000" + serialinr;
            //    //            break;
            //    //        case 6:
            //    //            addincr = "00" + serialinr;
            //    //            break;
            //    //        case 7:
            //    //            addincr = "0" + serialinr;
            //    //            break;                        
            //    //        case 8:
            //    //            addincr = serialinr.ToString();
            //    //            break;
            //    //    }
            //    //    itmCode = LogicCode + addincr;

            //    //}

            //}
            //else
            //{
            //    if (LogicCode.Length == 1 && (LogicCode == "C"))
            //    {                    
            //        itmCode = LogicCode + "000000001";
            //    }
            //    if (LogicCode.Length == 1 && (LogicCode == "2" || LogicCode == "4"))
            //    {

            //        itmCode = LogicCode + "00000001";
            //    }
            //    //if (LogicCode.Length == 2)
            //    //{                   
            //    //    itmCode = LogicCode + "00000001";
            //    //}
            //}

            return code;
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
            data = data.Replace("*****", "\",\"");

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
        public JsonResult GetValues(string Noun, string Modifier, string Attribute)
        {


            var arrStr = _CatalogueService.GetValues(Noun, Modifier, Attribute);
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
                // proCat._id = cat._id.ToString();
                //  proCat.Itemcode = cat.Itemcode;
                proCat.ItemStatus = cat.ItemStatus;
                //  proCat.Legacy = cat.Legacy;
                //  proCat.Legacy2 = cat.Legacy2;

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
        public string CheckValue(string Noun, string Modifier, string Attribute, string Value)
        {

            var res = _CatalogueService.CheckValue(Noun, Modifier, Attribute, Value);

            return res;
        }
        public bool AddValue(string Noun, string Modifier, string Attribute, string Value, string abb)
        {
            string user = Session["username"].ToString();
            var res = _CatalogueService.AddValue(Noun, Modifier, Attribute, Value, abb, user);

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
        public JsonResult GetRejectCode(string Itemcode, string Remarks)
        {
            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();
            var result = _CatalogueService.GetCodeForRejectedItems(Itemcode, Remarks, userid, username);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRejectCode1(string Itemcode, string RevRemarks)
        {
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
        //pvdata
        public JsonResult PVDATAASSIGN(string Itemcode, string Remarks, string usr)
        {

            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();
            var result = _CatalogueService.PVDATA(Itemcode, Remarks, userid, username);
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
        //public JsonResult GetDBNounList1()
        //{
        //    var dataList = _Nounmodifier.GetNounModifierList();
        //    // var dataList = _CatalogueService.GetTotalItem().ToList();          
        //    //  var result = dataList.Select(i => new { i.Noun,i.Modifier }).Distinct().ToList();        
        //    var res = Json(dataList, JsonRequestBehavior.AllowGet);
        //    res.MaxJsonLength = int.MaxValue;
        //    return res;

        //}
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
            var dataList = _CatalogueService.searchMaster(sCode, sSource, sShort, sLong, sNoun, sModifier, sUser, sType, sStatus);
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
                    locMdl.Junk = mdl.Junk;
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
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkShortLong(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
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
        public JsonResult BulkDictionary()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    res = _CatalogueService.BulkDictionary(file);
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetEquipsparelist(string Noun, string Modifier)
        {
            var SparesLst = _CatalogueService.Getsparelist(Noun, Modifier).ToList();

            var sparelist = new List<EquipDictionary>();

            foreach (Prosol_EquipDictionary cat in SparesLst)
            {
                var proCat = new EquipDictionary();
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.SpareList = cat.SpareList;
                proCat.NCP = cat.NCP;
                proCat.CP = cat.CP;
                proCat.SpareCategory = cat.SpareCategory;
                sparelist.Add(proCat);
            }

            var jsonResult = Json(sparelist, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [Authorize]
        public JsonResult GetEqu_Noun()
        {
            var arrStr = _Nounmodifier.GetNounList().ToList();
            var filterLst = arrStr.Where(x => x.RP == "Equ").ToList();
            var result = filterLst.Select(i => new { i.Noun }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetEqu_Modifier(string Noun)
        {
            var arrStr = _Nounmodifier.GetModifierList(Noun);
            var result = arrStr.Where(x => x.RP == "Equ").ToList();
            return this.Json(result, JsonRequestBehavior.AllowGet);

        }
    }


}