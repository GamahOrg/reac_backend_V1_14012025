using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestMircoDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string IsDone { get; set; } = null!;

}
