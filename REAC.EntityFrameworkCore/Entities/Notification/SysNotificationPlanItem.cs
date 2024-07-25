using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.EntityFrameworkCore.Entities.Notification
{
    public class SysNotificationPlanItem : BaseEntity<Guid>
    {
        public string SysNotificationPlanId { get; set; } = null!;
        public NotificationType NotificationType { get; set; }
        public string Template { get; set; } = null!;
        public virtual SysNotificationPlan SysNotificationPlan { get; set; } = null!;
        public virtual ICollection<SysNotificationPlanItemDestination> SysNotificationPlanItemDestinations { get; set; } = new List<SysNotificationPlanItemDestination>();

    }
}
