using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
   public class Prosol_ServiceGroup
    {
        public ObjectId _id { get; set; }
       // public string SAPGroupcode { get; set; }
       
        public string ServiceGroupcode { get; set; }
        public string ServiceGroupname { get; set; }
        public string SeviceCategorycode { get; set; }
        public string SeviceCategoryname { get; set; }
        public bool Islive { get; set; }
    }
}
