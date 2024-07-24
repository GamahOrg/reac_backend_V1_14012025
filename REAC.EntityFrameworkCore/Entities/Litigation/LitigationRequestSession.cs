using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestSession : BaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }
    public DateTime SessionDatetime { get; set; }
    public string? SessionUrl { get; set; }
    public virtual LitigationRequest LitigationRequest { get; set; } = null!;

}
