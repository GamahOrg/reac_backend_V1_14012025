using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestExtendPeriodRequestCreateUpdateDto : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public int RequiredDays { get; set; }

    public string Reason { get; set; } = null!;

    public Guid RequestedBy { get; set; }

    public int LitigationRequestExtendPeriodRequestStatusId { get; set; }

    public DateTime? FinishedOn { get; set; }

    public Guid? FinishedBy { get; set; }

    public string? FinishStatusReason { get; set; }

}
