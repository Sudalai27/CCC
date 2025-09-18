using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetCity
    {
        public string id { get; set; }
        public string Region_Id { get; set; }
        public string CityCode { get; set; }
        public string City { get; set; }

        public bool IsActive { get; set; }
    }
}