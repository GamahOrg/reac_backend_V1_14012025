using REAC.Domain.Shared.Enum;
using REAC.Domain.Shared.Enum.Wpf;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.System;

namespace REAC.EntityFrameworkCore.Seed
{
    public class DefaultWorkFlowDataCreator
    {
        public static REACDBContext _context;
        public static async Task CreateAsync(REACDBContext context)
        {
            _context = context;
            if (!context.WorkFlowStepEventSecurityLevel.Any())
            {
                await context.WorkFlowStepEventSecurityLevel.AddRangeAsync(GetWorkFlowStepEventSecurityLevel());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlowStepEventSecurityLevelActorTypes.Any())
            {
                await context.WorkFlowStepEventSecurityLevelActorTypes.AddRangeAsync(GetWorkFlowStepEventSecurityLevelActorTypes());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlowStepEventActionModes.Any())
            {
                await context.WorkFlowStepEventActionModes.AddRangeAsync(GetWorkFlowStepEventActionModes());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlowStepWorkModes.Any())
            {
                await context.WorkFlowStepWorkModes.AddRangeAsync(GetWorkFlowStepWorkModes());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlowStepStages.Any())
            {
                await context.WorkFlowStepStages.AddRangeAsync(GetWorkFlowStepStages());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlowStepsActorTypes.Any())
            {
                await context.WorkFlowStepsActorTypes.AddRangeAsync(GetWorkFlowStepsActorTypes());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlowNotificationPlans.Any())
            {
                await context.WorkFlowNotificationPlans.AddRangeAsync(GetWorkFlowNotificationPlans());
                await context.SaveChangesAsync();
            }

            if (!context.WorkFlows.Any())
            {
                await context.WorkFlows.AddRangeAsync(GetWorkFlows());
                await context.SaveChangesAsync();
            }
        }

        #region WorkFlow Step Event Security Level
        private static List<WorkFlowStepEventSecurityLevel> GetWorkFlowStepEventSecurityLevel()
        {
            return new List<WorkFlowStepEventSecurityLevel> {
                new WorkFlowStepEventSecurityLevel()
                {
                    Id = EventSecurityLevel.Specified,
                    SecurityLevel = "محدد",
                    Translations = new List<WorkFlowStepEventSecurityLevelTranslation>()
                    {
                        new WorkFlowStepEventSecurityLevelTranslation()
                        {
                            LanguageCode = "en-US",
                            SecurityLevel = "Specified"
                        },
                        new WorkFlowStepEventSecurityLevelTranslation()
                        {
                            LanguageCode = "ar-SA",
                            SecurityLevel = "محدد"
                        }
                    }
                },
                new WorkFlowStepEventSecurityLevel()
                {
                    Id = EventSecurityLevel.UnSpecified,
                    SecurityLevel = "غير محدد",
                    Translations = new List<WorkFlowStepEventSecurityLevelTranslation>()
                    {
                        new WorkFlowStepEventSecurityLevelTranslation()
                        {
                            LanguageCode = "en-US",
                            SecurityLevel = "UnSpecified"
                        },
                        new WorkFlowStepEventSecurityLevelTranslation()
                        {
                            LanguageCode = "ar-SA",
                            SecurityLevel = "غير محدد"
                        }
                    }
                }
            };
        }
        #endregion

        #region Work Flow Step Work Mode
        private static List<WorkFlowStepWorkMode> GetWorkFlowStepWorkModes()
        {
            return new List<WorkFlowStepWorkMode> {
                new WorkFlowStepWorkMode()
                {
                    Id = StepWorkMode.None,
                    WorkMode = "لا شئ",
                    Translations = new List<WorkFlowStepWorkModeTranslation>()
                    {
                        new WorkFlowStepWorkModeTranslation()
                        {
                            LanguageCode = "en-US",
                            WorkMode = "None"
                        },
                        new WorkFlowStepWorkModeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            WorkMode = "لا شئ"
                        }
                    }
                }
            };
        }
        #endregion

