using MagicalVilla_Utility;
using MagicalVilla_Web.Models;
using MagicalVilla_Web.Models.Dto;
using MagicalVilla_Web.Services.IServices;

namespace MagicalVilla_Web.Services
{
	public class AuthService : BaseService, IAuthService
	{
		public readonly IHttpClientFactory _clientFactory;
		private string authUrl;
		public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			authUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
		}
		public Task<T> LoginAsync<T>(LoginRequestDto obj)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.POST,
				Data = obj,
				Url = authUrl + "/api/v1/UsersAuthAPI/login"
			});
		}

		public Task<T> RegisterAsync<T>(RegistrationRequestDto obj)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.POST,
				Data = obj,
				Url = authUrl + "/api/v1/UsersAuthAPI/register"
            });
		}
	}
}
