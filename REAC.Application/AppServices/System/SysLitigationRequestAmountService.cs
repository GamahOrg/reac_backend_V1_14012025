using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.EntityFrameworkCore.Entities.System;


namespace REAC.Application.AppServices.System
{
    public class SysLitigationRequestAmountService : BasicService<SysLitigationRequestAmount, Guid>, ISysLitigationRequestAmountService
    {
        public SysLitigationRequestAmountService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
        public async Task<string?> GetLitigationRequestAmount(Guid LitigationRequestAmountId)
        {
            var quarable = await GetQueryableAsync();
            var litigationRequestAmount = quarable.FirstOrDefault(x => x.Id == LitigationRequestAmountId)?.LitigationAmount;
            return litigationRequestAmount;
        }
    }
}
