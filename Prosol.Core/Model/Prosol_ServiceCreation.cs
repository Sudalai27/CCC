using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
  public class Prosol_ServiceCreation
    {
        public ObjectId _id { get; set; }
        public string RequestId { get; set; }
        public string ServiceCode { get; set; }
        public string Legacy { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceCategoryCode { get; set; }
        public string ServiceGroupName { get; set; }
        public string ServiceGroupCode { get; set; }
        public string UomName { get; set; }
        public string UomCode { get; set; }
       
        public string RequesterId { get; set; }
        public string Requestername { get; set; }
        public DateTime RequestedOn { get; set; }
        public Prosol_UpdatedBy Cleanser { get; set; }
        public Prosol_UpdatedBy Reviewer { get; set; }
        public Prosol_UpdatedBy Releaser { get; set; }
        public string ServiceStatus { get; set; }
        public string RequestStatus { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime RejectedOn { get; set; }
        public string Reason_rejection { get; set; }


        //----

        public string Categorycode { get; set; }
        public string Categoryname { get; set; }
        public string Groupcode { get; set; }
        public string Groupname { get; set; }
        public string Uomcode { get; set; }
        public string Uomname { get; set; }
        public string Valuationcode { get; set; }
        public string ValuationTitle { get; set; }
        public string MainCode { get; set; }
        public string MainCodeTitle { get; set; }
        public string SubCode { get; set; }
        public string subCodeTitle { get; set; }
        public string SubSubCode { get; set; }
        public string SubSubCodeTitle { get; set; }
        public List<Prosol_ServiceAttributeList> Characteristics { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }   
        public DateTime UpdatedOn { get; set; }
        public Prosol_UpdatedBy RejectedBy { get; set; }
        public string Reject_reason { get; set; }
       

    }
}
