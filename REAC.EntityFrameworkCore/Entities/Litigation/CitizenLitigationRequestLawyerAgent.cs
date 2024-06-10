using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.EntityFrameworkCore.Entities.Litigation
{
    public class CitizenLitigationRequestLawyerAgent : BaseEntity<Guid>  // bridge table
    {
        public Guid CitizenId { get; set; } 
        public Citizen Citizen { get; set; } 
        public Guid LitigationRequestLawyerAgentId { get; set; }
        public LitigationRequestLawyerAgent LitigationRequestLawyerAgent { get; set; } 
    }
}
