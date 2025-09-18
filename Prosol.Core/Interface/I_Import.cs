using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Web;
using System.Data;

namespace Prosol.Core.Interface
{
    public partial interface I_Import
    {
        DataTable loaddata(HttpPostedFileBase file);
      DataTable downloadDup(DataTable dt_dup);

        bool save(IEnumerable<Prosol_Duplicate> listdup);

        IEnumerable<Prosol_Duplicate> show();

        IEnumerable<Prosol_Datamaster> checkitem();

        bool Import_submit(IEnumerable<Prosol_Datamaster> listimport);

        void delete();
    }
}
