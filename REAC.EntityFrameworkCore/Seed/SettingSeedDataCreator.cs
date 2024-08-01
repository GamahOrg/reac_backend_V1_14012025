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
                        Value = "https://api-dev.rega.gov.sa/v1/moj/external-attorney/GetAttorneys?request.attorneyCode={0}&request.identityNumber={1}",
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

                    #region Nafath Get User Info

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "NafathGetUserInfo_URL",
                        Value = "https://api-test.rega.gov.sa/v1/iam/identityServices/userInfo",
                    },

                    #endregion

                    #region MC

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "MC_GetCRInformationByCROwners_URL",
                        Value = "https://api-dev.rega.gov.sa/v1/mc/cr-owners?CRNumber={0}",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "MC_GetCRInformationByCRNumber_URL",
                        Value = "https://api-dev.rega.gov.sa/v1/mc/cr-information?CRNumber={0}",
                    },

                    #endregion

                    #region SMS

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Disable_SMS_Integration",
                        Value = "false",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "SMS_URL",
                        Value = "https://api-test.rega.gov.sa/v1/rega/notificationservice/notify",
                    },

                     new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "SMS_Notification_Type",
                        Value = "SMS",
                    }, new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Litigation_Request_Max_Session_ReScheduleTime",
                        Value = "3",
                    },
                    

                    #endregion

                    #region OTP Setting

                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "OTP.Lenght",
                        Value = "6",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "OTP.Key",
                        Value = "D6",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "OTP.TimeoutInSeconds",
                        Value = "120", // 2 minutes
                    },

                    #endregion

                });
            context.SaveChanges();
        }
    }
}
