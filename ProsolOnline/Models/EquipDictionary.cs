using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
    public class EquipDictionary
    {
        public string _id { get; set; }

        public string Noun { get; set; }
        public string Modifier { get; set; }
        public string SpareList { get; set; }
        public string NCP { get; set; }
        public string CP { get; set; }
        public string SpareCategory { get; set; }
    }
}       
