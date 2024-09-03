using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.Dtos.Citizen
{
    public class ReformerStopDecisionCreateDto
    {
        public Guid CitizenId { get; set; }
        public DateTime DecisionDate { get; set; }
        public StoppingReason? StoppingReason { get; set; }
        public string? LateLitigationRequestNumber { get; set; }
    }
}
