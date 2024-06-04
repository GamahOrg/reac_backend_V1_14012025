namespace REAC.Application.Contracts.Dtos.Wpf.Business
{
    public class ProcessRequestPayload
    {
        public Guid RequestId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid ProcessingLogId { get; set; }
        public Guid WorkflowStepEventId { get; set; }
        public string? Notes { get; set; }
        public List<Guid>? UploadedFilesIds { get; set; }
    }
}
