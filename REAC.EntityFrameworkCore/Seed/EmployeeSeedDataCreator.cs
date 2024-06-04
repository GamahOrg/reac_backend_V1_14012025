using Core.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using REAC.EntityFrameworkCore.Entities.System;

namespace Ticketing.EntityFrameworkCore.SeedData
{
    public static class EmployeeSeedDataCreator
    {
        public static async Task SeedAsync(REACDBContext context)
        {
            if (await context.SysDepartments.AnyAsync()) return;

            await context.SysDepartments.AddRangeAsync(
                new List<SysDepartment>
                {
                    new SysDepartment
                    {
                        Id = Guid.NewGuid(),
                        DepartmentName = "إدراة الصلح",
                        Translations=new List<SysDepartmentTranslation>
                        {
                            new SysDepartmentTranslation
                            {
                                DepartmentName="إدراة الصلح",
                                LanguageCode=Constants.EnglishLanguage
                            },
                            new SysDepartmentTranslation
                            {
                                DepartmentName="Conciliation Department",
                                LanguageCode=Constants.ArabicLanguage
                            }
                        },
                        SysEmployees = new List<SysEmployee>(){
                            new SysEmployee()
                            {
                                 Id = Guid.NewGuid(),
                                 CreatedOn = DateTime.Now,
                                 Email = "Con@admin.com",
                                 EmployeeName = "مدير إدارة الصلح",
                                 IsActive = true,
                                 IsLocked = false,
                                 IsDeleted = false,
                                 IsAdmin = true,
                                 Mobile = "0558500225",
                                 UserId = new Guid("c30016d7-556d-4a55-a4f0-ef9e5055a113"),
                                 Translations = new List<SysEmployeeTranslation>()
                                 {
                                     new SysEmployeeTranslation()
                                     {
                                         EmployeeName = "Conciliation Department admin",
                                         LanguageCode=Constants.EnglishLanguage
                                     },
                                     new SysEmployeeTranslation()
                                     {
                                         EmployeeName = "مدير إدارة الصلح",
                                         LanguageCode=Constants.ArabicLanguage
                                     }
                                 }
                            },
                            new SysEmployee()
                            {
                                Id= Guid.NewGuid(),
                                 CreatedOn = DateTime.Now,
                                 Email = "Con@Employee.com",
                                 EmployeeName = "موظف إدارة الصلح",
                                 IsActive = true,
                                 IsLocked = false,
                                 IsDeleted = false,
                                 IsAdmin = true,
                                 Mobile = "0558500225",
                                 UserId = new Guid("c30016d7-556d-4a55-a4f0-ef9e5055a113"),
                                 Translations = new List<SysEmployeeTranslation>()
                                 {
                                     new SysEmployeeTranslation()
                                     {
                                         EmployeeName = "Conciliation Department Employee",
                                         LanguageCode=Constants.EnglishLanguage
                                     },
                                     new SysEmployeeTranslation()
                                     {
                                         EmployeeName = "موظف إدارة الصلح",
                                         LanguageCode=Constants.ArabicLanguage
                                     }
                                 }
                            }
                        }
                    }
                });
        }
    }
}
