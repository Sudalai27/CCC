using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public partial interface IItemRequestLog
    {
        IEnumerable<Prosol_Request> get_itemsToApprove(string userid);
        IEnumerable<Prosol_Request> getApproved_Records(string userid);
        IEnumerable<Prosol_Request> getRejected_Records(string userid);
        IEnumerable<Prosol_Request> getsingle_requested_record(string abcsony);

        IEnumerable<Prosol_Request> getClarification_Records(string userid);
    }
}
