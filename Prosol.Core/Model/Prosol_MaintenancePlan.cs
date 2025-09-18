using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_MaintenancePlan
    {
        public ObjectId _id { get; set; }
        public string FunctionLocation { get; set; }
             public string GroupCount { get; set; }
             public string Type { get; set; }
             public string Equipment { get; set; }
             public string Model { get; set; }
             public string Make { get; set; }
             public string UpdatedBy { get; set; }
             public string UpdatedDate { get; set; }
    }
}
