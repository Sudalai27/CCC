using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class CatAssetModel
    {
        public string _id { get; set; }
        //Technical
        public string UniqueId { get; set; }
        public string AssetNo { get; set; }
        public string EquipmentNo { get; set; }
        public string pvAssetNo { get; set; }
        public string AssetQRCode { get; set; }
        public string Description { get; set; }
        public string Description_ { get; set; }
        public string Status { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public string category { get; set; }
        public string AdditionalInfo { get; set; }
        public string AdditionalNotes { get; set; }

        public List<Asset_AttributeList> Characteristics { get; set; }
        public Prosol_UpdatedBy User { get; set; }
        public Prosol_UpdatedBy Catalogue { get; set; }
        public Prosol_UpdatedBy Review { get; set; }
        public Prosol_UpdatedBy Release { get; set; }

        public string Rework { get; set; }
        public string Rework_Remarks { get; set; }
        public string Cat_Remarks { get; set; }
        public string assetConditionRemarks { get; set; }
        public int ItemStatus { get; set; }
        public string PVstatus { get; set; }


        public Prosol_UpdatedBy PVuser { get; set; }

        public AssetEquipment Equipment { get; set; }
        public AssetImages AssetImages { get; set; }
        public AssetCondition AssetCondition { get; set; }
        public AssetGIS GIS { get; set; }

        public string Remarks { get; set; }
        public string MaximoAssetInfo { get; set; }
        public string OldTagNo { get; set; }
        public string NewTagNo { get; set; }
        public string VirtualTagNo { get; set; }
        public string Equipment_Short { get; set; }
        public string Equipment_Long { get; set; }
        public string EnrichedValue { get; set; }
        public string MissingValue { get; set; }
        public string RepeatedValue { get; set; }
        public string Attachment { get; set; }
        public string Idle_Operational { get; set; }
        public bool Exchk { get; set; }
        public string Soureurl { get; set; }
        public BOM Bom { get; set; }
        public string Unspsc { get; set; }
        public bool Doc_Availability { get; set; }

        //ERP

        public string TechIdentNo { get; set; }
        public string Func_Location { get; set; }
        public string Plant { get; set; }
        public string Org_Code { get; set; }
        public string CostCenter { get; set; }
        public string CostCenter_Desc { get; set; }
        public string ABC_Indicator { get; set; }
        public string Equ_Category { get; set; }
        public string MainWorkCenter { get; set; }
        public string ObjType { get; set; }
        public string Parent { get; set; }
        public string System { get; set; }
        public string PartNo { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string ModelYear { get; set; }
        public string Manufacturer { get; set; }
        public string MfrCountry { get; set; }
        public string MfrYear { get; set; }
        public string AssCondition { get; set; }
        public string PID_Number { get; set; }
        public string PID_Desc { get; set; }
        public string Section_Number { get; set; }
        public string Section_Desc { get; set; }
        public string Discipline { get; set; }

    }
    public class AssetGIS
    {
        public string LattitudeStart { get; set; }
        public string LattitudeEnd { get; set; }
        public string LongitudeStart { get; set; }
        public string LongitudeEnd { get; set; }
        public string Lat_LongLength { get; set; }

    }
    public class AssetImages
    {
        public string[] AssetImage { get; set; }
        public string[] MatImgs { get; set; }
        public string[] NamePlateImge { get; set; }
        public string[] NamePlateImgeTwo { get; set; }
        public string[] NamePlateText { get; set; }
        public string[] NamePlateTextTwo { get; set; }
        public string[] NameplateImgs { get; set; }
        public string[] NewTagImage { get; set; }
        public string[] OldTagImage { get; set; }
        public string[] VirtualTagImage { get; set; }
    }
    public class Building
    {
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string[] BuildingImage { get; set; }
    }
    public class AssetCondition
    {
        public string Corrosion { get; set; }
        public string Damage { get; set; }
        public string Leakage { get; set; }
        public string Vibration { get; set; }
        public string Temparature { get; set; }
        public string Smell { get; set; }
        public string Noise { get; set; }
        public string Rank { get; set; }
        public string Asset_Condition { get; set; }

        public string[] CorrosionImage { get; set; }
        public string[] DamageImage { get; set; }
        public string[] LeakageImage { get; set; }
    }
    public class CLF_Remarks
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
    public class BOM_
    {

        public string BOMId { get; set; }
        public string BOMDescription { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
        public List<ASSEMBLY_> Assembly { get; set; }
        public List<MAT_> Mat { get; set; }

    }
    public class ASSEMBLY_
    {

        public string AssemblyId { get; set; }
        public string AssemblyDescription { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }

    }
    public class MAT_
    {

        public string Materialcode { get; set; }
        public string MaterialDescription { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
        public string Status { get; set; }

    }
}