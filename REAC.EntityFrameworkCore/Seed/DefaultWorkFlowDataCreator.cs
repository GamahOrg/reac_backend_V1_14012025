using REAC.Domain.Shared.Enum;
using REAC.Domain.Shared.Enum.Wpf;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.System;

namespace REAC.EntityFrameworkCore.Seed
{
    public class DefaultWorkFlowDataCreator
    {
        public static REACDBContext? _context;
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
                    StageName = "مرحلة التدقيق",
                    CreatedOn = DateTime.Now,
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
                            StageName = "مرحلة التدقيق"
                        }
                    }
                },
                new WorkFlowStepStage()
                {
                    Id = Guid.Parse("5406436a-9d9e-4996-81c6-87cc5e13a653"),
                    StageName = "مراحل دراسة الطلب",
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.NewGuid(),
                    IsDeleted = false,
                    Translations = new List<WorkFlowStepStageTranslation>()
                    {
                        new WorkFlowStepStageTranslation()
                        {
                            LanguageCode = "en-US",
                            StageName = "Request Study Stages"
                        },
                        new WorkFlowStepStageTranslation()
                        {
                            LanguageCode = "ar-SA",
                            StageName = "مراحل دراسة الطلب",
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
                    CreatedOn = DateTime.Now,
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
                // صلح
                new WorkFlow()
                {
                    Id = Guid.Parse("15d87d0d-3cf4-4969-9c7e-933aeea0e5f5"),
                    WorkFlowName = "دعوة صلح",
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.NewGuid(),
                    IsDeleted = false,
                    Translations = new List<WorkFlowTranslation>()
                    {
                        new WorkFlowTranslation()
                        {
                            LanguageCode = "ar-SA",
                            WorkFlowName = "دعوى صلح"
                        },
                        new WorkFlowTranslation()
                        {
                            LanguageCode = "en-US",
                            WorkFlowName = "Conciliation Lititgation"
                        }
                    },
                    WorkFlowSteps = new List<WorkFlowStep>()
                    {
                        new WorkFlowStep()
                        {
                            StepName = "معالجة الملاحظات",
                            ProcessActorType = ProcessActorType.Plaintiff,
                            WorkModeId = StepWorkMode.None,
                            Id = Guid.Parse("2a15b0d6-1f45-44f2-ba26-01c700b39203"),
                            Order = 0,
                            ActorTypeId = StepActorType.Client,
                            CreationLogMessage = "جارى معالجة الملاحظات",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Guid.NewGuid(),
                            IsDeleted = false,
                            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                            ShowForApplicant = true,
                            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                            {
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("88b70280-7c6a-48e4-bf20-66ba537c580b"),
                                    EventName = "إرسال طلب الدعوى",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.Edited,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.Edited,
                                    AllowCompletionNotificationPlan = true,
                                    //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = true,
                                    AllowChangeIsActive = true,
                                    IsActiveValue = true,
                                    AllowNotes = true,
                                    IsAdminApprovalRequired = false,
                                    SameEmployee = true,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = false,
                                    AllowChangeIsAllowEdit = true,
                                    IsAllowEditValue = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    LogMessage = "تم معالجة الملاحظات وإرسال الدعوى",
                                    ActionModeId = EventActionMode.GoPreviousStep,
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "إرسال طلب الدعوى",
                                            LogMessage = "تم معالجة الملاحظات وإرسال الدعوى",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Send Litigation Request",
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
                        }, // تعديل الدعوى الدعوى
                        new WorkFlowStep()
                        {
                            StepName = "قرار طلب المشارطة",
                            ProcessActorType = ProcessActorType.PlaintiffAndDefendant,
                            WorkModeId = StepWorkMode.None,
                            Id = Guid.Parse("d443b2ba-64a1-4eb7-b053-4267577be668"),
                            Order = 0,
                            ActorTypeId = StepActorType.Client,
                            CreationLogMessage = "جارى الرد على طلب المشارطة",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Guid.NewGuid(),
                            IsDeleted = false,
                            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                            ShowForApplicant = true,
                            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                            {
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
                        }, // تعديل الدعوى الدعوى
                        new WorkFlowStep()   //تدقيق الدعوى
                        {
                            StepName = "تدقيق الدعوى",

                            ProcessActorType = ProcessActorType.Employee,
                            WorkModeId = StepWorkMode.None,
                            Id = Guid.Parse("15154695-a36b-4fda-8572-4874f661000e"),
                            Order = 1,
                            ActorTypeId = StepActorType.Department,
                            CreationLogMessage = "تم إنشاء الدعوى وجارى تدقيقها 1",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Guid.NewGuid(),
                            IsDeleted = false,
                            InternalStatusId = RequestStatus.New,
                            ExternalStatusId = RequestStatus.New,

                           
                            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                            DepartmentId = Guid.Parse("657004f4-57e1-4715-addc-0e5ff0b00938"),
                            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                            ShowForApplicant = true,
                            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                            {
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("3f01924e-59f4-44b7-9506-75c85b89b773"),
                                    EventName = "قبول",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.Received,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.Received,
                                    LitigationRequestStage = LitigationRequestStage.Reconciliation,
                                    LitigationRequestSubStage = LitigationRequestSubStage.AddSession,
                                    AllowFileUpload = false,
                                    IsActiveValue = false,
                                    AllowChangeIsFinished = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    AllowNotes = false,
                                    IsAdminApprovalRequired = false,
                                    LogMessage = "تم قبول الدعوى",
                                    CreatedOn = DateTime.Now,
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
                                    InternalStatusId = RequestStatus.Rejected,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.Rejected,
                                    AllowCompletionNotificationPlan = true,
                                    //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                                    AllowFileUpload = false,
                                    IsAdminApprovalRequired = false,
                                    AllowChangeIsActive = true,
                                    AllowChangeIsFinished = true,
                                    IsFinishedValue = true,
                                    IsActiveValue = false,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    SameEmployee = false,
                                    AllowNotes = true,
                                    LogMessage = "تم رفض الدعوى",
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = true,
                                    IsAllowEditValue = true,
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
                                    Id = Guid.Parse("8a392baf-a959-429a-b2c9-eea05054ff11"),
                                    EventName = "إعادة الدعوى للتعديل",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.SentBackForEdit,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.SentBackForEdit,
                                    AllowFileUpload = true,
                                    AllowChangeIsActive = false,
                                    AllowNotes = true,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    LogMessage = "تم إعادة الدعوى للتعديل",
                                    ActionModeId = EventActionMode.SpecificStep,
                                    CreatedOn = DateTime.Now,
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
                                },
                                new WorkFlowStepEvent()
                                {
                                    Id = Guid.Parse("40b92a76-2c2d-4c32-a0f9-0c8bb03a67a9"),
                                    EventName = "إرسال طلب مشارطة",
                                    AllowChangeInternalStatus = true,
                                    InternalStatusId = RequestStatus.AgreementRequestHasBeenSent,
                                    AllowChangeExternalStatus = true,
                                    ExternalStatusId = RequestStatus.AgreementRequestHasBeenSent,
                                    AllowChangeIsActive = false,
                                    AllowNotes = true,
                                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                                    LogMessage = "تم إرسال طلب مشارطة",
                                    ActionModeId = EventActionMode.SpecificStep,
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = Guid.NewGuid(),
                                    IsDeleted = false,
                                    ShowLogForApplicant = true,
                                    ShowProcessingLogForApplicant = true,
                                    Translations  = new List<WorkFlowStepEventTranslation>()
                                    {
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "ar-SA",
                                            EventName = "إرسال طلب مشارطة",
                                            LogMessage = "تم إرسال طلب مشارطة",
                                        },
                                        new WorkFlowStepEventTranslation()
                                        {
                                            LanguageCode = "en-US",
                                            EventName = "Charter Request Sent",
                                            LogMessage = "Agreement Request Has Been Sent",
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
                                    CreationLogMessage = "Litigation has been created and is being reviewed"
                                },
                                new WorkFlowStepTranslation()
                                {
                                    LanguageCode = "ar-SA",
                                    StepName = "دراسة الدعوى",
                                    CreationLogMessage = "تم إنشاء الدعوى وجارى تدقيقها",
                                }
                            }
                        }, // تدقيق الدعوى
#region
                        //new WorkFlowStep()
                        //{
                        //    StepName = "تحديد جلسة الصلح",
                        //    ProcessActorType = ProcessActorType.Reformer,
                        //    WorkModeId = StepWorkMode.None,
                        //    Id = Guid.Parse("92d1235b-b8b3-42e4-aeda-a9828d7c6fee"),
                        //    Order = 2,
                        //    ActorTypeId = StepActorType.Department,
                        //    CreationLogMessage = "تم تدقيق الدعوى وجارى مرحلة الصلح",
                        //    CreatedOn = DateTime.Now,
                        //    CreatedBy = Guid.NewGuid(),
                        //    IsDeleted = false,
                        //    //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //    StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                        //    ShowForApplicant = true,
                        //    Translations = new List<WorkFlowStepTranslation>()
                        //    {
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "en-US",
                        //            StepName = "Add reconciliation session",
                        //            CreationLogMessage = "Request has been created and is being Audited"
                        //        },
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "ar-SA",
                        //            StepName = "تحديد جلسة الصلح",
                        //            CreationLogMessage = "تم تدقيق الدعوى وجارى مرحلة الصلح",
                        //        }
                        //    },
                        //    WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                        //    {
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("4d94b2fc-2dc8-4d2b-91c0-c80d687255b4"),
                        //            EventName = "تحديد موعد جلسة الصلح",
                        //            CustomActionType = CustomActionType.AddReconciliationSession,
                        //            AllowChangeInternalStatus = true,
                        //            InternalStatusId = RequestStatus.AsessionTimeHasBeenSet,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.AsessionTimeHasBeenSet,
                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = false,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = false,
                        //            IsFinishedValue = false,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            LogMessage = "تم تحديد موعد جلسة الصلح",
                        //            ActionModeId = EventActionMode.GoNextStep,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "تحديد موعد جلسة الصلح",
                        //                    LogMessage = "تم تحديد موعد جلسة الصلح",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Add reconciliation session",
                        //                    LogMessage = "A reconciliation session has been scheduled",
                        //                }
                        //            }
                        //        }
                        //    }

                        //}, // تحديد جلسة الصلح
                        //new WorkFlowStep()
                        //{
                        //    StepName = "مرحلة الصلح",
                        //    ProcessActorType = ProcessActorType.Reformer,
                        //    WorkModeId = StepWorkMode.None,
                        //    Id = Guid.Parse("6e1dce17-2c6f-4769-ae9f-13e192f6165e"),
                        //    Order = 3,
                        //    ActorTypeId = StepActorType.Department,
                        //    CreationLogMessage = "جارى تنفيذ جلسة الصلح",
                        //    CreatedOn = DateTime.Now,
                        //    CreatedBy = Guid.NewGuid(),
                        //    IsDeleted = false,
                        //    //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //    StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                        //    ShowForApplicant = true,
                        //    Translations = new List<WorkFlowStepTranslation>()
                        //    {
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "en-US",
                        //            StepName = "Request Studing",
                        //            CreationLogMessage = "Conciliation session is being carried out"
                        //        },
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "ar-SA",
                        //            StepName = "مرحلة الطلح",
                        //            CreationLogMessage = "جارى تنفيذ جلسة الصلح",
                        //        }
                        //    },
                        //    WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                        //    {
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("3c922f3c-316b-4552-b229-503e76beb7a5"),
                        //            EventName = "تغير موعد جلسة الصلح",
                        //            CustomActionType = CustomActionType.RescheduleReconciliationSession,
                        //            AllowChangeInternalStatus = true,
                        //            InternalStatusId = RequestStatus.AsessionTimeHasBeenReset,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.AsessionTimeHasBeenReset,
                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = false,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = false,
                        //            IsFinishedValue = false,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            LogMessage = "تم تغيير موعد جلسة الصلح",
                        //            ActionModeId = EventActionMode.GoNextStep,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "تغيير موعد جلسة الصلح",
                        //                    LogMessage = "تم تغيير موعد جلسة الصلح",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reschedule the reconciliation session",
                        //                    LogMessage = "A reconciliation session has been Rescheduled",
                        //                }
                        //            }
                        //        },
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("a8f392ba-6700-44c0-823c-801d07ca5527"),
                        //            EventName = "كتابة محضر الجلسة",
                        //            CustomActionType = CustomActionType.WritingMinutesOfTheMeeting,
                        //            AllowChangeInternalStatus = true,
                        //            InternalStatusId = RequestStatus.TheMinutesOfTheMeetingWereWritten,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.TheMinutesOfTheMeetingWereWritten,
                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = false,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = false,
                        //            IsFinishedValue = false,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            LogMessage = "تم كتابة محضر الجلسة",
                        //            ActionModeId = EventActionMode.GoNextStep,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "كتابة محضر الجلسة",
                        //                    LogMessage = "تم كتابة محضر الجلسة",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reschedule the reconciliation session",
                        //                    LogMessage = "A reconciliation session has been Rescheduled",
                        //                }
                        //            }
                        //        },
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("7a64f581-d44a-4fb3-a760-5dfe8d164950"),
                        //            EventName = "مدة فترة الصلح",
                        //            CustomActionType = CustomActionType.ExtendingReconciliationPeriod,
                        //            AllowChangeInternalStatus = false,
                        //            AllowChangeExternalStatus = false,
                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = false,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = false,
                        //            IsFinishedValue = false,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            LogMessage = "تم طلب تمديد فترة الصلح ",
                        //            ActionModeId = EventActionMode.GoNextStep,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "مدة فترة الصلح",
                        //                    LogMessage = "تم طلب تمديد فترة الصلح ",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Extend reconciliation period",
                        //                    LogMessage = "An extension of the reconciliation period was requested",
                        //                }
                        //            }
                        //        }
                        //    }
                        //}, // تنفيذ جلسة الصلح
                        //new WorkFlowStep()
                        //{
                        //    StepName = "مرحلة إتخاذ القرار من طرفى الدعوى",
                        //    ProcessActorType = ProcessActorType.PlaintiffAndDefendant,
                        //    WorkModeId = StepWorkMode.None,
                        //    Id = Guid.Parse("1290fdc7-d44f-436e-9828-27906963bebb"),
                        //    Order = 4,
                        //    ActorTypeId = StepActorType.Client,
                        //    CreationLogMessage = "جارى إتخاذ القرار من طرفى الدعوى",
                        //    CreatedOn = DateTime.Now,
                        //    CreatedBy = Guid.NewGuid(),
                        //    IsDeleted = false,
                        //    //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //    StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                        //    ShowForApplicant = true,
                        //    Translations = new List<WorkFlowStepTranslation>()
                        //    {
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "en-US",
                        //            StepName = "Request Studing",
                        //            CreationLogMessage = "The decision-making stage by both parties to the litigation"
                        //        },
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "ar-SA",
                        //            StepName = "مرحلة إتخاذ القرار من طرفى الدعوى",
                        //            CreationLogMessage = "جارى إتخاذ القرار من طرفى الدعوى",
                        //        }
                        //    },
                        //    WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                        //    {
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("df97cc45-1e98-441f-b1c1-329182807c30"),
                        //            EventName = "قبول",
                        //            CustomActionType = CustomActionType.ApproveMinutesOfTheMeeting,
                        //            AllowChangeInternalStatus = true,
                        //            AllowChangeExternalStatus = false,
                        //            ExternalStatusId = RequestStatus.AcceptanceOfTheReconciliationReport,
                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = false,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = false,
                        //            IsFinishedValue = false,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            ActionModeId = EventActionMode.GoNextStep,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "تغيير موعد جلسة الصلح",
                        //                    LogMessage = "تم تغيير موعد جلسة الصلح",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reschedule the reconciliation session",
                        //                    LogMessage = "A reconciliation session has been Rescheduled",
                        //                }
                        //            }
                        //        },
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("95d3d64e-dcda-4e2e-8f1a-2c74a159540a"),
                        //            EventName = "رفض",
                        //            CustomActionType = CustomActionType.RejectMinutesOfTheMeeting,
                        //            AllowChangeInternalStatus = true,
                        //            AllowChangeExternalStatus = false,
                        //            ExternalStatusId = RequestStatus.RejectionOfTheReconciliationReport,
                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = false,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = false,
                        //            IsFinishedValue = false,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            ActionModeId = EventActionMode.GoNextStep,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "تغيير موعد جلسة الصلح",
                        //                    LogMessage = "تم تغيير موعد جلسة الصلح",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reschedule the reconciliation session",
                        //                    LogMessage = "A reconciliation session has been Rescheduled",
                        //                }
                        //            }
                        //        },
                        //    }
                        //}, // مرحلة أتخاذ القرار من أطراف الدعوي
                        //new WorkFlowStep()
                        //{
                        //    StepName = "مرحلة إتخاذ القرار من المصلح",
                        //    ProcessActorType = ProcessActorType.Reformer,
                        //    WorkModeId = StepWorkMode.None,
                        //    Id = Guid.Parse("cbc28bea-e1d5-48d3-b40b-b2774cb28007"),
                        //    Order = 5,
                        //    ActorTypeId = StepActorType.Client,
                        //    CreationLogMessage = "جارى إتخاذ القرار من المصلح",
                        //    CreatedOn = DateTime.Now,
                        //    CreatedBy = Guid.NewGuid(),
                        //    IsDeleted = false,
                        //    //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //    StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                        //    ShowForApplicant = true,
                        //    Translations = new List<WorkFlowStepTranslation>()
                        //    {
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "en-US",
                        //            StepName = "Decision making stage from the reformer",
                        //            CreationLogMessage = "The decision is being made by the reformer"
                        //        },
                        //        new WorkFlowStepTranslation()
                        //        {
                        //            LanguageCode = "ar-SA",
                        //            StepName = "مرحلة إتخاذ القرار من المصلح",
                        //            CreationLogMessage = "جارى إتخاذ القرار من المصلح",
                        //        }
                        //    },
                        //    WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                        //    {
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("7bed8d46-078d-4efd-a499-ed327c859efb"),
                        //            EventName = "تنازل عن الحق",
                        //            AllowChangeInternalStatus = true,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.WaiverOfRight,
                        //            InternalStatusId = RequestStatus.WaiverOfRight,
                        //            LogMessage = "تم التنازل عن الحق",

                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = true,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = true,
                        //            IsFinishedValue = true,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "تنازل عن الحق",
                        //                    LogMessage = "تم التنازل عن الحق",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reschedule the reconciliation session",
                        //                    LogMessage = "A reconciliation session has been Rescheduled",
                        //                }
                        //            }
                        //        }, //تنازل عن الحق
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("6e2c2fc8-1dc0-4c7b-8b27-d1562d409ef5"),
                        //            AllowChangeInternalStatus = true,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.WaiverOfTheLitigation,
                        //            InternalStatusId = RequestStatus.WaiverOfTheLitigation,
                        //            LogMessage = "تم التنازل عن الدعوى",
                        //            EventName = "تنازل عن الدعوى",

                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = true,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = true,
                        //            IsFinishedValue = true,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    EventName = "تنازل عن الدعوى",
                        //                    LogMessage = "تم التنازل عن الدعوى",
                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Waiver Of The Litigation",
                        //                    LogMessage = "Waiver Of The Litigation Done",
                        //                }
                        //            }
                        //        }, //تنازل عن الدعوى
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("79b03b34-f4c3-463e-9de2-4a229ab395d2"),
                        //            AllowChangeInternalStatus = true,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.ReconciliationWasNotMade,
                        //            InternalStatusId = RequestStatus.ReconciliationWasNotMade,
                        //            LogMessage = "عدم الصلح",
                        //            EventName = "عدم الصلح",

                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = true,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = true,
                        //            IsFinishedValue = true,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    LogMessage = "عدم الصلح",
                        //                    EventName = "عدم الصلح"

                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reconciliation Was Not Made",
                        //                    LogMessage = "Reconciliation Was Not Made"
                        //                }
                        //            }
                        //        }, //عدم الصلح
                        //        new WorkFlowStepEvent()
                        //        {
                        //            Id = Guid.Parse("ed60a593-dacc-42a3-b88c-f5e91d6260b5"),
                        //            AllowChangeInternalStatus = true,
                        //            AllowChangeExternalStatus = true,
                        //            ExternalStatusId = RequestStatus.ReconciliationWasMade,
                        //            InternalStatusId = RequestStatus.ReconciliationWasMade,
                        //            LogMessage = "تم الصلح",
                        //            EventName = "تم الصلح",

                        //            AllowCompletionNotificationPlan = false,
                        //            //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                        //            AllowFileUpload = false,
                        //            AllowChangeIsActive = true,
                        //            IsActiveValue = false,
                        //            AllowNotes = false,
                        //            IsAdminApprovalRequired = false,
                        //            SameEmployee = false,
                        //            AllowChangeIsFinished = true,
                        //            IsFinishedValue = true,
                        //            AllowChangeIsAllowEdit = false,
                        //            IsAllowEditValue = false,
                        //            EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                        //            CreatedOn = DateTime.Now,
                        //            CreatedBy = Guid.NewGuid(),
                        //            IsDeleted = false,
                        //            ShowLogForApplicant = true,
                        //            ShowProcessingLogForApplicant = false,
                        //            Translations  = new List<WorkFlowStepEventTranslation>()
                        //            {
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "ar-SA",
                        //                    LogMessage = "تم الصلح",
                        //                    EventName = "تم الصلح",


                        //                },
                        //                new WorkFlowStepEventTranslation()
                        //                {
                        //                    LanguageCode = "en-US",
                        //                    EventName = "Reconciliation Was Made",
                        //                    LogMessage = "Reconciliation Was Made"
                        //                }
                        //            }
                        //        }, //تم الصلح

                        //    }
                        //}  // مرحلة أتخاذ القرار من المصلح
#endregion
                    }
                },
#region
                //// تحكيم
                //new WorkFlow()
                //{
                //    Id = Guid.Parse("e8a449f5-b1a0-49e8-bbf2-73ac94ef55f5"),
                //    WorkFlowName = "دعوة تحكيم",
                //    CreatedOn = DateTime.Now,
                //    CreatedBy = Guid.NewGuid(),
                //    IsDeleted = false,
                //    Translations = new List<WorkFlowTranslation>()
                //    {
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "ar-SA",
                //            WorkFlowName = "دعوى تحكيم"
                //        },
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "en-US",
                //            WorkFlowName = "Arbitration Lititgation"
                //        }
                //    },
                //    WorkFlowSteps = new List<WorkFlowStep>()
                //    {
                //        new WorkFlowStep()
                //        {
                //            ProcessActorType = ProcessActorType.Plaintiff,
                //            StepName = "معالجة الملاحظات",
                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("8ee69cf2-185b-4a31-b85a-f552d311a426"),
                //            Order = 0,
                //            ActorTypeId = StepActorType.Client,
                //            CreationLogMessage = "جارى معالجة الملاحظات",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.SentBackForEdit,
                //            ExternalStatusId = RequestStatus.SentBackForEdit,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("d1a94258-9e9f-4fbe-917f-dbb4de7e8d99"),
                //                    EventName = "إرسال طلب الدعوى",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Edited,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Edited,
                //                    AllowCompletionNotificationPlan = true,
                //                    //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //                    AllowFileUpload = true,
                //                    AllowChangeIsActive = true,
                //                    IsActiveValue = true,
                //                    AllowNotes = true,
                //                    IsAdminApprovalRequired = false,
                //                    SameEmployee = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = false,
                //                    AllowChangeIsAllowEdit = true,
                //                    IsAllowEditValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم معالجة الملاحظات وإرسال الدعوى",
                //                    ActionModeId = EventActionMode.GoPreviousStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إرسال طلب الدعوى",
                //                            LogMessage = "تم معالجة الملاحظات وإرسال الدعوى",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Litigation Request",
                //                            LogMessage = "Feedback has been processed and the request has been sent",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "معالجة الملاحظات",
                //                    CreationLogMessage = "جارى معالجة الملاحظات"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Processing Notes",
                //                    CreationLogMessage = "Feedback is being processed"
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "تدقيق الدعوى",
                //            ProcessActorType = ProcessActorType.Employee,

                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("25270ef5-fc30-4460-919d-d9977bed328f"),
                //            Order = 1,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم إنشاء الدعوى وجارى تدقيقها",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("657004f4-57e1-4715-addc-0e5ff0b00938"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.New,
                //            ExternalStatusId = RequestStatus.New,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("fbdbb687-8f7d-40d0-bad5-50433996cf66"),
                //                    EventName = "قبول",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Received,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Received,

                //                    AllowFileUpload = false,

                //                    IsActiveValue = false,
                //                    AllowChangeIsFinished = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    AllowNotes = false,
                //                    IsAdminApprovalRequired = false,
                //                    LogMessage = "تم قبول الدعوى",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    SameEmployee = false,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "قبول",
                //                            LogMessage = "تم قبول الدعوى",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Accept",
                //                            LogMessage = "Litigation has been accepted",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("36bbf82e-fccd-4487-bd05-adfb28adfbff"),
                //                    EventName = "رفض",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Rejected,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Rejected,
                //                    AllowCompletionNotificationPlan = true,
                //                    //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //                    AllowFileUpload = false,
                //                    IsAdminApprovalRequired = false,
                //                    AllowChangeIsActive = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = true,
                //                    IsActiveValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    SameEmployee = false,
                //                    AllowNotes = true,
                //                    LogMessage = "تم رفض الدعوى",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = true,
                //                    IsAllowEditValue = true,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "رفض",
                //                            LogMessage = "تم رفض الدعوى",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Reject",
                //                            LogMessage = "Request has been Rejected",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("5f3f6f13-599c-4665-8a51-7bb3f9267d15"),
                //                    EventName = "إعادة الدعوى للتعديل",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowFileUpload = true,
                //                    AllowChangeIsActive = false,
                //                    AllowNotes = true,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم إعادة الدعوى للتعديل",
                //                    ActionModeId = EventActionMode.SpecificStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    NextWorkFlowStepDestinationId = Guid.Parse("8ee69cf2-185b-4a31-b85a-f552d311a426"),
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إعادة  الدعوى للتعديل",
                //                            LogMessage = "تم دراسة الدعوى وإعادتها للتعديل",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Back For Edit",
                //                            LogMessage = "Request was studied and sent for edit",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Request Studing",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "دراسة الدعوى",
                //                    CreationLogMessage = "تم إنشاء الدعوى وجارى دراستها"
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "مرحلة التحكيم",
                //            ProcessActorType = ProcessActorType.Arbitrator,

                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("8a10f0be-9b7f-4c1d-a652-ae68286ccab4"),
                //            Order = 2,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم تدقيق الدعوى وجارى مرحلة التحكيم",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("a364bcfe-066a-473f-b852-621c5258945a"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.New,
                //            ExternalStatusId = RequestStatus.New,
                //            ShowForApplicant = true,
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Arbitration Step",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "مرحلة التحكيم",
                //                    CreationLogMessage = "تم تدقيق الدعوى وجارى مرحلة التحكيم",
                //                }
                //            }
                //        }
                //    }
                //},
                //// صلح وتحكيم
                //new WorkFlow()
                //{
                //    Id = Guid.Parse("832e07ba-354a-4d6e-8329-9adf2b28966a"),
                //    WorkFlowName = "دعوة صلح وتحكيم",
                //    CreatedOn = DateTime.Now,
                //    CreatedBy = Guid.NewGuid(),
                //    IsDeleted = false,
                //    Translations = new List<WorkFlowTranslation>()
                //    {
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "ar-SA",
                //            WorkFlowName = "دعوى صلح وتحكيم"
                //        },
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "en-US",
                //            WorkFlowName = "Conciliation And Arbitration Lititgation"
                //        }
                //    },
                //    WorkFlowSteps = new List<WorkFlowStep>()
                //    {
                //        new WorkFlowStep()
                //        {
                //            StepName = "معالجة الملاحظات",
                //            ProcessActorType = ProcessActorType.Plaintiff,
                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("42c0f621-ab89-444a-9970-a82933974b3d"),
                //            Order = 0,
                //            ActorTypeId = StepActorType.Client,
                //            CreationLogMessage = "جارى معالجة الملاحظات",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.SentBackForEdit,
                //            ExternalStatusId = RequestStatus.SentBackForEdit,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("ac118ddb-15c2-4dcf-9663-bdc5d198ad86"),
                //                    EventName = "إرسال طلب الدعوى",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Edited,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Edited,
                //                    AllowCompletionNotificationPlan = true,
                //                    //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //                    AllowFileUpload = true,
                //                    AllowChangeIsActive = true,
                //                    IsActiveValue = true,
                //                    AllowNotes = true,
                //                    IsAdminApprovalRequired = false,
                //                    SameEmployee = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = false,
                //                    AllowChangeIsAllowEdit = true,
                //                    IsAllowEditValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم معالجة الملاحظات وإرسال الدعوى",
                //                    ActionModeId = EventActionMode.GoPreviousStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إرسال طلب الدعوى",
                //                            LogMessage = "تم معالجة الملاحظات وإرسال الدعوى",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Litigation Request",
                //                            LogMessage = "Feedback has been processed and the request has been sent",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "معالجة الملاحظات",
                //                    CreationLogMessage = "جارى معالجة الملاحظات"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Processing Notes",
                //                    CreationLogMessage = "Feedback is being processed"
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "تدقيق الدعوى",
                //            ProcessActorType = ProcessActorType.Employee,

                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("f262881a-86b9-41eb-b679-ade1de303cb9"),
                //            Order = 1,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم إنشاء الدعوى وجارى تدقيقها",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("657004f4-57e1-4715-addc-0e5ff0b00938"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.New,
                //            ExternalStatusId = RequestStatus.New,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("4438fd81-dffb-4048-9ccc-323f33bdf59a"),
                //                    EventName = "قبول",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Received,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Received,

                //                    AllowFileUpload = false,

                //                    IsActiveValue = false,
                //                    AllowChangeIsFinished = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    AllowNotes = false,
                //                    IsAdminApprovalRequired = false,
                //                    LogMessage = "تم قبول الدعوى",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    SameEmployee = false,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "قبول",
                //                            LogMessage = "تم قبول الدعوى",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Accept",
                //                            LogMessage = "Litigation has been accepted",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("7df0b2ed-4a6b-46c9-b1a7-86d5d667e75b"),
                //                    EventName = "رفض",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Rejected,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Rejected,
                //                    AllowCompletionNotificationPlan = true,
                //                    //CompletionNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //                    AllowFileUpload = false,
                //                    IsAdminApprovalRequired = false,
                //                    AllowChangeIsActive = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = true,
                //                    IsActiveValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    SameEmployee = false,
                //                    AllowNotes = true,
                //                    LogMessage = "تم رفض الدعوى",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = true,
                //                    IsAllowEditValue = true,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "رفض",
                //                            LogMessage = "تم رفض الدعوى",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Reject",
                //                            LogMessage = "Request has been Rejected",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("556d214b-2a08-477d-b9da-7203f7fd59a5"),
                //                    EventName = "إعادة الدعوى للتعديل",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowFileUpload = true,
                //                    AllowChangeIsActive = false,
                //                    AllowNotes = true,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم إعادة الدعوى للتعديل",
                //                    ActionModeId = EventActionMode.SpecificStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    NextWorkFlowStepDestinationId = Guid.Parse("42c0f621-ab89-444a-9970-a82933974b3d"),
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إعادة  الدعوى للتعديل",
                //                            LogMessage = "تم دراسة الدعوى وإعادتها للتعديل",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Back For Edit",
                //                            LogMessage = "Request was studied and sent for edit",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Request Studing",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "دراسة الدعوى",
                //                    CreationLogMessage = "تم إنشاء الدعوى وجارى دراستها"
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "مرحلة الطلح",

                //            ProcessActorType = ProcessActorType.Reformer,
                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("ea2122a7-59bb-4a19-84ee-e7e9d177ad21"),
                //            Order = 2,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم تدقيق الدعوى وجارى مرحلة الصلح",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("fa69b94c-53f2-49ca-8384-072ea272fa08"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.New,
                //            ExternalStatusId = RequestStatus.New,
                //            ShowForApplicant = true,
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Request Studing",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "مرحلة الطلح",
                //                    CreationLogMessage = "تم تدقيق الدعوى وجارى مرحلة الصلح",
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "مرحلة التحكيم",
                //            ProcessActorType = ProcessActorType.Arbitrator,
                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("e6509bb4-4382-4796-b298-70e704e9d981"),
                //            Order = 3,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم مرحلة الصلح على الدعوى وجارى مرحلة التحكيم",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("a364bcfe-066a-473f-b852-621c5258945a"),
                //            StageId = _context.WorkFlowStepStages.ToList().FirstOrDefault().Id,
                //            InternalStatusId = RequestStatus.New,
                //            ExternalStatusId = RequestStatus.New,
                //            ShowForApplicant = true,
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Arbitration Step",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "مرحلة التحكيم",
                //                    CreationLogMessage = "تم مرحلة الصلح على الدعوى وجارى مرحلة التحكيم",
                //                }
                //            }
                //        }
                //    }
                //},
                
                //// طلبات المواطنين
                //new WorkFlow()
                //{
                //    Id = Guid.Parse("b7871240-69df-48cf-b9d4-2f644cb431d2"),
                //    WorkFlowName = "طلب المصلح",
                //    CreatedOn = DateTime.Now,
                //    CreatedBy = Guid.NewGuid(),
                //    IsDeleted = false,
                //    Translations = new List<WorkFlowTranslation>()
                //    {
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "ar-SA",
                //            WorkFlowName = "طلب المصلح",
                //        },
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "en-US",
                //            WorkFlowName = "Reformer Request"
                //        }
                //    },
                //    WorkFlowSteps = new List<WorkFlowStep>()
                //    {
                //        new WorkFlowStep()
                //        {
                //            StepName = "معالجة الملاحظات",
                //            WorkModeId = StepWorkMode.None,
                //            ProcessActorType = ProcessActorType.Plaintiff,
                //            Id = Guid.Parse("5783c080-c18c-4351-a20c-b07801a2de57"),
                //            Order = 0,
                //            ActorTypeId = StepActorType.Client,
                //            CreationLogMessage = "جارى معالجة الملاحظات",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            StageId = Guid.Parse("5406436a-9d9e-4996-81c6-87cc5e13a653"),
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("3513b065-7c5a-472a-b1c5-6d91faaf33bb"),
                //                    EventName = "إرسال الطلب",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Edited,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Edited,
                //                    AllowCompletionNotificationPlan = true,
                //                    //CompletionNotificationPlanId = Guid.Parse("8bcfc019-d5c5-4e37-8141-8111df074b17"),
                //                    AllowFileUpload = true,
                //                    AllowChangeIsActive = true,
                //                    IsActiveValue = true,
                //                    AllowNotes = true,
                //                    IsAdminApprovalRequired = false,
                //                    SameEmployee = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = false,
                //                    AllowChangeIsAllowEdit = true,
                //                    IsAllowEditValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم معالجة الملاحظات وإرسال الطلب",
                //                    ActionModeId = EventActionMode.GoPreviousStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إرسال الطلب",
                //                            LogMessage = "تم معالجة الملاحظات وإرسال الطلب",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Reformer Request",
                //                            LogMessage = "Feedback has been processed and the request has been sent",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "معالجة الملاحظات",
                //                    CreationLogMessage = "جارى معالجة الملاحظات"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Processing Notes",
                //                    CreationLogMessage = "Feedback is being processed"
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "مراجعة الطلب",
                //            ProcessActorType = ProcessActorType.Employee,

                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("c2af541d-83df-4a34-a7d4-831b6e32e3e1"),
                //            Order = 1,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم إنشاء الطلب وجارى مراجعته",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("fa69b94c-53f2-49ca-8384-072ea272fa08"),
                //            StageId = Guid.Parse("5406436a-9d9e-4996-81c6-87cc5e13a653"),
                //            InternalStatusId =  RequestStatus.New,
                //            ExternalStatusId =  RequestStatus.New,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("6c100bfa-5eb4-4838-84ae-6c7dcdf353cd"),
                //                    EventName = "قبول",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Received,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Received,

                //                    AllowFileUpload = false,

                //                    IsActiveValue = false,
                //                    AllowChangeIsFinished = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    AllowNotes = false,
                //                    IsAdminApprovalRequired = false,
                //                    LogMessage = "تم قبول الطلب",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    SameEmployee = false,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "قبول",
                //                            LogMessage = "تم قبول الطلب",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Accept",
                //                            LogMessage = "Request has been accepted",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("75cbea2d-bcb3-4e57-b9e4-649e23d4f48b"),
                //                    EventName = "رفض",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Rejected,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Rejected,
                //                    AllowCompletionNotificationPlan = true,
                //                    ////CompletionNotificationPlanId = Guid.Parse("0891dab2-0bad-49c8-8ff6-993644e4eeb1"),
                //                    AllowFileUpload = false,
                //                    IsAdminApprovalRequired = false,
                //                    AllowChangeIsActive = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = true,
                //                    IsActiveValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    SameEmployee = false,
                //                    AllowNotes = true,
                //                    LogMessage = "تم رفض الطلب",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = true,
                //                    IsAllowEditValue = true,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "رفض",
                //                            LogMessage = "تم رفض الطلب",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Reject",
                //                            LogMessage = "Request has been Rejected",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("45bcaa4f-58ff-481a-9565-8304a5a1d1f2"),
                //                    EventName = "إعادة الطلب للتعديل",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowFileUpload = false,
                //                    AllowChangeIsActive = false,
                //                    AllowNotes = true,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم إعادة الطلب للتعديل",
                //                    ActionModeId = EventActionMode.SpecificStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    NextWorkFlowStepDestinationId = Guid.Parse("5783c080-c18c-4351-a20c-b07801a2de57"),
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إعادة الطلب للتعديل",
                //                            LogMessage = "تم إعادة الطلب للتعديل",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Back For Edit",
                //                            LogMessage = "Request was studied and sent for edit",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Request Studing",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "دراسة الطلب",
                //                    CreationLogMessage = "تم إنشاء الطلب وجارى دراسته"
                //                }
                //            }
                //        }

                //    }
                //},
                //new WorkFlow()
                //{
                //    Id = Guid.Parse("dd6d470a-d29d-4545-82b1-32ffc82182cc"),
                //    WorkFlowName = "طلب المحكم",
                //    CreatedOn = DateTime.Now,
                //    CreatedBy = Guid.NewGuid(),
                //    IsDeleted = false,
                //    Translations = new List<WorkFlowTranslation>()
                //    {
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "ar-SA",
                //            WorkFlowName = "طلب المحكم",
                //        },
                //        new WorkFlowTranslation()
                //        {
                //            LanguageCode = "en-US",
                //            WorkFlowName = "Arbitrator Request"
                //        }
                //    },
                //    WorkFlowSteps = new List<WorkFlowStep>()
                //    {
                //        new WorkFlowStep()
                //        {
                //            StepName = "معالجة الملاحظات",
                //            ProcessActorType = ProcessActorType.Plaintiff,
                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("7696624b-a7de-477a-b08e-8345b779732e"),
                //            Order = 0,
                //            ActorTypeId = StepActorType.Client,
                //            CreationLogMessage = "جارى معالجة الملاحظات",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            StageId = Guid.Parse("5406436a-9d9e-4996-81c6-87cc5e13a653"),
                //            InternalStatusId = RequestStatus.SentBackForEdit,
                //            ExternalStatusId = RequestStatus.SentBackForEdit,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("6b9265c3-b585-40aa-bd5c-bf022b62efea"),
                //                    EventName = "إرسال الطلب",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Edited,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Edited,
                //                    AllowCompletionNotificationPlan = true,
                //                    //CompletionNotificationPlanId = Guid.Parse("8bcfc019-d5c5-4e37-8141-8111df074b17"),
                //                    AllowFileUpload = true,
                //                    AllowChangeIsActive = true,
                //                    IsActiveValue = true,
                //                    AllowNotes = true,
                //                    IsAdminApprovalRequired = false,
                //                    SameEmployee = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = false,
                //                    AllowChangeIsAllowEdit = true,
                //                    IsAllowEditValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم معالجة الملاحظات وإرسال الطلب",
                //                    ActionModeId = EventActionMode.GoPreviousStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إرسال الطلب",
                //                            LogMessage = "تم معالجة الملاحظات وإرسال الطلب",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Reformer Request",
                //                            LogMessage = "Feedback has been processed and the request has been sent",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "معالجة الملاحظات",
                //                    CreationLogMessage = "جارى معالجة الملاحظات"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Processing Notes",
                //                    CreationLogMessage = "Feedback is being processed"
                //                }
                //            }
                //        },
                //        new WorkFlowStep()
                //        {
                //            StepName = "مراجعة الطلب",
                //            ProcessActorType = ProcessActorType.Employee,
                //            WorkModeId = StepWorkMode.None,
                //            Id = Guid.Parse("fad68da1-8c59-4556-8c72-577cd2306e4e"),
                //            Order = 1,
                //            ActorTypeId = StepActorType.Department,
                //            CreationLogMessage = "تم إنشاء الطلب وجارى مراجعته",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = Guid.NewGuid(),
                //            IsDeleted = false,
                //            //CreationNotificationPlanId = Guid.Parse("f7c76133-2b18-4238-9081-d1fefbefb7cb"),
                //            DepartmentId = Guid.Parse("a364bcfe-066a-473f-b852-621c5258945a"),
                //            StageId = Guid.Parse("5406436a-9d9e-4996-81c6-87cc5e13a653"),
                //            InternalStatusId =  RequestStatus.New,
                //            ExternalStatusId =  RequestStatus.New,
                //            ShowForApplicant = true,
                //            WorkFlowStepEvents = new List<WorkFlowStepEvent>()
                //            {
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("5bd46d23-f8eb-458d-bca6-a1c7b8911371"),
                //                    EventName = "قبول",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Received,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Received,

                //                    AllowFileUpload = false,

                //                    IsActiveValue = false,
                //                    AllowChangeIsFinished = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    AllowNotes = false,
                //                    IsAdminApprovalRequired = false,
                //                    LogMessage = "تم قبول الطلب",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    SameEmployee = false,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "قبول",
                //                            LogMessage = "تم قبول الطلب",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Accept",
                //                            LogMessage = "Request has been accepted",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("a3d3b4fc-6c80-44a5-94a0-e4cc794897a1"),
                //                    EventName = "رفض",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.Rejected,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.Rejected,
                //                    AllowCompletionNotificationPlan = true,
                //                    ////CompletionNotificationPlanId = Guid.Parse("0891dab2-0bad-49c8-8ff6-993644e4eeb1"),
                //                    AllowFileUpload = false,
                //                    IsAdminApprovalRequired = false,
                //                    AllowChangeIsActive = true,
                //                    AllowChangeIsFinished = true,
                //                    IsFinishedValue = true,
                //                    IsActiveValue = false,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    SameEmployee = false,
                //                    AllowNotes = true,
                //                    LogMessage = "تم رفض الطلب",
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = true,
                //                    IsAllowEditValue = true,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "رفض",
                //                            LogMessage = "تم رفض الطلب",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Reject",
                //                            LogMessage = "Request has been Rejected",
                //                        }
                //                    }
                //                },
                //                new WorkFlowStepEvent()
                //                {
                //                    Id = Guid.Parse("ff702276-9c03-4c7a-9bf6-734548f5ca1a"),
                //                    EventName = "إعادة الطلب للتعديل",
                //                    AllowChangeInternalStatus = true,
                //                    InternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowChangeExternalStatus = true,
                //                    ExternalStatusId = RequestStatus.SentBackForEdit,
                //                    AllowFileUpload = false,
                //                    AllowChangeIsActive = false,
                //                    AllowNotes = true,
                //                    EventSecurityLevelId = EventSecurityLevel.UnSpecified,
                //                    LogMessage = "تم إعادة الطلب للتعديل",
                //                    ActionModeId = EventActionMode.SpecificStep,
                //                    CreatedOn = DateTime.Now,
                //                    CreatedBy = Guid.NewGuid(),
                //                    IsDeleted = false,
                //                    ShowLogForApplicant = true,
                //                    ShowProcessingLogForApplicant = true,
                //                    NextWorkFlowStepDestinationId = Guid.Parse("7696624b-a7de-477a-b08e-8345b779732e"),
                //                    Translations  = new List<WorkFlowStepEventTranslation>()
                //                    {
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "ar-SA",
                //                            EventName = "إعادة الطلب للتعديل",
                //                            LogMessage = "تم إعادة الطلب للتعديل",
                //                        },
                //                        new WorkFlowStepEventTranslation()
                //                        {
                //                            LanguageCode = "en-US",
                //                            EventName = "Send Back For Edit",
                //                            LogMessage = "Request was studied and sent for edit",
                //                        }
                //                    }
                //                }
                //            },
                //            Translations = new List<WorkFlowStepTranslation>()
                //            {
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "en-US",
                //                    StepName = "Request Studing",
                //                    CreationLogMessage = "Request has been created and is being Studied"
                //                },
                //                new WorkFlowStepTranslation()
                //                {
                //                    LanguageCode = "ar-SA",
                //                    StepName = "دراسة الطلب",
                //                    CreationLogMessage = "تم إنشاء الطلب وجارى دراسته"
                //                }
                //            }
                //        }

                //    }
                //}
#endregion
            };
        }
        #endregion
    }
}
