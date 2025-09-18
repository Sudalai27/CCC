using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetEquipType
    {
        public string id { get; set; }
        public string EquClass_Id { get; set; }
        public string EquTypeCode { get; set; }
        public string EquipmentType { get; set; }
        public bool IsActive { get; set; }
    }
}