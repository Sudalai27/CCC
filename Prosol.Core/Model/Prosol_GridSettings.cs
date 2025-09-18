using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_GridSettings
    {
        public ObjectId _id { get; set; }

        public string UserId { get; set; }
        public string ColName { get; set; }
        public string Display { get; set; }
        public bool Visible { get; set; }
    }
}
