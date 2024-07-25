
using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.EntityFrameworkCore.Entities.Notification
{
    public class SysNotificationPlanItemDestination : BaseEntity<Guid>
    {
        public Guid SysNotificationPlanItemId { get; set; }
        public NotificationDestinationType NotificationDestinationType { get; set; }
        public string Template { get; set; } = null!;
        public virtual SysNotificationPlanItem SysNotificationPlanItem { get; set; } = null!;
    }
}
