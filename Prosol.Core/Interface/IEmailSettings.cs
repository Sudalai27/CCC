using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
   public interface IEmailSettings
    {
        bool insertemaildata(Prosol_EmailSettings data, int update);
        bool sendmail(string to_mail, string subjectt, string body);
        string emailTest(string to_mail, string subjectt, string body);
        Prosol_EmailSettings ShowEmaildata();
        string[] AutoCompleteEmailId(string term);
        // IEnumerable<Prosol_Request> getmailbody(string[] m_mul_req_values);
        string getmailbody(DataTable tbl);
    }
}
