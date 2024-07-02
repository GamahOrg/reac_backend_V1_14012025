using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.IAppServices.System;



namespace REAC.Application.AppServices.System
{
    public class SysLitigationRequestMeetingDecisionService : BasicService<SysLitigationRequestMeetingDecision, LitigationRequestMeetingDecision>, ISysLitigationRequestMeetingDecisionService
    {
        public SysLitigationRequestMeetingDecisionService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
    }
}
