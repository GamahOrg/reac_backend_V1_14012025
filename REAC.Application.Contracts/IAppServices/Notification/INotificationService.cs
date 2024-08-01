using Core.Shared.Responses;
using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.IAppServices.Notification
{
    public interface INotificationService
    {
        Task<BaseResponse> GetNotificationList(Guid actorId, NotificationActorType actorTypeId);
        Task<BaseResponse> GetNotificationList(int pageIndex, int pageSize, Guid actorId, NotificationActorType actorTypeId);
        Task<BaseResponse> MarkNotificationViewed(Guid notificationId);
    }
}
