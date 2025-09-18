using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Region
    {
        public ObjectId _id { get; set; }
  
        public string RegionCode { get; set; }
        public string Region { get; set; }

        public bool IsActive { get; set; }
    }
}
