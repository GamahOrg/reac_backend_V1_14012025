using Core.GenericRepository.Shared.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.EntityFrameworkCore.Entities.Litigation
{
    public class LitigationArea : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public List<LitigationCity> Cities { get; set; }
        public ICollection<LitigationRequest> LitigationRequests { get; set; }

    }
}
