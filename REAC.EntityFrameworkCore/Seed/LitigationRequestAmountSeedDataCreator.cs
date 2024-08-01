using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed
{
    public partial class LitigationRequestAmountSeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysLitigationRequestAmounts.AnyAsync()) return;
            await context.SysLitigationRequestAmounts.AddRangeAsync(new List<SysLitigationRequestAmount>
            {
                new SysLitigationRequestAmount
                {
                    Id = Guid.NewGuid(),
                    LitigationAmount = "0-1000000",
                    Fees = 1000
                 },
                new SysLitigationRequestAmount
                {
                    Id = Guid.NewGuid(),
                    LitigationAmount = "1000001-5000000",
                    Fees = 2000

                },
                new SysLitigationRequestAmount
                {
                    Id = Guid.NewGuid(),
                    LitigationAmount = "5000001-10000000",
                                        Fees = 3000

                }

            }
            );
            context.SaveChanges();

        }
    }
}