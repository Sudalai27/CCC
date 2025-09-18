using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class email
    {
        public string email_to { get; set; }
        public string email_from { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string host { get; set; }
        public bool enablessl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public int Port { get; set; }

        public string password { get; set; }
    }
}
