using Core.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed;

public partial class LitigationRequestOwnerTypeSeedDataCreator
{
    /// <summary>
    /// مقدم عن نفسه – مفوض
    /// </summary>
    public static async Task SeedAsync(REACDBContext context)
    {
        if (await context.SysLitigationRequestOwnerTypes.AnyAsync()) return;
        await context.SysLitigationRequestOwnerTypes.AddRangeAsync(
            new List<SysLitigationRequestOwnerType>
            {
                    new SysLitigationRequestOwnerType
                    {
                        Id = LitigationRequestOwnerType.Personal,
                        LitigationRequestOwnerType = "مقدم عن نفسه",
                        Translations = new List<SysLitigationRequestOwnerTypeTranslation>
                        {
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "Personal",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "مقدم عن نفسه",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestOwnerType
                    {
                        Id = LitigationRequestOwnerType.Agent,
                        LitigationRequestOwnerType = "وكيل",
                        Translations=new List<SysLitigationRequestOwnerTypeTranslation>
                        {
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "Agent",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "وكيل",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                      new SysLitigationRequestOwnerType
                    {
                        Id = LitigationRequestOwnerType.Lawyer,
                        LitigationRequestOwnerType = "محامي",
                        Translations=new List<SysLitigationRequestOwnerTypeTranslation>
                        {
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "Lawyer",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "محامي",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                       new SysLitigationRequestOwnerType
                    {
                        Id = LitigationRequestOwnerType.CompanyRepresentative,
                        LitigationRequestOwnerType = "ممثل شركة",
                        Translations=new List<SysLitigationRequestOwnerTypeTranslation>
                        {
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "CompanyRepresentative",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestOwnerTypeTranslation
                            {
                                LitigationRequestOwnerType = "ممثل شركة",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    }
            });
    }

}
