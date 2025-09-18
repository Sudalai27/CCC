using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prosol.Core.Model;

namespace ProsolOnline.Models
{
    public class Service_Request
    {
        public string _id { get; set; }
        public string RequestId { get; set; }
        public string ItemId { get; set; }
        public string ServiceCode { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public string Legacy { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceCategoryCode { get; set; }
        public string ServiceGroupName { get; set; }
        public string ServiceGroupCode { get; set; }
        public string UomName { get; set; }
        public string UomCode { get; set; }
        public string Valuationcode { get; set; }
        public string ValuationName { get; set; }
        public string MainCode { get; set; }
        public string MainCodeName { get; set; }
        public string SubCode { get; set; }
        public string SubCodeName { get; set; }
        public string SubSubCode { get; set; }
        public string SubSubCodeName { get; set; }
        public List<Prosol_ServiceAttributeList> Characteristics { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public DateTime UpdatedOn { get; set; }
        //public string RequesterId { get; set; }
        //public string Requestername { get; set; }
        //public DateTime RequestedOn { get; set; }
        public Prosol_UpdatedBy Last_updatedBy { get; set; }
        public Prosol_UpdatedBy requester { get; set; }
        public Prosol_UpdatedBy Cleanserr { get; set; }
        public Prosol_UpdatedBy Reviewer { get; set; }
        public Prosol_UpdatedBy Releaser { get; set; }
        public string ServiceStatus { get; set; }
        public string RequestStatus { get; set; }
        public Prosol_Reject_Service RejectedBy { get; set; }
        public string Reject_reason { get; set; }
        public string Cleanser { get; set; }
        public string last_updated_date { get; set; }
        public string Remarks { get; set; }
        public string Class { get; set; }
        public string ClassTitle { get; set; }
        public string Unspsc { get; set; }
        public string CommodityTitle { get; set; }
        public string parent { get; set; }
        public string[] Child { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public DateTime? Clarification_On { get; set; }
        public DateTime? Approvedon { get; set; }
        public string approver { get; set; }

    }
}