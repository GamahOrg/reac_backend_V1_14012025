using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestExtendPeriodRequestFileCreateUpdateDto : BaseEntity<Guid>
{
    public Guid LitigationRequestExtendPeriodRequestId { get; set; }

    public Guid FileManagerId { get; set; }

}
