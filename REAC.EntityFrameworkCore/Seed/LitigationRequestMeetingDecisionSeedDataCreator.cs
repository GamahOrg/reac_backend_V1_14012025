using Core.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed;

public partial class LitigationRequestMeetingDecisionSeedDataCreator
{
    public static async Task SeedAsync(REACDBContext context)
    {
        if (await context.SysLitigationRequestMeetingDecisions.AnyAsync()) return;
        await context.SysLitigationRequestMeetingDecisions.AddRangeAsync(
            new List<SysLitigationRequestMeetingDecision>
            {
                    new SysLitigationRequestMeetingDecision
                    {
                        Id = Domain.Shared.Enum.LitigationRequestMeetingDecision.WaiverOfRight,
                        MeetingDecision = "تنازل عن الحق",
                        Translations=new List<SysLitigationRequestMeetingDecisionTranslation>
                        {
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "Waiver of right",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "تنازل عن الحق",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                   new SysLitigationRequestMeetingDecision
                    {
                        Id = Domain.Shared.Enum.LitigationRequestMeetingDecision.WaiverOfTheLawsuit,
                        MeetingDecision = "تنازل عن الدعوى",
                        Translations=new List<SysLitigationRequestMeetingDecisionTranslation>
                        {
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "Waiver of the lawsuit",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "تنازل عن الدعوى",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                   new SysLitigationRequestMeetingDecision
                    {
                        Id =  Domain.Shared.Enum.LitigationRequestMeetingDecision.LackOfReconciliation,
                        MeetingDecision = "عدم الصُلح",
                        Translations=new List<SysLitigationRequestMeetingDecisionTranslation>
                        {
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "Lack of reconciliation",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "عدم الصُلح",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                   new SysLitigationRequestMeetingDecision
                   {
                        Id = Domain.Shared.Enum.LitigationRequestMeetingDecision.SaveAndClose,
                        MeetingDecision = "حفظ واغلاق",
                        Translations=new List<SysLitigationRequestMeetingDecisionTranslation>
                        {
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "Save and close",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestMeetingDecisionTranslation
                            {
                                MeetingDecision = "حفظ واغلاق",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                   }
            });
    }
}
