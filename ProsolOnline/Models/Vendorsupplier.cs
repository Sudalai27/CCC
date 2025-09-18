using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class Vendorsupplier
    {
        public int slno { set; get; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string RefNo { get; set; }
        public string RefNoDup { get; set; }
        public string Refflag { get; set; }
        public int s { get; set; }
        public int l { get; set; }
        public string shortmfr { get; set; }

        public bool Valid { get; set; }

    }
}