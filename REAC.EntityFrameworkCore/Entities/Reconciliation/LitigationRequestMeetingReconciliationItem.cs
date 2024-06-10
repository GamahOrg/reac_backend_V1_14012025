using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestMeetingReconciliationItem : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestMeetingId { get; set; }

    public string ReconciliationItem { get; set; } = null!;

    public virtual LitigationRequestReconciliationMeeting LitigationRequestMeeting { get; set; } = null!;
}
