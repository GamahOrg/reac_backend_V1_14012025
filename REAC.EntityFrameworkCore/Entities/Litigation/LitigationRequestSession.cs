using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestSession : BaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }
    public DateTime SessionDatetime { get; set; }
    public LitigationRequestSessionRequestType LitigationRequestSessionRequestTypeId { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? SessionUrl { get; set; }
    public virtual LitigationRequest LitigationRequest { get; set; } = null!;

}