        #region WorkFlow Event Action Mode
        private static List<WorkFlowStepEventActionMode> GetWorkFlowStepEventActionModes()
        {
            return new List<WorkFlowStepEventActionMode> {
                new WorkFlowStepEventActionMode()
                {
                    Id = EventActionMode.GoNextStep,
                    ActionMode = "الخطوة التالية",
                    Translations = new List<WorkFlowStepEventActionModeTranslation>()
                    {
                        new WorkFlowStepEventActionModeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActionMode = "Go Next Step",
                        },
                        new WorkFlowStepEventActionModeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActionMode = "الخطوة التالية",
                        }
                    }
                },
                new WorkFlowStepEventActionMode()
                {
                    Id = EventActionMode.GoPreviousStep,
                    ActionMode = "الخطوة السابقة",
                    Translations = new List<WorkFlowStepEventActionModeTranslation>()
                    {
                        new WorkFlowStepEventActionModeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActionMode = "Previos Step",
                        },
                        new WorkFlowStepEventActionModeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActionMode = "الخطوة السابقة",
                        }
                    }
                },
                new WorkFlowStepEventActionMode()
                {
                    Id = EventActionMode.SpecificStep,
                    ActionMode = "خطوة محددة",
                    Translations = new List<WorkFlowStepEventActionModeTranslation>()
                    {
                        new WorkFlowStepEventActionModeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActionMode = "Specific Step",
                        },
                        new WorkFlowStepEventActionModeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActionMode = "خطوة محددة",
                        }
                    }
                }
            };
        }
        #endregion

        #region WorkFlow Step Stage
        private static List<WorkFlowStepStage> GetWorkFlowStepStages()
        {
            return new List<WorkFlowStepStage> {
                new WorkFlowStepStage()
                {
                    Id = Guid.Parse("124e0e29-31e4-47f1-9536-aa35914b57e3"),
                    StageName = "مرحلة الدراسة",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid(),
                    IsDeleted = false,
                    Translations = new List<WorkFlowStepStageTranslation>()
                    {
                        new WorkFlowStepStageTranslation()
                        {
                            LanguageCode = "en-US",
                            StageName = "Studing Stage"
                        },
                        new WorkFlowStepStageTranslation()
                        {
                            LanguageCode = "ar-SA",
                            StageName = "مرحلة الدراسة"
                        }
                    }
                }
            };
        }
        #endregion

        #region WorkFlow Steps Actor Type
        private static List<WorkFlowStepsActorType> GetWorkFlowStepsActorTypes()
        {
            return new List<WorkFlowStepsActorType> {
                new WorkFlowStepsActorType()
                {
                    Id = StepActorType.Employee,
                    ActorType = "موظف",
                    Translations = new List<WorkFlowStepsActorTypeTranslation>()
                    {
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActorType = "Employee"
                        },
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActorType = "موظف"
                        }
                    }
                },
                new WorkFlowStepsActorType()
                {
                    Id = StepActorType.Admin,
                    ActorType = "مدير إدارة",
                    Translations = new List<WorkFlowStepsActorTypeTranslation>()
                    {
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActorType = "Department Adminstrator"
                        },
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActorType = "مدير إدارة"
                        }
                    }
                },
                new WorkFlowStepsActorType()
                {
                    Id = StepActorType.Official,
                    ActorType = "مسئول",
                    Translations = new List<WorkFlowStepsActorTypeTranslation>()
                    {
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActorType = "Official"
                        },
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActorType = "مسئول"
                        }
                    }
                },
                new WorkFlowStepsActorType()
                {
                    Id = StepActorType.Department,
                    ActorType = "قسم",
                    Translations = new List<WorkFlowStepsActorTypeTranslation>()
                    {
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActorType = "Departmnet"
                        },
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActorType = "قسم"
                        }
                    }
                },
                new WorkFlowStepsActorType()
                {
                    Id = StepActorType.Client,
                    ActorType = "عميل",
                    Translations = new List<WorkFlowStepsActorTypeTranslation>()
                    {
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            ActorType = "Client"
                        },
                        new WorkFlowStepsActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            ActorType = "عميل"
                        }
                    }
                }
            };
        }
        #endregion

        #region WorkFlow Step Event Security Level Actor Type
        private static List<WorkFlowStepEventSecurityLevelActorType> GetWorkFlowStepEventSecurityLevelActorTypes()
        {
            return new List<WorkFlowStepEventSecurityLevelActorType> {
                new WorkFlowStepEventSecurityLevelActorType()
                {
                    Id = SecurityLevelActorType.Employee,
                    SecurityLevelActorType = "موظف",
                    Translations = new List<WorkFlowStepEventSecurityLevelActorTypeTranslation>()
                    {
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            SecurityLevelActorType = "Employee"
                        },
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            SecurityLevelActorType = "موظف"
                        }
                    }
                },
                new WorkFlowStepEventSecurityLevelActorType()
                {
                    Id = SecurityLevelActorType.Admin,
                    SecurityLevelActorType = "مدير إدارة",
                    Translations = new List<WorkFlowStepEventSecurityLevelActorTypeTranslation>()
                    {
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            SecurityLevelActorType = "Department Adminstrator"
                        },
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            SecurityLevelActorType = "مدير إدارة"
                        }
                    }
                },
                new WorkFlowStepEventSecurityLevelActorType()
                {
                    Id = SecurityLevelActorType.Official,
                    SecurityLevelActorType = "مسئول",
                    Translations = new List<WorkFlowStepEventSecurityLevelActorTypeTranslation>()
                    {
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            SecurityLevelActorType = "Official"
                        },
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            SecurityLevelActorType = "مسئول"
                        }
                    }
                },
                new WorkFlowStepEventSecurityLevelActorType()
                {
                    Id = SecurityLevelActorType.Department,
                    SecurityLevelActorType  = "قسم",
                    Translations = new List<WorkFlowStepEventSecurityLevelActorTypeTranslation>()
                    {
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "en-US",
                            SecurityLevelActorType = "Departmnet"
                        },
                        new WorkFlowStepEventSecurityLevelActorTypeTranslation()
                        {
                            LanguageCode = "ar-SA",
                            SecurityLevelActorType = "قسم"
                        }
                    }
                }
            };
        }
        #endregion

        #region WorkFlow Notification Plan
        private static List<WorkFlowNotificationPlan> GetWorkFlowNotificationPlans()
        {
            return new List<WorkFlowNotificationPlan> {
                new WorkFlowNotificationPlan()
                {
                    Id = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                    NotificationPlan = "خطة التنبيه",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid(),
                    Translations = new List<WorkFlowNotificationPlanTranslation>()
                    {
                        new WorkFlowNotificationPlanTranslation()
                        {
                            LanguageCode = "en-US",
                            NotificationPlan = "Notification Plan"
                        },
                        new WorkFlowNotificationPlanTranslation()
                        {
                            LanguageCode = "ar-SA",
                            NotificationPlan = "خطة التنيه",
                        }
                    }
                }
            };
        }
        #endregion

        #region WorkFlow Steps Actor Type
        private static List<WorkFlow> GetWorkFlows()
        {
            return new List<WorkFlow>()
            {
                new WorkFlow()
                {
                    Id = Guid.Parse("15d87d0d-3cf4-4969-9c7e-933aeea0e5f5"),
                    WorkFlowName = "تدقيق الدعوة",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid(),
                    IsDeleted = false,
                    Translations = new List<WorkFlowTranslation>()
                    {
                        new WorkFlowTranslation()
                        {
                            LanguageCode = "ar-SA",
                            WorkFlowName = "تدقيق الدعوة"
                        },
                        new WorkFlowTranslation()
                        {
                            LanguageCode = "en-US",
                            WorkFlowName = "Lititgation Audit"
                        }
                    },
                    WorkFlowSteps = new List<WorkFlowStep>()
                    {
                        new WorkFlowStep()
                        {
                            StepName = "تدقيق الدعوى",
                            WorkModeId = StepWorkMode.None,
                            Id = Guid.Parse("15154695-a36b-4fda-8572-4874f661000e"),
                            Order = 1,
                            ActorTypeId = StepActorType.Department,
                            CreationLogMessage = "تم إنشاء الدعوى وجارى تدقيقها",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = Guid.NewGuid(),
                            IsDeleted = false,
                            CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                            DepartmentId = _context.SysDepartments.FirstOrDefault().Id,
                            StageId = Guid.Parse("124e0e29-31e4-47f1-9536-aa35914b57e3"),
                            InternalStatusId = LitigationRequestStatus.New,
                            ExternalStatusId = LitigationRequestStatus.New,
                            ShowForApplicant = true,
                            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                            {
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("3f01924e-59f4-44b7-9506-75c85b89b773"),
                                    EventName = "قبول",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = LitigationRequestStatus.Received,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = LitigationRequestStatus.Received,
                                  
                                    AllowFileUpload = false,
                                  
                                    IsActiveValue = false,
                                    AllowChangeIsFinished = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    AllowNotes = false,
                                    IsAdminApprovalRequired = false,
                                    LogMessage = "تم قبول الدعوى",
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                    SameEmployee = false,
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "قبول",
                                            LogMessage = "تم قبول الدعوى",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Accept",
                                            LogMessage = "Litigation has been accepted",
                                        }
                                    }
                                },
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("8d305628-46f7-45da-a177-77274c06d433"),
                                    EventName = "رفض",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = LitigationRequestStatus.Rejected,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = LitigationRequestStatus.Rejected,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    IsAdminApprovalRequired = false,
                                    AllowChangeIsActive = false,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = true,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    SameEmployee = false,
                                    AllowNotes = true,
                                    LogMessage = "تم رفض الدعوى",
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "رفض",
                                            LogMessage = "تم رفض الدعوى",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Reject",
                                            LogMessage = "Request has been Rejected",
                                        }
                                    }
                                },
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("ab793310-f681-4b63-aa8b-ad9554adf2f3"),
                                    EventName = "إرسال طلب مشارطة",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = LitigationRequestStatus.CharterRequestSent,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = LitigationRequestStatus.CharterRequestSent,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    IsAdminApprovalRequired = false,
                                    AllowChangeIsActive = false,
                                    AllowChangeIsFinished = false,
                                    EventSecurityLevelId = EventSecurityLevel.Specified,
                                    SameEmployee = false,
                                    AllowNotes = false,
                                    LogMessage = "تم إرسال طلب المشارطة",
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "'طلب مشارطة",
                                            LogMessage = "تم إرسال طلب مشارطة",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "CharterPartyRequest",
                                            LogMessage = "CharterPartyRequest has sent",
                                        }
                                    },
                                    WorkFlowStepEventSecurityLevelActors = new List<WorkFlowStepEventSecurityLevelActor>()
                                    {
                                        new WorkFlowStepEventSecurityLevelActor()
                                        {
                                            SecurityLevelActorId = SecurityLevelActorType.Admin,
                                            DepartmentId = _context.SysDepartments.FirstOrDefault().Id,
                                            Id = Guid.Parse("fbd73a75-0f48-41a3-8d6f-d3ff46516d78")

                                        }
                                    }
                                },
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("8a392baf-a959-429a-b2c9-eea05054ff11"),
                                    EventName = "إعادة الدعوى للتعديل",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = LitigationRequestStatus.SentBackForEdit,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = LitigationRequestStatus.SentBackForEdit,
                                    AllowFileUpload = false,
                                    AllowChangeIsActive = false,
                                    AllowNotes = true,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    LogMessage = "تم إعادة الدعوى للتعديل",
                                    ActionModeId = EventActionMode.SpecificStep,
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                   // NextWorkFlowStepDestinationId = Guid.Parse("2a15b0d6-1f45-44f2-ba26-01c700b39203"),
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "إعادة  الدعوى للتعديل",
                                            LogMessage = "تم دراسة الدعوى وإعادتها للتعديل",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Send Back For Edit",
                                            LogMessage = "Request was studied and sent for edit",
                                        }
                                    }
                                }
                            },
                            Translations = new List<WorkFlowStepTranslation>()
                            {
                                new WorkFlowStepTranslation()
                                {
                                    LanguageCode = "en-US",
                                    StepName = "Request Studing",
                                    CreationLogMessage = "Request has been created and is being Studied"
                                },
                                new WorkFlowStepTranslation()
                                {
                                    LanguageCode = "ar-SA",
                                    StepName = "دراسة الدعوى",
                                    CreationLogMessage = "تم إنشاء الدعوى وجارى دراستها"
                                }
                            }
                        }

                    }
                }
            };
        }
        #endregion
    }
}
