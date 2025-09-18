using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ProsolOnline.Models
{
    public class PlantModel
    {

        public string _id { get; set; }
        public string Plantcode { get; set; }
        public string Plantname { get; set; }
        public bool Islive { get; set; }
    }
}