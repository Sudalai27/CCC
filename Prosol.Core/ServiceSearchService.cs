using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Prosol.Core
{
    public partial class ServiceSearchService : IServiceSearch
    {
        private readonly IRepository<Prosol_ServiceCategory> _ServiceSearchCatRepository;
        private readonly IRepository<Prosol_ServiceGroup> _ServiceSearchGRP;
        private readonly IRepository<Prosol_ServiceUom> _ServiceSearchUOM;
        private readonly IRepository<Prosol_RequestService> _ServiceSerReq;
        private readonly IRepository<Prosol_RequestService> _RequestService;

        public ServiceSearchService(IRepository<Prosol_ServiceCategory> ServiceSearchCatRepository,
                                    IRepository<Prosol_ServiceGroup> ServiceSearchGRP,
                                    IRepository<Prosol_ServiceUom> ServiceSearchUOM,
                                    IRepository<Prosol_RequestService> ServiceSerReq,
                                    IRepository<Prosol_RequestService> RequestService)
        {
            this._ServiceSearchCatRepository = ServiceSearchCatRepository;
            this._ServiceSearchGRP = ServiceSearchGRP;
            this._ServiceSearchUOM = ServiceSearchUOM;
            this._ServiceSerReq = ServiceSerReq;
            this._RequestService = RequestService;
        }
        public List<Prosol_ServiceCategory> getcategory()
        {

            var shwusr = _ServiceSearchCatRepository.FindAll().ToList();
            return shwusr;
        }
        public IEnumerable<Prosol_ServiceGroup> getgroupp(string ServiceCategorycode)
        {
           
            var query = Query.And(Query.EQ("Islive", true), Query.EQ("SeviceCategorycode", ServiceCategorycode));
            var vn = _ServiceSearchGRP.FindAll(query).ToList();
            return vn;
        }
        public List<Prosol_ServiceUom> getUOM()
        {
            var UOM = _ServiceSearchUOM.FindAll().ToList();
            return UOM;
        }
        public IEnumerable<Prosol_RequestService> getServiceCode(string cat, string grp, string uom)
        {
            if(cat != "undefined" && grp != "undefined" && uom != "undefined")
            {
                var query = Query.And(Query.EQ("ServiceCategoryCode", cat), Query.EQ("ServiceGroupCode", grp), Query.EQ("UomCode", uom));
                var result = _ServiceSerReq.FindAll(query).ToList();
                return result;

            }
            else if (cat != "undefined" && uom != "undefined")
            {
                var query = Query.And(Query.EQ("ServiceCategoryCode", cat), Query.EQ("UomCode", uom));
                var result = _ServiceSerReq.FindAll(query).ToList();
                return result;
            }
            else if (cat != "undefined" && grp != "undefined")
            {
                var query = Query.And(Query.EQ("ServiceCategoryCode", cat), Query.EQ("ServiceGroupCode", grp));
                var result = _ServiceSerReq.FindAll(query).ToList();
                return result;
            }
            else if (cat != "undefined")
            {
                var query = Query.EQ("ServiceCategoryCode", cat);
                var result = _ServiceSerReq.FindAll(query).ToList();
                return result;
            }
            else if(uom != "undefined")
            {
                var query = Query.EQ("UomCode", uom);
                var result = _ServiceSerReq.FindAll(query).ToList();
                return result;
            }
            else
            {
                var query = Query.And(Query.EQ("ServiceCategoryCode", cat), Query.EQ("ServiceGroupCode", grp), Query.EQ("UomCode", uom));
                var result = _ServiceSerReq.FindAll(query).ToList();
                return result;

            }

              //  var Table = _ServiceSerReq.FindAll().ToList();
          //  return Table;

        }
 

        public IEnumerable<Prosol_RequestService> gettabledetails(string ServiceCode)
        {
            var query = Query.EQ("ServiceCode", ServiceCode);
            var result = _ServiceSerReq.FindAll(query).ToList();
            return result;
        }

        public IEnumerable<Prosol_RequestService> getdetailsforcode(string code)
        {
            var query = Query.EQ("ServiceCode", code);
            var result = _ServiceSerReq.FindAll(query).ToList();
            return result;
        }

        public IEnumerable<Prosol_RequestService> getdetailsforsd(string sd)
        {
            if(sd.Contains(','))
            {
                var query = new List<IMongoQuery>();
                string[] sdseparated = sd.Split(',');
                foreach(string str in sdseparated)
                {
                    var qury = Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(str, RegexOptions.IgnoreCase)));
                    query.Add(qury);
                }
                var querry = Query.And(query);
                var result = _ServiceSerReq.FindAll(querry).ToList();
                return result;
               
            }
            else
            {
                var qury = Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(sd, RegexOptions.IgnoreCase)));
                var result = _ServiceSerReq.FindAll(qury).ToList();
                return result;
            }
        }

        public IEnumerable<Prosol_RequestService> ServiceMasterSearch(string search,string searchkey) 
        {

            var query = new List<IMongoQuery>();

            //string[] strArr = { "Itemcode", "Materialcode", "Shortdesc", "Longdesc", "Noun", "Modifier", "Manufacturer", "UOM", "Partno", "Characteristics", "Equipment", "Vendorsuppliers", "Duplicates", "Catalogue" };
            //var fields = Fields.Include(strArr);

            //  var qury1 = new List<IMongoQuery>();

            //if (prs.ServiceCategoryCode != "undefined" && prs.ServiceCategoryCode != null && prs.ServiceGroupCode != "undefined" && prs.ServiceGroupCode != null && prs.UomCode != "undefined" && prs.UomCode != null)
            //{
            //    var query1 = Query.And(Query.EQ("ServiceCategoryCode", prs.ServiceCategoryCode), Query.EQ("ServiceGroupCode", prs.ServiceGroupCode), Query.EQ("UomCode", prs.UomCode));
            //    query.Add(query1);
            //}
            //else if (prs.ServiceCategoryCode != "undefined" && prs.ServiceCategoryCode != null && prs.UomCode != "undefined" && prs.UomCode != null)
            //{
            //    var query1 = Query.And(Query.EQ("ServiceCategoryCode", prs.ServiceCategoryCode), Query.EQ("UomCode", prs.UomCode));
            //    query.Add(query1);
            //}
            //else if (prs.ServiceCategoryCode != "undefined" && prs.ServiceCategoryCode != null && prs.ServiceGroupCode != "undefined" && prs.ServiceGroupCode != null)
            //{
            //    var query1 = Query.And(Query.EQ("ServiceCategoryCode", prs.ServiceCategoryCode), Query.EQ("ServiceGroupCode", prs.ServiceGroupCode));
            //    query.Add(query1);
            //}
            //else if (prs.ServiceCategoryCode != "undefined" && prs.ServiceCategoryCode != null)
            //{
            //    var query1 = Query.EQ("ServiceCategoryCode", prs.ServiceCategoryCode);
            //    query.Add(query1);
            //}
            //else if (prs.UomCode != "undefined" && prs.UomCode != null)
            //{
            //    var query1 = Query.EQ("UomCode", prs.UomCode);
            //    query.Add(query1);
            //}
            //else
            //{

            //}

            searchkey = searchkey.Replace(',', '*');
            searchkey = searchkey.Replace('(', '*');
            searchkey = searchkey.Replace(')', '*');

            // short desc
            if (search == "Description")
            {
                if (searchkey != "" && searchkey != null && searchkey != "unknown" && searchkey != "undefined")
                    if (searchkey.Contains('*'))
                    {
                        string[] sdseparated = searchkey.Split('*');
                        foreach (string str in sdseparated)
                        {
                            // var qury = Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(str, RegexOptions.IgnoreCase)));
                            var qury = Query.Or(Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                            query.Add(qury);
                        }


                    }
                    else
                    {

                        var qury = Query.Or(Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(searchkey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(searchkey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                        // var qury = Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(prs.ShortDesc, RegexOptions.IgnoreCase)));
                        query.Add(qury);
                    }
            }


            //// long desc
            //if (prs.LongDesc != "" && prs.LongDesc != null && prs.LongDesc != "unknown" && prs.LongDesc != "undefined")
            //    if (prs.LongDesc.Contains(','))
            //    {
            //        string[] ldseparated = prs.LongDesc.Split(',');
            //        foreach (string str in ldseparated)
            //        {
            //            var qury = Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(str, RegexOptions.IgnoreCase)));
            //            query.Add(qury);
            //        }

            //    }
            //    else
            //    {
            //        var qury = Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(prs.LongDesc, RegexOptions.IgnoreCase)));
            //        query.Add(qury);
            //    }

            //}

            // for code

            else
            {
                searchkey = searchkey.Replace(',', '*');
               
                //  term = term.Replace(' ', '*');
                // term = term.Replace('-', '*');
                if (searchkey.Contains('*'))
                {


                    var QryLst = new List<IMongoQuery>();
                    var QryLst1 = new List<IMongoQuery>();
                    string[] sepArr = searchkey.Split('*');
                    if (sepArr.Length > 1)
                    {
                        foreach (string str in sepArr)
                        {
                            if (str != "")
                            {
                                // var Qry1 = Query.Or(Query.Matches("ServiceCode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.Matches("parent", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                                var Qry1 = Query.Or(Query.EQ("ServiceCode", str), Query.EQ("parent", str));
                                query.Add(Qry1);

                            }
                        }
                    }
                   
                }
                else
                {
                     var querycode = Query.Or(Query.EQ("ServiceCode", searchkey), Query.EQ("parent", searchkey));
                    query.Add(querycode);

                }
                var query1 = Query.Or(query);
                var result = _ServiceSerReq.FindAll(query1).ToList();
                return result;
            }

            //else
            //{

            //    var querycode = Query.Or(Query.EQ("ServiceCode", searchkey), Query.EQ("parent", searchkey));
            //    query.Add(querycode);

            //}

            if (query.Count > 0)
            { 
            var querry = Query.And(query);
            var result = _ServiceSerReq.FindAll(querry).ToList();
            return result;
            }
            else
            {
                List<Prosol_RequestService> pcs = new List<Prosol_RequestService>();
                pcs = null;
                return pcs;
            }
        }

        public IEnumerable<Prosol_RequestService> getdetailsforld(string ld)
        {
            if (ld.Contains(','))
            {
                var query = new List<IMongoQuery>();
                string[] ldseparated = ld.Split(',');
                foreach (string str in ldseparated)
                {
                    var qury = Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(str, RegexOptions.IgnoreCase)));
                    query.Add(qury);
                }
                var querry = Query.And(query);
                var result = _ServiceSerReq.FindAll(querry).ToList();
                return result;

            }
            else
            {
                var qury = Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(ld, RegexOptions.IgnoreCase)));
                var result = _ServiceSerReq.FindAll(qury).ToList();
                return result;
            }
        }



        public List<Prosol_serviceDashboard> BindTotalItem()
        {
            List<Prosol_serviceDashboard> lstdb = new List<Prosol_serviceDashboard>();

            var dataList = _RequestService.FindAll();
            var arrlist = dataList.Select(x => x.PlantCode).Distinct();
            // var arrlist = dataList.Select(x => new { x.PlantCode, x.MainCode }).Distinct();

            foreach (string str in arrlist)
            {
                Prosol_serviceDashboard mdl = new Prosol_serviceDashboard();
                var listplnt = from x in dataList where x.PlantCode.ToUpper() == str.ToUpper() select x;
                var listplntt = listplnt.ToList();
                if (listplnt.Count() > 0)
                {
                    var completed = from x in listplnt where x.ServiceStatus.ToUpper() == "COMPLETED" select x;
                    mdl.PlantCode = str;
                    mdl.PlantName = listplntt[0].PlantName;
                    mdl.TotalItems = listplnt.Count();

                    if (completed.Count() > 0)
                        mdl.CompletedItems = completed.Count();
                    else
                        mdl.CompletedItems = 0;


                    lstdb.Add(mdl);
                }
            }
            //category

            //var arraylist1 = dataList.Select(y => y.MainCode).Distinct();
            //foreach (string str1 in arraylist1)
            //{
            //    Prosol_serviceDashboard mdl1 = new Prosol_serviceDashboard();
            //    var maincodelist = from y in dataList where y.MainCode == str1 select y;
            //    var maincodelisttt = maincodelist.ToList();
            //    if (maincodelist.Count() > 0)
            //    {

            //        var completed = from y in maincodelist where y.ServiceStatus.ToUpper() == "COMPLETED" select y;
            //        mdl1.Maincode = str1;
            //        mdl1.MainCategoryName = maincodelisttt[0].MainCodeName;
            //        mdl1.TotalCategory = maincodelist.Count();

            //        if (completed.Count() > 0)
            //            mdl1.CompletedCategory = completed.Count();
            //        else
            //            mdl1.CompletedCategory = 0;

            //        lstdb.Add(mdl1);
            //    }

            //}


            return lstdb;
        }
        public List<Prosol_serviceDashboard> BindTotalItemcategory()
        {
            List<Prosol_serviceDashboard> lstdb1 = new List<Prosol_serviceDashboard>();

            var dataList = _RequestService.FindAll();

            var arraylist1 = dataList.Select(y => y.ServiceCategoryName).Distinct();
            foreach (string str1 in arraylist1)
            {
                Prosol_serviceDashboard mdl1 = new Prosol_serviceDashboard();
                var maincodelist = from y in dataList where y.ServiceCategoryName == str1 select y;
                var maincodelisttt = maincodelist.ToList();
                if (maincodelist.Count() > 0)
                {

                    var completed = from y in maincodelist where y.ServiceStatus.ToUpper() == "COMPLETED" select y;
                    mdl1.MainCategoryName = str1;
                    if (mdl1.MainCategoryName != null)
                    {
                        mdl1.MainCategoryName = maincodelisttt[0].ServiceCategoryName;
                    }
                    else
                        mdl1.MainCategoryName = "No Category";
                    mdl1.TotalCategory = maincodelist.Count();

                    if (completed.Count() > 0)
                        mdl1.CompletedCategory = completed.Count();
                    else
                        mdl1.CompletedCategory = 0;

                    lstdb1.Add(mdl1);
                }

            }


            return lstdb1;
        }
        public List<Prosol_serviceDashboard> BindTotalItemcategoryGroup()
        {
            List<Prosol_serviceDashboard> lstdb1 = new List<Prosol_serviceDashboard>();

            var dataList = _RequestService.FindAll();

            var arraylist1 = dataList.Select(y => y.ServiceGroupName).Distinct();
            foreach (string str1 in arraylist1)
            {
                Prosol_serviceDashboard mdl1 = new Prosol_serviceDashboard();
                var maincodelist = from y in dataList where y.ServiceGroupName == str1 select y;
                var maincodelisttt = maincodelist.ToList();
                if (maincodelist.Count() > 0)
                {

                    var completed = from y in maincodelist where y.ServiceStatus.ToUpper() == "COMPLETED" select y;
                    mdl1.ServiceGroupName = str1;
                    if (mdl1.ServiceGroupName != null)
                    {
                        mdl1.MainCategoryName = maincodelisttt[0].ServiceCategoryName;
                        //mdl1.MainCategoryName = maincodelisttt.ServiceCategoryName;
                    }
                    else
                        mdl1.MainCategoryName = "No Category";
                    mdl1.ServiceGroupName = maincodelisttt[0].ServiceGroupName;
                    mdl1.TotalService = maincodelist.Count();

                    if (completed.Count() > 0)
                        mdl1.CompletedService = completed.Count();
                    else
                        mdl1.CompletedService = 0;

                    lstdb1.Add(mdl1);
                }

            }


            return lstdb1;
        }
    }
}
