using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<PnsDbContext>();

            // Ensure database is updated with migrations
            await context.Database.MigrateAsync();

            // Seed Roles
            await SeedRoles(roleManager);

            // Seed Admin User
            await SeedAdminUser(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedAdminUser(UserManager<AppUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@localhost.com");
            if (adminUser == null)
            {
                var user = new AppUser
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            else
            {
                // Ensure password is correct if user exists
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, "Admin@123");
                
                // Ensure role is assigned
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
