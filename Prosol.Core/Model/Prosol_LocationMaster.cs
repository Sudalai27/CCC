using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_LocationMaster
    {
        public ObjectId _id { get; set; }

        public string User { get; set; }

        public string Business { get; set; }
        public string BusinessCode { get; set; }

        public string Major { get; set; }

        public string Minor { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string RegionCode { get; set; }
        public string Area { get; set; }
        public string AreaCode { get; set; }

        public string SubArea { get; set; }
        public string SubAreaCode { get; set; }
        public string Function { get; set; }
        public string FunctionCode { get; set; }


        public string Identifier { get; set; }
        public string IdentifierCode { get; set; }
        //   public string Equipment { get; set; }
        public string EquipmentClass{ get; set; }
      
        public string EquipmentType { get; set; }
        public string EquipmentTypeCode { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteType { get; set; }
        public string CompanyCode { get; set; }
        public string Objecttype { get; set; }
        public string EquipmentDesc { get; set; }
        public string Location { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Islive { get; set; }
    }
}
