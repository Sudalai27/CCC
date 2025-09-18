using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Nounscrub
    {

        public ObjectId _id { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public string Scrubnoun { get; set; }
        public string ScrubModifier { get; set; }

    }
}
