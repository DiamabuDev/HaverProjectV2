using HaverDevProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HaverDevProject.Views.Shared.Components
{
    public class UserDetailsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDetailsViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userEmail, string section)
        {
            if (userEmail == "Seed Data")
            {
                userEmail = GetEmailPerSection(section);
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            return View(user); 
        }

        private string GetEmailPerSection(string section)
        {
            return section switch
            {
                "qa" => "cspileanu@gmail.com",
                "eng" => "cspileanu@hotmail.com",
                "proc" => "lopezchavezvictorh@gmail.com",
                "op" => "diana.maldonado130@gmail.com",
                "reinsp" => "cspileanu@gmail.com",
                _ => "admin@outlook.com" 
            };
        }
    }
}
