using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_BPVendorModel
    {
        public ObjectId _id { get; set; }
        public string BPNumber { get; set; }
        public string VendorNo { get; set; }
        public string VendorCategory { get; set; }
        public string VendorType { get; set; }
        public string Status { get; set; }
        public string AuthGrp { get; set; }
        public string AccGrp { get; set; }
        public string Plant { get; set; }
        public string Existvend_num { get; set; }
        public string PlntRelvnt { get; set; }
        public string FactoryCal { get; set; }
        public string CompanyCode { get; set; }
        public string CR_number { get; set; }
        public string Payment_method { get; set; }
        public string Communication_Language { get; set; }
        public string RecACs { get; set; }
        public string Payterms { get; set; }
        public string AltPayee { get; set; }
        public string TaxType { get; set; }
        public string TaxBase { get; set; }
        public string TaxNo { get; set; }
        public string VatNo { get; set; }
        public string TOB { get; set; }
        public string TOI { get; set; }
        public string PurchOrg { get; set; }
        public string POCur { get; set; }
        public string Incoterms1 { get; set; }
        public string Incoterms2 { get; set; }
        public string GRBsdIV { get; set; }
        public string OAReq { get; set; }
        public string PurchaseGrp { get; set; }
        public string PDT { get; set; }
        public string PricingSchema { get; set; }
        public string ERS { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string IBan { get; set; }
        public string BankBranch { get; set; }
        public string BANK_ADDRESS_LINE_1 { get; set; }
        public string BANK_ADDRESS_LINE_2 { get; set; }
        public string City { get; set; }
        public string HouseBank { get; set; }
        public string Bank_Country { get; set; }
        public string Bank_Key { get; set; }
        public List<PartnerFunn> PartnerFuncs { get; set; }
    }
    public class PartnerFunn
    {
        public string PartnerFunc { get; set; }
        public string PartnerNo { get; set; }
        public string PartnerDesc { get; set; }

    }
}
