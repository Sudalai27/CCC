using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AttrubutesSL
    {
        public string _id { get; set; }
        public string MainCode { get; set; }
        public string MainDiscription { get; set; }
        public string SubCode { get; set; }
        public string SubDiscription { get; set; }
        public string Attributes { get; set; }
        public int Sequence { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Value { get; set; }
    }
}