using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Notification
{
    public class SysNotificationPlanItem : BaseEntity<Guid>
    {
        public Guid SysNotificationPlanId { get; set; }
        public NotificationType NotificationType { get; set; }

        public Guid? DepartmentId { get; set; }

        public string Template { get; set; } = null!;

        public virtual SysNotificationPlan SysNotificationPlan { get; set; } = null!;

        public virtual SysDepartment? SysDepartment { get; set; }

        public virtual ICollection<SysNotificationPlanItemDestination> SysNotificationPlanItemDestinations { get; set; } = new List<SysNotificationPlanItemDestination>();

    }
}
