using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetLocation
    {
        public string id { get; set; }
        public string Area_Id { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }

        public bool IsActive { get; set; }
    }
}