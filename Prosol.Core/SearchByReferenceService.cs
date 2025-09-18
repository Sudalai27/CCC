using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;

// for mail
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web.Mvc;
using MongoDB.Bson.IO;


namespace Prosol.Core
{
    public partial class SearchByReferenceService : ISearchByReference
    {
        private readonly IRepository<Prosol_ERPInfo> _ERPInfoRepository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_Plants> _PlantRepository;
        private readonly IRepository<Prosol_Master> _MasterRepository;


        public SearchByReferenceService(IRepository<Prosol_ERPInfo> ERPInfoRepository, IRepository<Prosol_Datamaster> DatamasterRepository, IRepository<Prosol_Plants> PlantRepository, IRepository<Prosol_Master> MasterRepository)
        {
            this._ERPInfoRepository =ERPInfoRepository;
            this._DatamasterRepository = DatamasterRepository;
            this._PlantRepository = PlantRepository;
            this._MasterRepository = MasterRepository;
        }

        public IEnumerable<Prosol_Datamaster> getresultsforcode(string Code)
        {
            var query = Query.EQ("Itemcode", Code);
            var codelist = _ERPInfoRepository.FindAll(query).ToList();

            if (codelist.Count > 0)
            {
                var query1 = Query.EQ("Itemcode",codelist[0].Itemcode);
                var result = _DatamasterRepository.FindAll(query1).ToList();
                return result;
            }
            else
            {
                IEnumerable<Prosol_Datamaster> results = null;
                return results;
            }
        }
        public IEnumerable<Prosol_Plants> GetPlants()
        {
            var result = _PlantRepository.FindAll();
            return result;
        }

        public IEnumerable<Prosol_Master> GetStorageLocations(string Plant)
        {
            var query1 = Query.EQ("Plantname", Plant);

            var plant_res = _PlantRepository.FindAll(query1).ToList();

            if (plant_res.Count > 0)
            {
                var query = Query.And(Query.EQ("Plantcode", plant_res[0].Plantcode), Query.EQ("Label", "Storage location"), Query.EQ("Islive", true));
                var result = _MasterRepository.FindAll(query);
                return result;
            }
            else
            {
                IEnumerable<Prosol_Master> result = null;
                return result;
            }           
        }

        public IEnumerable<Prosol_Master> GetStorageBin(string Plant, string sl)
        {
            var query1 = Query.EQ("Plantname", Plant);

            var plant_res = _PlantRepository.FindAll(query1).ToList();

            if (plant_res.Count > 0)
            {
                var query2 = Query.And(Query.EQ("Label", "Storage location"), Query.EQ("Title", sl), Query.EQ("Plantcode", plant_res[0].Plantcode));

                var sl_res = _MasterRepository.FindAll(query2).ToList();
                if (sl_res.Count > 0)
                {
                    var query = Query.And(Query.EQ("Plantcode", plant_res[0].Plantcode), Query.EQ("Label", "Storage bin"), Query.EQ("Islive", true),Query.EQ("Storagelocationcode", sl_res[0].Code));
                    var result = _MasterRepository.FindAll(query);
                    return result;
                }
                else
                {
                    IEnumerable<Prosol_Master> result = null;
                    return result;
                }             
            }
            else
            {
                IEnumerable<Prosol_Master> result = null;
                return result;
            }


        }

        public IEnumerable<Prosol_Datamaster> GetResultForCode(string code)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(strArr);

            var query = Query.Or(Query.EQ("Itemcode", code), Query.EQ("Materialcode", code));
            var result = _DatamasterRepository.FindAll(fields,query).ToList();

            if(result.Count > 0)
            {
                return result;
            }
            else
            {
                IEnumerable<Prosol_Datamaster> result1 = null;
                return result1;
            }
        }

        public IEnumerable<Prosol_Datamaster> GetResultFornoun_modifier(string noun, string modifier)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(strArr);


