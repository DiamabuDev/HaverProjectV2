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
using HaverDevProject.Utilities;

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
        public async Task<IActionResult> Index(string SearchUser, string SearchRole, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "FirstName")
        {
            ViewData["Filtering"] = "btn-block invisible";
            int numberFilters = 0;
            
            string[] sortOptions = new[] { "FirstName", "LastName", "Email", "Role" };

            var users = await (from u in _context.Users
                               .OrderBy(u => u.UserName)
                               select new CreateUserVM
                               {
                                   ID = u.Id,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Email = u.Email
                               }).ToListAsync();
            foreach (var u in users)
            {
                var _user = await _userManager.FindByIdAsync(u.ID);
                var roles = (List<string>)await _userManager.GetRolesAsync(_user);
                u.SelectedRole = roles[0]; 
                //Note: we needed the explicit cast above because GetRolesAsync() returns an IList<string>
            };                      

            //Filterig values                       
            if (!String.IsNullOrEmpty(SearchUser))
            {
                users = users.Where(u =>
                    u.FirstName.ToUpper().Contains(SearchUser.ToUpper()) ||
                    u.LastName.ToUpper().Contains(SearchUser.ToUpper()))
                    .ToList();
                numberFilters++;
            }

            if (!String.IsNullOrEmpty(SearchRole))
            {
                users = users.Where(u =>
                    u.SelectedRole.ToUpper().Contains(SearchRole.ToUpper()))
                    .ToList();
                numberFilters++;
            }

            //keep track of the number of filters 
            if (numberFilters != 0)
            {
                ViewData["Filtering"] = " btn-danger";
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
            }

            //Sorting columns
            //if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
            //{
            //    page = 1; //Reset page to start

            //    if (sortOptions.Contains(actionButton)) //Change of sort is requested
            //    {
            //        if (actionButton == sortField) //Reverse order on same field
            //        {
            //            sortDirection = sortDirection == "asc" ? "desc" : "asc";
            //        }
            //        sortField = actionButton; //Sort by the button clicked
            //    }
            //}

            ////Now we know which field and direction to sort by
            //if (sortField == "FirstName")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        users = users.OrderBy(p => p.FirstName).ToList();
            //        ViewData["filterApplied:UserFirstName"] = "<i class='bi bi-sort-up'></i>";
            //    }
            //    else
            //    {
            //        users = users.OrderByDescending(p => p.FirstName).ToList();
            //        ViewData["filterApplied:UserFirstName"] = "<i class='bi bi-sort-down'></i>";
            //    }
            //}
            //else if (sortField == "LastName")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        users = users.OrderBy(p => p.LastName).ToList();
            //        ViewData["filterApplied:UserLastName"] = "<i class='bi bi-sort-up'></i>";
            //    }
            //    else
            //    {
            //        users = users.OrderByDescending(p => p.LastName).ToList();
            //        ViewData["filterApplied:UserLastName"] = "<i class='bi bi-sort-down'></i>";
            //    }
            //}
            //else if (sortField == "Email") //Sorting by Email
            //{
            //    if (sortDirection == "asc")
            //    {
            //        users = users.OrderBy(p => p.Email).ToList();
            //        ViewData["filterApplied:UserEmail"] = "<i class='bi bi-sort-up'></i>";
            //    }
            //    else
            //    {
            //        users = users.OrderByDescending(p => p.Email).ToList();
            //        ViewData["filterApplied:UserEmail"] = "<i class='bi bi-sort-down'></i>";
            //    }
            //}
            //else //Sorting by Role
            //{
            //    if (sortDirection == "asc")
            //    {
            //        users = users.OrderBy(s => s.SelectedRole).ToList();
            //        ViewData["filterApplied:UserRole"] = "<i class='bi bi-sort-up'></i>";
            //    }
            //    else
            //    {
            //        users = users.OrderByDescending(s => s.SelectedRole).ToList();
            //        ViewData["filterApplied:UserRole"] = "<i class='bi bi-sort-down'></i>";
            //    }
            //}

            ////Set sort for next time
            //ViewData["sortField"] = sortField;
            //ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<CreateUserVM>.CreateAsync(
                users.AsQueryable(),
                page ?? 1,
                pageSize
            );

            return View(pagedData);
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

