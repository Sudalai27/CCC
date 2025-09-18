using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public partial interface IEmailservice
    {
      //  bool sendmail(email email);
        bool sendmail(string to_mail, string subjectt, string body);
    }
}
