using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
    public class Prosol_Duplicate
    {
        public ObjectId _id { get; set; }

        public string Itemcode { get; set; }
        public string Materialcode { get; set; }
        public string Legacy { get; set; }

        public string Noun { get; set; }

        public string Modifier { get; set; }

        public string Legacy2 { get; set; }

        public string UOM { get; set; }

        public string username { get; set; }

    }
}







