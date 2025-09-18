using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Master
    {
        public ObjectId _id { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Storagelocationcode { get; set; }
        public string Plantcode { get; set; }
        public bool Islive { get; set; }
        public int Count { get; set; }


    }
}
