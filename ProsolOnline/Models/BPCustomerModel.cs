using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class BPCustomerModel
    {

        public string _id { get; set; }
        public string BPNumber { get; set; }
        public string Companycode { get; set; }
        public string Salesorg { get; set; }
        public string DistChan { get; set; }
        public string Divison { get; set; }
        public string CustNo { get; set; }
        public string Bankkey { get; set; }
        public string BankAcc { get; set; }
        public string bankCountry { get; set; }
        public string Cntrlkey { get; set; }
        public string PBT { get; set; }
        public string AccNo { get; set; }
        public string Reference { get; set; }
        public string AccHldr { get; set; }
        public string ReconAcc { get; set; }
        public string PaymntTerms { get; set; }
        public string CmpltDelivery { get; set; }
        public string ODT { get; set; }
        public string UDT { get; set; }
        public string DelyPrior { get; set; }
        public string POD { get; set; }
        public string ShpngCndtn { get; set; }
        public string Delyplant { get; set; }
        public string CCA { get; set; }
        public string IncoTerms1 { get; set; }
        public string IncoTerms2 { get; set; }
        public string Payterm { get; set; }
        public string AAGC { get; set; }
        public string CustCur { get; set; }
        public string OrdrComb { get; set; }
        public string OrdrProb { get; set; }
        public string PartnerNo { get; set; }
        public string PartnerFunc { get; set; }
        public string PartnerDesc { get; set; }
        public string SalesOfc { get; set; }
        public string SalesGrp { get; set; }
        public string CPP { get; set; }
        public string Custgrp { get; set; }
        public string ABC { get; set; }
        public string Pricegrp { get; set; }
        public string Pricelst { get; set; }
        public string CSG { get; set; }
        public string Validfrom { get; set; }
        public string Validto { get; set; }
        public List<PartnerFun> PartnerFuncs { get; set; }
    }
    
}