namespace REAC.Application.Contracts.Dtos.Wpf.Business
{
    public class AssignRequestPayload
    {
        public Guid RequestId { get; set; }

        public Guid SourceEmployeeId { get; set; }

        public Guid DestinationEmployeeId { get; set; }
    }
}
