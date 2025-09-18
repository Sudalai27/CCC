using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
  public  class Prosol_ServiceUom
    {
        public ObjectId _id { get; set; }
        public string ServiceUomcode { get; set; }
        public string ServiceUomname { get; set; }
        public bool Islive { get; set; }

    }
}
