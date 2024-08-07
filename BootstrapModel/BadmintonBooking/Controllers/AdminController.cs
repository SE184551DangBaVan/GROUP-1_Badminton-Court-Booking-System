﻿using BadmintonBooking.Models;
using BadmintonBooking.ViewModels;
using demobadminton.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BadmintonBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DemobadmintonContext _context;
        public AdminController(IWebHostEnvironment environment, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,DemobadmintonContext context)
        {
            this.environment = environment;
            this._UserManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Show(int page = 1, string sortOrder = "")
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var courtlist = context.Courts.ToList();
            //sort
            ViewBag.SortOrder = sortOrder;

            switch (sortOrder)
            {
                case "name_asc":
                    courtlist = courtlist.OrderBy(c => c.CoName).ToList();
                    break;
                case "name_desc":
                    courtlist = courtlist.OrderByDescending(c => c.CoName).ToList();
                    break;
                case "price_asc":
                    courtlist = courtlist.OrderBy(c => c.CoPrice).ToList();
                    break;
                case "price_desc":
                    courtlist = courtlist.OrderByDescending(c => c.CoPrice).ToList();
                    break;
                default:
                    courtlist = courtlist.OrderBy(c => c.CoId).ToList();
                    break;
            }

            // paging
            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(courtlist.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            courtlist = courtlist.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(courtlist);
        }
        public IActionResult AddCourt()
        {
            var addressList = _context.Courts.Where(c=>c.CoStatus==true)
       .Select(c => c.CoAddress)
       .Distinct()
       .ToList();
            ViewBag.AddressList = new SelectList(addressList);
            return View();
        }
        [HttpPost]
        public IActionResult AddCourt(Court model,string CoAddressTextBox)
        {
           /* try
            {
                ModelState.Remove("UserId");
                ModelState.Remove("User");
                ModelState.Remove("ImagePath");
          //      if(model.CoAddress == null && CoAddressTextBox == null)
            //    {
            //        TempData["error"] = "You cant leave blank address ";
             //       return View(model);
             //   } */
                if (model.CoAddress == null)
                {
                    ModelState.Remove("CoAddress");
                }else
                {
                    ModelState.Remove("CoAddressTextBox");
                }
          /*      if (ModelState.IsValid)
                {
          */

                    DemobadmintonContext context = new DemobadmintonContext();

                    string uniqueFileName = UploadImage(model);
                    string userid = _UserManager.GetUserId(User);
                    var data = new Court();

                    data.CoName = model.CoName;
                    if (model.CoAddress == null) {
                        data.CoAddress = CoAddressTextBox;
                    }
                    else
                    {
                        data.CoAddress = model.CoAddress;
                    }
                    
                    data.CoInfo = model.CoInfo;
                    data.CoPrice = model.CoPrice;
                    data.UserId = userid;
                    data.CoStatus = true;
                    data.CoPath = uniqueFileName;
                    
                    context.Courts.Add(data);
                    context.SaveChanges();
                    TempData["message"] = "Record has been saved successfully";


                    return RedirectToAction("Show");

                /*     }
                   ModelState.AddModelError(string.Empty, "Please check all fields again");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return View(model); */
            }


            public IActionResult DetailCourt(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);


        }
        public IActionResult EditCourt(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var addressList = _context.Courts.Where(c=>c.CoStatus==true)
      .Select(c => c.CoAddress)
      .Distinct()
      .ToList();
            /*var statusList = _context.Courts.Select(c => c.CoStatus)
       .Distinct()
       .ToList(); */
            //   ViewBag.StatusList = new SelectList(statusList);
            ViewBag.AddressList = new SelectList(addressList);
        
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data != null)
            {
                return View(data);
            }
            else
            {
                return RedirectToAction("Show", "Admin");
            }





        }
        [HttpPost]
        public IActionResult EditCourt(Court model)
        {
            DemobadmintonContext context = new DemobadmintonContext();
           
            var data = context.Courts.FirstOrDefault(c => c.CoId == model.CoId);
           /* try
            {
                ModelState.Remove("UserId");
                ModelState.Remove("User");
                ModelState.Remove("ImagePath");
                if (ModelState.IsValid)
                {
           */
                    string uniqueFileName = string.Empty;
                    if (model.ImagePath != null)
                    {
                        if (data.CoPath != null)
                        {
                            string filepath = Path.Combine(environment.WebRootPath, "Upload/Image", data.CoPath);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);

                            }
                        }
                        uniqueFileName = UploadImage(model);
                    }
                    data.CoId = model.CoId;
                    data.CoName = model.CoName;
                    data.CoAddress = model.CoAddress;
                    data.CoInfo = model.CoInfo;
                    data.CoPrice = model.CoPrice;
            if (model.CoStatus==false)
            {
                data.CoStatus = false;
            }
            else
            {
                data.CoStatus = true;
            }
                   
                    data.UserId = model.UserId;

                    if (model.ImagePath != null)
                    {
                        data.CoPath = uniqueFileName;
                    }
                    context.Courts.Update(data);
                    context.SaveChanges();
                    TempData["message"] = "Record has been updated successfully";
              /*  }
                else
                {
                    return View(model);
                }


            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }*/
            return RedirectToAction("Show", "Admin");
        }
        public IActionResult DeleteCourt(int id,int page,string sortOrder)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            bool hasCurrentBooking = context.Bookings
                .Any(b => b.CoId == id && context.TimeSlots
                    .Any(ts => ts.BId == b.BId && ts.TsDate >= currentDate));

            if (hasCurrentBooking)
            {
                TempData["error"] = "Court cannot be deleted as it has bookings for today or future.";
                return RedirectToAction("Show", new { page = page, sortOrder = sortOrder });
            }
            if (data == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                data.CoStatus = false;
                context.Courts.Update(data);
                context.SaveChanges();
                TempData["message"] = "Record has been deleted successfully";
            }
            return RedirectToAction("Show", "Admin");
        }
        public IActionResult Activate(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                data.CoStatus = true;
                context.Courts.Update(data);
                context.SaveChanges();
                TempData["message"] = "Record has been activated successfully";
            }
            return RedirectToAction("Show", "Admin");
        }





        private string UploadImage(Court model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Upload/Image/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }



            }
            return uniqueFileName;
        }
        public IActionResult CustomerInfo(int page = 1,string sortOrder="")
        {
            int NoOfRecordPerPage = 5;
            DemobadmintonContext context = new DemobadmintonContext();
            var data = _UserManager.Users.ToList();

            //var users = _context.Users.ToList();

            //var falseUserStatus = _context.UserActiveStatuses.Where(x => x.IsActive == false).Select(x => x.Id).ToList();
            //data.RemoveAll(x => falseUserStatus.Contains(x.Id));

            //// Assuming you are passing both the users and their statuses to the view
            //var model = users.Select(u => new
            //{
            //    User = u,
            //    UserActiveStatus = userStatuses.FirstOrDefault(s => s.Id == u.Id)
            //}).ToList();
            switch (sortOrder)
            {
                case "Admin":
                    data = data.Where(u => _UserManager.IsInRoleAsync(u, "Admin").Result).ToList();
                    break;
                case "Manager":
                    data = data.Where(u => _UserManager.IsInRoleAsync(u, "Manager").Result).ToList();
                    break;
                case "Staff":
                    data = data.Where(u => _UserManager.IsInRoleAsync(u, "Staff").Result).ToList();
                    break;
                case "Customer":
                    data = data.Where(u =>
                        !_UserManager.IsInRoleAsync(u, "Admin").Result &&
                        !_UserManager.IsInRoleAsync(u, "Manager").Result &&
                        !_UserManager.IsInRoleAsync(u, "Staff").Result
                    ).ToList();
                    break;
                default:
                    data = _UserManager.Users.ToList();
                    break;

            }

            //pagination
            int totalRecords = data.Count;
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            //ViewBag properties
            ViewBag.sortOrder = sortOrder;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;


            return View(pagedData);
        }
        // public IActionResult OverView()
        //{
        //  return View();
        //}



        //**********************************************
        [HttpGet]
        public async Task<IActionResult> EditUser(string UserId)
        {
            ViewData["Layout"] = null;
            //First Fetch the User Details by UserId
            var user = await _UserManager.FindByIdAsync(UserId);
            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            // GetRolesAsync returns the list of user Roles
            var userRoles = await _UserManager.GetRolesAsync(user);
            //Store all the information in the EditUserViewModel instance
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = userRoles
            };
            //Pass the Model to the View
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model, string? caller)
        {
            //First Fetch the User by Id from the database
            var user = await _UserManager.FindByIdAsync(model.Id);

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
                var result = await _UserManager.UpdateAsync(user);


                if (result.Succeeded)
                {
                    //Once user data updated redirect to the ListUsers view
                    if (!string.IsNullOrEmpty(caller))
                    {
                        TempData["message"] = "Profile updated successfully!";
                        TempData["info"] = "Log in again to take effect for your username on view panel";
                        return RedirectToAction("Profile", "Home");
                    }
                    else
                        return RedirectToAction("CustomerInfo");
                }
                else
                {
                    //In case any error, stay in the same view and show the model validation error
                    foreach (var error in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(caller))
                            TempData["error"] = "Profile updated failed";
                        TempData["info"] = "Our website hasnt supported unikey text names :( ";
                        return RedirectToAction("Profile", "Home");
                    }
                }
                if (!string.IsNullOrEmpty(caller)) return RedirectToAction("Profile", "Home");

                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string UserId)
        {
            ViewData["Layout"] = null;
            //First Fetch the User Information from the Identity database by user Id
            var user = await _UserManager.FindByIdAsync(UserId);
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
                if (await _UserManager.IsInRoleAsync(user, role.Name))
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

        //[HttpPost]
        //public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string UserId)
        //{
        //    var user = await _UserManager.FindByIdAsync(UserId);

        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
        //        return View("NotFound");
        //    }

        //    //fetch the list of roles the specified user belongs to
        //    var roles = await _UserManager.GetRolesAsync(user);

        //    //Then remove all the assigned roles for this user
        //    var result = await _UserManager.RemoveFromRolesAsync(user, roles);

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Cannot remove user existing roles");
        //        return View(model);
        //    }

        //    List<string> RolesToBeAssigned = model.Where(x => x.IsSelected).Select(y => y.RoleName).ToList();

        //    //If At least 1 Role is assigned, Any Method will return true
        //    if (RolesToBeAssigned.Any())
        //    {
        //        //add a user to multiple roles simultaneously

        //        result = await _UserManager.AddToRolesAsync(user, RolesToBeAssigned);
        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", "Cannot Add Selected Roles to User");
        //            return View(model);
        //        }
        //    }

        //    return RedirectToAction("EditUser", new { UserId = UserId });
        //}


        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(string SelectedRole, string UserId)
        {
            var user = await _UserManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            var roles = await _UserManager.GetRolesAsync(user);
            var result = await _UserManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(await ManageUserRoles(UserId));
            }

            if (!string.IsNullOrEmpty(SelectedRole))
            {
                var role = await _roleManager.FindByIdAsync(SelectedRole);
                if (role != null)
                {
                    result = await _UserManager.AddToRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot add selected role to user");
                        return View(await ManageUserRoles(UserId));
                    }
                }
            }
            return RedirectToAction("EditUser", new { UserId = UserId });
        }
        [HttpPost]
        public async Task<IActionResult> SuspendUser(string userId)
        {
            //First Fetch the User you want to Delete
            var user = await _UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                // Handle the case where the user wasn't found
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            else
            {
                //Delete the User Using DeleteAsync Method of UserManager Service
                if (_UserManager.GetUserId(User) == user.Id)
                {
                    ModelState.AddModelError("", "Can't delete the current user");
                    TempData["error"] = "Can not delete current user";
                    return RedirectToAction("CustomerInfo", "admin");
                }
                //actually delete 
                //var result = await _UserManager.DeleteAsync(user);

                //if (result.Succeeded)
                //{
                //    // Handle a successful delete
                //    return RedirectToAction("CustomerInfo");
                //}
                //else
                //{
                //    // Handle failure
                //    foreach (var error in result.Errors)
                //    {
                //        ModelState.AddModelError("", error.Description);
                //    }
                //}


                // Update user status to inactive
                var userStatus = _context.UserActiveStatuses.SingleOrDefault(u => u.Id == userId);
                if (userStatus == null)
                {
                    userStatus = new UserActiveStatus
                    {
                        Id = userId,
                        IsActive = false
                    };
                    _context.UserActiveStatuses.Add(userStatus);
                }
                else
                {
                    userStatus.IsActive = false;
                    _context.UserActiveStatuses.Update(userStatus);
                }

                await _context.SaveChangesAsync();

                TempData["success"] = "User status updated successfully";
                return RedirectToAction("CustomerInfo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            //First Fetch the User you want to Delete
            var user = await _UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                // Handle the case where the user wasn't found
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            else
            {
               

                // Update user status to active
                var userStatus = _context.UserActiveStatuses.SingleOrDefault(u => u.Id == userId);
                if (userStatus == null)
                {
                    userStatus = new UserActiveStatus
                    {
                        Id = userId,
                        IsActive = true
                    };
                    _context.UserActiveStatuses.Add(userStatus);
                }
                else
                {
                    userStatus.IsActive = true;
                    _context.UserActiveStatuses.Update(userStatus);
                }

                await _context.SaveChangesAsync();

                TempData["success"] = "User status updated successfully";
                return RedirectToAction("CustomerInfo");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["RoleError"] = "Role name cannot be empty.";
                return RedirectToAction("CustomerInfo");
            }

            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                TempData["RoleError"] = "Role already exists.";
                return RedirectToAction("CustomerInfo");
            }

            IdentityRole identityRole = new IdentityRole { Name = roleName };
            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                TempData["RoleSuccess"] = "Role created successfully.";
            }
            else
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["RoleError"] = errors;
            }

            return RedirectToAction("CustomerInfo");
        }

        [HttpGet]
        public IActionResult CreateNewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(RegisterVM model, string selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = true };
                var result = await _UserManager.CreateAsync(user, model.Password);
                var roleToAdd = await _roleManager.FindByNameAsync(selectedRole);
                if (result.Succeeded)
                {
                    if (roleToAdd != null)
                    {
                        var roleResult = await _UserManager.AddToRoleAsync(user, selectedRole);
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Error assigning roles to the user.");
                            return View(model);
                        }
                        var NewlyCreatedUserId = _UserManager.FindByEmailAsync(user.Email).Result.Id;
                        _context.UserActiveStatuses.Add(new UserActiveStatus { Id = NewlyCreatedUserId, IsActive = true });
                        _context.SaveChanges();
                    }


                    return RedirectToAction("CustomerInfo");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
