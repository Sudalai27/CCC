using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ValuesList
    {
        public int slno { set; get; }
      
        public string AttributeName { set; get; }
       
        public string Values { set; get; }
    }
}