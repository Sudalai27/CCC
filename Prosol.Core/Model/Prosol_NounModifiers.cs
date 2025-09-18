using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_NounModifiers
    {

        public ObjectId _id { get; set; }


        public string Noun { get; set; }

        public string NounEqu { get; set; }
        //[Required]
        //[StringLength(10, MinimumLength = 2)]
        //[Display(Name = "Noun Abv")]
        public string Nounabv { get; set; }


        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Noun Definition")]
        public string NounDefinition { get; set; }


        public string Modifier { get; set; }
        //[Required]
        //[StringLength(10, MinimumLength = 2)]
        //[Display(Name = "Modifier Abv")]
        public string ModifierEqu { get; set; }
        public string Modifierabv { get; set; }


        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Modifier Definition")]
        public string ModifierDefinition { get; set; }
        public string Similaritems { get; set; }
        public string ImageId { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public string FileData { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Formatted { get; set; }
        public string RP { get; set; }
        public string[] uomlist { get; set; }
    }
}
