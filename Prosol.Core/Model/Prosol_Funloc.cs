using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_Funloc
    {
        public ObjectId _id { get; set; }

      
        public string FunctLocation { get; set; }

        public string FunctDesc { get; set; }

        public string FunctLocCat { get; set; }

        public string ABCindic { get; set; }
        public string SupFunctLoc
        { get; set; }
        public string Objecttype { get; set; }

        public string Startdate
        { get; set; }
        public string AuthGroup
        { get; set; }
        public string MainWrk { get; set; }
        public string WBSelement { get; set; }

        public string Plantsection
        { get; set; }

        public string Catalogprofile
        { get; set; }

        public string Singleinst
        { get; set; }
        public string Manufacturer
        { get; set; }
        public string ManufCon
        { get; set; }

        public string Modelno { get; set; }

        public string ManufSerialNo
        { get; set; }
        public string FunclocClass1

        { get; set; }

        public string FunclocClass2

        { get; set; }


        public string Asset

        { get; set; }

        public string Comment

        { get; set; }


        //Equipments Bom
        public string TechIdentNo { get; set; }

      //  public string FunctLocation { get; set; }

        public string EquipDesc { get; set; }

        public string EquipCategory { get; set; }

        //    public string Objecttype { get; set; }
        
        public string status { get; set; }
        public string Weight { get; set; }

        public string UOM { get; set; }
        public string Size { get; set; }

        public string AcquisValue
        { get; set; }

        public string AcquistnDat
        { get; set; }

     //   public string Manufacturer
     //   { get; set; }

     //   public string ManufCon
      //  { get; set; }

      //  public string Modelno { get; set; }

        public string ConstructYear
        { get; set; }
        public string ConstructMth

        { get; set; }

        public string ManufPartNo
        { get; set; }

     //   public string ManufSerialNo
     //   { get; set; }

   //     public string AuthGroup
     //   { get; set; }

        public string Startupdate
        { get; set; }

        public string MaintPlant
        { get; set; }
        public string Companycode
        { get; set; }

     //   public string Asset

     //   { get; set; }

        public string Subno

        { get; set; }

        public string ConID

        { get; set; }

        //public string Catalogprofile

        //{ get; set; }
        public string Mainworkcenter

        { get; set; }

        public string mcpcode { get; set; }

        //New
        public string SuperiorLocation { get; set; }
        public string SLDesc { get; set; }
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
        public string Level5 { get; set; }
        public string Level6 { get; set; }
        public string Level7 { get; set; }
        public string Equipment { get; set; }
        public string PrimaryEquipment { get; set; }
        public string SubEquipment1 { get; set; }
        public string SubEquipment2 { get; set; }
        public string SubEquipment3 { get; set; }
        public string SectionNo { get; set; }
        public string Sequence { get; set; }
        public string UniqueId { get; set; }
        public string BOMId { get; set; }
        public bool Islive { get; set; }
    }
}
