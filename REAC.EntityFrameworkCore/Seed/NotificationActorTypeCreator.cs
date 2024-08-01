using Core.Security;
using REAC.EntityFrameworkCore.Entities.Notification;

namespace Wafi.EntityFrameworkCore.Seed;

public partial class DefaultDataAppCreator
{
    private static List<SysNotificationActorType> GetSysNotificationActorType(IdentityContext identityContext)
    {
        return new List<SysNotificationActorType> {
                new SysNotificationActorType
                {
                    Id = REAC.Domain.Shared.Enum.NotificationActorType.Citizen,
                    ActorType = "مواطن",
                },
                new SysNotificationActorType
                {
                    Id = REAC.Domain.Shared.Enum.NotificationActorType.Employee,
                    ActorType = "موظف",
                }
        };
    }
}