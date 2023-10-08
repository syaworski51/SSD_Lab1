using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using SSD_Lab1.Models;
using System.Runtime.InteropServices;

namespace SSD_Lab1.Data
{
    /// <summary>
    ///     Initializes the database with data for managers and employees.
    ///     Manager and employee data generated via https://www.mockaroo.com/
    /// </summary>
    public class Lab1DbInitializer
    {
        public static AppSecrets appSecrets { get; set; }

        public static string[] roles = { "Manager", "Employee" };
        
        public static ApplicationUser[] managers =
        {
            new ApplicationUser
            {
                FirstName = "Stanton",
                LastName = "Yaworski",
                UserName = "stanton.yaworski@mohawkcollege.ca",
                Email = "stanton.yaworski@mohawkcollege.ca",
                EmailConfirmed = true,
                BirthDate = new DateTimeOffset(2001, 12, 19, 0, 0, 0, new TimeSpan(-4, 0, 0))
            },
            new ApplicationUser
            {
                FirstName = "Dolorita",
                LastName = "Cranch",
                UserName = "dolorita.cranch@mohawkcollege.ca",
                Email = "dolorita.cranch@mohawkcollege.ca",
                EmailConfirmed = true,
                BirthDate = new DateTimeOffset(1977, 6, 30, 0, 0, 0, new TimeSpan(-4, 0, 0))
            }
        };
        
        public static ApplicationUser[] employees =
        {
            new ApplicationUser
            {
                FirstName = "Annmarie",
                LastName = "Voase",
                UserName = "annmarie.voase@mohawkcollege.ca",
                Email = "annmarie.voase@mohawkcollege.ca",
                EmailConfirmed = true,
                BirthDate = new DateTimeOffset(1987, 9, 1, 0, 0, 0, new TimeSpan(-4, 0, 0))
            },
            new ApplicationUser
            {
                FirstName = "Paolina",
                LastName = "Pittoli",
                UserName = "paolina.pittoli@mohawkcollege.ca",
                Email = "paolina.pittoli@mohawkcollege.ca",
                EmailConfirmed = true,
                BirthDate = new DateTimeOffset(1987, 4, 21, 0, 0, 0, new TimeSpan(-4, 0, 0))
            },
            new ApplicationUser
            {
                FirstName = "Jeniece",
                LastName = "Gotcliff",
                UserName = "jeniece.gotcliff@mohawkcollege.ca",
                Email = "jeniece.gotcliff@mohawkcollege.ca",
                EmailConfirmed = true,
                BirthDate = new DateTimeOffset(2000, 12, 5, 0, 0, 0, new TimeSpan(-4, 0, 0))
            },
            new ApplicationUser
            {
                FirstName = "Elbert",
                LastName = "Zmitruk",
                UserName = "elbert.zmitruk@mohawkcollege.ca",
                Email = "elbert.zmitruk@mohawkcollege.ca",
                EmailConfirmed = true,
                BirthDate = new DateTimeOffset(2002, 11, 26, 0, 0, 0, new TimeSpan(-4, 0, 0))
            }
        };

        public static async Task<int> SeedRolesAndUsers(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            int result;

            if (roleManager.Roles.Any())
            {
                Console.WriteLine("There are roles already in the database.");
            }
            else
            {
                result = await SeedRoles(roleManager);
                if (result != 0)
                {
                    Console.WriteLine("Seeding roles was not successful.");
                    return 1;
                }
            }
            

            if (userManager.Users.Any())
            {
                Console.WriteLine("There are users already in the database.");
                return 2;
            }

            result = await SeedUsers(userManager);
            if (result != 0)
            {
                Console.WriteLine("Seeding users was not successful.");
                return 3;
            }

            return 0;
        }

        public static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in roles)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                    return roleManager.Roles.Count();
            }

            return 0;
        }

        public static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            int step = 1;
            
            foreach (var manager in managers)
            {
                var result = await userManager.CreateAsync(manager, appSecrets.ManagerPassword);
                if (!result.Succeeded)
                    return step;

                result = await userManager.AddToRoleAsync(manager, roles[0]);
                if (!result.Succeeded)
                    return step + 1;
            }

            step += 2;
            foreach (var employee in employees)
            {
                var result = await userManager.CreateAsync(employee, appSecrets.EmployerPassword);
                if (!result.Succeeded)
                    return step;

                result = await userManager.AddToRoleAsync(employee, roles[1]);
                if (!result.Succeeded)
                    return step + 1;
            }

            return 0;
        }
    }
}
