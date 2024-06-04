using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestReconciliationMeeting : AuditedBaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public DateTime MeetingDate { get; set; }

    public Guid LitigationRequestMeetingConfigurationId { get; set; }

    public int LitigationRequestMeetingStatusId { get; set; }

    public string? ReconciliationMinutes { get; set; }

    public LitigationRequestMeetingDecision? LitigationRequestMeetingDecisionId { get; set; }

    public bool? IsFirstPartyApprove { get; set; }

    public DateTime? FirstPartyApprovedOn { get; set; }

    public bool? IsSecondPartyApprove { get; set; }

    public DateTime? SecondPartyApprovedOn { get; set; }

    public virtual LitigationRequest LitigationRequest { get; set; } = null!;

    public virtual SysLitigationRequestMeetingConfiguration LitigationRequestMeetingConfiguration { get; set; } = null!;

    public virtual SysLitigationRequestMeetingDecision? LitigationRequestMeetingDecision { get; set; }

    public virtual ICollection<LitigationRequestMeetingReconciliationItem> LitigationRequestMeetingReconciliationItems { get; set; } = new List<LitigationRequestMeetingReconciliationItem>();

}
