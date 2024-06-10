using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Reconciliation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;


namespace REAC.Application.Services.Reconciliation
{
    public class LitigationRequestCorrespondenceFileService : BasicService<LitigationRequestCorrespondenceFile, Guid>, ILitigationRequestCorrespondenceFileService
    {
        public LitigationRequestCorrespondenceFileService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
    }
}
