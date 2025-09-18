using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetRegion
    {
        public string id { get; set; }
        public string RegionCode { get; set; }
        public string Region { get; set; }

        public bool IsActive { get; set; }
    }
}