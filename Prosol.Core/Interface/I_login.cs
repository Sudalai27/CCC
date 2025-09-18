using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;

namespace Prosol.Core.Interface
{
    public partial interface I_login
    {
        IEnumerable<Prosol_Users> checklogin_details(string Username, string Password);
        void UpdateLoginDate(string uid);


        void UpdateLogOutDate(string uid);

        List<Prosol_Idealtime> getIdeal(string uid);
    }
}
