using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Business
    {
        public ObjectId _id { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessName { get; set; }
        public bool IsActive { get; set; }
        public bool Islive { get; set; }
    }
}
