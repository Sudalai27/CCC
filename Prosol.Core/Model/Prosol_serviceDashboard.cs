using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_serviceDashboard
    {
        public string PlantCode { get; set; }
        public string PlantName { get; set; }       
        public int TotalItems { get; set; }
        public int CompletedItems { get; set; }
        //basedon category
        public string MainCategoryCode { get; set; }
        public string MainCategoryName { get; set; }
        public int TotalCategory { get; set; }
        public int CompletedCategory { get; set; }

       // public List<Prosol_RequestService> DataList { get; set; }
       
            //based on group
       public string ServiceGroupCode { get; set; }
        public string ServiceGroupName { get; set; }
        public int TotalService { get; set; }
        public int CompletedService { get; set; }



    }
}
