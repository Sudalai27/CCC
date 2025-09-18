using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{

   public class Prosol_Plants
    {
        public ObjectId _id { get; set; }

        public string Plantcode { get; set; }
        public string Plantname { get; set; }
        public bool Islive { get; set; }

    }
}
