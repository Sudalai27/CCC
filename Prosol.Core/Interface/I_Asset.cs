using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core.Interface
{
    public interface I_Asset
    {
        List<prosol_FARdashboard> GetFARInfo();
        bool Insertasset(Prosol_AssetMaster assetvalues, Prosol_AssetAttributes assetattri, HttpFileCollectionBase files);
        Prosol_AssetMaster getasset(string UniqueId);
        IEnumerable<Prosol_AssetMaster> GetAssetDataList(string uId);
        Prosol_AssetMaster GetAssetInfo(string UniqueId);
        int submitasset(List<Prosol_AssetMaster> AssetList);
        List<Dictionary<string, object>> getallassetdata(string FARId, string Role, string User, string Status, string fromdate, string todate);
       
          IEnumerable<Prosol_AssetMaster> getall(string PV, string Region, string fromdate, string todate);
        List<Dictionary<string, object>> loadAssetdata1(string PV, string Region, string Fromdate, string Todate);
        IEnumerable<Prosol_AssetMaster> trackAssetcodelist(string User, string Region);
        IEnumerable<Prosol_Users> getpvuser();
        IEnumerable<Prosol_AssetMaster> getallregion(string Region, string fromdate, string todate);
     
     

        //asset assign

      
        IEnumerable<Prosol_Users> getuserpv(string role);
        IEnumerable<Prosol_AssetMaster> reloaddata1(string Region, string City, string Area, string SubArea);
        IEnumerable<Prosol_AssetMaster> reloaddataa(string farId, string user);
        bool reassign_submit1(string item, string role, Prosol_AssetMaster reasgn);
        Prosol_Users GetCatId(string name);
        IEnumerable<Prosol_AssetMaster> catloaddata1();
        IEnumerable<Prosol_Users> getcatuser();
        bool catreassign_submit(string item, string role, Prosol_AssetMaster reasgn);
        string AssetCatBulk_Upload(HttpPostedFileBase file);

        List<Prosol_AssetBOM> getAllAssetBom(string EquipmentId);
        int UpdateAssetBom(List<Prosol_AssetBOM> ListBOM);
        Prosol_AssetAttributes GetAttributeInfo(string UniqueId);
        DataTable loaddata(HttpPostedFileBase file);
        string AssetBulk_Upload(HttpPostedFileBase file, string user);
        string BOMBulk_Upload(HttpPostedFileBase file);
        string AttriBulk_Upload(HttpPostedFileBase file);

        CommonMaster getAllCommonMaster();


        bool InsertDataRegn(Prosol_Region data);

        bool DisableReg(string id, bool Islive);
        //bool EnableReg(string id);
        CommonMaster getAllCommonMasterforTools();
        //city
        bool InsertDataCity(Prosol_City data);
        bool InsertDataAre(Prosol_Area data);
        bool InsertDataSubAre(Prosol_SubArea data);
        bool DisableCity(string id, bool Islive);
        bool DisableArea(string id, bool Islive);
        bool DisableSubArea(string id, bool Islive);
        bool DisableEquipType(string id, bool Islive);
        bool InsertDataEquipType(Prosol_EquipmentType data);
        bool DisableEquipClass(string id, bool Islive);
        bool InsertDataEquipClass(Prosol_EquipmentClass data);
        bool DisableLoc(string id, bool Islive);
        bool InsertDataLoc(Prosol_Location data);
        bool InsertDataBusiness(Prosol_FARMaster data);
        bool InsertDataFar(Prosol_FARRepository data);
        bool RemoveMfr(string id, bool sts, string flg);
        bool DisableBus(string id, bool sts);

        bool Disablemjr(string id, bool sts);

        bool Disablemnr(string id, bool sts);



        bool InsertData(Prosol_MajorClass data);
        bool InsertData1(Prosol_MinorClass data);

        bool InsertDataIdent(Prosol_Identifier data);
        bool Disableidnt(string id, bool sts);

        string IdentifierBulk_Upload(HttpPostedFileBase file);
        List<Prosol_AssetMaster> searchItemCode(string sCode, string sSource, string sUser, string sStatus, string sQR);
        IEnumerable<Prosol_City> getAllCities(string City);
        IEnumerable<Prosol_Users> getuseronly(string username);
        IEnumerable<Prosol_AssetMaster> PV_Reassign(string username, string Role);
        IEnumerable<Prosol_Users> AutoSearchUserName(string term);
        bool PVreassign_submit(string item, string role, Prosol_AssetMaster reasgn);

       bool insertImages(Prosol_AssetMaster imgAsset);
        bool insertBomImages(Prosol_AssetBOM imgBom);

        Prosol_AssetBOM getBomInfo(string Bomid);
        bool AssetRework(string UniqueId, string RevRemarks, string CondRemarks, string ImageRemarks, string AddRemarks, int sts);

        IEnumerable<Prosol_AssetMaster> getAssetDataByFun(string Busi, string Major, string FunStr);
        IEnumerable<Prosol_AssetMaster> getAssetDataByFun(string Busi, string Major);
        bool InserGridflds(List<Prosol_GridSettings> flds);
        List<Prosol_GridSettings> getAllFields(string UserId);
        int ApproveFAR(List<Prosol_AssetMaster> flds);
        IEnumerable<Prosol_AssetMaster> catreloaddata1(string Business, string Major, string Region, string City, string Area, string SubArea);
        IEnumerable<Prosol_Users> getuser();
        List<Prosol_FARRepository> GetFarMaster();
        List<Prosol_FARMaster> GetTarMaster();
        List<Prosol_SiteMaster> GetSiteMaster();
        List<Prosol_AssetTypeMaster> GetAssetTypeMaster();
        List<Prosol_Location> GetLocationMaster();
        bool InsertDataSite(Prosol_SiteMaster data);
        bool InsertDataLoc1(Prosol_Location data);
        bool InsertDataAT(Prosol_AssetTypeMaster data);
        IEnumerable<Prosol_AssetMaster> catreloaddataa(string farId);
        String[] GenerateShortLong(Prosol_AssetMaster cat);
        List<Prosol_AssetMaster> getMasterItems();
        List<Prosol_AssetMaster> getSKUItems();
        List<Prosol_AssetMaster> getReqItems();
        List<Prosol_AssetMaster> getPendItems();
        int BulkShortLong(HttpPostedFileBase file);
        List<Dictionary<string, object>> Downloaddata(string username, string status);
        string BulkCatData(HttpPostedFileBase file);
        string BulkQcData(HttpPostedFileBase file);
        string AssetBulkData(HttpPostedFileBase file);
        bool AddValue(string Noun, string Modifier, string Attribute, string Value, string abb, string user, string role);
        List<Dictionary<string, object>> Downloadvendordata(string username, string status);
        int BulkRework(HttpPostedFileBase file);
        List<Dictionary<string, object>> getallattrdata(string FARId, string Role, string User, string Status, string fromdate, string todate);

        int BulkDashboard(HttpPostedFileBase file);

        IEnumerable<Prosol_Dashboard> getDashboard();

        IEnumerable<Prosol_AssetMaster> trackmulticodelist(string codestr);
        List<Dictionary<string, object>> Downloaddatacodes(string[] code_split);
        List<Dictionary<string, object>> Downloadvendordatacodes(string[] code_split);
        string AssetProxyBulk_Upload(HttpPostedFileBase file, string user);
        bool DisableNotes(string id, bool sts);
        string VendorBulk_Upload(HttpPostedFileBase file);
        IEnumerable<QRObj> AssetQRCheck(HttpPostedFileBase file);
        List<Prosol_FARMaster> GetMfrMaster();
        string InsertMfr(Prosol_FARMaster item);
        IEnumerable<Prosol_FARMaster> GetDataList(string Label);
        IEnumerable<Prosol_FARMaster> GetDataList();
        int BulkSwap(HttpPostedFileBase file);
        int BulkAdditional(HttpPostedFileBase file);
        List<Dictionary<string, object>> DownloadMFR(string id);
        //int BulkLocation(HttpPostedFileBase file);
        int MfrBulkUpload(HttpPostedFileBase file);
        int GetLastRunning(string label);
        bool UpdateRunning(string label, int runNo);
        string GenerateQRCode(QRRequestModel model, out string tagNo);
        List<Prosol_AssetBOM> GetBOM(string id);
        List<string> GetAssetValues(string Noun, string Modifier, string Attribute);
        List<Prosol_AssetMaster> BindAll();
        IEnumerable<Prosol_Users> BindUsersByRole(string role);
        List<Prosol_Funloc> GetFuncLoc();
        bool InsertDataFL(Prosol_Funloc data);
        bool UpdateDataBusiness(Prosol_FARMaster data);
        bool DisableFunLoc(string section, string id, bool sts);
        bool UpdateDataFL(Prosol_Funloc data);
        int BulkParent(HttpPostedFileBase file);
        int BulkObject(HttpPostedFileBase file);
        int BulkUNSPSC(HttpPostedFileBase file);
        int BulkAssetNo(HttpPostedFileBase file);
        int BulkCost(HttpPostedFileBase file);
        int BulkDiscipline(HttpPostedFileBase file);
        int BulkWorkC(HttpPostedFileBase file);
        bool Deletefile(string uniqueId, string fileName);
        int BulkAutoShortLong();
        int BulkURL(HttpPostedFileBase file);
        List<Prosol_Funloc> GetFL(string id);
        List<Prosol_AssetBOM> getFLBom(string id);
        int BulkTag(HttpPostedFileBase file);
        int BulkLegacy(HttpPostedFileBase file);
    }
}
