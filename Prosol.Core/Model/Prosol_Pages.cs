using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Pages
    {
        public ObjectId _id { get; set; }
       
        public string Pages { get; set; }

        public string Status { get; set; }

        public string Mainmenu { get; set; }

    }
}
