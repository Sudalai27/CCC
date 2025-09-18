using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_RandomPC
    {
        public ObjectId _id { get; set; }
        public string userid { get; set; }
        public int rndm { get; set; }
    }
}
