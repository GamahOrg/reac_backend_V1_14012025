using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestMeetingReconciliationItemCreateUpdateDto : BaseEntity<Guid>
{
    public Guid LitigationRequestMeetingId { get; set; }

    public string ReconciliationItem { get; set; } = null!;

}
