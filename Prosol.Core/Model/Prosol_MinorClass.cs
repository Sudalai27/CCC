using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_MinorClass
    {
        public ObjectId _id { get; set; }
        public string Business_id { get; set; }
        public string Major_id { get; set; }
        public string MinorCode { get; set; }
        public string MinorClass { get; set; }

        public bool IsActive { get; set; }
    }
}
