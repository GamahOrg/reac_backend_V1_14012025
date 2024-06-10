using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.EntityFrameworkCore.Entities.System;



namespace REAC.Application.AppServices.System
{
    public class SysSettingService : BasicService<SysSetting, Guid>, ISysSettingService
    {
        public SysSettingService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _repository = repository;
        }
        public async Task<DataResponse<SysSetting>> GetBySettingKey(string SettingKey)
        {
            var SettingObj = (_repository.GetQueryable<SysSetting, Guid>())
                            .Result?.Where(x => x.Key == SettingKey)?.FirstOrDefault();
            if (SettingObj != null)
                return new DataResponse<SysSetting> { Data = SettingObj, IsDone = true };
            else
                return new DataResponse<SysSetting> { Data = null, IsDone = false };

        }
    }
}
