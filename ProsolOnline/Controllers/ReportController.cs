using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Web.Mvc;
using Prosol.Core;
using ProsolOnline.Models;
using Prosol.Common;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.ViewModel;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using System.Globalization;
using System.Data;
using System.IO;
using ExcelLibrary.Office.Excel;


namespace ProsolOnline.Controllers
{
    public class ReportController : Controller
    {
        private readonly I_Report _Userreportservice;
        private readonly IUserCreate _Usercreateservice;
        public ReportController(I_Report usrreportservice, IUserCreate usrservice)
        {
            _Userreportservice = usrreportservice;
            _Usercreateservice = usrservice;
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


        // GET: Report
        [Authorize]
        public ActionResult Export()
        {
            if (CheckAccess("Export") == 1)
                return View("Export");
            else if (CheckAccess("Export") == 0)
                return View("Denied");
            else return View("Login");


        }
        [Authorize]
        public ActionResult Tracking()
        {
            if (CheckAccess("Tracking") == 1)
                return View("Tracking");
            else if (CheckAccess("Tracking") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult ERP_logs()
        {
            if (CheckAccess("Tracking") == 1)
                return View("ERP_logs");
            else if (CheckAccess("Tracking") == 0)
                return View("Denied");
            else return View("Login");

        }
        /*To load data of export*/
        [Authorize]
        [HttpPost]
        public JsonResult loadexport()
        {
            var selection = Request.Form["selection"];
            string where = Request.Form["Where"];
            string value = Request.Form["Value"];
            string fromdate = Request.Form["Fromdate"];
            string role = Request.Form["Role"];
            string status = Request.Form["Status"];
            if (value == "undefined")
                value = null;

            if (role == "undefined")
                role = null;
            if (status == "undefined")
                status = null;
            if (fromdate == "undefined")
                fromdate = null;
            string todate = Request.Form["Todate"];
            if (todate == "undefined")
                todate = null;
            String[] gtoption = { };
            List<string> tets = new List<string>();
            tets = JsonConvert.DeserializeObject<List<string>>(selection);
            tets.Add("Itemcode");
            tets.Add("Materialcode");
            gtoption = tets.ToArray();
            var Exportlist = _Userreportservice.loaddata(gtoption, where, value, fromdate, todate, role, status).ToList();
            var jsonResult = Json(Exportlist, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return Json(Exportlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult loadstatusexport()
        {
            var selection = Request.Form["selection"];
            string fromdate = Request.Form["Fromdate"];
            string todate = Request.Form["Todate"];
            if (fromdate == "undefined")
                fromdate = null;

            if (todate == "undefined")
                todate = null;
            String[] gtoption = { };
            List<string> tets = new List<string>();
            tets = JsonConvert.DeserializeObject<List<string>>(selection);
            gtoption = tets.ToArray();
            var Exportlist = _Userreportservice.loadstatusdata(gtoption, fromdate, todate).ToList();
            var jsonResult = Json(Exportlist, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return Json(Exportlist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public void StatusDownload(string selection, string Fromdate, string Todate)
        {



            String[] gtoption = { };
            List<string> tets = new List<string>();
            tets = JsonConvert.DeserializeObject<List<string>>(selection);
            gtoption = tets.ToArray();
            var Exportlist = _Userreportservice.loadstatusdata(gtoption, Fromdate, Todate).ToList();

            DataTable dt = new DataTable();

            foreach (IDictionary<string, object> row in Exportlist)
            {
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                }
                dt.Rows.Add(row.Values.ToArray());
            }


            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "tab1");
            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();




        }
        /*To load Userlist*/
        [Authorize]
        [HttpGet]
        public JsonResult getuser(string role)
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            var userlist = _Userreportservice.getuser(role, usrInfo.Plantcode).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        /*To load Codelist*/
        [Authorize]
        [HttpGet]
        public JsonResult getcode(string role)
        {
            if (role == "Cataloguer")
            {
                role = "2";
            }
            else if (role == "Reviewer")
            {
                role = "3";
            }
            else
            {
                role = "4";
            }
            var userlist = _Userreportservice.getcode(role).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void Download(string selection, string Where, string Value, string Role, string Status, string Fromdate, string Todate)
        {


            if (Status == "undefined" || Status == " ")
            {
                String[] gtoption1 = { };
                List<string> tets1 = new List<string>();
                tets1 = JsonConvert.DeserializeObject<List<string>>(selection);
                tets1.Add("Itemcode");
                tets1.Add("Materialcode");
                tets1.Add("Soureurl");
                tets1.Add("Catalogue");
                tets1.Add("Review");
                tets1.Add("Release");

                tets1.Add("Remarks");
                tets1.Add("RevRemarks");
                tets1.Add("RelRemarks");

                tets1.Add("RejectedBy");
                tets1.Add("ItemStatus");

                gtoption1 = tets1.ToArray();

                //  var res1 = _Userreportservice.loaddata2(gtoption1).ToList();
                var res1 = _Userreportservice.loaddata2(gtoption1, Fromdate, Todate).ToList();
                ////nowblock

                //var strJson1 = JsonConvert.SerializeObject(res1);
                //DataTable dt1 = (DataTable)JsonConvert.DeserializeObject(strJson1, (typeof(DataTable)));
                //Response.ClearContent();
                //Response.Buffer = true;

                //string filename = "Report";
                //string fls = filename.ToString() + ".xls";
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fls));
                //Response.ContentType = "application/ms-excel";
                //string str1 = string.Empty;
                //for (int i = 0; i < dt1.Columns.Count; i++)
                //{
                //    Response.Write(dt1.Columns[i].ToString().ToUpper() + "\t");
                //}
                //Response.Write("\n");
                //foreach (DataRow dr in dt1.Rows)
                //{
                //    str1 = "";
                //    for (int j = 0; j < dt1.Columns.Count; j++)
                //    {
                //        if (dr[j].ToString().Contains('\n'))
                //        {
                //            dr[j] = dr[j].ToString().Replace('\n', ',');
                //        }
                //        Response.Write(str1 + dr[j]);
                //        str1 = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.Flush();
                //Response.End();

                ////block end


                //foreach (DataRow dr in dt1.Rows)
                //{
                //    str1 = "";
                //    for (int j = 0; j < dt1.Columns.Count; j++)
                //    {
                //        Response.Write(str1 + dr[j]);
                //        str1 = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();


                ///////////////////tt
                DataTable dt = new DataTable();

                foreach (IDictionary<string, object> row in res1)
                {
                    foreach (KeyValuePair<string, object> entry in row)
                    {
                        if (!dt.Columns.Contains(entry.Key.ToString()))
                        {
                            dt.Columns.Add(entry.Key);
                        }
                    }
                    dt.Rows.Add(row.Values.ToArray());
                }


                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, "tab1");
                // Prepare the response
                HttpResponseBase httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Provide you file name here
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
                /////////////tt
                //       DataTable dt = (DataTable)JsonConvert.DeserializeObject(dt, (typeof(DataTable)));
                //string file = Server.MapPath("~/common/") + "Export.xls";
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "Export.xls"));
                //Response.ContentType = "application/ms-excel";

                //Workbook workbook = new Workbook();
                //Worksheet worksheet = new Worksheet("First Sheet");

                //int i = 0;

                //foreach (DataRow dr in dt.Rows)
                //{
                //    int j = 0;
                //    foreach (DataColumn dc in dt.Columns)
                //    {
                //        if (i == 0)
                //        {
                //            worksheet.Cells[i, j] = new Cell(Convert.ToString(dc));
                //            j++;
                //        }
                //        else
                //        {
                //            worksheet.Cells[i, j] = new Cell(Convert.ToString(dr[j]));
                //            j++;
                //        }
                //    }
                //    i++;
                //}

                //workbook.Worksheets.Add(worksheet);
                //workbook.Save(file);

                //Response.TransmitFile(@"/Common/Export.xls");
                //Response.End();

            }
            else
            {
                if (Value == "undefined")
                    Value = null;

                if (Role == "undefined")
                    Role = null;
                if (Status == "undefined")
                    Status = null;

                if (Fromdate == "null")
                    Fromdate = null;
                if (Todate == "null")
                    Todate = null;

                String[] gtoption = { };
                List<string> tets = new List<string>();
                tets = JsonConvert.DeserializeObject<List<string>>(selection);
                tets.Add("Itemcode");
                tets.Add("Materialcode");

                gtoption = tets.ToArray();



                var res = _Userreportservice.loaddata(gtoption, Where, Value, Fromdate, Todate, Role, Status).ToList();

                DataTable dt = new DataTable();

                foreach (IDictionary<string, object> row in res)
                {
                    foreach (KeyValuePair<string, object> entry in row)
                    {
                        if (!dt.Columns.Contains(entry.Key.ToString()))
                        {
                            dt.Columns.Add(entry.Key);
                        }
                    }
                    dt.Rows.Add(row.Values.ToArray());
                }


                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, "tab1");
                // Prepare the response
                HttpResponseBase httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Provide you file name here
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"Export.xlsx\"");

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();

                //var strJson = JsonConvert.SerializeObject(res);


                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
                //Response.ClearContent();
                //Response.Buffer = true;

                //string filename = "Report";
                //string fls = filename.ToString() + ".xls";
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fls));
                //Response.ContentType = "application/ms-excel";
                //string str = string.Empty;
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                //}
                //Response.Write("\n");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    str = "";
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        Response.Write(str + dr[j]);
                //        str = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
            }



        }
        [HttpGet]
        public void DownloadOverall(string selection, string Where, string Value, string Role, string Status, string Fromdate, string Todate)
        {
            if (Value == "undefined")
                Value = null;

            if (Role == "undefined")
                Role = null;
            if (Status == "undefined")
                Status = null;

            if (Fromdate == "null")
                Fromdate = null;
            if (Todate == "null")
                Todate = null;

            String[] gtoption = { };
            List<string> tets = new List<string>();
            tets = JsonConvert.DeserializeObject<List<string>>(selection);
            tets.Add("Itemcode");
            tets.Add("Materialcode");
            tets.Add("Catalogue");
            tets.Add("RejectedBy");
            tets.Add("ItemStatus");
            tets.Add("Remarks");

            gtoption = tets.ToArray();



            var res = _Userreportservice.loaddata1(gtoption, Where, Value, Fromdate, Todate, Role, Status).ToList();


            DataTable dt = new DataTable();

            foreach (IDictionary<string, object> row in res)
            {
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                }
                dt.Rows.Add(row.Values.ToArray());
            }


            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "tab1");
            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"Export.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        /*To Export loaded data*/
        [Authorize]
        public void exportexcel(string tables)
        {


            DataTable dt = (DataTable)JsonConvert.DeserializeObject(tables, (typeof(DataTable)));
            Response.ClearContent();
            Response.Buffer = true;
            string filename = Convert.ToString(DateTime.Now);
            string fls = filename.ToString() + ".xls";
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fls));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        /*To get userlist*/
        [Authorize]
        [HttpGet]
        public JsonResult getusertrack(string role)
        {
            if (role == "Cataloguer")
            {
                string username = Convert.ToString(Session["username"]);
                var gtuser = _Userreportservice.getuseronly(username).ToList();
                return Json(gtuser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
                var gtuser = _Userreportservice.getuser(role, usrInfo.Plantcode).ToList();
                return Json(gtuser, JsonRequestBehavior.AllowGet);
            }
        }

        /*to get plant list*/
        [Authorize]
        public JsonResult getplant()
        {
            var gtplant = _Userreportservice.getplant().ToList();
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));

            List<Prosol_Plants> lst = new List<Prosol_Plants>();
            foreach (Prosol_Plants mdl in gtplant)
            {
                if (usrInfo.Plantcode == null)
                {
                    lst.Add(mdl);
                }
                else
                {
                    foreach (string cd in usrInfo.Plantcode)
                    {
                        if (cd == mdl.Plantcode)
                        {
                            lst.Add(mdl);
                        }
                    }

                }

            }
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        /*to get department list*/
        [Authorize]
        public JsonResult getdepartment()
        {
            var gtdepartment = _Userreportservice.getdepartment().ToList();
            return Json(gtdepartment, JsonRequestBehavior.AllowGet);
        }


        /*to Load data of tracking*/
        [Authorize]
        [HttpPost]
        public JsonResult Trackload()
        {
            string plant = Request.Form["plant"];
            string fromdate = Request.Form["fromdate"];
            string todate = Request.Form["todate"];
            string option = Request.Form["option"];
            var materialcode = Request.Form["materialcode"];
            var Materialtype = Request.Form["Materialtype"];
            /////
            if (Materialtype != "" && Materialtype != null && Materialtype != "undefined" && option == "MaterialType")
            {
                var GetErpdata = _Userreportservice.Getmaterialtypedata(Materialtype).ToList();
                var Exportlist = _Userreportservice.getalldata(plant, fromdate, todate).ToList();
                var datalist = from first in Exportlist
                               join second in GetErpdata
                                           on first.Itemcode equals second.Itemcode
                               select first;
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;
                bool goo1;
                var mergelist1 = (dynamic)null;
                foreach (var cde in datalist)
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
                    if (cde.UpdatedOn != null)
                    {
                        DateTime date = DateTime.Parse(Convert.ToString(cde.UpdatedOn));
                        row.Add("UpdatedOn", date.ToString("dd/MM/yyyy"));
                    }
                    else row.Add("UpdatedOn", "");
                    row.Add("Legacy", cde.Legacy);
                    row.Add("Label Shortdesc", cde.Shortdesc);
                    row.Add("Shortdesc", cde.Shortdesc_);
                    row.Add("Longdesc", cde.Longdesc);
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
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            if (materialcode != "" && materialcode != null && materialcode != "undefined")
            {
                string[] codestr;
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Prosol_Datamaster> get_assigndata = new List<Prosol_Datamaster>();
                List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
                //List<multiplecodelist> getcode = new List<multiplecodelist>();
                List<string> code_split = new List<string>();
                codestr = materialcode.Split(',');
                foreach (string n in codestr)
                {
                    if (!code_split.Contains(n.ToString().Trim()))
                        code_split.Add(n.ToString().Trim());
                }
                foreach (string cdn in code_split)
                {
                    gd_data = _Userreportservice.trackmulticodelist(cdn).ToList();
                    get_assigndata.AddRange(gd_data);

                }

                var lst = new List<multiplecodelist>();
                foreach (Prosol_Datamaster cde in get_assigndata)
                {
                    //var obj = new multiplecodelist();
                    //obj.Itemcode = mdl.Itemcode;
                    //obj.MaterialCode = mdl.Materialcode;
                    ////obj.CreatedOn = mdl.CreatedOn;
                    //if (mdl.CreatedOn != null)
                    //{
                    //    DateTime date = DateTime.Parse(Convert.ToString(mdl.CreatedOn));

                    //    obj.CreatedOn = date.ToString("dd/MM/yyyy");
                    //}
                    ////  obj.CreatedOn = date.ToString("dd/MM/yyyy");
                    //// DateTime date = DateTime.Parse(Convert.ToString(mdl.CreatedOn));
                    //obj.Legacy = mdl.Legacy;
                    //obj.Shortdesc = mdl.Shortdesc;
                    //obj.Shortdesc_ = mdl.Shortdesc_;
                    //obj.Longdesc = mdl.Longdesc;
                    //obj.Cataloguer = (mdl.Catalogue != null) ? (mdl.Catalogue.Name) : "";
                    //obj.Reviewer = (mdl.Review != null) ? (mdl.Review.Name) : "";
                    //obj.Releaser = (mdl.Release != null) ? (mdl.Release.Name) : "";
                    //obj.Remarks = mdl.Remarks;
                    //// var temp = _Userreportservice.getplant(mdl.Itemcode).ToList();
                    ////  obj.Plant = temp[0].Plant;
                    //// obj.Plant = "";
                    //lst.Add(obj);


                    row = new Dictionary<string, object>();

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
                    row.Add("Label Shortdesc", cde.Shortdesc);
                    row.Add("Shortdesc", cde.Shortdesc_);
                    row.Add("Longdesc", cde.Longdesc);

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
                        }
                        else
                        {
                            row.Add("ATTRIBUTE NAME " + i, "");
                            row.Add("ATTRIBUTE VALUE " + i, "");
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
                            if (ven.Refflag == "ITEM MODEL")
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
                    row.Add("MODEL NO", modelno);
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
                var jsonResult = Json(rows, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
                //return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // var Exportlist = null;
                var Exportlist = _Userreportservice.Trackload(plant, fromdate, todate, option).ToList();
                // return Json(Exportlist, JsonRequestBehavior.AllowGet);
                var jsonResult = Json(Exportlist, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }
        public class multiplecodelist
        {
            public string Itemcode;
            public string MaterialCode;
            public string CreatedOn;
            public string Legacy;
            public string Shortdesc;
            public string Shortdesc_;
            public string Longdesc;
            public string Cataloguer;
            public string Reviewer;
            public string Releaser;
            public string Remarks;
            public string Plant;
        }
        [HttpGet]
        public void DownloadTrack(string plant, string Fromdate, string Todate, string option, string MaterialType)
        {
            if (option == "MaterialType")
            {
                var GetErpdata = _Userreportservice.Getmaterialtypedata(MaterialType).ToList();
                var Exportlist = _Userreportservice.getalldata(plant, Fromdate, Todate).ToList();
                var datalist = from first in Exportlist
                               join second in GetErpdata
                                           on first.Itemcode equals second.Itemcode
                               select first;
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;
                bool goo1;
                var mergelist1 = (dynamic)null;
                foreach (var cde in datalist)
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
                    if (cde.UpdatedOn != null)
                    {
                        DateTime date = DateTime.Parse(Convert.ToString(cde.UpdatedOn));
                        row.Add("UpdatedOn", date.ToString("dd/MM/yyyy"));
                    }
                    else row.Add("UpdatedOn", "");
                    row.Add("Legacy", cde.Legacy);
                    row.Add("Shortdesc", cde.Shortdesc);
                    row.Add("Longdesc", cde.Longdesc);
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
                    row.Add("Material Type", MaterialType);
                    rows.Add(row);

                }
                DataTable dt = new DataTable();

                foreach (IDictionary<string, object> rw in rows)
                {
                    foreach (KeyValuePair<string, object> entry in rw)
                    {
                        if (!dt.Columns.Contains(entry.Key.ToString()))
                        {
                            dt.Columns.Add(entry.Key);
                        }
                    }
                    dt.Rows.Add(rw.Values.ToArray());
                }

                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, "tab1");

                // Prepare the response
                HttpResponseBase httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Provide you file name here
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"TrackReport.xlsx\"");

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();

            }
            else
            {
                if (plant == "undefined")
                    plant = null;

                if (option == "undefined")
                    option = null;

                if (Fromdate == "null")
                    Fromdate = null;
                if (Todate == "null")
                    Todate = null;

                var res = _Userreportservice.Trackload(plant, Fromdate, Todate, option).ToList();

                DataTable dt = new DataTable();

                foreach (IDictionary<string, object> row in res)
                {
                    // Ensure all columns exist
                    foreach (var entry in row)
                    {
                        if (!dt.Columns.Contains(entry.Key))
                        {
                            dt.Columns.Add(entry.Key);
                        }
                    }

                    // Create a new DataRow and fill it by column name
                    DataRow dr = dt.NewRow();
                    foreach (var entry in row)
                    {
                        dr[entry.Key] = entry.Value ?? DBNull.Value;
                    }

                    dt.Rows.Add(dr);
                }

                string fileName = "TrackReport";

                if (option == "Overall")
                {
                    fileName = "Tracking_Report";
                }
                else if (option == "Pvcompleted")
                {
                    fileName = "PV_Report";
                }
                else if (option == "Delivery")
                {
                    fileName = "Delivery_Report";
                }

                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, "tab1");

                // Prepare the response
                HttpResponseBase httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Provide you file name here
                //httpResponse.AddHeader("content-disposition", "attachment;filename=\"TrackReport.xlsx\"");
                httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}.xlsx\"");

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();

            }

        }
        [HttpPost]
        public JsonResult DownloadTrack1()
        {
            var materialcode = Request.Form["materialcode"];
            Session["materialcode"] = materialcode;

            string[] codestr;
            List<Prosol_Datamaster> get_assigndata = new List<Prosol_Datamaster>();
            List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
            //List<multiplecodelist> getcode = new List<multiplecodelist>();
            List<string> code_split = new List<string>();
            codestr = materialcode.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }
            foreach (string cdn in code_split)
            {
                gd_data = _Userreportservice.trackmulticodelist(cdn).ToList();
                get_assigndata.AddRange(gd_data);

            }
            var cunt = gd_data.Count;
            return Json(cunt, JsonRequestBehavior.AllowGet);

        }
        //public void DownloadMulticode()
        //{
        //    var materialcode = Session["materialcode"].ToString();
        //    string[] codestr;
        //    List<Prosol_Datamaster> get_assigndata = new List<Prosol_Datamaster>();
        //    List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
        //    //List<multiplecodelist> getcode = new List<multiplecodelist>();
        //    List<string> code_split = new List<string>();
        //    codestr = materialcode.Split(',');
        //    foreach (string n in codestr)
        //    {
        //        if (!code_split.Contains(n.ToString().Trim()))
        //            code_split.Add(n.ToString().Trim());
        //    }
        //    foreach (string cdn in code_split)
        //    {
        //        gd_data = _Userreportservice.trackmulticodelist(cdn).ToList();
        //        get_assigndata.AddRange(gd_data);

        //    }
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    foreach (var cde in get_assigndata)
        //    {
        //        row = new Dictionary<string, object>();

        //        row.Add("Item Code", cde.Itemcode);
        //        row.Add("Material Code", cde.Materialcode);
        //        if (cde.CreatedOn != null)
        //        {
        //            DateTime date = DateTime.Parse(Convert.ToString(cde.CreatedOn));
        //            row.Add("CreatedOn", date.ToString("dd/MM/yyyy"));
        //        }
        //        else row.Add("CreatedOn", "");
        //        if (cde.UpdatedOn != null)
        //        {
        //            DateTime date = DateTime.Parse(Convert.ToString(cde.UpdatedOn));
        //            row.Add("UpdatedOn", date.ToString("dd/MM/yyyy"));
        //        }
        //        else row.Add("UpdatedOn", "");
        //        row.Add("Legacy", cde.Legacy);
        //        row.Add("Noun", cde.Noun);
        //        row.Add("Modifier", cde.Modifier);
        //        row.Add("UOM", cde.UOM);
        //        row.Add("Label Shortdesc", cde.Shortdesc);
        //        row.Add("Shortdesc", cde.Shortdesc_);
        //        row.Add("Longdesc", cde.Longdesc);

        //        for (int i = 1; i <= 35; i++)
        //        {
        //            if (cde.Characteristics != null && i <= cde.Characteristics.Count)
        //            {
        //                var attr = cde.Characteristics[i - 1];
        //                if (attr.Characteristic != null)
        //                {
        //                    row.Add("ATTRIBUTE NAME " + i, attr.Characteristic);
        //                }
        //                else
        //                {
        //                    row.Add("ATTRIBUTE NAME " + i, "");
        //                }

        //                if (attr.Value != null)
        //                {
        //                    row.Add("ATTRIBUTE VALUE " + i, attr.Value);
        //                }
        //                else
        //                {
        //                    row.Add("ATTRIBUTE VALUE " + i, "");
        //                }
        //            }
        //            else
        //            {
        //                row.Add("ATTRIBUTE NAME " + i, "");
        //                row.Add("ATTRIBUTE VALUE " + i, "");
        //            }
        //        }
        //        if (cde.Equipment != null)
        //        {
        //            if (!string.IsNullOrEmpty(cde.Equipment.Name))
        //            {
        //                row.Add("EQUIPMENT NAME", cde.Equipment.Name);
        //            }
        //            else
        //                row.Add("EQUIPMENT NAME", "");
        //            if (!string.IsNullOrEmpty(cde.Equipment.Modelno))
        //            {
        //                row.Add("EQUIPMENT MODELNO", cde.Equipment.Modelno);
        //            }
        //            else
        //                row.Add("EQUIPMENT MODELNO", "");
        //            if (!string.IsNullOrEmpty(cde.Equipment.Serialno))
        //            {
        //                row.Add("EQUIPMENT SERIALNO", cde.Equipment.Serialno);
        //            }
        //            else
        //                row.Add("EQUIPMENT SERIALNO", "");
        //            if (!string.IsNullOrEmpty(cde.Equipment.Tagno))
        //            {
        //                row.Add("EQUIPMENT TAGNO", cde.Equipment.Tagno);
        //            }
        //            else
        //                row.Add("EQUIPMENT TAGNO", "");
        //            if (!string.IsNullOrEmpty(cde.Equipment.Manufacturer))
        //            {
        //                row.Add("EQUIPMENT MANUFACTURE", cde.Equipment.Manufacturer);
        //            }
        //            else
        //                row.Add("EQUIPMENT MANUFACTURE", "");
        //            if (!string.IsNullOrEmpty(cde.Equipment.Additionalinfo))
        //            {
        //                row.Add("EQUIPMENT SPECIFICATION", cde.Equipment.Additionalinfo);
        //            }
        //            else
        //                row.Add("EQUIPMENT SPECIFICATION", "");
        //            if (!string.IsNullOrEmpty(cde.Equipment.SuperiorEquipment))
        //            {
        //                row.Add("SUPERIOR EQUIPMENT", cde.Equipment.SuperiorEquipment);
        //            }
        //            else
        //                row.Add("SUPERIOR EQUIPMENT", "");
        //        }
        //        else
        //        {
        //            row.Add("EQUIPMENT NAME", "");
        //            row.Add("EQUIPMENT MODELNO", "");
        //            row.Add("EQUIPMENT SERIALNO", "");
        //            row.Add("EQUIPMENT TAGNO", "");
        //            row.Add("EQUIPMENT MANUFACTURE", "");
        //            row.Add("EQUIPMENT SPECIFICATION", "");
        //            row.Add("SUPERIOR EQUIPMENT", "");
        //        }

        //        string modelno = "";
        //        string brand = "";
        //        string designation = "";
        //        string drwno = "";
        //        string posno = "";
        //        string serial = "";
        //        string supplier = "";
        //        string suppartno = "";
        //        var partLst = new List<string>();
        //        var mfrLst = new List<string>();
        //        if (cde.Vendorsuppliers != null)
        //        {
        //            foreach (var ven in cde.Vendorsuppliers)
        //            {
        //                if (ven.Refflag == "MODEL NUMBER")
        //                {
        //                    modelno = ven.RefNo;
        //                }
        //                if (ven.Refflag == "DESIGNATION")
        //                {
        //                    designation = ven.RefNo;
        //                }
        //                if (ven.Refflag == "BRAND NAME")
        //                {
        //                    brand = ven.Name;
        //                }
        //                if (ven.Refflag == "DRAWING NUMBER")
        //                {
        //                    drwno = ven.RefNo;
        //                }
        //                if (ven.Refflag == "POSITION NUMBER")
        //                {
        //                    posno = ven.RefNo;
        //                }
        //                if (ven.Refflag == "SERIAL NUMBER")
        //                {
        //                    serial = ven.RefNo;
        //                }
        //                if (ven.Refflag == "SUPPLIER PART NUMBER")
        //                {
        //                    suppartno = ven.RefNo;
        //                }
        //                if (ven.Type == "SUPPLIER")
        //                {
        //                    supplier = ven.Name;
        //                }
        //                if (ven.Refflag == "PART NUMBER")
        //                {
        //                    partLst.Add(ven.RefNo);
        //                }
        //                if (ven.Type == "MANUFACTURER")
        //                {
        //                    mfrLst.Add(ven.Name);
        //                }
        //            }
        //        }
        //        row.Add("MODEL NUMBER", modelno);
        //        row.Add("BRAND NAME", brand);
        //        row.Add("DESIGNATION", designation);
        //        for (var i = 0; i < 5; i++)
        //        {
        //            if (partLst != null && partLst.Count > i)
        //            {
        //                row.Add("PART NO " + (i + 1), partLst[i] ?? "");
        //            }
        //            else
        //            {
        //                row.Add("PART NO " + (i + 1), "");
        //            }

        //            if (mfrLst != null && mfrLst.Count > i)
        //            {
        //                row.Add("MFR NAME " + (i + 1), mfrLst[i] ?? "");
        //            }
        //            else
        //            {
        //                row.Add("MFR NAME " + (i + 1), "");
        //            }

        //        }

        //        row.Add("DRAWING NUMBER", drwno);
        //        row.Add("POSITION NUMBER", posno);
        //        row.Add("SERIAL NUMBER", serial);
        //        row.Add("VENDOR/SUPPLIER NAME", supplier);
        //        row.Add("VENDOR/SUPPLIER PART NO", suppartno);
        //        row.Add("SOURCE URL", cde.Soureurl);

        //        if (cde.Catalogue != null)
        //            row.Add("Cataloguer", cde.Catalogue.Name);
        //        else row.Add("Cataloguer", "");

        //        if (cde.Review != null)
        //            row.Add("QC", cde.Review.Name);
        //        else row.Add("QC", "");

        //        if (cde.Release != null)
        //            row.Add("QA", cde.Release.Name);
        //        else row.Add("QA", "");


        //        row.Add("Remarks", cde.Remarks);
        //        rows.Add(row);
        //        //row = new Dictionary<string, object>();
        //        //row.Add("Item Code", cde.Itemcode);
        //        //row.Add("Material Code", cde.Materialcode);
        //        //if (cde.CreatedOn != null)
        //        //{
        //        //    DateTime date = DateTime.Parse(Convert.ToString(cde.CreatedOn));
        //        //    row.Add("CreatedOn", date.ToString("dd/MM/yyyy"));
        //        //}
        //        //row.Add("Legacy", cde.Legacy);
        //        //row.Add("Shortdesc", cde.Shortdesc);
        //        //string temp = cde.Longdesc;
        //        //if (temp.Length < 73)
        //        //{
        //        //    row.Add("Longdesc 1", cde.Longdesc);
        //        //}
        //        //else
        //        //{


        //        //    for (int i = 0, j = 0; j <= 20; j++)//, i = i + 72
        //        //    {
        //        //        if (72 < (cde.Longdesc.Length - i))
        //        //        {

        //        //            string spltLong = cde.Longdesc.Substring(i, 72);
        //        //            int lln = spltLong.LastIndexOf(' ');
        //        //            if (lln > 0)
        //        //            {
        //        //                spltLong = cde.Longdesc.Substring(i, lln);
        //        //                row.Add("Longdesc " + j, spltLong);

        //        //                i = i + lln;

        //        //            }
        //        //            else
        //        //            {
        //        //                spltLong = cde.Longdesc.Substring(i, 72);
        //        //                row.Add("Longdesc " + j, spltLong);

        //        //                i = i + 72;

        //        //            }


        //        //        }
        //        //        else
        //        //        {

        //        //            string spltLong = cde.Longdesc.Substring(i, (cde.Longdesc.Length - i));
        //        //            row.Add("Longdesc " + j, spltLong);
        //        //            break;

        //        //        }
        //        //    }

        //        //}

        //        ////row.Add("Cataloguer", cde.Catalogue);
        //        ////row.Add("QC", cde.Review);
        //        ////row.Add("QA", cde.Release);
        //        ////row.Add("Remarks", cde.Remarks);
        //        ////if (goo1 == true)
        //        ////{
        //        ////    row.Add("Plant", cde.Plant);
        //        ////}
        //        //rows.Add(row);
        //    }

        //    //var lst = new List<multiplecodelist>();
        //    //foreach (Prosol_Datamaster mdl in get_assigndata)
        //    //{
        //    //    var obj = new multiplecodelist();
        //    //    obj.Itemcode = mdl.Itemcode;
        //    //    obj.MaterialCode = mdl.Materialcode;
        //    //    //obj.CreatedOn = mdl.CreatedOn;
        //    //    if (mdl.CreatedOn != null)
        //    //    {
        //    //        DateTime date = DateTime.Parse(Convert.ToString(mdl.CreatedOn));

        //    //        obj.CreatedOn = date.ToString("dd/MM/yyyy");
        //    //    }
        //    //    //  obj.CreatedOn = date.ToString("dd/MM/yyyy");
        //    //    // DateTime date = DateTime.Parse(Convert.ToString(mdl.CreatedOn));
        //    //    obj.Legacy = mdl.Legacy;
        //    //    obj.Shortdesc = mdl.Shortdesc;
        //    //    obj.Longdesc = mdl.Longdesc;
        //    //    obj.Cataloguer = (mdl.Catalogue != null) ? (mdl.Catalogue.Name) : "";
        //    //    obj.Reviewer = (mdl.Review != null) ? (mdl.Review.Name) : "";
        //    //    obj.Releaser = (mdl.Release != null) ? (mdl.Release.Name) : "";
        //    //    obj.Remarks = mdl.Remarks;
        //    //    var temp = _Userreportservice.getplant(mdl.Itemcode).ToList();
        //    //    obj.Plant = temp[0].Plant;
        //    //    // obj.Plant = "";
        //    //    lst.Add(obj);
        //    //}
        //    var strJson = JsonConvert.SerializeObject(rows);
        //    DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));




        //    ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        //    wbook.Worksheets.Add(dt, "tab1");
        //    // Prepare the response
        //    HttpResponseBase httpResponse = Response;
        //    httpResponse.Clear();
        //    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    //Provide you file name here
        //    httpResponse.AddHeader("content-disposition", "attachment;filename=\"TrackReport.xlsx\"");

        //    // Flush the workbook to the Response.OutputStream
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        wbook.SaveAs(memoryStream);
        //        memoryStream.WriteTo(httpResponse.OutputStream);
        //        memoryStream.Close();
        //    }

        //    httpResponse.End();

        //    //string file = Server.MapPath("~/common/") + "MyExcelFile.xls";
        //    //Workbook workbook = new Workbook();
        //    //Worksheet worksheet = new Worksheet("First Sheet");
        //    //int i = 0;
        //    //foreach (DataRow dr in dt.Rows)
        //    //{
        //    //    int j = 0;
        //    //    foreach (DataColumn dc in dt.Columns)
        //    //    {
        //    //        worksheet.Cells[i, j] = new Cell(Convert.ToString(dr[j]));
        //    //        j++;
        //    //    }
        //    //    i++;
        //    //}
        //    ////worksheet.Cells[0, 1] = new Cell((short)1);
        //    ////worksheet.Cells[2, 0] = new Cell(9999999);
        //    ////worksheet.Cells[3, 3] = new Cell((decimal)3.45);
        //    ////worksheet.Cells[2, 2] = new Cell("Text string");
        //    ////worksheet.Cells[2, 4] = new Cell("Second string");
        //    ////worksheet.Cells[4, 0] = new Cell(32764.5, "#,##0.00");
        //    ////worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY\-MM\-DD");
        //    ////worksheet.Cells.ColumnWidth[0, 1] = 3000;
        //    //workbook.Worksheets.Add(worksheet);
        //    //workbook.Save(file);

        //    //return Json(lst, JsonRequestBehavior.AllowGet);

        //}
        public void DownloadMulticode()
        {
            var materialcode = Session["materialcode"].ToString();
            string[] codestr;
            List<Prosol_Datamaster> get_assigndata = new List<Prosol_Datamaster>();
            List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
            //List<multiplecodelist> getcode = new List<multiplecodelist>();
            List<string> code_split = new List<string>();
            codestr = materialcode.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }

            //foreach (string cdn in code_split)
            //{
            //    gd_data = _Userreportservice.trackmulticodelist(cdn).ToList();
            //    get_assigndata.AddRange(gd_data);
            //}



            var res1 = _Userreportservice.TrackDownloaddata(codestr);
            var res2 = _Userreportservice.TrackDownloadvendordata(codestr);

            DataTable dt = new DataTable();
            foreach (IDictionary<string, object> row in res1)
            {
                var rw = dt.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt.Rows.Add(rw);
            }
            DataTable dt1 = new DataTable();
            foreach (IDictionary<string, object> row in res2)
            {
                var rw = dt1.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt1.Columns.Contains(entry.Key.ToString()))
                    {
                        dt1.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt1.Rows.Add(rw);
            }
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "ITEM");
            wbook.Worksheets.Add(dt1, "MANUFACTURER");
            string fileName = "Items";
            //if (id != "QA")
            //{
            //  Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");
            httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}.xlsx\"");
            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();

        }



        [HttpPost]
        public JsonResult Expv(string Code, string Chk)
        //         [HttpGet]
        //   public void Expv(string Code, string Chk)
        {
            int cunt = 0;
            var lst = new List<vendor>();

            if (Chk == "true")
            {
                var res = _Userreportservice.bulkv().ToList();
                foreach (Prosol_Datamaster v in res)
                {
                    if (v.Vendorsuppliers != null)
                        foreach (Vendorsuppliers ven in v.Vendorsuppliers)
                        {
                            var a = new vendor();
                            a.Itemcode = v.Itemcode;
                            a.Type = ven.Type;
                            a.Name = ven.Name;
                            a.Refflag = ven.Refflag;
                            a.RefNo = ven.RefNo;
                            a.s = ven.s;
                            a.l = ven.l;
                            a.shortmfr = ven.shortmfr;
                            lst.Add(a);
                            cunt++;
                        }

                }
            }
            else if (Code != "undefined")
            {
                var res = _Userreportservice.bulkvv(Code).ToList();

                foreach (Prosol_Datamaster v in res)
                {
                    if (v.Vendorsuppliers != null)
                        foreach (Vendorsuppliers ven in v.Vendorsuppliers)
                        {
                            var a = new vendor();
                            a.Itemcode = v.Itemcode;
                            a.Type = ven.Type;
                            a.Name = ven.Name;
                            a.Refflag = ven.Refflag;
                            a.RefNo = ven.RefNo;
                            a.s = ven.s;
                            a.l = ven.l;
                            a.shortmfr = ven.shortmfr;
                            lst.Add(a);
                            cunt++;
                        }

                }

            }
            //if (lst.Count != 0)
            //{
            //    var strJson = JsonConvert.SerializeObject(lst);
            //    DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    string filename = Convert.ToString(DateTime.Now);
            //    string fls = filename.ToString() + ".xls";
            //    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fls));
            //    Response.ContentType = "application/ms-excel";
            //    string str = string.Empty;
            //    for (int i = 0; i < dt.Columns.Count; i++)
            //    {
            //        Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            //    }
            //    Response.Write("\n");
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        str = "";
            //        for (int j = 0; j < dt.Columns.Count; j++)
            //        {
            //            Response.Write(str + dr[j]);
            //            str = "\t";
            //        }
            //        Response.Write("\n");
            //    }
            //    Response.End();
            //}
            return Json(cunt, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public void Expv1(string Code, string Chk)
        {

            var lst = new List<vendor>();

            if (Chk == "true")
            {
                var res = _Userreportservice.bulkv().ToList();
                foreach (Prosol_Datamaster v in res)
                {
                    if (v.Vendorsuppliers != null)
                        foreach (Vendorsuppliers ven in v.Vendorsuppliers)
                        {
                            var a = new vendor();
                            a.Itemcode = v.Itemcode;
                            a.Type = ven.Type;
                            a.Name = ven.Name;
                            a.Refflag = ven.Refflag;
                            a.RefNo = ven.RefNo;
                            a.s = ven.s;
                            a.l = ven.l;
                            a.shortmfr = ven.shortmfr;
                            lst.Add(a);

                        }

                }
            }
            else if (Code != "undefined")
            {
                var res = _Userreportservice.bulkvv(Code).ToList();

                foreach (Prosol_Datamaster v in res)
                {
                    if (v.Vendorsuppliers != null)
                        foreach (Vendorsuppliers ven in v.Vendorsuppliers)
                        {
                            var a = new vendor();
                            a.Itemcode = v.Itemcode;
                            a.Type = ven.Type;
                            a.Name = ven.Name;
                            a.Refflag = ven.Refflag;
                            a.RefNo = ven.RefNo;
                            a.s = ven.s;
                            a.l = ven.l;
                            a.shortmfr = ven.shortmfr;
                            lst.Add(a);

                        }

                }

            }
            if (lst.Count != 0)
            {


                var strJson = JsonConvert.SerializeObject(lst);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
                Response.ClearContent();
                Response.Buffer = true;
                string filename = "Vendor Report";
                string fls = filename.ToString() + ".xls";
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fls));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }
                Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Response.Write(str + dr[j]);
                        str = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }

        }
        //CHARCETSTIC
        [HttpPost]
        public JsonResult EXPORTCHAR(string cat, string cat1, string Chk1)

        {
            int cunt = 0;
            var lst = new List<Characteristics>();
            if (Chk1 == "true")
            {
                var res = _Userreportservice.bulkchar().ToList();



                foreach (Prosol_Charateristics v in res)
                {
                    if (v.Values != null)
                    {


                        foreach (string id in v.Values)
                        {
                            var resres1 = _Userreportservice.getvalue(id);

                            //foreach (Prosol_Abbrevate ven in q1)
                            //{
                            if (resres1 != null)
                            {
                                var a = new Characteristics();
                                a.Noun = v.Noun;
                                a.Modifier = v.Modifier;
                                a.Characteristic = v.Characteristic;
                                a.Value = resres1.Value;
                                a.UOM = resres1.vunit;
                                a.Abbreviated = resres1.Abbrevated;

                                lst.Add(a);
                                cunt++;
                                //  }
                            }
                        }
                    }


                }
            }
            else if (cat != "undefined")
            {

                var res = _Userreportservice.bulkchar1(cat, cat1).ToList();



                foreach (Prosol_Charateristics v in res)
                {
                    if (v.Values != null)
                    {


                        foreach (string id in v.Values)
                        {
                            var resres1 = _Userreportservice.getvalue(id);

                            //foreach (Prosol_Abbrevate ven in q1)
                            //{
                            if (resres1 != null)
                            {
                                var a = new Characteristics();
                                a.Noun = v.Noun;
                                a.Modifier = v.Modifier;
                                a.Characteristic = v.Characteristic;
                                a.Value = resres1.Value;
                                a.UOM = resres1.vunit;
                                a.Abbreviated = resres1.Abbrevated;

                                lst.Add(a);
                                cunt++;
                                //  }
                            }
                        }
                    }


                }


            }


            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        //exportchar

        public void EXPORTCHAR1(string cat, string cat1, string Chk1)
        {
            int cunt = 0;
            var lst = new List<Characteristics>();

            if (Chk1 == "true")
            {
                var res = _Userreportservice.bulkchar().ToList();



                foreach (Prosol_Charateristics v in res)
                {
                    if (v.Values != null)
                    {


                        foreach (string id in v.Values)
                        {
                            var resres1 = _Userreportservice.getvalue(id);

                            //foreach (Prosol_Abbrevate ven in q1)
                            //{
                            if (resres1 != null)
                            {
                                var a = new Characteristics();
                                a.Noun = v.Noun;
                                a.Modifier = v.Modifier;
                                a.Characteristic = v.Characteristic;
                                a.Value = resres1.Value;
                                a.UOM = resres1.vunit;
                                a.Abbreviated = resres1.Abbrevated;

                                lst.Add(a);
                                cunt++;
                                //  }
                            }
                        }
                    }
                }

            }
            else if (cat != "undefined")
            {

                var res = _Userreportservice.bulkchar1(cat, cat1).ToList();



                foreach (Prosol_Charateristics v in res)
                {
                    if (v.Values != null)
                    {


                        foreach (string id in v.Values)
                        {
                            var resres1 = _Userreportservice.getvalue(id);

                            //foreach (Prosol_Abbrevate ven in q1)
                            //{
                            if (resres1 != null)
                            {
                                var a = new Characteristics();
                                a.Noun = v.Noun;
                                a.Modifier = v.Modifier;
                                a.Characteristic = v.Characteristic;
                                a.Value = resres1.Value;
                                a.UOM = resres1.vunit;
                                a.Abbreviated = resres1.Abbrevated;

                                lst.Add(a);
                                cunt++;
                                //  }
                            }
                        }
                    }


                }


            }
            if (lst.Count != 0)
            {



                var strJson = JsonConvert.SerializeObject(lst);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));

              ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, "tab1");
                // Prepare the response
                HttpResponseBase httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Provide you file name here
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"CharValueTrackReport.xlsx\"");

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();


            }

        }



        public class vendor
        {
            public string Itemcode { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Refflag { get; set; }
            public string RefNo { get; set; }
            public string shortmfr { get; set; }

            public int s { get; set; }
            public int l { get; set; }


        }

        public class Characteristics
        {
            public string Noun { get; set; }
            public string Modifier { get; set; }
            public string Characteristic { get; set; }
            public string Value { get; set; }
            public string Abbreviated { get; set; }
            public string UOM { get; set; }




        }
        //ERPLogsne


        [Authorize]
        public JsonResult GetERPLogs(string code)
        {
            var logs = new List<ERPLogModel>();

            var List = _Userreportservice.geterplogs(code);
            foreach (Prosol_ERPLog l in List)
            {
                var log = new ERPLogModel();
                log.Itemcode = l.Itemcode;
                log.fieldname = l.fieldname;
                log.oldValue = l.oldValue;
                log.newvalue = l.newvalue;
                log.page = l.page;
                log.module = l.module;
                log.CreatedOn = DateTime.Parse(Convert.ToString(l.CreatedOn));

                logs.Add(log);
            }
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public void Catdownload(string id)
        {
            var res1 = _Userreportservice.Downloaddata(Convert.ToString(Session["username"]), id).ToList();
            DataTable dt = new DataTable();
            foreach (IDictionary<string, object> row in res1)
            {
                var rw = dt.NewRow();
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                    rw[entry.Key] = entry.Value;
                }
                dt.Rows.Add(rw);
            }
            DataTable dt1 = new DataTable();
            if (id != "All")
            {
                var res2 = _Userreportservice.Downloadvendordata(Convert.ToString(Session["username"]), id).ToList();
                foreach (IDictionary<string, object> row in res2)
                {
                    var rw = dt1.NewRow();
                    foreach (KeyValuePair<string, object> entry in row)
                    {
                        if (!dt1.Columns.Contains(entry.Key.ToString()))
                        {
                            dt1.Columns.Add(entry.Key);
                        }
                        rw[entry.Key] = entry.Value;
                    }
                    dt1.Rows.Add(rw);
                }
            }
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "ITEM");
            if (id != "All")
            {
                wbook.Worksheets.Add(dt1, "MANUFACTURER");
            }
            string fileName = "Items";

            if (id == "cat")
            {
                fileName = "Cat_Items";
            }
            else if (id == "qc")
            {
                fileName = "QC_Items";
            }
            //if (id != "QA")
            //{
            //  Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //httpResponse.AddHeader("content-disposition", "attachment;filename=\"Report.xlsx\"");
            httpResponse.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}.xlsx\"");
            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();

            //   }
            //else
            //{
            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    string filename = "QAReport";
            //    string fls = filename.ToString() + ".csv";




            //    Response.ContentType = "application/csv";
            //    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fls));




            //    string str = string.Empty;
            //    for (int i = 0; i < dt.Columns.Count; i++)
            //    {
            //        Response.Write((dt.Columns[i].ToString().ToUpper()) + "\t");
            //    }
            //    Response.Write("\n");
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        str = "";
            //        for (int j = 0; j < dt.Columns.Count; j++)
            //        {
            //            Response.Output.Write(str + Convert.ToString(dr[j]).Replace("\n", " ").Replace("\t", " "));
            //            str = "\t";




            //        }
            //        Response.Write("\n");





            //    }
            //    Response.End();
            //}
        }

    }
}