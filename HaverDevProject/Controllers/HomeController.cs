using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using HaverDevProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace HaverDevProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var primaryRole = roles.FirstOrDefault();

                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Role = primaryRole;

                if (primaryRole == "Admin" || primaryRole == "Quality")
                {
                    return View();
                }
                else
                {
                    switch (primaryRole)
                    {
                        case "Engineer":
                            return RedirectToAction("Index", "NcrEng");
                        case "Operations":
                            return RedirectToAction("Index", "NcrOperation");
                        case "Procurement":
                            return RedirectToAction("Index", "NcrProcurement");
                        default:
                            return RedirectToAction("AccessDenied", "Identity/Account/Login");
                    }
                }
            }
            else
            {
                ViewBag.FirstName = "Guest";
                ViewBag.LastName = "";
                ViewBag.Role = "None";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult KpiDashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
