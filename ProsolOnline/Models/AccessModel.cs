using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

using System.ComponentModel.DataAnnotations;

namespace ProsolOnline.Models
{
    public class AccessModel
    {
        public string _id { get; set; }

        public string Userid { get; set; }
        public string Pages { get; set; }

        public DateTime? Createdon { get; set; }

        public string Status { get; set; }
    }
}