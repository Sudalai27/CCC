using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core
{
   public class SequenceService: ISequence
    {

        private readonly IRepository<Prosol_Sequence> _SequenceRepository;
        private readonly IRepository<Prosol_UOMSettings> _UOMSettingsRepository;

        public SequenceService(IRepository<Prosol_Sequence> seqRepository, IRepository<Prosol_UOMSettings> uomRepository)
        {
            this._SequenceRepository = seqRepository;
            this._UOMSettingsRepository = uomRepository;

        }


        //UOM
        public virtual bool CreateSequence(List<Prosol_Sequence> seqList)
        {
          
            var res = false;
            foreach (Prosol_Sequence seq in seqList)
            {
                seq.UpdatedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                var query = Query.And(Query.EQ("Category", seq.Category), Query.EQ("Description", seq.Description));
                var um = _SequenceRepository.FindAll(query).ToList();
                if (um.Count == 1 && um[0]._id == seq._id)
                {
                    res = _SequenceRepository.Add(seq);
                }
            }
            return res;
        }
        public virtual IEnumerable<Prosol_Sequence> GetSequenceList()
        {
            var sort = SortBy.Ascending("Seq").Ascending("Description");
            var seqList = _SequenceRepository.FindAll(sort);
            return seqList;
        }
        public virtual bool CreateUOMSettings(Prosol_UOMSettings xset)
        {

            var res = false;
            res = _UOMSettingsRepository.Add(xset);
            return res;
        }
        public virtual Prosol_UOMSettings GetUOMSettings()
        {          
            var seqList = _UOMSettingsRepository.FindOne();
            return seqList;
        }
    }
}
