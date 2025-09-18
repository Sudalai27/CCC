using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ProsolOnline.Models
{
    public class NounModifierModel
    {
        public string _id { get; set; }


        [Required]
       // [StringLength(50, MinimumLength = 2)]
        public string Noun { get; set; }
        public string NounEqu { get; set; }
        //  [Required]
        //[StringLength(10, MinimumLength = 2)]
        //[Display(Name = "Noun Abv")]
        public string Nounabv { get; set; }


        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Noun Definition")]
        public string NounDefinition { get; set; }

        [Required]
        //[StringLength(50, MinimumLength = 2)]
        public string Modifier { get; set; }

        public string ModifierEqu { get; set; }
        //[Required]
        //[StringLength(10, MinimumLength = 2)]
        //[Display(Name = "Modifier Abv")]
        public string Modifierabv { get; set; }


        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Modifier Definition")]
        public string ModifierDefinition { get; set; }

        public string Similaritems { get; set; }
        public string ImageId { get; set; }
        
        public string FileData { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public int Formatted { get; set; }

        public string[] uomlist { get; set; }
    }



}