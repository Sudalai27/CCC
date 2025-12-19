using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
    public class Prosol_FARMaster
    {
        public ObjectId _id { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Parent { get; set; }
        public int Sequence { get; set; }
        public bool Islive { get; set; }
    }
}