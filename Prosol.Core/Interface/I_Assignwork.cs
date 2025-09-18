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
    public partial interface I_Assignwork
    {
        IEnumerable<Prosol_Datamaster> loaddata();

        IEnumerable<Prosol_Datamaster> multicode_search(string codestr);

        IEnumerable<Prosol_Datamaster> multicode_reassignsearch(string role, string username);

        bool reassign_submit(string item, string role, Prosol_Datamaster reasgn);

        IEnumerable<Prosol_Datamaster> check_item(string itemcode);

        IEnumerable<Prosol_Users> getuser(string role, string[] plants);
        IEnumerable<Prosol_Users> userdetails(string username);
        IEnumerable<Prosol_Plants> plnt(string Plantcode);
        IEnumerable<Prosol_Datamaster> reloaddata(string role, string username);

        IEnumerable<Prosol_Users> getuseronly(string username);

        bool assign_submit(string item, Prosol_Datamaster pd);

        bool download(List<Prosol_Datamaster> selecteditem);

        IEnumerable<Prosol_Users> AutoSearchUserName(string term);
        //pvdata
        IEnumerable<Prosol_Datamaster> loaddata1();
        IEnumerable<Prosol_Users> getuserpv(string role);
        bool PVUSER(string item, string role, Prosol_Datamaster reasgn);
    }
}
