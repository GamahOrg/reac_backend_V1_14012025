using Core.Security;
using Core.Security.Authorization;
using Core.Security.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace REAC.EntityFrameworkCore.Seed
{
    public static class DefaultUsersCreator
    {
        /// <summary>
        /// Seed Default admin users 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task CreateAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            await SeedAdminUserAsync(userManager, roleManger, context);

            await SeedAdmin1UserAsync(userManager, roleManger, context);
            await SeedSubAdmin1UserAsync(userManager, roleManger, context);

            await SeedAdmin2UserAsync(userManager, roleManger, context);
            await SeedSubAdmin2UserAsync(userManager, roleManger, context);

            await SeedIntegratonUserAsync(userManager, roleManger, context);
        }

        public static async Task SeedIntegratonUserAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            var defaultUser = new User
            {
                Id = Guid.Parse("6b4dfc44-508e-45e1-9f12-ab96b5aec1c4"),
                UserName = "IntegrationUser",
                Email = "integrationuser@domain.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123456");
                await context.SaveChangesAsync();

                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.OnlyView.ToString() });
                await context.SaveChangesAsync();

            }

            //await roleManger.AddPermissionClaimsForUser(Roles.OnlyView.ToString(), context);

        }

        public static async Task SeedAdminUserAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            var defaultUser = new User
            {
                Id = Guid.Parse("c30016d7-556d-4a55-a4f0-ef9e5055a113"),
                UserName = "admin",
                Email = "superadmin@domain.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123456");
                await context.SaveChangesAsync();

                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.SuperAdmin.ToString() });
                await context.SaveChangesAsync();

            }

            await roleManger.AddPermissionClaimsForUser(Roles.SuperAdmin.ToString(), context);

        }

        public static async Task SeedAdmin1UserAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            var defaultUser = new User
            {
                Id = Guid.Parse("5b08a219-9ae2-4814-9f79-e070aaeee6c5"),
                UserName = "admin1",
                Email = "admin1@domain.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123456");
                await context.SaveChangesAsync();

                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.SuperAdmin.ToString() });
                await context.SaveChangesAsync();

            }

            await roleManger.AddPermissionClaimsForUser(Roles.SuperAdmin.ToString(), context);

        }

        public static async Task SeedSubAdmin1UserAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            var defaultUser = new User
            {
                Id = Guid.Parse("c1c3cfb1-c22d-4a2b-b036-6056443a61e8"),
                UserName = "subadmin1",
                Email = "subadmin1@domain.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123456");
                await context.SaveChangesAsync();

                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.SuperAdmin.ToString() });
                await context.SaveChangesAsync();

            }

            await roleManger.AddPermissionClaimsForUser(Roles.SuperAdmin.ToString(), context);

        }

        public static async Task SeedAdmin2UserAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            var defaultUser = new User
            {
                Id = Guid.Parse("afc6cdd8-1ec2-4abb-a34d-828de40bc308"),
                UserName = "admin2",
                Email = "admin2@domain.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123456");
                await context.SaveChangesAsync();

                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.SuperAdmin.ToString() });
                await context.SaveChangesAsync();

            }

            await roleManger.AddPermissionClaimsForUser(Roles.SuperAdmin.ToString(), context);

        }

        public static async Task SeedSubAdmin2UserAsync(UserManager<User> userManager, RoleManager<Role> roleManger, IdentityContext context)
        {
            var defaultUser = new User
            {
                Id = Guid.Parse("66e5c034-eca3-448e-96d9-c4a2a0b946fe"),
                UserName = "subadmin2",
                Email = "subadmin2@domain.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123456");
                await context.SaveChangesAsync();

                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.SuperAdmin.ToString() });
                await context.SaveChangesAsync();

            }

            await roleManger.AddPermissionClaimsForUser(Roles.SuperAdmin.ToString(), context);

        }

        #region Permission Seed

        public static async Task AddPermissionClaimsForUser(this RoleManager<Role> roleManager, string roleName, IdentityContext context)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GenerateAllPermissions();
            if (roleName == Roles.OnlyView.ToString())
                allPermissions = allPermissions.Where(x => x.Contains(Policy.View)).ToList();

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == Permissions.PolicyType && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Permissions.PolicyType, permission));
            }
            await context.SaveChangesAsync();

        }
        #endregion
    }
}