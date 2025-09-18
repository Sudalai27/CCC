using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_Location
    {

        public ObjectId _id { get; set; }
        public string Area_Id { get; set; }
        public string Location { get; set; }
        public string LocationCode { get; set; }
        public string LocationHierarchy { get; set; }
        public bool Islive { get; set; }
        public bool IsActive { get; set; }

    }
}
