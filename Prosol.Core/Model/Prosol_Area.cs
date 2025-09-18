using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Area
    {
        public ObjectId _id { get; set; }
        public string Region_Id { get; set; }

        public string City_Id { get; set; }
        public string AreaCode { get; set; }
        public string Area { get; set; }

        public bool IsActive { get; set; }
    }
}
