using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
   public interface ISequence
    {
        //UOM
        bool CreateSequence(List<Prosol_Sequence> seqList);
        IEnumerable<Prosol_Sequence> GetSequenceList();

        bool CreateUOMSettings(Prosol_UOMSettings xSet);
        Prosol_UOMSettings GetUOMSettings();

    }
}
