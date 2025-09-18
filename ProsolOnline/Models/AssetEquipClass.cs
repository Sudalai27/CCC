using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetEquipClass
    {
        public string id { get; set; }
        public string EquipmentClass { get; set; }
        public string EquipmentCode { get; set; }
        public bool IsActive { get; set; }
    }
}