using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class SysLitigationRequestMeetingConfiguration : FullAuditedBaseEntity<Guid>
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public virtual ICollection<LitigationRequestReconciliationMeeting> LitigationRequestMeetings { get; set; } = new List<LitigationRequestReconciliationMeeting>();
}
