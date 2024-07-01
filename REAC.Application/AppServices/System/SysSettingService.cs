using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Localizations;
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

        public async Task<DataResponse<List<SysSetting>>> GetByAllSetting()
        {
            var SettingObj = (_repository.GetQueryable<SysSetting, Guid>()).Result.ToList();
            if (SettingObj != null)
                return new DataResponse<List<SysSetting>> { Data = SettingObj, IsDone = true };
            else
                return new DataResponse<List<SysSetting>> { Data = null, IsDone = false };
        }

        public async Task<DataResponse<SysSetting>> UpdateBySettingKey(SysSetting updateSetting)
        {
            _repository.Update<SysSetting, Guid>(updateSetting);
            await _repository.SaveChangesAsync();
            return new DataResponse<SysSetting> { Data = updateSetting, IsDone = true ,Message = Localizations.UpdateSuccefully};
        }
    }
}
