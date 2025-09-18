using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ServiceCategory
    {

        public string _id { get; set; }
      //  public string SAPCategorycode { get; set; }
        public string SeviceCategorycode { get; set; }
        public string SeviceCategoryname { get; set; }

        public bool Islive { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}