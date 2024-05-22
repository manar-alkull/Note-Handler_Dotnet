using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NoteManager.Models;
using NoteManager.Models.Requests;
using NoteManager.tools;
using NuGet.Common;
using System.Text.Encodings.Web;

namespace ASPNETCoreIdentityDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await confirmEmail( user);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        //----------------------------------------  login ----------------------------------------------------

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Handle successful login
                    return RedirectToAction("index", "home");
                }
                if (result.RequiresTwoFactor)
                {
                    // Handle two-factor authentication case
                }
                if (result.IsLockedOut)
                {
                    // Handle lockout scenario
                }
                else
                {
                    // Handle failure
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    await SendForgotPasswordEmail(user.Email, user);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string Token, string Email)
        {
            if (Token == null || Email == null)
            {
                ViewBag.ErrorTitle = "Invalid Password Reset Token";
                ViewBag.ErrorMessage = "The Link is Expired or Invalid";
                return View("Error");
            }
            else
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.Token = Token;
                model.Email = Email;
                return View(model);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        EmailSender emailSender = new EmailSender();


        private async Task SendForgotPasswordEmail(string? email, IdentityUser? user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetLink = Url.Action("ResetPassword", "Account",
                    new { Email = email, Token = token }, protocol: HttpContext.Request.Scheme);

            await emailSender.SendEmailAsync(email, "Reset Your Password", $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(passwordResetLink)}'>clicking here</a>.", true);
        }




        private async Task confirmEmail(IdentityUser? user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new Exception("user Email confirmation faild");
            }
        }

        private async Task confirmUser(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                if (await (userManager.IsEmailConfirmedAsync(user)))
                    return;

                confirmEmail(user);
            }
        }
    }
}
