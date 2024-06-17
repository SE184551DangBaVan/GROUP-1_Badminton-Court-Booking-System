using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BadmintonBooking.ViewModels;

namespace BadmintonBooking.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string UserId)
        {
            //First Fetch the User Details by UserId
            var user = await _userManager.FindByIdAsync(UserId);
            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            //Store all the information in the EditUserViewModel instance
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            //Pass the Model to the View
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            //First Fetch the User by Id from the database
            var user = await _userManager.FindByIdAsync(model.Id);

            //Check if the User Exists in the database
            if (user == null)
            {
                //If the User does not exists in the database, then return Not Found Error View
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                //If the User Exists, then proceed and update the data
                //Populate the user instance with the data from EditUserViewModel
                user.Email = model.Email;
                user.UserName = model.UserName;

                //UpdateAsync Method will update the user data in the AspNetUsers Identity table
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    //Once user data updated redirect to the ListUsers view
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    //In case any error, stay in the same view and show the model validation error
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            //First Fetch the User you want to Delete
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                // Handle the case where the user wasn't found
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            else
            {
                //Delete the User Using DeleteAsync Method of UserManager Service
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // Handle a successful delete
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    // Handle failure
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View("ListUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string UserId)
        {
            //First Fetch the User Information from the Identity database by user Id
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                //handle if User Not Found in the Database
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            //Store the UserId in the ViewBag which is required while updating the Data
            //Store the UserName in the ViewBag which is used for displaying purpose
            ViewBag.UserId = UserId;
            ViewBag.UserName = user.UserName;
            //Create a List to Hold all the Roles Information
            var model = new List<UserRolesViewModel>();
            //Loop Through Each role and populate the model 
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                //Check if the Role is already assigned to this user
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                //Add the userRolesViewModel to the model
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            //fetch the list of roles the specified user belongs to
            var roles = await _userManager.GetRolesAsync(user);

            //Then remove all the assigned roles for this user
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            List<string> RolesToBeAssigned = model.Where(x => x.IsSelected).Select(y => y.RoleName).ToList();

            //If At least 1 Role is assigned, Any Method will return true
            if (RolesToBeAssigned.Any())
            {
                //add a user to multiple roles simultaneously

                result = await _userManager.AddToRolesAsync(user, RolesToBeAssigned);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot Add Selected Roles to User");
                    return View(model);
                }
            }

            return RedirectToAction("EditUser", new { UserId = UserId });
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(model?.RoleName);
                if (roleExists)
                {
                    ModelState.AddModelError(string.Empty, "Role already exists");
                }
                else
                {
                    IdentityRole identityRole = new IdentityRole() { Name = model?.RoleName };
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            //First Get the role information from the database
            IdentityRole? role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Handle the scenario when the role is not found
                return View("Error");
            }

            //Populate the EditRoleViewModel from the data retrived from the database
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
                // You can add other properties here if needed
            };

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    // Handle the scenario when the role is not found
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    // Update other properties if needed

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles"); // Redirect to the roles list
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Role not found, handle accordingly
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                // Role deletion successful
                return RedirectToAction("ListRoles"); // Redirect to the roles list page
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // If we reach here, something went wrong, return to the view
            return View("ListRoles", await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            ViewBag.RollName = role.Name;
            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //First check whether the Role Exists or not
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult? result;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    //If IsSelected is true and User is not already in this role, then add the user
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    //If IsSelected is false and User is already in this role, then remove the user
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    //Don't do anything simply continue the loop
                    continue;
                }

                //If you add or remove any user, please check the Succeeded of the IdentityResult
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { roleId = roleId });
                }
            }

            return RedirectToAction("EditRole", new { roleId = roleId });
        }
    }
}
