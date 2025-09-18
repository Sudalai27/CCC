using Excel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core
{
    public partial class ServiceReportService : IServiceReport
    {
        private readonly IRepository<Prosol_Users> _ServiceReportService;
        private readonly IRepository<Prosol_RequestService> _ServiceRequestService;
        private readonly IRepository<Prosol_ServiceCategory> _ServiceCategory;
        private readonly IRepository<Prosol_ServiceGroup> _ServiceGroup;
        private readonly IRepository<Prosol_SMainCode> _ServiceMainCodeRep;
        private readonly IRepository<Prosol_SSubCode> _ServiceSubCodeRep;
        private readonly IRepository<Prosol_SSubSubCode> _ServiceSubSubCodeRep;
        private readonly IRepository<Prosol_MSAttribute> _MSAttributeRep;
        private readonly IRepository<Prosol_Abbrevate> _abbreviateRepository;

        public ServiceReportService(IRepository<Prosol_Users> ServiceReportService, IRepository<Prosol_RequestService> ServiceRequestService,
                                    IRepository<Prosol_ServiceCategory> ServiceCategory,
                                    IRepository<Prosol_ServiceGroup> ServiceGroup,
                                    IRepository<Prosol_SMainCode> ServiceMainCodeRep,
                                    IRepository<Prosol_SSubCode> ServiceSubCodeRep,
                                    IRepository<Prosol_SSubSubCode> ServiceSubSubCodeRep,
                                    IRepository<Prosol_MSAttribute> MSAttributeRep,
                                     IRepository<Prosol_Abbrevate> AbbreviateRepository)
        {
            this._ServiceReportService = ServiceReportService;
            this._ServiceRequestService = ServiceRequestService;
            this._ServiceCategory = ServiceCategory;
            this._ServiceGroup = ServiceGroup;
            this._ServiceMainCodeRep = ServiceMainCodeRep;
            this._ServiceSubCodeRep = ServiceSubCodeRep;
            this._ServiceSubSubCodeRep = ServiceSubSubCodeRep;
            this._MSAttributeRep = MSAttributeRep;
            this._abbreviateRepository = AbbreviateRepository;
        }
        public IEnumerable<Prosol_Users> getuser(string role)
        {

          //  var query = Query.EQ("Usertype", role); 
            var query = Query.EQ("Roles.Name", role);
            var vn = _ServiceReportService.FindAll(query).ToList();
            return vn;
        }

        public IEnumerable<Prosol_RequestService> getsearchresult(string PlantCode, string role, string Userid, string[] options, string Fromdate, string Todate)            
        {

            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            //List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            //Dictionary<string, object> rowh;
            // str = str.Remove(str.Length - 1, 1) + ",";
            if (Todate != "" && Todate != null && Todate != "undefined")
            Todate = Todate.Remove(Todate.Length -13, 13) + "23:59:59.000Z";
           
            var query = new List<IMongoQuery>();

           //SS int check = 0;

            // code for options starts
            if (options != null)
            {
                if (options.Count() == 1)
                {
                    if (options[0] == "Pending")
                    {
                        if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
                        {
                            if (role == "Cleanser")
                            {
                                //var query11 = Query.EQ("ServiceStatus", "Cleanse");
                                var query11 = Query.Or(Query.EQ("ServiceStatus", "Cleanse"), Query.EQ("ServiceStatus", "Cleansed"));
                                query.Add(query11);
                                
                            }
                            else if (role == "Reviewer")
                            {
                                // var query11 = Query.EQ("ServiceStatus", "Review");
                                var query11 = Query.Or(Query.EQ("ServiceStatus", "Review"), Query.EQ("ServiceStatus", "QC"));
                                query.Add(query11);
                            }
                            else if (role == "Releaser")
                            {
                                // var query11 = Query.EQ("ServiceStatus", "Release");
                                var query11 = Query.Or(Query.EQ("ServiceStatus", "Release"), Query.EQ("ServiceStatus", "QA"));
                                query.Add(query11);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var query11 = Query.And(Query.NE("ServiceStatus", "Completed"), Query.NE("ServiceStatus", "Rejected"));
                            query.Add(query11);
                        }

                    }
                    else if (options[0] == "Completed")
                    {
                        if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
                        {

                            if (role == "Cleanser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    date1 = date1.AddDays(1);
                                    date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Rejected"),Query.GTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date1)));
                                    query.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Rejected"));
                                    query.Add(qury2);

                                }

                            }
                            else if (role == "Reviewer")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"),Query.NE("ServiceStatus", "Rejected"), Query.GTE("Reviewer.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Reviewer.UpdatedOn", BsonDateTime.Create(date1)));
                                    query.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.NE("ServiceStatus", "Rejected"));
                                    query.Add(qury2);

                                }

                            }
                            else if (role == "Releaser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"),Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC") ,Query.NE("ServiceStatus", "Release"),Query.NE("ServiceStatus", "QA"), Query.NE("ServiceStatus", "Rejected"), Query.GTE("Releaser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Releaser.UpdatedOn", BsonDateTime.Create(date1)));
                                    query.Add(qury2);
                                }
                                else
                                {
                                   var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"), Query.NE("ServiceStatus", "Rejected"));
                                   
                                    query.Add(qury2);

                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var query11 = Query.EQ("ServiceStatus", "Completed");
                            query.Add(query11);
                        }

                    }
                    else if (options[0] == "Rejected")
                    {
                        if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
                        {
                            if (role == "Cleanser")
                            {

                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));


                                    //  var qury2 = Query.And(Query.EQ("RejectedBy", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    // var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    query.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"));
                                    query.Add(qury2);

                                }


                            }
                            else if (role == "Reviewer")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));

                                    // var qury2 = Query.And(Query.EQ("RejectedBy", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    query.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"));
                                    query.Add(qury2);
                                }

                            }
                            else if (role == "Releaser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    // var qury2 = Query.And(Query.EQ("RejectedBy", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
                                    query.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"));
                                    query.Add(qury2);
                                }

                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var query11 = Query.EQ("ServiceStatus", "Rejected");
                            query.Add(query11);

                        }
                    }
                    else
                    {

                    }
                }
                if (options.Count() == 2)
                {
                    string str1 = options[0];
                    string str2 = options[1];
                    var optionquery = new List<IMongoQuery>();

                    if (str1 != "Pending" && str2 != "Pending")
                    {
                        if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
                        {
                            if (role == "Cleanser")

                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.GTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"));
                                    query.Add(qury2);
                                }
                            }
                            if (role == "Reviewer")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"),Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.GTE("Reviewer.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Reviewer.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"),Query.EQ("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"));
                                    query.Add(qury2);
                                }
                            }
                            if (role == "Releaser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"), Query.GTE("Releaser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Releaser.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"),Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"));
                                    query.Add(qury2);
                                }
                            }
                        }
                        else
                        {
                            var qury2 = Query.Or(Query.EQ("ServiceStatus", "Completed"), Query.EQ("ServiceStatus", "Rejected"));
                            query.Add(qury2);
                        }
                    }
                    if (str1 != "Completed" && str2 != "Completed")
                    {
                        if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
                        {
                            if (role == "Cleanser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Completed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC") ,Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"), Query.GTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Completed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"));
                                    query.Add(qury2);
                                }
                            }
                            if (role == "Review")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed") ,Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"), Query.NE("ServiceStatus", "Completed"), Query.GTE("Reviewer.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Reviewer.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Completed"), Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Release"), Query.NE("ServiceStatus", "QA"));
                                    query.Add(qury2);
                                }
                            }
                            if (role == "Release")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.NE("ServiceStatus", "Completed"), Query.GTE("Release.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Release.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"),Query.NE("ServiceStatus", "Completed"));
                                    query.Add(qury2);
                                }
                            }
                        }
                        else
                        {
                            var qury2 = Query.NE("ServiceStatus", "Completed");
                            query.Add(qury2);
                        }


                    }
                    if (str1 != "Rejected" && str2 != "Rejected")
                    {
                        if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
                        {
                            if (role == "Cleanser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Rejected"), Query.GTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Rejected"));
                                    query.Add(qury2);
                                }
                            }

                            if (role == "Reviewer")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Rejected"), Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.GTE("Reviewer.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Reviewer.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Rejected"), Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"));
                                    query.Add(qury2);
                                }
                            }

                            if (role == "Releaser")
                            {
                                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                                {
                                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Rejected"), Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"), Query.GTE("Releaser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Releaser.UpdatedOn", BsonDateTime.Create(date1)));
                                    optionquery.Add(qury2);
                                }
                                else
                                {
                                    var qury2 = Query.And(Query.NE("ServiceStatus", "Rejected"), Query.NE("ServiceStatus", "Cleanse"), Query.NE("ServiceStatus", "Cleansed"), Query.NE("ServiceStatus", "Review"), Query.NE("ServiceStatus", "QC"));
                                    query.Add(qury2);
                                }
                            }
                        }
                        else
                        {
                            var qury2 = Query.NE("ServiceStatus", "Rejected");
                            query.Add(qury2);
                        }

                    }

                }
                if (options.Count() == 3)
                {


                }


            }


            // code for optoins ends






            if (PlantCode != null && PlantCode != "" && PlantCode != "undefined" )
            {
                if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null && options!=null? options.Count() < 1: false )
                {
                    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                      var qury2 = Query.And(Query.EQ("PlantCode", PlantCode), Query.GTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Last_updatedBy.UpdatedOn", BsonDateTime.Create(date1)));
                        query.Add(qury2);
                }
                else
                {
                    var query1 = Query.EQ("PlantCode", PlantCode);
                    query.Add(query1);
                }
            }

            if (role != "Requester" )
                if (role != null && role != "" && role != "undefined" && Userid != null && Userid != "" && Userid != "undefined")
                {

                    if (role == "Cleanser")
                    {
                        if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null && options != null ? options.Count() < 1 : false)
                        {
                            var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                            var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                            var qury2 = Query.And(Query.EQ("Cleanserr.UserId", Userid), Query.GTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Cleanserr.UpdatedOn", BsonDateTime.Create(date1)));
                            query.Add(qury2);
                        }
                        else
                        {
                            var query11 = Query.EQ("Cleanserr.UserId", Userid);
                            query.Add(query11);
                        }
                    }

                    if (role == "Reviewer")
                    {
                        if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null && options != null ? options.Count() < 1 : false)
                        {
                            var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                            var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                            var qury2 = Query.And(Query.EQ("Reviewer.UserId", Userid), Query.GTE("Reviewer.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Reviewer.UpdatedOn", BsonDateTime.Create(date1)));
                            query.Add(qury2);
                        }
                        else
                        {
                            var query11 = Query.EQ("Reviewer.UserId", Userid);
                            query.Add(query11);
                        }
                    }

                    if (role == "Releaser")
                    {
                        if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null && options != null ? options.Count() < 1 : false)
                        {
                            var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                            var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                            var qury2 = Query.And(Query.EQ("Releaser.UserId", Userid), Query.GTE("Releaser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Releaser.UpdatedOn", BsonDateTime.Create(date1)));
                            query.Add(qury2);
                        }
                        else
                        {
                            var query11 = Query.EQ("Releaser.UserId", Userid);
                            query.Add(query11);
                        }
                    }

                }
                else
                {
                    if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null && Userid != "" && Userid != null && Userid != "undefined" && options != null ? options.Count() < 1 : false)
                    {
                        var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                        var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                        var qury2 = Query.And(Query.EQ("requester.UserId", Userid), Query.GTE("requester.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("requester.UpdatedOn", BsonDateTime.Create(date1)));
                        query.Add(qury2);
                    }
                    //else 
                    //{
                    //    if (Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null)
                    //    {
                    //        var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
                    //        var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
                    //        var qury2 = Query.And(Query.GTE("requester.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("requester.UpdatedOn", BsonDateTime.Create(date1)));
                    //        query.Add(qury2);
                    //    }
                    //}
                    //else
                    //{

                    //    var query11 = Query.EQ("requester.UserId", Userid);
                    //    query.Add(query11);
                    //}
                    //new enable
                    //else
                    //{

                    //    var query11 = Query.EQ("requester.UserId", Userid);
                    //    query.Add(query11);
                    //}

                }

            //////////////NEWCODE

            //if (PlantCode != "" && PlantCode != "undefined" && PlantCode != null && Fromdate != "" && Todate != "" && Fromdate != "undefined" && Todate != "undefined" && Fromdate != null && Todate != null && options[0] == "Rejected")
            //{

            //    var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
            //    var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
            //    var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.EQ("ServiceStatus", "Rejected"), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
            //    query.Add(qury2);
            //}

            //else
            //{

            //    var query11 = Query.EQ("RejectedBy.UserId", Userid);
            //    query.Add(query11);
            //}
            ////////////


        //   code for rejected starts

            //if (options[0] == "Rejected")
            //{
            //    if (role == "Cleanser" || role == "Reviewer" || role == "Releaser" || role == "Requester")
            //    {


            //        if (role == "Cleanser")
            //        {
            //            var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
            //            var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
            //            var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
            //            query.Add(qury2);
            //        }

            //        else if (role == "Reviewer")
            //        {
            //            var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
            //            var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
            //            var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
            //            query.Add(qury2);
            //        }


            //        else if (role == "Releaser")
            //        {
            //            var date = DateTime.Parse(Fromdate, new CultureInfo("en-US", true));
            //            var date1 = DateTime.Parse(Todate, new CultureInfo("en-US", true));
            //            var qury2 = Query.And(Query.EQ("RejectedBy.UserId", Userid), Query.GTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("RejectedBy.UpdatedOn", BsonDateTime.Create(date1)));
            //            query.Add(qury2);
            //        }


            //        else
            //        {

            //            var query11 = Query.EQ("RejectedBy.UserId", Userid);
            //            query.Add(query11);
            //        }
            //    }
            //}


             //     rejected code ends
            //////



            if (query.Count > 0)
            {
                var querry = Query.And(query);
                var result = _ServiceRequestService.FindAll(querry).ToList();
                return result;
            }
            else
            {
                List<Prosol_RequestService> pcs = new List<Prosol_RequestService>();
                pcs = null;
                return pcs;
            }

        }



        public virtual int BulkServiceCategory(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];

            var LstNM = new List<Prosol_ServiceCategory>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "")
                {
                    var Mdl = new Prosol_ServiceCategory();
                    // Mdl.Attribute = dr[0].ToString();
                    Mdl.SeviceCategorycode = dr[0].ToString().ToUpper();
                    Mdl.SeviceCategoryname = dr[1].ToString().ToUpper();
                    Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    Mdl.Islive = true;
                    LstNM.Add(Mdl);
                }
            }
            if (LstNM.Count > 0)
            {
                //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

           //     List<Prosol_ServiceCategory> filteredList = LstNM.GroupBy(p => new { p.SeviceCategorycode, p.SeviceCategoryname }).Select(g => g.First()).ToList();
              //  if (filteredList.Count > 0)
              //  {
                    var fRes = new List<Prosol_ServiceCategory>();
                    foreach (Prosol_ServiceCategory nm in LstNM)
                    {
                        var query = Query.Or(Query.EQ("SeviceCategorycode", nm.SeviceCategorycode), Query.EQ("SeviceCategoryname", nm.SeviceCategoryname));
                        var ObjStr = _ServiceCategory.FindOne(query);
                        if (ObjStr == null)
                        {
                            fRes.Add(nm);

                        }
                    }
                    cunt = _ServiceCategory.Add(fRes);

                //}
            }
            return cunt;

        }

        //bulkgroup



        public virtual int bulkGroupUpload(HttpPostedFileBase file)
        {      
            
            // Getting excel data to DT using Strem     
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];

            // Checking Servicecatagoryname and finding servicecatagorycode
            int count = 0;
            var LstNM = new List<Prosol_ServiceGroup>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                {
                    var sort = Query.EQ("SeviceCategoryname",dr[0].ToString() );
                    var groupcodes = _ServiceCategory.FindOne(sort);
                    if(groupcodes != null)
                    { 
                    var Mdl = new Prosol_ServiceGroup();
                     Mdl.ServiceGroupcode = dr[1].ToString().ToUpper();
                    Mdl.ServiceGroupname = dr[2].ToString().ToUpper();
                        Mdl.SeviceCategorycode = groupcodes.SeviceCategorycode;
                        Mdl.SeviceCategoryname = dr[0].ToString().ToUpper();
                    Mdl.Islive = true;
                    LstNM.Add(Mdl);
                        count++;
                }

                    else
                    {

                    }
                }
            }

            // inserting groupcodes of bulk to db
          
            if (LstNM.Count > 0)
            {
                //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

                //     List<Prosol_ServiceCategory> filteredList = LstNM.GroupBy(p => new { p.SeviceCategorycode, p.SeviceCategoryname }).Select(g => g.First()).ToList();
                //  if (filteredList.Count > 0)
                //  {
                var fRes = new List<Prosol_ServiceGroup>();
                foreach (Prosol_ServiceGroup nm in LstNM)
                {
                    var query = Query.And(Query.EQ("SeviceCategorycode", nm.SeviceCategorycode),Query.Or(Query.EQ("ServiceGroupcode", nm.ServiceGroupcode), Query.EQ("ServiceGroupname", nm.ServiceGroupname)));
                    var ObjStr = _ServiceGroup.FindOne(query);
                    if (ObjStr == null)
                    {
                        fRes.Add(nm);

                    }
                }
             //   count = _ServiceCategory.Add(fRes);

                //}
                count = _ServiceGroup.Add(fRes);
            }
           

            // returning no of records inserted into db
            return count;

        }

        //bulkmaincode

        public virtual int bulkServiceMainCodeUpload(HttpPostedFileBase file)
        {

            // Getting excel data to DT using Strem     
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];

            // Checking Servicecatagoryname and finding servicecatagorycode
            int count = 0;
            var LstSM = new List<Prosol_SMainCode>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                {
                    var sort = Query.EQ("SeviceCategoryname", dr[0].ToString());
                    var groupcodes = _ServiceCategory.FindOne(sort);
                    if (groupcodes != null)
                    {
                        var Mdl = new Prosol_SMainCode();
                        Mdl.MainCode = dr[1].ToString().ToUpper();
                        Mdl.MainDiscription = dr[2].ToString().ToUpper();
                        Mdl.SeviceCategorycode = groupcodes.SeviceCategorycode;
                        Mdl.SeviceCategoryname = dr[0].ToString().ToUpper();
                        Mdl.Islive = true;
                        LstSM.Add(Mdl);
                        count++;
                    }

                    else
                    {

                    }
                }
            }

            // inserting groupcodes of bulk to db

            if (LstSM.Count > 0)
            {

                var fRes = new List<Prosol_SMainCode>();
                foreach (Prosol_SMainCode sm in LstSM)
                {
                    var query = Query.And(Query.EQ("SeviceCategorycode", sm.SeviceCategorycode), Query.Or(Query.EQ("MainCode", sm.MainCode), Query.EQ("MainDiscription", sm.MainDiscription)));
                    var ObjStr = _ServiceMainCodeRep.FindOne(query);
                    if (ObjStr == null)
                    {
                        fRes.Add(sm);

                    }
                }
                //   count = _ServiceCategory.Add(fRes);

                //}
                count = _ServiceMainCodeRep.Add(fRes);
            }


            // returning no of records inserted into db
            return count;

        }

        //subcoe

        public virtual int bulkServiceSubCodeUpload(HttpPostedFileBase file)
        {

            // Getting excel data to DT using Strem     
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];

            // Checking Servicecatagoryname and finding servicecatagorycode
            int count = 0;
            var LstSM = new List<Prosol_SSubCode>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                {
                    var sort = Query.EQ("MainDiscription", dr[0].ToString());
                    var groupcodes = _ServiceMainCodeRep.FindOne(sort);
                    if (groupcodes != null)
                    {
                        var Mdl = new Prosol_SSubCode();
                        Mdl.SubCode = dr[1].ToString().ToUpper();
                        Mdl.SubDiscription = dr[2].ToString().ToUpper();
                        Mdl.MainCode = groupcodes.MainCode;
                        Mdl.MainDiscription = dr[0].ToString().ToUpper();
                        Mdl.Islive = true;
                        LstSM.Add(Mdl);
                        count++;
                    }

                    else
                    {

                    }
                }
            }

            // inserting groupcodes of bulk to db

            if (LstSM.Count > 0)
            {

                var fRes = new List<Prosol_SSubCode>();
                foreach (Prosol_SSubCode sm in LstSM)
                {
                    var query = Query.And(Query.EQ("MainCode", sm.MainCode), Query.Or(Query.EQ("SubCode", sm.SubCode), Query.EQ("SubDiscription", sm.SubDiscription)));
                    var ObjStr = _ServiceSubCodeRep.FindOne(query);
                    if (ObjStr == null)
                    {
                        fRes.Add(sm);

                    }
                }
                //   count = _ServiceCategory.Add(fRes);

                //}
                count = _ServiceSubCodeRep.Add(fRes);
            }


            // returning no of records inserted into db
            return count;

        }

        //subsubcode

        public virtual int bulkServiceSubSubCodeUpload(HttpPostedFileBase file)
        {

            // Getting excel data to DT using Strem     
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];

            // Checking Servicecatagoryname and finding servicecatagorycode
            int count = 0;
            var LstSM = new List<Prosol_SSubSubCode>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "" && dr[3].ToString() != "")
                {
                    // var sort = Query.EQ("MainDiscription", dr[0].ToString());
                    // var sort = Query.And(Query.EQ("MainDiscription", dr[0].ToString()), Query.EQ("SubDiscription", dr[1].ToString()));
                    var sort = Query.And(Query.EQ("MainDiscription", dr[0].ToString()), Query.EQ("SubDiscription", dr[1].ToString()));
                    var Codes = _ServiceSubCodeRep.FindOne(sort);
                    if (Codes != null)
                    {
                        var Mdl = new Prosol_SSubSubCode();

                        Mdl.MainDiscription = dr[0].ToString().ToUpper();
                        Mdl.SubDiscription = dr[1].ToString().ToUpper();
                        Mdl.SubSubCode = dr[2].ToString().ToUpper();
                        Mdl.SubSubDiscription = dr[3].ToString().ToUpper();
                        Mdl.MainCode = Codes.MainCode;
                        Mdl.SubCode = Codes.SubCode;
                 
                        Mdl.Islive = true;
                        LstSM.Add(Mdl);
                        count++;
                    }

                    else
                    {

                    }
                }
            }

            // inserting groupcodes of bulk to db

            if (LstSM.Count > 0)
            {

                var fRes = new List<Prosol_SSubSubCode>();
                foreach (Prosol_SSubSubCode sm in LstSM)
                {
                    var query = Query.And(Query.EQ("MainCode", sm.MainCode), Query.EQ("SubCode", sm.SubCode), Query.Or(Query.EQ("SubSubCode", sm.SubSubCode), Query.EQ("SubSubDiscription", sm.SubSubDiscription)));

                    var ObjStr = _ServiceSubSubCodeRep.FindOne(query);
                    if (ObjStr == null)
                    {
                        fRes.Add(sm);

                    }
                }
                //   count = _ServiceCategory.Add(fRes);

                //}
                count = _ServiceSubSubCodeRep.Add(fRes);
            }


            // returning no of records inserted into db
            return count;

        }

        //bulkcharateristics
        public int BulkCharacteristicUpload(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;

            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];
            var LstChar = new List<Prosol_MSAttribute>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                {
                    // var sort = Query.EQ("MainDiscription", dr[0].ToString());

                    // var sort = Query.And(Query.EQ("MainDiscription", dr[0].ToString()), Query.EQ("SubDiscription", dr[1].ToString()));
                   // var Codes = _ServiceSubCodeRep.FindOne(sort);
                    var sort = Query.And(Query.EQ("Noun", dr[0].ToString()), Query.EQ("Modifier", dr[1].ToString()));
                    var Codes = _MSAttributeRep.FindOne(sort);
                    
                  
                    if (Codes != null)
                    {
                        var Mdl = new Prosol_MSAttribute();

                        Mdl.Noun = dr[0].ToString().ToUpper();
                        Mdl.Modifier = dr[1].ToString().ToUpper();
                        Mdl.Attributes = dr[2].ToString().ToUpper();
                        Mdl.Sequence = dr[3] != null ? Convert.ToInt16(dr[3]) : 0;
                        //Mdl.MainCode = Codes.SeviceCategorycode;
                        //Mdl.SubCode = Codes.ServiceGroupcode;


                        LstChar.Add(Mdl);
                        cunt++;
                    }

                    else
                    {
                        var Mdl = new Prosol_MSAttribute();

                        Mdl.Noun = dr[0].ToString().ToUpper();
                        Mdl.Modifier = dr[1].ToString().ToUpper();
                        Mdl.Attributes = dr[2].ToString().ToUpper();
                        Mdl.Sequence = dr[3] != null ? Convert.ToInt16(dr[3]) : 0;
                        //Mdl.MainCode = Codes.SeviceCategorycode;
                        //Mdl.SubCode = Codes.ServiceGroupcode;


                        // LstChar.Add(Mdl);
                        _MSAttributeRep.Add(Mdl);
                        cunt++;
                    }
                }
            }

            //if (LstChar.Count > 0)
            //{

            //    //List<Prosol_Charateristics> NMList = LstChar.GroupBy(p => new { p.Noun, p.Modifier }).Select(g => g.First()).ToList();

            //    List<Prosol_MSAttribute> filteredList = LstChar.GroupBy(p => new { p.MainDiscription, p.SubDiscription, p.Attributes }).Select(g => g.First()).ToList();

            //    if (filteredList.Count > 0)
            //    {
            //        var fRes = new List<Prosol_MSAttribute>();
            //        int flg = 0;
            //        foreach (Prosol_MSAttribute nm in filteredList.ToList())
            //        {
            //            //var query1 = Query.And(Query.EQ("MainDiscription", nm.MainDiscription), Query.EQ("SubDiscription", nm.SubDiscription));
            //            //  var ObjStr1 = _ServiceSubCodeRep.FindOne(query1);
            //            //var query1 = Query.And(Query.EQ("SeviceCategoryname", nm.MainDiscription), Query.EQ("ServiceGroupname", nm.SubDiscription));
            //            //var ObjStr1 = _ServiceGroup.FindOne(query1);
            //            //var ObjStr1 = _MSAttributeRep.FindOne(query1);
            //            if (ObjStr1 != null)
            //            {
            //                flg = 1;
            //                var query = Query.And(Query.EQ("MainDiscription", nm.MainDiscription), Query.EQ("SubDiscription", nm.SubDiscription), Query.EQ("Attributes", nm.Attributes));
            //                var ObjStr = _MSAttributeRep.FindOne(query);
            //                if (ObjStr == null)
            //                {
            //                    fRes.Add(nm);
            //                }
            //            }
            //        }
            //        if (flg == 1 && fRes.Count > 0)
            //        {
            //            cunt = _MSAttributeRep.Add(fRes);
            //        }
            //        if (flg == 1 && fRes.Count == 0)
            //        {
            //            cunt = 0;
            //        }
            //        else if (flg == 0) cunt = -1;

            //    }
            //}
            return cunt;


        }

        public int BulkvalueUpload(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;

            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];
            DataTable resTbl = dt.AsEnumerable().GroupBy(r => new { Col1 = r["Activity"], Col2 = r["Attributes"] }).Select(x => x.First()).CopyToDataTable();
            var LstChar = new List<Prosol_Charateristics>();
            foreach (DataRow dr in resTbl.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                {

                    DataRow[] result = dt.Select("Activity = '" + dr[0].ToString() + "' and Attributes = '" + dr[1].ToString() + "'");
                    List<string> LstStr = new List<string>();
                    // string[] str = new string[result.Length];
                    //  int i = 0;
                    foreach (DataRow row in result)
                    {
                        //str[i]= row["VALUE"].ToString();                      
                        var Qry = Query.EQ("Value", row["Values"].ToString());
                        var obj = _abbreviateRepository.FindOne(Qry);
                        if (obj != null)
                        {
                            LstStr.Add(obj._id.ToString());
                        }
                        else
                        {
                            var tmpmdl = new Prosol_Abbrevate();
                            tmpmdl.Value = row["Values"].ToString();
                            tmpmdl.Approved = "Yes";
                            _abbreviateRepository.Add(tmpmdl);
                            var obj1 = _abbreviateRepository.FindOne(Qry);
                            LstStr.Add(obj1._id.ToString());

                        }
                    }
                    var query = Query.And(Query.EQ("Activity", dr[0].ToString()), Query.EQ("Attributes", dr[1].ToString()));
                    var ObjStr = _MSAttributeRep.FindOne(query);
                    if (ObjStr != null)
                    {
                       // var Lstobj = ObjStr.Values.ToList();
                        if (ObjStr.Values != null)
                        {
                           var Lstobj = ObjStr.Values.ToList();
                            foreach (string stng in LstStr)
                            {
                                if (!ObjStr.Values.Contains(stng))
                                {
                                    Lstobj.Add(stng);
                                }
                            }
                            ObjStr.Values = Lstobj.ToArray();
                        }
                        else
                        {
                            //Lstobj = ObjStr.Values.ToList();
                            ObjStr.Values = LstStr.ToArray();

                        }
                        _MSAttributeRep.Add(ObjStr);
                    }




                    cunt++;
                }
            }
            return cunt;
        }
    }
}