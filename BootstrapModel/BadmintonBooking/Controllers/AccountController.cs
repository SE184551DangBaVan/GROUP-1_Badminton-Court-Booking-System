using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BadmintonBooking.Models;
using BadmintonBooking.ViewModels;
using demobadminton.Repository.Interface;
using demobadminton.Repository.Service;
using demobadminton.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using NuGet.Common;

namespace demobadminton.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<AccountController> _logger;
        private readonly Repository.Interface.IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration configuration;
        public AccountController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger,
            Repository.Interface.IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            Response response = new Response();
            bool createdByAdmin = (_signInManager.IsSignedIn(User) && User.IsInRole("Admin")) ? true: false;
                try
            {
                if (ModelState.IsValid)
                {
                    //check email if already existed
                    var chkEmail= await _userManager.FindByEmailAsync(model.Email);
                    if(chkEmail != null)
                    {
                        TempData["error"] = "Email is already existed";
                        ModelState.AddModelError(string.Empty, "Email is already existed");
                        return View(model);

                    }
                    //check username if already existed
                    var chkUserName = await _userManager.FindByNameAsync(model.UserName);
                    if (chkUserName != null)
                    {
                        TempData["error"] = "Username is already existed";
                        ModelState.AddModelError(string.Empty, "Username is already existed");
                        return View(model);
                    }
                    var user = new IdentityUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        EmailConfirmed = createdByAdmin,
                        
                    };
                    var result=await _userManager.CreateAsync(user,model.Password);
                    if (result.Succeeded)
                    {
                        if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        {

                            return RedirectToAction("show", "admin");
                        }

                        var userId = await _userManager.GetUserIdAsync(user);


                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmMail", "Account", new { userId=userId,Token = code},protocol:Request.Scheme);


                        //    string emailBody =  GetEmailBody(model.Email, "Email Confirmation", confirmationLink, "EmailConfirmation");

                        // Email template
                        string html = $@"
                <table align='center'>
                   <tr>
                       <td>
                           <table style='background-color:#0d494c' width='500px' align='center'>
                               <tbody>
                                   <tr>
                                       <td>
                                           <table>
                                               <tr>
                                                   <td height='43' align='center'>
                                                       <h1 style='color:#fff;line-height:1rem'>Email Confirmation</h1>
                                                   </td>
                                               </tr>
                                           </table>
                                       </td>
                                   </tr>
                               </tbody>
                           </table>
                           <table style='width:500px;background-color:azure' align='center'>
                               <tr>
                                   <td style='padding:20px 20px 0px 0px' align='left'>
                                       <p style='text-align:justify;margin-top:0px'>
                                        You are receiving this message because you have recently signed up for a
                                        <br/>
                                        <b><em>Badminton Court website</em></b> 
                                        Confirm your email address by clicking the button below
                                       </p>
                                       <div style='text-align:center;margin-top:30px;margin-bottom:30px'>
                                           <a href='{confirmationLink}' style='background-color:blue;color:white;
            padding:8px 15px;text-decoration:none;border-radius:10px'>Confirm Email</a>
                                       </div>
                                       <p style='text-align:center;margin-top:0px'>This link will expire in <b style='color:red'><i>24 hours</i></b></p>
                                   <div style='text-align:center'>
                                       <a href='' target='_blank'></a>
                                   </div>
                                   </td>
                               </tr>
                           </table>
                       </td>
                   </tr>
                </table>";

                      //  string html = $"<p>Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a></p>";
                        bool status = await _emailSender.EmailSendAsync(model.Email, "Email Confirmation", html);
                        if (status)
                        {
                            response.Message = "Please check your email for verification";
                            return RedirectToAction("ForgetPasswordConfirmation", "Account",response);
                        }
                        //await _signInManager.SignInAsync(user,isPersistent:false);
                        return RedirectToAction("Index", "Home");

                    }
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                }

            }catch(Exception e)
            {

            }
            return View();
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null) {
            ViewData["ReturnUrl"] = ReturnUrl;
            var loginVM = new LoginVM
            {
                Schemes = _signInManager.GetExternalAuthenticationSchemesAsync().Result // Populate Schemes here
            };
            return View(loginVM);
                
                }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser checkEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (checkEmail == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email is not found");
                        TempData["error"] = "Email is not found";
                        return View(model);
                    }
                    // Check if the user has a null password and email is confirmed
                    if (checkEmail.PasswordHash == null && await _userManager.IsEmailConfirmedAsync(checkEmail))
                    {
                        ModelState.AddModelError(string.Empty, "This email is already registered for a Google account");
                        TempData["error"] = "This email is already registered for a Google account";
                        return View(model);
                    }
                    if (await _userManager.CheckPasswordAsync(checkEmail, model.Password) == false)
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password or Email");
                        TempData["error"] = "Incorrect Password or Email";
                        return View(model);

                    }
                    bool confirmStatus = await _userManager.IsEmailConfirmedAsync(checkEmail);
                    if (!confirmStatus) {
                        ModelState.AddModelError("", "Email not confirmed");
                        TempData["error"] = "Email not confirmed";
                        return View(model);
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(_userManager.FindByEmailAsync(model.Email).Result, model.Password, model.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else if (User.IsInRole("Admin"))
                            {
                                TempData["message"] = "Login Successfully as Admin!";
                                return RedirectToAction("Show", "Admin");
                            }
                            else if (User.IsInRole("Staff"))
                            {
                                TempData["message"] = "Login Successfully as Staff!";
                                return RedirectToAction("staff","Home");

                            } else
                            {
                                TempData["message"] = "Login Successfully!";
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    }

                }
            }
            catch(Exception )
            {
                throw;
            }
            return View(model);
            }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
			TempData["message"] = "Logout Successfully!";
			return RedirectToAction("Login", "Account");
        }
    /*    public string GetEmailBody(string username,string? title,string? callbackUrl,string?EmailTemplateName)
        {
            string url = configuration.GetValue<string>("Urls:LoginUrl");
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Template/"+ EmailTemplateName +".cshtml");
            string htmlStrig = System.IO.File.ReadAllText(path);
            htmlStrig = htmlStrig.Replace("{{title}}", title);
            htmlStrig = htmlStrig.Replace("{{Username}}", username);
            htmlStrig = htmlStrig.Replace("{{url}}", url);
            htmlStrig = htmlStrig.Replace("{{callbackUrl}}", callbackUrl);
            return htmlStrig;

        }

        */


        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        public async Task<IActionResult> ForgetPassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordOrUserNameVM forget)
        {
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");
            ModelState.Remove("Token");
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
            {
                return View(forget);
            }
            var user = await _userManager.FindByEmailAsync(forget.Email);
            if (user!=null)
            {
                var code =await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, Token = code },
                    protocol:Request.Scheme);
                //string emailBody =  GetEmailBody("", "Reset Password", callbackUrl, "ResetPassword");
                //Email template:
                string html = $@"
        <table align='center'>
           <tr>
               <td>
                   <table style='background-color:#0d494c' width='500px' align='center'>
                       <tbody>
                           <tr>
                               <td>
                                   <table>
                                       <tr>
                                           <td height='43' align='center'>
                                               <h1 style='color:#fff;line-height:1rem'>Reset Password</h1>
                                           </td>
                                       </tr>
                                   </table>
                               </td>
                           </tr>
                       </tbody>
                   </table>
                   <table style='width:500px;background-color:azure' align='center'>
                       <tr>
                           <td style='padding:20px 20px 0px 0px' align='left'>
                               <p style='text-align:justify;margin-top:0px'>
                                Seem like you forgot your password for <b><em>Badminton Court</em></b>.
                                If this is true, click below to reset your password.
                               </p>
                               <div style='text-align:center;margin-top:30px;margin-bottom:30px'>
                                   <a href='{callbackUrl}' style='background-color:blue;color:white;
            padding:8px 15px;text-decoration:none;border-radius:10px'>Reset Password</a>
                               </div>
                               <p style='text-align:justify;margin-top:0px'>If you did not forget your password, you can ignore this email.</p>
                           <div style='text-align:center'>
                               <a href='' target='_blank'></a>
                           </div>
                           </td>
                       </tr>
                   </table>
               </td>
           </tr>
        </table>";
                bool isSendEmail = await _emailSender.EmailSendAsync(forget.Email,"ResetPassword",html);
                if (isSendEmail)
                {
                    Response response = new Response();
                    response.Message = "Reset Password Link";
                    return RedirectToAction("ForgetPasswordConfirmation", "Account",response);
                }

            }
            return View();
        }
        public IActionResult ForgetPasswordConfirmation(Response response)
        {
            return View(response);
        }
        public IActionResult ResetPassword(string userId,string Token)
        {

            var model = new ForgetPasswordOrUserNameVM()
            {
                Token = Token,
                UserId = userId,

            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgetPasswordOrUserNameVM forget)
        {
            Response response=new Response();
            ModelState.Remove("Email");
            if (!ModelState.IsValid)
            {
                return View(forget);

            }
            var user =  await _userManager.FindByIdAsync(forget.UserId);
            if(user==null)
            {
                return View(forget);

            }
            var result = await _userManager.ResetPasswordAsync(user, forget.Token, forget.Password);
            if (result.Succeeded)
            {
                response.Message = "Your password has been successfully reset";
                return RedirectToAction("ForgetPasswordConfirmation", response);
            }
            return View(forget);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmMail(string userId,string Token)
        {
            Response response = new Response();
            if (User!=null&&Token!=null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View("Error");
                }
                else
                {
                    var result = await _userManager.ConfirmEmailAsync(user, Token);
                if (result.Succeeded)
                    {
                        response.Message = "Thank you for confirming your email";
                        return RedirectToAction("ForgetPasswordConfirmation", "Account",response);


                    }
                }
            }
            return View("Error");


        }
        public IActionResult ExternalLogin(string provider, string returnUrl = "")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "", string remoteError = "")
        {
            var loginVM = new LoginVM
            {
                Schemes = _signInManager.GetExternalAuthenticationSchemesAsync().Result
            };
            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from external login provide:{remoteError}");
                return View("Login", loginVM);

            }
            //info login

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", $"Error from external login provide:{remoteError}");
                return View("Login", loginVM);
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey
                , isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
               
                return RedirectToAction("index", "home");
            }
            else
            {
                var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var user = await _userManager.FindByEmailAsync(userEmail);
                    if (user == null)
                    {
                        user = new IdentityUser()
                        {
                            UserName = userEmail,
                            Email = userEmail,
                            EmailConfirmed = true,
                        };
                        await _userManager.CreateAsync(user);
                        //add role here


                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
					TempData["message"] = "Login Successfully!";
					return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", $"Something went wrong");
            return View("Login", loginVM);

        }


        //Change current password at profile
        [Authorize]
        public IActionResult ChangeCurrentPassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeCurrentPassword(ChangePasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if the current password is correct
            var user = await _userManager.GetUserAsync(User);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordValid)
            {
                ModelState.AddModelError(string.Empty, "Incorrect current password.");
                return View(model); // Return the view with an error if the current password is incorrect
            }


            // Change the user's password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model); // Return the view with errors if changing the password fails
            }

            // Redirect to a success page or action after successful password change
            TempData["message"] = "Password changed successfully.";
            return RedirectToAction("Index", "Home");
        }

        //edit user
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model, string? caller)
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
            var chkUserName = await _userManager.FindByNameAsync(model.UserName);
            if (chkUserName != null)
            {
                TempData["error"] = "Username is already existed or you can not update your current name as new name";
                ModelState.AddModelError(string.Empty, "Username is already existed");
                return RedirectToAction("Profile", "Home");
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
                    if (!string.IsNullOrEmpty(caller))
                    {
                        TempData["message"] = "Profile updated successfully!";
                        TempData["info"] = "Log in again to take effect for your username on view panel";
                        return RedirectToAction("Profile", "Home");
                    }
                    else
                        return RedirectToAction("ListUsers");
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
                        ModelState.AddModelError("", error.Description);
                    }
                }
                if (!string.IsNullOrEmpty(caller)) return RedirectToAction("Profile", "Home");

                return View(model);
            }

        }
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM model)
        {
           Response response = new Response();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["error"] = "Email is not found";
                TempData["info"] = "Have you created your account yet?";
                ModelState.AddModelError(string.Empty, "Email is not found");
                return View(model);
            }
            if (user.EmailConfirmed)
            {
                TempData["info"] = "Email is already confirmed";
                ModelState.AddModelError(string.Empty, "Email is already confirmed");
                return View(model);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmMail", "Account", new { userId = user.Id, Token = code }, protocol: Request.Scheme);
            //REsend email confirm template
            string html = $@"
        <table align='center'>
           <tr>
               <td>
                   <table style='background-color:#0d494c' width='500px' align='center'>
                       <tbody>
                           <tr>
                               <td>
                                   <table>
                                       <tr>
                                           <td height='43' align='center'>
                                               <h3 style='color:#fff;line-height:1rem'>Resend Email Confirmation</h3>
                                           </td>
                                       </tr>
                                   </table>
                               </td>
                           </tr>
                       </tbody>
                   </table>
                   <table style='width:500px;background-color:azure' align='center'>
                       <tr>
                           <td style='padding:20px 20px 0px 0px' align='left'>
                               <p style='text-align:justify;margin-top:0px'>
                                Seem like you want to confirm your account for <b><em>Badminton Court</em></b>.
                                If this is true, click below to authenticate your account.
                               </p>
                               <div style='text-align:center;margin-top:30px;margin-bottom:30px'>
                                   <a href='{confirmationLink}' style='background-color:blue;color:white;
            padding:8px 15px;text-decoration:none;border-radius:10px'>Confirm Email</a>
                               </div>
                               <p style='text-align:justify;margin-top:0px'>Thank you for trusting our services.</p>
                           <div style='text-align:center'>
                               <a href='' target='_blank'></a>
                           </div>
                           </td>
                       </tr>
                   </table>
               </td>
           </tr>
        </table>";

            bool status = await _emailSender.EmailSendAsync(model.Email, "Resend Email Confirmation", html);
            if (status)
            {
                response.Message = "Please check your email for verification";
                return RedirectToAction("ForgetPasswordConfirmation", "Account", response);
            }

            return View();
        }






    }
}
