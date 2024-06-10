using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.Dtos.System.LitigationRequestMeetingDecisionDtos;

public partial class SysLitigationRequestMeetingDecisionDto : BaseEntity<LitigationRequestMeetingDecision>
{
    public string MeetingDecision { get; set; } = null!;

}
