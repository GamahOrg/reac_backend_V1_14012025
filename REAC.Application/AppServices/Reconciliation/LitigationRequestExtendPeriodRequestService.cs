using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Reconciliation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;



namespace REAC.Application.AppServices.Reconciliation
{
    public class LitigationRequestExtendPeriodRequestService : BasicService<LitigationRequestExtendPeriodRequest, Guid>, ILitigationRequestExtendPeriodRequestService
    {
        public LitigationRequestExtendPeriodRequestService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
    }
}
