using FerryData.IS4.ViewModels.AccountViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError("UserName", "User not found");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.Remember, true);

            if (!result.Succeeded)
            {
                return View(model);
            }

            return Redirect(model.ReturnUrl);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            await _signInManager.SignOutAsync();

            return Redirect(logout.PostLogoutRedirectUri);
        }

        [HttpGet("[action]")]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
                await _signInManager.SignInAsync(user, false);

                return Redirect(model.ReturnUrl);
            }

            return View(model);
        }
    }
}
