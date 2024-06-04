using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestNoteCreateUpdateDto : BaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public string? Title { get; set; }

    public string Notes { get; set; } = null!;

}
