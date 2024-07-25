using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.EntityFrameworkCore.Entities.Notification;

namespace REAC.Application.AppServices.System
{
    public partial class NotificationPlanService : BasicService<SysNotificationPlan, Guid>, ISysNotificationPlanService
    {
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;
        private readonly ISysFileManagerService _fileService;
        public NotificationPlanService(ISysFileManagerService fileService, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen) : base(repository, mapper, loggerManager, jwtReader)
        {
            _fileService = fileService;
            _jwtReader = jwtReader;
            _mapper = mapper;
            _citizen = citizen;
        }

        public async Task<BaseResponse> ExecuteNotificationPlan(Guid notificationPlanId)
        {
            var notificationPlan = (await _repository.GetQueryable<SysNotificationPlan, Guid>())
                                    .Include(x => x.SysNotificationPlanItems)
                                    .ThenInclude(x => x.SysNotificationPlanItemDestinations)
                                    .Include(x => x.SysNotificationPlanItems)
                                    .ThenInclude(x => x.SysDepartment)
                                    .ThenInclude(x => x.SysEmployees)
                                    .Where(x => x.Id == notificationPlanId).ToList().FirstOrDefault();



            throw new NotImplementedException();
        }
    }
}
