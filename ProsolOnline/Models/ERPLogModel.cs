using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProsolOnline.Models
{
    public class ERPLogModel
    {
        public string _id { get; set; }

        public string Itemcode { get; set; }
        public string fieldname { get; set; }
        public string oldValue { get; set; }
        public string newvalue { get; set; }
        public string page { get; set; }
        public string module { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
