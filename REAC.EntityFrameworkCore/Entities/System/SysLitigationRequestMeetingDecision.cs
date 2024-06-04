using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class SysLitigationRequestMeetingDecision : BaseEntity<LitigationRequestMeetingDecision>
{
    public string MeetingDecision { get; set; } = null!;
    public virtual ICollection<SysLitigationRequestMeetingDecisionTranslation> Translations { get; set; } = new List<SysLitigationRequestMeetingDecisionTranslation>();


    public virtual ICollection<LitigationRequestReconciliationMeeting> LitigationRequestMeetings { get; set; } = new List<LitigationRequestReconciliationMeeting>();
}
