using Prosol.Core.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core.Interface
{
    public partial interface  IServiceMaster
    {
        //category
        List<Prosol_Users> getCleanser();
        bool InsertDatasercat(Prosol_ServiceCategory data, int update);
        IEnumerable<Prosol_ServiceCategory> showall_Categoryuser();
        bool Dservicecategory(string id, bool sts);
        bool DelServicecode(string id);
        //group
        bool InsertData(Prosol_ServiceGroup data, int update);
     
        IEnumerable<Prosol_ServiceGroup> showall_groupuser();
        bool Dservicegroup(string id, bool sts);
        bool DelGroupcode(string id);
        IEnumerable<Prosol_ServiceGroup> getgroup(string SeviceCategorycode);

        //Uom
        bool InsertDataUom(Prosol_ServiceUom data, int update);
        IEnumerable<Prosol_ServiceUom> showall_Uomuser();
        bool DserviceUom(string id, bool sts);

        bool DelUOMcode(string id);
        //valuation
        bool InsertDataValuation(Prosol_ServiceValuation data, int update);
        IEnumerable<Prosol_ServiceValuation> showall_Valuationuser();
        bool Dservicevaluation(string id, bool sts);
        bool DelValuationcode(string id);

        //coading
        //maincode
        bool InsertDataMainCode(Prosol_SMainCode data, int update);
        IEnumerable<Prosol_SMainCode> showall_MainCode(string catagory);
        IEnumerable<Prosol_SMainCode> showall_MainCode();
        bool DMainCode(string id, bool sts);
        bool DelMaincode(string id);
        //subcode
        bool InsertDataSubCode(Prosol_SSubCode data,int update);
        IEnumerable<Prosol_SSubCode> showall_SubCodeUser();
        bool DSubCode(string id, bool sts);
        bool DelSubcode(string id);

        //subsub
        bool InsertDataSubSub(Prosol_SSubSubCode data,int update);
        IEnumerable<Prosol_SSubCode> getSubList(string MainCode);
     //   IEnumerable<Prosol_SSubCode> subcode1(string MainCode);
        IEnumerable<Prosol_SSubSubCode> showall_SubSubUser();
        bool DSubSub(string id, bool sts);
        bool DelSubSubcode(string id);

        //attributes
        //  bool addMS(Prosol_SMainSubs data);
        bool attribute(List<Prosol_MSAttribute> data);
       // IEnumerable<Prosol_MSAttribute> getAttribute(string Activity);
        List<Prosol_MSAttribute> getAttributeUniqueList();
        //requestservice
        IEnumerable<Prosol_Users> get_approvercodename();
        //running

        int getlast_request_R_no1(string str);
      
        bool newRequestService(Prosol_RequestService req_model);

        IEnumerable<Prosol_Users> get_approvercode_name_using_approverid1(string app);
        bool insert_multiplerequest1(List<Prosol_RequestService> request, string reqid);

        // characteristic value and abbreviation
        bool Insertvalabbr(Prosol_ServiceCharacteristicValue pcv,int update);
        IEnumerable<Prosol_ServiceCharacteristicValue> ListValAbbr();

        bool Delvalabbrcode(string id);
         bool DValAbbr(string id, bool sts);
        //bulkupload for service
        List<BulkRequestService_Load> loaddata1(HttpPostedFileBase file);
        //bulkcheck
        IEnumerable<Prosol_Plants> getplantCode_Name();
        IEnumerable<Prosol_ServiceCategory> getcat();
        IEnumerable<Prosol_ServiceGroup> getgroupCode_Name();
        IEnumerable<Prosol_UOMMODEL> getsuom_Name();
      //  IEnumerable<Prosol_Users> get_approvercode_name_using_approverid2(string app);

        IEnumerable<Prosol_UOMMODEL> getuomlist();
        IEnumerable<Prosol_Attribute> GetAttributes();
        // IEnumerable<Prosol_Logic> GetValues(string Attribute);
        List<string> GetValues(string Noun,string Modifier, string Attribute);
        //defaultattr
        bool InsertDefaultAttr(Prosol_ServiceDefaultAttr data);
        IEnumerable<Prosol_ServiceDefaultAttr> ShwDefaultAttr();
        bool DeleteDefAttr(string id);
        IEnumerable<Prosol_ServiceDefaultAttr> getAttribute();
        ///atributeeee
       IEnumerable<Prosol_Abbrevate> GetAbbrList();
        Prosol_Attribute GetAttributeDetail(string Name);
      
        Prosol_MSAttribute GetCharacteristicvalues(string Name, string Activity);
        IEnumerable<Prosol_Abbrevate> GetAbbrList(string srchtxt);

        // bool addMS(Prosol_SSubCode data);
        string getusernameforrequest(string id);
        string getuserIDforrequest(string name);
        IEnumerable<Prosol_ServiceGroup> getSubList1(string MainCode);
        List<Prosol_RequestService> frstmat();
        List<Prosol_RequestService> searching1(string term);
        List<Prosol_RequestService> searching(string term);
        int InsertChildData(string[] data, string item2);
        List<Prosol_RequestService> srchchild(string term);
        IEnumerable<Prosol_RequestService> get_itemsToApprove(string userid);
        IEnumerable<Prosol_RequestService> getsingle_requested_record(string abcsony);
        IEnumerable<Prosol_RequestService> getappApproved_Records(string userid);
        IEnumerable<Prosol_RequestService> getreqApproved_Records(string userid);
        IEnumerable<Prosol_RequestService> getRejected_Records(string userid);
        IEnumerable<Prosol_RequestService> getreqRejected_Records(string userid);
        Prosol_Users getcatname(string Userid);
        bool submit_servcapproval(Prosol_RequestService pro_req, Prosol_UpdatedBy pub, string catname);
        IEnumerable<Prosol_MSAttribute> getAttribute(string Noun, string Modifier);

        string[] AutoSearchNoun(string term);
        Prosol_MSAttribute GetNounDetail(string Noun);
        IEnumerable<Prosol_MSAttribute> GetModifierList(string Noun);
        bool deleteRequest(string ItemId);
        bool insert_Rerequest(List<Prosol_RequestService> request);
        IEnumerable<Prosol_RequestService> getClarification_Records(string userid);
      //  IEnumerable<Prosol_Users> getcleansername(string sr);



    }
}
