using Core.Shared.Responses;
using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.IAppServices.Notification
{
    public interface INotificationService
    {
        Task<BaseResponse> GetNotificationListForPortal(Guid actorId, NotificationActorType actorTypeId);
        Task<BaseResponse> GetNotificationListForBackOffice(Guid actorId, NotificationActorType actorTypeId);
        Task<BaseResponse> GetNotificationListForPortal(int pageIndex, int pageSize, Guid actorId, NotificationActorType actorTypeId);
        Task<BaseResponse> GetNotificationListForBackOffice(int pageIndex, int pageSize, Guid actorId, NotificationActorType actorTypeId);
        Task<BaseResponse> MarkNotificationViewed(Guid notificationId);
    }
}
