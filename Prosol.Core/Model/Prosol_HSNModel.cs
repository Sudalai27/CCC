
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_HSNModel
    {
        public ObjectId _id { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public string HSNID { get; set; }
        public string Desc { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
