using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class ItemStatusMap
    {
        public string RequestId { get; set; }
        public string Itemcode { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Date { get; set; }
        public int Status { get; set; }      

    }
}
