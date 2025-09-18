using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
  public class Prosol_ServiceCharacteristicValue
    {
        public ObjectId _id { get; set; }
        public string CharacteristicValue { get; set; }
        public string ValueAbbreviation { get; set; }
        public bool Islive { get; set; }
    }
}
