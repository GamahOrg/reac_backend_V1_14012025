using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace Ticketing.EntityFrameworkCore.SeedData
{
    public static class SettingSeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysSettings.AnyAsync()) return;
            await context.SysSettings.AddRangeAsync(
                new List<SysSetting>
                {
                    new SysSetting
                    {
                        Key =REAC.Domain.Shared.Constants.SysSettingKeyConstants.SecurityApiUrl,
                        Value = REAC.Domain.Shared.Constants.SysSettingKeyConstants.SecurityApiUrl
                    },
                });
        }
    }
}
