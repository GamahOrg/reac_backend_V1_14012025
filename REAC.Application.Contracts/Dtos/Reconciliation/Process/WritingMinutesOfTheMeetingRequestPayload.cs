using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Application.Contracts.Dtos.Reconciliation.Process
{
    public class WritingMinutesOfTheMeetingRequestPayload
    {
        public Guid RequestId { get; set; }
        public string ReconciliationSessionDetails { get; set; } = null!;
        public string? ModificationRequest { get; set; } = null!;
        public List<Guid>? UploadedFilesIds { get; set; }
        public virtual ICollection<LitigationRequestReconciliationReportItemCreateUpdateDto> LitigationRequestMeetingReconciliationItems { get; set; } = new List<LitigationRequestReconciliationReportItemCreateUpdateDto>();
    }
}
