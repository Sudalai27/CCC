using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Reject_Service
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Reject_reason { get; set; }
        public string Rejected_as { get; set; }
    }
}
