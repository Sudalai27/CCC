using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public partial interface IMaster
    {
        bool InsertData(Prosol_Master data);      
        IEnumerable<Prosol_Master> GetDataList(string Label);
        bool DelData(string id);

        IEnumerable<Prosol_Master> GetMaster();
        bool DisableData(string id, bool sts);
        //bin
        IEnumerable<Prosol_Master> getbincode(string lable, string StorageLocation);

        //plant
        bool InsertDataplnt(Prosol_Plants data);
        IEnumerable<Prosol_Plants> GetDataListplnt();
        bool DelDataplnt(string id);
        IEnumerable<Prosol_Plants> GetMasterplnt();
        bool DisablePlant(string id, bool sts);

        //department
        bool InsertDatawithdept(Prosol_Departments data);
        IEnumerable<Prosol_Departments> GetDataListdept();
        bool DelDatadept(string id);
        IEnumerable<Prosol_Departments> GetMasterdept();
        bool DisableDept(string id, bool sts);

        bool InsertDatawithplant(Prosol_Master data);
        IEnumerable<Prosol_Master> getstoragelocation(string plant);
        bool InsertDatawithstorage(Prosol_Master data);
         IEnumerable<Prosol_Master> GetDataListstorage(string Label);
        bool DelDatastorage(string id);
         IEnumerable<Prosol_Master> GetMasterstorage();
        IEnumerable<Prosol_ServiceCategory> Getcategory();



    }

    
}
