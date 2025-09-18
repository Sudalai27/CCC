using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Prosol.Core.Interface
{
    public interface IGeneralSettings
    {

        //Group codes
        bool CreateGroupcode(Prosol_GroupCodes grp);
        IEnumerable<Prosol_GroupCodes> GetGroupcodeList();
        IEnumerable<Prosol_GroupCodes> GetGroupcodeList(string srchtxt);
        bool DeleteGroupcode(string id);
        bool Duplicatecheck(string id, bool sts);
        //Sub Group codes
        bool CreateSubGroupcode(Prosol_SubGroupCodes grp);
        IEnumerable<Prosol_SubGroupCodes> GetSubGroupcodeList();
        IEnumerable<Prosol_SubGroupCodes> GetSubGroupcodeList1(string srchtxt);
        bool DeleteSubGroupcode(string id);
        //subsubgroup
        IEnumerable<Prosol_SubGroupCodes> GetSubGroupcodeList(string maingroup);
        bool InsertSubSubgroup(Prosol_SubSubGroupCode data, int update);
        IEnumerable<Prosol_SubSubGroupCode> ListofSubSubUser();
        IEnumerable<Prosol_SubSubGroupCode> GetSubsubGroupcodeList(string SubGroupCode);
        IEnumerable<Prosol_SubSubGroupCode> GetSubSubGroupListSearch(string srchtxt);
        bool DeleteSubsubGroupcode(string id);
        //UOM
        bool CreateUOM(Prosol_UOM uom); 
        IEnumerable<Prosol_UOM> GetUOMList();
        IEnumerable<Prosol_UOM> GetUOMList(string srchtxt);
        string[] GetUOM(string label);
        bool DeleteUOM(string id);
        int BulkUOM(HttpPostedFileBase file);
        //UOM1
        bool InsertData(Prosol_UOMMODEL data, int update);
        IEnumerable<Prosol_UOMMODEL> getlistuom();
        bool DeleteUOM1(string id);
        //IEnumerable<Prosol_UOMMODEL> GetUOMList1();
        //IEnumerable<Prosol_UOMMODEL> GetUOMList1(string srchtxt);


        //Vendor

        bool CreateVendor(Prosol_Vendor vendor);
        IEnumerable<Prosol_Vendor> GetVendorList(); 
        IEnumerable<Prosol_Vendor> GetVendorList(string srchtxt);
        bool DeleteVendor(string id);
        string getNextCode();
        bool DisableVendor(string id,bool sts);
        int BulkVendor(HttpPostedFileBase file);
        IEnumerable<Prosol_Vendor> GetVendorLst(string term);
        IEnumerable<Prosol_UNSPSC> GetUnspscc();
        //Abbrevate      
        bool CreateAbbr(Prosol_Abbrevate uom);
        IEnumerable<Prosol_Abbrevate> GetAbbrList();
        IEnumerable<Prosol_Abbrevate> GetAbbrList(string srchtxt);
        bool DeleteAbbr(string id,string val);
        bool unAbbrDel(string id);
        int BulkAbbri(HttpPostedFileBase file, string User);
        List<Prosol_Abbrevate> DownloadValuemaster(string FrmDte, string ToDte);

        IEnumerable<Prosol_Charateristics> getvaluelist();

        //Vendortype      
        bool CreateVendortype(Prosol_Vendortype Vendortype);
        IEnumerable<Prosol_Vendortype> GetVendortypeList();
        bool DeleteVendortype(string id);

        //Reference type      
        bool CreateReftype(Prosol_Reftype Vendortype);
        IEnumerable<Prosol_Reftype> GetReftypeList();
        bool DeleteReftype(string id);

        //logics

        IEnumerable<Prosol_Charateristics> GetAttributesList();
        bool CreateLogics(Prosol_Logics logic);
        Prosol_Logics GetLogic(string Noun, string Modifier);

        //UNSPSC
        int BulkUNSPSC(HttpPostedFileBase file);
        IEnumerable<Prosol_UNSPSC> GetUnspsc(string Noun, String Modifier);
        IEnumerable<Prosol_UNSPSC> GetUNSPSCListSearch(string srchtxt);

        bool Delunspsc(string id);
        string[] loadversion();

        IEnumerable<Prosol_UNSPSC> GetUnspsc();
        //Bulkdata

        string BulkData(HttpPostedFileBase file);
         List<Dictionary<string, object>> BulkAttribute(HttpPostedFileBase file);
        IEnumerable<Prosol_UOM> Getunit();
        Prosol_Vendor getVendorAbbrForShortDesc(string mfr);
       
        //new code
        Prosol_Vendor FINDVENDORMASTER(string mfr);

        //HSN
        IEnumerable<Prosol_HSNModel> GetHSNList(string srchtxt);
        int BulkHSN(HttpPostedFileBase file);
        IEnumerable<Prosol_HSNModel> GetHSNList();
        bool DeleteHSN(string HSNID);
        bool CreateHSN(Prosol_HSNModel hsn);
        bool CreateHSN1(string Noun, string Modifier, string HSNID, string DESC);
        Prosol_HSNModel GetHsn(string Noun, string Modifier);
        string BulkPVData(HttpPostedFileBase file);
        string BulkCatData(HttpPostedFileBase file);
        string BulkQcData(HttpPostedFileBase file);
        int BulkRework(HttpPostedFileBase file);
    }
}
