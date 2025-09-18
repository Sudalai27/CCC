using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProsolOnline.Models
{
    public class ImportModel
    {
        public string _id { get; set; }

        public string Itemcode { get; set; }

        public string Materialcode { get; set; }
        public string Legacy { get; set; }

        public string Noun { get; set; }

        public string Modifier { get; set; }

        public string PhysicalVerification { get; set; }
        public string UOM { get; set; }
        public string username { get; set; }

        public string Type { get; set; }

    }
    }








