using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ServiceCharacteristicValue
    {
        public string _id { get; set; }
        public string CharacteristicValue { get; set; }
        public string ValueAbbreviation { get; set; }
        public bool Islive { get; set; }
    }
}