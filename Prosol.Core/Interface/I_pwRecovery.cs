using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;

namespace Prosol.Core.Interface
{
   public partial interface I_pwRecovery
    {
        IEnumerable<Prosol_Users> sendemail_forPR(string email);

        bool updatepassword(string pw, string userid,int rndm);

        bool saveRandom(string userid, int rndm);
    }
}
