using Microsoft.EntityFrameworkCore;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Notification;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed
{
    public partial class NotificationActorTypeSeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysNotificationActorTypes.AnyAsync()) return;
            await context.SysNotificationActorTypes.AddRangeAsync(new List<SysNotificationActorType> {
                new SysNotificationActorType
                {
                    Id = NotificationActorType.Citizen,
                    ActorType = "مواطن",
                },
                new SysNotificationActorType
                {
                    Id = NotificationActorType.Employee,
                    ActorType = "موظف",
                }
        });
            context.SaveChanges();
        }
    }
}