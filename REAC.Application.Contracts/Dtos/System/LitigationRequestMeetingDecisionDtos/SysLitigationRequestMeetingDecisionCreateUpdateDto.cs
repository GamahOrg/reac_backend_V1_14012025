namespace REAC.Application.Contracts.Dtos.System.LitigationRequestMeetingDecisionDtos;

public partial class SysLitigationRequestMeetingDecisionCreateUpdateDto : SysLitigationRequestMeetingDecisionDto
{
    public virtual ICollection<SysLitigationRequestMeetingDecisionTranslationCreateUpdateDto> Translations { get; set; } = new List<SysLitigationRequestMeetingDecisionTranslationCreateUpdateDto>();
}
