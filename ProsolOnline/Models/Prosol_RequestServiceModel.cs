using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class Prosol_RequestServiceModel
    {
        public string id { get; set; }
        public string requestId { get; set; }
        public string itemId { get; set; }
        public string ServiceDiscription { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceGroup { get; set; }
        public string UOM { get; set; }
  
        public string requester { get; set; }
        public DateTime requestedOn { get; set; }
        public DateTime? Approvedon { get; set; }
        public string approver { get; set; }
        public string itemStatus { get; set; }
        public string requestStatus { get; set; }
        public DateTime approvedOn { get; set; }
        public DateTime rejectedOn { get; set; }
        public DateTime? Rejectedon { get; set; }


    }
}