            if (modifier != "Select")
            {
                var query = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier));
                var result = _DatamasterRepository.FindAll(fields, query).ToList();

                if (result.Count > 0)
                {
                    return result;
                }
                else
                {
                    IEnumerable<Prosol_Datamaster> result1 = null;
                    return result1;
                }
            }
            else
            {
                var query = Query.EQ("Noun", noun);
                var result = _DatamasterRepository.FindAll(fields, query).ToList();

                if (result.Count > 0)
                {
                    return result;
                }
                else
                {
                    IEnumerable<Prosol_Datamaster> result1 = null;
                    return result1;
                }
            }
        }




        public IEnumerable<Prosol_Datamaster> getresultsforrest(string Noun, string Modifier, string Industry_Sector, string Material_Type, string Inspection_Type, string Plant, string Profit_Center, string Storage_Location, string Storage_Bin, string Valuation_Class, string Price_Control, string MRP_Type, string MRP_Controller, string Procurement_Type)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(strArr);

            if (Modifier != "Select")
            {
                var DataMaster_query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
                var DataMaster = _DatamasterRepository.FindAll(fields, DataMaster_query).ToList();               

                var LstQry = new List<MongoDB.Driver.IMongoQuery>();

                if (Industry_Sector != "Select")
                {
                    LstQry.Add(Query.EQ("Industrysector_", Industry_Sector));
                }
                if (Material_Type != "Select")
                {
                    LstQry.Add(Query.EQ("Materialtype_", Material_Type));
                }
                if (Inspection_Type != "Select")
                {
                    LstQry.Add(Query.EQ("Inspectiontype_", Inspection_Type));
                }
                if (Profit_Center != "Select")
                {
                    LstQry.Add(Query.EQ("ProfitCenter_", Profit_Center));
                }
                if (Storage_Location != "Select")
                {
                    LstQry.Add(Query.EQ("StorageLocation_", Storage_Location));
                }
                if (Storage_Bin != "Select")
                {
                    LstQry.Add(Query.EQ("StorageBin_", Storage_Bin));
                }
                if (Valuation_Class != "Select")
                {
                    LstQry.Add(Query.EQ("ValuationClass_", Valuation_Class));
                }
                if (Price_Control != "Select")
                {
                    LstQry.Add(Query.EQ("PriceControl_", Price_Control));
                }
                if (MRP_Type != "Select")
                {
                    LstQry.Add(Query.EQ("MRPType_", MRP_Type));
                }
                if (MRP_Controller != "Select")
                {
                    LstQry.Add(Query.EQ("MRPController_", MRP_Controller));
                }
                if (Procurement_Type != "Select")
                {
                    LstQry.Add(Query.EQ("ProcurementType_", Procurement_Type));
                }
                if (Plant != "Select")
                {
                    LstQry.Add(Query.EQ("Plant_", Plant));
                }

                //Query.And(Query.NE("Select", Industry_Sector), Query.EQ("Industrysector_", Industry_Sector)),
                //Query.And(Query.NE("Select", Material_Type), Query.EQ("Materialtype_", Material_Type))  ,
                //Query.And(Query.NE("Select", Inspection_Type), Query.EQ("Inspectiontype_", Inspection_Type)),
                //Query.And(Query.NE("Select", Profit_Center), Query.EQ("ProfitCenter_", Profit_Center)),
                //Query.And(Query.NE("Select", Storage_Location), Query.EQ("StorageLocation_", Storage_Location)),
                //Query.And(Query.NE("Select", Storage_Bin), Query.EQ("StorageBin_", Storage_Bin)),
                //Query.And(Query.NE("Select", Valuation_Class), Query.EQ("ValuationClass_", Valuation_Class)),
                //Query.And(Query.NE("Select", Price_Control), Query.EQ("PriceControl_", Price_Control)),
                //Query.And(Query.NE("Select", MRP_Type), Query.EQ("MRPType_", MRP_Type)),
                //Query.And(Query.NE("Select", MRP_Controller), Query.EQ("MRPController_", MRP_Controller)),
                //Query.And(Query.NE("Select", Procurement_Type), Query.EQ("ProcurementType_", Procurement_Type)),
                //Query.And(Query.NE("Select", Plant), Query.EQ("Plant_", Plant))


                var Field_query = Query.And(LstQry);
                var FieldsRes = _ERPInfoRepository.FindAll(Field_query).ToList();

                var F_reslut = (from d in DataMaster.AsQueryable()
                                join f in FieldsRes.AsQueryable() on d.Itemcode equals f.Itemcode
                                select new Prosol_Datamaster { Itemcode = d.Itemcode, Shortdesc = d.Shortdesc, Longdesc = d.Longdesc }).ToList();
                if (F_reslut.Count > 0)
                {
                    return F_reslut;
                }
                else
                {
                    List<Prosol_Datamaster> result1 = null;
                    return result1;
                }
            }
            else if (Noun != "Select")
            {
                var DataMaster_query = Query.EQ("Noun", Noun);              

                var DataMaster = _DatamasterRepository.FindAll(fields, DataMaster_query).ToList();

              

                var LstQry = new List<MongoDB.Driver.IMongoQuery>();

                if (Industry_Sector != "Select")
                {
                    LstQry.Add(Query.EQ("Industrysector_", Industry_Sector));
                }
                if (Material_Type != "Select")
                {
                    LstQry.Add(Query.EQ("Materialtype_", Material_Type));
                }
                if (Inspection_Type != "Select")
                {
                    LstQry.Add(Query.EQ("Inspectiontype_", Inspection_Type));
                }
                if (Profit_Center != "Select")
                {
                    LstQry.Add(Query.EQ("ProfitCenter_", Profit_Center));
                }
                if (Storage_Location != "Select")
                {
                    LstQry.Add(Query.EQ("StorageLocation_", Storage_Location));
                }
                if (Storage_Bin != "Select")
                {
                    LstQry.Add(Query.EQ("StorageBin_", Storage_Bin));
                }
                if (Valuation_Class != "Select")
                {
                    LstQry.Add(Query.EQ("ValuationClass_", Valuation_Class));
                }
                if (Price_Control != "Select")
                {
                    LstQry.Add(Query.EQ("PriceControl_", Price_Control));
                }
                if (MRP_Type != "Select")
                {
                    LstQry.Add(Query.EQ("MRPType_", MRP_Type));
                }
                if (MRP_Controller != "Select")
                {
                    LstQry.Add(Query.EQ("MRPController_", MRP_Controller));
                }
                if (Procurement_Type != "Select")
                {
                    LstQry.Add(Query.EQ("ProcurementType_", Procurement_Type));
                }
                if (Plant != "Select")
                {
                    LstQry.Add(Query.EQ("Plant_", Plant));
                }
                var Field_query = Query.And(LstQry);
                var FieldsRes = _ERPInfoRepository.FindAll(Field_query).ToList();

                var F_reslut = (from d in DataMaster.AsQueryable()
                                join f in FieldsRes.AsQueryable() on d.Itemcode equals f.Itemcode
                                select new Prosol_Datamaster { Itemcode = d.Itemcode, Shortdesc = d.Shortdesc, Longdesc = d.Longdesc }).ToList();
                if (F_reslut.Count > 0)
                {
                    return F_reslut;
                }
                else
                {
                    List<Prosol_Datamaster> result1 = null;
                    return result1;
                }
            }
            else
            {
                var DataMaster = _DatamasterRepository.FindAll(fields).ToList();

                var LstQry = new List<MongoDB.Driver.IMongoQuery>();
                if (Industry_Sector != "Select")
                {
                    LstQry.Add(Query.EQ("Industrysector_", Industry_Sector));
                }
                if (Material_Type != "Select")
                {
                    LstQry.Add(Query.EQ("Materialtype_", Material_Type));
                }
                if (Inspection_Type != "Select")
                {
                    LstQry.Add(Query.EQ("Inspectiontype_", Inspection_Type));
                }
                if (Profit_Center != "Select")
                {
                    LstQry.Add(Query.EQ("ProfitCenter_", Profit_Center));
                }
                if (Storage_Location != "Select")
                {
                    LstQry.Add(Query.EQ("StorageLocation_", Storage_Location));
                }
                if (Storage_Bin != "Select")
                {
                    LstQry.Add(Query.EQ("StorageBin_", Storage_Bin));
                }
                if (Valuation_Class != "Select")
                {
                    LstQry.Add(Query.EQ("ValuationClass_", Valuation_Class));
                }
                if (Price_Control != "Select")
                {
                    LstQry.Add(Query.EQ("PriceControl_", Price_Control));
                }
                if (MRP_Type != "Select")
                {
                    LstQry.Add(Query.EQ("MRPType_", MRP_Type));
                }
                if (MRP_Controller != "Select")
                {
                    LstQry.Add(Query.EQ("MRPController_", MRP_Controller));
                }
                if (Procurement_Type != "Select")
                {
                    LstQry.Add(Query.EQ("ProcurementType_", Procurement_Type));
                }
                if (Plant != "Select")
                {
                    LstQry.Add(Query.EQ("Plant_", Plant));
                }
               
                var Field_query = Query.And(LstQry);
                var FieldsRes = _ERPInfoRepository.FindAll(Field_query).ToList();

                var F_reslut = (from d in DataMaster.AsQueryable()
                                join f in FieldsRes.AsQueryable() on d.Itemcode equals f.Itemcode
                                select new Prosol_Datamaster { Itemcode = d.Itemcode, Shortdesc = d.Shortdesc, Longdesc = d.Longdesc }).ToList();
                if (F_reslut.Count > 0)
                {
                    return F_reslut;
                }
                else
                {
                    List<Prosol_Datamaster> result1 = null;
                    return result1;
                }
            }

        }
    }
}
