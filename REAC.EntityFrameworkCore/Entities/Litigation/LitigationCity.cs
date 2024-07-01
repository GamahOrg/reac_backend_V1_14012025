using Core.GenericRepository.Shared.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.EntityFrameworkCore.Entities.Litigation
{
    public class LitigationCity : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public Guid AreaId { get; set; }
        public LitigationArea Area { get; set; }
        public ICollection<LitigationRequest> LitigationRequests { get; set; }

    }
}
