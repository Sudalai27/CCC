using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_ServiceCategory
    {
        public ObjectId _id { get; set; }
        //public string SAPCategorycode { get; set; }
        public string SeviceCategorycode { get; set; }
        public string SeviceCategoryname { get; set; }
        public bool Islive { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
