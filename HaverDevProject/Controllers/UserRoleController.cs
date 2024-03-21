using HaverDevProject.CustomControllers;
using HaverDevProject.Data;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace HaverDevProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : CognizantController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                               .OrderBy(u => u.UserName)
                               .Select(user => new UserVM
                               {
                                   ID = user.Id,
                                   UserName = user.UserName,
                                   SelectedRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
                               })
                              .ToListAsync();
            return View(users);
        }

        //GET: Users/Create
        [HttpGet]
        public IActionResult Create()
        {
            PopulateRoles();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true
                };

                string defaultPassword = "Pa55w@rd";

                var createResult = await _userManager.CreateAsync(user, defaultPassword);

                if (createResult.Succeeded && !string.IsNullOrWhiteSpace(model.SelectedRole))
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    if (!roleResult.Succeeded)
                    {
                        // Handle error in role assignment
                        ModelState.AddModelError("", "Failed to assign role.");
                        PopulateRoles();
                        return View(model);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle user creation failure
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            PopulateRoles();
            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new CreateUserVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SelectedRole = userRoles.FirstOrDefault() 
            };

            PopulateRoles();
            return View(model);
        }

        // POST: Users/Edit/5
        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CreateUserVM model)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Email; 
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    PopulateRoles();
                    return View(model);
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRoleResult.Succeeded)
                {
                    foreach (var error in removeRoleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    PopulateRoles();
                    return View(model);
                }

                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    if (!addRoleResult.Succeeded)
                    {
                        foreach (var error in addRoleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        PopulateRoles();
                        return View(model);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateRoles(); 
            return View(model);
        }


        private void PopulateRoles()
        {
            var roles = _context.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
        }

        //private async Task UpdateUserRoles(string[] selectedRoles, UserVM userToUpdate)
        //{
        //    var UserRoles = userToUpdate.UserRoles;//Current roles use is in
        //    var _user = await _userManager.FindByIdAsync(userToUpdate.ID);//ApplicationUser

        //    if (selectedRoles == null)
        //    {
        //        //No roles selected so just remove any currently assigned
        //        foreach (var r in UserRoles)
        //        {
        //            await _userManager.RemoveFromRoleAsync(_user, r);
        //        }
        //    }
        //    else
        //    {
        //        //At least one role checked so loop through all the roles
        //        //and add or remove as required

        //        //We need to do this next line because foreach loops don't always work well
        //        //for data returned by EF when working async.  Pulling it into an IList<>
        //        //first means we can safely loop over the colleciton making async calls and avoid
        //        //the error 'New transaction is not allowed because there are other threads running in the session'
        //        IList<IdentityRole> allRoles = _context.Roles.ToList<IdentityRole>();

        //        foreach (var r in allRoles)
        //        {
        //            if (selectedRoles.Contains(r.Name))
        //            {
        //                if (!UserRoles.Contains(r.Name))
        //                {
        //                    await _userManager.AddToRoleAsync(_user, r.Name);
        //                }
        //            }
        //            else
        //            {
        //                if (UserRoles.Contains(r.Name))
        //                {
        //                    await _userManager.RemoveFromRoleAsync(_user, r.Name);
        //                }
        //            }
        //        }
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

