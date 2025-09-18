using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AttributeModel
    {

        public string _id { get; set; }

        public string Attribute { get; set; }
        public string[] ValueList { get; set; }
        public string[] UOMList { get; set; }
        public int Validation { get; set; }
    }
}