using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ServiceCreation
    {
        public string _id { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Servicecategorycode { get; set; }
        public string Servicecategoryname { get; set; }
        public string Servicegroupcode { get; set; }
        public string Servicegroupname { get; set; }

        public string ServiceUomcode { get; set; }
        public string ServiceUomname { get; set; }
        public string Servicevaluationcode { get; set; }
        public string Servicevaluationname { get; set; }
        public string Servicemaincode { get; set; }
        public string Servicemaindiscription { get; set; }
        public string Servicesubcode { get; set; }
        public string Servicesubdiscription { get; set; }
        public string Servicesubsubcode { get; set; }
        public string Servicesubsubdiscription { get; set; }
        public List<ServiceAttributeList> Attributess { get; set; }




    }
}