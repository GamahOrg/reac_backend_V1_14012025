
using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.EntityFrameworkCore.Entities.Notification
{
    public class SysNotificationPlanItemDestination : BaseEntity<Guid>
    {
        public string SysNotificationPlanItemId { get; set; } = null!;
        public NotificationDestinationType NotificationDestinationType { get; set; }
        public string Template { get; set; } = null!;
        public virtual SysNotificationPlanItem SysNotificationPlanItem { get; set; } = null!;
    }
}
