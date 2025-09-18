using Excel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core
{
    public class BusinessPartnerService : IBusinessPartner
    {
        private readonly IRepository<Prosol_BPMaster> _BPMasterRepository;
        private readonly IRepository<Prosol_BPCustomerModel> _BPCustomerRepository;
        private readonly IRepository<Prosol_BPGenralModel> _BPGenralRepository;
        private readonly IRepository<Prosol_BPVendorModel> _BPVendorModelRepository;
        public BusinessPartnerService(IRepository<Prosol_BPMaster> BPMasterRepository, IRepository<Prosol_BPCustomerModel> BPCustomerRepository, 
            IRepository<Prosol_BPGenralModel> BPGenralRepository, IRepository<Prosol_BPVendorModel> BPVendorModelRepository)
        {
            this._BPMasterRepository = BPMasterRepository;
            this._BPCustomerRepository = BPCustomerRepository;
            this._BPGenralRepository = BPGenralRepository;
            this._BPVendorModelRepository = BPVendorModelRepository;

        }

        public bool InsertData(Prosol_BPMaster data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.And(Query.EQ("Label", data.Label), (Query.Or(Query.EQ("Code", data.Code), Query.EQ("Title", data.Title))));
            var vn = _BPMasterRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _BPMasterRepository.Add(data);
            }
            return res;

        }
        public IEnumerable<Prosol_BPMaster> GetDataList(string Label)
        {
            var query = Query.And(Query.EQ("Label", Label));
            var lst = _BPMasterRepository.FindAll(query);
            return lst;

        }
        //public bool DelData(string id)
        //{
        //    var query = Query.EQ("_id", new ObjectId(id));
        //    var Updae = Update.Set("Islive", false);
        //    var flg = UpdateFlags.Upsert;
        //    var res = _BPMasterRepository.Update(query, Updae, flg);
        //    return res;
        //}
        public IEnumerable<Prosol_BPMaster> GetMaster()
        {
            var lst = _BPMasterRepository.FindAll();
            return lst;

        }
        public IEnumerable<Prosol_BPGenralModel> GenList()
        {
            var lst = _BPGenralRepository.FindAll();
            return lst;

        }
        public IEnumerable<Prosol_BPCustomerModel> CustList()
        {
            var lst = _BPCustomerRepository.FindAll();
            return lst;

        }
        public IEnumerable<Prosol_BPVendorModel> VenList()
        {
            var lst = _BPVendorModelRepository.FindAll();
            return lst;

        }
        public bool DisableData(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _BPMasterRepository.Update(query, Updae, flg);
            return res;

        }

        public bool CreateGen(Prosol_BPGenralModel data)
        {
            bool res = false;
          
            res = _BPGenralRepository.Add(data);
           
            return res;

        }
        public bool CreateCust(Prosol_BPCustomerModel data)
        {
            bool res = false;

            res = _BPCustomerRepository.Add(data);

            return res;
        }
        public bool CreateVen(Prosol_BPVendorModel data)
        {
            bool res = false;

            res = _BPVendorModelRepository.Add(data);

            return res;

        }
        public bool DelData(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));

            var res = _BPGenralRepository.Delete(query);
            return res;
        }
        public bool custdel(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));

            var res = _BPCustomerRepository.Delete(query);
            return res;
        }
        public bool venDel(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));

            var res = _BPVendorModelRepository.Delete(query);
            return res;
        }
        public bool UpdateGen(Prosol_BPGenralModel mdl, string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));

            var obj = _BPGenralRepository.FindOne(query);
            // var x = new Prosol_BPGenralModel();
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
            var result1 = _BPGenralRepository.Add(obj);
            return result1;

        }
        public bool UpdateVen(Prosol_BPVendorModel mdl, string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));

            var obj = _BPVendorModelRepository.FindOne(query);
            // var x = new Prosol_BPGenralModel();
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
            var result1 = _BPVendorModelRepository.Add(obj);
            return result1;

        }
        public bool UpdateCust(Prosol_BPCustomerModel mdl, string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));

            var obj = _BPCustomerRepository.FindOne(query);
            // var x = new Prosol_BPGenralModel();
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
            var result1 = _BPCustomerRepository.Add(obj);
            return result1;

        }
    }

    
 
}
