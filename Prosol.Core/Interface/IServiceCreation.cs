using Prosol.Core.Model;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public partial interface IServiceCreation
    {
        //servicecreation
        //get mainsub attribute table
        IEnumerable<Prosol_MSAttribute> GetMainSubAttributeTable(string SubCode);
        IEnumerable<Prosol_SSubSubCode> GetSubSubList(string MainCode,string SubCode);
        bool rejectitem(string id, Prosol_UpdatedBy pu, string rejectedas, string Remarks);
        bool senttoReview(string _id, string Remarks);
        bool senttocleanser(string _id, string Remarks);
        string getabbr(string ab);
        List<Prosol_ServiceGroup> getgroupcodeforcatagory(string catagory);
        Prosol_RequestService getdatalistforupdate(string id);
        bool InsertServiceCreation(Prosol_RequestService prs);
        bool updateServiceReview(Prosol_RequestService prs);
        bool updateServiceRelease(Prosol_RequestService prs);
        string getRunningNo(string mc, string sc, string ssc);
        IEnumerable<Prosol_Users> LoadReviewer();
        IEnumerable<Prosol_Users> LoadReleaser();
        IEnumerable<Prosol_RequestService> getdatalistforReviewer(string Reviewer);
        IEnumerable<Prosol_RequestService> getdatalistforReleaser(string Releaser);
        IEnumerable<Prosol_RequestService> getdatalistforCleanser(string Cleanser);
        //checkvalue

        string CheckValue(string MainCode, string SubCode, string Attribute, string Value);
        //addvalue
        bool AddValue1(string Noun, string Modifier, string Attribute, string Value, string abb);
        bool codesaveforlogic(Prosol_Servicecodelogic data);
        string[] loadversionforservice();
        Prosol_Servicecodelogic Showdataservice();
        Prosol_Servicecodelogic Showdatacodelogic();
        IEnumerable<Prosol_UNSPSC> getCOMMList(string sKey);
        IEnumerable<Prosol_UNSPSC> getCOMMCOMMList(string sKey);
        string getclchk();
        string generateServicecodeunspsc(string SubCode);
        // string generateServicecodeitem(string itemcode);
        string[] showall_user();
        IEnumerable<Prosol_ServiceGroup> GetcatList();
        //default
        IEnumerable<Prosol_ServiceDefaultAttr> Defaultattribute();
        List<Prosol_RequestService> searchMaster1(string sCode,string iCode, string sSource, string sShort, string sLong, string sCategory, string sGroup, string sUser, string sStatus);
        //csi
        Prosol_RequestService GetSingleItemser(string ItemId);
        IEnumerable<Prosol_RequestService> checkDuplicate(string Short, string itemid);
        //bool GetCodeForclarifiyItems(Prosol_RequestService prs);
         //bool GetCodeForclarifiyItems1(Prosol_RequestService rs, string itemid, string remark, string userid, string username);
         //bool GetCodeForclarifiyItems2(Prosol_RequestService rs, string itemid, string remark, string userid, string username);
    }
}
