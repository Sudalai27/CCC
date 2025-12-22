using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Prosol.Common;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MongoDB.Driver;
using System.Globalization;

namespace Prosol.Core
{

    public partial class ReportService : I_Report
    {
        private readonly IRepository<Prosol_Datamaster> _UserreportRepository;
        private readonly IRepository<Prosol_Request> _UserrequestRepository;

       // private readonly IRepository<Prosol_Plantinfo> _UserreportplntRepository
          private readonly IRepository<Prosol_ERPInfo> _ERPInfoRepository;

        private readonly IRepository<Prosol_Users> _UserlistRepository;
        private readonly IRepository<Prosol_Plants> _UserplntlistRepository;
        private readonly IRepository<Prosol_Departments> _UserdeplistRepository;
        private readonly IRepository<Prosol_Attachment> _attachlistRepository;
        private readonly IRepository<Prosol_NounModifiers> _NMRepository;
        private readonly IRepository<Prosol_Charateristics> _charRepository;
        private readonly IRepository<Prosol_Abbrevate> _AbbrevateRepository;
        private readonly IRepository<Prosol_ERPLog> _ERPLogRepository;
        private readonly IRepository<Prosol_UNSPSC> _unspsclistRepository;
        public ReportService(IRepository<Prosol_Datamaster> Userreportservice,
            IRepository<Prosol_ERPInfo> ERPInfoRepository,
            IRepository<Prosol_Users> Userlistservice,
            IRepository<Prosol_Plants> Userplntlistservice,
            IRepository<Prosol_Departments> Userdeptlistservice,
            IRepository<Prosol_Request> Userreqlistservice,
            IRepository<Prosol_Attachment> attachlistRepository,
            IRepository<Prosol_NounModifiers> NMRepository,
             IRepository<Prosol_Abbrevate> abbRepository,
             IRepository<Prosol_Charateristics> charRepository,
             IRepository<Prosol_UNSPSC> unspsclistRepository,
            IRepository<Prosol_ERPLog> ERPLogRepository)
        {
            this._UserreportRepository = Userreportservice;
            //  this._UserreportplntRepository = Userreportplntservice;
            this._ERPInfoRepository = ERPInfoRepository;
            this._UserlistRepository = Userlistservice;
            this._UserplntlistRepository = Userplntlistservice;
            this._UserdeplistRepository = Userdeptlistservice;
            this._UserrequestRepository = Userreqlistservice;
            this._attachlistRepository = attachlistRepository;
            this._NMRepository = NMRepository;
            this._charRepository = charRepository;
            this._AbbrevateRepository = abbRepository;
            this._ERPLogRepository = ERPLogRepository;
            this._unspsclistRepository = unspsclistRepository;
        }
        public List<Dictionary<string, object>> loaddata(string[] options, string where, string value, string fromdate, string todate, string role, string status)
        {

            IMongoQuery query, query1, querychr, queryplnt, query2, qry;
            var fields = Fields.Include(options).Exclude("_id");
            if (where == "UserName")
            {

                if (role == "Cataloguer")
                {
                    if (value != null && value != "null")
                        query = Query.EQ("Catalogue.Name", value);
                    else query = Query.NE("Catalogue", BsonNull.Value);
                }
                else if (role == "Reviewer")
                {
                    if (value != null && value != "null")
                        query = Query.EQ("Review.Name", value);
                    else query = Query.NE("Review", BsonNull.Value);
                }
                else
                {
                    if (value != null && value != "null")
                        query = Query.EQ("Release.Name", value);
                    else query = Query.NE("Release", BsonNull.Value);
                }
            }
            else
            {
                query = Query.EQ("Itemcode", value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Completed")
                {
                    if (role == "Cataloguer")
                        query1 = Query.And(query, Query.GT("ItemStatus", 1), Query.NE("ItemStatus", 7));
                    else if (role == "Reviewer")
                        query1 = Query.And(query, Query.GT("ItemStatus", 3), Query.NE("ItemStatus", 7));
                    else if (role == "Releaser")
                        query1 = Query.And(query, Query.GT("ItemStatus", 5), Query.NE("ItemStatus", 7));
                    else
                        query1 = query;
                }
                else
                {
                    if (role == "Cataloguer")
                    {
                        qry = Query.And(Query.LT("ItemStatus", 2), Query.NE("ItemStatus", -1));
                        //qry = Query.LT("ItemStatus", 2);
                        query1 = Query.And(query, qry);
                    }
                    else if (role == "Reviewer")
                    {
                        qry = Query.And(Query.LT("ItemStatus", 4), Query.GT("ItemStatus", 1));
                        query1 = Query.And(query, qry);
                    }
                    else if (role == "Releaser")
                    {
                        qry = Query.And(Query.LT("ItemStatus", 6), Query.GT("ItemStatus", 3));
                        query1 = Query.And(query, qry);
                    }
                    else
                        query1 = query;

                }
            }
            else
            {

                if (role == "Cataloguer")
                    query1 = Query.And(query, Query.GTE("ItemStatus", 0), Query.NE("ItemStatus", 7));
                else if (role == "Reviewer")
                    query1 = Query.And(query, Query.GTE("ItemStatus", 2), Query.NE("ItemStatus", 7));
                else if (role == "Releaser")
                    query1 = Query.And(query, Query.GTE("ItemStatus", 4), Query.NE("ItemStatus", 7));
                else
                    query1 = query;
            }
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
                date1 = date1.AddDays(1);
                date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);

                if (role == "Cataloguer")
                    query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                else if (role == "Reviewer")
                    query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                else
                    query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));

            }
            else if (!string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

                if (role == "Cataloguer")
                    query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                else if (role == "Reviewer")
                    query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                else
                    query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));

                //  query2 = Query.And(query1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
            }
            else
            {
                if (role == "Cataloguer")
                    query2 = Query.And(query1);
                else if (role == "Reviewer")
                    query2 = Query.And(query1);
                else
                    query2 = Query.And(query1);
            }
          //  var NM_master = _NMRepository.FindAll();
            var datalist = _UserreportRepository.FindAll(fields, query2).ToList();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            bool goo;
            var mergelist = (dynamic)null;
            if (options.Contains("Plants"))
            {
                var plntlist = _ERPInfoRepository.FindAll().ToList();
                // mergelist = (from data in datalist join plant in plntlist on data.Itemcode equals plant.Itemcode join nm in NM_master on data.Noun equals nm.Noun where data.Modifier== nm.Modifier select new { Plant = plant.Plant != null ? plant.Plant : "", Itemcode = data.Itemcode, Formatted = nm.Formatted  , Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment= getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus  = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks , Soureurl = data.Soureurl }).ToList();
                mergelist = (from data in datalist join plant in plntlist on data.Itemcode equals plant.Itemcode select new { Plant = plant.Plant != null ? plant.Plant : "", Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Additionalinfo = data.Additionalinfo }).ToList();
                if (mergelist.Count == 0)
                {
                    mergelist = datalist;
                    goo = false;

                }
                else
                {
                    goo = true;
                }
            }
            else
            {
          
              //  mergelist = datalist;
                mergelist = (from data in datalist  select new { Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Additionalinfo =data.Additionalinfo }).ToList();
                // mergelist = (from data in datalist join nm in NM_master on data.Noun equals nm.Noun where data.Modifier == nm.Modifier select new { Itemcode = data.Itemcode, Formatted = nm.Formatted, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, Soureurl = data.Soureurl }).ToList();
                goo = false;
            }
            if (options.Contains("Characteristics"))
            {
                int flg = 0;
                foreach (var code in mergelist)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Itemcode", code.Itemcode);
                    row.Add("Material code", code.Materialcode);

                    if (options.Contains("Noun"))
                        row.Add("Noun", code.Noun);
                    else row.Add("Noun", "");

                    if (options.Contains("Modifier"))
                        row.Add("Modifier", code.Modifier);
                    else row.Add("Modifier", "");

                    if (options.Contains("UOM"))
                        row.Add("UOM", code.UOM);
                    else row.Add("UOM", "");

                    if (options.Contains("Legacy"))
                        row.Add("Legacy", code.Legacy);
                    else row.Add("Legacy", "");

                    if (options.Contains("Legacy2"))
                        row.Add("PvData", code.Legacy2);
                    else row.Add("PvData", "");

                    if (options.Contains("Shortdesc"))
                        row.Add("Shortdesc", code.Shortdesc);
                    else row.Add("Shortdesc", "");

                    if (options.Contains("Longdesc"))
                        row.Add("Longdesc", code.Longdesc);
                    else row.Add("Longdesc", "");

                    if (options.Contains("Additionalinfo"))
                        row.Add("Additionalinfo", code.Additionalinfo);
                    else row.Add("Additionalinfo", "");                    

                    if (code.ItemStatus == 0 || code.ItemStatus == 1)
                        row.Add("ItemStatus", "Catalogue");
                    else if (code.ItemStatus == 2 || code.ItemStatus == 3)
                        row.Add("ItemStatus", "QC");
                    else if (code.ItemStatus == 4 || code.ItemStatus == 5)
                        row.Add("ItemStatus", "QA");
                    else if (code.ItemStatus == 6)
                        row.Add("ItemStatus", "Released");
                    else if (code.ItemStatus == -1)
                        row.Add("ItemStatus", "Clarification");
                    else row.Add("ItemStatus", "");

                    if (options.Contains("Vendorsuppliers"))                        
                          
                        row.Add("Vendor Details", code.Manufacturer);
                    else row.Add("Vendor Details", "");

                    if (options.Contains("Equipment"))
                        row.Add("Equipment Details", code.Equipment);
                    else row.Add("Equipment Details", "");

                    //if (options.Contains("Partno"))
                    //{
                    //    row.Add("Part No", code.Partno);
                    //}
                    //else row.Add("Part No", "");

                    if (code.Catalogue != null)
                    {
                        row.Add("Cataloguer", code.Catalogue.Name);
                        if (code.Catalogue.UpdatedOn != null)
                        {
                            row.Add("Cat-Createdon", code.Catalogue.UpdatedOn);
                        }
                        else
                            row.Add("Cat-Createdon", "");
                    }
                    else
                    {
                        row.Add("Cataloguer", "");
                        row.Add("Cat-Createdon", "");
                    }



                    if (code.Review != null)
                    {
                        row.Add("Reviewer", code.Review.Name);
                        if (code.Review.UpdatedOn != null)
                        {
                            row.Add("Rev-Createdon", code.Review.UpdatedOn);
                        }
                        else
                            row.Add("Rev-Createdon", "");

                    }
                    else
                    {
                        row.Add("Reviewer", "");
                        row.Add("Rev-Createdon", "");
                    }


                    if (code.Release != null)
                    {
                        row.Add("Releaser", code.Release.Name);
                        if (code.Release.UpdatedOn != null)
                        {
                            row.Add("Rel-Createdon", code.Release.UpdatedOn);
                        }
                        else
                            row.Add("Rel-Createdon", "");

                    }
                    else
                    {
                        row.Add("Releaser", "");
                        row.Add("Rel-Createdon", "");
                    }


                    if (code.RejectedBy != null)
                    {
                        row.Add("RejectedBy", code.RejectedBy.Name);
                        if (code.RejectedBy.UpdatedOn != null)
                        {
                            row.Add("RejectedOn", code.RejectedBy.UpdatedOn);
                        }
                        else
                            row.Add("RejectedOn", "");

                    }
                    else
                    {
                        row.Add("RejectedBy", "");
                        row.Add("RejectedOn", "");
                    }
                    //if (code.Remarks != null)
                    //{
                    //    row.Add("Remarks", code.Remarks);
                    //}
                    //else row.Add("Remarks", "");
                    if (code.Remarks != null)
                    {
                        row.Add("Cat.Remarks", code.Remarks);
                    }
                    else
                        row.Add("Cat.Remarks", "");

                    if (code.RevRemarks != null)
                    {
                        row.Add("Rev.Remarks", code.RevRemarks);
                    }
                    else
                        row.Add("Rev.Remarks", "");

                    if (code.RelRemarks != null)
                    {
                        row.Add("Rel.Remarks", code.RelRemarks);
                    }
                    else
                        row.Add("Rel.Remarks", "");

                    if (goo == true)
                    {
                        if (options.Contains("Plants"))
                            row.Add("Plant", code.Plant);
                        else row.Add("Plant", "");
                    }
                    if (options.Contains("Characteristics"))
                    {
                        if (code.Characteristics != null)
                        {
                            int i = 1;
                            foreach (var at in code.Characteristics)
                            {
                                row.Add("Attribute" + i, at.Characteristic);
                                row.Add("Value" + i, at.Value);
                                row.Add("UOM" + i, at.UOM);
                               // row.Add("Source" + i, at.Source);
                              //  row.Add("SourceUrl" + i, at.SourceUrl);
                                i++;
                            }
                        }
                    }
                    rows.Add(row);
                }
            }
            else
            {
                foreach (var code in mergelist)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Itemcode", code.Itemcode);
                    row.Add("Matrial Code", code.Materialcode);
                    if (options.Contains("Noun"))
                        row.Add("Noun", code.Noun);
                    else row.Add("Noun", "");

                    if (options.Contains("Modifier"))
                        row.Add("Modifier", code.Modifier);
                    else row.Add("Modifier", "");

                    if (options.Contains("UOM"))
                        row.Add("UOM", code.UOM);
                    else row.Add("UOM", "");

                    if (options.Contains("Legacy"))
                        row.Add("Legacy", code.Legacy);
                    else row.Add("Legacy", "");

                    if (options.Contains("Legacy2"))
                        row.Add("PvData", code.Legacy2);
                    else row.Add("PvData", "");

                    if (options.Contains("Shortdesc"))
                        row.Add("Shortdesc", code.Shortdesc);
                    else row.Add("Shortdesc", "");
                    if (options.Contains("Longdesc"))
                        row.Add("Longdesc", code.Longdesc);
                    else row.Add("Longdesc", "");

                    if (options.Contains("Vendorsuppliers"))

                        row.Add("Vendor Details", code.Manufacturer);
                    else row.Add("Vendor Details", "");

                    if (code.Catalogue != null)
                    {
                        row.Add("Cataloguer", code.Catalogue.Name);
                        if (code.Catalogue.UpdatedOn != null)
                        {
                            row.Add("Cat-Createdon", code.Catalogue.UpdatedOn);
                        }
                        else
                            row.Add("Cat-Createdon", "");
                    }
                    else
                    {
                        row.Add("Cataloguer", "");
                        row.Add("Cat-Createdon", "");
                    }



                    if (code.Review != null)
                    {
                        row.Add("Reviewer", code.Review.Name);
                        if (code.Review.UpdatedOn != null)
                        {
                            row.Add("Rev-Createdon", code.Review.UpdatedOn);
                        }
                        else
                            row.Add("Rev-Createdon", "");

                    }
                    else
                    {
                        row.Add("Reviewer", "");
                        row.Add("Rev-Createdon", "");
                    }


                    if (code.Release != null)
                    {
                        row.Add("Releaser", code.Release.Name);
                        if (code.Release.UpdatedOn != null)
                        {
                            row.Add("Rel-Createdon", code.Release.UpdatedOn);
                        }
                        else
                            row.Add("Rel-Createdon", "");

                    }
                    else
                    {
                        row.Add("Releaser", "");
                        row.Add("Rel-Createdon", "");
                    }


                    if (code.RejectedBy != null)
                    {
                        row.Add("RejectedBy", code.RejectedBy.Name);
                        if (code.RejectedBy.UpdatedOn != null)
                        {
                            row.Add("RejectedOn", code.RejectedBy.UpdatedOn);
                        }
                        else
                            row.Add("RejectedOn", "");

                    }
                    else
                    {
                        row.Add("RejectedBy", "");
                        row.Add("RejectedOn", "");
                    }
                    //if (code.Remarks != null)
                    //{
                    //    row.Add("Remarks", code.Remarks);
                    //}
                    //else row.Add("Remarks", "");
                    if (code.Remarks != null)
                    {
                        row.Add("Cat.Remarks", code.Remarks);
                    }
                    else
                        row.Add("Cat.Remarks", "");

                    if (code.RevRemarks != null)
                    {
                        row.Add("Rev.Remarks", code.RevRemarks);
                    }
                    else
                        row.Add("Rev.Remarks", "");

                    if (code.RelRemarks != null)
                    {
                        row.Add("Rel.Remarks", code.RelRemarks);
                    }
                    else
                        row.Add("Rel.Remarks", "");
                    //if (options.Contains("Partno"))
                    //{
                    //    row.Add("Part No", code.Partno);

                    //}
                    //else row.Add("Part No", "");

                    if (options.Contains("Additionalinfo"))
                        row.Add("Additionalinfo", code.Additionalinfo);
                    else row.Add("Additionalinfo", "");

                    if (options.Contains("Equipment"))
                        row.Add("Equipment", code.Equipment);
                    else row.Add("Equipment", "");

                    if (goo == true)
                    {
                        if (options.Contains("Plants"))
                            row.Add("Plant", code.Plant);
                        else row.Add("Plant", "");
                    }
                    if (options.Contains("Characteristics"))
                    {
                        if (code.Characteristics != null)
                        {
                            int i = 1;
                            foreach (var at in code.Characteristics)
                            {
                                row.Add("Attribute" + i, at.Characteristic);
                                row.Add("Value" + i, at.Value);
                                row.Add("UOM" + i, at.UOM);
                                //row.Add("Source" + i, at.Source);
                               // row.Add("SourceUrl" + i, at.SourceUrl);
                                i++;
                            }
                        }
                    }
                    rows.Add(row);
                }
            }
            //foreach (var ct in rows.OrderByDescending(s => s.Keys.Count))
            //{
            //    rows.Add(ct);
            //    break;
            //}
            return rows;
        }
        public List<Dictionary<string, object>> loaddata1(string[] options, string where, string value, string fromdate, string todate, string role, string status)
        {

            IMongoQuery query2;
            var fields = Fields.Include(options).Exclude("_id");
        
            query2 = Query.EQ("ItemStatus", -1);
            var datalist = _UserreportRepository.FindAll(fields, query2).ToList();
          //  var NM_master = _NMRepository.FindAll();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            bool goo;
            var mergelist = (dynamic)null;
            if (options.Contains("Plants"))
            {
                var plntlist = _ERPInfoRepository.FindAll().ToList();
                 mergelist = (from data in datalist join plant in plntlist on data.Itemcode equals plant.Itemcode select new { Plant = plant.Plant != null ? plant.Plant : "", Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks , Additionalinfo =data.Additionalinfo }).ToList();
               // mergelist = (from data in datalist join plant in plntlist on data.Itemcode equals plant.Itemcode join nm in NM_master on data.Noun equals nm.Noun where data.Modifier == nm.Modifier select new { Plant = plant.Plant != null ? plant.Plant : "", Itemcode = data.Itemcode, Formatted = nm.Formatted, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, Soureurl = data.Soureurl }).ToList();
                if (mergelist.Count == 0)
                {
                    mergelist = datalist;
                    goo = false;

                }
                else
                {
                    goo = true;
                }
            }
            else
            {
                mergelist = (from data in datalist select new { Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Additionalinfo = data.Additionalinfo }).ToList();
                //  mergelist = datalist;

                // mergelist = (from data in datalist join nm in NM_master on data.Noun equals nm.Noun where data.Modifier == nm.Modifier select new { Itemcode = data.Itemcode, Formatted = nm.Formatted, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, Soureurl = data.Soureurl }).ToList();
                goo = false;
            }
            if (options.Contains("Characteristics"))
            {
                int flg = 0;
                foreach (var code in mergelist)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Itemcode", code.Itemcode);
                    row.Add("Material code", code.Materialcode);

                    if (options.Contains("Noun"))
                        row.Add("Noun", code.Noun);
                    else
                        row.Add("Noun", "");
                    if (options.Contains("Modifier"))
                        row.Add("Modifier", code.Modifier);
                    else
                        row.Add("Modifier", "");
                    if (options.Contains("UOM"))
                        row.Add("UOM", code.UOM);
                    else
                        row.Add("UOM", "");
                    if (options.Contains("Legacy"))
                        row.Add("Legacy", code.Legacy);
                    else
                        row.Add("Legacy", "");
                    if (options.Contains("Legacy2"))
                        row.Add("PvData", code.Legacy2);
                    else
                        row.Add("PvData", "");
                    if (options.Contains("Shortdesc"))
                        row.Add("Shortdesc", code.Shortdesc);
                    else
                        row.Add("Shortdesc", "");
                    if (options.Contains("Longdesc"))
                        row.Add("Longdesc", code.Longdesc);
                    else
                        row.Add("Longdesc", "");
                    if (options.Contains("Additionalinfo"))
                        row.Add("Additionalinfo", code.Additionalinfo);
                    else
                        row.Add("Additionalinfo", "");
                    //if (code.Formatted == 0)

                    //    row.Add("nmtype", "OEM");

                    //else if (code.Formatted == 1)

                    //    row.Add("nmtype", "Genric");

                    //else if (code.Formatted == 2)
                    //    row.Add("nmtype", "OPM");

                    if (options.Contains("Vendorsuppliers"))

                        row.Add("Vendor Details", code.Manufacturer);
                    else row.Add("Vendor Details", "");

                    if (options.Contains("Equipment"))
                        row.Add("Equipment", code.Equipment);
                    else row.Add("Equipment", "");

                    //if (options.Contains("Partno"))
                    //{
                    //    row.Add("Part No", code.Partno);
                    //}

                    if (code.Catalogue != null)
                    {
                        row.Add("Cataloguer", code.Catalogue.Name);
                        if (code.Catalogue.UpdatedOn != null)
                        {
                            row.Add("Cat-Createdon", code.Catalogue.UpdatedOn);
                        }
                        else
                            row.Add("Cat-Createdon", "");
                    }
                    else
                    {
                        row.Add("Cataloguer", "");
                        row.Add("Cat-Createdon", "");
                    }



                    if (code.Review != null)
                    {
                        row.Add("Reviewer", code.Review.Name);
                        if (code.Review.UpdatedOn != null)
                        {
                            row.Add("Rev-Createdon", code.Review.UpdatedOn);
                        }
                        else
                            row.Add("Rev-Createdon", "");

                    }
                    else
                    {
                        row.Add("Reviewer", "");
                        row.Add("Rev-Createdon", "");
                    }


                    if (code.Release != null)
                    {
                        row.Add("Releaser", code.Release.Name);
                        if (code.Release.UpdatedOn != null)
                        {
                            row.Add("Rel-Createdon", code.Release.UpdatedOn);
                        }
                        else
                            row.Add("Rel-Createdon", "");

                    }
                    else
                    {
                        row.Add("Releaser", "");
                        row.Add("Rel-Createdon", "");
                    }


                    if (code.RejectedBy != null)
                    {
                        row.Add("RejectedBy", code.RejectedBy.Name);
                        if (code.RejectedBy.UpdatedOn != null)
                        {
                            row.Add("RejectedOn", code.RejectedBy.UpdatedOn);
                        }
                        else
                            row.Add("RejectedOn", "");

                    }
                    else
                    {
                        row.Add("RejectedBy", "");
                        row.Add("RejectedOn", "");
                    }
                    //if (code.Remarks != null)
                    //{
                    //    row.Add("Remarks", code.Remarks);
                    //}
                    //else row.Add("Remarks", "");
                    if (code.Remarks != null)
                    {
                        row.Add("Cat.Remarks", code.Remarks);
                    }
                    else
                        row.Add("Cat.Remarks", "");

                    if (code.RevRemarks != null)
                    {
                        row.Add("Rev.Remarks", code.RevRemarks);
                    }
                    else
                        row.Add("Rev.Remarks", "");

                    if (code.RelRemarks != null)
                    {
                        row.Add("Rel.Remarks", code.RelRemarks);
                    }
                    else
                        row.Add("Rel.Remarks", "");
                    if (goo == true)
                    {
                        if (options.Contains("Plants"))
                            row.Add("Plant", code.Plant);
                    }
                    if (options.Contains("Characteristics"))
                    {
                        if (code.Characteristics != null)
                        {
                            int i = 1;
                            foreach (var at in code.Characteristics)
                            {
                                row.Add("Attribute" + i, at.Characteristic);
                                row.Add("Value" + i, at.Value);
                                row.Add("UOM" + i, at.UOM);
                               // row.Add("Source" + i, at.Source);
                               // row.Add("SourceUrl" + i, at.SourceUrl);
                                i++;
                            }
                        }
                    }
                    rows.Add(row);
                }
            }
            else
            {
                foreach (var code in mergelist)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Itemcode", code.Itemcode);
                    row.Add("Matrial Code", code.Materialcode);
                    if (options.Contains("Noun"))
                        row.Add("Noun", code.Noun);
                    else
                        row.Add("Noun", "");
                    if (options.Contains("Modifier"))
                        row.Add("Modifier", code.Modifier);
                    else
                        row.Add("Modifier", "");
                    if (options.Contains("UOM"))
                        row.Add("UOM", code.UOM);
                    else
                        row.Add("UOM", "");
                    if (options.Contains("Legacy"))
                        row.Add("Legacy", code.Legacy);
                    else
                        row.Add("Legacy", "");
                    if (options.Contains("Legacy2"))
                        row.Add("PvData", code.Legacy2);
                    else
                        row.Add("PvData", "");
                    if (options.Contains("Shortdesc"))
                        row.Add("Shortdesc", code.Shortdesc);
                    else
                        row.Add("Shortdesc", "");
                    if (options.Contains("Longdesc"))
                        row.Add("Longdesc", code.Longdesc);
                    else
                        row.Add("Longdesc", "");
                    if (options.Contains("Additionalinfo"))
                        row.Add("Additionalinfo", code.Additionalinfo);
                    else
                        row.Add("Additionalinfo", "");

                    if (options.Contains("Vendorsuppliers"))

                        row.Add("Vendor Details", code.Manufacturer);
                    else row.Add("Vendor Details", "");

                    if (options.Contains("Equipment"))
                        row.Add("Equipment", code.Equipment);
                    else row.Add("Equipment", "");

                    if (code.Catalogue != null)
                    {
                        row.Add("Cataloguer", code.Catalogue.Name);
                        if (code.Catalogue.UpdatedOn != null)
                        {
                            row.Add("Cat-Createdon", code.Catalogue.UpdatedOn);
                        }
                        else
                            row.Add("Cat-Createdon", "");
                    }
                    else
                    {
                        row.Add("Cataloguer", "");
                        row.Add("Cat-Createdon", "");
                    }



                    if (code.Review != null)
                    {
                        row.Add("Reviewer", code.Review.Name);
                        if (code.Review.UpdatedOn != null)
                        {
                            row.Add("Rev-Createdon", code.Review.UpdatedOn);
                        }
                        else
                            row.Add("Rev-Createdon", "");

                    }
                    else
                    {
                        row.Add("Reviewer", "");
                        row.Add("Rev-Createdon", "");
                    }


                    if (code.Release != null)
                    {
                        row.Add("Releaser", code.Release.Name);
                        if (code.Release.UpdatedOn != null)
                        {
                            row.Add("Rel-Createdon", code.Release.UpdatedOn);
                        }
                        else
                            row.Add("Rel-Createdon", "");

                    }
                    else
                    { 
                        row.Add("Releaser", "");
                        row.Add("Rel-Createdon", "");
                    }


                    if (code.RejectedBy != null)
                    {
                        row.Add("RejectedBy", code.RejectedBy.Name);
                        if (code.RejectedBy.UpdatedOn != null)
                        {
                            row.Add("RejectedOn", code.RejectedBy.UpdatedOn);
                        }
                        else
                            row.Add("RejectedOn", "");

                    }
                    else
                    { 
                        row.Add("RejectedBy", "");
                        row.Add("RejectedOn", "");
                    }
                    //if (code.Remarks != null)
                    //{
                    //    row.Add("Remarks", code.Remarks);
                    //}
                    //else row.Add("Remarks", "");
                    if (code.Remarks != null)
                    {
                        row.Add("Cat.Remarks", code.Remarks);
                    }
                    else
                        row.Add("Cat.Remarks", "");

                    if (code.RevRemarks != null)
                    {
                        row.Add("Rev.Remarks", code.RevRemarks);
                    }
                    else
                        row.Add("Rev.Remarks", "");

                    if (code.RelRemarks != null)
                    {
                        row.Add("Rel.Remarks", code.RelRemarks);
                    }
                    else
                        row.Add("Rel.Remarks", "");

                    //if (options.Contains("Partno"))
                    //{
                    //    row.Add("Part No", code.Partno);

                    //}
                                  

                    if (options.Contains("Plants"))
                        if (goo == true)
                        {
                            row.Add("Plant", code.Plant);
                        }
                    if (options.Contains("Characteristics"))
                        if (code.Characteristics != null)
                        {
                            int i = 1;
                            foreach (var at in code.Characteristics)
                            {
                                row.Add("Attribute" + i, at.Characteristic);
                                row.Add("Value" + i, at.Value);
                                row.Add("UOM" + i, at.UOM);
                                i++;
                            }
                        }
                    rows.Add(row);
                }
            }
            //foreach (var ct in rows.OrderByDescending(s => s.Keys.Count))
            //{
            //    rows.Add(ct);
            //    break;
            //}
            return rows;
        }

        public List<Dictionary<string, object>> loaddata2(string[] options, string fromdate, string todate)
        {

            IMongoQuery query2;
            var datalist = new List<Prosol_Datamaster>();
            var fields = Fields.Include(options).Exclude("_id");
           
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
                date1 = date1.AddDays(1);
                date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
                query2 = Query.And(Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                datalist = _UserreportRepository.FindAll(fields, query2).ToList();
            }
            else if (!string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                query2 = Query.And(Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                datalist = _UserreportRepository.FindAll(fields, query2).ToList();
            }
           
         


            //var fields = Fields.Include(options).Exclude("_id");
            //var datalist = _UserreportRepository.FindAll(fields).ToList();

           // var NM_master = _NMRepository.FindAll();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            bool goo;
            var mergelist = (dynamic)null;
            var plntlist = _ERPInfoRepository.FindAll().ToList();
            // mergelist = (from data in datalist join plant in plntlist on data.Itemcode equals plant.Itemcode join nm in NM_master on data.Noun equals nm.Noun where data.Modifier == nm.Modifier select new { Plant = plant.Plant != null ? plant.Plant : "", Itemcode = data.Itemcode, Materialcode = data.Materialcode, Formatted = nm.Formatted, Noun = data.Noun, Modifier = data.Modifier, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Soureurl = data.Soureurl }).ToList();
            mergelist = (from data in datalist select new { Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Soureurl = data.Soureurl, Additionalinfo = data.Additionalinfo }).ToList();
          //  mergelist = (from data in datalist join plant in plntlist on data.Itemcode equals plant.Itemcode select new { Plant = plant.Plant != null ? plant.Plant : "", Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks , RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Soureurl = data.Soureurl, Additionalinfo =data.Additionalinfo }).ToList();
            if (mergelist.Count == 0)
                {
                    mergelist = datalist;
                    goo = false;

                }
                else
                {
                    goo = true;
                }
           
                 int flg = 0;
                foreach (var code in mergelist)
                {
                    row = new Dictionary<string, object>();
               
                    row.Add("Itemcode", code.Itemcode);

                    row.Add("Material code", code.Materialcode);

                if (options.Contains("Noun"))
                    row.Add("Noun", code.Noun);
                else
                    row.Add("Noun", "");
                if (options.Contains("Modifier"))
                    row.Add("Modifier", code.Modifier);
                else
                    row.Add("Modifier", "");
                if (options.Contains("UOM"))
                    row.Add("UOM", code.UOM);
                else
                    row.Add("UOM", "");
                if (options.Contains("Legacy"))
                    row.Add("Legacy", code.Legacy);
                else
                    row.Add("Legacy", "");
                if (options.Contains("Legacy2"))
                    row.Add("PvData", code.Legacy2);
                else
                    row.Add("PvData", "");
                if (options.Contains("Shortdesc"))
                    row.Add("Shortdesc", code.Shortdesc);
                else
                    row.Add("Shortdesc", "");
                if (options.Contains("Longdesc"))
                    row.Add("Longdesc", code.Longdesc);
                else
                    row.Add("Longdesc", "");
                if (options.Contains("Additionalinfo"))
                    row.Add("Additionalinfo", code.Additionalinfo);
                else
                    row.Add("Additionalinfo", "");

                if (options.Contains("Vendorsuppliers"))

                    row.Add("Vendor Details", code.Manufacturer);
                else row.Add("Vendor Details", "");

                if (options.Contains("Equipment"))
                    row.Add("Equipment", code.Equipment);
                else row.Add("Equipment", "");
                //if (options.Contains("Partno"))
                //    {
                //        row.Add("Part No", code.Partno);
                //    }
                //if (options.Contains("Additionalinfo"))
                //    row.Add("Additionalinfo", code.Additionalinfo);
                //else
                //    row.Add("Additionalinfo", "");
                //if (code.Formatted == 0)

                //    row.Add("nmtype", "OEM");

                //else if (code.Formatted == 1)

                //    row.Add("nmtype", "Genric");

                //else if (code.Formatted == 2)
                //    row.Add("nmtype", "OPM");

                if (code.Catalogue != null)
                {
                    row.Add("Cataloguer", code.Catalogue.Name);
                    if (code.Catalogue.UpdatedOn != null)
                    {
                        row.Add("Cat-Createdon", code.Catalogue.UpdatedOn);
                    }
                    else
                        row.Add("Cat-Createdon", "");
                }
                else
                {
                    row.Add("Cataloguer", "");
                    row.Add("Cat-Createdon", "");
                }



                if (code.Review != null)
                {
                    row.Add("Reviewer", code.Review.Name);
                    if (code.Review.UpdatedOn != null)
                    {
                        row.Add("Rev-Createdon", code.Review.UpdatedOn);
                    }
                    else
                        row.Add("Rev-Createdon", "");

                }
                else
                {
                    row.Add("Reviewer", "");
                    row.Add("Rev-Createdon", "");
                }


                if (code.Release != null)
                {
                    row.Add("Releaser", code.Release.Name);
                    if (code.Release.UpdatedOn != null)
                    {
                        row.Add("Rel-Createdon", code.Release.UpdatedOn);
                    }
                    else
                        row.Add("Rel-Createdon", "");

                }
                else
                {
                    row.Add("Releaser", "");
                    row.Add("Rel-Createdon", "");
                }


                if (code.RejectedBy != null)
                {
                    row.Add("RejectedBy", code.RejectedBy.Name);
                    if (code.RejectedBy.UpdatedOn != null)
                    {
                        row.Add("RejectedOn", code.RejectedBy.UpdatedOn);
                    }
                    else
                        row.Add("RejectedOn", "");

                }
                else
                {
                    row.Add("RejectedBy", "");
                    row.Add("RejectedOn", "");
                }


                if (code.ItemStatus == 0 || code.ItemStatus == 1)
                    row.Add("ItemStatus", "Catalogue");
                else if (code.ItemStatus == 2 || code.ItemStatus == 3)
                    row.Add("ItemStatus", "QC");
                else if (code.ItemStatus == 4 || code.ItemStatus == 5)
                    row.Add("ItemStatus", "QA");
                else if (code.ItemStatus == 6)
                    row.Add("ItemStatus", "Released");
                else row.Add("ItemStatus", "Clarification");

                //row.Add("Cat.Remarks", code.Remarks);
                //row.Add("Rev.Remarks", code.RevRemarks);
                //row.Add("Rel.Remarks", code.RelRemarks);
                if (code.Remarks != null)
                {
                    row.Add("Cat.Remarks", code.Remarks);
                }
                else
                    row.Add("Cat.Remarks", "");

                if (code.RevRemarks != null)
                {
                    row.Add("Rev.Remarks", code.RevRemarks);
                }
                else
                    row.Add("Rev.Remarks", "");

                if (code.RelRemarks != null)
                {
                    row.Add("Rel.Remarks", code.RelRemarks);
                }
                else
                    row.Add("Rel.Remarks", "");
                if (goo == true)
                    {
                        if (options.Contains("Plants"))
                            row.Add("Plant", code.Plant);
                    }
                    if (options.Contains("Characteristics"))
                        if (code.Characteristics != null)
                        {
                            int i = 1;
                            foreach (var at in code.Characteristics)
                        {
                               if (at.Characteristic !=null)
                                row.Add("Attribute" + i, at.Characteristic);                              
                                row.Add("Value" + i, at.Value);
                                row.Add("UOM" + i, at.UOM);
                               /// row.Add("Source" + i, at.Source);
                               // row.Add("SourceUrl" + i, at.SourceUrl);
                                i++;
                            }
                        }

                if (code.Soureurl != null)
                {
                    row.Add("Soureurl", code.Soureurl);
                }
                rows.Add(row);
                }
            
           
            return rows;
        }


        public string getequip(Prosol_Datamaster data)
        {
            string mfrr = "";
            string mfrr1 = "";
            if (data.Equipment != null)
          
            {
                if (data.Equipment.Name != null && data.Equipment.Name != "")
                {
                    mfrr += "Name" + ":" + data.Equipment.Name + ",";

                }


                if (data.Equipment.Manufacturer != null && data.Equipment.Manufacturer != "")
                {
                    mfrr += "Manufacturer" + ":" + data.Equipment.Manufacturer + ",";

                }
                if (data.Equipment.Modelno != null && data.Equipment.Modelno != "")
                {
                    mfrr += "Modelno" + ":" + data.Equipment.Modelno + ",";

                }


                if (data.Equipment.Tagno != null && data.Equipment.Tagno != "")
                {
                    mfrr += "Tagno" + ":" + data.Equipment.Tagno + ",";

                }
                if (data.Equipment.Serialno != null && data.Equipment.Serialno != "")
                {
                    mfrr += "Serialno" + ":" + data.Equipment.Serialno + ",";

                }


                if (data.Equipment.Additionalinfo != null && data.Equipment.Additionalinfo != "")
                {
                    mfrr += "Additionalinfo" + ":" + data.Equipment.Additionalinfo + ",";

                }
                if(mfrr.Length != 0)
                {
                    mfrr1 = mfrr.Remove(mfrr.Length - 1);
                }
               

            }
          
            return mfrr1;
        }

        public string getmfr(Prosol_Datamaster data)
        {
            string mfrr = "";
            string mfrr1 = "";
            if (data.Vendorsuppliers != null)
                foreach (Vendorsuppliers vs in data.Vendorsuppliers)
                {
                    //if (vs.s == 1)
                    //{ v
                    //    return vs.Name;
                    //}
                    //if(vs.Name != null && vs.Name != "")
                    //{
                    //    if(!mfrr.Contains(vs.Name))
                    //    mfrr = mfrr == "" ? vs.Name : mfrr +"/"+ vs.Name;
                    //}
                    //  int g = 0;
                    // if (vs.l == 1 && g == 0 && vs.s == 1 && vs.Name != null && vs.Name != "")


                    //if ( vs.Name != null && vs.Name != "" && vs.RefNo != null && vs.RefNo != "")
                    //{
                    //    mfrr += vs.Type + ":" + vs.Name + "," + vs.Refflag + ":" + vs.RefNo + ",";

                    //}
                    if (vs.Name != null && vs.Name != "")
                    {
                        mfrr += vs.Type + ":" + vs.Name + ",";

                    }


                    if (vs.RefNo != null && vs.RefNo != "")
                    {
                        mfrr += vs.Refflag + ":" + vs.RefNo + "/";
                        mfrr1 = mfrr.Remove(mfrr.Length - 1);
                    }


                }
            return mfrr1; ;
        }

        //public IEnumerable<Vendorsuppliers> getmfr(Prosol_Datamaster data)
        //{
        //    var lst = new List<Vendorsuppliers>();
        //    if (data.Vendorsuppliers.Count > 0)
        //    {

        //        foreach (Vendorsuppliers vs in data.Vendorsuppliers)
        //        {
        //            var obj = new Vendorsuppliers();
        //            obj.Name =vs.Name;
        //            obj.RefNo=vs.RefNo;
        //            obj.Refflag = vs.Refflag;
        //            obj.Type = vs.Type;
        //            lst.Add(obj);
        //        }


        //    }
        //    return lst;
        //}


        public IEnumerable<Prosol_Plants> getplant()
        {
            var query =  Query.EQ("Islive", true);
            var mxplnt = _UserplntlistRepository.FindAll(query).ToList();
            return mxplnt;
        }

        public IEnumerable<Prosol_Users> getuser(string role, string[] plants)
        {


            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", role), Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Islive", "Active"));
            var mxplnt = _UserlistRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }

        public IEnumerable<Prosol_Users> getuseronly(string username)
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("UserName", username), Query.EQ("Islive", "Active"));
            var mxplnt = _UserlistRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }

        public IEnumerable<Prosol_Datamaster> getcode(string role)
        {
            string[] itemfld = { "Itemcode" };
            var fields = Fields.Include(itemfld).Exclude("_id");
            var query = Query.EQ("ItemStatus",Convert.ToInt16(role));
            var codelist = _UserreportRepository.FindAll(fields, query).ToList();
            return codelist;
        }

        public IEnumerable<Prosol_Departments> getdepartment()
        {
            var sort = SortBy.Ascending("Departmentname");
            string[] depfield = { "Departmentname" };
            var fields = Fields.Include(depfield).Exclude("_id");
            var gtdepartment = _UserdeplistRepository.FindAll(fields).ToList();
            return gtdepartment;
        }

     
        public IEnumerable<Prosol_Datamaster> trackmulticodelist(string codestr)
        {
          //  string[] search_field = { "Itemcode", "Legacy", "Noun", "Modifier", "Legacy2" };
           // var fields = Fields.Include(search_field).Exclude("_id");
            var query = Query.Or(Query.EQ("Itemcode", codestr), (Query.EQ("Materialcode", codestr)));
            var getdata = _UserreportRepository.FindAll(query).ToList();
            return getdata;
        }

        public IEnumerable<Prosol_ERPInfo> getplant(string Itemcode)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var getplant = _ERPInfoRepository.FindAll(query).ToList();
            return getplant;

        }
        public IEnumerable<Prosol_Datamaster> bulkv()
        {
            var get = _UserreportRepository.FindAll().ToList();

            return get;

        }
        
        public IEnumerable<Prosol_Charateristics> bulkchar()
        {

            var sort = SortBy.Ascending("Noun");

            //  string[] depfield = { "Noun", "Modifier", "Characteristic", "Values", "_id" };
            // var fields = Fields.Include(depfield);

            var get = _charRepository.FindAll(sort).ToList();
            return get;

        }
        public IEnumerable<Prosol_Charateristics> bulkchar1(string cat,string cat1)
        {


           string[] str1 = { "Noun", "Modifier" ,"Characteristic", "Values", "_id" };
            var fields = Fields.Include(str1);
            var query = Query.And(Query.EQ("Noun", cat), Query.EQ("Modifier", cat1));

            var val = _charRepository.FindAll(fields,query).ToList();
            return val;
        }



        public Prosol_Abbrevate getvalue(string id)
        {
        var q = Query.EQ("_id", new ObjectId(id));
        var q1 = _AbbrevateRepository.FindOne(q);
            return q1;

    }

        public IEnumerable<Prosol_Datamaster> bulkvv(string term)
        {
            string[] str1 = { "Itemcode", "Vendorsuppliers" };
            var fields = Fields.Include(str1);

            if (term.Contains('*'))
            {


                var QryLst = new List<IMongoQuery>();

                string[] sepArr = term.Split('*');
                if (sepArr.Length > 2)
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            var Qry1 = Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));



                            QryLst.Add(Qry1);

                        }
                    }
                }
                else
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            if (term.IndexOf('*') > 0)
                            {
                                //Start with
                                var Qry1 = Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));



                                QryLst.Add(Qry1);

                            }
                            else
                            {
                                //End with

                                var Qry1 = Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase)));




                                QryLst.Add(Qry1);


                            }

                        }
                    }


                }
                var query = Query.Or(QryLst);
                var arrResult = _UserreportRepository.FindAll(fields, query).ToList();
                return arrResult;


            }
            else
            {


                    var qry1 = Query.EQ("Itemcode", term);

                var Result = _UserreportRepository.FindAll(qry1).ToList();
                return Result;
            }

        }

        private Dictionary<string, object> BuildBaseRow(dynamic cde)
        {
            var row = new Dictionary<string, object>();
            row.Add("Item Code", cde.Itemcode);
            row.Add("Material Code", cde.Materialcode);
            row.Add("Legacy", cde.Legacy);
            row.Add("Long Legacy", cde.Legacy2);
            row.Add("Old Material Code", cde.exMaterialcode);
            row.Add("Short Desc", cde.Shortdesc);
            row.Add("Long Desc", cde.Longdesc);
            row.Add("UOM", cde.UOM);
            row.Add("Addtional Info", cde.Additionalinfo ?? "");
            row.Add("Quantity Check Remarks", cde.des_remark ?? "");
            row.Add("QR Remarks", cde.barcode_rm ?? "");
            row.Add("NM Remarks", cde.Remarks ?? "");
            row.Add("PV Remarks", cde.PVRemarks ?? "");
            row.Add("Item Status", cde.Specification);

            row.Add("PV User", (cde.PVuser != null && cde.PVuser.Name != null) ? cde.PVuser.Name : "");

            if (cde.PVuser != null && cde.PVuser.UpdatedOn != null)
            {
                DateTime date = DateTime.Parse(Convert.ToString(cde.PVuser.UpdatedOn));
                row.Add("PV Completed Date", date.ToString("dd/MM/yyyy"));
            }
            else
            {
                row.Add("PV Completed Date", "");
            }

            //if (cde.AssetImages != null &&
            //    ((cde.AssetImages.AssetImgs != null && cde.AssetImages.AssetImgs.Any()) ||
            //     (cde.AssetImages.NameplateImgs != null && cde.AssetImages.NameplateImgs.Any())))
            //{
            //    row.Add("Image Status", "Image Available");
            //}
            //else
            //{
            //    row.Add("Image Status", "Image Not Available");
            //}

            return row;
        }

        public  List<Dictionary<string, object>> Trackload(string plant, string fromdate, string todate, string option)
        {
            {
                IMongoQuery qury1, qury2, qury3, qury4, quryplnt;
                if (option == "Created")
                {
                    qury1 = Query.EQ("ItemStatus", 6);
                }
                else if (option == "Duplicate")
                {
                    qury1 = Query.And(Query.NE("Duplicates", BsonNull.Value), Query.NE("Duplicates", ""));
                }
                else if (option == "Request")
                {
                    qury1 = Query.EQ("ItemStatus", 6);
                }
                else if (option == "PVcompleted")
                {
                    //qury1 = Query.And(Query.Or(Query.EQ("ItemStatus", 13), Query.EQ("ItemStatus", 0)), Query.EQ("PVstatus", "Completed"));
                    qury1 = Query.And(Query.EQ("PVstatus", "Completed"), Query.NE("Rework", "PV"));
                }
                else if (option == "Overall")
                {
                    qury1 = Query.And(Query.GTE("ItemStatus", -1), Query.LTE("ItemStatus", 13));
                }
                else if (option == "Delivery")
                {
                    //qury1 = Query.And(Query.GT("ItemStatus", 0), Query.LT("ItemStatus", 7));
                    qury1 = Query.GTE("ItemStatus", 2);
                }

                //else if (option == "Overall")
                //{
                //    qury1 = Query.And(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex("AD".TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.NE("Materialcode", BsonNull.Value));
                //}

                else
                {
                    qury1 = Query.EQ("ItemStatus", -1);


                }

                if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
                {

                    var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                    date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
                    date1 = date1.AddDays(1);
                    date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);

                    if (option == "Created")
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                    }
                    else if (option == "Duplicate")
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                    }
                    else if (option == "Request")
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                    }
                    else if (option == "PVcompleted")
                    {
                        qury2 = Query.And(qury1, Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
                    }
                    else if (option == "Overall")
                    {
                        qury2 = qury1;
                        //qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                    }
                    else if (option == "Delivery")
                    {
                        qury2 = Query.And(qury1, Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
                    }
                    //else if (option == "Overall")
                    //{
                    //    qury2 = qury1;
                    //}
                    else
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                    }



                }
                else if (!string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
                {
                    var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                    date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    if (option == "Created")
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }
                    else if (option == "Duplicate")
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }
                    else if (option == "Request")
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }
                    else if (option == "PVcompleted")
                    {
                        qury2 = Query.And(qury1, Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }
                    else if (option == "Overall")
                    {
                        qury2 = Query.And(qury1, Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }
                    else if (option == "Delivery")
                    {
                        qury2 = Query.And(qury1, Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }
                    //else if (option == "Overall")
                    //{
                    //    qury2 = qury1;
                    //}
                    else
                    {
                        qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                    }


                }
                else
                {
                    qury2 = qury1;

                }
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;
                bool goo1;
                var mergelist1 = (dynamic)null;

                var comLst = _unspsclistRepository.FindAll().ToList();

                if (option == "Created")
                {
                    // string[] flds = { "Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Catalogue.Name", "Review.Name", "Release.Name", "Remarks" };
                    // var fields = Fields.Include(flds).Exclude("_id");
                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    // quryplnt = Query.EQ("Plant", plant);
                    // var plntlist = _ERPInfoRepository.FindAll(quryplnt).ToList();
                    //mergelist1 = (from data in datalist1 join plnt in plntlist on data.Itemcode equals plnt.Itemcode select new { Plant = plnt.Plant, Itemcode = data.Itemcode, Materialcode = data.Materialcode, CreatedOn = data.UpdatedOn, Legacy = data.Legacy, Shortdesc = data.Shortdesc, Longdesc = data.Longdesc, Catalogue = data.Catalogue != null ? data.Catalogue.Name : "", Review = data.Review != null ? data.Review.Name : "", Release = data.Release != null ? data.Release.Name : "", Remarks = data.Remarks }).ToList();
                    //if (mergelist1.Count == 0)
                    //{
                    //    goo1 = false;
                    //}
                    //else
                    //{
                    //    goo1 = true;
                    //}
                    foreach (var cde in datalist1)
                    {

                        row = new Dictionary<string, object>();

                        row.Add("Item Code", cde.Itemcode);
                        row.Add("Material Code", cde.Materialcode);
                        if (cde.Catalogue != null)
                        {
                            if (cde.Catalogue.UpdatedOn != null)
                                row.Add("Created On", cde.Catalogue.UpdatedOn);
                            else
                                row.Add("Created On", "");
                        }
                        else
                            row.Add("Created On", "");
                        if (cde.UpdatedOn != null)
                        {
                            DateTime date = DateTime.Parse(Convert.ToString(cde.UpdatedOn));
                            row.Add("UpdatedOn", date.ToString("dd/MM/yyyy"));
                        }
                        else row.Add("UpdatedOn", "");
                        row.Add("Legacy", cde.Legacy);
                        row.Add("Shortdesc", cde.Shortdesc);
                        row.Add("Longdesc", cde.Longdesc);
                        row.Add("Existing Manufacturer", cde.exManufacturer);
                        row.Add("Existing Partno", cde.exPartno);
                        if (cde.Catalogue != null)
                            row.Add("Cataloguer", cde.Catalogue.Name);
                        else row.Add("Cataloguer", "");

                        if (cde.Review != null)
                            row.Add("QC", cde.Review.Name);
                        else row.Add("QC", "");

                        if (cde.Release != null)
                            row.Add("QA", cde.Release.Name);
                        else row.Add("QA", "");


                        row.Add("Remarks", cde.Remarks);
                        rows.Add(row);
                    }
                }
                else if (option == "Duplicate")
                {
                    // string[] flds = { "Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Catalogue.Name", "Review.Name", "Release.Name", "Remarks" };
                    // var fields = Fields.Include(flds).Exclude("_id");
                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    //quryplnt = Query.EQ("Plant", plant);
                    //var plntlist = _ERPInfoRepository.FindAll(quryplnt).ToList();
                    //mergelist1 = (from data in datalist1 join plnt in plntlist on data.Itemcode equals plnt.Itemcode orderby data.Itemcode select new { Plant = plnt.Plant, Itemcode = data.Itemcode, Materialcode=data.Materialcode, CreatedOn = data.UpdatedOn, Legacy = data.Legacy, Shortdesc = data.Shortdesc, Longdesc = data.Longdesc, Catalogue = data.Catalogue != null ? data.Catalogue.Name : "", Review = data.Review != null ? data.Review.Name : "", Release = data.Release != null ? data.Release.Name : "", Remarks = data.Remarks,Duplicate=data.Duplicates }).ToList();
                    //if (mergelist1.Count == 0)
                    //{
                    //    goo1 = false;
                    //}
                    //else
                    //{
                    //    goo1 = true;
                    //}
                    foreach (var cde in datalist1)
                    {

                        row = new Dictionary<string, object>();
                        row.Add("Item Code", cde.Itemcode);
                        row.Add("Material Code", cde.Materialcode);
                        if (cde.CreatedOn != null)
                        {
                            DateTime date = DateTime.Parse(Convert.ToString(cde.CreatedOn));
                            row.Add("CreatedOn", date.ToString("dd/MM/yyyy"));
                        }
                        else row.Add("CreatedOn", "");
                        row.Add("Legacy", cde.Legacy);
                        row.Add("Shortdesc", cde.Shortdesc);
                        row.Add("Longdesc", cde.Longdesc);
                        row.Add("Existing Manufacturer", cde.exManufacturer);
                        row.Add("Existing Partno", cde.exPartno);
                        if (cde.Catalogue != null)
                            row.Add("Cataloguer", cde.Catalogue.Name);
                        else row.Add("Cataloguer", "");

                        if (cde.Review != null)
                            row.Add("QC", cde.Review.Name);
                        else row.Add("QC", "");

                        if (cde.Release != null)
                            row.Add("QA", cde.Release.Name);
                        else row.Add("QA", "");
                        row.Add("Remarks", cde.Remarks);
                        row.Add("Duplicates", cde.Duplicates);

                        //if (goo1 == true)
                        //{
                        //    row.Add("Plant", cde.Plant);
                        //}
                        rows.Add(row);
                    }

                }
                else if (option == "Request")
                {
                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    quryplnt = Query.EQ("Plant", plant);
                    var plntlist = _ERPInfoRepository.FindAll(quryplnt).ToList();
                    mergelist1 = (from data in datalist1 join plnt in plntlist on data.Itemcode equals plnt.Itemcode where (data.Maincode == "R" || data.Maincode == "D") orderby data.Itemcode select new { Plant = plnt.Plant, Itemcode = data.Itemcode, Materialcode = data.Materialcode, CreatedOn = data.UpdatedOn, Legacy = data.Legacy, Shortdesc = data.Shortdesc, Longdesc = data.Longdesc, Catalogue = data.Catalogue != null ? data.Catalogue.Name : "", Review = data.Review != null ? data.Review.Name : "", Release = data.Release != null ? data.Release.Name : "", Remarks = data.Remarks, Duplicate = data.Duplicates }).ToList();
                    if (mergelist1.Count == 0)
                    {
                        goo1 = false;
                    }
                    else
                    {
                        goo1 = true;
                    }
                    foreach (var cde in mergelist1)
                    {

                        row = new Dictionary<string, object>();
                        //  row.Add("Item Code", cde.Itemcode);
                        row.Add("Material Code", cde.Materialcode);
                        if (cde.Catalogue != null)
                        {
                            if (cde.Catalogue.UpdatedOn != null)
                                row.Add("Created On", cde.Catalogue.UpdatedOn);
                            else
                                row.Add("Created On", "");
                        }
                        else
                            row.Add("Created On", "");
                        row.Add("Legacy", cde.Legacy);
                        row.Add("Shortdesc", cde.Shortdesc);
                        row.Add("Longdesc", cde.Longdesc);
                        row.Add("Existing Manufacturer", cde.exManufacturer);
                        row.Add("Existing Partno", cde.exPartno);
                        row.Add("Cataloguer", cde.Catalogue.Name);
                        row.Add("QC", cde.Review.Name);
                        row.Add("QA", cde.Release.Name);
                        row.Add("Remarks", cde.Remarks);
                        // row.Add("Duplicates", cde.Duplicate);

                        if (goo1 == true)
                        {
                            row.Add("Plant", cde.Plant);
                        }
                        rows.Add(row);
                    }
                }
                else if (option == "PVcompleted")
                {
                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    foreach (var cde in datalist1)
                    {
                        if(cde.Materialcode == "18002072")
                        {

                        }
                        var erp = _ERPInfoRepository.FindOne(Query.EQ("Itemcode", cde.Itemcode));

                        if (cde.StorageLocations != null && cde.StorageLocations.Count() > 0)
                        {
                            foreach (var sl in cde.StorageLocations)
                            {
                                if (sl.DataCollection != null && sl.DataCollection.Count() > 0)
                                {
                                    int sbCnt = 1;
                                    foreach (var sb in sl.DataCollection)
                                    {
                                        row = BuildBaseRow(cde);
                                        row.Add("Storage Location", sl.StorageLocation);
                                        row.Add("Storage Location Desc", sl.StorageLocation_);
                                        row.Add("Storage Bin " + sbCnt, sb.Observation);
                                        row.Add("System Quantity " + sbCnt, sb.sQty);
                                        row.Add("Physical Quantity " + sbCnt, sb.dQty);

                                        // Price / Unit mapping
                                        if (sbCnt == 1) row.Add("Price / Unit " + sbCnt, erp.Price_Unit);
                                        else if (sbCnt == 2) row.Add("Price / Unit " + sbCnt, erp.Price_Unit2);
                                        else if (sbCnt == 3) row.Add("Price / Unit " + sbCnt, erp.Price_Unit3);
                                        else if (sbCnt == 4) row.Add("Price / Unit " + sbCnt, erp.Price_Unit4);
                                        else if (sbCnt == 5) row.Add("Price / Unit " + sbCnt, erp.Price_Unit5);
                                        else row.Add("Price / Unit " + sbCnt, "");

                                        // Currency mapping
                                        if (sbCnt == 1) row.Add("Currency " + sbCnt, erp.Currency);
                                        else if (sbCnt == 2) row.Add("Currency " + sbCnt, erp.Currency2);
                                        else if (sbCnt == 3) row.Add("Currency " + sbCnt, erp.Currency3);
                                        else if (sbCnt == 4) row.Add("Currency " + sbCnt, erp.Currency4);
                                        else if (sbCnt == 5) row.Add("Currency " + sbCnt, erp.Currency5);
                                        else row.Add("Currency " + sbCnt, "");

                                        rows.Add(row);
                                        sbCnt++;
                                    }
                                }
                                else
                                {
                                    row = BuildBaseRow(cde);
                                    row.Add("Storage Location", sl.StorageLocation);
                                    row.Add("Storage Location Desc", sl.StorageLocation_);
                                    row.Add("Storage Bin", "");
                                    row.Add("System Quantity", "");
                                    row.Add("Physical Quantity", "");
                                    row.Add("Price / Unit", "");
                                    row.Add("Currency", "");
                                    rows.Add(row);
                                }
                            }
                        }
                        else
                        {
                            row = BuildBaseRow(cde);
                            row.Add("Storage Location", "");
                            row.Add("Storage Location Desc", "");
                            row.Add("Storage Bin", "");
                            row.Add("System Quantity", "");
                            row.Add("Physical Quantity", "");
                            row.Add("Price / Unit", "");
                            row.Add("Currency", "");
                            rows.Add(row);
                        }
                    }

                }
                else if (option == "Overall")
                {
                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    foreach (var cde in datalist1)
                    {
                        row = new Dictionary<string, object>();

                        if (cde.Catalogue != null)
                        {
                            if (cde.Catalogue.UpdatedOn != null)
                                row.Add("Created On", cde.Catalogue.UpdatedOn);
                            else
                                row.Add("Created On", "");
                        }
                        else
                            row.Add("Created On", "");
                        row.Add("Item Code", cde.Itemcode);
                        row.Add("Material Code", cde.Materialcode);
                        if (cde.CreatedOn != null)
                        {
                            DateTime date = DateTime.Parse(Convert.ToString(cde.CreatedOn));
                            row.Add("CreatedOn", date.ToString("dd/MM/yyyy"));
                        }
                        else row.Add("CreatedOn", "");
                        if (cde.UpdatedOn != null)
                        {
                            DateTime date = DateTime.Parse(Convert.ToString(cde.UpdatedOn));
                            row.Add("UpdatedOn", date.ToString("dd/MM/yyyy"));
                        }
                        else row.Add("UpdatedOn", "");
                        row.Add("Legacy", cde.Legacy);
                        row.Add("Noun", cde.Noun);
                        row.Add("Modifier", cde.Modifier);
                        row.Add("UOM", cde.UOM);
                        row.Add("Label Shortdesc", cde.Shortdesc);
                        row.Add("Shortdesc", cde.Shortdesc_);
                        row.Add("Longdesc", cde.Longdesc);
                        row.Add("Missing Value", cde.MissingValue);
                        row.Add("Exisiting Missing Value", cde.exMissingValue);
                        row.Add("Enriched Value", cde.EnrichedValue);
                        row.Add("Repeated Value", RepeatedValue(cde.Longdesc));

                        for (int i = 1; i <= 35; i++)
                        {
                            if (cde.Characteristics != null && i <= cde.Characteristics.Count)
                            {
                                var attr = cde.Characteristics[i - 1];
                                if (attr.Characteristic != null)
                                {
                                    row.Add("ATTRIBUTE NAME " + i, attr.Characteristic);
                                }
                                else
                                {
                                    row.Add("ATTRIBUTE NAME " + i, "");
                                }

                                if (attr.Value != null)
                                {
                                    row.Add("ATTRIBUTE VALUE " + i, attr.Value);
                                }
                                else
                                {
                                    row.Add("ATTRIBUTE VALUE " + i, "");
                                }

                                if (attr.UOM != null)
                                {
                                    row.Add("ATTRIBUTE UOM " + i, attr.UOM);
                                }
                                else
                                {
                                    row.Add("ATTRIBUTE UOM " + i, "");
                                }
                            }
                            else
                            {
                                row.Add("ATTRIBUTE NAME " + i, "");
                                row.Add("ATTRIBUTE VALUE " + i, "");
                                row.Add("ATTRIBUTE UOM " + i, "");
                            }
                        }
                        if (cde.Equipment != null)
                        {
                            if (!string.IsNullOrEmpty(cde.Equipment.Name))
                            {
                                row.Add("EQUIPMENT NAME", cde.Equipment.Name);
                            }
                            else
                                row.Add("EQUIPMENT NAME", "");
                            if (!string.IsNullOrEmpty(cde.Equipment.Modelno))
                            {
                                row.Add("EQUIPMENT MODELNO", cde.Equipment.Modelno);
                            }
                            else
                                row.Add("EQUIPMENT MODELNO", "");
                            if (!string.IsNullOrEmpty(cde.Equipment.Serialno))
                            {
                                row.Add("EQUIPMENT SERIALNO", cde.Equipment.Serialno);
                            }
                            else
                                row.Add("EQUIPMENT SERIALNO", "");
                            if (!string.IsNullOrEmpty(cde.Equipment.Tagno))
                            {
                                row.Add("EQUIPMENT TAGNO", cde.Equipment.Tagno);
                            }
                            else
                                row.Add("EQUIPMENT TAGNO", "");
                            if (!string.IsNullOrEmpty(cde.Equipment.Manufacturer))
                            {
                                row.Add("EQUIPMENT MANUFACTURE", cde.Equipment.Manufacturer);
                            }
                            else
                                row.Add("EQUIPMENT MANUFACTURE", "");
                            if (!string.IsNullOrEmpty(cde.Equipment.Additionalinfo))
                            {
                                row.Add("EQUIPMENT SPECIFICATION", cde.Equipment.Additionalinfo);
                            }
                            else
                                row.Add("EQUIPMENT SPECIFICATION", "");
                            if (!string.IsNullOrEmpty(cde.Equipment.SuperiorEquipment))
                            {
                                row.Add("SUPERIOR EQUIPMENT", cde.Equipment.SuperiorEquipment);
                            }
                            else
                                row.Add("SUPERIOR EQUIPMENT", "");
                        }
                        else
                        {
                            row.Add("EQUIPMENT NAME", "");
                            row.Add("EQUIPMENT MODELNO", "");
                            row.Add("EQUIPMENT SERIALNO", "");
                            row.Add("EQUIPMENT TAGNO", "");
                            row.Add("EQUIPMENT MANUFACTURE", "");
                            row.Add("EQUIPMENT SPECIFICATION", "");
                            row.Add("SUPERIOR EQUIPMENT", "");
                        }

                        string modelno = "";
                        string brand = "";
                        var designationChar = cde.Characteristics?.FirstOrDefault(c => c.Characteristic == "DESIGNATION");
                        string designation = designationChar?.Value ?? "";

                        string drwno = "";
                        string posno = "";
                        string serial = "";
                        string supplier = "";
                        string suppartno = "";
                        var partLst = new List<string>();
                        var mfrLst = new List<string>();
                        if (cde.Vendorsuppliers != null)
                        {
                            foreach (var ven in cde.Vendorsuppliers)
                            {
                                if (ven.Refflag == "MODEL NUMBER")
                                {
                                    modelno = ven.RefNo;
                                }
                                if (ven.Refflag == "DESIGNATION")
                                {
                                    designation = ven.RefNo;
                                }
                                if (ven.Refflag == "BRAND NAME")
                                {
                                    brand = ven.Name;
                                }
                                if (ven.Refflag == "DRAWING NUMBER")
                                {
                                    drwno = ven.RefNo;
                                }
                                if (ven.Refflag == "POSITION NUMBER")
                                {
                                    posno = ven.RefNo;
                                }
                                if (ven.Refflag == "SERIAL NUMBER")
                                {
                                    serial = ven.RefNo;
                                }
                                if (ven.Refflag == "SUPPLIER PART NUMBER")
                                {
                                    suppartno = ven.RefNo;
                                }
                                if (ven.Type == "SUPPLIER")
                                {
                                    supplier = ven.Name;
                                }
                                if (ven.Refflag == "PART NUMBER")
                                {
                                    partLst.Add(ven.RefNo);
                                }
                                if (ven.Type == "MANUFACTURER")
                                {
                                    mfrLst.Add(ven.Name);
                                }
                            }
                        }
                        row.Add("MODEL NUMBER", modelno);
                        row.Add("BRAND NAME", brand);
                        row.Add("DESIGNATION", designation);
                        for (var i = 0; i < 5; i++)
                        {
                            if (partLst != null && partLst.Count > i)
                            {
                                row.Add("PART NO " + (i + 1), partLst[i] ?? "");
                            }
                            else
                            {
                                row.Add("PART NO " + (i + 1), "");
                            }

                            if (mfrLst != null && mfrLst.Count > i)
                            {
                                row.Add("MFR NAME " + (i + 1), mfrLst[i] ?? "");
                            }
                            else
                            {
                                row.Add("MFR NAME " + (i + 1), "");
                            }

                        }

                        row.Add("DRAWING NUMBER", drwno);
                        row.Add("POSITION NUMBER", posno);
                        row.Add("SERIAL NUMBER", serial);
                        row.Add("VENDOR/SUPPLIER NAME", supplier);
                        row.Add("VENDOR/SUPPLIER PART NO", suppartno);
                        row.Add("STATUS", cde.Specification);
                        row.Add("SOURCE URL", cde.Soureurl);

                        if (cde.Catalogue != null)
                            row.Add("Cataloguer", cde.Catalogue.Name);
                        else row.Add("Cataloguer", "");

                        if (cde.Review != null)
                            row.Add("QC", cde.Review.Name);
                        else row.Add("QC", "");

                        if (cde.Release != null)
                            row.Add("QA", cde.Release.Name);
                        else row.Add("QA", "");


                        if (cde.ItemStatus == -1)
                        {
                            row.Add("ITEM STATUS", "Clarification");
                        }
                        else if (cde.ItemStatus == 11)
                        {
                            row.Add("ITEM STATUS", "PV Pending");
                        }
                        else if (cde.ItemStatus == 0 || cde.ItemStatus == 13)
                        {
                            row.Add("ITEM STATUS", "Catalogue Not Saved");
                        }
                        else if (cde.ItemStatus == 1)
                        {
                            row.Add("ITEM STATUS", "Catalogue Saved");
                        }
                        else if (cde.ItemStatus == 2)
                        {
                            row.Add("ITEM STATUS", "Catalogue Submit");
                        }
                        else if (cde.ItemStatus == 3)
                        {
                            row.Add("ITEM STATUS", "QC Saved");
                        }
                        else if (cde.ItemStatus == 4)
                        {
                            row.Add("ITEM STATUS", "QC Submit");
                        }
                        else if (cde.ItemStatus == 5)
                        {
                            row.Add("ITEM STATUS", "QA Saved");
                        }
                        else if (cde.ItemStatus == 6 && !string.IsNullOrEmpty(cde.category))
                        {
                            row.Add("ITEM STATUS", "Released");
                        }
                        else
                        {
                            row.Add("ITEM STATUS", "Not Assigned");
                        }

                        row.Add("UNSPSC", cde.Unspsc);
                        row.Add("ITEM ADDITIONAL INFO", cde.Additionalinfo ?? "");
                        row.Add("REMARKS", cde.RevRemarks);
                        row.Add("PV REMARKS", cde.PVRemarks ?? "");
                        //row.Add("Batch", cde.Maincode);
                        rows.Add(row);
                    }
                }
                else if (option == "Delivery")
                {
                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    int sNo = 1;
                    foreach (var cde in datalist1)
                    {
                        row = new Dictionary<string, object>();

                        var erp = _ERPInfoRepository.FindOne(Query.EQ("Itemcode", cde.Itemcode));

                        row.Add("S.NO", sNo.ToString());
                        row.Add("Material SAP Code", cde.Materialcode);
                        row.Add("Old Material Number", cde.exMaterialcode);
                        row.Add("Material Description", cde.Legacy);
                        row.Add("SALES_EN_TEXT", cde.Legacy2);
                        row.Add("Bin Location", cde.Bin);
                        row.Add("Main Store", "");
                        row.Add("Valuation Price", "");
                        row.Add("Material Type", erp?.Materialtype ?? "");
                        row.Add("Material Type Description", erp?.Materialtype_ ?? "");
                        row.Add("Material Group", erp?.MaterialStrategicGroup ?? "");
                        row.Add("Material Group Description", erp?.MaterialStrategicGroup_ ?? "");
                        row.Add("MRP Type", erp?.MRPType ?? "");
                        row.Add("MRP Type Description", erp?.MRPType_ ?? "");
                        row.Add("Reorder Point", erp?.ReOrderPoint_ ?? "");
                        row.Add("Maximum Stock Level", erp?.MaxStockLevel_ ?? "");

                        row.Add("ITEMCODE", cde.Itemcode);
                        row.Add("MATERIAL DESCRIPTION (SHORT)", cde.Shortdesc);
                        row.Add("MATERIAL DESCRIPTION (LONG)", cde.Longdesc);
                        row.Add("UOM CODE", cde.UOM);
                        row.Add("UOM DESCRIPTION", "");

                        var unspsc = comLst.Find(i => i.Commodity == cde.Unspsc);

                        row.Add("UNSPSC CODE", cde.Unspsc);
                        row.Add("UNSPSC DESCRIPTION", unspsc?.CommodityTitle ?? "");
                        row.Add("UNSPSC SEGMENT", unspsc?.Segment ?? "");
                        row.Add("UNSPSC SEGMENT DESCRIPTION", unspsc?.SegmentTitle ?? "");
                        row.Add("UNSPSC FAMILY CODE", unspsc?.Family ?? "");
                        row.Add("UNSPSC FAMILY DESCRIPTION", unspsc?.FamilyTitle ?? "");
                        row.Add("UNSPSC CLASS CODE", unspsc?.ClassTitle ?? "");
                        row.Add("UNSPSC CLASS DESCRIPTION", unspsc?.ClassTitle ?? "");
                        row.Add("UNSPSC COMMODITY CODE", unspsc?.Commodity ?? "");
                        row.Add("UNSPSC COMMODITY DESCRIPTION", unspsc?.CommodityTitle ?? "");

                        string modelno = "";
                        string brand = "";
                        string designation = "";
                        string drwno = "";
                        string posno = "";
                        string serial = "";
                        string supplier = "";
                        string suppartno = "";
                        string drwposno = "";
                        var partLst = new List<string>();
                        var mfrLst = new List<string>();
                        if (cde.Vendorsuppliers != null)
                        {
                            foreach (var ven in cde.Vendorsuppliers)
                            {
                                if (ven.Refflag == "MODEL NUMBER")
                                {
                                    modelno = ven.RefNo;
                                }
                                if (ven.Refflag == "DESIGNATION")
                                {
                                    designation = ven.RefNo;
                                }
                                if (ven.Refflag == "BRAND NAME")
                                {
                                    brand = ven.Name;
                                }
                                if (ven.Refflag == "DRAWING NUMBER")
                                {
                                    drwno = ven.RefNo;
                                }
                                if (ven.Refflag == "POSITION NUMBER")
                                {
                                    posno = ven.RefNo;
                                }
                                if (ven.Refflag == "SERIAL NUMBER")
                                {
                                    serial = ven.RefNo;
                                }
                                if (ven.Refflag == "SUPPLIER PART NUMBER")
                                {
                                    suppartno = ven.RefNo;
                                }
                                if (ven.Type == "SUPPLIER")
                                {
                                    supplier = ven.Name;
                                }
                                if (ven.Refflag == "DRAWING & POSITION NUMBER")
                                {
                                    drwposno = ven.RefNo;
                                }
                                if (ven.Refflag == "PART NUMBER")
                                {
                                    partLst.Add(ven.RefNo);
                                }
                                if (ven.Type == "MANUFACTURER")
                                {
                                    mfrLst.Add(ven.Name);
                                }
                            }
                        }

                        for (var i = 0; i < 5; i++)
                        {
                            if (partLst != null && partLst.Count > i)
                            {
                                row.Add("PART NUMBER " + (i + 1), partLst[i] ?? "");
                            }
                            else
                            {
                                row.Add("PART NUMBER " + (i + 1), "");
                            }

                            if (mfrLst != null && mfrLst.Count > i)
                            {
                                row.Add("MANUFACTURER NAME " + (i + 1), mfrLst[i] ?? "");
                            }
                            else
                            {
                                row.Add("MANUFACTURER NAME " + (i + 1), "");
                            }

                        }
                        row.Add("SUPPLIER", supplier);
                        row.Add("MODEL NUMBER", modelno);
                        row.Add("DESIGNATION", designation);
                        row.Add("POSITION NUMBER", posno);
                        row.Add("DRAWING NUMBER", drwno);
                        row.Add("DRAWING & POSITION NUMBER", drwposno);
                        row.Add("SERIAL NUMBER", serial);

                        row.Add("ADDITIONAL INFORMATION", cde.Additionalinfo ?? "");
                        row.Add("ORDERING INFORMATION", "");
                        row.Add("EQUIPMENT NAME", cde.Equipment?.Name ?? "");
                        row.Add("EQUIPMENT MANUFACTURER", cde.Equipment?.Manufacturer ?? "");
                        row.Add("EQUIPMENT MODELNO", cde.Equipment?.Modelno ?? "");
                        row.Add("EQUIPMENT TAGNO", cde.Equipment?.Tagno ?? "");
                        row.Add("EQUIPMENT SERIALNO", cde.Equipment?.Serialno ?? "");
                        row.Add("EQUIPMENT ADDITIONALINFO", cde.Equipment?.Additionalinfo ?? "");
                        row.Add("SOURCEURL 1", cde.Soureurl ?? "");
                        row.Add("STATUS", cde.Specification ?? "");
                        row.Add("NOUN", cde.Noun ?? "");
                        row.Add("MODIFIER", cde.Modifier ?? "");

                        for (int i = 1; i <= 22; i++)
                        {
                            if (cde.Characteristics != null && i <= cde.Characteristics.Count)
                            {
                                var attr = cde.Characteristics[i - 1];
                                if (attr.Characteristic != null)
                                {
                                    row.Add("ATTRIBUTE NAME " + i, attr.Characteristic);
                                }
                                else
                                {
                                    row.Add("ATTRIBUTE NAME " + i, "");
                                }

                                if (attr.Value != null)
                                {
                                    row.Add("ATTRIBUTE VALUE " + i, attr.Value);
                                }
                                else
                                {
                                    row.Add("ATTRIBUTE VALUE " + i, "");
                                }

                                if (!string.IsNullOrEmpty(attr.UOM))
                                {
                                    row.Add("ATTRIBUTE UOM " + i, attr.UOM);
                                }
                                else
                                {
                                    row.Add("ATTRIBUTE UOM " + i, "");
                                }
                            }
                            else
                            {
                                row.Add("ATTRIBUTE NAME " + i, "");
                                row.Add("ATTRIBUTE VALUE " + i, "");
                                row.Add("ATTRIBUTE UOM " + i, "");
                            }
                        }

                        row.Add("MAIN STORE", "");
                        row.Add("BIN LOCATION", erp?.StorageBin);
                        row.Add("CODA VALUATION PRICE", "");
                        row.Add("FSN CLASSIFICATION", "");
                        row.Add("STRATEGIC CLASS", "");
                        row.Add("STRATEGIC / NON-STRATEGIC", "");
                        row.Add("CODA MATERIAL TYPE", erp?.Materialtype);
                        row.Add("CODA MATERIAL TYPE DESCRIPTION", erp?.Materialtype_);
                        row.Add("CODA MATERIAL GROUP", erp?.MaterialStrategicGroup);
                        row.Add("CODA MATERIAL GROUP DESCRIPTION", erp?.MaterialStrategicGroup_);
                        row.Add("MRP TYPE", erp?.MRPType);
                        row.Add("MRP TYPE DESCRIPTION", erp?.MRPType_);
                        row.Add("REORDER POINT", erp?.ReOrderPoint_);
                        row.Add("MAXIMUM STOCK LEVEL", erp?.MaxStockLevel_);
                        row.Add("SHELF LIFE ITEM", "");

                        rows.Add(row);
                        sNo++;
                    }
                }
                //else if (option == "Overall")
                //{
                //    string[] flds = { "Itemcode","Materialcode","Legacy", "Noun", "Modifier", "UOM", "Remarks", "ItemStatus" };
                //    var fields = Fields.Include(flds).Exclude("_id");
                //    var datalist1 = _UserreportRepository.FindAll(fields,qury2).ToList();
                //    foreach (var cde in datalist1)
                //    {
                //        var erp = _ERPInfoRepository.FindOne(Query.EQ("Itemcode", cde.Itemcode));

                //        row = new Dictionary<string, object>
                //        {
                //            {"Itemcode", cde.Itemcode},
                //            {"Material Code", cde.Materialcode ?? string.Empty},
                //            {"Legacy", cde.Legacy ?? string.Empty},
                //            {"Noun", cde.Noun ?? string.Empty},
                //            {"Modifier", cde.Modifier ?? string.Empty},
                //            {"UOM", cde.UOM ?? string.Empty},
                //            {"Storage Location", erp?.StorageLocation ?? string.Empty},
                //            {"Storage Bin", erp?.StorageBin ?? string.Empty},
                //            {"Bin Updation", erp?.StorageBin_ ?? ""},
                //            {"Remarks", cde.Remarks ?? string.Empty},
                //            {"Status", cde.ItemStatus == 13 ? "PV Completed" : cde.ItemStatus == 11 ? "PV Pending": cde.ItemStatus == 0 ? "Catalogue Pending" : cde.ItemStatus == 1 ? "Catalogue Saved" : cde.ItemStatus == 2 ? "Catalogue Completed"  : string.Empty}
                //        };

                //        rows.Add(row);
                //    }
                //}

                else
                {
                    //  string[] flds = { "requestId", "itemId", "source", "plant", "requester", "requestedOn", "approver", "itemStatus", "approvedOn", "rejectedOn" };

                    var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                    quryplnt = Query.EQ("Plant", plant);
                    var plntlist = _ERPInfoRepository.FindAll(quryplnt).ToList();
                    mergelist1 = (from data in datalist1 join plnt in plntlist on data.Itemcode equals plnt.Itemcode orderby data.Itemcode select new { Plant = plnt.Plant, Itemcode = data.Itemcode, Materialcode = data.Materialcode, CreatedOn = data.UpdatedOn, Legacy = data.Legacy, Shortdesc = data.Shortdesc, Longdesc = data.Longdesc, Catalogue = data.Catalogue != null ? data.Catalogue.Name : "", Review = data.Review != null ? data.Review.Name : "", Release = data.Release != null ? data.Release.Name : "", Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, RejectedBy = data.RejectedBy != null ? data.RejectedBy.Name : "", RejectedOn = data.RejectedBy != null ? data.RejectedBy.UpdatedOn : null }).ToList();
                    if (mergelist1.Count == 0)
                    {
                        goo1 = false;
                    }
                    else
                    {
                        goo1 = true;
                    }

                    foreach (var cde in mergelist1)
                    {

                        row = new Dictionary<string, object>();
                        // row.Add("Item Code", cde.Itemcode);
                        row.Add("Material Code", cde.Materialcode);
                        if (cde.CreatedOn != null)
                        {
                            DateTime date = DateTime.Parse(Convert.ToString(cde.CreatedOn));
                            row.Add("CreatedOn", date.ToString("dd/MM/yyyy"));
                        }
                        else row.Add("CreatedOn", "");
                        row.Add("Legacy", cde.Legacy);
                        row.Add("Shortdesc", cde.Shortdesc);
                        row.Add("Longdesc", cde.Longdesc);
                        row.Add("Existing Manufacturer", cde.exManufacturer);
                        row.Add("Existing Partno", cde.exPartno);
                        row.Add("Cataloguer", cde.Catalogue.Name);
                        row.Add("QC", cde.Review.Name);
                        row.Add("QA", cde.Release.Name);

                        row.Add("PVRemarks", cde.PVRemarks ?? "");
                        row.Add("Remarks", cde.Remarks);
                        row.Add("RevRemarks", cde.RevRemarks);
                        row.Add("RelRemarks", cde.RelRemarks);
                        row.Add("RejectedBy", cde.RejectedBy);
                        if (cde.RejectedOn != null)
                        {
                            DateTime date11 = DateTime.Parse(Convert.ToString(cde.RejectedOn));
                            row.Add("RejectedOn", date11.ToString("dd/MM/yyyy"));
                        }
                        else row.Add("RejectedOn", "");

                        //row.Add("RejectedOn", cde.RejectedOn);

                        // row.Add("Duplicates", cde.Duplicate);

                        if (goo1 == true)
                        {
                            row.Add("Plant", cde.Plant);
                        }
                        rows.Add(row);
                    }



                }
                return rows;
            }
        }
        public List<Dictionary<string, object>> Delivery(string plant, string fromdate, string todate, string option)
        {
            IMongoQuery qury1, qury2, quryplnt;
            IMongoSortBy sortField = SortBy.Ascending("Noun");
            if (option == "Duplicate")
            {
                qury1 = Query.And(Query.EQ("ItemStatus", 6),Query.NE("Duplicates", BsonNull.Value), Query.NE("Materialcode", BsonNull.Value),
                    Query.NE("Materialcode", ""), Query.NE("Duplicates", ""),Query.NE("Shortdesc", ""), Query.NE("Shortdesc", BsonNull.Value));
            }
            else if (option == "Clarification")
            {
                qury1 = Query.EQ("ItemStatus", -1);
            }
            else if (option == "Unique")
            {
                qury1 = Query.And(Query.NE("Materialcode", BsonNull.Value),
                    Query.NE("Materialcode", ""), Query.EQ("ItemStatus", 6));
            }
            else if (option == "Critical")
            {
                qury1 = Query.And(Query.NE("Materialcode", BsonNull.Value),Query.NE("Materialcode", ""), Query.EQ("ItemStatus", 6), Query.NE("Shortdesc", ""), Query.NE("Shortdesc", BsonNull.Value));
            }
            else
            {
                qury1 = Query.EQ("ItemStatus", 6);
            }
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {

                var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
                date1 = date1.AddDays(1);
                date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);

                if (option == "Duplicate")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                }
                else if (option == "Clarification")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                }
                else if (option == "Unique")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                }
                else if (option == "Critical")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                }
                else
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                }



            }
            else if (!string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                if (option == "Duplicate")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                }
                else if (option == "Clarification")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                }
                else if (option == "Unique")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                }
                else if (option == "Critical")
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                }
                else
                {
                    qury2 = Query.And(qury1, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
                }


            }
            else
            {
                qury2 = qury1;

            }
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            bool goo1;
            var mergelist1 = (dynamic)null;
            if (option == "Duplicate")
            {
                var datalist1 = _UserreportRepository.FindAll(qury2,sortField).ToList();
                var filterlist = datalist1.Where(x => x.Itemcode != x.Materialcode &&  x.Materialcode == x.Duplicates && x.Duplicates != null &&
               x.Duplicates != "").ToList();
                List<Prosol_Datamaster> SortedList = filterlist.OrderBy(o => o.Materialcode).ToList();
                foreach (var cde in SortedList)
                {
                    row = new Dictionary<string, object>();
                    row.Add("MATERIALCODE", cde.Materialcode);
                    row.Add("ITEMCODE", cde.Itemcode);
                    row.Add("LEGACY", cde.Legacy);
                    var query = Query.And(Query.EQ("Noun", cde.Noun), Query.EQ("Modifier", cde.Modifier));
                    var x = _NMRepository.FindOne(query);
                    if (x != null)
                    {
                        if (x.Formatted == 0)
                        {
                            row.Add("CATEGORY", "OEM");
                        }
                        else if (x.Formatted == 1)
                        {
                            row.Add("CATEGORY", "GENERIC");
                        }
                        else if (x.Formatted == 2)
                        {
                            row.Add("CATEGORY", "OPM");

                        }
                    }
                    else
                    {
                        row.Add("CATEGORY", "");
                    }
                    row.Add("NOUN", cde.Noun);
                    row.Add("MODIFIER", cde.Modifier);
                    row.Add("SHORT DESCRIPTION", cde.Shortdesc);
                    row.Add("UOM", cde.UOM);
                    row.Add("LONG DESCRIPTION", cde.Longdesc);
                    if (cde.RevRemarks != null)
                    {
                        row.Add("REMARKS", cde.Remarks +"REV-REMARKS: " +cde.RevRemarks);
                    }
                    else

                    {
                        row.Add("REMARKS", cde.Remarks);
                    }
                       

                    rows.Add(row);
                }
            }
            else if (option == "Clarification")
            {

                var datalist1 = _UserreportRepository.FindAll(qury2,sortField).ToList();
                foreach (var cde in datalist1)
                {

                    row = new Dictionary<string, object>();
                    row.Add("ITEMCODE", cde.Itemcode);
                    row.Add("LEGACY", cde.Legacy);
                    if (cde.RevRemarks != null)
                    {
                        row.Add("REMARKS", cde.Remarks + "REV-REMARKS: " + cde.RevRemarks);
                    }
                    else

                    {
                        row.Add("REMARKS", cde.Remarks);
                    }
                    rows.Add(row);
                }

            }
            else if (option == "Unique")
            {
                var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                var filterlist = datalist1.Where(x => x.Materialcode == x.Itemcode).ToList();
                var fList = new List<Prosol_Datamaster>();
                var CriticalList = new List<Prosol_Datamaster>();
                foreach (Prosol_Datamaster pdm in filterlist)
                {
                    if(filterlist.Count(x => x.Shortdesc == pdm.Shortdesc) == 1)
                    {
                        fList.Add(pdm);
                    }                    

                }
                List<Prosol_Datamaster> SortedList = fList.OrderBy(o => o.Noun).ToList();
                foreach (var cde in SortedList)
                {

                    row = new Dictionary<string, object>();
                    row.Add("ITEMCODE", cde.Itemcode);
                    row.Add("MATERIALCODE", cde.Materialcode);
                    row.Add("LEGACY", cde.Legacy);
                    var query = Query.And(Query.EQ("Noun", cde.Noun), Query.EQ("Modifier", cde.Modifier));
                    var x = _NMRepository.FindOne(query);
                    if (x != null)
                    {
                        if (x.Formatted == 0)
                        {
                            row.Add("CATEGORY", "OEM");
                        }
                        else if (x.Formatted == 1)
                        {
                            row.Add("CATEGORY", "GENERIC");
                        }
                        else if (x.Formatted == 2)
                        {
                            row.Add("CATEGORY", "OPM");

                        }
                    }
                    else
                    {
                        row.Add("CATEGORY", "");
                    }
                    row.Add("NOUN", cde.Noun);
                    row.Add("MODIFIER", cde.Modifier);
                    row.Add("SHORT DESCRIPTION", cde.Shortdesc);
                    row.Add("UOM", cde.UOM);
                    row.Add("LONG DESCRIPTION", cde.Longdesc);
                    if (cde.RevRemarks != null)
                    {
                        row.Add("REMARKS", cde.Remarks + "REV-REMARKS: " + cde.RevRemarks);
                    }
                    else

                    {
                        row.Add("REMARKS", cde.Remarks);
                    }
                    rows.Add(row);
                }
            }
            else if (option == "Critical")
            {
                var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                var filterlist = datalist1.Where(x => x.Materialcode == x.Itemcode).ToList();
                var fList = new List<Prosol_Datamaster>();
                var CriticalList = new List<Prosol_Datamaster>();
                foreach (Prosol_Datamaster pdm in filterlist)
                {
                    if (filterlist.Count(x => x.Shortdesc == pdm.Shortdesc) > 1)
                    {
                        fList.Add(pdm);
                    }
                    

                }
                List<Prosol_Datamaster> SortedList = fList.OrderBy(o => o.Noun).ToList();
                foreach (var cde in SortedList)
                {

                    row = new Dictionary<string, object>();
                    row.Add("ITEMCODE", cde.Itemcode);
                    row.Add("MATERIALCODE", cde.Materialcode);
                    row.Add("LEGACY", cde.Legacy);
                    var query = Query.And(Query.EQ("Noun", cde.Noun), Query.EQ("Modifier", cde.Modifier));
                    var x = _NMRepository.FindOne(query);
                    if (x != null)
                    {
                        if (x.Formatted == 0)
                        {
                            row.Add("CATEGORY", "OEM");
                        }
                        else if (x.Formatted == 1)
                        {
                            row.Add("CATEGORY", "GENERIC");
                        }
                        else if (x.Formatted == 2)
                        {
                            row.Add("CATEGORY", "OPM");

                        }
                    }
                    else
                    {
                        row.Add("CATEGORY", "");
                    }
                    row.Add("NOUN", cde.Noun);
                    row.Add("MODIFIER", cde.Modifier);
                    row.Add("SHORT DESCRIPTION", cde.Shortdesc);
                    row.Add("UOM", cde.UOM);
                    row.Add("LONG DESCRIPTION", cde.Longdesc);
                    if (cde.RevRemarks != null)
                    {
                        row.Add("REMARKS", cde.Remarks + "REV-REMARKS: " + cde.RevRemarks);
                    }
                    else

                    {
                        row.Add("REMARKS", cde.Remarks);
                    }
                    rows.Add(row);
                }
            }
            else
            {
                //  string[] flds = { "requestId", "itemId", "source", "plant", "requester", "requestedOn", "approver", "itemStatus", "approvedOn", "rejectedOn" };

                var datalist1 = _UserreportRepository.FindAll(qury2).ToList();
                quryplnt = Query.EQ("Plant", plant);
                var plntlist = _ERPInfoRepository.FindAll(quryplnt).ToList();
                mergelist1 = (from data in datalist1 join plnt in plntlist on data.Itemcode equals plnt.Itemcode orderby data.Itemcode select new { Plant = plnt.Plant, Itemcode = data.Itemcode, Materialcode = data.Materialcode, CreatedOn = data.UpdatedOn, Legacy = data.Legacy, Shortdesc = data.Shortdesc, Longdesc = data.Longdesc, Catalogue = data.Catalogue != null ? data.Catalogue.Name : "", Review = data.Review != null ? data.Review.Name : "", Release = data.Release != null ? data.Release.Name : "", Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, RejectedBy = data.RejectedBy != null ? data.RejectedBy.Name : "", RejectedOn = data.RejectedBy != null ? data.RejectedBy.UpdatedOn : null }).ToList();
                if (mergelist1.Count == 0)
                {
                    goo1 = false;
                }
                else
                {
                    goo1 = true;
                }

                foreach (var cde in mergelist1)
                {

                    row = new Dictionary<string, object>();
                    // row.Add("Item Code", cde.Itemcode);
                    row.Add("Material Code", cde.Materialcode);
                    if (cde.CreatedOn != null)
                    {
                        DateTime date = DateTime.Parse(Convert.ToString(cde.CreatedOn));
                        row.Add("CreatedOn", date.ToString("dd/MM/yyyy"));
                    }
                    else row.Add("CreatedOn", "");
                    row.Add("Legacy", cde.Legacy);
                    row.Add("Shortdesc", cde.Shortdesc);
                    row.Add("Longdesc", cde.Longdesc);
                    row.Add("Cataloguer", cde.Catalogue.Name);
                    row.Add("QC", cde.Review.Name);
                    row.Add("QA", cde.Release.Name);

                    row.Add("Remarks", cde.Remarks);
                    row.Add("RevRemarks", cde.RevRemarks);
                    row.Add("RelRemarks", cde.RelRemarks);
                    row.Add("RejectedBy", cde.RejectedBy);
                    if (cde.RejectedOn != null)
                    {
                        DateTime date11 = DateTime.Parse(Convert.ToString(cde.RejectedOn));
                        row.Add("RejectedOn", date11.ToString("dd/MM/yyyy"));
                    }
                    else row.Add("RejectedOn", "");

                    //row.Add("RejectedOn", cde.RejectedOn);

                    // row.Add("Duplicates", cde.Duplicate);

                    if (goo1 == true)
                    {
                        row.Add("Plant", cde.Plant);
                    }
                    rows.Add(row);
                }



            }

            return rows;
        }

        public IEnumerable<Prosol_ERPInfo> Getmaterialtypedata(string Materialtype)
        {

            var query = Query.EQ("Materialtype", Materialtype);
            var get = _ERPInfoRepository.FindAll(query).ToList();
            return get;

        }
        public IEnumerable<Prosol_Datamaster> getalldata(string plant, string fromdate, string todate)
        {
            var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
            date1 = date1.AddDays(1);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
            var query = Query.And(Query.EQ("ItemStatus", 6), Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
           
            var get = _UserreportRepository.FindAll(query).ToList();
            return get;

        }
        public List<Dictionary<string, object>> loadstatusdata(string[] options, string fromdate, string todate)
        {
            //  var QryLst = new List<IMongoQuery>();
          //  int[] gtoption1 = { };
            List<BsonValue> tets1 = new List<BsonValue>();
          
           
            var requestdata = new List<Prosol_Request>();
            var fields = Fields.Exclude("_id");
            var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
            date1 = date1.AddDays(1);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
            foreach (string x in options)
            {

                if (x == "Request")
                {
                    var query1 = Query.And(Query.EQ("itemStatus", "pending"), Query.GTE("requestedOn", BsonDateTime.Create(date)), Query.LTE("requestedOn", BsonDateTime.Create(date1)));
                    requestdata = _UserrequestRepository.FindAll(query1).ToList();
                }
                else if (x == "Approve")
                {
                    tets1.Add(0);
                   // var query = Query.EQ("ItemStatus", 0);
                   //  QryLst.Add(query);
                }
                else if (x == "Catalogue")
                {
                    //var query = Query.EQ("ItemStatus", 1);
                    //QryLst.Add(query);
                    tets1.Add(1);
                }
                else if (x == "QA")
                {
                    //var query = Query.A nd(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3));
                    // QryLst.Add(query);
                    tets1.Add(2);
                    tets1.Add(3);
                }
                else if (x == "QC")
                {
                    //var query = Query.And(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5));
                    //QryLst.Add(query);
                    tets1.Add(4);
                    tets1.Add(5);
                }
                else if (x == "Released")
                {
                    //var query = Query.EQ("ItemStatus", 6);
                    //QryLst.Add(query);
                    tets1.Add(6);
                }
            }
            
            var users = _UserlistRepository.FindAll().ToList();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            if (requestdata.Count > 0)
            {
                foreach (var code in requestdata)
                {
                    var user = users.Where(x => x.Userid == code.requester).ToList();
                    row = new Dictionary<string, object>();
                    row.Add("Itemcode", code.itemId);
                    row.Add("Material code", "");
                    row.Add("Noun", "");
                    row.Add("Modifier", "");
                    row.Add("UOM", "");
                    row.Add("Legacy", code.source);
                    row.Add("PvData", "");
                    row.Add("Shortdesc", "");
                    row.Add("Longdesc", "");
                    row.Add("Additionalinfo", "");
                    row.Add("ItemStatus", "Request");
                    row.Add("Vendor Details", "");
                    row.Add("Equipment Details", "");
                    row.Add("Requester", user[0].UserName);
                    row.Add("Req-Createdon", code.requestedOn);
                    row.Add("Cataloguer", "");
                    row.Add("Cat-Createdon", "");
                    row.Add("Reviewer", "");
                    row.Add("Rev-Createdon", "");
                    row.Add("Releaser", "");
                    row.Add("Rel-Createdon", "");
                    row.Add("RejectedBy", "");
                    row.Add("RejectedOn", "");

                    row.Add("Cat.Remarks", "");
                    row.Add("Rev.Remarks", "");
                    row.Add("Rel.Remarks", "");
                    row.Add("Plant", "");
                    rows.Add(row);
                }
            }
            bool goo;
            var mergelist = (dynamic)null;


            if (tets1.Count() > 0)
            {
                var query2 = Query.In("ItemStatus", tets1.ToArray());
                var query3 = Query.And(query2, Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));
                var datalist = _UserreportRepository.FindAll(fields, query3).ToList();

                //  mergelist = datalist;
                mergelist = (from data in datalist select new { Itemcode = data.Itemcode, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, UOM = data.UOM, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, RevRemarks = data.RevRemarks, RelRemarks = data.RelRemarks, Additionalinfo = data.Additionalinfo }).ToList();
                // mergelist = (from data in datalist join nm in NM_master on data.Noun equals nm.Noun where data.Modifier == nm.Modifier select new { Itemcode = data.Itemcode, Formatted = nm.Formatted, Materialcode = data.Materialcode, Noun = data.Noun, Modifier = data.Modifier, Legacy = data.Legacy, Longdesc = data.Longdesc, Legacy2 = data.Legacy2, Manufacturer = getmfr(data), Equipment = getequip(data), /*Partno = getpartno(data),*/ Shortdesc = data.Shortdesc, Characteristics = data.Characteristics, Catalogue = data.Catalogue, Review = data.Review, Release = data.Release, ItemStatus = data.ItemStatus, RejectedBy = data.RejectedBy, Remarks = data.Remarks, Soureurl = data.Soureurl }).ToList();
                goo = false;

                int flg = 0;
                foreach (var code in mergelist)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Itemcode", code.Itemcode);
                    row.Add("Material code", code.Materialcode);

                    if (code.Noun != null && code.Noun != "")
                        row.Add("Noun", code.Noun);
                    else row.Add("Noun", "");

                    if (code.Modifier != null && code.Modifier != "")
                        row.Add("Modifier", code.Modifier);
                    else row.Add("Modifier", "");

                    if (code.UOM != null && code.UOM != "")
                        row.Add("UOM", code.UOM);
                    else row.Add("UOM", "");

                    if (code.Legacy != null && code.Legacy != "")
                        row.Add("Legacy", code.Legacy);
                    else row.Add("Legacy", "");

                    if (code.Legacy2 != null && code.Legacy2 != "")
                        row.Add("PvData", code.Legacy2);
                    else row.Add("PvData", "");

                    if (code.Shortdesc != null && code.Shortdesc != "")
                        row.Add("Shortdesc", code.Shortdesc);
                    else row.Add("Shortdesc", "");

                    if (code.Longdesc != null && code.Longdesc != "")
                        row.Add("Longdesc", code.Longdesc);
                    else row.Add("Longdesc", "");

                    if (code.Additionalinfo != null && code.Additionalinfo != "")
                        row.Add("Additionalinfo", code.Additionalinfo);
                    else row.Add("Additionalinfo", "");

                    if (code.ItemStatus == 0 || code.ItemStatus == 1)
                        row.Add("ItemStatus", "Catalogue");
                    else if (code.ItemStatus == 2 || code.ItemStatus == 3)
                        row.Add("ItemStatus", "QC");
                    else if (code.ItemStatus == 4 || code.ItemStatus == 5)
                        row.Add("ItemStatus", "QA");
                    else if (code.ItemStatus == 6)
                        row.Add("ItemStatus", "Released");
                    else if (code.ItemStatus == -1)
                        row.Add("ItemStatus", "Clarification");
                    else row.Add("ItemStatus", "");

                    if (code.Manufacturer != null && code.Manufacturer != "")

                        row.Add("Vendor Details", code.Manufacturer);
                    else row.Add("Vendor Details", "");

                    if (code.Equipment != null && code.Equipment != "")
                        row.Add("Equipment Details", code.Equipment);
                    else row.Add("Equipment Details", "");
                    if (code.Catalogue != null)
                    {
                        row.Add("Cataloguer", code.Catalogue.Name);
                        if (code.Catalogue.UpdatedOn != null)
                        {
                            row.Add("Cat-Createdon", code.Catalogue.UpdatedOn);
                        }
                        else
                            row.Add("Cat-Createdon", "");
                    }
                    else
                    {
                        row.Add("Cataloguer", "");
                        row.Add("Cat-Createdon", "");
                    }



                    if (code.Review != null)
                    {
                        row.Add("Reviewer", code.Review.Name);
                        if (code.Review.UpdatedOn != null)
                        {
                            row.Add("Rev-Createdon", code.Review.UpdatedOn);
                        }
                        else
                            row.Add("Rev-Createdon", "");

                    }
                    else
                    {
                        row.Add("Reviewer", "");
                        row.Add("Rev-Createdon", "");
                    }


                    if (code.Release != null)
                    {
                        row.Add("Releaser", code.Release.Name);
                        if (code.Release.UpdatedOn != null)
                        {
                            row.Add("Rel-Createdon", code.Release.UpdatedOn);
                        }
                        else
                            row.Add("Rel-Createdon", "");

                    }
                    else
                    {
                        row.Add("Releaser", "");
                        row.Add("Rel-Createdon", "");
                    }


                    if (code.RejectedBy != null)
                    {
                        row.Add("RejectedBy", code.RejectedBy.Name);
                        if (code.RejectedBy.UpdatedOn != null)
                        {
                            row.Add("RejectedOn", code.RejectedBy.UpdatedOn);
                        }
                        else
                            row.Add("RejectedOn", "");

                    }
                    else
                    {
                        row.Add("RejectedBy", "");
                        row.Add("RejectedOn", "");
                    }
                    if (code.Remarks != null)
                    {
                        row.Add("Cat.Remarks", code.Remarks);
                    }
                    else
                        row.Add("Cat.Remarks", "");

                    if (code.RevRemarks != null)
                    {
                        row.Add("Rev.Remarks", code.RevRemarks);
                    }
                    else
                        row.Add("Rev.Remarks", "");

                    if (code.RelRemarks != null)
                    {
                        row.Add("Rel.Remarks", code.RelRemarks);
                    }
                    else
                        row.Add("Rel.Remarks", "");

                    if (goo == true)
                    {
                        if (code.Plant != null)
                            row.Add("Plant", code.Plant);
                        else row.Add("Plant", "");
                    }

                    if (code.Characteristics != null)
                    {
                        int i = 1;
                        foreach (var at in code.Characteristics)
                        {
                            row.Add("Attribute" + i, at.Characteristic);
                            row.Add("Value" + i, at.Value);
                            row.Add("UOM" + i, at.UOM);
                            // row.Add("Source" + i, at.Source);
                            //  row.Add("SourceUrl" + i, at.SourceUrl);
                            i++;
                        }
                    }
                    rows.Add(row);
                }
            }
            return rows;
        }
        public List<Prosol_ERPLog> geterplogs(string code)
        {
            var Qry1 = Query.Or(Query.EQ("Itemcode", code), (Query.EQ("Materialcode", code)));
            var Result = _ERPLogRepository.FindAll(Qry1).ToList();
            return Result;
        }


        public List<Dictionary<string, object>> Downloaddata(string username, string status)
        {
            try
            {
                IMongoQuery query;
                var datalist = new List<Prosol_Datamaster>();
                if (status == "cat")
                {
                    query = Query.And(Query.EQ("Catalogue.Name", username), Query.Or(Query.EQ("ItemStatus", 0), Query.EQ("ItemStatus", 1), Query.EQ("ItemStatus", 13)));

                }
                else if(status == "qc")
                {
                    query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus",2), Query.EQ("ItemStatus", 3)));
                    if(username == "Ramkumar" || username == "Aravindm")
                    {
                        query = Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                    }

                }
                else if (status == "All")
                {
                    query = Query.Or(Query.NE("Catalogue", BsonNull.Value), Query.GT("ItemStatus", 0));
                }
                else if (status == "QA")
                {
                    query = Query.And(Query.EQ("Release.Name", username), Query.GTE("ItemStatus", 4), Query.EQ("category", BsonNull.Value));

                }
                else
                {
                    //query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                    query = Query.GTE("ItemStatus", 8);
                }
                datalist = _UserreportRepository.FindAll(query).ToList();


                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;

                foreach (var code in datalist)
                {
                    //    var ven = code.Vendorsuppliers.Where(x => x.s == 1).ToList();
                    //     var Charateristics = code.Characteristics.OrderBy(x => x.Squence).ToList();
                    var Charateristics = code.Characteristics;
                    if (status != "All")
                    {
                        if (!string.IsNullOrEmpty(code.Noun) && !string.IsNullOrEmpty(code.Modifier))
                        {
                            var QR = Query.And(Query.EQ("Noun", code.Noun), Query.EQ("Modifier", code.Modifier), Query.EQ("Definition" , "MM"));
                            var mm = _charRepository.FindAll(QR).ToList();
                            var nm = mm.OrderBy(x => x.Squence).ToList();
                            if (nm.Count > 0)
                            {
                                string NMUnspsc = "";
                                var uns = Query.And(Query.EQ("Noun", code.Noun), (Query.EQ("Modifier", code.Modifier)));
                                var unspc = _unspsclistRepository.FindOne(uns);
                                if (unspc != null)
                                {
                                    if (!String.IsNullOrEmpty(unspc.Commodity))
                                        NMUnspsc = unspc.Commodity;
                                    else NMUnspsc = unspc.Class;
                                }




                                foreach (var nm1 in nm)
                                {
                                    if (Charateristics != null && Charateristics.Count > 0)
                                    {

                                        // foreach (var at in Charateristics)
                                        // {

                                        if (nm1.Noun == code.Noun && nm1.Modifier == code.Modifier && !string.IsNullOrEmpty(nm1.Characteristic))
                                        {
                                            var at = code.Characteristics.Where(x => x.Characteristic == nm1.Characteristic).ToList();
                                            if (at.Count > 0)
                                            {
                                                row = new Dictionary<string, object>();
                                                if(code.Catalogue != null)
                                                {
                                                    if(code.Catalogue.UpdatedOn != null)
                                                        row.Add("Created On", code.Catalogue.UpdatedOn);
                                                    else
                                                        row.Add("Created On", "");
                                                }
                                                else
                                                    row.Add("Created On", "");
                                                row.Add("Itemcode", code.Itemcode);
                                                row.Add("Materialcode", code.Materialcode);
                                                row.Add("S-Legacy", code.Legacy);
                                                row.Add("L-Legacy", code.Legacy2);
                                                //   row.Add("ML-Missingvalue", code.Legacy2);
                                                row.Add("Noun", code.Noun);
                                                row.Add("Modifier", code.Modifier);
                                                row.Add("Labelshort", code.Shortdesc);
                                                row.Add("Shortdesc", code.Shortdesc_);
                                                row.Add("Longdesc", code.Longdesc);
                                                row.Add("Attribute", nm1.Characteristic);
                                                row.Add("Value", at[0].Value);
                                                row.Add("UOM", at[0].UOM);
                                                row.Add("Mandatory", nm1.Mandatory);
                                                row.Add("Sequence", nm1.Squence);
                                                row.Add("Additionalinfo", code.Additionalinfo);
                                                if (!String.IsNullOrEmpty(code.Unspsc))
                                                {
                                                    row.Add("Unspsc", code.Unspsc);
                                                }
                                                else row.Add("Unspsc", NMUnspsc);



                                                row.Add("Soureurl", code.Soureurl);
                                                if (code.Equipment != null)
                                                {
                                                    row.Add("EQ_Name", code.Equipment.Name);
                                                    row.Add("EQ_Name_s", code.Equipment.ENS);
                                                    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                                    row.Add("EQ_MNo", code.Equipment.Modelno);
                                                    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                                    row.Add("EQ_TNo", code.Equipment.Tagno);
                                                    row.Add("EQ_SNo", code.Equipment.Serialno);
                                                    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                                }
                                                else
                                                {
                                                    row.Add("EQ_Name", "");
                                                    row.Add("EQ_Name_s", "");
                                                    row.Add("EQ_Manf", "");
                                                    row.Add("EQ_MNo", "");
                                                    row.Add("EQ_MNo_s", "");
                                                    row.Add("EQ_TNo", "");
                                                    row.Add("EQ_SNo", "");
                                                    row.Add("EQ_Add", "");
                                                }
                                                row.Add("Remarks", code.Remarks);
                                                row.Add("QC-Remarks", code.RevRemarks);
                                                row.Add("MissingValue", code.MissingValue);
                                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                                if (code.Materialcode == "10023896")
                                                {

                                                }
                                                if (code.ItemStatus == -1)
                                                {
                                                    row.Add("ItemStatus", "Clarification");
                                                }
                                                else if (code.ItemStatus == 0)
                                                {
                                                    row.Add("ItemStatus", "Not Saved");
                                                }
                                                else if (code.ItemStatus == 1)
                                                {
                                                    row.Add("ItemStatus", "Saved");
                                                }
                                                else if (code.ItemStatus == 2)
                                                {
                                                    row.Add("ItemStatus", "Catalogue Submit");
                                                }
                                                else if (code.ItemStatus == 3)
                                                {
                                                    row.Add("ItemStatus", "QC Saved");
                                                }
                                                else if (code.ItemStatus == 4)
                                                {
                                                    row.Add("ItemStatus", "QC Submit");
                                                }
                                                else if (code.ItemStatus == 5)
                                                {
                                                    row.Add("ItemStatus", "QA Saved");
                                                }
                                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                                {
                                                    row.Add("ItemStatus", "Released-Delivered");
                                                }
                                                else
                                                {
                                                    row.Add("ItemStatus", "Released");
                                                }
                                                rows.Add(row);
                                                //break;
                                            }
                                            else
                                            {
                                                row = new Dictionary<string, object>();
                                                row.Add("Created On", code.CreatedOn);
                                                row.Add("Itemcode", code.Itemcode);
                                                row.Add("Materialcode", code.Materialcode);
                                                row.Add("S-Legacy", code.Legacy);
                                                row.Add("L-Legacy", code.Legacy2);
                                                row.Add("Noun", code.Noun);
                                                row.Add("Modifier", code.Modifier);
                                                row.Add("Labelshort", code.Shortdesc);
                                                row.Add("Shortdesc", code.Shortdesc_);
                                                row.Add("Longdesc", code.Longdesc);
                                                row.Add("Attribute", nm1.Characteristic);
                                                row.Add("Value", "");
                                                row.Add("UOM", "");
                                                row.Add("Mandatory", nm1.Mandatory);
                                                row.Add("Sequence", nm1.Squence);
                                                row.Add("Additionalinfo", code.Additionalinfo);
                                                if (!String.IsNullOrEmpty(code.Unspsc))
                                                {
                                                    row.Add("Unspsc", code.Unspsc);
                                                }
                                                else row.Add("Unspsc", NMUnspsc);

                                                row.Add("Soureurl", code.Soureurl);
                                                if (code.Equipment != null)
                                                {
                                                    row.Add("EQ_Name", code.Equipment.Name);
                                                    row.Add("EQ_Name_s", code.Equipment.ENS);
                                                    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                                    row.Add("EQ_MNo", code.Equipment.Modelno);
                                                    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                                    row.Add("EQ_TNo", code.Equipment.Tagno);
                                                    row.Add("EQ_SNo", code.Equipment.Serialno);
                                                    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                                }
                                                else
                                                {
                                                    row.Add("EQ_Name", "");
                                                    row.Add("EQ_Name_s", "");
                                                    row.Add("EQ_Manf", "");
                                                    row.Add("EQ_MNo", "");
                                                    row.Add("EQ_MNo_s", "");
                                                    row.Add("EQ_TNo", "");
                                                    row.Add("EQ_SNo", "");
                                                    row.Add("EQ_Add", "");
                                                }
                                                row.Add("Remarks", code.Remarks);
                                                row.Add("QC-Remarks", code.RevRemarks);
                                                row.Add("MissingValue", code.MissingValue);
                                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                                if (code.Materialcode == "10023896")
                                                {

                                                }
                                                if (code.ItemStatus == -1)
                                                {
                                                    row.Add("ItemStatus", "Clarification");
                                                }
                                                else if (code.ItemStatus == 0)
                                                {
                                                    row.Add("ItemStatus", "Not Saved");
                                                }
                                                else if (code.ItemStatus == 1)
                                                {
                                                    row.Add("ItemStatus", "Saved");
                                                }
                                                else if (code.ItemStatus == 2)
                                                {
                                                    row.Add("ItemStatus", "Catalogue Submit");
                                                }
                                                else if (code.ItemStatus == 3)
                                                {
                                                    row.Add("ItemStatus", "QC Saved");
                                                }
                                                else if (code.ItemStatus == 4)
                                                {
                                                    row.Add("ItemStatus", "QC Submit");
                                                }
                                                else if (code.ItemStatus == 5)
                                                {
                                                    row.Add("ItemStatus", "QA Saved");
                                                }
                                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                                {
                                                    row.Add("ItemStatus", "Released-Delivered");
                                                }
                                                else
                                                {
                                                    row.Add("ItemStatus", "Released");
                                                }
                                                rows.Add(row);
                                            }
                                        }

                                        // }
                                    }
                                    else
                                    {

                                        row = new Dictionary<string, object>();
                                        row.Add("Created On", code.CreatedOn);
                                        row.Add("Itemcode", code.Itemcode);
                                        row.Add("Materialcode", code.Materialcode);
                                        row.Add("S-Legacy", code.Legacy);
                                        row.Add("L-Legacy", code.Legacy2);
                                        //  row.Add("ML-Missingvalue", code.Legacy2);
                                        row.Add("Noun", code.Noun);
                                        row.Add("Modifier", code.Modifier);
                                        row.Add("Labelshort", code.Shortdesc);
                                        row.Add("Shortdesc", code.Shortdesc_);
                                        row.Add("Longdesc", code.Longdesc);
                                        row.Add("Attribute", nm1.Characteristic);
                                        row.Add("Value", "");
                                        row.Add("UOM", "");
                                        row.Add("Mandatory", nm1.Mandatory);
                                        row.Add("Sequence", nm1.Squence);
                                        row.Add("Additionalinfo", code.Additionalinfo);

                                        row.Add("Unspsc", NMUnspsc);



                                        row.Add("Soureurl", code.Soureurl);
                                        if (code.Equipment != null)
                                        {
                                            row.Add("EQ_Name", code.Equipment.Name);
                                            row.Add("EQ_Name_s", code.Equipment.ENS);
                                            row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                            row.Add("EQ_MNo", code.Equipment.Modelno);
                                            row.Add("EQ_MNo_s", code.Equipment.ENS);
                                            row.Add("EQ_TNo", code.Equipment.Tagno);
                                            row.Add("EQ_SNo", code.Equipment.Serialno);
                                            row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                        }
                                        //row.Add("Alt_Unspsc", code.Referenceno);
                                        //row.Add("Status", code.Application);
                                        //row.Add("Soureurl", code.Soureurl);
                                        //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                                        //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                                        //row.Add("Batch", code.batch);
                                        row.Add("Remarks", code.Remarks);
                                        row.Add("QC-Remarks", code.RevRemarks);
                                        row.Add("MissingValue", code.MissingValue);
                                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                        row.Add("QC", code.Review != null ? code.Review.Name : "");
                                        if (code.Materialcode == "10023896")
                                        {

                                        }
                                        if (code.ItemStatus == -1)
                                        {
                                            row.Add("ItemStatus", "Clarification");
                                        }
                                        else if (code.ItemStatus == 0)
                                        {
                                            row.Add("ItemStatus", "Not Saved");
                                        }
                                        else if (code.ItemStatus == 1)
                                        {
                                            row.Add("ItemStatus", "Saved");
                                        }
                                        else if (code.ItemStatus == 2)
                                        {
                                            row.Add("ItemStatus", "Catalogue Submit");
                                        }
                                        else if (code.ItemStatus == 3)
                                        {
                                            row.Add("ItemStatus", "QC Saved");
                                        }
                                        else if (code.ItemStatus == 4)
                                        {
                                            row.Add("ItemStatus", "QC Submit");
                                        }
                                        else if (code.ItemStatus == 5)
                                        {
                                            row.Add("ItemStatus", "QA Saved");
                                        }
                                        else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                        {
                                            row.Add("ItemStatus", "Released-Delivered");
                                        }
                                        else
                                        {
                                            row.Add("ItemStatus", "Released");
                                        }
                                        rows.Add(row);
                                    }

                                }

                            }
                            else
                            {
                                row = new Dictionary<string, object>();
                                row.Add("Created On", code.CreatedOn);
                                row.Add("Itemcode", code.Itemcode);
                                row.Add("Materialcode", code.Materialcode);
                                row.Add("S-Legacy", code.Legacy);
                                row.Add("L-Legacy", code.Legacy2);
                                //  row.Add("ML-Missingvalue", code.Legacy2);
                                row.Add("Noun", "");
                                row.Add("Modifier", "");
                                row.Add("Labelshort", "");
                                row.Add("Shortdesc", "");
                                row.Add("Longdesc", "");
                                row.Add("Attribute", "");
                                row.Add("Value", "");
                                row.Add("UOM", "");
                                row.Add("Mandatory", "");
                                row.Add("Sequence", "");
                                row.Add("Additionalinfo", code.Additionalinfo);

                                row.Add("Unspsc", "");

                                row.Add("Soureurl", code.Soureurl);
                                if (code.Equipment != null)
                                {
                                    row.Add("EQ_Name", code.Equipment.Name);
                                    row.Add("EQ_Name_s", code.Equipment.ENS);
                                    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                    row.Add("EQ_MNo", code.Equipment.Modelno);
                                    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                    row.Add("EQ_TNo", code.Equipment.Tagno);
                                    row.Add("EQ_SNo", code.Equipment.Serialno);
                                    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                }
                                //row.Add("Alt_Unspsc", code.Referenceno);
                                //row.Add("Status", code.Application);
                                //row.Add("Soureurl", code.Soureurl);
                                //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                                //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                                //row.Add("Batch", code.batch);
                                row.Add("Remarks", code.Remarks);
                                row.Add("QC-Remarks", code.RevRemarks);
                                row.Add("MissingValue", code.MissingValue);
                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                if (code.Materialcode == "10023896")
                                {

                                }
                                if (code.ItemStatus == -1)
                                {
                                    row.Add("ItemStatus", "Clarification");
                                }
                                else if (code.ItemStatus == 0)
                                {
                                    row.Add("ItemStatus", "Not Saved");
                                }
                                else if (code.ItemStatus == 1)
                                {
                                    row.Add("ItemStatus", "Saved");
                                }
                                else if (code.ItemStatus == 2)
                                {
                                    row.Add("ItemStatus", "Catalogue Submit");
                                }
                                else if (code.ItemStatus == 3)
                                {
                                    row.Add("ItemStatus", "QC Saved");
                                }
                                else if (code.ItemStatus == 4)
                                {
                                    row.Add("ItemStatus", "QC Submit");
                                }
                                else if (code.ItemStatus == 5)
                                {
                                    row.Add("ItemStatus", "QA Saved");
                                }
                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                {
                                    row.Add("ItemStatus", "Released-Delivered");
                                }
                                else
                                {
                                    row.Add("ItemStatus", "Released");
                                }
                                rows.Add(row);
                            }
                        }
                        else
                        {
                            row = new Dictionary<string, object>();
                            row.Add("Created On", code.CreatedOn);
                            row.Add("Itemcode", code.Itemcode);
                            row.Add("Materialcode", code.Materialcode);
                            row.Add("S-Legacy", code.Legacy);
                            row.Add("L-Legacy", code.Legacy2);
                            //  row.Add("ML-Missingvalue", code.Legacy2);
                            row.Add("Noun", "");
                            row.Add("Modifier", "");
                            row.Add("Labelshort", "");
                            row.Add("Shortdesc", "");
                            row.Add("Longdesc", "");
                            row.Add("Attribute", "");
                            row.Add("Value", "");
                            row.Add("UOM", "");
                            row.Add("Mandatory", "");
                            row.Add("Sequence", "");
                            row.Add("Additionalinfo", code.Additionalinfo);

                            row.Add("Unspsc", "");

                            row.Add("Soureurl", code.Soureurl);
                            if (code.Equipment != null)
                            {
                                row.Add("EQ_Name", code.Equipment.Name);
                                row.Add("EQ_Name_s", code.Equipment.ENS);
                                row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                row.Add("EQ_MNo", code.Equipment.Modelno);
                                row.Add("EQ_MNo_s", code.Equipment.ENS);
                                row.Add("EQ_TNo", code.Equipment.Tagno);
                                row.Add("EQ_SNo", code.Equipment.Serialno);
                                row.Add("EQ_Add", code.Equipment.Additionalinfo);
                            }
                            //row.Add("Alt_Unspsc", code.Referenceno);
                            //row.Add("Status", code.Application);
                            //row.Add("Soureurl", code.Soureurl);
                            //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                            //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                            //row.Add("Batch", code.batch);
                            row.Add("Remarks", code.Remarks);
                            row.Add("QC-Remarks", code.RevRemarks);
                            row.Add("MissingValue", code.MissingValue);
                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                            if (code.Materialcode == "10023896")
                            {

                            }
                            if (code.ItemStatus == -1)
                            {
                                row.Add("ItemStatus", "Clarification");
                            }
                            else if (code.ItemStatus == 0)
                            {
                                row.Add("ItemStatus", "Not Saved");
                            }
                            else if (code.ItemStatus == 1)
                            {
                                row.Add("ItemStatus", "Saved");
                            }
                            else if (code.ItemStatus == 2)
                            {
                                row.Add("ItemStatus", "Catalogue Submit");
                            }
                            else if (code.ItemStatus == 3)
                            {
                                row.Add("ItemStatus", "QC Saved");
                            }
                            else if (code.ItemStatus == 4)
                            {
                                row.Add("ItemStatus", "QC Submit");
                            }
                            else if (code.ItemStatus == 5)
                            {
                                row.Add("ItemStatus", "QA Saved");
                            }
                            else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                            {
                                row.Add("ItemStatus", "Released-Delivered");
                            }
                            else
                            {
                                row.Add("ItemStatus", "Released");
                            }
                            rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new Dictionary<string, object>();
                        row.Add("Created On", code.CreatedOn);
                        row.Add("Itemcode", code.Itemcode);
                        row.Add("Materialcode", code.Materialcode);
                        //   row.Add("Legacy", code.Legacy);
                        //  row.Add("ML-Missingvalue", code.Legacy2);
                        row.Add("Noun", string.IsNullOrEmpty(code.Noun) ? "" : code.Noun);
                        row.Add("Modifier", string.IsNullOrEmpty(code.Modifier) ? "" : code.Modifier);
                        //   row.Add("Attribute", "");
                        //   row.Add("Value", "");
                        // row.Add("UOM", "");
                        //  row.Add("Mandatory", "");
                        // row.Add("Sequence", "");
                        //  row.Add("Additionalinfo", code.Additionalinfo);
                        //  row.Add("Unspsc", code.Unspsc);
                        //  row.Add("MissingValue", code.MissingValue);
                        //      row.Add("Status", code.Application);
                        //  row.Add("Soureurl", code.Soureurl);
                        //  row.Add("Batch", code.batch);
                        // row.Add("Remarks", code.Remarks);
                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                        row.Add("QC", code.Review != null ? code.Review.Name : "");
                        if (code.Materialcode == "10023896")
                        {

                        }
                        if (code.ItemStatus == -1)
                        {
                            row.Add("ItemStatus", "Clarification");
                        }
                        else if (code.ItemStatus == 0)
                        {
                            row.Add("ItemStatus", "Not Saved");
                        }
                        else if (code.ItemStatus == 1)
                        {
                            row.Add("ItemStatus", "Saved");
                        }
                        else if (code.ItemStatus == 2)
                        {
                            row.Add("ItemStatus", "Catalogue Submit");
                        }
                        else if (code.ItemStatus == 3)
                        {
                            row.Add("ItemStatus", "QC Saved");
                        }
                        else if (code.ItemStatus == 4)
                        {
                            row.Add("ItemStatus", "QC Submit");
                        }
                        else if (code.ItemStatus == 5)
                        {
                            row.Add("ItemStatus", "QA Saved");
                        }
                        else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                        {
                            row.Add("ItemStatus", "Released-Delivered");
                        }
                        else
                        {
                            row.Add("ItemStatus", "Released");
                        }

                        row.Add("QC-UpdatedOn", code.Review != null ? code.Review.UpdatedOn.ToString() : "");
                        rows.Add(row);

                    }
                }
                return rows;
            }
            catch (Exception e)
            {
                return null;
            }


        }


        public List<Dictionary<string, object>> Downloadvendordata(string username, string status)
        {

            IMongoQuery query;
            var datalist = new List<Prosol_Datamaster>();
            if (status == "cat")
            {
                query = Query.And(Query.EQ("Catalogue.Name", username), Query.Or(Query.EQ("ItemStatus", 0), Query.EQ("ItemStatus", 1)));

            }
            else if(status == "qc")
            {
                query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                if (username == "Ramkumar" || username == "Aravindm")
                {
                    query = Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                }

            }
            else if (status == "All")
            {
                query = Query.GT("ItemStatus", 0);
            }
            else if (status == "QA")
            {
                query = Query.And(Query.EQ("Release.Name", username), Query.GTE("ItemStatus", 4), Query.EQ("category", BsonNull.Value));

            }
            else
            {
                // query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                query = Query.GTE("ItemStatus", 8);
            }
            datalist = _UserreportRepository.FindAll(query).ToList();


            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;

            foreach (var code in datalist)
            {
                int M = 0, s = 0, flg = 0;
                row = new Dictionary<string, object>();
                row.Add("Itemcode", code.Itemcode);
                row.Add("Materialcode", code.Materialcode);

                if (code.Vendorsuppliers != null && code.Vendorsuppliers.Count > 0)
                {
                    foreach (var at in code.Vendorsuppliers)
                    {

                        //if (at.Type == "MANUFACTURER")
                        //{
                        row.Add("LABEL_" + M, at.Type);
                        row.Add("NAME_" + M, at.Name);
                        row.Add("Flag_" + M, at.Refflag);
                        row.Add("No_" + M, at.RefNo);
                        row.Add("S_" + M, at.s);
                        row.Add("L_" + M, at.l);
                        M++;
                        //    rows.Add(row);
                        flg = 1;

                        //   }
                        //else if (at.Type == "SUPPLIER")
                        //{
                        //    row.Add("SUP_NAME_" + s, at.Name);
                        //    row.Add("SUP_Flag_" + s, at.Refflag);
                        //    row.Add("SUP_No_" + s, at.RefNo);
                        //    row.Add("SUP_S_" + s, at.s);
                        //    row.Add("SUP_L_" + s, at.l);
                        //    s++;
                        //}else if(!string.IsNullOrEmpty(at.RefNo))
                        //{
                        //    row.Add("MFR_NAME_" + M, "");
                        //    row.Add("MFR_Flag_" + M, at.Refflag);
                        //    row.Add("MFR_No_" + M, at.RefNo);
                        //    row.Add("MFR_S_" + M, at.s);
                        //    row.Add("MFR_L_" + M, at.l);
                        //    M++;
                        //}


                    }
                    rows.Add(row);


                }
                if (flg == 0)
                {
                    row.Add("LABEL_" + M, "");
                    row.Add("NAME_" + M, "");
                    row.Add("Flag_" + M, "");
                    row.Add("No_" + M, "");
                    row.Add("S_" + M, "");
                    row.Add("L_" + M, "");
                    rows.Add(row);

                }
            }



            return rows;


        }


        public List<Dictionary<string, object>> TrackDownloaddata(string[] code_split)
        {
            try
            {
                var datalist = new List<Prosol_Datamaster>();
                foreach (var code in code_split)
                {
                    var data = new Prosol_Datamaster();
                    var query = Query.Or(Query.EQ("Materialcode", code), Query.EQ("Itemcode", code));
                    data = _UserreportRepository.FindOne(query);
                    datalist.Add(data);
                }

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;

                foreach (var code in datalist)
                {
                    //    var ven = code.Vendorsuppliers.Where(x => x.s == 1).ToList();
                    //     var Charateristics = code.Characteristics.OrderBy(x => x.Squence).ToList();
                    var Charateristics = code.Characteristics;
                    if (!string.IsNullOrEmpty(code.Noun) && !string.IsNullOrEmpty(code.Modifier))
                    {
                        var QR = Query.And(Query.EQ("Noun", code.Noun), Query.EQ("Modifier", code.Modifier), Query.EQ("Definition", "MM"));
                        var mm = _charRepository.FindAll(QR).ToList();
                        var nm = mm.OrderBy(x => x.Squence).ToList();
                        if (nm.Count > 0)
                        {
                            string NMUnspsc = "";
                            var uns = Query.And(Query.EQ("Noun", code.Noun), (Query.EQ("Modifier", code.Modifier)));
                            var unspc = _unspsclistRepository.FindOne(uns);
                            if (unspc != null)
                            {
                                if (!String.IsNullOrEmpty(unspc.Commodity))
                                    NMUnspsc = unspc.Commodity;
                                else NMUnspsc = unspc.Class;
                            }




                            foreach (var nm1 in nm)
                            {
                                if (Charateristics != null && Charateristics.Count > 0)
                                {

                                    // foreach (var at in Charateristics)
                                    // {

                                    if (nm1.Noun == code.Noun && nm1.Modifier == code.Modifier && !string.IsNullOrEmpty(nm1.Characteristic))
                                    {
                                        var at = code.Characteristics.Where(x => x.Characteristic == nm1.Characteristic).ToList();
                                        if (at.Count > 0)
                                        {
                                            row = new Dictionary<string, object>();

                                            row.Add("Created On", code.CreatedOn);
                                            row.Add("Itemcode", code.Itemcode);
                                            row.Add("Materialcode", code.Materialcode);
                                            row.Add("S-Legacy", code.Legacy);
                                            row.Add("L-Legacy", code.Legacy2);
                                            //   row.Add("ML-Missingvalue", code.Legacy2);
                                            row.Add("Noun", code.Noun);
                                            row.Add("Modifier", code.Modifier);
                                            row.Add("Labelshort", code.Shortdesc);
                                            row.Add("Shortdesc", code.Shortdesc_);
                                            row.Add("Longdesc", code.Longdesc);
                                            row.Add("Attribute", nm1.Characteristic);
                                            row.Add("Value", at[0].Value);
                                            row.Add("UOM", at[0].UOM);
                                            row.Add("Mandatory", nm1.Mandatory);
                                            row.Add("Sequence", nm1.Squence);
                                            row.Add("Additionalinfo", code.Additionalinfo);
                                            if (!String.IsNullOrEmpty(code.Unspsc))
                                            {
                                                row.Add("Unspsc", code.Unspsc);
                                            }
                                            else row.Add("Unspsc", NMUnspsc);



                                            row.Add("Status", code.Specification);
                                            row.Add("Soureurl", code.Soureurl);
                                            if (code.Equipment != null)
                                            {
                                                row.Add("EQ_Name", code.Equipment.Name);
                                                row.Add("EQ_Name_s", code.Equipment.ENS);
                                                row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                                row.Add("EQ_MNo", code.Equipment.Modelno);
                                                row.Add("EQ_MNo_s", code.Equipment.ENS);
                                                row.Add("EQ_TNo", code.Equipment.Tagno);
                                                row.Add("EQ_SNo", code.Equipment.Serialno);
                                                row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                            }
                                            else
                                            {
                                                row.Add("EQ_Name", "");
                                                row.Add("EQ_Name_s", "");
                                                row.Add("EQ_Manf", "");
                                                row.Add("EQ_MNo", "");
                                                row.Add("EQ_MNo_s", "");
                                                row.Add("EQ_TNo", "");
                                                row.Add("EQ_SNo", "");
                                                row.Add("EQ_Add", "");
                                            }
                                            row.Add("Remarks", code.Remarks);
                                            row.Add("QC-Remarks", code.RevRemarks);
                                            row.Add("Missing Value", code.MissingValue);
                                            row.Add("Existing Missing Value", code.exMissingValue);
                                            row.Add("Enriched Value", code.EnrichedValue);
                                            row.Add("Repeated Value", RepeatedValue(code.Longdesc));
                                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                                            if (code.ItemStatus == -1)
                                            {
                                                row.Add("ItemStatus", "Clarification");
                                            }
                                            else if (code.ItemStatus == 0)
                                            {
                                                row.Add("ItemStatus", "Not Saved");
                                            }
                                            else if (code.ItemStatus == 1)
                                            {
                                                row.Add("ItemStatus", "Saved");
                                            }
                                            else if (code.ItemStatus == 2)
                                            {
                                                row.Add("ItemStatus", "Catalogue Submit");
                                            }
                                            else if (code.ItemStatus == 3)
                                            {
                                                row.Add("ItemStatus", "QC Saved");
                                            }
                                            else if (code.ItemStatus == 4)
                                            {
                                                row.Add("ItemStatus", "QC Submit");
                                            }
                                            else if (code.ItemStatus == 5)
                                            {
                                                row.Add("ItemStatus", "QA Saved");
                                            }
                                            else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                            {
                                                row.Add("ItemStatus", "Released-Delivered");
                                            }
                                            else
                                            {
                                                row.Add("ItemStatus", "Released");
                                            }
                                            rows.Add(row);
                                            //break;
                                        }
                                        else
                                        {
                                            row = new Dictionary<string, object>();
                                            row.Add("Created On", code.CreatedOn);
                                            row.Add("Itemcode", code.Itemcode);
                                            row.Add("Materialcode", code.Materialcode);
                                            row.Add("S-Legacy", code.Legacy);
                                            row.Add("L-Legacy", code.Legacy2);
                                            row.Add("Noun", code.Noun);
                                            row.Add("Modifier", code.Modifier);
                                            row.Add("Labelshort", code.Shortdesc);
                                            row.Add("Shortdesc", code.Shortdesc_);
                                            row.Add("Longdesc", code.Longdesc);
                                            row.Add("Attribute", nm1.Characteristic);
                                            row.Add("Value", "");
                                            row.Add("UOM", "");
                                            row.Add("Mandatory", nm1.Mandatory);
                                            row.Add("Sequence", nm1.Squence);
                                            row.Add("Additionalinfo", code.Additionalinfo);
                                            if (!String.IsNullOrEmpty(code.Unspsc))
                                            {
                                                row.Add("Unspsc", code.Unspsc);
                                            }
                                            else row.Add("Unspsc", NMUnspsc);

                                            row.Add("Soureurl", code.Soureurl);
                                            if (code.Equipment != null)
                                            {
                                                row.Add("EQ_Name", code.Equipment.Name);
                                                row.Add("EQ_Name_s", code.Equipment.ENS);
                                                row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                                row.Add("EQ_MNo", code.Equipment.Modelno);
                                                row.Add("EQ_MNo_s", code.Equipment.ENS);
                                                row.Add("EQ_TNo", code.Equipment.Tagno);
                                                row.Add("EQ_SNo", code.Equipment.Serialno);
                                                row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                            }
                                            else
                                            {
                                                row.Add("EQ_Name", "");
                                                row.Add("EQ_Name_s", "");
                                                row.Add("EQ_Manf", "");
                                                row.Add("EQ_MNo", "");
                                                row.Add("EQ_MNo_s", "");
                                                row.Add("EQ_TNo", "");
                                                row.Add("EQ_SNo", "");
                                                row.Add("EQ_Add", "");
                                            }
                                            row.Add("Remarks", code.Remarks);
                                            row.Add("QC-Remarks", code.RevRemarks);
                                                row.Add("Missing Value", code.MissingValue);
                                                row.Add("Existing Missing Value", code.exMissingValue);
                                                row.Add("Enriched Value", code.EnrichedValue);
                                            row.Add("Repeated Value", RepeatedValue(code.Longdesc));
                                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                                            if (code.ItemStatus == -1)
                                            {
                                                row.Add("ItemStatus", "Clarification");
                                            }
                                            else if (code.ItemStatus == 0)
                                            {
                                                row.Add("ItemStatus", "Not Saved");
                                            }
                                            else if (code.ItemStatus == 1)
                                            {
                                                row.Add("ItemStatus", "Saved");
                                            }
                                            else if (code.ItemStatus == 2)
                                            {
                                                row.Add("ItemStatus", "Catalogue Submit");
                                            }
                                            else if (code.ItemStatus == 3)
                                            {
                                                row.Add("ItemStatus", "QC Saved");
                                            }
                                            else if (code.ItemStatus == 4)
                                            {
                                                row.Add("ItemStatus", "QC Submit");
                                            }
                                            else if (code.ItemStatus == 5)
                                            {
                                                row.Add("ItemStatus", "QA Saved");
                                            }
                                            else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                            {
                                                row.Add("ItemStatus", "Released-Delivered");
                                            }
                                            else
                                            {
                                                row.Add("ItemStatus", "Released");
                                            }
                                            rows.Add(row);
                                        }
                                    }

                                    // }
                                }
                                else
                                {

                                    row = new Dictionary<string, object>();
                                    row.Add("Created On", code.CreatedOn);
                                    row.Add("Itemcode", code.Itemcode);
                                    row.Add("Materialcode", code.Materialcode);
                                    row.Add("S-Legacy", code.Legacy);
                                    row.Add("L-Legacy", code.Legacy2);
                                    //  row.Add("ML-Missingvalue", code.Legacy2);
                                    row.Add("Noun", code.Noun);
                                    row.Add("Modifier", code.Modifier);
                                    row.Add("Labelshort", code.Shortdesc);
                                    row.Add("Shortdesc", code.Shortdesc_);
                                    row.Add("Longdesc", code.Longdesc);
                                    row.Add("Attribute", nm1.Characteristic);
                                    row.Add("Value", "");
                                    row.Add("UOM", "");
                                    row.Add("Mandatory", nm1.Mandatory);
                                    row.Add("Sequence", nm1.Squence);
                                    row.Add("Additionalinfo", code.Additionalinfo);

                                    row.Add("Unspsc", NMUnspsc);


                                    row.Add("Status", code.Specification);

                                    row.Add("Soureurl", code.Soureurl);
                                    if (code.Equipment != null)
                                    {
                                        row.Add("EQ_Name", code.Equipment.Name);
                                        row.Add("EQ_Name_s", code.Equipment.ENS);
                                        row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                        row.Add("EQ_MNo", code.Equipment.Modelno);
                                        row.Add("EQ_MNo_s", code.Equipment.ENS);
                                        row.Add("EQ_TNo", code.Equipment.Tagno);
                                        row.Add("EQ_SNo", code.Equipment.Serialno);
                                        row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                    }
                                    //row.Add("Alt_Unspsc", code.Referenceno);
                                    //row.Add("Status", code.Application);
                                    //row.Add("Soureurl", code.Soureurl);
                                    //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                                    //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                                    //row.Add("Batch", code.batch);
                                    row.Add("Remarks", code.Remarks);
                                    row.Add("QC-Remarks", code.RevRemarks);
                                        row.Add("Missing Value", code.MissingValue);
                                        row.Add("Existing Missing Value", code.exMissingValue);
                                        row.Add("Enriched Value", code.EnrichedValue);
                                    row.Add("Repeated Value", RepeatedValue(code.Longdesc));
                                    row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                    row.Add("QC", code.Review != null ? code.Review.Name : "");
                                    if (code.ItemStatus == -1)
                                    {
                                        row.Add("ItemStatus", "Clarification");
                                    }
                                    else if (code.ItemStatus == 0)
                                    {
                                        row.Add("ItemStatus", "Not Saved");
                                    }
                                    else if (code.ItemStatus == 1)
                                    {
                                        row.Add("ItemStatus", "Saved");
                                    }
                                    else if (code.ItemStatus == 2)
                                    {
                                        row.Add("ItemStatus", "Catalogue Submit");
                                    }
                                    else if (code.ItemStatus == 3)
                                    {
                                        row.Add("ItemStatus", "QC Saved");
                                    }
                                    else if (code.ItemStatus == 4)
                                    {
                                        row.Add("ItemStatus", "QC Submit");
                                    }
                                    else if (code.ItemStatus == 5)
                                    {
                                        row.Add("ItemStatus", "QA Saved");
                                    }
                                    else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                    {
                                        row.Add("ItemStatus", "Released-Delivered");
                                    }
                                    else
                                    {
                                        row.Add("ItemStatus", "Released");
                                    }
                                    rows.Add(row);
                                }

                            }

                        }
                        else
                        {
                            row = new Dictionary<string, object>();
                            row.Add("Created On", code.CreatedOn);
                            row.Add("Itemcode", code.Itemcode);
                            row.Add("Materialcode", code.Materialcode);
                            row.Add("S-Legacy", code.Legacy);
                            row.Add("L-Legacy", code.Legacy2);
                            //  row.Add("ML-Missingvalue", code.Legacy2);
                            row.Add("Noun", "");
                            row.Add("Modifier", "");
                            row.Add("Labelshort", "");
                            row.Add("Shortdesc", "");
                            row.Add("Longdesc", "");
                            row.Add("Attribute", "");
                            row.Add("Value", "");
                            row.Add("UOM", "");
                            row.Add("Mandatory", "");
                            row.Add("Sequence", "");
                            row.Add("Additionalinfo", code.Additionalinfo);

                            row.Add("Unspsc", "");

                            row.Add("Status", code.Specification);
                            row.Add("Soureurl", code.Soureurl);
                            if (code.Equipment != null)
                            {
                                row.Add("EQ_Name", code.Equipment.Name);
                                row.Add("EQ_Name_s", code.Equipment.ENS);
                                row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                row.Add("EQ_MNo", code.Equipment.Modelno);
                                row.Add("EQ_MNo_s", code.Equipment.ENS);
                                row.Add("EQ_TNo", code.Equipment.Tagno);
                                row.Add("EQ_SNo", code.Equipment.Serialno);
                                row.Add("EQ_Add", code.Equipment.Additionalinfo);
                            }
                            //row.Add("Alt_Unspsc", code.Referenceno);
                            //row.Add("Status", code.Application);
                            //row.Add("Soureurl", code.Soureurl);
                            //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                            //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                            //row.Add("Batch", code.batch);
                            row.Add("Remarks", code.Remarks);
                            row.Add("QC-Remarks", code.RevRemarks);
                                row.Add("Missing Value", code.MissingValue);
                                row.Add("Existing Missing Value", code.exMissingValue);
                                row.Add("Enriched Value", code.EnrichedValue);
                            row.Add("Repeated Value", RepeatedValue(code.Longdesc));
                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                            if (code.ItemStatus == -1)
                            {
                                row.Add("ItemStatus", "Clarification");
                            }
                            else if (code.ItemStatus == 0)
                            {
                                row.Add("ItemStatus", "Not Saved");
                            }
                            else if (code.ItemStatus == 1)
                            {
                                row.Add("ItemStatus", "Saved");
                            }
                            else if (code.ItemStatus == 2)
                            {
                                row.Add("ItemStatus", "Catalogue Submit");
                            }
                            else if (code.ItemStatus == 3)
                            {
                                row.Add("ItemStatus", "QC Saved");
                            }
                            else if (code.ItemStatus == 4)
                            {
                                row.Add("ItemStatus", "QC Submit");
                            }
                            else if (code.ItemStatus == 5)
                            {
                                row.Add("ItemStatus", "QA Saved");
                            }
                            else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                            {
                                row.Add("ItemStatus", "Released-Delivered");
                            }
                            else
                            {
                                row.Add("ItemStatus", "Released");
                            }
                            rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new Dictionary<string, object>();
                        row.Add("Created On", code.CreatedOn);
                        row.Add("Itemcode", code.Itemcode);
                        row.Add("Materialcode", code.Materialcode);
                        row.Add("S-Legacy", code.Legacy);
                        row.Add("L-Legacy", code.Legacy2);
                        //  row.Add("ML-Missingvalue", code.Legacy2);
                        row.Add("Noun", "");
                        row.Add("Modifier", "");
                        row.Add("Labelshort", "");
                        row.Add("Shortdesc", "");
                        row.Add("Longdesc", "");
                        row.Add("Attribute", "");
                        row.Add("Value", "");
                        row.Add("UOM", "");
                        row.Add("Mandatory", "");
                        row.Add("Sequence", "");
                        row.Add("Additionalinfo", code.Additionalinfo);

                        row.Add("Unspsc", "");

                        row.Add("Status", code.Specification);
                        row.Add("Soureurl", code.Soureurl);
                        if (code.Equipment != null)
                        {
                            row.Add("EQ_Name", code.Equipment.Name);
                            row.Add("EQ_Name_s", code.Equipment.ENS);
                            row.Add("EQ_Manf", code.Equipment.Manufacturer);
                            row.Add("EQ_MNo", code.Equipment.Modelno);
                            row.Add("EQ_MNo_s", code.Equipment.ENS);
                            row.Add("EQ_TNo", code.Equipment.Tagno);
                            row.Add("EQ_SNo", code.Equipment.Serialno);
                            row.Add("EQ_Add", code.Equipment.Additionalinfo);
                        }
                        //row.Add("Alt_Unspsc", code.Referenceno);
                        //row.Add("Status", code.Application);
                        //row.Add("Soureurl", code.Soureurl);
                        //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                        //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                        //row.Add("Batch", code.batch);
                        row.Add("Remarks", code.Remarks);
                        row.Add("QC-Remarks", code.RevRemarks);
                        row.Add("MissingValue", code.MissingValue);
                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                        row.Add("QC", code.Review != null ? code.Review.Name : "");
                        row.Add("ItemStatus", code.ItemStatus == 0 ?
                                              (code.ItemStatus == 1 ?
                                              (code.ItemStatus == 2 ?
                                              (code.ItemStatus == 5 ?
                                              (code.ItemStatus == 3 ?
                                              (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category)) ?
                                              "Released-Delivered"
                                              : "QC Submit"
                                              : "QC Saved")
                                              : "QA Saved")
                                              : "Catalogue Submit")
                                              : "Saved")
                                              : "Not Saved");
                        rows.Add(row);
                    }
                }
                return rows;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public List<Dictionary<string, object>> TrackDownloadvendordata(string[] code_split)
        {

            var datalist = new List<Prosol_Datamaster>();
            foreach (var code in code_split)
            {
                var data = new Prosol_Datamaster();
                var query = Query.Or(Query.EQ("Materialcode", code), Query.EQ("Itemcode", code));
                data = _UserreportRepository.FindOne(query);
                datalist.Add(data);
            }


            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;

            foreach (var code in datalist)
            {
                int M = 0, s = 0, flg = 0;
                row = new Dictionary<string, object>();
                row.Add("Itemcode", code.Itemcode);
                row.Add("Materialcode", code.Materialcode);

                if (code.Vendorsuppliers != null && code.Vendorsuppliers.Count > 0)
                {
                    foreach (var at in code.Vendorsuppliers)
                    {

                        //if (at.Type == "MANUFACTURER")
                        //{
                        row.Add("LABEL_" + M, at.Type);
                        row.Add("NAME_" + M, at.Name);
                        row.Add("Flag_" + M, at.Refflag);
                        row.Add("No_" + M, at.RefNo);
                        row.Add("S_" + M, at.s);
                        row.Add("L_" + M, at.l);
                        M++;
                        //    rows.Add(row);
                        flg = 1;

                        //   }
                        //else if (at.Type == "SUPPLIER")
                        //{
                        //    row.Add("SUP_NAME_" + s, at.Name);
                        //    row.Add("SUP_Flag_" + s, at.Refflag);
                        //    row.Add("SUP_No_" + s, at.RefNo);
                        //    row.Add("SUP_S_" + s, at.s);
                        //    row.Add("SUP_L_" + s, at.l);
                        //    s++;
                        //}else if(!string.IsNullOrEmpty(at.RefNo))
                        //{
                        //    row.Add("MFR_NAME_" + M, "");
                        //    row.Add("MFR_Flag_" + M, at.Refflag);
                        //    row.Add("MFR_No_" + M, at.RefNo);
                        //    row.Add("MFR_S_" + M, at.s);
                        //    row.Add("MFR_L_" + M, at.l);
                        //    M++;
                        //}


                    }
                    rows.Add(row);


                }
                if (flg == 0)
                {
                    row.Add("LABEL_" + M, "");
                    row.Add("NAME_" + M, "");
                    row.Add("Flag_" + M, "");
                    row.Add("No_" + M, "");
                    row.Add("S_" + M, "");
                    row.Add("L_" + M, "");
                    rows.Add(row);

                }
            }



            return rows;


        }
        private string RepeatedValue(string cat)
        {
            string constr = "";
            string tmpStr = "";

            string[] tmpArr = (cat?.Split(',')) ?? new string[0];

            for (int i = 0; i < tmpArr.Length; i++)
            {
                if (tmpArr[i].Contains(':'))
                {
                    string[] strSplit = tmpArr[i].Split(':');
                    tmpArr[i] = strSplit.Length > 1 ? strSplit[1].Trim() : tmpArr[i];
                }

                tmpStr += tmpArr[i].Trim() + (i < tmpArr.Length - 1 ? "," : "");
            }

            string[] words = tmpStr.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            HashSet<string> seenWords = new HashSet<string>();
            HashSet<string> repeatedWords = new HashSet<string>();

            foreach (string word in words)
            {
                if (seenWords.Contains(word) && !repeatedWords.Contains(word) && word != "--")
                {
                    repeatedWords.Add(word);
                }
                seenWords.Add(word);
            }

            constr = string.Join(",", repeatedWords);

            return constr;
        }
    }
}
