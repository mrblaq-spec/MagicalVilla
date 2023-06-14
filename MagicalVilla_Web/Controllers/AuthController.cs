using MagicalVilla_Utility;
using MagicalVilla_Web.Models;
using MagicalVilla_Web.Models.Dto;
using MagicalVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MagicalVilla_Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;

        public AuthController(IAuthService autherService)
        {
			_authService = autherService;
        }
		[HttpGet]
        public IActionResult Login()
		{
			LoginRequestDto obj = new();
			return View(obj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDto obj)
		{
			APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
			if(response != null && response.IsSuccess)
			{
				LoginResponseDto model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
				identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(SD.SessionToken, model.Token);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("CustomError", response.ErrorMessages.ToString());
				return View(obj);
			}
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegistrationRequestDto obj)
		{
			APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
			if (result != null && result.IsSuccess)
			{
				return RedirectToAction("Login");
			}
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString(SD.SessionToken, "");
			return RedirectToAction("Index", "Home");

		}

		public async Task<IActionResult> AccessDenied()
		{
			return View();
		}
	}
}
