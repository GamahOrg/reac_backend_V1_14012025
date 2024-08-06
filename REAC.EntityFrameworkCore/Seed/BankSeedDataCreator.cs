using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace Ticketing.EntityFrameworkCore.SeedData
{
    public static class BankSeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysBanks.AnyAsync()) return;
            await context.SysBanks.AddRangeAsync(
                new List<SysBank>
                {
                    new SysBank
                    {
                        Id = Guid.NewGuid(),
                        BankName = "بنك فيصل الإسلامي",
                    },
                    new SysBank
                    {
                        Id = Guid.NewGuid(),
                        BankName = "بنك البركة",
                    }
                });
            context.SaveChanges();
        }
    }
}
