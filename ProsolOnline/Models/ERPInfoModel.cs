using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class ERPInfoModel
    {

        public string _id { get; set; }
        public string Itemcode { get; set; }

        //General
        public string Industrysector { get; set; }
        public string Materialtype { get; set; }
        public string BaseUOP { get; set; }
        public string Unit_issue { get; set; }
        public string AlternateUOM { get; set; }       
        public string Inspectiontype { get; set; }
        public string Inspectioncode { get; set; }       
        public string Division { get; set; }
        public string Salesunit { get; set; }


        public string Industrysector_ { get; set; }
        public string Materialtype_ { get; set; }
        public string BaseUOP_ { get; set; }
        public string Unit_issue_ { get; set; }
        public string AlternateUOM_ { get; set; }
        public string Numerator_ { get; set; }
        public string Denominator_ { get; set; }
        public string Inspectiontype_ { get; set; }
        public string Inspectioncode_ { get; set; }
        public string Oldmaterialno_ { get; set; }
        public string Division_ { get; set; }
        public string Salesunit_ { get; set; }

        //Plant

        public string Plant { get; set; }
        public string ProfitCenter { get; set; }
        public string StorageLocation { get; set; }
        public string StorageLocation2 { get; set; }
        public string StorageLocation3 { get; set; }
        public string StorageLocation4 { get; set; }
        public string StorageLocation5 { get; set; }
        public string StorageBin { get; set; }
        public string StorageBin2 { get; set; }
        public string StorageBin3 { get; set; }
        public string StorageBin4 { get; set; }
        public string StorageBin5 { get; set; }
        public string ValuationClass { get; set; }      
        public string PriceControl { get; set; }      
        public string ValuationCategory { get; set; }
        public string VarianceKey { get; set; }
        public string BatchManagement { get; set; }
        public string SnProfile { get; set; }
        
            

        public string Plant_ { get; set; }
        public string ProfitCenter_ { get; set; }
        public string StorageLocation_ { get; set; }
        public string StorageLocation2_ { get; set; }
        public string StorageLocation3_ { get; set; }
        public string StorageLocation4_ { get; set; }
        public string StorageLocation5_ { get; set; }
        public string StorageBin_ { get; set; }
        public string ValuationClass_ { get; set; }
        public string ValuationType_ { get; set; }
        public string PriceControl_ { get; set; }
        public string StandardPrice_ { get; set; }
        public string MovingAvgprice_ { get; set; }
        public string ValuationCategory_ { get; set; }
        public string VarianceKey_ { get; set; }

        //Mrp data

        public string MRPType { get; set; }
        public string MRPController { get; set; }
        public string LOTSize { get; set; }      
        public string ProcurementType { get; set; }       
        public string PlanningStrgyGrp { get; set; }
        public string AvailCheck { get; set; }
        public string ScheduleMargin { get; set; }


        public string MRPType_ { get; set; }
        public string MRPController_ { get; set; }
        public string LOTSize_ { get; set; }
        public string ReOrderPoint_ { get; set; }
        public string ProcurementType_ { get; set; }
        public string SafetyStock_ { get; set; }
        public string MaxStockLevel_ { get; set; }
        public string MinStockLevel_ { get; set; }
        public string PlanningStrgyGrp_ { get; set; }
        public string AvailCheck_ { get; set; }
        public string ScheduleMargin_ { get; set; }

        //Sales & others

        public string AccAsignmtCategory { get; set; }
        public string TaxClassificationGroup { get; set; }
        public string TaxClassificationGroup1 { get; set; }
        public string ItemCategoryGroup { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string MaterialStrategicGroup { get; set; }
        public string PurchasingGroup { get; set; }
        public string PurchasingValueKey { get; set; }      

        public string AccAsignmtCategory_ { get; set; }
        public string TaxClassificationGroup_ { get; set; }
        public string ItemCategoryGroup_ { get; set; }
        public string SalesOrganization_ { get; set; }
        public string DistributionChannel_ { get; set; }
        public string MaterialStrategicGroup_ { get; set; }
        public string PurchasingGroup_ { get; set; }
        public string PurchasingValueKey_ { get; set; }
        public string GoodsReceptprocessingTime_ { get; set; }
        public string Quantity_ { get; set; }
        public string Quantity2_ { get; set; }
        public string Quantity3_ { get; set; }
        public string Quantity4_ { get; set; }
        public string Quantity5_ { get; set; }

        // new fields for vmarket
        public string DeliveringPlant { get; set; }
        public string TaxClassificationDep { get; set; }
        public string TaxClassificationType { get; set; }

        public string TaxClassificationDep1 { get; set; }
        public string TaxClassificationType1 { get; set; }

        public string TransportationGroup { get; set; }
        public string LoadingGroup { get; set; }
        public string SalesText { get; set; }
        public string OrderUnit { get; set; }
        public string AutomaticPO { get; set; }
        public string PurchaseOrderText { get; set; }
        public string PlannedDeliveryTime { get; set; }

        public string DeliveringPlant_ { get; set; }
       // public string TaxClassificationDep_ { get; set; }
        public string TransportationGroup_ { get; set; }
        public string LoadingGroup_ { get; set; }
        public string SalesText_ { get; set; }
        public string OrderUnit_ { get; set; }
        public string AutomaticPO_ { get; set; }
        public string PurchaseOrderText_ { get; set; }
        public string PlannedDeliveryTime_ { get; set; }
        public string Price_Unit { get; set; }
        public string Price_Unit2 { get; set; }
        public string Price_Unit3 { get; set; }
        public string Price_Unit4 { get; set; }
        public string Price_Unit5 { get; set; }
        public string Currency { get; set; }
        public string Currency2 { get; set; }
        public string Currency3 { get; set; }
        public string Currency4 { get; set; }
        public string Currency5 { get; set; }
    }
}