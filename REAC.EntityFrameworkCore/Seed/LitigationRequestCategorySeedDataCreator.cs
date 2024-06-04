using Core.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed;

public partial class LitigationRequestCategorySeedDataCreator
{
    /// <summary>
    /// صلح – تحكيم-صلح وتحكيم
    /// </summary>
    public static async Task SeedAsync(REACDBContext context)
    {
        if (await context.SysLitigationRequestCategories.AnyAsync()) return;
        await context.SysLitigationRequestCategories.AddRangeAsync(
            new List<SysLitigationRequestCategory>
            {
                    new SysLitigationRequestCategory
                    {
                        Id = Domain.Shared.Enum.LitigationRequestCategory.Reconciliation,
                        CategoryName = "صلح",
                        Translations=new List<SysLitigationRequestCategoryTranslation>
                        {
                            new SysLitigationRequestCategoryTranslation
                            {
                                CategoryName = "Reconciliation",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestCategoryTranslation
                            {
                                CategoryName = "صلح",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestCategory
                    {
                        Id =  Domain.Shared.Enum.LitigationRequestCategory.Arbitration,
                        CategoryName = "تحكيم",
                        Translations=new List<SysLitigationRequestCategoryTranslation>
                        {
                            new SysLitigationRequestCategoryTranslation
                            {
                                CategoryName = "Arbitration",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestCategoryTranslation
                            {
                                CategoryName = "تحكيم",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestCategory
                    {
                        Id = Domain.Shared.Enum.LitigationRequestCategory.ReconciliationAndArbitration,
                        CategoryName = "صلح وتحكيم",
                        Translations=new List<SysLitigationRequestCategoryTranslation>
                        {
                            new SysLitigationRequestCategoryTranslation
                            {
                                CategoryName = "Reconciliation and arbitration",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestCategoryTranslation
                            {
                                CategoryName = "صلح وتحكيم",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
            });
    }
}
