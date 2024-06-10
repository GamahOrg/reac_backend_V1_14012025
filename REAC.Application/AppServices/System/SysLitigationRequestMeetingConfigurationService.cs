using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.EntityFrameworkCore.Entities.System;



namespace REAC.Application.AppServices.System
{
    public class SysLitigationRequestMeetingConfigurationService : BasicService<SysLitigationRequestMeetingConfiguration, Guid>, ISysLitigationRequestMeetingConfigurationService
    {
        public SysLitigationRequestMeetingConfigurationService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
    }
}
