using REAC.Application.Contracts;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.IAppServices.System
{
    public interface ISysLitigationRequestMeetingDecisionService : IBasicService<SysLitigationRequestMeetingDecision, LitigationRequestMeetingDecision>
    {
    }
}
