using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nest;
using Elasticsearch.Net;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Net;
using System.Text;
using System.ServiceModel;
using Prosol.Core.ServiceReference2;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Mail;

namespace ProsolOnline.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearch _SearchService;
        private readonly INounModifier _NounModifiService;
        private readonly IUserCreate _UserCreateService;
        private readonly IEmailSettings _EmailRepository;
        public SearchController(ISearch searchService, INounModifier NMservice, IUserCreate UserCreateService, IEmailSettings EmailRepository)
        {
            _SearchService = searchService;
            _NounModifiService = NMservice;
            _UserCreateService = UserCreateService;
            _EmailRepository = EmailRepository;
        }
        // GET: Search
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("Search by DESC") == 1)
                return View("Searchbydesc");
            else if (CheckAccess("Search by DESC") == 0)
                return View("Denied");
            else return View("Login");

        }
        public ActionResult ProsolSearch()
        {
            return View();

        }
        [Authorize]
        public ActionResult Searchbydesc()
        {
            if (CheckAccess("Master Search") == 1)
                return View();
            else if (CheckAccess("Master Search") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult MLSearch()
        {
         
            if (CheckAccess("Master Search") == 1)
                return View();
            else if (CheckAccess("Master Search") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult MLSearchAPI()
        {

            if (CheckAccess("Master Search") == 1)
                return View();
            else if (CheckAccess("Master Search") == 0)
                return View("Denied");
            else return View("Login");

        }
        
        public int CheckAccess(string pageName)
        {
            string pages = Convert.ToString(Session["access"]);
            if (!string.IsNullOrEmpty(pages))
            {
                String[] stringArray = pages.Split(',');
                if (Array.IndexOf(stringArray, pageName) > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    return 1;
                else return 0;

            }
            else return -1;

        }

        [Authorize]
        public JsonResult GetSearchResult(string sKey, string sBy)
       {
            var lstCatalogue = new List<CatalogueModel>();
            //if (sBy == "Description")
            //{
            //    ConnectionSettings connectionSettings;
            //    ElasticClient elasticClient;
            //    StaticConnectionPool connectionPool;
            //    var nodes = new Uri[]
            //    {
            //    new Uri("http://localhost:9200"),
            //        //new Uri("Add server 2 address")   //Add cluster addresses here
            //        //new Uri("Add server 3 address")
            //    };

            //    connectionPool = new StaticConnectionPool(nodes);
            //    connectionSettings = new ConnectionSettings(connectionPool);
            //    elasticClient = new ElasticClient(connectionSettings);
            //    //  var ronse = elasticClient.Search<ESdata>(s => s.Index("datamaster").Type("Short").Query(q => q.Match(m => m.Field("longdesc").Query(sKey))));
            //    var ronse = elasticClient.Search<ESdata>(s => s.Index("datamaster").Type("Short").Query(q => q.MultiMatch(m => m.Fields(fs => fs.Field(p => p.Shortdesc).Field(p => p.Longdesc)).Operator(Operator.And).Query(sKey))).Size(10000));
            //    List<ESdata> ListesData = new List<ESdata>();
            //    //foreach (var hit in ronse.Hits)
            //    //{
            //    //    ESdata esd = new ESdata();
            //    //    esd.id = hit.Source.id;
            //    //    esd.Shortdesc = hit.Source.Shortdesc;
            //    //    esd.Longdesc = hit.Source.Longdesc;
            //    //    ListesData.Add(esd);

            //    //}
            //    foreach (var hit in ronse.Hits)
            //    {
            //        var proCat = new CatalogueModel();
            //        proCat._id = hit.Source.id;
            //        proCat.Itemcode = hit.Source.Itemcode;
            //        proCat.Shortdesc = hit.Source.Shortdesc;
            //        proCat.Longdesc = hit.Source.Longdesc;
            //        lstCatalogue.Add(proCat);
            //    }
            //    var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            //    jsonResult.MaxJsonLength = int.MaxValue;
            //    return jsonResult;
            //}
            //else
            //{
            var srchList = new List<Prosol_Datamaster>();
            if (sBy == "Part/Model/Ref No")
            {
                 srchList = _SearchService.SearchRef(sKey);
            }
            else
            {
                srchList = _SearchService.SearchDesc(sKey, sBy);
            }
          
            foreach (Prosol_Datamaster cat in srchList)
            {
                var erpinfo = _SearchService.getItemERP(cat.Itemcode);
                var proCat = new CatalogueModel();
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        lstCharateristics.Add(AttrMdl);
                    }
                }
                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        LstVendors.Add(vndMdl);
                    }
                }

                proCat._id = cat._id.ToString();
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;
                proCat.Materialcode = cat.Materialcode;
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Partno = cat.Partno;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.UOM = cat.UOM;
                proCat.Duplicates = cat.Duplicates;
                proCat.Characteristics = lstCharateristics;
                proCat.Vendorsuppliers = LstVendors;


                //  var erp = new ERPInfoModel();
                //proCat.Plant = erpinfo.Plant;
                //proCat.ValuationClass = erpinfo.ValuationClass;
                //proCat.ValuationType = erpinfo.ValuationType_;
                //proCat.StorageLocation = erpinfo.StorageLocation;
                //proCat.StorageBin = erpinfo.StorageBin;
                //proCat.ProfitCenter = erpinfo.ProfitCenter;

                //proCat.Plant = cat.Plant;
                //proCat.Plant = cat.Plant;
                //proCat.Plant = cat.Plant;
                // proCat.Catalogue = 



                //var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

                //string tmpStr = "";
                //foreach (TargetExn ent in usrInfo.Roles)
                //{
                //    if (ent.Name == "Cataloguer") tmpStr = ent.TargetId;
                //}
                //var relUname = _UserCreateService.getimage(tmpStr);
                // string uID = tmpStr;
                // string uName = relUname.UserName;

                // var cataloguer = new UpdatedBy();
                // cataloguer.UserId = uID;
                // cataloguer.Name = uName;

                // proCat.Catalogue = cataloguer;

               
                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // }
            // return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);


        }

        [Authorize]
        public JsonResult AssetSearchResult(string sKey, string sBy)
       {
            var lstCatalogue = new List<CatAssetModel>();
            var srchList = new List<Prosol_AssetMaster>();
            //if (sBy == "Part/Model/Ref No")
            //{
            //     srchList = _SearchService.SearchRef(sKey);
            //}
            //else
            //{
            //    srchList = _SearchService.AssetSearchDesc(sKey, sBy);
            //}
            srchList = _SearchService.AssetSearchDesc(sKey, sBy);
            foreach (Prosol_AssetMaster cat in srchList)
            {
                var attrinfo = _SearchService.getAssetAttributes(cat.UniqueId);
                var proCat = new CatAssetModel();
                var lstCharateristics = new List<Asset_AttributeList>();
                foreach (Asset_AttributeList pattri in attrinfo.Characterisitics)
                {
                    var AttrMdl = new Asset_AttributeList();
                    AttrMdl.Characteristic = pattri.Characteristic;
                    AttrMdl.Value = pattri.Value;
                    AttrMdl.UOM = pattri.UOM;
                    AttrMdl.Squence = pattri.Squence;
                    AttrMdl.ShortSquence = pattri.ShortSquence;
                    AttrMdl.Source = pattri.Source;
                    AttrMdl.SourceUrl = pattri.SourceUrl;
                    lstCharateristics.Add(AttrMdl);
                }


                proCat._id = cat._id.ToString();
                proCat.Description = cat.Description;
                proCat.Description_ = cat.Description_;
                proCat.AssetNo = cat.AssetNo;
                proCat.UniqueId = cat.UniqueId;
                proCat.Equipment_Short = cat.Equipment_Short;
                proCat.Equipment_Long = cat.Equipment_Long;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.ModelNo = cat.ModelNo;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.ModelYear = cat.ModelYear;
                proCat.SerialNo = cat.SerialNo;
                proCat.ItemStatus = cat.ItemStatus;
                proCat.Characteristics = lstCharateristics;


                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [Authorize]
        public JsonResult GetSearcherpResult(string sKey, string sBy)
        {
            var lstCatalogue = new List<ERPInfoModel>();
          
            var srchList = new List<Prosol_Datamaster>();
            if (sBy == "Part/Model/Ref No")
            {
                srchList = _SearchService.SearchRef(sKey);
            }
            else
            {
                srchList = _SearchService.SearchDesc(sKey, sBy);
            }

            foreach (Prosol_Datamaster cat in srchList)
            {
                var erpinfo = _SearchService.getItemERP(cat.Itemcode);
                var erp = new ERPInfoModel();
                if (erpinfo != null)
                {
                    erp.Itemcode = erpinfo.Itemcode;
                    erp.Materialtype_ = erpinfo.Materialtype_;
                    erp.BaseUOP_ = erpinfo.BaseUOP_;
                    erp.Unit_issue_ = erpinfo.Unit_issue_;
                    erp.AlternateUOM_ = erpinfo.AlternateUOM_;
                    erp.Oldmaterialno_ = erpinfo.Oldmaterialno_;
                    erp.Division_ = erpinfo.Division_;
                    erp.Plant = erpinfo.Plant;
                    erp.StorageLocation = erpinfo.StorageLocation;
                    erp.StorageBin = erpinfo.StorageBin;
                    erp.PriceControl = erpinfo.PriceControl;
                    erp.MRPType = erpinfo.MRPType;
                    erp.LOTSize = erpinfo.LOTSize;
                    erp.ProcurementType = erpinfo.ProcurementType;
                    erp.MaxStockLevel_ = erpinfo.MaxStockLevel_;
                    erp.MinStockLevel_ = erpinfo.MinStockLevel_;
                    erp.MaterialStrategicGroup = erpinfo.MaterialStrategicGroup;
                    erp.PurchasingGroup = erpinfo.PurchasingGroup;
                    erp.Quantity_ = erpinfo.Quantity_;
                    erp.PurchaseOrderText = erpinfo.PurchaseOrderText;
                    erp.PlannedDeliveryTime = erpinfo.PlannedDeliveryTime;
                    erp.DeliveringPlant_ = erpinfo.DeliveringPlant_;
                    erp.OrderUnit_ = erpinfo.OrderUnit_;



                    lstCatalogue.Add(erp);
                }
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
          

        }
        [Authorize]
        public JsonResult Getitemdetails(string itemcode)
        {
           

            
                var erpinfo = _SearchService.getItemERP(itemcode);
                var erp = new ERPInfoModel();

                erp.Itemcode = erpinfo.Itemcode;
                erp.Materialtype_ = erpinfo.Materialtype_;
                erp.BaseUOP_ = erpinfo.BaseUOP_;
                erp.Unit_issue_ = erpinfo.Unit_issue_;
                erp.AlternateUOM_ = erpinfo.AlternateUOM_;
                erp.Oldmaterialno_ = erpinfo.Oldmaterialno_;
                erp.Division_ = erpinfo.Division_;
                erp.Plant = erpinfo.Plant;
                erp.StorageLocation = erpinfo.StorageLocation;
                erp.StorageBin = erpinfo.StorageBin;
                erp.PriceControl = erpinfo.PriceControl;
                erp.MRPType = erpinfo.MRPType;
                erp.LOTSize = erpinfo.LOTSize;
                erp.ProcurementType = erpinfo.ProcurementType;
                erp.MaxStockLevel_ = erpinfo.MaxStockLevel_;
                erp.MinStockLevel_ = erpinfo.MinStockLevel_;
                erp.MaterialStrategicGroup = erpinfo.MaterialStrategicGroup;
                erp.PurchasingGroup = erpinfo.PurchasingGroup;
                erp.Quantity_ = erpinfo.Quantity_;
                erp.PurchaseOrderText = erpinfo.PurchaseOrderText;
                erp.PlannedDeliveryTime = erpinfo.PlannedDeliveryTime;
                erp.DeliveringPlant_ = erpinfo.DeliveringPlant_;
                erp.OrderUnit_ = erpinfo.OrderUnit_;
            erp.Materialtype = erpinfo.Materialtype;
            var jsonResult = Json(erp, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
        public JsonResult SendMail(string from, string subject, string msgbox,string To,string mailtable)
        {
            try
            {
                string body = mailtable;
                 body += msgbox ; 
                    
                    var val = _EmailRepository.sendmail(To, subject, body);
                var jsonResult = Json(val, JsonRequestBehavior.AllowGet);
              
                return jsonResult;
            }
           catch (Exception e)
            {
               
                var jsonResult = Json(false, JsonRequestBehavior.AllowGet);

                return jsonResult;
            }


        }
        [Authorize]        public JsonResult UploadMailAttach()        {            int res = 0;            var file = Request.Files.Count > 0 ? Request.Files[0] : null;            if (file != null && file.ContentLength > 0)            {                string folderPath = Server.MapPath("~/Attachment/SearchMailAttach/");                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();                if (filePaths.Contains(file.FileName))                {                    res = 2;                }                  if (res != 2)                    {                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/SearchMailAttach/"), file.FileName));                        res = 1;                    }                            }            return this.Json(res, JsonRequestBehavior.AllowGet);        }
        [Authorize]
        public JsonResult DeleteAttachment(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/SearchMailAttach/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult attachfiles()
        {
            string folderPath = Server.MapPath("~/Attachment/SearchMailAttach/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public class Apidata
        {
            public string status { get; set; }
            public List<data> data { get; set; }
            public string result { get; set; }
        }
        public class data
        {
            public string Itemcode { get; set; }
            public string Shortdesc { get; set; }
            public string Longdesc { get; set; }
            public string vector_score { get; set; }
            public string short_score { get; set; }
            public string slab_score { get; set; }
        }
        public class Apidata1
        {
            public string status { get; set; }
            public List<data1> data { get; set; }
            public string result { get; set; }
        }
        public class data1
        {
            public string Input_Term { get; set; }
            public string Matched_Term { get; set; }
            public string Type { get; set; }
            public string Freq { get; set; }
        }
        [Authorize]
        public JsonResult GetAPIResult(string sKey, string sBy)
        {
            var lstCatalogue = new List<CatalogueModel>();
            
            var request = (HttpWebRequest)WebRequest.Create("https://kanaiyazhi.com/API/search_API/api_attributesearch.php");
            var postData = "key=" + Uri.EscapeDataString(sKey);
            postData += "&type=" + Uri.EscapeDataString(sBy.ToLower());
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if(sBy == "Search")
            {
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Apidata>>(responseString);
                foreach (Apidata cat in item)
                {
                    foreach (data a in cat.data)
                    {
                        var proCat = new CatalogueModel();
                        proCat.Itemcode = a.Itemcode;
                        proCat.Shortdesc = a.Shortdesc;
                        proCat.Longdesc = a.Longdesc;
                        proCat.Maincode = a.vector_score;
                        proCat.Subcode = a.short_score;
                        proCat.Subsubcode = a.slab_score;
                        lstCatalogue.Add(proCat);
                    }
                }
            }
            else
            {
               var item1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Apidata1>>(responseString);
                foreach (Apidata1 cat in item1)
                {
                    foreach (data1 a in cat.data)
                    {
                        var proCat = new CatalogueModel(); 
                        proCat.Longdesc = a.Input_Term;
                        proCat.Maincode = a.Matched_Term;
                        proCat.Subcode = a.Type;
                        proCat.Subsubcode = a.Freq;
                        lstCatalogue.Add(proCat);
                    }
                }
            }
          
            
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // }
            // return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSearch(string sKey)
        {

           // sKey = sKey.ToUpper();
            var lstCatalogue = new List<CatalogueModel>();


            var srchList = _SearchService.SearchDesc(sKey, "Description");

            var s = sKey.Split(new string[] { " ", ",", ":", "*" }, StringSplitOptions.RemoveEmptyEntries);



            foreach (Prosol_Datamaster cat in srchList)
            {
                int flg = 0;
                var proCat = new CatalogueModel();
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        lstCharateristics.Add(AttrMdl);
                    }
                }
                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        if (vndMdl.RefNo == sKey)
                        {
                            flg = 1;
                        }
                        vndMdl.Refflag = vndrs.Refflag;
                        vndMdl.l = vndrs.l;
                        vndMdl.s = vndrs.s;
                        LstVendors.Add(vndMdl);
                    }
                }

                proCat._id = cat._id.ToString();
                proCat.Legacy = cat.Legacy;
                proCat.Legacy2 = cat.Legacy2;
                proCat.Materialcode = cat.Materialcode;
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Partno = cat.Partno;
                proCat.Manufacturer = cat.Manufacturer;
               // proCat.Duplicates = cat.Duplicates;
                proCat.UOM = cat.UOM;
                proCat.Characteristics = lstCharateristics;
                proCat.Vendorsuppliers = LstVendors;
                if (flg == 0)
                {

                    var res1 = cat.Longdesc.Split(new string[] { " ", ",", ":" }, StringSplitOptions.RemoveEmptyEntries);

                    var res = (from char l in cat.Longdesc
                               select new
                               {
                                   count = cat.Longdesc.Split(new char[] { ' ', ',', ':' }).Sum(p => s.Contains(p) ? 1 : 0)
                               }).OrderByDescending(p => p.count).First();

                    int p1 = res.count;
                    int p2 = res1.Length;
                    var p3 = ((p1 * 100) / p2);
                    proCat.Percentage = p3;
                    proCat.width = p3 + "%";
                }
                else
                {
                    proCat.Percentage = 100;
                    proCat.width = 100 + "%";
                }
                lstCatalogue.Add(proCat);

            }
            // List<CatalogueModel> SortedList = lstCatalogue.OrderByDescending(o => o.Percentage).ToList();
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        //public JsonResult GetSearchResult(string sKey, string sBy)
        //{
        //    var lstCatalogue = new List<CatalogueModel>();
        //    //if (sBy == "Description")
        //    //{
        //    //    ConnectionSettings connectionSettings;
        //    //    ElasticClient elasticClient;
        //    //    StaticConnectionPool connectionPool;
        //    //    var nodes = new Uri[]
        //    //    {
        //    //    new Uri("http://localhost:9200"),
        //    //        //new Uri("Add server 2 address")   //Add cluster addresses here
        //    //        //new Uri("Add server 3 address")
        //    //    };

        //    //    connectionPool = new StaticConnectionPool(nodes);
        //    //    connectionSettings = new ConnectionSettings(connectionPool);
        //    //    elasticClient = new ElasticClient(connectionSettings);
        //    //    //  var ronse = elasticClient.Search<ESdata>(s => s.Index("datamaster").Type("Short").Query(q => q.Match(m => m.Field("longdesc").Query(sKey))));
        //    //    var ronse = elasticClient.Search<ESdata>(s => s.Index("datamaster").Type("Short").Query(q => q.MultiMatch(m => m.Fields(fs => fs.Field(p => p.Shortdesc).Field(p => p.Longdesc)).Operator(Operator.And).Query(sKey))).Size(10000));
        //    //    List<ESdata> ListesData = new List<ESdata>();
        //    //    //foreach (var hit in ronse.Hits)
        //    //    //{
        //    //    //    ESdata esd = new ESdata();
        //    //    //    esd.id = hit.Source.id;
        //    //    //    esd.Shortdesc = hit.Source.Shortdesc;
        //    //    //    esd.Longdesc = hit.Source.Longdesc;
        //    //    //    ListesData.Add(esd);

        //    //    //}
        //    //    foreach (var hit in ronse.Hits)
        //    //    {
        //    //        var proCat = new CatalogueModel();
        //    //        proCat._id = hit.Source.id;
        //    //        proCat.Itemcode = hit.Source.Itemcode;
        //    //        proCat.Shortdesc = hit.Source.Shortdesc;
        //    //        proCat.Longdesc = hit.Source.Longdesc;
        //    //        lstCatalogue.Add(proCat);
        //    //    }
        //    //    var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
        //    //    jsonResult.MaxJsonLength = int.MaxValue;
        //    //    return jsonResult;
        //    //}
        //    //else
        //    //{

        //    var srchList = _SearchService.SearchDesc(sKey, sBy);
        //    var proCat = new CatalogueModel();
        //    foreach (Prosol_Datamaster cat in srchList)
        //    {
        //        var lstCharateristics = new List<AttributeList>();
        //        if (cat.Characteristics != null)
        //        {
        //            foreach (Prosol_AttributeList pattri in cat.Characteristics)
        //            {
        //                var AttrMdl = new AttributeList();
        //                AttrMdl.Characteristic = pattri.Characteristic;
        //                AttrMdl.Value = pattri.Value;
        //                AttrMdl.UOM = pattri.UOM;
        //                AttrMdl.Squence = pattri.Squence;
        //                AttrMdl.ShortSquence = pattri.ShortSquence;
        //                lstCharateristics.Add(AttrMdl);
        //            }
        //        }
        //        var Equi_mdl = new Equipment();
        //        if (cat.Equipment != null)
        //        {
        //            Equi_mdl.Name = cat.Equipment.Name;
        //            Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
        //            Equi_mdl.Modelno = cat.Equipment.Modelno;
        //            Equi_mdl.Tagno = cat.Equipment.Tagno;
        //            Equi_mdl.Serialno = cat.Equipment.Serialno;
        //            Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

        //            proCat.Equipment = Equi_mdl;
        //        }

        //        var LstVendors = new List<Vendorsupplier>();
        //        if (cat.Vendorsuppliers != null)
        //        {
        //            foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
        //            {
        //                var vndMdl = new Vendorsupplier();
        //                vndMdl.slno = vndrs.slno;
        //                vndMdl.Code = vndrs.Code;
        //                vndMdl.Name = vndrs.Name;
        //                vndMdl.Type = vndrs.Type;
        //                vndMdl.RefNo = vndrs.RefNo;
        //                vndMdl.Refflag = vndrs.Refflag;
        //                LstVendors.Add(vndMdl);
        //            }
        //        }

        //        proCat._id = cat._id.ToString();
        //        proCat.Materialcode = cat.Materialcode;
        //        proCat.Itemcode = cat.Itemcode;
        //        proCat.Shortdesc = cat.Shortdesc;
        //        proCat.Longdesc = cat.Longdesc;
        //        proCat.Noun = cat.Noun;
        //        proCat.Modifier = cat.Modifier;
        //        proCat.Partno = cat.Partno;
        //        proCat.Manufacturer = cat.Manufacturer;
        //        proCat.UOM = cat.UOM;
        //        proCat.Characteristics = lstCharateristics;
        //        proCat.Vendorsuppliers = LstVendors;
        //        lstCatalogue.Add(proCat);
        //    }
        //    var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //    // }
        //    // return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        //}
        [Authorize]
        public JsonResult GetItemDetail(string Itmcode)
        {

            var cat = _SearchService.getItemInfo(Itmcode);
            var proCat = new CatalogueModel();
            if (cat != null)
            {
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        lstCharateristics.Add(AttrMdl);
                    }
                }

                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        LstVendors.Add(vndMdl);
                    }
                }

                proCat.Vendorsuppliers = LstVendors;

                proCat.Itemcode = cat.Itemcode;
                proCat.Legacy = cat.Legacy;
                proCat.Materialcode = cat.Materialcode;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.UOM = cat.UOM;
                proCat.Additionalinfo = cat.Additionalinfo;
                proCat.Soureurl = cat.Soureurl;
                proCat.Manufacturercode = cat.Manufacturercode;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;
               
                proCat.Application = cat.Application;
                proCat.Drawingno = cat.Drawingno;
                proCat.Referenceno = cat.Referenceno;
                proCat.Unspsc = cat.Unspsc;
                proCat.Characteristics = lstCharateristics;



            }
            return this.Json(proCat, JsonRequestBehavior.AllowGet);
        }

  
        [Authorize]
        public JsonResult GetItemERP(string Itmcode)
        {
            var mdl = _SearchService.getItemERP(Itmcode);

            var erpMdl = new Prosol_ERPInfo();
            if (mdl != null)
            {
                //General
                //erpMdl.Industrysector = mdl.Industrysector;
                //erpMdl.Materialtype = mdl.Materialtype;
                //erpMdl.BaseUOP = mdl.BaseUOP;
                //erpMdl.Unit_issue = mdl.Unit_issue;
                //erpMdl.AlternateUOM = mdl.AlternateUOM;
                //erpMdl.Inspectiontype = mdl.Inspectiontype;
                //erpMdl.Inspectioncode = mdl.Inspectioncode;
                //erpMdl.Division = mdl.Division;
                //erpMdl.Salesunit = mdl.Salesunit;


                erpMdl.Industrysector_ = mdl.Industrysector_;
                erpMdl.Materialtype_ = mdl.Materialtype_;
                erpMdl.BaseUOP_ = mdl.BaseUOP_;
                erpMdl.Unit_issue_ = mdl.Unit_issue_;
                erpMdl.AlternateUOM_ = mdl.AlternateUOM_;
                erpMdl.Inspectiontype_ = mdl.Inspectiontype_;
                erpMdl.Inspectioncode_ = mdl.Inspectioncode_;
                erpMdl.Division_ = mdl.Division_;
                erpMdl.Salesunit_ = mdl.Salesunit_;

                erpMdl.Numerator_ = mdl.Numerator_;
                erpMdl.Denominator_ = mdl.Denominator_;
                erpMdl.Oldmaterialno_ = mdl.Oldmaterialno_;

                //Plant

                //erpMdl.Plant = mdl.Plant;
                //erpMdl.ProfitCenter = mdl.ProfitCenter;
                //erpMdl.StorageLocation = mdl.StorageLocation;
                //erpMdl.StorageBin = mdl.StorageBin;
                //erpMdl.ValuationClass = mdl.ValuationClass;
                //erpMdl.PriceControl = mdl.PriceControl;
                //erpMdl.ValuationCategory = mdl.ValuationCategory;
                //erpMdl.VarianceKey = mdl.VarianceKey;

                erpMdl.Plant_ = mdl.Plant_;
                erpMdl.ProfitCenter_ = mdl.ProfitCenter_;
                erpMdl.StorageLocation_ = mdl.StorageLocation_;
                erpMdl.StorageBin_ = mdl.StorageBin_;
                erpMdl.ValuationClass_ = mdl.ValuationClass_;
                erpMdl.PriceControl_ = mdl.PriceControl_;
                erpMdl.ValuationCategory_ = mdl.ValuationCategory_;
                erpMdl.VarianceKey_ = mdl.VarianceKey_;

                erpMdl.ValuationType_ = mdl.ValuationType_;
                erpMdl.StandardPrice_ = mdl.StandardPrice_;
                erpMdl.MovingAvgprice_ = mdl.MovingAvgprice_;

                erpMdl.BatchManagement = mdl.BatchManagement;
                erpMdl.SnProfile = mdl.SnProfile;
                //erpMdl.ValuationCategory_ = mdl.ValuationCategory_;
                //erpMdl.VarianceKey_ = mdl.VarianceKey_;

                //erpMdl.ValuationType_ = mdl.ValuationType_;
                //erpMdl.StandardPrice_ = mdl.StandardPrice_;
                //erpMdl.MovingAvgprice_ = mdl.MovingAvgprice_;

                //Mrp data

                //erpMdl.MRPType = mdl.MRPType;
                //erpMdl.MRPController = mdl.MRPController;
                //erpMdl.LOTSize = mdl.LOTSize;
                //erpMdl.ProcurementType = mdl.ProcurementType;
                //erpMdl.PlanningStrgyGrp = mdl.PlanningStrgyGrp;
                //erpMdl.AvailCheck = mdl.AvailCheck;
                //erpMdl.ScheduleMargin = mdl.ScheduleMargin;


                erpMdl.MRPType_ = mdl.MRPType_;
                erpMdl.MRPController_ = mdl.MRPController_;
                erpMdl.LOTSize_ = mdl.LOTSize_;
                erpMdl.ProcurementType_ = mdl.ProcurementType_;
                erpMdl.PlanningStrgyGrp_ = mdl.PlanningStrgyGrp_;
                erpMdl.PlannedDeliveryTime_ = mdl.PlannedDeliveryTime_;
                erpMdl.AvailCheck_ = mdl.AvailCheck_;
                erpMdl.ScheduleMargin_ = mdl.ScheduleMargin_;
                erpMdl.Quantity_ = mdl.Quantity_;
                erpMdl.SafetyStock_ = mdl.SafetyStock_;
                erpMdl.ReOrderPoint_ = mdl.ReOrderPoint_;
                erpMdl.MaxStockLevel_ = mdl.MaxStockLevel_;
                erpMdl.MinStockLevel_ = mdl.MinStockLevel_;

                // Sales & Others
                //erpMdl.AccAsignmtCategory = mdl.AccAsignmtCategory;
                //erpMdl.TaxClassificationGroup = mdl.TaxClassificationGroup;
                //erpMdl.ItemCategoryGroup = mdl.ItemCategoryGroup;
                //erpMdl.SalesOrganization = mdl.SalesOrganization;
                //erpMdl.DistributionChannel = mdl.DistributionChannel;
                //erpMdl.MaterialStrategicGroup = mdl.MaterialStrategicGroup;
                //erpMdl.PurchasingGroup = mdl.PurchasingGroup;
                //erpMdl.PurchasingValueKey = mdl.PurchasingValueKey;

                erpMdl.AccAsignmtCategory_ = mdl.AccAsignmtCategory_;
                erpMdl.TaxClassificationGroup_ = mdl.TaxClassificationGroup_;
                erpMdl.ItemCategoryGroup_ = mdl.ItemCategoryGroup_;
                erpMdl.SalesOrganization_ = mdl.SalesOrganization_;
                erpMdl.DistributionChannel_ = mdl.DistributionChannel_;
                erpMdl.MaterialStrategicGroup_ = mdl.MaterialStrategicGroup_;
                erpMdl.PurchasingGroup_ = mdl.PurchasingGroup_;
                erpMdl.PurchasingValueKey_ = mdl.PurchasingValueKey_;


                erpMdl.TaxClassificationType= mdl.TaxClassificationType;

                erpMdl.TaxClassificationDep = mdl.TaxClassificationDep;
                erpMdl.TaxClassificationGroup1 = mdl.TaxClassificationGroup1;
                erpMdl.TaxClassificationType1 = mdl.TaxClassificationType1;
                erpMdl.TaxClassificationDep1 = mdl.TaxClassificationDep1;
                erpMdl.DeliveringPlant_ = mdl.DeliveringPlant_;
                erpMdl.TransportationGroup_ = mdl.TransportationGroup_;
                erpMdl.LoadingGroup_ = mdl.LoadingGroup_;
                erpMdl.SalesText_= mdl.SalesText_;
                erpMdl.OrderUnit_ = mdl.OrderUnit_;
                erpMdl.AutomaticPO_ = mdl.AutomaticPO_;
                


                erpMdl.GoodsReceptprocessingTime_ = mdl.GoodsReceptprocessingTime_;
                erpMdl.Quantity_ = mdl.Quantity_;
            }
            return this.Json(erpMdl, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetImage(string Noun, string Modifier)
        {
            var arrStr = _NounModifiService.GetNounModifier(Noun, Modifier);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }


        [Authorize]
        public JsonResult Attri()
        {
            var cat1 = Request.Form["cat"];
            var cat = JsonConvert.DeserializeObject<CatalogueModel>(cat1);
            var Charas = Request.Form["attri"];
            var ListCharas = JsonConvert.DeserializeObject<List<AttributeList>>(Charas);
            var proCat = new Prosol_Datamaster();
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            List<Prosol_AttributeList> lst = new List<Prosol_AttributeList>();
         
            foreach (AttributeList LstAtt in ListCharas)

            {
                var Attr = new Prosol_AttributeList();
                if (LstAtt.Value != null && LstAtt.Value != "")
                {
                    Attr.Characteristic = LstAtt.Characteristic;
                    Attr.Value = LstAtt.Value;
                    lst.Add(Attr);
                }

            }
            if(lst.Count > 0)
            {
                var arrStr = _SearchService.getattri(proCat, lst);
                return this.Json(arrStr, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [Authorize]
        public JsonResult Codalogic()
        {
            var cat1 = Request.Form["cat"];
            var cat = JsonConvert.DeserializeObject<CatalogueModel>(cat1);
            var arrStr = _SearchService.GetCodalogic(cat.Subsubcode);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Segment()
        {

            var List = _SearchService.GetSegment();
            var lst = new List<UNSPSCModel>();
            foreach (Prosol_UNSPSC mdl in List)
            {
                var grp = new UNSPSCModel();

                grp.Segment = mdl.Segment;
                grp.SegmentTitle = mdl.SegmentTitle + " ( " + mdl.Segment + " )";
                lst.Add(grp);
            }
            var distinctList = lst.DistinctBy(x => x.Segment).ToList();
            return this.Json(distinctList, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Family(string Segment)
        {

            var List = _SearchService.GetFamily(Segment);
            var lst = new List<UNSPSCModel>();
            foreach (Prosol_UNSPSC mdl in List)
            {
                var grp = new UNSPSCModel();

                grp.Family = mdl.Family;
                grp.FamilyTitle = mdl.FamilyTitle + " ( " + mdl.Family + " )";
                lst.Add(grp);
            }
            var distinctList = lst.DistinctBy(x => x.Family).ToList();
            return this.Json(distinctList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Class(string Family)
        {

            var List = _SearchService.GetClass(Family);
            var lst = new List<UNSPSCModel>();
            foreach (Prosol_UNSPSC mdl in List)
            {
                var grp = new UNSPSCModel();

                grp.Class = mdl.Class;
                grp.ClassTitle = mdl.ClassTitle + " ( " + mdl.Class + " )";
                lst.Add(grp);
            }
            var distinctList = lst.DistinctBy(x => x.Class).ToList();
            return this.Json(distinctList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Commodity(string Class)
        {

            var List = _SearchService.GetCommodity(Class);
            var lst = new List<UNSPSCModel>();
            foreach (Prosol_UNSPSC mdl in List)
            {
                var grp = new UNSPSCModel();

                grp.Commodity = mdl.Commodity;
                grp.CommodityTitle = mdl.CommodityTitle + " ( " + mdl.Commodity + " )";
                lst.Add(grp);
            }
            var distinctList = lst.DistinctBy(x => x.Family).ToList();
            return this.Json(distinctList, JsonRequestBehavior.AllowGet);

        }





        public JsonResult Unspsclogic()
        {
            var U = Request.Form["U"];
            var lstCatalogue = new List<CatalogueModel>();
            var cat1 = JsonConvert.DeserializeObject<Prosol_UNSPSC>(U);
            var List = _SearchService.GetUnspsclogic(cat1.Commodity);
            foreach (Prosol_Datamaster cat in List)
            {
                var proCat = new CatalogueModel();
                proCat._id = cat._id.ToString();
                proCat.Materialcode = cat.Materialcode;
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                lstCatalogue.Add(proCat);
            }
            // var distinctList = lst.DistinctBy(x => x.Family).ToList();
            return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }



        public JsonResult GetEquip(string EName)
        {
            var LstE = _SearchService.GetEquip(EName);

            var attachMdlList = new List<Vendorsuppliers>();
            foreach (Prosol_Datamaster atm in LstE)
            {
                var lMdl = new Vendorsuppliers();
                if (atm.Vendorsuppliers != null)
                {
                    foreach(Vendorsuppliers v in atm.Vendorsuppliers)
                    if (v.Name != null || v.Name != " ")
                    {
                        lMdl.Name = v.Name;
                        attachMdlList.Add(lMdl);
                    }


                }



            }

            var distinctList = attachMdlList
                        .Select(m => new { m.Name })
                        .Distinct()
                        .ToList();


            return this.Json(distinctList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchMan(string Name)
        {
            // var key = Request.Form["key"];
            // var e = JsonConvert.DeserializeObject<Equipment>(Name);
            var lstCatalogue = new List<CatalogueModel>();
            var List = _SearchService.Getmanu(Name);
            foreach (Prosol_Datamaster cat in List)
            {
                var proCat = new CatalogueModel();
                proCat._id = cat._id.ToString();
                proCat.Materialcode = cat.Materialcode;
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                lstCatalogue.Add(proCat);
            }
            // var distinctList = lst.DistinctBy(x => x.Family).ToList();
            return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }



        public JsonResult SearchRef(string Name)
        {
            // var key = Request.Form["key"];
            // var e = JsonConvert.DeserializeObject<Equipment>(Name);
            var lstCatalogue = new List<CatalogueModel>();
            var List = _SearchService.SearchRef(Name);
            foreach (Prosol_Datamaster cat in List)
            {
                var proCat = new CatalogueModel();
                proCat._id = cat._id.ToString();
                proCat.Materialcode = cat.Materialcode;
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                lstCatalogue.Add(proCat);
            }
            // var distinctList = lst.DistinctBy(x => x.Family).ToList();
            return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }


        // Elastic Search

        public JsonResult runElasticSearch()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;

            //Connection string for Elasticsearch
            /*connectionSettings = new ConnectionSettings(new Uri("http://localhost Jump :9200/")); //local PC
            elasticClient = new ElasticClient(connectionSettings);*/

            //Multiple node for fail over (cluster addresses)
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200"),
                //new Uri("Add server 2 address")   //Add cluster addresses here
                //new Uri("Add server 3 address")
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);
            elasticClient = new ElasticClient(connectionSettings);
            int cnt = 0;
            var datalst = _SearchService.getDataforES();
            List<ESdata> esData = new List<ESdata>();
            foreach (Prosol_Datamaster mdl in datalst)
            {
                ESdata esd = new ESdata();

                esd.id = mdl._id.ToString();
                esd.Itemcode = mdl.Itemcode;
                esd.Shortdesc = mdl.Shortdesc;
                esd.Longdesc = mdl.Longdesc;
                esData.Add(esd);

            }
            // Update by Partial Document
            foreach (ESdata mdl in esData)
            {
                var response = elasticClient.Update<ESdata>(mdl.id, d => d.Index("datamaster").Type("Short")
                .Doc(new ESdata
                {
                    Itemcode = mdl.Itemcode,
                    Shortdesc = mdl.Shortdesc,
                    Longdesc = mdl.Longdesc
                }));

                if (!response.IsValid)
                {
                    var res = elasticClient.Index(mdl, i => i.Index("datamaster").Type("Short").Id(mdl.id));
                    if (res.IsValid)
                    {
                        cnt++;
                    }

                }
                else cnt++;
            }
            return this.Json(cnt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkCataloguer()
        {
            var resultOfCat = _SearchService.CataloguerCheck(Session["username"].ToString());
            return this.Json(resultOfCat, JsonRequestBehavior.AllowGet);
        }
        //mapduplicate

        public JsonResult Mapdulpicateitem(string Itemcode)
        {
            var dataList = _SearchService.Mapduplicate(Itemcode).ToList();

            return this.Json(dataList, JsonRequestBehavior.AllowGet);
        }
        //unmap
        public JsonResult unmapcode(string Itemcode)
        {
            var dataList = _SearchService.unmapcode(Itemcode);

            return this.Json(dataList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult savemapduplicate(string new_code, string existing_code)
        {
            var res = _SearchService.savemapduplicate(new_code, existing_code);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult savemapduplicate1(string new_code, string existing_code)


        {

            string userid = Session["userid"].ToString();
            string username = Session["username"].ToString();
            var res = _SearchService.savemapduplicate(new_code, existing_code, userid, username);
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Getitem(string Noun, string Modifier)
        {
            var lstCatalogue = new List<CatalogueModel>();

            var srchList = _SearchService.Get(Noun, Modifier);
            foreach (Prosol_Datamaster cat in srchList)
            {
                var proCat = new CatalogueModel();
                var lstCharateristics = new List<AttributeList>();
                if (cat.Characteristics != null)
                {
                    foreach (Prosol_AttributeList pattri in cat.Characteristics)
                    {
                        var AttrMdl = new AttributeList();
                        AttrMdl.Characteristic = pattri.Characteristic;
                        AttrMdl.Value = pattri.Value;
                        AttrMdl.UOM = pattri.UOM;
                        AttrMdl.Squence = pattri.Squence;
                        AttrMdl.ShortSquence = pattri.ShortSquence;
                        lstCharateristics.Add(AttrMdl);
                    }
                }
                var Equi_mdl = new Equipment();
                if (cat.Equipment != null)
                {
                    Equi_mdl.Name = cat.Equipment.Name;
                    Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                    Equi_mdl.Modelno = cat.Equipment.Modelno;
                    Equi_mdl.Tagno = cat.Equipment.Tagno;
                    Equi_mdl.Serialno = cat.Equipment.Serialno;
                    Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;

                    proCat.Equipment = Equi_mdl;
                }

                var LstVendors = new List<Vendorsupplier>();
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                    {
                        var vndMdl = new Vendorsupplier();
                        vndMdl.slno = vndrs.slno;
                        vndMdl.Code = vndrs.Code;
                        vndMdl.Name = vndrs.Name;
                        vndMdl.Type = vndrs.Type;
                        vndMdl.RefNo = vndrs.RefNo;
                        vndMdl.Refflag = vndrs.Refflag;
                        LstVendors.Add(vndMdl);
                    }
                }

                proCat._id = cat._id.ToString();
                proCat.Materialcode = cat.Materialcode;
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Partno = cat.Partno;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.UOM = cat.UOM;
                proCat.Characteristics = lstCharateristics;
                proCat.Vendorsuppliers = LstVendors;

                proCat.Percentage = 100;
                proCat.width = 100 + "%";
                lstCatalogue.Add(proCat);

            }

            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public JsonResult Pushtosap(string Matcode)
        {

            Session["sapresponse"] = "Please close this tab.";
            var queryCollection = System.Web.HttpUtility.ParseQueryString(HttpContext.Request.UrlReferrer.Query);
            //if (queryCollection.AllKeys.Contains("REF_ID") && !string.IsNullOrEmpty(queryCollection.Get("REF_ID")))
            //{
            //    string Ref = queryCollection.Get("REF_ID");
              
                var x = _SearchService.Pushtosap(Matcode, "");
                if (Matcode == "" || Matcode == null)
                {

                    return this.Json("go", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Session["sapresponse"] = x + ", Please close this tab.";
                    return this.Json("go", JsonRequestBehavior.AllowGet);
                }

           // }

            return this.Json("go", JsonRequestBehavior.AllowGet);
        }

    }
    public class ESdata
    {
        public string id { set; get; }
        public string Itemcode { set; get; }
        public string Shortdesc { set; get; }
        public string Longdesc { set; get; }
    }
}