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

            // Seed Notification Types
            await SeedNotificationTypes(context);

            // Seed Priorities
            await SeedPriorities(context);
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
        private static async Task SeedNotificationTypes(PnsDbContext context)
        {
            if (!context.NotificationTypes.Any(t => t.Name == "Email"))
            {
                context.NotificationTypes.Add(new NotificationType { Id = Guid.NewGuid(), Name = "Email", Description = "Email notifications" });
            }

            if (!context.NotificationTypes.Any(t => t.Name == "SMS"))
            {
                context.NotificationTypes.Add(new NotificationType { Id = Guid.NewGuid(), Name = "SMS", Description = "SMS text messages" });
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedPriorities(PnsDbContext context)
        {
            var priorities = new[]
            {
                new { Name = "Low", Desc = "Low priority", Level = 1 },
                new { Name = "Normal", Desc = "Normal priority", Level = 2 },
                new { Name = "High", Desc = "High priority", Level = 3 }
            };

            foreach (var p in priorities)
            {
                if (!context.Priorities.Any(pr => pr.Name == p.Name))
                {
                    context.Priorities.Add(new Priority { Id = Guid.NewGuid(), Name = p.Name, Description = p.Desc, Level = p.Level });
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
