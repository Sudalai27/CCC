using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;

namespace Prosol.Core.Interface
{
    public partial interface IItemApprove
    {
        IEnumerable<Prosol_Request> get_itemsToApprove(string userid);

        IEnumerable<Prosol_Request> getApproved_Records(string userid);

        IEnumerable<Prosol_Request> getRejected_Records(string userid);

        IEnumerable<Prosol_Request> getsingle_requested_record(string abcsony);
        IEnumerable<Prosol_Users> getcataloguernames_id();
        bool submit_approval(Prosol_Request pro_req, Prosol_UpdatedBy pub);

        IEnumerable<Prosol_Users> get_cataloguer_emailid(string userid);

        IEnumerable<Prosol_Users> get_req(string usrname);
       
    }
}
