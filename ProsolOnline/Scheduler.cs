using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Configuration;
using Prosol.Core.Interface;
using Prosol.Core;
using MongoDB.Driver;
using Prosol.Core.Model;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace TestWeb
{
    public class Scheduler : IHttpModule
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["prosolconnect"].ConnectionString;

        private static MongoServer _server = new MongoClient(_connectionString).GetServer();

        // private string _collectionName;

        private static MongoDatabase _db;

        protected static Timer timer;
        protected static object someAction;
        static Scheduler()
        {
            _db = _server.GetDatabase(MongoUrl.Create(_connectionString).DatabaseName);
            someAction = new object();
        }
        void IHttpModule.Init(HttpApplication application)
        {
            if (timer == null)
            {
                var timerCallback = new TimerCallback(ProcessSomething);
                // var startTime = 14400000;
                var startTime = 12;
                long timerInterval = Convert.ToInt64(ConfigurationSettings.AppSettings["SchedulerInterval"]); // 5 Minutes
                timer = new Timer(timerCallback, null, startTime, timerInterval);
            }
        }

        protected void ProcessSomething(object state)
        {
            lock (someAction)
            {
                try
                {
                    var day = DateTime.Today.DayOfWeek;

                    // if (day.ToString() == "Tuesday" && DateTime.Now.Hour >= 13 && DateTime.Now.Hour <= 14)
                    //  BatchInfo();
                    //if ((DateTime.Now.Hour >= 13 && DateTime.Now.Hour <= 14) || (DateTime.Now.Hour >= 22 && DateTime.Now.Hour <= 23))
                    //{
                    //    catalogOneTimeLog();
                    //    QCOneTimeLog();
                    //}
                    //else
                    //{
                    //  catalogLog();
                       
                   // }
                }
                catch (Exception e)
                {

                }
            }


        }
        protected void catalogLog()
        {
            DateTime Cur_dte = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);           
            var DashboardData = _db.GetCollection<prosol_FARdashboard>("prosol_FARdashboard");
            var AssetData = _db.GetCollection<Prosol_AssetMaster>("Prosol_AssetMaster");
            var Business = _db.GetCollection<Prosol_Business>("Prosol_Business");
            var Major = _db.GetCollection<Prosol_MajorClass>("Prosol_MajorClass");
            var Minor = _db.GetCollection<Prosol_MinorClass>("Prosol_MinorClass");
            var Region = _db.GetCollection<Prosol_Region>("Prosol_Region");
            var City = _db.GetCollection<Prosol_City>("Prosol_City");


            var dashboardItems = DashboardData.FindAll().ToList();
            foreach (var mdl in dashboardItems)
            {
               
                var QryLst = new List<IMongoQuery>();
                var orQryLst = new List<IMongoQuery>();
               
                QryLst.Add(Query.GTE("ItemStatus",2));
             
                var sort = SortBy.Descending("CreatedOn");
                string business_id = "", major_id = "", minor_id = "", region_id = "", city_id = "", area_id = "", subarea_id = "";
                //var day = DateTime.Today.DayOfWeek;
                if (mdl.Business == "Electricity Directorate")
                {
                   

                    foreach (var col in mdl.HierarchyCols)
                    {
                       
                        if (col.colName == "Business")
                        {
                            var SAQry = Query.EQ("BusinessName", col.Value);
                            var busin = Business.FindOne(SAQry);
                            business_id = busin._id.ToString();                          
                            QryLst.Add(Query.EQ(col.colName, business_id));
                        }

                        if (col.colName == "MajorClass")
                        {
                            var SAQry = Query.And(Query.EQ("Business_id", business_id), Query.EQ(col.colName, col.Value));
                            var major = Major.FindOne(SAQry);
                            major_id = major._id.ToString();
                            SAQry = Query.EQ(col.colName, major_id);
                            orQryLst.Add(SAQry);
                        }
                        if (col.colName == "MinorClass")
                        {
                            var SAQry  = Query.And(Query.EQ("Major_id", major_id), Query.EQ(col.colName, col.Value));
                            var minor = Minor.FindOne(SAQry);
                            minor_id = minor._id.ToString();
                            QryLst.Add(Query.EQ(col.colName, minor_id));
                           // QryLst.Add(SAQry);
                        }
                        if (col.colName == "Region")
                        {
                            var SAQry  = Query.EQ(col.colName, col.Value);
                            var region = Region.FindOne(SAQry);
                            if (region != null)
                            {

                                region_id = region._id.ToString();
                            }
                            QryLst.Add(Query.EQ(col.colName, region_id));
                            //QryLst.Add(SAQry);
                        }
                        if (col.colName == "City")
                        {

                            var SAQry  = Query.And(Query.EQ("Region_Id", region_id), Query.EQ(col.colName, col.Value));
                            var city = City.FindOne(SAQry);
                            if (city != null)
                            {

                                city_id = city._id.ToString();
                            }
                            SAQry = Query.EQ(col.colName, city_id);
                            orQryLst.Add(SAQry);
                        }

                    }

                    var andQry = Query.And(QryLst);
                    var or1Qry = Query.Or(orQryLst);
                    var Qry = Query.And(andQry, or1Qry);


                    var resData = AssetData.Find(Qry).SetSortOrder(sort).ToList();

                    mdl.ActualComplete = resData.Count.ToString();
                    if (!string.IsNullOrEmpty(mdl.Estimated) && Convert.ToInt32(mdl.Estimated) > 0)
                    {
                        var s = ((Convert.ToDecimal(mdl.ActualComplete) / Convert.ToDecimal(mdl.Estimated)) * 100);
                        mdl.ActualPerc = Convert.ToDecimal(s).ToString("F");

                        var p = ((Convert.ToDecimal(mdl.plannedComplete) / Convert.ToDecimal(mdl.Estimated)) * 100);
                        mdl.PlannedPerc = Convert.ToDecimal(p).ToString("F");

                    }
                    mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var histList = new List<Logs>();
                    if (mdl.History != null && mdl.History.Count > 0)
                    {
                        int flg = 0;
                        foreach (var sob in mdl.History)
                        {
                            if (sob.CreatedOn == Cur_dte)
                            {

                                sob.Estimated = mdl.Estimated;
                                sob.plannedComplete = mdl.plannedComplete;
                                sob.ActualComplete = mdl.ActualComplete;
                                sob.PlannedPerc = mdl.PlannedPerc;
                                sob.ActualPerc = mdl.ActualPerc;
                                sob.CreatedOn = Cur_dte;
                               // mdl.History.Add(sob);
                                flg = 1;
                               
                            }
                        }
                        if (flg == 0)
                        {
                            var his = new Logs();
                            his.Estimated = mdl.Estimated;
                            his.plannedComplete = mdl.plannedComplete;
                            his.ActualComplete = mdl.ActualComplete;
                            his.PlannedPerc = mdl.PlannedPerc;
                            his.ActualPerc = mdl.ActualPerc;
                            his.CreatedOn = Cur_dte;
                            mdl.History.Add(his);
                        }

                    }
                    else
                    {
                       
                        var his = new Logs();
                        his.Estimated = mdl.Estimated;
                        his.plannedComplete = mdl.plannedComplete;
                        his.ActualComplete = mdl.ActualComplete;
                        his.PlannedPerc = mdl.PlannedPerc;
                        his.ActualPerc = mdl.ActualPerc;
                        his.CreatedOn = Cur_dte;
                        histList.Add(his);
                        mdl.History = histList;

                    }
                  
                    DashboardData.Save(mdl);

                }
                    if (mdl.Business == "Water Directorate")
                {
                    foreach (var col in mdl.HierarchyCols)
                    {
                        var SAQry = Query.EQ(col.colName, col.Value);
                        if (col.colName == "Business")
                        {
                            SAQry = Query.EQ("BusinessName", col.Value);
                            var busin = Business.FindOne(SAQry);
                            business_id = busin._id.ToString();
                            SAQry = Query.EQ(col.colName, business_id);
                            QryLst.Add(SAQry);
                        }

                        if (col.colName == "MajorClass")
                        {
                            SAQry = Query.And(Query.EQ("Business_id", business_id), Query.EQ(col.colName, col.Value));
                            var major = Major.FindOne(SAQry);
                            major_id = major._id.ToString();
                            SAQry = Query.EQ(col.colName, major_id);
                            if (mdl.Category == "Linear Assets")
                            {
                                orQryLst.Add(SAQry);
                            }
                        }
                       
                        if (col.colName == "MinorClass" && mdl.Category== "Linear Assets")
                        {
                            SAQry = Query.And(Query.EQ("Major_id", major_id), Query.Matches(col.colName, BsonRegularExpression.Create(new Regex(string.Format("^{0}", col.Value.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));
                            var minor = Minor.Find(SAQry).ToList();
                            foreach (var smd in minor)
                            {
                                minor_id = smd._id.ToString();
                                SAQry = Query.NE(col.colName, minor_id);
                                QryLst.Add(SAQry);
                            }
                        }
                        if (col.colName == "MinorClass" && mdl.Category.StartsWith("Non-Linear Assets"))
                        {
                            SAQry = Query.And(Query.EQ("Major_id", major_id), Query.Matches(col.colName, BsonRegularExpression.Create(new Regex(string.Format("^{0}", col.Value.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));
                            var minor = Minor.Find(SAQry).ToList();
                            foreach (var smd in minor)
                            {
                              
                                minor_id = smd._id.ToString();
                                SAQry = Query.EQ(col.colName, minor_id);
                                orQryLst.Add(SAQry);
                            }
                        }
                        if (col.colName == "Region")
                        {
                            SAQry = Query.EQ(col.colName, col.Value);
                            var region = Region.FindOne(SAQry);
                            if (region != null)
                            {

                                region_id = region._id.ToString();
                            }
                            SAQry = Query.EQ(col.colName, region_id);
                            QryLst.Add(SAQry);
                        }
                        if (col.colName == "City")
                        {

                            SAQry = Query.And(Query.EQ("Region_Id", region_id), Query.EQ(col.colName, col.Value));
                            var city = City.FindOne(SAQry);
                            if (city != null)
                            {

                                city_id = city._id.ToString();
                            }
                            SAQry = Query.EQ(col.colName, city_id);
                            QryLst.Add(SAQry);
                        }

                    }
                    var Qry111 = Query.And(QryLst);
                    if (orQryLst.Count > 0)
                    {
                        var or1Qry = Query.Or(orQryLst);
                       Qry111 = Query.And(Qry111, or1Qry);
                    }
                  
                  

                    var resData = AssetData.Find(Qry111).SetSortOrder(sort).ToList();

                    mdl.ActualComplete = resData.Count.ToString();
                    if (!string.IsNullOrEmpty(mdl.Estimated) && Convert.ToInt32(mdl.Estimated) > 0)
                    {
                        var s = ((Convert.ToDecimal(mdl.ActualComplete) / Convert.ToDecimal(mdl.Estimated)) * 100);
                        mdl.ActualPerc = Convert.ToDecimal(s).ToString("F");

                        var p = ((Convert.ToDecimal(mdl.plannedComplete) / Convert.ToDecimal(mdl.Estimated)) * 100);
                        mdl.PlannedPerc = Convert.ToDecimal(p).ToString("F");

                    }
                    mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var histList = new List<Logs>();
                    if (mdl.History != null && mdl.History.Count > 0)
                    {
                        int flg = 0;
                        foreach (var sob in mdl.History)
                        {
                            if (sob.CreatedOn == Cur_dte)
                            {

                                sob.Estimated = mdl.Estimated;
                                sob.plannedComplete = mdl.plannedComplete;
                                sob.ActualComplete = mdl.ActualComplete;
                                sob.PlannedPerc = mdl.PlannedPerc;
                                sob.ActualPerc = mdl.ActualPerc;
                                sob.CreatedOn = Cur_dte;
                                // mdl.History.Add(sob);
                                flg = 1;

                            }
                        }
                        if (flg == 0)
                        {
                            var his = new Logs();
                            his.Estimated = mdl.Estimated;
                            his.plannedComplete = mdl.plannedComplete;
                            his.ActualComplete = mdl.ActualComplete;
                            his.PlannedPerc = mdl.PlannedPerc;
                            his.ActualPerc = mdl.ActualPerc;
                            his.CreatedOn = Cur_dte;
                            mdl.History.Add(his);
                        }

                    }
                    else
                    {

                        var his = new Logs();
                        his.Estimated = mdl.Estimated;
                        his.plannedComplete = mdl.plannedComplete;
                        his.ActualComplete = mdl.ActualComplete;
                        his.PlannedPerc = mdl.PlannedPerc;
                        his.ActualPerc = mdl.ActualPerc;
                        his.CreatedOn = Cur_dte;
                        histList.Add(his);
                        mdl.History = histList;

                    }

                    DashboardData.Save(mdl);

                }
                if (mdl.Business == "Generation and Production Directorate")
                {
                    foreach (var col in mdl.HierarchyCols)
                    {
                        var SAQry = Query.EQ(col.colName, col.Value);
                        if (col.colName == "Business")
                        {
                            SAQry = Query.EQ("BusinessName", col.Value);
                            var busin = Business.FindOne(SAQry);
                            business_id = busin._id.ToString();
                            SAQry = Query.EQ(col.colName, business_id);
                            QryLst.Add(SAQry);
                        }

                        if (col.colName == "MajorClass")
                        {
                            SAQry = Query.And(Query.EQ("Business_id", business_id), Query.EQ(col.colName, col.Value));
                            var major = Major.FindOne(SAQry);
                            major_id = major._id.ToString();
                            SAQry = Query.EQ(col.colName, major_id);

                            QryLst.Add(SAQry);

                        }                     
                        if (col.colName == "Area")
                        {
                            var Area = _db.GetCollection<Prosol_Area>("Prosol_Area");
                            SAQry = Query.EQ(col.colName, col.Value);
                            var area = Area.FindOne(SAQry);
                            if (area != null)
                            {

                                area_id = area._id.ToString();
                            }
                            SAQry = Query.EQ(col.colName, area_id);
                            QryLst.Add(SAQry);
                        }
                        if (col.colName == "SubArea")
                        {
                            var SubArea = _db.GetCollection<Prosol_SubArea>("Prosol_SubArea");
                            SAQry = Query.And(Query.EQ("Area_Id", area_id), Query.EQ(col.colName, col.Value));
                            var subarea = SubArea.FindOne(SAQry);
                            if (subarea != null)
                            {

                                subarea_id = subarea._id.ToString();
                            }
                            SAQry = Query.EQ(col.colName, subarea_id);
                            orQryLst.Add(SAQry);
                        }

                    }
                    var Qry111 = Query.And(QryLst);
                   
                        var or1Qry = Query.Or(orQryLst);
                        Qry111 = Query.And(Qry111, or1Qry);
                   



                    var resData = AssetData.Find(Qry111).SetSortOrder(sort).ToList();

                    mdl.ActualComplete = resData.Count.ToString();
                    if (!string.IsNullOrEmpty(mdl.Estimated) && Convert.ToInt32(mdl.Estimated) > 0)
                    {
                        var s = ((Convert.ToDecimal(mdl.ActualComplete) / Convert.ToDecimal(mdl.Estimated)) * 100);
                        mdl.ActualPerc = Convert.ToDecimal(s).ToString("F");

                        var p = ((Convert.ToDecimal(mdl.plannedComplete) / Convert.ToDecimal(mdl.Estimated)) * 100);
                        mdl.PlannedPerc = Convert.ToDecimal(p).ToString("F");

                    }
                    mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var histList = new List<Logs>();
                    if (mdl.History != null && mdl.History.Count > 0)
                    {
                        int flg = 0;
                        foreach (var sob in mdl.History)
                        {
                            if (sob.CreatedOn == Cur_dte)
                            {

                                sob.Estimated = mdl.Estimated;
                                sob.plannedComplete = mdl.plannedComplete;
                                sob.ActualComplete = mdl.ActualComplete;
                                sob.PlannedPerc = mdl.PlannedPerc;
                                sob.ActualPerc = mdl.ActualPerc;
                                sob.CreatedOn = Cur_dte;
                                // mdl.History.Add(sob);
                                flg = 1;

                            }
                        }
                        if (flg == 0)
                        {
                            var his = new Logs();
                            his.Estimated = mdl.Estimated;
                            his.plannedComplete = mdl.plannedComplete;
                            his.ActualComplete = mdl.ActualComplete;
                            his.PlannedPerc = mdl.PlannedPerc;
                            his.ActualPerc = mdl.ActualPerc;
                            his.CreatedOn = Cur_dte;
                            mdl.History.Add(his);
                        }

                    }
                    else
                    {

                        var his = new Logs();
                        his.Estimated = mdl.Estimated;
                        his.plannedComplete = mdl.plannedComplete;
                        his.ActualComplete = mdl.ActualComplete;
                        his.PlannedPerc = mdl.PlannedPerc;
                        his.ActualPerc = mdl.ActualPerc;
                        his.CreatedOn = Cur_dte;
                        histList.Add(his);
                        mdl.History = histList;

                    }

                    DashboardData.Save(mdl);
                }
                if (mdl.Business == "Shared Services Directorate")
                {

                }
                if (mdl.Business == "Capital Spare Parts")
                {

                }
              
              
            }
        }
      
        public void Dispose() { }

    }
    public class UserList
    {
        public string Name { set; get; }
        public int Completed { set; get; }
        public int TotalTarget { set; get; }
        public string Category { set; get; }
        public string Role { set; get; }
        public string Email { set; get; }

    }
}