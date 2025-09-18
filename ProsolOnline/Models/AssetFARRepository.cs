using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetFARRepository
    {
        public string _id { get; set; }
        public string Label { get; set; }
        public string FARId { get; set; }
        public string Region { get; set; }
        public string AssetDesc { get; set; }
        public bool Islive { get; set; }
    }
}