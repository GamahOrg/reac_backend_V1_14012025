using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.EntityFrameworkCore.Entities.Litigation
{
    public class LitigationRequestLawyerAgent :  BaseEntity<Guid> 
    {
        public string? AgencyNumber { get; set; }
        public string? LawLicenseNumber { get; set; }
        public DateTime? LawLicenseExpirationDate { get; set; }
        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}
