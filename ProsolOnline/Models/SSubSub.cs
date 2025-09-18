using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class SSubSub
    {
        public string _id { get; set; }
        public string MainCode { get; set; }
        public string MainDiscription { get; set; }
        public string SubCode { get; set; }
        public string SubDiscription { get; set; }
        public string SubSubCode { get; set; }
        public string SubSubDiscription { get; set; }
        public bool Islive { get; set; }

    }
}