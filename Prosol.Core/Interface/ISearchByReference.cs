using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prosol.Core.Model;

namespace Prosol.Core.Interface
{
    public partial interface ISearchByReference
    {
        IEnumerable<Prosol_Datamaster> getresultsforcode(string Code);

        IEnumerable<Prosol_Datamaster> getresultsforrest(string Noun, string Modifier, string Industry_Sector, string Material_Type, string Inspection_Type, string Plant, string Profit_Center, string Storage_Location, string Storage_Bin, string Valuation_Class, string Price_Control, string MRP_Type, string MRP_Controller, string Procurement_Type);

        IEnumerable<Prosol_Plants> GetPlants();

        IEnumerable<Prosol_Master> GetStorageLocations(string Plant);

        IEnumerable<Prosol_Master> GetStorageBin(string Plant, string sl);

        IEnumerable<Prosol_Datamaster> GetResultForCode(string code);
        IEnumerable<Prosol_Datamaster> GetResultFornoun_modifier(string noun, string modifier);

    }
}
