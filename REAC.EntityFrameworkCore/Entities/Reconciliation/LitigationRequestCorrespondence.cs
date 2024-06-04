using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestCorrespondence : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public LitigationRequestCorrespondenceDestination LitigationRequestCorrespondenceDestinationId { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public virtual LitigationRequest LitigationRequest { get; set; } = null!;

    public virtual SysLitigationRequestCorrespondenceDestination LitigationRequestCorrespondenceDestination { get; set; } = null!;

    public virtual ICollection<LitigationRequestCorrespondenceFile> LitigationRequestCorrespondenceFiles { get; set; } = new List<LitigationRequestCorrespondenceFile>();
}
