using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Web;

namespace Prosol.Core.Interface
{
    public partial interface I_ItemRequest
    {
       IEnumerable<Prosol_Plants> getplantCode_Name();
        IEnumerable<Prosol_Master> getStorageCode_Name(string value);
        List<Prosol_Datamaster> getpossibledup(string term);
        IEnumerable<Prosol_GroupCodes> getgroupCode_Name();

        IEnumerable<Prosol_SubGroupCodes> getsubgroupCode_Name(string str);

        IEnumerable<Prosol_Users> get_approvercode_name();

        bool newRequest(Prosol_Request req_model);

        IEnumerable<Prosol_Users> get_approvercode_name_using_approverid(string app);
        int getlast_request_R_no(string str);

        bool insert_multiplerequest(List<Prosol_Request> request, string reqid, HttpFileCollectionBase file);

        bool deleteRequest(string Itemid);
        List<Bulkrequest_load> loaddata(HttpPostedFileBase file);
        IEnumerable<Prosol_Master> getsl();
        IEnumerable<Prosol_Master> findmasterdata();
        IEnumerable<Prosol_SubGroupCodes> getsgcode_Name();

        bool insert_Rerequest(List<Prosol_Request> request, HttpFileCollectionBase file);
        IEnumerable<Prosol_Master> getslname(string sl);


    }
}
