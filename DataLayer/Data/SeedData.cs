using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data
{
    public static class SeedData
    {
        public static async Task InitializeUser(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            if (await userManager.Users.AnyAsync())
                return;
            var roles = new List<IdentityRole<int>>()
                {
                new IdentityRole<int>{Name="Project Delivery Manager"},
                new IdentityRole<int>{Name="HR Officer"},
                };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            var Admin = new User()
            {
                Fname = "Hamza",

                Lname = "Alsahouri",
                UserName = "admin_nxt2024",
                Email = "admin2024@nextwo.net",
                EmailConfirmed = true,
                PhoneNumber = "0777777777",
                Gender = "male"

            };
            await userManager.CreateAsync(Admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(Admin, "Project Delivery Manager");
            var HR = new User()
            {
                Fname = "hr",
                Lname = "nxt",
                UserName = "hr_nxt2024",
                Email = "hr2024@nextwo.net",
                PhoneNumber = "0777777777",
                Gender = "female"
            };
            await userManager.CreateAsync(HR, "Pa$$w0rd");
            await userManager.AddToRoleAsync(HR, "HR Officer");
        }

    }
}
