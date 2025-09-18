using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_MajorClass
    {
        public ObjectId _id { get; set; }
        public string Business_id { get; set; }
        public string MajorCode { get; set; }
        public string MajorClass { get; set; }
        public bool IsActive { get; set; }
    }
}
