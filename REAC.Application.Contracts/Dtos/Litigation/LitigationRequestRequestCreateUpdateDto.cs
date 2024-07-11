using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestRequestCreateUpdateDto : BaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }
    public SysFileManagerCreateUpdateDto Request { get; set; }
    public string RequestContent { get; set; } = null!;
}
