using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.System.LitigationRequestContractTypeDtos;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices.System
{
    public class SysLitigationRequestContractTypeService : BasicService<SysLitigationRequestContractType, LitigationRequestContractType>, ISysLitigationRequestContractTypeService
    {
        public SysLitigationRequestContractTypeService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
        public async Task<BaseResponse> GetListAsync(CancellationToken cancellationToken)
        {
            return await base.GetListAsync<SysLitigationRequestContractTypeGetDto>(cancellationToken);
        }
        public async Task<BaseResponse> GetTermsConditionsAsync(CancellationToken cancellationToken)
        {
            return await base.GetListAsync<SysLitigationRequestContractTypeDto>(cancellationToken);
        }
    }
}
