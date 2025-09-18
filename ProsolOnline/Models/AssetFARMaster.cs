using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetFARMaster
    {
        public string _id { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool Islive { get; set; }
    }
}