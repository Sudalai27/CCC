using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Prosol.Core.Interface
{
    public interface IBusinessPartner
    {

        //Group codes
        bool InsertData(Prosol_BPMaster data);
        IEnumerable<Prosol_BPMaster> GetDataList(string Label);
        IEnumerable<Prosol_BPGenralModel> GenList();
        IEnumerable<Prosol_BPCustomerModel> CustList();
        IEnumerable<Prosol_BPVendorModel> VenList();
        bool DelData(string id);

        IEnumerable<Prosol_BPMaster> GetMaster();
        bool DisableData(string id, bool sts);

        bool CreateGen(Prosol_BPGenralModel data);
        bool CreateCust(Prosol_BPCustomerModel data);
        bool CreateVen(Prosol_BPVendorModel data);
        bool custdel(string id);
        bool venDel(string id);
        bool UpdateGen(Prosol_BPGenralModel general, string id);
        bool UpdateVen(Prosol_BPVendorModel mdl, string id);
        bool UpdateCust(Prosol_BPCustomerModel mdl, string id);
    }
}
