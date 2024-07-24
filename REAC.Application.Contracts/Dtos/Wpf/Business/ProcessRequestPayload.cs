using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Application.Contracts.Dtos.Wpf.Business
{
    public class ProcessRequestPayload
    {
        public Guid RequestId { get; set; }
        public Guid ProcessingLogId { get; set; }
        public Guid WorkflowStepEventId { get; set; }
        public string? Notes { get; set; }
        public List<Guid>? UploadedFilesIds { get; set; }
    }
    public class ScheduleSessionProcessRequestPayload
    {
        public Guid RequestId { get; set; }
        public Guid ProcessingLogId { get; set; }
        public Guid WorkflowStepEventId { get; set; }
        public string? Notes { get; set; }
        public List<Guid>? UploadedFilesIds { get; set; }
        public DateTime SessionDateTime { get; set; }
    }
    public class ReScheduleSessionProcessRequestPayload
    {
        public Guid RequestId { get; set; }
        public Guid ProcessingLogId { get; set; }
        public Guid WorkflowStepEventId { get; set; }
        public string? Notes { get; set; }
        public List<Guid>? UploadedFilesIds { get; set; }
        public DateTime SessionDateTime { get; set; }
    }
    public class WritingMinutesOfTheMeetingRequestPayload
    {
        public Guid RequestId { get; set; }
        public Guid ProcessingLogId { get; set; }
        public Guid WorkflowStepEventId { get; set; }
        public string? Notes { get; set; }
        public List<Guid>? UploadedFilesIds { get; set; }
        public string ReconciliationSessionDetails { get; set; } = null!;

        public string? ModificationRequest { get; set; } = null!;

        public virtual ICollection<LitigationRequestReconciliationReportItemCreateUpdateDto> LitigationRequestMeetingReconciliationItems { get; set; } = new List<LitigationRequestReconciliationReportItemCreateUpdateDto>();
    }
    public class ExtendingReconciliationPeriodRequestPayload
    {
        public Guid RequestId { get; set; }
        public Guid ProcessingLogId { get; set; }
        public Guid WorkflowStepEventId { get; set; }
        public string? Notes { get; set; }
        public List<Guid>? UploadedFilesIds { get; set; }

        public int RequiredDays { get; set; }

        public string Reason { get; set; } = null!;

        public string? FinishStatusReason { get; set; }

        public virtual ICollection<Guid> LitigationRequestExtendPeriodRequestFiles { get; set; } = new List<Guid>();

    }



}
