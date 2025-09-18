using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_Sequence
    {

        public ObjectId _id { get; set; }

        public string Category { get; set; }
        public int CatId { get; set; }
        public string Description { get; set; }
        public int Seq { get; set; }
        public string Required { get; set; }
        public string Separator { get; set; }
        public int ShortLength { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
