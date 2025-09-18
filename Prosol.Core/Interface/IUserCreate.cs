using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Web;

namespace Prosol.Core.Interface
{
    public partial interface IUserCreate
    {

        IEnumerable<Prosol_Users> getuser(string[] plants);
        //IEnumerable<Prosol_Pages> getpages();

        IEnumerable<Prosol_Rolepage> getrolepage(string Role,string Module);

        IEnumerable<Prosol_Plants> getplant();
        IEnumerable<Prosol_Departments> getdepartment();

        IEnumerable<Prosol_Users> getforupdate(string id);

        IEnumerable<Prosol_Users> showall_user();
        IEnumerable<Prosol_Users> showall_user(string[] plant);

        int getmaxid();
        bool save(Prosol_Users usrcm);

        bool setforupdate(string id, Prosol_Users userup);

        IEnumerable<Prosol_Users> checkusername_avalibility(string UserName);

        IEnumerable<Prosol_Users> checkemailid_avalibility(string EmailID);
        IEnumerable<Prosol_Users> getReviewerList();

         Prosol_Users getimage(string id);
        Prosol_Users getcleansername(string id);
        IEnumerable<Prosol_Users> AccountUser(string id);

        bool Changepasswordsubmit(string id, string Password);

        bool Profilesubmit(string id, Prosol_Users prf, HttpPostedFileBase file);

        List<ItemStatusMap> getItemstatusmap(string Itemcode);
        Prosol_Users Getreqinfo(string id);
        //IEnumerable<Prosol_Users> getappname(string id);

      
    }
}
