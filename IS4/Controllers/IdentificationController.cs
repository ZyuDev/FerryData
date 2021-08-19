using System.Threading.Tasks;
using FerryData.IS4.ViewModels.AccountViewModels;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FerryData.IS4.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class IdentificationController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentificationController(
            IIdentityServerInteractionService interaction,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager
            )
        {
            _interaction = interaction;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please. Validate your credentials and try again.");
                return View(model);
            }

            if (model.UserName != "admin@admin.com")
            {
                ModelState.AddModelError("", "User not accepted!");
                return View(model);
            }

            await HttpContext.SignInAsync(new IdentityServerUser(model.UserName));

            //var user = await _userManager.FindByNameAsync(model.UserName);
            //if (user == null)
            //{
            //    ModelState.AddModelError("UserName", "User not found");
            //    return View(model);
            //}

            //var signResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            //if (!signResult.Succeeded)
            //{
            //    ModelState.AddModelError("", "Something went wrong");
            //    return View(model);
            //}

            return Redirect(model.ReturnUrl);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return Redirect(logout.PostLogoutRedirectUri);
        }
    }
}
