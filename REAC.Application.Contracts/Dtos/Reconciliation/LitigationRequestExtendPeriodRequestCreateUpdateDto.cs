using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestExtendPeriodRequestCreateUpdateDto : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public int RequiredDays { get; set; }

    public string RequestReason { get; set; } = null!;

    public int LitigationRequestExtendPeriodRequestStatusId { get; set; }

    public string? FinishStatusReason { get; set; }

    public List<Guid> Files = new List<Guid>();

}
