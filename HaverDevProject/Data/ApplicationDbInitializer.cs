using HaverDevProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HaverDevProject.Data
{
    public static class ApplicationDbInitializer
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            ApplicationDbContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();

                //Create Roles
                var RoleManager = applicationBuilder.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Admin", "Quality", "Engineer", "Operations", "Procurement" };
                IdentityResult roleResult;
                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
                //Create Users
                var userManager = applicationBuilder.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (userManager.FindByEmailAsync("admin@outlook.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Leslie",
                        LastName = "Pentland",
                        UserName = "admin@outlook.com",
                        Email = "admin@outlook.com",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("quality@outlook.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Rachel",
                        LastName = "May",
                        UserName = "quality@outlook.com",
                        Email = "quality@outlook.com",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Quality").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("engineer@outlook.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Michael",
                        LastName = "Smith",
                        UserName = "engineer@outlook.com",
                        Email = "engineer@outlook.com",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Engineer").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("operations@outlook.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Leslie",
                        LastName = "Pentland",
                        UserName = "operations@outlook.com",
                        Email = "operations@outlook.com",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Operations").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("procurement@outlook.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Rick",
                        LastName = "Johnson",
                        UserName = "procurement@outlook.com",
                        Email = "procurement@outlook.com",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Procurement").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("vlopezchavez1@ncstudents.niagaracollege.ca").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Victor",
                        LastName = "Lopez",
                        UserName = "vlopezchavez1@ncstudents.niagaracollege.ca",
                        Email = "vlopezchavez1@ncstudents.niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("ispirleanu1@ncstudents.niagaracollege.ca").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Catalin",
                        LastName = "Spirleanu",
                        UserName = "ispirleanu1@ncstudents.niagaracollege.ca",
                        Email = "ispirleanu1@ncstudents.niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("dmaldonadoburgo1@ncstudents.niagaracollege.ca").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Diana",
                        LastName = "Maldonado",
                        UserName = "dmaldonadoburgo1@ncstudents.niagaracollege.ca",
                        Email = "dmaldonadoburgo1@ncstudents.niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("rcote6@ncstudents.niagaracollege.ca").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Ryan",
                        LastName = "Cote",
                        UserName = "rcote6@ncstudents.niagaracollege.ca",
                        Email = "rcote6@ncstudents.niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("ntemple1@ncstudents.niagaracollege.ca").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Nigel",
                        LastName = "Temple",
                        UserName = "ntemple1@ncstudents.niagaracollege.ca",
                        Email = "ntemple1@ncstudents.niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                if (userManager.FindByEmailAsync("jcastanomejia1@ncstudents.niagaracollege.ca").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Jorge",
                        LastName = "Castaño",
                        UserName = "jcastanomejia1@ncstudents.niagaracollege.ca",
                        Email = "jcastanomejia1@ncstudents.niagaracollege.ca",
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Pa55w@rd").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
