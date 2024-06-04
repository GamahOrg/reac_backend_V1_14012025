using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Reconciliation;

public partial class LitigationRequestReconciliationMeetingCreateUpdateDto : BaseEntity<Guid>
{
    public Guid LitigationRequestId { get; set; }

    public DateTime MeetingDate { get; set; }

    public Guid LitigationRequestMeetingConfigurationId { get; set; }

    public int LitigationRequestMeetingStatusId { get; set; }

    public string? ReconciliationMinutes { get; set; }

    public int? LitigationRequestMeetingDecisionId { get; set; }

    public bool? IsFirstPartyApprove { get; set; }

    public DateTime? FirstPartyApprovedOn { get; set; }

    public bool? IsSecondPartyApprove { get; set; }

    public DateTime? SecondPartyApprovedOn { get; set; }

}
