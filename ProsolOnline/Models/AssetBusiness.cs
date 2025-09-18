using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetBusiness
    {
        public string id { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessName { get; set; }
        public bool IsActive { get; set; }
    }
}