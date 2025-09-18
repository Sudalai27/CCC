using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Abbrevate
    {
        public ObjectId _id { get; set; }

        public string Value { get; set; }
        public string Abbrevated { get; set; }
        public string vunit { get; set; }
        public string Equivalent { get; set; }
        public string eunit { get; set; }
        public string LikelyWords { get; set; }
        public string User { get; set; }
        

        public DateTime? UpdatedOn { get; set; }

        public string Approved { get; set; }

        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
    }
}
