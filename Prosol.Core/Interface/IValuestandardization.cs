using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Prosol.Core.Model;

namespace Prosol.Core.Interface
{
    public partial interface IValuestandardization
    {
        IEnumerable<Prosol_NounModifiers> GetNoun();
        IEnumerable<Prosol_NounModifiers> GetModifier(string noun);
        IEnumerable<Prosol_Charateristics> GetAttributes(string noun, string modifier);
        IEnumerable<Prosol_Datamaster> load_values(string noun, string modifier, string attribute);
        int update_values(string noun, string modifier, string attribute, string value, string newvalue,string UOM,string newUOM);
        int Delete_values(string noun, string modifier, string attribute, string value);
        

    }
}
