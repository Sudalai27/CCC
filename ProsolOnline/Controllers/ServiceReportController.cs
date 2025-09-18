using Newtonsoft.Json;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using ExcelLibrary.Office.Excel;

namespace ProsolOnline.Controllers
{
    public class ServiceReportController : Controller
    {
        private readonly IServiceReport _ServiceReportService;
        
        public ServiceReportController(IServiceReport ServiceReportService)

        {
            _ServiceReportService = ServiceReportService;
            
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
        // GET: ServiceReport
        public ActionResult Index()
        {

            if (CheckAccess("Service Report") == 1)
                return View();
            else if (CheckAccess("Service Report") == 0)
                return View("Denied");
            else return View("Login");
            //return View();
        }
        public JsonResult getuser(string role)
        {
            var result = _ServiceReportService.getuser(role);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServiceReportSearch()
        {
            Prosol_RequestService prs1 = new Prosol_RequestService();

         //   var  pc = Request.Form["PlantCode"];
            string PlantCode = JsonConvert.DeserializeObject<String>(Request.Form["PlantCode"]);
            string role = JsonConvert.DeserializeObject<String>(Request.Form["role"]);// Request.Form["role"];
            string Userid = JsonConvert.DeserializeObject<String>(Request.Form["Userid"]); // Request.Form["Userid"];
            string[] selection = JsonConvert.DeserializeObject<String[]>(Request.Form["selection"]); //Request.Form["selection"];
            string Fromdate = JsonConvert.DeserializeObject<String>(Request.Form["Fromdate"]); //Request.Form["Fromdate"];
            string Todate = JsonConvert.DeserializeObject<String>(Request.Form["Todate"]); //Request.Form["Todate"];

            var result = _ServiceReportService.getsearchresult(PlantCode, role, Userid, selection, Fromdate, Todate);

                                                                                              //  DateTime fd = Convert.ToDateTime(Fromdate.ToString());

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        //donloadexcel


        public void DownloadServiceReport(string PlantCode, string Fromdate, string Todate, string[] options, string role,string Userid)
        {

            if (PlantCode == "undefined")
                PlantCode = null;

            //if (options[] == "undefined")
            //    options = null;

            if (Fromdate == "null")
                Fromdate = null;
            if (Todate == "null")
                Todate = null;
            var res = _ServiceReportService.getsearchresult(PlantCode, role, Userid, options, Fromdate, Todate).ToList();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            foreach(Prosol_RequestService mdl in res)
            {
                row = new Dictionary<string, object>();
                row.Add("ItemId", mdl.ItemId);
                row.Add("ServiceCode", mdl.ServiceCode);
                row.Add("ServiceCategoryName", mdl.ServiceCategoryName);
                row.Add("ServiceGroupName", mdl.ServiceGroupName);
                row.Add("PlantName", mdl.PlantName);
                row.Add("Legacy", mdl.Legacy);
                row.Add("ShortDesc", mdl.ShortDesc);
                row.Add("Noun", mdl.Noun);
                row.Add("Modifier", mdl.Modifier);
                row.Add("Valuation Class", mdl.ValuationName);
                row.Add("UOM", mdl.UomCode);
                row.Add("LongDesc", mdl.LongDesc);
                row.Add("Relationship", mdl.parent=="Yes"?"Parent":mdl.parent==null&&mdl.Child==null?"":"Child");
                if(mdl.Child != null)
                {
                    string tmp = "";
                    foreach(string x in mdl.Child)
                    {
                        tmp= tmp+  x+',';
                    }
                    tmp = tmp.TrimEnd(',');
                    row.Add("Parent/Child", tmp);
                }
                else
                {
                    row.Add("Parent/Child", "");
                }
                //if (mdl.Last_updatedBy != null)
                //{
                //    row.Add("Last_updatedBy", mdl.Last_updatedBy.Name);
                //}
                //else row.Add("Last_updatedBy", "");

                //if (mdl.Cleanserr != null)
                //{
                //    row.Add("Cleanserr", mdl.Cleanserr.Name);
                //}
                //else row.Add("Cleanserr", "");

                if (mdl.Characteristics != null)
                {
                    int j = 1;
                    foreach (var at in mdl.Characteristics)
                    {
                        row.Add("Attribute" + j, at.Attributes);
                        row.Add("Value" + j, at.Value);
                  
                        j++;
                    }
                }

                rows.Add(row);


            }
            var strJson = JsonConvert.SerializeObject(rows);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
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
            //////////////////////////////////////////
            //    DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            //   // string file = Server.MapPath("~/common/") + "ExportService.xls";
            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ExportService.xls"));
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
            //            if (dr[j].ToString().Contains('\n'))
            //            {
            //                dr[j] = dr[j].ToString().Replace('\n', ' ');
            //            }
            //            Response.Write(str + dr[j]);
            //            str = "\t";
            //        }
            //        Response.Write("\n");
            //    }
            ////    Response.TransmitFile(@"/Common/ExportService.xls");
            //    Response.Flush();
            //    Response.End();



        }
    }
}