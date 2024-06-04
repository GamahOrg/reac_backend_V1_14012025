using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;

namespace REAC.Application.AppServices.Wpf.Business
{
    public class WorkFlowProcessingLogService : BasicService<WorkFlowProcessingLog, Guid>, IWorkFlowProcessingLogService
    {
        public WorkFlowProcessingLogService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {

        }
    }
}
