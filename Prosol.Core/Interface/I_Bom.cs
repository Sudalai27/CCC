using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Web;


namespace Prosol.Core.Interface
{
    public partial interface I_Bom

    {
        // string getItem();
        int InsertData(List<Prosol_equipbom> data);
        int InsertMatData(List<Prosol_MaterialBom> data);
        List<Prosol_MaterialBom> getmatspares(string term);
        List<Prosol_MaterialBom> allmat2();
        List<Prosol_Funloc> master();
        List<Prosol_equipbom> getspares(string term);
        List<Prosol_equipbom> getreport(string term, bool a, bool b, bool c);
        List<Prosol_MaterialBom> getreport2(string term);
        List<Prosol_MaterialBom> getreport12(string term);
        List<Prosol_equipbom> manfacsearch(string term);
        List<Prosol_Vendor> mandcsearch(string term);
        List<Prosol_Funloc> funlocsearch(string term);
        List<Prosol_Funloc> Total();
        List<Prosol_equipbom> funequip(string term);
        List<Prosol_equipbom> complete();
        List<Prosol_Funloc> funlocsearch1();
        List<Prosol_Datamaster> frstmat();
        List<Prosol_MaterialBom> matequip(string term);
        List<Prosol_MaterialBom> material();
        List<Prosol_Master> matty(string term);
        List<Prosol_equipbom> Codecount(string term);
        List<Prosol_ERPInfo> get(string term);
        List<Prosol_equipbom> srch(string term);
        List<Prosol_MaterialBom> srchbom(string term);
        List<Prosol_Datamaster> BulkMat1(string term);
        List<Prosol_equipbom> fun(string term);
        List<Prosol_equipbom> getreport1(string term);
        // List<Prosol_Datamaster> searching11(string term, bool a, bool b, bool c);
        List<Prosol_Datamaster> searching(string term);
        List<Prosol_equipbom> allmat();
        List<Prosol_Datamaster> totalmat();
        List<Prosol_Datamaster> searching1(string term);
        bool Singlefun(Prosol_Funloc data);
        bool delfun(string id);
        bool Equipsingle(Prosol_Funloc data);

        List<Prosol_Funloc> gethei(string term);
        List<Prosol_equipbom> get2(string term);
        List<Prosol_Funloc> funlocsearch2();
        int BulkEquip(HttpPostedFileBase file);
        int BulkFunloc(HttpPostedFileBase file);

        List<Prosol_MaterialBom> masterdata();
        IEnumerable<Prosol_Funloc> getFUNLOCList();
        string genaratemcpcode(string LogicCode);
        bool insetmcp(prosol_Mcp prs);
        List<prosol_Mcp> get_mcp_list();
        List<prosol_Mcp> remove_mcp(string mcpcode);
        prosol_Mcp getsinglemcp(string mcpcode);
        List<prosol_Mcp> getmcpwithcondition(string discipline, string drive, string equipmnt);
        bool CreateTasklist(Prosol_Tasklist ptl);
        List<Tasklist_operationSequence> getmcpforMP(string eqpmnt, string model, string make);
        Prosol_Funloc getfunc_loc(string fl);
        bool updateMP(Prosol_MaintenancePlan pmp);
        IEnumerable<Prosol_IOModel> getinrecord();
        List<Dictionary<string, object>> Trackload(string materialcode, string fromdate, string todate, string option);
        IEnumerable<Prosol_IOModel> trackmulticodelist(string codestr);
        bool DelEquip(string id);
        

    }
}
