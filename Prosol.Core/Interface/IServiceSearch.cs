using Prosol.Core.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public partial interface IServiceSearch
    {
        List<Prosol_ServiceCategory> getcategory();
        IEnumerable<Prosol_ServiceGroup> getgroupp(string ServiceCategorycode);
        List<Prosol_ServiceUom> getUOM();
        IEnumerable<Prosol_RequestService> getServiceCode(string ServiceCategoryCode, string ServiceGroupCode, string UomName);
        IEnumerable<Prosol_RequestService> gettabledetails(string ServiceCode);
        IEnumerable<Prosol_RequestService> getdetailsforcode(string code);
        IEnumerable<Prosol_RequestService> getdetailsforsd(string sd);
        IEnumerable<Prosol_RequestService> getdetailsforld(string ld);
        IEnumerable<Prosol_RequestService> ServiceMasterSearch(string search, string searchkey);
        //dashboard
        //   Prosol_Users getimage(string id);
        List<Prosol_serviceDashboard> BindTotalItem();
        List<Prosol_serviceDashboard> BindTotalItemcategory();
        List<Prosol_serviceDashboard> BindTotalItemcategoryGroup();
    }
}
