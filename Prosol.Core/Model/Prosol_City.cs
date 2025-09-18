using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_City
    {

        public ObjectId _id { get; set; }
        public string Region_Id { get; set; }
        public string CityCode { get; set; }
        public string City { get; set; }

        public bool IsActive { get; set; }
    }
}
