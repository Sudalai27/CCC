using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetMinorClass
    {
        public string id { get; set; }
        public string Business_Id { get; set; }
        public string Major_id { get; set; }
        public string MinorCode { get; set; }
        public string MinorClass { get; set; }

        public bool IsActive { get; set; }
    }
}