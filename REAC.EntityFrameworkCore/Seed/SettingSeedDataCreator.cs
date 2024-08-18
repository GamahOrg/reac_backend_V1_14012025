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

                    #region
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Period",
                        Value = "30",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Request.Study.Period",
                        Value = "3",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Request.Process.Notes.Period",
                        Value = "2",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Reformer.Approve.Mission.Period",
                        Value = "3",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.LitigationParties.Response.Period",
                        Value = "3",
                    },
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),

                        Key = "Reconciliation.Report.Creation",
                        Value = "تم تحرير محضر الصُلح على الدعوى رقم {Litigation.Code}",
                    },
                    #endregion

                    #region NotificationMessages
                    new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Citizen.AddBankAccount",
                        Value = "تم اضافة بيانات الحساب البنكي الخاصه بحسابك",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Citizen.EditBankAccount",
                        Value = "تم تعديل بيانات الحساب البنكي الخاص بك",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = " Reformer.JoiningRequest",
                        Value = "تم اضافة طلب جديد بالانضمام كمصلح وبأنتظار الاعتماد",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = " Arbitrator.JoiningRequest",
                        Value = "تم اضافة طلب جديد بالانضمام كمحكم وبأنتظار الاعتماد",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = " Reformer.AcceptJoiningRequest",
                        Value = " تم اعتماد طلبكم بالانضمام كمصلح ",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = " Arbitrator.AcceptJoiningRequest",
                        Value = "تم اعتماد طلبكم بالانضمام كمحكم ",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = " Reformer.AcceptJoiningRequest.WaitingForFees",
                        Value = "تم اعتماد طلبكم للانضمام كمصلح و انتظار دفع رسوم العضوية.",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = " Arbitrator.AcceptJoiningRequest.WaitingForFees",
                        Value = "تم اعتماد طلبكم للانضمام كمحكم و انتظار دفع رسوم العضوية.",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.AcceptJoiningRequest.NoFees",
                        Value = "تم اعتماد طلبكم للانضمام كمصلح مع عدم وجود رسوم.",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.AcceptJoiningRequest.NoFees",
                        Value = "تم اعتماد طلبكم للانضمام كمحكم مع عدم وجود رسوم.",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.JoiningRequest.Updated",
                        Value = "تم تعديل طلب بالانضمام كمحكم للمحكم {ArbitratorName}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.JoiningRequest.Updated",
                        Value = "تم تعديل طلب بالانضمام كمصلح للمصلح {ReformerName}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.UpdateJoiningRequest.WithFees",
                        Value = "تنويه بانه تم إعادة طلبكم للانضمام كمحكم مع وجود رسوم للتعديل لوجود ملاحظات",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.UpdateJoiningRequest.WithFees",
                        Value = "تنويه بانه تم إعادة طلبكم للانضمام كمصلح مع وجود رسوم للتعديل لوجود ملاحظات",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.UpdateJoiningRequest.NoFees",
                        Value = "تنويه بانه تم إعادة طلبكم للانضمام كمصلح مع عدم وجود رسوم للتعديل لوجود ملاحظات",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.UpdateJoiningRequest.NoFees",
                        Value = "تنويه بانه تم إعادة طلبكم للانضمام كمحكم مع عدم وجود رسوم للتعديل لوجود ملاحظات",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.RejectJoiningRequest",
                        Value = "تنويه بانه تم رفض طلبكم للانضمام كمصلح",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.RejectJoiningRequest",
                        Value = "تنويه بانه تم رفض طلبكم للانضمام كمحكم",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.Activate",
                        Value = "تم تفعيل حسابك كمصلح معتمد",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.Activate",
                        Value = "تم تفعيل حسابك كمحكم معتمد",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reformer.DeActivate",
                        Value = "تم إيقاف حسابك",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrator.DeActivate",
                        Value = "تم إيقاف حسابك",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "Arbitration.Arbitraor.Assigned",
                        Value = "تم تعيينك كمحكم على الدعوى رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "Arbitration.ArbitraorChairman.Assigned",
                        Value = "تم تعيينك كرئيس هيئة تحكيم على الدعوى رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "Arbitration.ArbitraorAssigned.SendToChairman",
                        Value = "عزيزي {ArbitratorCahirman} \r\nنود إبالغكم بأن الدعوى رقم {LitigationCode} قد تم إحالتها إلي مرحلة التحكيم وتم استلامها من المحكم {ArbitratorName}\r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Reconciliator.AcceptTask",
                        Value = "تم قبول طلب المهمة لدعوى رقم {LitigationCode} من قبل المصلح {ReconciliatorName}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "Arbitration.Arbitraor.AcceptTask",
                        Value = "تم قبول طلب المهمة لدعوى رقم {LitigationCode} من قبل المحكم {ArbitratorName}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Reconciliator.RejectTask",
                        Value = "تم رفض طلب المهمة لدعوى رقم {LitigationCode} من قبل المصلح {ArbitratorName}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "Arbitration.Arbitraor.RejectTask",
                        Value = "تم رفض طلب المهمة لدعوى رقم {LitigationCode} من قبل المحكم {ArbitratorName}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.ReSendTask",
                        Value = "تم إرسال مهمة العمل كمصلح بالدعوى رقم {LitigationCode} للمصلح {ReconciliatorName}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitraorRecusal.AddRequest",
                        Value = "يوجد طلب رد محكم جديد {ArbitratorName} للدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitraorRecusal.AddRequest.Approve",
                        Value = "تم الموافقة على طلب رد محكم {ArbitratorName} للدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitraorRecusal.AddRequest.Reject",
                        Value = "تم رفض طلب رد محكم {ArbitratorName} للدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitraorRecusal.InformArbitrator.Reject",
                        Value = "تم إلغاء تعيينك كمحكم على الدعوى رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitraorRecusal.InformArbitratorChairman.Rejected",
                        Value = "تم إلغاء تعيينك كرئيس هيئة تحكيم على الدعوى رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitratorsChairmanNominationList.Added",
                        Value = "تم إضافة قائمة مرشحى رئيس هيئة التحكيم لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitratorsNominationList.Added.FirstParty",
                        Value = "عزيزي {FirstPartyName} تم أحالة طلبكم للتحكيم, لاختيار المحكم يمكنك الدخول الي حسابك في منصة التحكيم العقاري واختيار المحكم المرشح,علما بأنه سيتم انتهاء صلاحية ترشيح محكم في خلال {ExpirationDate}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitratorsNominationList.Added.SecondParty",
                        Value = "عزيزي {SecondPartyName} تم أحالة طلبكم للتحكيم, لاختيار المحكم يمكنك الدخول الي حسابك في منصة التحكيم العقاري واختيار المحكم المرشح,علما بأنه سيتم انتهاء صلاحية ترشيح محكم في خلال {ExpirationDate}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitratorsNominationList.AssignedAndSent",
                        Value = "تم تعيين هيئة التحكيم وإرسال طلبات المهام لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                       Id = Guid.NewGuid(),
                        Key = "ArbitratorsChairmanNominationListReply.Added",
                        Value = "تم الإنتهاء من تحديد اولوية الاختيار لرئيس هيئة التحكيم من قبل المحكمين لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitration.ExplanationReport.Added",
                        Value = "تم اضافة طلب تفسير على حكم التحكيم لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitration.ExplanationReport.RepliedOn",
                        Value = "تم الرد على طلب تفسير حكم التحكيم لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitration.ExtendRequest.Added",
                        Value = "عزيزي {ArbitratorCahirman}\r\n نود إبلاغكم بانه قام المحكم {ArbitratorName}\r\n بطلب تمديد مدة التحكيم علي الدعوى رقم {LitigationCode}\r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitration.ExtendRequest.Accepted",
                        Value = "تم قبول طلب مد مدة التحكيم لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitration.ExtendRequest.Rejected",
                        Value = "تم رفض طلب مد مدة التحكيم لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrators.Count.Added",
                        Value = "تم الإنتهاء من تحديد عدد المحكمين النهائي لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrators.Count.Added.AfterAgreement",
                        Value = "تم الإنتهاء من تحديد عدد المحكمين النهائي لاتفاق كلا المدعى والمدعى عليه على عدد المحكمين لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Arbitrators.Count.FromBothSides",
                        Value = "تم الإنتهاء من تحديد عدد المحكمين من قبل المدعى {FirstParty} والمدعى عليه {SecondParty} لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ArbitratorSelectionDuration.Added",
                        Value = "تم تحديد مدة تحديد المحكمين لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ArbitrationReport.Added.FirstParty",
                        Value = "عزيزي {FirstPartyName}\r\n نود إبلاغكم بانه تم إصدار حكم التحكيم علي الدعوى رقم {LitigationCode}\r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ArbitrationReport.Added.SecondParty",
                        Value = "عزيزي {SecondPartyName}\r\n نود إبلاغكم بانه تم إصدار حكم التحكيم علي الدعوى رقم {LitigationCode}\r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ArbitrationSession.Added.FirstParty",
                        Value = "عزيزي {FirstPartyName}\r\n نود إبلاغكم بأن الدعوى رقم {LitigationCode}\r\n قد تم تحديد موعد جلسة التحكيم لها بتاريخ {LitigationDateTime}\r\nبرجاء الحضور في الموعد المحدد.\r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ArbitrationSession.Added.SecondParty",
                        Value = "عزيزي {SecondPartyName}\r\n نود إبلاغكم بأن الدعوى رقم {LitigationCode}\r\n قد تم تحديد موعد جلسة التحكيم لها بتاريخ {LitigationDateTime}\r\nبرجاء الحضور في الموعد المحدد.\r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ExcuteWorkflow",
                        Value = "تم إرسال الدعوى رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ProcessWorkflow.Accepted",
                        Value = "تم قبول طلب الدعوي رقم {LitigationCode} \r\nبرجاء دفع رسوم قبول الطلب في مدة اقصاها {ExpirationDays} يوم \r\n",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ProcessWorkflow.Reject",
                        Value = "تم رفض الدعوى رقم {LitigationCode} بسبب {RejectionReason}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ProcessWorkflow.SentBackForEdit",
                        Value = "تم إعادة الطلب لوجود بعض الملاحظات لدعوى رقم {LitigationCode} علما بأنه سيتم انتهاء صلاحية التعديل في خلال {ExpirationDate}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ProcessWorkflow.Edited",
                        Value = "تم تعديل الدعوى وإرسالها مرة أخرى رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.Cancelled",
                        Value = "تم إلغاء المهمة (دعوى صلح) رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.ReAssigned",
                        Value = "تم إرسال مهمة مصلح لمصلح أخر لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "LitigationRequest.Decision.WaiverOfRight",
                        Value = "تم التنازل عن الحق لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "LitigationRequest.Decision.ReconciliationWasMade",
                        Value = "تم الصلح لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "LitigationRequest.Decision.WaiverOfTheLitigation",
                        Value = "تم التنازل عن الدعوى لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "LitigationRequest.Decision.ReconciliationWasNotMadeAndMoveToArbitration",
                        Value = "عدم الصلح والانتقال للتحكيم لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "LitigationRequest.Decision.ReconciliationWasNotMade",
                        Value = " عدم الصلح لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.ExtendRequest.Added",
                        Value = "تم اضافة طلب مد مدة الصلح لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.ExtendRequest.Accepted",
                        Value = "تم قبول طلب مد مدة الصلح لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "Reconciliation.ExtendRequest.Rejected",
                        Value = "تم رفض طلب مد مدة الصلح لدعوي رقم {LitigationCode}",
                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ReconciliationSession.Set",
                        Value = "تم تحديد موعد لجلسة الصلح\r\nرقم الدعوى: {LitigationCode}\r\nنوع الدعوى: {LitigationType}\r\nتاريت الجلسة: {LitigationDate}\r\nالتوقيت: {LitigationDateTime}\r\n \r\nبرجاء التواجد ف الموعد المحدد",

                    },new SysSetting
                    {
                        Id = Guid.NewGuid(),
                        Key = "ReconciliationSession.Reset",
                        Value = "تم أعادة تعيين جلسة الصلح\r\nترقم الدعوى: {LitigationCode}\r\nنوع الدعوى: {LitigationType}\r\nتاريت الجلسة: {LitigationDate}\r\nالتوقيت: {LitigationDateTime}\r\n \r\nبرجاء التواجد ف الموعد المحدد",

                    }
	                #endregion
                });
            context.SaveChanges();
        }

    }
}
