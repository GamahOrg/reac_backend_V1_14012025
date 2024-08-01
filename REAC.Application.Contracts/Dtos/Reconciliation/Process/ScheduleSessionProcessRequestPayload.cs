namespace REAC.Application.Contracts.Dtos.Reconciliation.Process
{
    public class ScheduleSessionProcessRequestPayload
    {
        public Guid RequestId { get; set; }
        public DateTime SessionDateTime { get; set; }
    }
}
