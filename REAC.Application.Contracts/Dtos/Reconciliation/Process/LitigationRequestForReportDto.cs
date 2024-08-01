using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Application.Contracts.Dtos.Reconciliation.Process
{
    public class LitigationRequestForReportDto
    {
        public bool AllowAdd { get; set; }
        public LitigationRequestShortDto? LitigationRequestShortDto { get; set; }
        public LitigationRequestReconciliationReportCreateUpdateDto? Report { get; set; }
    }

    public class LitigationRequestForExtendPeriodRequestDto
    {
        public bool AllowAdd { get; set; }
        public LitigationRequestShortDto? LitigationRequestShortDto { get; set; }
        public LitigationRequestExtendPeriodRequestCreateUpdateDto? LitigationRequestExtendPeriodRequestCreateUpdateDto { get; set; }
    }

    public class LitigationRequestForReportApprovalDto
    {
        public bool AllowAdd { get; set; }
        public LitigationRequestShortDto? LitigationRequestShortDto { get; set; }

        public MinutesOfMeetingApprovalStatus? minutesOfMeetingApprovalStatus { get; set; }
        public DateTime? minutesOfMeetingApprovalStatusOn { get; set; }
    }
}
