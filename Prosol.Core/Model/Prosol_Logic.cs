using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_Logic
    {
        public ObjectId _id { get; set; }

        public string Noun { get; set; }
        public string Modifier { get; set; }

        public string AttributeName1 { get; set; }
        public string Value1 { get; set; }
        public string Unitname1 { get; set; }
        public string AttributeName2 { get; set; }
        public string Value2 { get; set; }
        public string Unitname2 { get; set; }
        public string AttributeName3 { get; set; }
        public string Value3 { get; set; }
        public string Unitname3 { get; set; }
        public string AttributeName4 { get; set; }
        public string Value4 { get; set; }
        public string Unitname4 { get; set; }


    }
}
