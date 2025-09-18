using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core.Interface
{
    public interface IEquipment
    {
        IEnumerable<Prosol_Vendor> GetVendorList(string term);

        bool WriteERPInfo(Prosol_ERPInfo gen);

        //bool WriteGeneralinfo(Prosol_Generalinfo gen);
        //bool WritePlantinfo(Prosol_Plantinfo plant);
        //bool WriteMRPdata(Prosol_MRPdata mrp);
        //bool WriteSalesinfo(Prosol_Salesinfo sales);

        int WriteData(Prosol_Datamaster cat, string stus);

        int WriteDataList(List<Prosol_Datamaster> catList);
        int WriteRDataList(List<Prosol_Datamaster> catList);
        int WriteReleaseDataList(List<Prosol_Datamaster> catList);

        int WriteAttachment(List<Prosol_Attachment> lisAttachment, HttpFileCollectionBase files);
        int AddexistSpares(List<Prosol_Datamaster> catList);
        IEnumerable<Prosol_Datamaster> GetDataList(int sts, int sts1, string uId);
        IEnumerable<Prosol_Datamaster> GetRDataList(int sts, int sts1, string uId);
        IEnumerable<Prosol_Datamaster> GetReleaseDataList(int sts, int sts1, string uId);
        IEnumerable<Prosol_Datamaster> GetSparesList(string itmcde);
        IEnumerable<Prosol_Datamaster> FetchExist(string srchcde);
        Prosol_Datamaster GetSingleItem(string Itemcode);
        Prosol_Datamaster GetRSingleItem(string Itemcode);
        Prosol_Datamaster GetReleaseSingleItem(string Itemcode);


        Prosol_ERPInfo GetERPInfo(string Itemcode);
        //Prosol_Generalinfo GetGeneralinfo(string Itemcode);
        //Prosol_Plantinfo GetPlantinfo(string Itemcode);
        //Prosol_MRPdata GetMRPdata(string Itemcode);
        //Prosol_Salesinfo GetSalesinfo(string Itemcode);
        IEnumerable<Prosol_Attachment> GetAttachment(string Itemcode);

        bool UpdateData(List<Prosol_Datamaster> Listcat);
        IEnumerable<Prosol_Users> getReviewerList();
        IEnumerable<Prosol_Users> getReleaserList();

        bool Deletefile(string id, string imgId);
        byte[] Downloadfile(string imgId);

        // List<Prosol_Datamaster> checkPartno(string Partno, string icode);
        List<Prosol_Datamaster> checkPartno(string Partno, string icode, string Flag);
        List<Prosol_Datamaster> checkDuplicate(Prosol_Datamaster cat);


        bool Reworkreview(string itemcode);
        bool Reworkrelease(string itemcode);

        string getItem();
        string getMaterialCode(string LogicCode);

        List<string> GetValuesList(string Noun, string Modifier, string Characteristic);
        List<string> GetValues(string Noun, string Modifier, string Attribute);
        string CheckValue(string Noun, string Modifier, string Attribute, string Value);
        bool AddValue(string Noun, string Modifier, string Attribute, string Value, string abb, string user);
        List<Prosol_Attribute> GetUnits();
        int checkValidate(string Attribute);
        IEnumerable<Prosol_Plants> getplantCode_Name();

        List<Prosol_Datamaster> GetsimItemsList(string Noun, string Modifier);
        //uom
        //  IEnumerable<Prosol_UOM> getuomlist();
        IEnumerable<Prosol_UOMMODEL> getuomlist();

        IEnumerable<Prosol_HSNModel> getHSNList(string sKey);
        //Logic

        List<Prosol_Logic> checkLogic(string Attribute);

        string getunitforvalue(string Value);

        //reject
        bool GetCodeForRejectedItems(string Itemcode, string Remarks, string userid, string username);
        bool GetCodeForRejectedItems1(string Itemcode, string RevRemarks, string userid, string username);
        bool GetCodeForRejectedItems2(string Itemcode, string RelRemarks, string userid, string username);
        //pvdat
        bool PVDATA(string Itemcode, string Remarks, string userid, string username);

        // for over all serarch
        IEnumerable<Prosol_Datamaster> GetTotalItem();

        //db search

        List<Prosol_Datamaster> searchMaster(string sCode, string sSource, string sShort, string sLong, string sNoun, string sModifier, string sUser, string sType, string sStatus);

        string[] showall_user();

        String[] GenerateShortLong(Prosol_Datamaster cat);

        // for gettting image name to delete image
        string GetDeletefile(string id);

        // for getting last file name
        string getlastinsertedfilename(string Itemcode);
        string getlastinsertedpdfname(string Itemcode);

        // get abbrivated vaue
        string getsm_value(string value1);
        int GetCodeForRejectedItems(List<Prosol_Datamaster> lpdm);

        List<Prosol_Attachment> findattachmentss(string str);
        List<Prosol_Datamaster> GetEquip(string EName);
        int BulkData(HttpPostedFileBase file);
        int BulkAttribute(HttpPostedFileBase file);
        int BulkVendor(HttpPostedFileBase file);
        int BulkEquip(HttpPostedFileBase file);
        int BulkShortLong(HttpPostedFileBase file);
        int BulkDictionary(HttpPostedFileBase file);
        
        void change_statusforedit(string userid, string itemcode);

        //PVDATA
        List<Prosol_Datamaster> searchmasterpv(string sCode, string Plant, string StorageLocation, string storagebin, string sNoun, string sModifier);

        IEnumerable<Prosol_Datamaster> GetDataListpv(string Pending, string uId);

        //apimodel
      //  bool writeapimodel(Prosol_ApiModel apiml);

        int RemoveEquipment(List<Prosol_Datamaster> catList);
        int CopyEquip(List<Prosol_Datamaster> catList);
        string PotentialEquip(List<Prosol_Datamaster> catList);

        int RemoveSpare(List<Prosol_Datamaster> catList,string equip);
        int CopySpare(List<Prosol_Datamaster> catList, string equip);

        List<Prosol_Funloc> GetFunLocList(string srch);

        List<Prosol_EquipDictionary> Getsparelist(string Noun, string Modifier);
    }
}
