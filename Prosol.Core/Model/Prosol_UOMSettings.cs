using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_UOMSettings
    {

        public ObjectId _id { get; set; }
        public string Short_space { get; set; }

        public string Long_space { get; set; }

        public string Short_required { get; set; }
    }
}
