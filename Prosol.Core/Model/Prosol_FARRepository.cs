using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
    public class Prosol_FARRepository
    {
        public ObjectId _id { get; set; }
        public string Label { get; set; }
        public string FARId { get; set; }
        public string Region { get; set; }
        public string AssetDesc { get; set; }
        public bool Islive { get; set; }
    }
}