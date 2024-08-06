using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Arbitration;

public partial class LitigationRequestArbitrator : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public Guid ArbitratorCitizenId { get; set; }

    public virtual LitigationRequest LitigationRequest { get; set; } = null!;
    public virtual Citizen ArbitratorCitizen { get; set; } = null!;

}
