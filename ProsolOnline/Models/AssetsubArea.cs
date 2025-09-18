using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetsubArea
    {
        public string id { get; set; }
        public string Region_Id{ get; set; }
        public string City_Id { get; set; }
        public string Area_Id { get; set; }
        public string SubAreaCode { get; set; }
        public string SubArea { get; set; }

        public bool IsActive { get; set; }
    }
}