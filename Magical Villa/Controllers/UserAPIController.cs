using MagicalVilla_API.Models;
using MagicalVilla_API.Models.Dto;
using MagicalVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicalVilla_API.Controllers
{
	//[Route("api/UsersAuthAPI")]
	[Route("api/v{version:apiVersion}/UsersAuthAPI")]
	[ApiController]
	public class UserAPIController : Controller
	{
		public readonly IUserRepository _userRepo;
		protected APIResponse _response;
        public UserAPIController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
			this._response = new();
        }

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
		{
			var loginResponse = await _userRepo.Login(model);
			if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Username or Password is incorrect.");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = loginResponse;
			return Ok(_response);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
		{
			bool UniqueUser = _userRepo.IsUniqueUser(model.UserName);
			if(!UniqueUser)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Username already Exists!");
				return BadRequest(_response);
			}
			var user = await _userRepo.Register(model);
			if (user == null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Error while Registering");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			return Ok(_response);
		}
	}
}
