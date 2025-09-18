using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core.Interface
{
   public partial interface IServiceReport
    {
        IEnumerable<Prosol_Users> getuser(string role);
        IEnumerable<Prosol_RequestService> getsearchresult(string PlantCode, string role, string Userid, string[] selection, string Fromdate, string Todate);
        // IEnumerable<Prosol_RequestService> ServiceReportSearch();

        ////bulktemplate for service

        int BulkServiceCategory(HttpPostedFileBase file);
        int bulkGroupUpload(HttpPostedFileBase file);
        int bulkServiceMainCodeUpload(HttpPostedFileBase file);
        int bulkServiceSubCodeUpload(HttpPostedFileBase file);
        int bulkServiceSubSubCodeUpload(HttpPostedFileBase file);
        int BulkCharacteristicUpload(HttpPostedFileBase file);
        int BulkvalueUpload(HttpPostedFileBase file);

    }
}
