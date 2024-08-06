using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Litigation
{
    public partial class LitigationRequestReformerAssigningRequest : BaseEntity<Guid>
    {
        public Guid LitigationRequestId { get; set; }
        public Guid CitizenId { get; set; }
        public LitigationRequestReformerAssigningRequestStatus Response { get; set; }
        public DateTime? ResponseOn { get; set; }
        public virtual LitigationRequest LitigationRequest { get; set; } = null!;
        public virtual Citizen ReformerCitizen { get; set; } = null!;

    }
}
