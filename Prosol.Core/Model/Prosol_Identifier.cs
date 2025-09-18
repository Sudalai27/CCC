using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Identifier
    {
        public ObjectId _id { get; set; }
        public string Business_Id { get; set; }
        public string MajorClass_Id { get; set; }
        public string fClass_Id { get; set; }
        public string IdentifierCode { get; set; }
        public string Identifier { get; set; }

        public bool IsActive { get; set; }
    }
}
