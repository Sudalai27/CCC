using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Web;
using System.Data;


namespace Prosol.Core.Interface
{
    public partial interface I_Report
    {
        List<Dictionary<string, object>> loaddata(string[] options, string where, string value, string fromdate, string todate, string role, string status);
        List<Dictionary<string, object>> loaddata1(string[] options, string where, string value, string fromdate, string todate, string role, string status);
        List<Dictionary<string, object>> loaddata2(string[] options, string fromdate, string todate);
        IEnumerable<Prosol_Plants> getplant();
        // IEnumerable<Prosol_Users> AutoSearchUserName(string term);

        IEnumerable<Prosol_Users> getuser(string role, string[] plants);
        IEnumerable<Prosol_Datamaster> getcode(string role);
        IEnumerable<Prosol_Datamaster> bulkvv(string term);
        // IEnumerable<Prosol_Users> getuser();
        //  IEnumerable<Prosol_Plants> getplantlist();
        IEnumerable<Prosol_Departments> getdepartment();
        
        IEnumerable<Prosol_Datamaster> bulkv();
        IEnumerable<Prosol_Users> getuseronly(string username);

        List<Dictionary<string, object>> Trackload(string plant, string fromdate, string todate, string option);
        //multicode
        IEnumerable<Prosol_Datamaster> trackmulticodelist(string codestr);
        IEnumerable<Prosol_ERPInfo> getplant(string Itemcode);
        IEnumerable<Prosol_Charateristics> bulkchar();
        Prosol_Abbrevate getvalue(string id);
        IEnumerable<Prosol_Charateristics> bulkchar1(string cat,string cat1);
        List<Dictionary<string, object>> Delivery(string plant, string fromdate, string todate, string option);
        IEnumerable<Prosol_ERPInfo> Getmaterialtypedata(string Materialtype);
        IEnumerable<Prosol_Datamaster> getalldata(string plant, string fromdate, string todate);
        List<Dictionary<string, object>> loadstatusdata(string[] options, string fromdate, string todate);
        List<Prosol_ERPLog> geterplogs(string code);

        List<Dictionary<string, object>> Downloaddata(string username, string status);

        List<Dictionary<string, object>> Downloadvendordata(string username, string status);
        List<Dictionary<string, object>> TrackDownloaddata(string[] code_split);
        List<Dictionary<string, object>> TrackDownloadvendordata(string[] code_split);

    }
}
