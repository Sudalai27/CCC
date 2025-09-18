using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AttributeList
    {
       // public string _id { get; set; }       
        public string Characteristic { get; set; }
        public string Value { get; set; }  
        public string UOM { get; set; }
        public int Squence { get; set; }
        public int ShortSquence { get; set; }
        public string Source { get; set; }
        public string SourceUrl { get; set; }
        public bool Approve { get; set; }
        public string Abbrevated { get; set; }

        public string[] Uom { get; set; }
        public string UomMandatory { get; set; }

    }
}