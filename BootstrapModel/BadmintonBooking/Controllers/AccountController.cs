using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
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
            Response response = new Response();
            bool createdByAdmin = (_signInManager.IsSignedIn(User) && User.IsInRole("Admin")) ? true: false;
                try
            {
                if (ModelState.IsValid)
                {
                    var chkEmail= await _userManager.FindByEmailAsync(model.Email);
                    if(chkEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email is already existed");
                        return View(model);

                    }
                    var user = new IdentityUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = createdByAdmin
                    };
                    var result=await _userManager.CreateAsync(user,model.Password);
                    if (result.Succeeded)
                    {
                        if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        {

                            return RedirectToAction("ListUsers", "Administration");
                        }

                        var userId = await _userManager.GetUserIdAsync(user);


                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmMail", "Account", new { userId=userId,Token = code},protocol:Request.Scheme);


                        string emailBody =  GetEmailBody(model.Email, "Email Confirmation", confirmationLink, "EmailConfirmation");
                        bool status=   await _emailSender.EmailSendAsync(model.Email, "Email Confirmation",emailBody);
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
            return View();
                
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
                        return View(model);
                    }
                    if (await _userManager.CheckPasswordAsync(checkEmail, model.Password) == false)
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password or Email");
                        return View(model);

                    }
                    bool confirmStatus = await _userManager.IsEmailConfirmedAsync(checkEmail);
                    if (!confirmStatus) {
                        ModelState.AddModelError("", "Email not confirmed");
                        return View(model);
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
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
            return RedirectToAction("Login", "Account");
        }
        public string GetEmailBody(string username,string? title,string? callbackUrl,string?EmailTemplateName)
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
                string emailBody =  GetEmailBody("", "Reset Password", callbackUrl, "ResetPassword");
                bool isSendEmail = await _emailSender.EmailSendAsync(forget.Email,"ResetPassword",emailBody);
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
        

        

    }
}
