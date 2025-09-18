using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;

namespace Prosol.Core.Interface
{
    public partial interface IUserAccess
    {
        IEnumerable<Prosol_Users> getuser();
        IEnumerable<Prosol_Pages> getpages();

        IEnumerable<Prosol_Access> search(string id);

        bool pagelimit(string id,string pagename); 

        bool delete(string id);

        bool submit(Prosol_Access acs);

        IEnumerable<Prosol_Users> AutoSearchUserName(string term);
    }
}
