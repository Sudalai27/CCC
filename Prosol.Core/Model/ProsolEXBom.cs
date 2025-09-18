using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class ProsolEXBom
    {
        public string Eqno { get; set; }
        public string Equipment_Text { get; set; }
        public string Flo { get; set; }
        public string FlocText { get; set; }
        public string Object_Type { get; set; }
        public string MNFR { get; set; }
        public string MNFR_model { get; set; }
        public string MNFR_serial { get; set; }
        public string CostructID_1 { get; set; }
        public string CostructID_1_short { get; set; }
   //     public string CostructID1cat { get; set; }
     //   public string CostructID1Instqty { get; set; }
        public string CostructID_2 { get; set; }
        public string CostructID_2_short { get; set; }
  //      public string CostructID2cat { get; set; }
   //     public string CostructID2Instqty { get; set; }
        public string CostructID_3 { get; set; }
        public string CostructID_3_short { get; set; }
  //      public string CostructID3cat { get; set; }
   //     public string CostructID3Instqty { get; set; }
        public string CostructID_4 { get; set; }
        public string CostructID_4_short { get; set; }
        //     public string CostructID4cat { get; set; }
        //     public string CostructID4Instqty { get; set; }
        public string Bom_Spare_Id { get; set; }
        public string Short { get; set; }
        public string Category { get; set; }
        public string Part_Quantity { get; set; }


    }
}
