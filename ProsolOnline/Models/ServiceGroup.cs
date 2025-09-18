using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ServiceGroup
    {
        public string _id { get; set; }
        public string ServiceGroupcode { get; set; }
        public string ServiceGroupname { get; set; }
        public string SeviceCategorycode { get; set; }
        public string SeviceCategoryname { get; set; }
        public bool Islive { get; set; }
    }
}