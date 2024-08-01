using Microsoft.EntityFrameworkCore;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Notification;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed
{
    public partial class RequestTypeSeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysRequestTypes.AnyAsync()) return;
            await context.SysRequestTypes.AddRangeAsync(new List<SysRequestType>()
            {
                new SysRequestType
                {
                    Id = RequestType.LitigationRequest,
                    RequestTypeName = "طلب دعوى",
                },
                new SysRequestType
                {
                    Id = RequestType.ExtendArbitrationPeriodRequest,
                    RequestTypeName = "طلب مد مدة التحكيم",
                },
                new SysRequestType
                {
                    Id = RequestType.ExtendReconciliationPeriodRequest,
                    RequestTypeName = "طلب مد مدة الصلح",
                }
            });
            context.SaveChanges();
        }
    }
}