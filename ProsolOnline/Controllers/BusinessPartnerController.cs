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
    public class BusinessPartnerController : Controller
    {

        private readonly IBusinessPartner _BusinessPartnerService;
        private readonly IMaster _MasterService;
        public BusinessPartnerController(IBusinessPartner BPservice, IMaster Masterservice)
        {
            _BusinessPartnerService = BPservice;
            _MasterService = Masterservice;

        }
        // GET: GeneralSettings
       
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
        public ActionResult General()
        {
            if (CheckAccess("BP-General") == 1)
                return View();
            else if (CheckAccess("General") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Cust()
        {
            if (CheckAccess("BP-Customer") == 1)
                return View();
            else if (CheckAccess("BP-Customer") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Ven()
        {
            if (CheckAccess("BP-Vendor") == 1)
                return View();
            else if (CheckAccess("BP-Vendor") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult creation()
        {

            if (CheckAccess("BP-Creation") == 1)
                return View();
            else if (CheckAccess("BP-Creation") == 0)
                return View("Denied");
            else return View("Login");

        }


        [HttpPost]
        public JsonResult InsertData()
        {
            var obj = Request.Form["data"];
            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<BPMasterModel>(obj);
            bool res = false;
            try
            {
                if (Model == null)
                    Model = new BPMasterModel();
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {  //UOM DB write
                    var mdl = new Prosol_BPMaster();
                    mdl._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    mdl.Label = Model.Label;
                    mdl.Code = Model.Code;
                    mdl.Title = Model.Title;
                    res = _BusinessPartnerService.InsertData(mdl);
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
            var objList = _BusinessPartnerService.GetDataList(label);
            var lst = new List<BPMasterModel>();
            foreach (Prosol_BPMaster mdl in objList)
            {
                var obj = new BPMasterModel();
                obj._id = mdl._id.ToString();
                obj.Label = mdl.Label;
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBPMaster()
        {
            var objList = _BusinessPartnerService.GetMaster();
            var lst = new List<BPMasterModel>();
            foreach (Prosol_BPMaster mdl in objList)
            {
                var obj = new BPMasterModel();
                obj._id = mdl._id.ToString();
                obj.Label = mdl.Label;
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetMaterialMaster()
        {
            var objList = _MasterService.GetMaster();
            var lst = new List<MasterModel>();
            foreach (Prosol_Master mdl in objList)
            {
                var obj = new MasterModel();
                obj._id = mdl._id.ToString();
                obj.Label = mdl.Label;
                obj.Code = mdl.Code;
                obj.Title = mdl.Title;
                obj.Islive = mdl.Islive;
                lst.Add(obj);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }
        
        //public JsonResult DelData(string id)
        //{

        //    var res = _BusinessPartnerService.DelData(id);
        //    return this.Json(res, JsonRequestBehavior.AllowGet);

        //}
        public JsonResult DisableData(string id, bool Islive)
        {

            var res = _BusinessPartnerService.DisableData(id, Islive);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CreateBP(string sKey)
        {
            var res = false;
            if (sKey == "GEN")
            {
                var Gen = Request.Form["Genral"];
                var Genral = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_BPGenralModel>(Gen);
                res = _BusinessPartnerService.CreateGen(Genral);
            }
            else if (sKey == "CUST")
            {
                var Cust = Request.Form["Cust"];
                var Customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_BPCustomerModel>(Cust);
                var CustPF = Request.Form["CustPF"];
                var CustomerPF = JsonConvert.DeserializeObject<List<PartnerFun>>(CustPF);
                var PartnerFuns = new List<PartnerFunn>();
                foreach (PartnerFun ext in CustomerPF)
                {
                    var x = new PartnerFunn();
                    x.PartnerFunc = ext.PartnerFunc;
                    x.PartnerNo = ext.PartnerNo;
                    x.PartnerDesc = ext.PartnerDesc;
                    PartnerFuns.Add(x);
                }
                Customer.PartnerFuncs = PartnerFuns;
                res = _BusinessPartnerService.CreateCust(Customer);
            }
            else if (sKey == "VEN")
            {
                var Ven = Request.Form["Ven"];
                var Vendor = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_BPVendorModel>(Ven);
               
                var VenPF = Request.Form["VenPF"];
                var VendorPF = JsonConvert.DeserializeObject<List<PartnerFun>>(VenPF);
                var PartnerFuns = new List<PartnerFunn>();
                foreach (PartnerFun ext in VendorPF)
                {
                    var x = new PartnerFunn();
                    x.PartnerFunc = ext.PartnerFunc;
                    x.PartnerNo = ext.PartnerNo;
                    x.PartnerDesc = ext.PartnerDesc;
                    PartnerFuns.Add(x);
                }
                Vendor.PartnerFuncs = PartnerFuns;
                res = _BusinessPartnerService.CreateVen(Vendor);
            }
            else if (sKey == "ALL")
            {
                var Gen = Request.Form["Genral"];
                var Genral = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_BPGenralModel>(Gen);
                res = _BusinessPartnerService.CreateGen(Genral);
                var Cust = Request.Form["Cust"];
                var Customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_BPCustomerModel>(Cust);
                var CustPF = Request.Form["CustPF"];
                var CustomerPF = JsonConvert.DeserializeObject<List<PartnerFun>>(CustPF);
                var PartnerFuns1 = new List<PartnerFunn>();
                foreach (PartnerFun ext in CustomerPF)
                {
                    var x = new PartnerFunn();
                    x.PartnerFunc = ext.PartnerFunc;
                    x.PartnerNo = ext.PartnerNo;
                    x.PartnerDesc = ext.PartnerDesc;
                    PartnerFuns1.Add(x);
                }
                Customer.PartnerFuncs = PartnerFuns1;
                var Ven = Request.Form["Ven"];
                var Vendor = Newtonsoft.Json.JsonConvert.DeserializeObject<Prosol_BPVendorModel>(Ven);
                var VenPF = Request.Form["VenPF"];
                var VendorPF = JsonConvert.DeserializeObject<List<PartnerFun>>(VenPF);
                var PartnerFuns2 = new List<PartnerFunn>();
                foreach (PartnerFun ext in VendorPF)
                {
                    var x = new PartnerFunn();
                    x.PartnerFunc = ext.PartnerFunc;
                    x.PartnerNo = ext.PartnerNo;
                    x.PartnerDesc = ext.PartnerDesc;
                    PartnerFuns2.Add(x);
                }
                Vendor.PartnerFuncs = PartnerFuns2;
                res = _BusinessPartnerService.CreateGen(Genral);
                res = _BusinessPartnerService.CreateCust(Customer);
                res = _BusinessPartnerService.CreateVen(Vendor);
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
            
        }
        public class total
        {
            public List<Prosol_BPGenralModel> GenList { set; get; }
            public List<Prosol_BPCustomerModel> CustList { set; get; }
            public List<Prosol_BPVendorModel> VenList { set; get; }

        }
        public JsonResult Updategeneral(string id)
        {
            var res = false;

            var Gen = Request.Form["GENERAL"];

            var Genral = JsonConvert.DeserializeObject<List<Prosol_BPGenralModel>>(Gen);
            var mirm = JsonConvert.DeserializeObject<List<BPGenralModel>>(Gen);
            var list = new List<Prosol_BPGenralModel>();
            //var x = new Prosol_BPGenralModel();
            //x._id = Gen.
            foreach (BPGenralModel mdl in mirm)

            {
                // string str = prm.requestedOn.ToLongDateString();
                //  string str = DateTime.Parse(prm.requestedOn.ToString()).ToString("dd/MM/yyyy HH:MM tt");
                Prosol_BPGenralModel obj = new Prosol_BPGenralModel();

                obj.BPNumber = mdl.BPNumber;
                obj.Name1 = mdl.Name1;
                obj.Srcterm1 = mdl.Srcterm1;
                obj.City = mdl.City;
                obj.Zipcode = mdl.Zipcode;
                obj.Country = mdl.Country;
                obj.Language = mdl.Language;
                obj.BPCategory = mdl.BPCategory;
                obj.BPType = mdl.BPType;
                obj.BPGrouping = mdl.BPGrouping;
                obj.BPNOEXT = mdl.BPNOEXT;
                obj.AuthGrp = mdl.AuthGrp;
                obj.Title = mdl.Title;
                obj.Name1 = mdl.Name1;
                obj.Name2 = mdl.Name2;
                obj.Name3 = mdl.Name3;
                obj.Srcterm2 = mdl.Srcterm2;
                obj.Email = mdl.Email;
                obj.Building = mdl.Building;
                obj.RoomNo = mdl.RoomNo;
                obj.Floor = mdl.Floor;
                obj.Address1 = mdl.Address1;
                obj.Address2 = mdl.Address2;
                obj.Address3 = mdl.Address3;
                obj.Address4 = mdl.Address4;
                obj.Address5 = mdl.Address5;
                obj.District = mdl.District;
                obj.Country = mdl.Country;
                obj.Region = mdl.Region;
                obj.PostCode1 = mdl.PostCode1;
                obj.PostCode2 = mdl.PostCode2;
                obj.PObox = mdl.PObox;

                obj.TransZone = mdl.TransZone;
                obj.Tele = mdl.Tele;
                obj.Teleextn = mdl.Teleextn;
                obj.Fax = mdl.Fax;
                obj.Faxextn = mdl.Faxextn;
                obj.TaxJur = mdl.TaxJur;
                obj.Person_name = mdl.Person_name;

                res = _BusinessPartnerService.UpdateGen(obj, id);
            }


            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Updatevendor(string id)
        {
            var res = false;

            var ven = Request.Form["vendor"];

            var vendor = JsonConvert.DeserializeObject<List<Prosol_BPVendorModel>>(ven);
            var mirm = JsonConvert.DeserializeObject<List<BPVendorModel>>(ven);
            var list = new List<Prosol_BPVendorModel>();
            //var x = new Prosol_BPGenralModel();
            //x._id = Gen.
            foreach (BPVendorModel mdl in mirm)

            {
                // string str = prm.requestedOn.ToLongDateString();
                //  string str = DateTime.Parse(prm.requestedOn.ToString()).ToString("dd/MM/yyyy HH:MM tt");
                Prosol_BPVendorModel obj = new Prosol_BPVendorModel();

                obj.BPNumber = mdl.BPNumber;
                obj.VendorNo = mdl.VendorNo;
                obj.AccGrp = mdl.AccGrp;
                obj.PurchOrg = mdl.PurchOrg;
                obj.Payterms = mdl.Payterms;
                obj.Plant = mdl.Plant;
                obj.Existvend_num = mdl.Existvend_num;
                obj.VendorType = mdl.VendorType;
                obj.Status = mdl.Status;
                obj.AuthGrp = mdl.AuthGrp;
                obj.PlntRelvnt = mdl.PlntRelvnt;
                obj.FactoryCal = mdl.FactoryCal;
                obj.CompanyCode = mdl.CompanyCode;
                obj.CR_number = mdl.CR_number;
                obj.Payment_method = mdl.Payment_method;
                obj.Communication_Language = mdl.Communication_Language;
                obj.RecACs = mdl.RecACs;
                obj.AltPayee = mdl.AltPayee;
                obj.TaxType = mdl.TaxType;
                obj.TaxBase = mdl.TaxBase;
                obj.TaxNo = mdl.TaxNo;
                obj.VatNo = mdl.VatNo;
                obj.TOB = mdl.TOB;
                obj.TOI = mdl.TOI;
                obj.PurchOrg = mdl.PurchOrg;
                obj.PurchaseGrp = mdl.PurchaseGrp;
                obj.POCur = mdl.POCur;
                obj.Incoterms1 = mdl.Incoterms1;
                obj.Incoterms2 = mdl.Incoterms2;
                obj.GRBsdIV = mdl.GRBsdIV;
                obj.OAReq = mdl.OAReq;
                obj.PDT = mdl.PDT;
                obj.PricingSchema = mdl.PricingSchema;
                obj.ERS = mdl.ERS;
                obj.BankAccount = mdl.BankAccount;
                obj.BankBranch = mdl.BankBranch;
                obj.BankName = mdl.BankName;
                obj.BANK_ADDRESS_LINE_1 = mdl.BANK_ADDRESS_LINE_1;
                obj.BANK_ADDRESS_LINE_2 = mdl.BANK_ADDRESS_LINE_2;
                obj.City = mdl.City;
                obj.Bank_Key = mdl.Bank_Key;
                obj.Bank_Country = mdl.Bank_Country;
                obj.HouseBank = mdl.HouseBank;

                res = _BusinessPartnerService.UpdateVen(obj, id);
            }


            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Updatecust(string id)
        {
            var res = false;

            var CUS = Request.Form["CUSTOMER"];

            var vendor = JsonConvert.DeserializeObject<List<Prosol_BPCustomerModel>>(CUS);
            var mirm = JsonConvert.DeserializeObject<List<BPCustomerModel>>(CUS);
            var list = new List<Prosol_BPCustomerModel>();
            //var x = new Prosol_BPGenralModel();
            //x._id = Gen.
            foreach (BPCustomerModel mdl in mirm)

            {
                // string str = prm.requestedOn.ToLongDateString();
                //  string str = DateTime.Parse(prm.requestedOn.ToString()).ToString("dd/MM/yyyy HH:MM tt");
                Prosol_BPCustomerModel obj = new Prosol_BPCustomerModel();

                obj.BPNumber = mdl.BPNumber;
                obj.BPNumber = mdl.BPNumber;
                obj.Salesorg = mdl.Salesorg;
                obj.DistChan = mdl.DistChan;
                obj.Divison = mdl.Divison;
                obj.Companycode = mdl.Companycode;
                obj.SalesOfc = mdl.SalesOfc;
                obj.SalesGrp = mdl.SalesGrp;
                obj.CustCur = mdl.CustCur;



                obj.CustNo = mdl.CustNo;
                obj.Bankkey = mdl.Bankkey;
                obj.BankAcc = mdl.BankAcc;
                obj.bankCountry = mdl.bankCountry;
                obj.Cntrlkey = mdl.Cntrlkey;
                obj.PBT = mdl.PBT;
                obj.AccNo = mdl.AccNo;
                obj.Reference = mdl.Reference;
                obj.AccHldr = mdl.AccHldr;
                obj.ReconAcc = mdl.ReconAcc;
                obj.PaymntTerms = mdl.PaymntTerms;
                obj.CmpltDelivery = mdl.CmpltDelivery;
                obj.ODT = mdl.ODT;
                obj.UDT = mdl.UDT;
                obj.DelyPrior = mdl.DelyPrior;
                obj.POD = mdl.POD;
                obj.ShpngCndtn = mdl.ShpngCndtn;
                obj.Delyplant = mdl.Delyplant;
                obj.CCA = mdl.CCA;
                obj.IncoTerms1 = mdl.IncoTerms1;
                obj.IncoTerms2 = mdl.IncoTerms2;
                obj.Payterm = mdl.Payterm;
                obj.AAGC = mdl.AAGC;

                obj.OrdrComb = mdl.OrdrComb;
                obj.OrdrProb = mdl.OrdrProb;
                obj.PartnerNo = mdl.PartnerNo;
                obj.PartnerFunc = mdl.PartnerFunc;
                obj.PartnerDesc = mdl.PartnerDesc;

                obj.CPP = mdl.CPP;
                obj.Custgrp = mdl.Custgrp;
                obj.ABC = mdl.ABC;
                obj.Pricegrp = mdl.Pricegrp;
                obj.Pricelst = mdl.Pricelst;
                obj.CSG = mdl.CSG;

                res = _BusinessPartnerService.UpdateCust(obj, id);
            }


            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelData(string id)
        {

            var res = _BusinessPartnerService.DelData(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult custDel(string id)
        {

            var res = _BusinessPartnerService.custdel(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult venDel(string id)
        {

            var res = _BusinessPartnerService.venDel(id);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBPList()
        {
            var Gen = _BusinessPartnerService.GenList();
            var list = new List<BPGenralModel>();
            foreach (Prosol_BPGenralModel mdl in Gen)
            {
                var obj = new BPGenralModel();
                obj._id = mdl._id.ToString();
                obj.BPNumber = mdl.BPNumber;
                obj.Name1 = mdl.Name1;
                obj.Srcterm1 = mdl.Srcterm1;
                obj.City = mdl.City;
                obj.Zipcode = mdl.Zipcode;
                obj.Country = mdl.Country;
                obj.Language = mdl.Language;
                obj.BPCategory = mdl.BPCategory;
                obj.BPType = mdl.BPType;
                obj.BPGrouping = mdl.BPGrouping;
                obj.BPNOEXT = mdl.BPNOEXT;
                obj.AuthGrp = mdl.AuthGrp;
                obj.Title = mdl.Title;
                obj.Name1 = mdl.Name1;
                obj.Name2 = mdl.Name2;
                obj.Name3 = mdl.Name3;
                obj.Srcterm2 = mdl.Srcterm2;
                obj.Email = mdl.Email;
                obj.Building = mdl.Building;
                obj.RoomNo = mdl.RoomNo;
                obj.Floor = mdl.Floor;
                obj.Address1 = mdl.Address1;
                obj.Address2 = mdl.Address2;
                obj.Address3 = mdl.Address3;
                obj.Address4 = mdl.Address4;
                obj.Address5 = mdl.Address5;
                obj.District = mdl.District;
                obj.Country = mdl.Country;
                obj.Region = mdl.Region;
                obj.PostCode1 = mdl.PostCode1;
                obj.PostCode2 = mdl.PostCode2;
                obj.PObox = mdl.PObox;

                obj.TransZone = mdl.TransZone;
                obj.Tele = mdl.Tele;
                obj.Teleextn = mdl.Teleextn;
                obj.Fax = mdl.Fax;
                obj.Faxextn = mdl.Faxextn;
                obj.TaxJur = mdl.TaxJur;
                obj.Person_name = mdl.Person_name;

                list.Add(obj);
            }
            //  var Ven = _BusinessPartnerService.VenList().ToList();
            //var Cust = _BusinessPartnerService.CustList().ToList();
            //var list = new total();
            //list.GenList = Gen;
            //list.CustList = Cust;
            //list.VenList = Ven;
            return this.Json(list, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetcustList()
        {
            var Gen = _BusinessPartnerService.CustList();
            var list = new List<BPCustomerModel>();
            foreach (Prosol_BPCustomerModel mdl in Gen)
            {
                var obj = new BPCustomerModel();
                obj._id = mdl._id.ToString();
                obj.BPNumber = mdl.BPNumber;
                obj.Salesorg = mdl.Salesorg;
                obj.DistChan = mdl.DistChan;
                obj.Divison = mdl.Divison;
                obj.Companycode = mdl.Companycode;
                obj.SalesOfc = mdl.SalesOfc;
                obj.SalesGrp = mdl.SalesGrp;
                obj.CustCur = mdl.CustCur;



                obj.CustNo = mdl.CustNo;
                obj.Bankkey = mdl.Bankkey;
                obj.BankAcc = mdl.BankAcc;
                obj.bankCountry = mdl.bankCountry;
                obj.Cntrlkey = mdl.Cntrlkey;
                obj.PBT = mdl.PBT;
                obj.AccNo = mdl.AccNo;
                obj.Reference = mdl.Reference;
                obj.AccHldr = mdl.AccHldr;
                obj.ReconAcc = mdl.ReconAcc;
                obj.PaymntTerms = mdl.PaymntTerms;
                obj.CmpltDelivery = mdl.CmpltDelivery;
                obj.ODT = mdl.ODT;
                obj.UDT = mdl.UDT;
                obj.DelyPrior = mdl.DelyPrior;
                obj.POD = mdl.POD;
                obj.ShpngCndtn = mdl.ShpngCndtn;
                obj.Delyplant = mdl.Delyplant;
                obj.CCA = mdl.CCA;
                obj.IncoTerms1 = mdl.IncoTerms1;
                obj.IncoTerms2 = mdl.IncoTerms2;
                obj.Payterm = mdl.Payterm;
                obj.AAGC = mdl.AAGC;

                obj.OrdrComb = mdl.OrdrComb;
                obj.OrdrProb = mdl.OrdrProb;
                obj.PartnerNo = mdl.PartnerNo;
                obj.PartnerFunc = mdl.PartnerFunc;
                obj.PartnerDesc = mdl.PartnerDesc;

                obj.CPP = mdl.CPP;
                obj.Custgrp = mdl.Custgrp;
                obj.ABC = mdl.ABC;
                obj.Pricegrp = mdl.Pricegrp;
                obj.Pricelst = mdl.Pricelst;
                obj.CSG = mdl.CSG;

                obj.Validto = mdl.Validto;
                obj.Validfrom = mdl.Validfrom;
                list.Add(obj);
            }

            return this.Json(list, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetvenList()
        {
            var Gen = _BusinessPartnerService.VenList();
            var list = new List<BPVendorModel>();
            foreach (Prosol_BPVendorModel mdl in Gen)
            {
                var obj = new BPVendorModel();
                obj._id = mdl._id.ToString();
                obj.BPNumber = mdl.BPNumber;
                obj.VendorNo = mdl.VendorNo;
                obj.AccGrp = mdl.AccGrp;
                obj.PurchOrg = mdl.PurchOrg;
                obj.Payterms = mdl.Payterms;
                obj.Plant = mdl.Plant;
                obj.Existvend_num = mdl.Existvend_num;
                obj.VendorType = mdl.VendorType;
                obj.Status = mdl.Status;
                obj.AuthGrp = mdl.AuthGrp;
                obj.PlntRelvnt = mdl.PlntRelvnt;
                obj.FactoryCal = mdl.FactoryCal;
                obj.CompanyCode = mdl.CompanyCode;
                obj.CR_number = mdl.CR_number;
                obj.Payment_method = mdl.Payment_method;
                obj.Communication_Language = mdl.Communication_Language;
                obj.RecACs = mdl.RecACs;
                obj.AltPayee = mdl.AltPayee;
                obj.TaxType = mdl.TaxType;
                obj.TaxBase = mdl.TaxBase;
                obj.TaxNo = mdl.TaxNo;
                obj.VatNo = mdl.VatNo;
                obj.TOB = mdl.TOB;
                obj.TOI = mdl.TOI;
                obj.PurchOrg = mdl.PurchOrg;
                obj.PurchaseGrp = mdl.PurchaseGrp;
                obj.POCur = mdl.POCur;
                obj.Incoterms1 = mdl.Incoterms1;
                obj.Incoterms2 = mdl.Incoterms2;
                obj.GRBsdIV = mdl.GRBsdIV;
                obj.OAReq = mdl.OAReq;
                obj.PDT = mdl.PDT;
                obj.PricingSchema = mdl.PricingSchema;
                obj.ERS = mdl.ERS;
                obj.BankAccount = mdl.BankAccount;
                obj.BankBranch = mdl.BankBranch;
                obj.BankName = mdl.BankName;
                obj.BANK_ADDRESS_LINE_1 = mdl.BANK_ADDRESS_LINE_1;
                obj.BANK_ADDRESS_LINE_2 = mdl.BANK_ADDRESS_LINE_2;
                obj.City = mdl.City;
                obj.Bank_Key = mdl.Bank_Key;
                obj.Bank_Country = mdl.Bank_Country;
                obj.HouseBank = mdl.HouseBank;
                // obj.PartnerFuncs = mdl.PartnerFuncs;

                list.Add(obj);
            }

            return this.Json(list, JsonRequestBehavior.AllowGet);

        }
    }
}

    