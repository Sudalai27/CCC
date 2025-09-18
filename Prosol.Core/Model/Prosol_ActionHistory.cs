using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_ActionHistory
    {
        public string UserName { get; set; }
        public string Plant { get; set; }
        public string UserId { get; set; }     
        public string Role { get; set; }
        public DateTime Datetime { get; set; }
        public string ActionPerformed { get; set; }
        //ItemHistoryGO
        //wrote by saikat chowdhury 07/01/2019
        public DateTime CreatedOn { get; set; }

        public int Pending { get; set; }
        public int Completed { get; set; }
        public int total { get; set; }

        public int Clarification { get; set; }
        //public string UserName { get; set; }

    }
}
