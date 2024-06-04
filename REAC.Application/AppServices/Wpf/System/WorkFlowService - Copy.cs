using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices.Wpf.System
{
    public class SysExemptionReductionCategoryService : BasicService<SysExemptionReductionCategory, Guid>, ISysExemptionReductionCategoryService
    {
        public SysExemptionReductionCategoryService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
    }
}
