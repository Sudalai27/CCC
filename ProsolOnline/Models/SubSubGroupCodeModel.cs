using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class SubSubGroupCodeModel
    {
        public string _id { get; set; }
        public string MainGroupCode { get; set; }
        public string MainGroupTitle { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupTitle { get; set; }
        public string SubSubGroupCode { get; set; }
        public string SubSubGroupTitle { get; set; }
    }
}