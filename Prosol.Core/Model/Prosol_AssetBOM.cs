using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
   public class Prosol_AssetBOM
    {
        public ObjectId _id { get; set; }

        //New
        public string BOMId { get; set; }
        public string BOMDesc { get; set; }
        public string BOMLongDesc { get; set; }
        public string AssemblyId { get; set; }
        public string AssemblyDesc { get; set; }
        public string AssemblyLongDesc { get; set; }
        public string ComponentId { get; set; }
        public string ComponentDesc { get; set; }
        public string ComponentLongDesc { get; set; }
        public string Quantity { get; set; }            
        public string Sequence { get; set; }
        public string Category { get; set; }
        public string TechIdentNo { get; set; }
        public string Func_Location { get; set; }

        //
        public string EquipmentId { get; set; }
        public string UniqueId { get; set; }
        public string TagNo { get; set; }
        public string Barcode { get; set; }
        public string OldTag { get; set; }
        public string BOMName { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string UOM { get; set; }
        public string OldTagImg { get; set; }
        public string BOMImg { get; set; }
        public string NamePlateImg { get; set; }
        public string BarCodeImg { get; set; }


    }
}
