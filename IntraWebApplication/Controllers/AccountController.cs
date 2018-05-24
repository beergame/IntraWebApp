using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IntraWebApp.Business.Models;
using IntraWebApp.Business.Models.User;
using IntraWebApp.Business.Services;
using IntraWebApp.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntraWebApplication.Controllers
{
	public class AccountController : Controller
    {
		private readonly IUserService _userService;
		private ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);


		public AccountController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var newUser = new Register
				{
					Civility = model.Civility,
					FirstName = model.FirstName,
					LastName = model.LastName,
					Username = model.Username,
					Password = model.Password
				};

				var result = await _userService.Create(newUser);
				if (result == SystemResponse.Success)
				{
					return RedirectToAction("Login");
				}
				else {                     
					    ModelState.AddModelError("register", "Error while processing"); 
                    } 
			    }
			return View(model);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var userToAuthenticate = new Authenticate
				{ 
					Username = model.Username,
					Password = model.Password
				};
				var result = await _userService.Authenticate(userToAuthenticate);
				var role = result.IsAdmin ? "Admin" : "Normal";
				List<Claim> claims = new List<Claim>
                {
				    new Claim(ClaimTypes.Name, model.Username),
					new Claim(ClaimTypes.UserData, result.AccessToken),
					new Claim(ClaimTypes.Role, role)
                };

				identity.AddClaims(claims);
				ClaimsPrincipal principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                
				return RedirectToAction("Index", "Home");

			}
			return View(model);
		}

		public async Task<IActionResult> Logout()
        {
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

		[HttpGet]
		public async Task<IActionResult> Update()
		{
			var accessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
            var result = await _userService.GetUser(accessToken);
			var model = new UpdateViewModel();
            if (result != null)
            {
                model.FirstName = result.FirstName;
                model.LastName = result.LastName;
            }

			ViewData["UserInfo"] = model;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateViewModel model)
		{
			if (ModelState.IsValid)
			{
				var accessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
                var userToUpdate = new Update
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.NewPassword
                };
                var result = await _userService.Update(accessToken, userToUpdate);
                if (result == SystemResponse.Success)
                {
                    return RedirectToAction("Update");
                }
			}

			return View(model);
		}

		// GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
