using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_EquipmentClass
    {
        public ObjectId _id { get; set; }
        public string EquipmentClass { get; set; }
        public string EquipmentCode { get; set; }
        public bool IsActive { get; set; }

    }
}
