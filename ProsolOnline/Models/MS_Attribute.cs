using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class MS_Attribute
    {
        public string _id { get; set; }
        public string Noun { get; set; }        public string Modifier { get; set; }
        public string MainCode { get; set; }
        public string MainDiscription { get; set; }
        public string SubCode { get; set; }
        public string SubDiscription { get; set; }

        public string Attributes { get; set; }
        public string[] Values { get; set; }

        public int Sequence { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string Service { get; set; }

        public string Activity { get; set; }
    }
}