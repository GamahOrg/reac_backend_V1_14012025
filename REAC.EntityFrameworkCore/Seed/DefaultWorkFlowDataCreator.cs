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
                    WorkFlowName = "طلب التراخيص",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid(),
                    IsDeleted = false,
                    Translations = new List<WorkFlowTranslation>()
                    {
                        new WorkFlowTranslation()
                        {
                            LanguageCode = "ar-SA",
                            WorkFlowName = "طلب التراخيص"
                        },
                        new WorkFlowTranslation()
                        {
                            LanguageCode = "en-US",
                            WorkFlowName = "License Request"
                        }
                    },
                    WorkFlowSteps = new List<WorkFlowStep>()
                    {
                        new WorkFlowStep()
                        {
                            StepName = "معالجة الملاحظات",
                            WorkModeId = StepWorkMode.None,
                            Id = Guid.Parse("2a15b0d6-1f45-44f2-ba26-01c700b39203"),
                            Order = 0,
                            ActorTypeId = StepActorType.Client,
                            CreationLogMessage = "جارى معالجة الملاحظات",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = Guid.NewGuid(),
                            IsDeleted = false,
                            CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                            StageId = Guid.Parse("124e0e29-31e4-47f1-9536-aa35914b57e3"),
                            InternalStatusId = RequestStatus.RequestHasBeenCreatedAndIsBeingStudied,
                            ExternalStatusId = RequestStatus.RequestHasBeenCreatedAndIsBeingStudied,
                            ShowForApplicant = true,
                            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                            {
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("88b70280-7c6a-48e4-bf20-66ba537c580b"),
                                    EventName = "إرسال الطلب",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.FeedbackHasBeenProcessedAndTheRequestHasBeenSent,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.FeedbackHasBeenProcessedAndTheRequestHasBeenSent,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = true,
                                    AllowChangeIsActive = true,
                                    IsActiveValue = true,
                                    AllowNotes = true,
                                    IsAdminApprovalRequired = false,
                                    SameEmployee = true,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    LogMessage = "تم معالجة الملاحظات وإرسال الطلب",
                                    ActionModeId = EventActionMode.GoPreviousStep,
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
                                            EventName = "إرسال الطلب",
                                            LogMessage = "تم معالجة الملاحظات وإرسال الطلب",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Send Request",
                                            LogMessage = "Feedback has been processed and the request has been sent",
                                        }
                                    }
                                }
                            },
                            Translations = new List<WorkFlowStepTranslation>()
                            {
                                new WorkFlowStepTranslation()
                                {
                                    LanguageCode = "ar-SA",
                                    StepName = "معالجة الملاحظات",
                                    CreationLogMessage = "جارى معالجة الملاحظات"
                                },
                                new WorkFlowStepTranslation()
                                {
                                    LanguageCode = "en-US",
                                    StepName = "Processing Notes",
                                    CreationLogMessage = "Feedback is being processed"
                                }
                            }
                        },
                        new WorkFlowStep()
                        {
                            StepName = "دراسة الطلب",
                            WorkModeId = StepWorkMode.None,
                            Id = Guid.Parse("15154695-a36b-4fda-8572-4874f661000e"),
                            Order = 1,
                            ActorTypeId = StepActorType.Department,
                            CreationLogMessage = "تم إنشاء الطلب وجارى دراسته",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = Guid.NewGuid(),
                            IsDeleted = false,
                            CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                            DepartmentId = _context.SysDepartments.FirstOrDefault().Id,
                            StageId = Guid.Parse("124e0e29-31e4-47f1-9536-aa35914b57e3"),
                            InternalStatusId = RequestStatus.RequestHasBeenCreatedAndIsBeingStudied,
                            ExternalStatusId = RequestStatus.RequestHasBeenCreatedAndIsBeingStudied,
                            ShowForApplicant = true,
                            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                            {
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("3f01924e-59f4-44b7-9506-75c85b89b773"),
                                    EventName = "قبول",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.RequestHasBeenAccepted,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.RequestHasBeenAccepted,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    AllowChangeIsActive = true,
                                    IsActiveValue = false,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    AllowNotes = false,
                                    IsAdminApprovalRequired = true,
                                    LogMessage = "تم قبول الطلب",
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
                                            LogMessage = "تم قبول الطلب",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Accept",
                                            LogMessage = "Request has been accepted",
                                        }
                                    }
                                },
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("8d305628-46f7-45da-a177-77274c06d433"),
                                    EventName = "رفض",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.RequestHasBeenRejected,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.RequestHasBeenRejected,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    IsAdminApprovalRequired = true,
                                    AllowChangeIsActive = false,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = true,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    SameEmployee = false,
                                    AllowNotes = true,
                                    LogMessage = "تم رفض الطلب",
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
                                            LogMessage = "تم رفض الطلب",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Rejact",
                                            LogMessage = "Request has been Rejacted",
                                        }
                                    }
                                },
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("ab793310-f681-4b63-aa8b-ad9554adf2f3"),
                                    EventName = "إغلاق",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.RequestHasBeenRejected,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.RequestHasBeenRejected,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    IsAdminApprovalRequired = true,
                                    AllowChangeIsActive = true,
                                    IsActiveValue = false,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = true,
                                    EventSecurityLevelId = EventSecurityLevel.Specified,
                                    SameEmployee = false,
                                    AllowNotes = true,
                                    LogMessage = "تم إغلاق الطلب",
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
                                            EventName = "إغلاق",
                                            LogMessage = "تم إغلاق الطلب",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Close",
                                            LogMessage = "Request has been Closed",
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
                                    EventName = "إرسال ملاحظات",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.FeedbackHasBeenSentToTheClient,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.FeedbackHasBeenSentToTheClient,
                                    AllowCompletionNotificationPlan = true,
                                    CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    AllowChangeIsActive = true,
                                    IsActiveValue = false,
                                    AllowChangeIsFinished = true,
                                    AllowNotes = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    SameEmployee = false,
                                    LogMessage = "تم دراسة الطلب وإرسال الملاحظات",
                                    ActionModeId = EventActionMode.SpecificStep,
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                    NextWorkFlowStepDestinationId = Guid.Parse("2a15b0d6-1f45-44f2-ba26-01c700b39203"),
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "إرسال ملاحظات",
                                            LogMessage = "تم دراسة الطلب وإرسال الملاحظات",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Send Notes",
                                            LogMessage = "Request was studied and comments were sent",
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
                                    StepName = "دراسة الطلب",
                                    CreationLogMessage = "تم إنشاء الطلب وجارى دراسته"
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
