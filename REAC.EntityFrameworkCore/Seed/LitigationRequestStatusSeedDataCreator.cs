using Core.GenericRepository.Shared.BaseEntities;
using Core.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed;

public partial class LitigationRequestStatusSeedDataCreator : FullAuditedBaseEntity<int>
{
    public static async Task SeedAsync(REACDBContext context)
    {
        if (await context.SysLitigationRequestStatuses.AnyAsync()) return;
        await context.SysLitigationRequestStatuses.AddRangeAsync(
            new List<SysLitigationRequestStatus>
            {
                    new SysLitigationRequestStatus
                    {
                        Id =  Domain.Shared.Enum.RequestStatus.New,
                        RequestStatus = "جديدة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "New",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "جديدة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id =  Domain.Shared.Enum.RequestStatus.Received,
                        RequestStatus = "مُستلمة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Received",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "مُستلمة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.AsessionTimeHasBeenSet,
                        RequestStatus = "تم تحديد ميعاد جلسة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "A session time has been set",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "تم تحديد ميعاد جلسة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.TheSessionWasCompleted,
                        RequestStatus = "تمت الجلسة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "The session was completed",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "تمت الجلسة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id =  Domain.Shared.Enum.RequestStatus.TheMinutesOfTheMeetingWereWritten,
                        RequestStatus = "تم كتابة محضر الجلسة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "The minutes of the meeting were written",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "تم كتابة محضر الجلسة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.WaiverOfRight,
                        RequestStatus = "تنازل عن الحق",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Waiver of right",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "تنازل عن الحق",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id =  Domain.Shared.Enum.RequestStatus.WaiverOfTheLawsuit,
                        RequestStatus = "تنازل عن الدعوى",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Waiver of the lawsuit",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "تنازل عن الدعوى",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.LackOfReconciliation,
                        RequestStatus = "عدم الصُلح",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Lack of reconciliation",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "عدم الصُلح",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.SaveAndClose,
                        RequestStatus = "حفظ وإغلاق",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Save and close",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "حفظ وإغلاق",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.SaveAsDraft,
                        RequestStatus = "حفظ كمسودة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Save as Draft",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus =  "حفظ كمسودة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.Rejected,
                        RequestStatus = "مرفوضة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Rejected",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus =  "مرفوضة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.SentBackForEdit,
                        RequestStatus = "معادة للتعديل",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "SendBackForEdit",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus =  "معادة للتعديل",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.Edited,
                        RequestStatus = "تم التعديل",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "Edited",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus =  "تم التعديل",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestStatus
                    {
                        Id = Domain.Shared.Enum.RequestStatus.CharterRequestSent,
                        RequestStatus = "قيد المشارطة",
                        CreatedOn = DateTime.Now,
                        Translations=new List<SysLitigationRequestStatusTranslation>
                        {
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus = "CharterRequestSent ",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestStatusTranslation
                            {
                                RequestStatus =  "قيد المشارطة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
            });
        context.SaveChanges();
    }
}
