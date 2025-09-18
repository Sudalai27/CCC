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
using MongoDB.Driver;
using System.Web;
using System.IO;
using Excel;
using System.Data;
using System.Globalization;

namespace Prosol.Core.Interface
{

    public partial class BomService : I_Bom
    {
        private readonly IRepository<Prosol_equipbom> _equipbomRepository;
        private readonly IRepository<Prosol_MaterialBom> _matbomRepository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_Funloc> _FunlocRepository;
        private readonly IRepository<Prosol_Master> _masterRepository;
        private readonly IRepository<Prosol_ERPInfo> _ERPInfoRepository;
        private readonly IRepository<Prosol_Vendor> _VendorRepository;
        private readonly IRepository<prosol_Mcp> _mcprepository;
        private readonly IRepository<Prosol_Tasklist> _tasklistrepository;
        private readonly IRepository<Prosol_MaintenancePlan> _MPrepository;
        private readonly IRepository<Prosol_IOModel> _IOModel;



        public BomService(IRepository<Prosol_Vendor> vendorRepository, 
            IRepository<Prosol_ERPInfo> ERPInfoRepository, 
            IRepository<Prosol_Master> MasterRepository, 
            IRepository<Prosol_Datamaster> datamasterRepository,
            IRepository<Prosol_equipbom> EquipbomRepository, 
            IRepository<Prosol_MaterialBom> MatbomRepository, 
            IRepository<Prosol_Funloc> funlocRepository,
            IRepository<prosol_Mcp> mcprepository,
             IRepository<Prosol_Tasklist> tasklistrepository,
              IRepository<Prosol_MaintenancePlan> MPrepository,
                IRepository<Prosol_IOModel> IOModel)

        {
            this._equipbomRepository = EquipbomRepository;
            this._FunlocRepository = funlocRepository;
            this._matbomRepository = MatbomRepository;
            this._DatamasterRepository = datamasterRepository;
            this._masterRepository = MasterRepository;
            this._ERPInfoRepository = ERPInfoRepository;
            this._VendorRepository = vendorRepository;
            this._mcprepository = mcprepository;
            this._tasklistrepository = tasklistrepository;
            this._MPrepository = MPrepository;
            this._IOModel = IOModel;
        }
        public int InsertData(List<Prosol_equipbom> data)
        {
            bool res = false;
            //   int cou = 0;
            var query = Query.EQ("TechIdentNo", data[0].TechIdentNo);

            _equipbomRepository.Delete(query);

            foreach (Prosol_equipbom data1 in data)
            {

                res = _equipbomRepository.Add(data1);

            }
            return data.Count;

        }
        public int InsertMatData(List<Prosol_MaterialBom> data)
        {
            bool res = false;
            var query = Query.EQ("HeaderBID", data[0].HeaderBID);
            var vn = _matbomRepository.Delete(query);
            foreach (Prosol_MaterialBom data1 in data)
            {


                res = _matbomRepository.Add(data1);

            }
            return data.Count;

        }

        public bool updateMP(Prosol_MaintenancePlan pmp)
        {
           var res =  _MPrepository.Add(pmp);

            return res;
        }
        //public string getItem()
        //{
        //    string code = "";
        //   // var sort = SortBy.Descending("_id");

        //    var query = Query.Matches("Itemcode", new BsonRegularExpression("^[0-9]*$"));          
        //    var Itmcode = _equipbomRepository.FindAll(query).ToList();
        //    if (Itmcode != null && Itmcode.Count > 0)
        //    {
        //        code = Itmcode[0].Itemcode;
        //    }
        //    return code;
        //}
        public List<Prosol_Master>matty(string term)
        {
            string[] str = { "Label","Code"};
            var fields = Fields.Include(str);
            var qry = Query.Or(Query.Matches("Label", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));


            var arrResult1 = _masterRepository.FindAll(fields, qry).ToList();
            return arrResult1;
        }
        public List<Prosol_Funloc> Total()
        {
            var arrResult = _FunlocRepository.FindAll().ToList();
            return arrResult;
        }
        public List<Prosol_equipbom> complete()
        {
            var arrResult = _equipbomRepository.FindAll().ToList();
           
            return arrResult;

        }
        public List<Prosol_Funloc> master()
        {
            var arrResult = _FunlocRepository.FindAll().ToList();

            return arrResult;

        }
        public List<Prosol_MaterialBom> masterdata()
        {
            var arrResult = _matbomRepository.FindAll().ToList();

            return arrResult;

        }
        
