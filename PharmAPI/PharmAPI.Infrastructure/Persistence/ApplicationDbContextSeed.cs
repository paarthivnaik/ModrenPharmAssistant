using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PharmAPI.Domain.Entities;

namespace PharmAPI.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAndRolesAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger logger)
    {
        // 1. Seed Roles
        var defaultRoles = new[] { "Admin", "Staff" };
        foreach (var roleName in defaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName));
                logger.LogInformation("Seeded role: {Role}", roleName);
            }
        }

        // 2. Seed Default Admin User
        var adminEmail = "admin@pharmassistant.com";
        var defaultAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (defaultAdmin == null)
        {
            defaultAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "System Administrator",
                ContactNo = "1234567890",
                Address = "Pharmacy HQ",
                IsActive = true
            };

            var result = await userManager.CreateAsync(defaultAdmin, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultAdmin, "Admin");
                logger.LogInformation("Seeded default admin user: {Email}", adminEmail);
            }
            else
            {
                logger.LogError("Failed to seed admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // 3. Seed Default Staff User
        var staffEmail = "staff@pharmassistant.com";
        var defaultStaff = await userManager.FindByEmailAsync(staffEmail);
        if (defaultStaff == null)
        {
            defaultStaff = new ApplicationUser
            {
                UserName = staffEmail,
                Email = staffEmail,
                EmailConfirmed = true,
                FullName = "Pharmacy Staff",
                ContactNo = "0987654321",
                Address = "Pharmacy Counter",
                IsActive = true
            };

            var result = await userManager.CreateAsync(defaultStaff, "Staff@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultStaff, "Staff");
                logger.LogInformation("Seeded default staff user: {Email}", staffEmail);
            }
            else
            {
                logger.LogError("Failed to seed staff user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
