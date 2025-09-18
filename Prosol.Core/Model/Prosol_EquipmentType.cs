using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_EquipmentType
    {
        public ObjectId _id { get; set; }
        public string EquClass_Id { get; set; }
        public string EquTypeCode { get; set; }
        public string EquipmentType { get; set; }
        public bool IsActive { get; set; }

    }
}
