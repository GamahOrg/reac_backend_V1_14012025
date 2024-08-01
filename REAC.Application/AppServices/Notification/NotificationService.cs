using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.Pagging;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Notification;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Common;
using REAC.Application.Contracts.IAppServices.Notification;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Notification;

namespace REAC.Application.AppServices.Notification
{
    public class NotificationService : BasicService<SysNotification, Guid>, INotificationService
    {
        private readonly IJsonStringLocalizer _localization;

        public NotificationService(IEntityRepository repository,
            IMapper mapper,
            ILoggerManager loggerManager,
            IJsonStringLocalizer localization,
            IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _localization = localization;
        }


        public async Task<BaseResponse> GetNotificationList(Guid actorId, NotificationActorType actorTypeId)
        {
            var query = await _repository.GetQueryable<SysNotification, Guid>();

            query = query.Where(x => x.ActorId == actorId && x.NotificationActorTypeId == actorTypeId);


            var notifications = await query.Include(x => x.SysNotificationActorType)
                .Include(x => x.SysRequestType).OrderByDescending(x => x.CreatedOn).Take(5).ToListAsync();
            var notViewNotificationCount = await query.CountAsync(x => x.IsViewed == false);


            return new DataResponse<NotificationListDto>
            {
                IsDone = true,
                Data = new NotificationListDto
                {
                    NotificationList = _mapper.Map<List<NotificationDto>>(notifications),
                    NotViewedCount = notViewNotificationCount,
                }
            };
        }

        public async Task<BaseResponse> MarkNotificationViewed(Guid notificationId)
        {
            try
            {
                if (notificationId != Guid.Empty)
                {
                    var notification = (await _repository.GetQueryable<SysNotification, Guid>()).FirstOrDefault(x => x.Id == notificationId);
                    if (notification == null)
                        return new BaseResponse { IsDone = false, Message = _localization["GeneralNotFound"] };

                    notification.IsViewed = true;
                    notification.ViewedOn = DateTime.Now;
                    _repository.Update<SysNotification, Guid>(notification);
                    return await _repository.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return new BaseResponse { IsDone = false, Message = e.Message + e.InnerException };
            }

            return new BaseResponse { IsDone = false, Message = _localization["GeneralNotFound"] };

        }

        public async Task<BaseResponse> GetNotificationList(int pageIndex, int pageSize, Guid actorId, NotificationActorType actorTypeId)
        {
            PagedResult<SysNotification> entities = null;

            var entitiesQuerable = (await _repository.GetQueryable<SysNotification, Guid>())
                                   .Include(x => x.SysNotificationActorType)
                                   .Include(x => x.SysRequestType);


            entities = entitiesQuerable.Where(x => x.ActorId == actorId && x.NotificationActorTypeId == actorTypeId)
                                       .OrderByDescending(x => x.CreatedOn)
                                       .AsSplitQuery()
                                       .AsNoTracking()
                                       .GetPaged(pageIndex, pageSize);


            return new DataResponse<PagedResult<NotificationDto>>
            {
                Data =
                    new PagedResult<NotificationDto>
                    {
                        Results = _mapper.Map<List<NotificationDto>>(entities.Results),
                        CurrentPage = entities.CurrentPage,
                        PageCount = entities.PageCount,
                        PageSize = entities.PageSize,
                        TotalCount = entities.TotalCount
                    },
                IsDone = true
            };

        }

    }
}
