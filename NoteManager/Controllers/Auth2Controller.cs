using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteManager.Models;
using NuGet.Protocol;

namespace NoteManager.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]

    [ApiController]
    [AllowAnonymous]
    public class Auth2Controller : ControllerBase
    {
        //userManager will hold the UserManager instance
        private readonly UserManager<IdentityUser> userManager;
        //signInManager will hold the SignInManager instance
        private readonly SignInManager<IdentityUser> signInManager;

        //Both UserManager and SignInManager services are injected into the AccountController
        //using constructor injection
        public Auth2Controller(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<object> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    signInManager.SignInAsync(user, isPersistent: false);
                    var res2 = signInManager.ToJToken();
                    return Ok(res2);
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Ok(ModelState);
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("Register")]
        public async Task<object> Register(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    signInManager.SignInAsync(user, isPersistent: false);
                    var res2 = signInManager.ToJToken();
                    return Ok(res2);
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Ok(ModelState);
            }

            return BadRequest();
        }




        [HttpPost]
        [AllowAnonymous]
        [Route("Signin")]
        public async Task<object> Signin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Handle successful login
                    return Ok(result.ToJson());
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
                    return BadRequest(ModelState);
                }
            }

            // If we got this far, something failed, redisplay form
            return Ok(model);
        }

    }
}