        public List<Prosol_ERPInfo> get(string term)
        {
            string[] str = { "Materialtype" };
            var fields = Fields.Include(str);
           

          
            var qry = Query.Or(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var arrResult = _ERPInfoRepository.FindAll(fields, qry).ToList();
            return arrResult;

        }
        public List<Prosol_equipbom> Codecount(string term)
        {
            string[] str = { "Materialtype" };
            var fields = Fields.Include(str);
            var qry = Query.Or(Query.Matches("Materialtype", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var arrResult = _equipbomRepository.FindAll(fields, qry).ToList();
            return arrResult;

        }
        public List<Prosol_Datamaster> totalmat()
        {
            var arrResult = _DatamasterRepository.FindAll().ToList();
            return arrResult;

        }
        
        public List<Prosol_equipbom> allmat()
        {
            string[] str = { "Itemcode" };
            var fields = Fields.Include(str);
           
            var arrResult = _equipbomRepository.FindAll(fields).ToList();
            return arrResult;

        }
        public List<Prosol_MaterialBom> material()
        {
            var arrResult = _matbomRepository.FindAll().ToList();
            return arrResult;

        }
        public List<Prosol_Funloc> funlocsearch1()
        {

            var arrResult = _FunlocRepository.FindAll().ToList();
            return arrResult;
           
        }
        public List<Prosol_Funloc> funlocsearch2()
        {

            var arrResult = _FunlocRepository.FindAll().ToList();
            return arrResult;

        }
        public IEnumerable<Prosol_IOModel> getinrecord()
        {



            //string[] Userfield = { "SeviceCategorycode", "SeviceCategoryname", "Islive", "_id" };
            // var fields = Fields.Exclude("_id");
            //var sort = SortBy.Descending("UpdatedOn");
            //var query = Query.EQ("INtime")

            //string[] str = { "INtime", "Materialcode" };
            //  var fields = Fields.Include(str);
            var shwusr = _IOModel.FindAll().ToList();
            return shwusr;
        }
        public IEnumerable<Prosol_IOModel> trackmulticodelist(string codestr)
        {
            //  string[] search_field = { "Itemcode", "Legacy", "Noun", "Modifier", "Legacy2" };
            // var fields = Fields.Include(search_field).Exclude("_id");
            var query = Query.Or(Query.EQ("FunctLocation", codestr), (Query.EQ("Materialcode", codestr)), (Query.EQ("TechIdentNo", codestr)));
            var getdata = _IOModel.FindAll(query).ToList();
            return getdata;
        }
        //reportinandout
        public List<Dictionary<string, object>> Trackload(string materialcode, string fromdate, string todate, string option)
        {
            IMongoQuery qury1, qury2;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            //  if (!string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            //  {
            var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
            date1 = date1.AddDays(1);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);

            if (option == "InTime")
            {
                qury2 = Query.And(Query.GTE("INtime", BsonDateTime.Create(date)), Query.LTE("INtime", BsonDateTime.Create(date1)));
            }
            else
            {
                qury2 = Query.And(Query.GTE("OUTtime", BsonDateTime.Create(date)), Query.LTE("OUTtime", BsonDateTime.Create(date1)));
            }
            //var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
            //date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            //if (option == "InTime")
            //{
            //    qury2 = Query.And(Query.GTE("INtime", BsonDateTime.Create(date)), Query.LTE("INtime", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
            //}
            //else
            //{
            //    qury2 = Query.And(Query.GTE("OUTtime", BsonDateTime.Create(date)), Query.LTE("OUTtime", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
            //}




            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            bool goo1;
            var mergelist1 = (dynamic)null;
            if (option == "InTime")
            {

                // string[] flds = { "Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Catalogue.Name", "Review.Name", "Release.Name", "Remarks" };
                var datalist1 = _IOModel.FindAll(qury2).ToList();
                //quryplnt = Query.EQ("Plant", plant);
                //var plntlist = _ERPInfoRepository.FindAll(quryplnt).ToList();
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

                    row.Add("Material Code", cde.Materialcode);

                    DateTime date2 = DateTime.Parse(Convert.ToString(cde.Createdon));
                    row.Add("CreatedOn", date2.ToString("dd/MM/yyyy"));

                    row.Add("FunctLocation", cde.FunctLocation);
                    row.Add("TechIdentNo", cde.TechIdentNo);
                    if (cde.INtime != null)
                    {
                        DateTime date22 = DateTime.Parse(Convert.ToString(cde.INtime));
                        row.Add("INtime", date22.ToString("dd/MM/yyyy"));
                    }
                    if (cde.OUTtime != null)
                    {
                        DateTime date20 = DateTime.Parse(Convert.ToString(cde.OUTtime));
                        row.Add("OUTtime", date20.ToString("dd/MM/yyyy"));
                    }
                    // row.Add("INtime", cde.INtime);
                    //row.Add("OUTtime", cde.OUTtime);


                    rows.Add(row);
                }
            }
            else
            {
                // string[] flds = { "Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Catalogue.Name", "Review.Name", "Release.Name", "Remarks" };
                // var fields = Fields.Include(flds).Exclude("_id");
                var datalist1 = _IOModel.FindAll(qury2).ToList();

                foreach (var cde in datalist1)
                {

                    row = new Dictionary<string, object>();

                    row.Add("MaterialCode", cde.Materialcode);
                    DateTime date2 = DateTime.Parse(Convert.ToString(cde.Createdon));
                    row.Add("CreatedOn", date2.ToString("dd/MM/yyyy"));
                    row.Add("FunctLocation", cde.FunctLocation);
                    row.Add("TechIdentNo", cde.TechIdentNo);
                    if (cde.INtime != null)
                    {
                        DateTime date22 = DateTime.Parse(Convert.ToString(cde.INtime));
                        row.Add("INtime", date22.ToString("dd/MM/yyyy"));
                    }
                    if (cde.OUTtime != null)
                    {
                        DateTime date20 = DateTime.Parse(Convert.ToString(cde.OUTtime));
                        row.Add("OUTtime", date20.ToString("dd/MM/yyyy"));
                    }
                    //   row.Add("INtime", cde.INtime);
                    //  row.Add("OUTtime", cde.OUTtime);



                    rows.Add(row);
                }

            }




            //   }
            return rows;
        }
        public List<Prosol_Datamaster> frstmat()
        {
            var qry = Query.EQ("ItemStatus", 6);
            var Result = _DatamasterRepository.FindAll(qry).ToList();
            return Result;

        }
        public List<Prosol_equipbom> manfacsearch(string term)
        {
            //string[] str = { "FunctLocation", "FunctDesc", "SupFunctLoc", "Objecttype", "TechIdentNo", "EquipDesc",
            //    "Manufacturer", "ManufCon", "Modelno", "ManufSerialNo","ABCindic" };
            //var fields = Fields.Include(str);
            var qry = Query.Or(Query.EQ("Manufacturer", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
            
            var arrResult = _equipbomRepository.FindAll(qry).ToList();
            return arrResult;

        }
        public List<Prosol_Vendor> mandcsearch(string term)
        {
            string[] str = { "Name", "Address", "City", "Region", "Postal", "Country",
                "Phone"};
            var fields = Fields.Include(str);
            var qry =Query.EQ("ShortDescName",term);
        
            var arrResult = _VendorRepository.FindAll(fields, qry).ToList();
            return arrResult;
          

        }
        public List<Prosol_Funloc> gethei(string term)
        {
            var qry = Query.EQ("SupFunctLoc", term);

            var arrResult = _FunlocRepository.FindAll(qry).ToList();
            return arrResult;
        }


        public List<Prosol_Funloc> funlocsearch(string term)
        {
            string[] str1 = { "FunctLocation", "FunctDesc", "SupFunctLoc", "Objecttype", "TechIdentNo", "EquipDesc",
                "Manufacturer", "ManufCon", "Modelno", "ManufSerialNo","ABCindic" };
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
                            var Qry1 = Query.Or(Query.EQ("FunctLocation",str), Query.EQ("TechIdentNo", str));


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
                                var Qry1 = Query.Or(Query.EQ("FunctLocation", str), Query.EQ("TechIdentNo", str));



                                QryLst.Add(Qry1);
                             
                            }
                            else
                            {
                                //End with

                                var Qry1 = Query.Or(Query.EQ("FunctLocation", str), Query.EQ("TechIdentNo", str));




                                QryLst.Add(Qry1);
                              

                            }

                        }
                    }


                }
                var query = Query.Or(QryLst);
                var arrResult = _FunlocRepository.FindAll(fields, query).ToList();
                return arrResult;


            }
            else
            {
                //string[] str = { "FunctLocation", "FunctDesc", "SupFunctLoc", "Objecttype", "TechIdentNo", "EquipDesc",
                //"Manufacturer", "ManufCon", "Modelno", "ManufSerialNo","ABCindic" };
                //var fields = Fields.Include(str);

                var qry = Query.Or(Query.EQ("FunctLocation", term.TrimStart().TrimEnd()),
                   Query.EQ("FunctDesc", term.TrimStart().TrimEnd()),
                   Query.EQ("SupFunctLoc", term.TrimStart().TrimEnd()),
                   Query.EQ("Objecttype", term.TrimStart().TrimEnd()),
                   Query.EQ("TechIdentNo", term.TrimStart().TrimEnd()),
                   Query.EQ("Manufacturer", term.TrimStart().TrimEnd()),
                   Query.EQ("ManufCon", term.TrimStart().TrimEnd()),
                   Query.EQ("Modelno", term.TrimStart().TrimEnd()),
                   Query.EQ("ManufSerialNo", term.TrimStart().TrimEnd()),
                   Query.EQ("ABCindic", term.TrimStart().TrimEnd()),
                   Query.EQ("EquipDesc", term.TrimStart().TrimEnd()));

                //var qry = Query.Or(Query.Matches("FunctLocation", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("FunctDesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("SupFunctLoc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("Objecttype", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("TechIdentNo", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("Manufacturer", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("ManufCon", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("Modelno", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("ManufSerialNo", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("ABCindic", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                //    Query.Matches("EquipDesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                var arrResult = _FunlocRepository.FindAll(fields, qry).ToList();
                return arrResult;
            }
           
        }
        public List<Prosol_Datamaster> BulkMat1(string term)
        {
            string[] str1 = { "Itemcode" , "Materialcode"};
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
                            var Qry1 = Query.Or(Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                                    Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));




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
                                var Qry1 = Query.Or(Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))),
                                    Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));
            



                                QryLst.Add(Qry1);

                            }
                            else
                            {
                                //End with

                                var Qry1 = Query.Or(Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase))),
                                    Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase))));
                                         



                                QryLst.Add(Qry1);


                            }

                        }
                    }


                }
                var query = Query.Or(QryLst);
                var arrResult = _DatamasterRepository.FindAll(fields, query).ToList();
                return arrResult;


            }
            else
            {
               
               
                var qry1 = Query.Or(Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), 
                    Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                    Query.Matches("Noun", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                    Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                    Query.Matches("Manufacturer", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                    Query.Matches("Partno", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                    Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                    Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

                var Result = _DatamasterRepository.FindAll(fields, qry1).ToList();
                return Result;
            }

        }
        public List<Prosol_equipbom> funequip(string term)
        {

            var qry = Query.Or(Query.Matches("TechIdentNo", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
            var arrResult = _equipbomRepository.FindAll(qry).ToList();

            return arrResult;
        }
        public List<Prosol_MaterialBom> matequip(string term)
        {

            var qry = Query.Or(Query.Matches("HeaderBID", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
            var arrResult = _matbomRepository.FindAll(qry).ToList();

            return arrResult;
        }
        public List<Prosol_equipbom> srch(string term)
        {
           
            var qry = Query.EQ("FunctLocation", term);


            var arrResult1 = _equipbomRepository.FindAll(qry).ToList();
            return arrResult1;
        }
        public List<Prosol_MaterialBom> srchbom(string term)
        {
           
            var qry = Query.EQ("HeaderBID", term);


            var arrResult2 = _matbomRepository.FindAll(qry).ToList();
            return arrResult2;
        }
        public List<Prosol_equipbom> getspares(string term)
        {
            var qry = Query.EQ("TechIdentNo",term.TrimStart().TrimEnd());
            //var qry = Query.Or(Query.Matches("TechIdentNo", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
            var arrResult = _equipbomRepository.FindAll(qry).ToList();
            
            return arrResult;
        }
        public List<Prosol_MaterialBom> getmatspares(string term)
        {

            var qry = Query.Or(Query.Matches("HeaderBID", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
            var arrResult = _matbomRepository.FindAll(qry).ToList();
            return arrResult;
        }
        public List<Prosol_equipbom> get2(string term)
        {

            var qry = Query.EQ("FunctLocation",term);
            var arrResult = _equipbomRepository.FindAll(qry).ToList();
            return arrResult;
        }
        
        public List<Prosol_Datamaster> searching1(string term)
        {
            string[] str = { "Itemcode", "Materialcode", "Noun", "Modifier", "Manufacturer", "Partno", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(str);
            var qry1 = Query.Or(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Noun", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Manufacturer", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Partno", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var Result = _DatamasterRepository.FindAll(fields, qry1).ToList();
            return Result;
        }
        //public List<Prosol_Datamaster> searching11(string term, bool a,bool b,bool c)
        //{
        //    string[] str = { "Itemcode", "Noun", "Modifier", "Manufacturer", "Partno", "Shortdesc", "Longdesc" };
        //    var fields = Fields.Include(str);
        //    var qry1 = Query.Or(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
        //        Query.Matches("Noun", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
        //        Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
        //        Query.Matches("Manufacturer", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
        //        Query.Matches("Partno", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
        //        Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
        //        Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

        //    if (a == true)
        //        qry1 = Query.Or(qry1, Query.Matches("ABCindic", "A"));
        //    if (b == true)
        //        qry1 = Query.Or(qry1, Query.Matches("ABCindic", "B"));
        //    if (c == true)
        //        qry1 = Query.Or(qry1, Query.Matches("ABCindic", "C"));


        //    var Result = _DatamasterRepository.FindAll(fields, qry1).ToList();
        //    return Result;
        //}
        public List<Prosol_equipbom> getreport(string term, bool a, bool b, bool c)
        {


            string aa = "";
            string bb = "";
            string cc = "";

            if (a == true)
                aa = "A";
            if (b == true)
                bb = "B";
            if (c == true)
                cc = "C";

           // var qry = Query.And(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.Or(Query.EQ("ABCindic", aa), Query.EQ("ABCindic", bb), Query.EQ("ABCindic", cc)));
            var qry = Query.EQ("Itemcode", term.TrimStart().TrimEnd());
            var arrResult = _equipbomRepository.FindAll(qry).ToList();

            return arrResult;
        }
        
             public List<Prosol_MaterialBom> getreport12(string term)
        {
            var qry = Query.EQ("Itemcode", term.TrimStart().TrimEnd());

            var arrResult = _matbomRepository.FindAll(qry).ToList();

            return arrResult;
        }
        public List<Prosol_MaterialBom> getreport2(string term)
        {
            var qry = Query.And(Query.EQ("HeaderBID", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd()))));

            var arrResult = _matbomRepository.FindAll(qry).ToList();

            return arrResult;
        }

        public bool Singlefun(Prosol_Funloc data)
        {

            var qry = Query.EQ("FunctLocation", data.FunctLocation);
            var arrResult = _FunlocRepository.FindAll(qry).ToList();
            if(arrResult.Count == 0 )
            {
                var result = _FunlocRepository.Add(data);
                return result;

            }
            
            return false;


        }
        public bool Equipsingle(Prosol_Funloc data)
        {
         //   var Result1 = _FunlocRepository.FindAll().ToList();
            var qry = Query.EQ("FunctLocation", data.FunctLocation);
            var Result = _FunlocRepository.FindAll(qry).ToList();

           
                Result[0].TechIdentNo = data.TechIdentNo;
                Result[0].FunctLocation = data.FunctLocation;
                Result[0].EquipDesc = data.EquipDesc;
                Result[0].EquipCategory = data.EquipCategory;
                Result[0].Weight = data.Weight;
                Result[0].UOM = data.UOM;
                Result[0].Size = data.Size;
                Result[0].AcquisValue = data.AcquisValue;
                Result[0].AcquistnDat = data.AcquistnDat;
                Result[0].Manufacturer = data.Manufacturer;
                Result[0].ManufCon = data.ManufCon;
                Result[0].Modelno = data.Modelno;
                Result[0].ConstructYear = data.ConstructYear;
                Result[0].ConstructMth = data.ConstructMth;
                Result[0].ManufPartNo = data.ManufPartNo;
                Result[0].ManufSerialNo = data.ManufSerialNo;
                Result[0].AuthGroup = data.AuthGroup;
                Result[0].Startupdate = data.Startupdate;
                Result[0].MaintPlant = data.MaintPlant;
                Result[0].Companycode = data.Companycode;
                Result[0].Asset = data.Asset;
                Result[0].Subno = data.Subno;
                Result[0].Catalogprofile = data.Catalogprofile;
                Result[0].Mainworkcenter = data.Mainworkcenter;
                Result[0].ConID = data.ConID;
            if (data.ConID != null)
            {

                List<Prosol_equipbom> ert = new List<Prosol_equipbom>();
                var query1 = Query.And(Query.EQ("FunctLocation", data.FunctLocation));
                var ObjStr1 = _FunlocRepository.FindOne(query1);
                var s = fun1(data.ConID);
                if (s != null)
                {
                    Prosol_equipbom eee = new Prosol_equipbom();
                    eee.FunctLocation = ObjStr1.FunctLocation;
                    eee.FunctDesc = ObjStr1.FunctDesc;
                    eee.Objecttype = ObjStr1.Objecttype;
                    eee.TechIdentNo = data.TechIdentNo;
                    eee.SupFunctLoc = ObjStr1.SupFunctLoc;
                    eee.EquipDesc = data.EquipDesc;
                    eee.Manufacturer = data.Manufacturer;
                    eee.ManufCon = data.ManufCon;
                    eee.Modelno = data.Modelno;
                    eee.ManufSerialNo = data.ManufSerialNo;
                    eee.ABCindic = ObjStr1.ABCindic;
                    eee.itemcat = "I";
                    eee.Itemcode = s.HeaderBID;
                    var ss = searching3(s.HeaderBID);
                    eee.partqnt = "1";
                    eee.Shortdesc = ss.Shortdesc;
                    eee.Longdesc = ss.Longdesc;
                    ert.Add(eee);
                    InsertData(ert);
                }
            }
            var result = _FunlocRepository.Add(Result[0]);
                return result;


            }
        public bool delfun(string id)
        {
           var _id = new MongoDB.Bson.ObjectId(id);
            var qry = Query.EQ("_id", _id);
            var result = _FunlocRepository.Delete(qry);
            return result;
           
        }
        public List<Prosol_MaterialBom> allmat2()
        {
            

            var arrResult = _matbomRepository.FindAll().ToList();

            return arrResult;
        }

        public List<prosol_Mcp> remove_mcp(string mcpcode)
        {
            var query1 = Query.EQ("mcpcode", mcpcode);
               _mcprepository.Delete(query1);

           var res = _FunlocRepository.FindAll(query1).ToList();
            foreach(Prosol_Funloc pf in res)
            {
                Prosol_Funloc nn = new Prosol_Funloc();
                nn = pf;
                nn.mcpcode = "";
                _FunlocRepository.Add(pf);
            }

            return _mcprepository.FindAll().ToList();
        }
        public List<Prosol_equipbom> getreport1(string term)
        {
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

                var arrResult = _equipbomRepository.FindAll(query).ToList();
                return arrResult;


            }
            else
            {



                var qry = Query.And(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

                var arrResult = _equipbomRepository.FindAll(qry).ToList();

                return arrResult;
            }
        }
        public List<Prosol_equipbom> fun(string term)
        {



            var qry = Query.And(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var arrResult = _equipbomRepository.FindAll(qry).ToList();

            return arrResult;
        }
        public Prosol_MaterialBom fun1(string term)
        {



            var qry = Query.And(Query.Matches("HeaderBID", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var arrResult = _matbomRepository.FindOne(qry);

            return arrResult;
        }
        public List<Prosol_Datamaster> searching(string term)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(strArr);

            //if (term.Contains(' '))
            //{
            var QryLst = new List<IMongoQuery>();
            var QryLst1 = new List<IMongoQuery>();
            string[] sepArr = term.Split(' ');
            if (sepArr.Length > 2)
            {
                foreach (string str in sepArr)
                {
                    if (str != "")
                    {

                        var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                        var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

                        // var dynmic = Query.Or(Qry1, Qry2);
                        QryLst.Add(Qry1);
                        QryLst1.Add(Qry2);
                    }
                }
            }
            else
            {
                foreach (string str in sepArr)
                {
                    if (str != "")
                    {
                        //var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", new MongoRegex(string.Format("^{0}", searchKey), "i")
                        var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));
                        // var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(str, RegexOptions.IgnoreCase))));
                        var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));

                        // var dynmic = Query.Or(Qry1, Qry2);
                        QryLst.Add(Qry1);
                        QryLst1.Add(Qry2);
                    }
                }
            }
            var query = Query.And(QryLst);
            var arrResult = _DatamasterRepository.FindAll(fields, query).ToList();

            query = Query.And(QryLst1);
            var LongResult = _DatamasterRepository.FindAll(fields, query).ToList();
            foreach (Prosol_Datamaster mdl in LongResult)
            {
                if (-1 == arrResult.FindIndex(f => f.Itemcode.Equals(mdl.Itemcode)))
                    arrResult.Add(mdl);

            }
            return arrResult;


        }
        public Prosol_Datamaster searching3(string term)
        {

            var qry = Query.And(Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                               Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var arrResult = _DatamasterRepository.FindOne(qry);

            return arrResult;

        }



        //BulkImport
        public virtual int BulkFunloc(HttpPostedFileBase file)
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

            var LstNM = new List<Prosol_Funloc>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1].ToString() != "")
                {
                    var Mdl = new Prosol_Funloc();

                    Mdl.FunctLocation = dr[0].ToString();
                    Mdl.FunctLocCat = dr[1].ToString();
                    Mdl.FunctDesc = dr[2].ToString();
                    Mdl.ABCindic = dr[3].ToString();
                    Mdl.SupFunctLoc = dr[4].ToString();
                    Mdl.Objecttype = dr[5].ToString();
                    Mdl.Startdate = dr[6].ToString();
                    Mdl.AuthGroup = dr[7].ToString();
                    Mdl.MainWrk = dr[8].ToString();
                    Mdl.WBSelement = dr[9].ToString();
                    Mdl.Plantsection = dr[10].ToString();
                    Mdl.Catalogprofile = dr[11].ToString();
                    Mdl.Singleinst = dr[12].ToString();
                    Mdl.Manufacturer = dr[13].ToString();
                    Mdl.ManufCon = dr[14].ToString();
                    Mdl.Modelno = dr[15].ToString();
                    Mdl.ManufSerialNo = dr[16].ToString();
                    Mdl.FunclocClass1 = dr[17].ToString();
                    Mdl.FunclocClass2 = dr[18].ToString();
                    Mdl.AuthGroup = dr[19].ToString();
                    Mdl.Comment = dr[20].ToString();

                    LstNM.Add(Mdl);
                }
            }
            if (LstNM.Count > 0)
            {


                List<Prosol_Funloc> filteredList = LstNM.GroupBy(p => new { p.FunctLocation }).Select(g => g.First()).ToList();
                if (filteredList.Count > 0)
                {
                    var fRes = new List<Prosol_Funloc>();
                    foreach (Prosol_Funloc nm in filteredList.ToList())
                    {
                        var query = Query.And(Query.EQ("FunctLocation", nm.FunctLocation));
                        var ObjStr = _FunlocRepository.FindOne(query);
                        if (ObjStr == null)
                        {
                            fRes.Add(nm);

                        }
                    }
                    cunt = _FunlocRepository.Add(fRes);

                }
                return cunt;
            }






            return cunt;

        }


        public virtual int BulkEquip(HttpPostedFileBase file)
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

            var LstNM = new List<Prosol_Funloc>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1].ToString() != "")
                {

                    var query = Query.And(Query.EQ("FunctLocation", dr[1].ToString()));
                    var ObjStr = _FunlocRepository.FindOne(query);
                    if (ObjStr != null)
                    {



                        //  var ObjStr = new Prosol_Funloc();
                        ObjStr.TechIdentNo = dr[0].ToString();
                        //    ObjStr.FunctLocation = dr[1].ToString();
                        ObjStr.EquipDesc = dr[2].ToString();
                        ObjStr.EquipCategory = dr[3].ToString();
                        //  ObjStr.Objecttype = dr[4].ToString();
                        ObjStr.Weight = dr[5].ToString();
                        ObjStr.UOM = dr[6].ToString();
                        ObjStr.Size = dr[7].ToString();
                        ObjStr.AcquisValue = dr[8].ToString();
                        ObjStr.AcquistnDat = dr[9].ToString();
                        ObjStr.Manufacturer = dr[10].ToString();
                        ObjStr.ManufCon = dr[11].ToString();
                        ObjStr.Modelno = dr[12].ToString();
                        ObjStr.ConstructYear = dr[13].ToString();
                        ObjStr.ConstructMth = dr[14].ToString();
                        ObjStr.ManufPartNo = dr[15].ToString();
                        ObjStr.ManufSerialNo = dr[16].ToString();
                        ObjStr.AuthGroup = dr[17].ToString();
                        ObjStr.Startupdate = dr[18].ToString();
                        ObjStr.MaintPlant = dr[19].ToString();
                        ObjStr.Companycode = dr[20].ToString();
                        ObjStr.Asset = dr[21].ToString();
                        ObjStr.Subno = dr[22].ToString();
                        ObjStr.ConID = dr[23].ToString();
                        ObjStr.Catalogprofile = dr[24].ToString();
                        ObjStr.Mainworkcenter = dr[25].ToString();
                        _FunlocRepository.Add(ObjStr);

                        cunt++;

                    }
                    if (ObjStr.ConID != null)
                    {
                       
                        List<Prosol_equipbom> ert = new List<Prosol_equipbom>();
                        var query1 = Query.And(Query.EQ("FunctLocation", dr[1].ToString()));
                        var ObjStr1 = _FunlocRepository.FindOne(query1);
                        var s = fun1(ObjStr.ConID);
                        if (s != null)
                        {


                            //  foreach (Prosol_equipbom EBM in s)
                            //  {
                            Prosol_equipbom eee = new Prosol_equipbom();
                            eee.FunctLocation = ObjStr1.FunctLocation;
                            eee.FunctDesc = ObjStr1.FunctDesc;
                            eee.Objecttype = ObjStr1.Objecttype;
                            eee.TechIdentNo = ObjStr.TechIdentNo;
                            eee.SupFunctLoc = ObjStr1.SupFunctLoc;
                            eee.EquipDesc = ObjStr.EquipDesc;
                            eee.Manufacturer = ObjStr.Manufacturer;
                            eee.ManufCon = ObjStr.ManufCon;
                            eee.Modelno = ObjStr.Modelno;
                            eee.ManufSerialNo = ObjStr.ManufSerialNo;
                            eee.ABCindic = ObjStr1.ABCindic;
                            eee.itemcat = "I";
                            eee.Itemcode = s.HeaderBID;
                            var ss = searching3(s.HeaderBID);
                            eee.partqnt = "1";
                            eee.Shortdesc = ss.Shortdesc;
                            eee.Longdesc = ss.Longdesc;

                            ert.Add(eee);
                            InsertData(ert);
                            //  }
                        }
                    }
                }
              
            }
            var fields = Fields.Include("TechIdentNo", "FunctLocation");
            var Result = _FunlocRepository.FindAll(fields).ToList();
            foreach (Prosol_Funloc r in Result)
            {
                if (r.TechIdentNo == null)
                {
                    var query1 = Query.EQ("FunctLocation", r.FunctLocation);

                    _FunlocRepository.Delete(query1);

                }
            }
            return cunt;

        }



        public IEnumerable<Prosol_Funloc> getFUNLOCList()
        {

            string[] str = { "TechIdentNo", "FunctLocation", "EquipDesc" };
            var fields = Fields.Include(str);
            var Result = _FunlocRepository.FindAll(fields).ToList();
            return Result;
        }
        public string genaratemcpcode(string LogicCode)
        {
            // string code = "";
            var query = Query.Matches("mcpcode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", LogicCode.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
            var rn = _mcprepository.FindAll(query).ToList();


            if (rn != null && rn.Count > 0)
            {



                //if (rn.Count > 0)
                //{
                int rr = rn.Count;
                //  int lenth = rn.Count.ToString().Length;
                if (rn.Count.ToString().Length == 1)
                {
                    if (rr != 9)
                        return LogicCode + "000" + (++rr).ToString();
                    else
                        return LogicCode + "00" + (++rr).ToString();
                }
                else if (rn.Count.ToString().Length == 2)
                {
                    if (rr != 99)
                        return LogicCode + "00" + (++rr).ToString();
                    else
                        return LogicCode + "0" + (++rr).ToString();
                }
                else if (rn.Count.ToString().Length == 3)
                {
                    if (rr != 999)
                        return LogicCode + "0" + (++rr).ToString();
                    else
                        return LogicCode + (++rr).ToString();
                }
               
                else
                {
                    return LogicCode + (++rr).ToString();
                }
            }
            else
            {
                return LogicCode + "001";
            }
        }
        public bool insetmcp(prosol_Mcp prs)
        {
            var res = _mcprepository.Add(prs);

            foreach(string strr in prs.functionlocation)
            {
                var querye = Query.EQ("FunctLocation", strr);
                var resss = _FunlocRepository.FindOne(querye);
                if (resss != null)
                {
                    resss.mcpcode = prs.mcpcode;
                    _FunlocRepository.Add(resss);
                }

            }
            return true;
        }

        public List<prosol_Mcp> getmcpwithcondition(string discipline, string drive, string equipmnt)
        {
            var querrry = Query.And(Query.EQ("decipline", discipline), Query.EQ("Drivefunction", drive), Query.EQ("equipment", equipmnt));
            var res = _mcprepository.FindAll(querrry).ToList();

            return res;


        }

        public bool CreateTasklist( Prosol_Tasklist ptl)
        {
            
                var queryyy =Query.And( Query.EQ("Equipment", ptl.Equipment.ToUpper().Trim()), Query.EQ("Model", ptl.Model.ToUpper().Trim()), Query.EQ("Make", ptl.Make.ToUpper().Trim()));
                var res = _tasklistrepository.FindOne(queryyy);

                if(res != null)
                {
                    res.Equipment = ptl.Equipment.ToUpper().Trim();
                    res.Make = ptl.Make.ToUpper().Trim();
                    res.Model = ptl.Model.ToUpper().Trim();
                    res.TLOS = ptl.TLOS;                
                var resss = _tasklistrepository.Add(res);
                return resss;
            }
            ptl.Equipment = ptl.Equipment.ToUpper().Trim();
            ptl.Make = ptl.Make.ToUpper().Trim();
            ptl.Model = ptl.Model.ToUpper().Trim();

            var ress = _tasklistrepository.Add(ptl);
            return ress;
           
        }

        public List<Tasklist_operationSequence> getmcpforMP(string eqpmnt, string model, string make)
        {

            var querryy = Query.And(Query.EQ("Equipment", eqpmnt.ToUpper().Trim()), Query.EQ("Model", model.ToUpper().Trim()), Query.EQ("Make", make.ToUpper().Trim()));
            var res = _tasklistrepository.FindOne(querryy);

            if(res != null)
            {
                foreach(Tasklist_operationSequence tlos in res.TLOS)
                {
                    var qry = Query.EQ("mcpcode", tlos.mcp1);
                    var res_mcp = _mcprepository.FindOne(qry);

                    if(res_mcp != null)
                    tlos.link = res_mcp.file;
                }
                return res.TLOS.ToList();
            }

            return null;
        }

        public List<prosol_Mcp> get_mcp_list()
        {
            return _mcprepository.FindAll().ToList();
        }


        public Prosol_Funloc getfunc_loc(string fl)
        {
            var qry = Query.EQ("FunctLocation", fl);

            var res = _FunlocRepository.FindOne(qry);

            return res;
        }

        public bool DelEquip(string id)
        {
            var qry = Query.EQ("TechIdentNo", id);
            var Result = _FunlocRepository.FindAll(qry).ToList();
            Result[0].TechIdentNo = null;
            Result[0].EquipDesc = null;
            Result[0].EquipCategory = null;
            Result[0].Weight = null;
            Result[0].UOM = null;
            Result[0].Size = null;
            Result[0].AcquisValue = null;
            Result[0].AcquistnDat = null;
            Result[0].Manufacturer = null;
            Result[0].ManufCon = null;
            Result[0].Modelno = null;
            Result[0].ConstructYear = null;
            Result[0].ConstructMth = null;
            Result[0].ManufPartNo = null;
            Result[0].ManufSerialNo = null;
            Result[0].AuthGroup = null;
            Result[0].Startupdate = null;
            Result[0].MaintPlant = null;
            Result[0].Companycode = null;
            Result[0].Asset = null;
            Result[0].Subno = null;
            Result[0].Catalogprofile = null;
            Result[0].Mainworkcenter = null;

            var result = _FunlocRepository.Add(Result[0]);
            return result;

        }
        
             public prosol_Mcp getsinglemcp(string mcpcode)
        {
            var queryyy = Query.EQ("mcpcode", mcpcode);
            return _mcprepository.FindOne(queryyy);
        }
    }
}

