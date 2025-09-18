using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_SMainCode
    {
        public ObjectId _id { get; set; }
        public string SeviceCategorycode { get; set; }
        public string SeviceCategoryname { get; set; }
        public string MainCode { get; set; }
        public string MainDiscription { get; set; }
        public bool Islive { get; set; }

    }
}
