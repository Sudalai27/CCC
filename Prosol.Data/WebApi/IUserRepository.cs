using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;

namespace Prosol.WebApi
{
    public interface IUserRepository
    {
        string AuthenUr(string Name);
        IEnumerable<ApiModel> Getstock(string uNme);
        //  User Get(string customerID);
        // User Add(User item);
        // bool Remove(string customerID);
        string Updatestock(List<ApiModel> MobileReturnList);
        IEnumerable<RfidModel> Getstockrfid();
        string insertIN(List<IOModel> List);
        string insertOUT(List<IOModel> List1);
    }
}
