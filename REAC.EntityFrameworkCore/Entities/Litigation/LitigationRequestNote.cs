using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestNote : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public string? Title { get; set; }

    public string Notes { get; set; } = null!;

    public virtual LitigationRequest LitigationRequest { get; set; } = null!;

    public virtual ICollection<LitigationRequestNoteFile> LitigationRequestNoteFiles { get; set; } = new List<LitigationRequestNoteFile>();
}
