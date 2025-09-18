using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetArea
    {
        public string id { get; set; }
        public string Region_Id { get; set; }
        public string City_Id { get; set; }
        public string AreaCode { get; set; }
        public string Area { get; set; }

        public bool IsActive { get; set; }
    }
}