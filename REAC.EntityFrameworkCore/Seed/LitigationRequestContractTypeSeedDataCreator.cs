using Core.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed;

public partial class LitigationRequestContractTypeSeedDataCreator
{
    /// <summary>
    /// إيجار – أملاك-وافى
    /// </summary>
    public static async Task SeedAsync(REACDBContext context)
    {
        if (await context.SysLitigationRequestContractTypes.AnyAsync()) return;
        await context.SysLitigationRequestContractTypes.AddRangeAsync(
            new List<SysLitigationRequestContractType>
            {
                //RealEstateMediation
                    new SysLitigationRequestContractType
                    {
                        Id = Domain.Shared.Enum.LitigationRequestContractType.Rent,
                        CodingPrefixSymbol = "R",
                        TypeName = "إيجار",
                        Translations=new List<SysLitigationRequestContractTypeTranslation>
                        {
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "Rent",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "إيجار",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestContractType
                    {
                        Id = Domain.Shared.Enum.LitigationRequestContractType.Properties,
                        TypeName = "أملاك",
                        CodingPrefixSymbol = "O",
                        Translations=new List<SysLitigationRequestContractTypeTranslation>
                        {
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "Properties",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "أملاك",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestContractType
                    {
                        Id = Domain.Shared.Enum.LitigationRequestContractType.Wafi,
                        TypeName = "وافى",
                        CodingPrefixSymbol = "W",
                        Translations=new List<SysLitigationRequestContractTypeTranslation>
                        {
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "Wafi",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "وافى",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestContractType
                    {
                        Id = Domain.Shared.Enum.LitigationRequestContractType.RealEstateBrokerage,
                        TypeName = "وساطه عقارية",
                        CodingPrefixSymbol = "B",
                        Translations=new List<SysLitigationRequestContractTypeTranslation>
                        {
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "Real Estate Brokerage",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "وساطه عقارية",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    },
                    new SysLitigationRequestContractType
                    {
                        Id = Domain.Shared.Enum.LitigationRequestContractType.OtherServices,
                        TypeName = "خدمات أخرى",
                        CodingPrefixSymbol = "N",
                        Translations=new List<SysLitigationRequestContractTypeTranslation>
                        {
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "Other Services",
                                LanguageCode = Constants.EnglishLanguage
                            },
                            new SysLitigationRequestContractTypeTranslation
                            {
                                TypeName = "خدمات أخرى",
                                LanguageCode = Constants.ArabicLanguage
                            }
                        }
                    }
            });
    }
}
