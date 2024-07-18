using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Seed
{
    public partial class NationalitySeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysNationalities.AnyAsync()) return;
            await context.SysNationalities.AddRangeAsync(new List<SysNationality>
            {
                new SysNationality
                {
                    Id = new Guid("236fab90-035e-44d5-8c30-d6f635141522"),
                    NationalityName = "مصري",
                    NationalityCode = 207,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("059ab94d-7799-4582-8fce-20f2eb8af32a"),
                    NationalityName = "سعودى",
                    NationalityCode = 113,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("a4f0ce7e-c007-4149-a491-f237085085aa"),
                    NationalityName = "بحريني",
                    NationalityCode = 103,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("a5b7140e-abb1-43b9-9511-6771705f25b8"),
                    NationalityName = "كويتي",
                    NationalityCode = 109,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("7ff15522-6c01-4837-b2ec-6dc64b295345"),
                    NationalityName = "عماني",
                    NationalityCode = 106,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("321e0114-0b5f-47d1-9367-586a519b2fe4"),
                    NationalityName = "قطري",
                    NationalityCode = 108,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("918380d5-0f1e-49eb-8cfa-b488b5d5af47"),
                    NationalityName = "إماراتي",
                    NationalityCode = 101,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("c2ee5854-38bc-4775-a82b-b9b1f221c4c9"),
                    NationalityName = "جزائري",
                    NationalityCode = 202,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("cbea9d02-4901-4c76-98ff-5d198a3f8703"),
                    NationalityName = "جزر القمر",
                    NationalityCode = 410,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("cd352163-c9ed-4f4d-8dcb-b5fec49da2be"),
                    NationalityName = "جيبوتي",
                    NationalityCode = 203,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("93b339c3-498f-40bd-aba1-a0805efd2cd5"),
                    NationalityName = "عراقي",
                    NationalityCode = 207,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("ebd9ee46-6cff-49bc-a2a0-cb7458a9fd2e"),
                    NationalityName = "أردني",
                    NationalityCode = 102,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("39af8bc5-5770-445e-a4f9-7387aa17407f"),
                    NationalityName = "لبناني",
                    NationalityCode = 110,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("f2f98dd2-e447-462b-8348-ed2567056858"),
                    NationalityName = "ليبي",
                    NationalityCode = 206,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("ed799edd-3d28-4bee-ad22-4778cc65ece1"),
                    NationalityName = "موريتاني",
                    NationalityCode = 209,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("6fce18dc-2948-452a-9fc2-faa5e8d6ffb7"),
                    NationalityName = "مغربي",
                    NationalityCode = 208,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("a59c43a0-20c7-4f2d-ae25-609afb2efb55"),
                    NationalityName = "فلسطيني",
                    NationalityCode = 107,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("5c57e556-a83d-4f97-8e3a-78edc68b0d60"),
                    NationalityName = "صومالي",
                    NationalityCode = 205,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("8864e502-1e2d-4f11-b8c7-bee8c2fd1ac5"),
                    NationalityName = "سوداني",
                    NationalityCode = 204,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("95969d94-0c49-4c70-b7c7-644779525f18"),
                    NationalityName = "سوري",
                    NationalityCode = 104,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("328d92ca-41f7-4f56-a0e7-fff64450eb06"),
                    NationalityName = "تونسي",
                    NationalityCode = 201,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new SysNationality
                {
                    Id = new Guid("51713d86-e6ce-4997-8c95-3aa6470b6592"),
                    NationalityName = "يمني",
                    NationalityCode = 111,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                }
            }
            );
            context.SaveChanges();
        }
    }
}