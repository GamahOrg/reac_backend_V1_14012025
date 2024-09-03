using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestCorrespondenceCreateUpdateDto : BaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public int LitigationRequestCorrespondenceDestinationId { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

}
