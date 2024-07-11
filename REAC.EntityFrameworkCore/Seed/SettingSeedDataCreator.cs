using Core.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
                    //new SysSetting
                    //{
                    //    Id = Guid.NewGuid(),
                    //    Key = REAC.Domain.Shared.Constants.SysKeyConstants.SecurityApiUrl,
                    //    Value = REAC.Domain.Shared.Constants.SysKeyConstants.SecurityApiUrl,
                    //},
                    #region GEE Login Integration

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "GEE_client_id",
                        Value = "UGgZBIiwnTm0KqwerVrAkcblxWnpYc1l",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "GEE_client_secret",
                        Value = "g33z5iKMO3BWpcAe",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "GEE_grant_type",
                        Value = "client_credentials",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "GEE_LoginURL",
                        Value = "https://api-test.rega.gov.sa/v1/oauth",
                    },

                    #endregion

                    #region MOJ

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "MOJ_URL",
                        Value = "https://api-dev.rega.gov.sa/v1/moj/external-attorney/GetAttorneys",
                    },

                    #endregion

                    #region Nafath

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Nafath_URL",
                        Value = "https://api-dev.rega.gov.sa/v1/iam/nafath",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Nafath_Action",
                        Value = "SpRequest",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Nafath_Service",
                        Value = "AdvancedLogin",
                    },

                    #endregion

                    #region Nafath Check

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "NafathCheck_Action",
                        Value = "CheckSpRequest",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "NafathCheck_URL",
                        Value = "https://api-dev.rega.gov.sa/v1/iam/nafath",
                    },
                    
                    #endregion


                });
        }
    }
}
