using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ServiceValuation
    {
        public string _id { get; set; }
        public string ServiceValuationcode { get; set; }
        public string ServiceValuationname { get; set; }
        public bool Islive { get; set; }
    }
}