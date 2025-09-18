using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetTAR
    {

        public string _id { get; set; }

        //TAR
        public string SiteId { get; set; }
        public string AssetNo { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string LocationHierarchy { get; set; }
        public string Cluster { get; set; }
        public string HighLevelLocation { get; set; }
        public string ClassificationHierarchyDesc { get; set; }
        public string AggregatedClassSpecAttr { get; set; }
        public string Status { get; set; }
        public string AssetType { get; set; }
        public string FailureCode { get; set; }
        public string Priority { get; set; }
        public string FixedAssetNo { get; set; }
        public string MaintaineBy { get; set; }
        public string ReportGroup { get; set; }
        public string GL_Account { get; set; }
        public string CapexGL_Account { get; set; }
        public string WarrentyExpDate { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string ModelYear { get; set; }
        public string LegacyAssetNo { get; set; }
        public string AssCondition { get; set; }
        public string OwnedBySite { get; set; }
        public string Vendor { get; set; }
        public string Manufacturer { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchasePrice { get; set; }
        public string InstallDate { get; set; }
        public string PO_Contract { get; set; }
        public string LoadCertExpDate { get; set; }
        public string CalCertExpDate { get; set; }
        public string CertExpDate { get; set; }
        public string TrafficCertExpDate { get; set; }
        public string ManagedBy { get; set; }

    }
}