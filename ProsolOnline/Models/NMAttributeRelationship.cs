using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class NMAttributeRelationship
    {
        public string _id { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public List<Prosol_KeyAttribute> Characteristics { get; set; }
        // public string characteristic { get; set; }
      //  public string Value { get; set; }
       // public string Uom { get; set; }
        public string KeyAttribute { get; set; }
        public string KeyValue { get; set; }

    }
}