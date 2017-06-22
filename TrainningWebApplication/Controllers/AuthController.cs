using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TrainningWebApplication
{
    public class AuthController : Controller
    {
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			bool auth = (model.Username == "tx" && model.Password == "33");
			if (!auth)
			{
				return View(model);
			}
			var principal = new ClaimsPrincipal(
				new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.Name, model.Username),
					new Claim(ClaimTypes.Role, "Admin")
				}, "FormsAuthentication"));
			await HttpContext.Authentication.SignInAsync("Cookies", principal);
			if (model.ReturnUrl != null)
			{
				return LocalRedirect(model.ReturnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Signout()
		{
			await HttpContext.Authentication.SignOutAsync("Cookies");
			return RedirectToAction("Index", "Home");
		}
    }
}