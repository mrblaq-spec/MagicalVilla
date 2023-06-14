using MagicalVilla_Web.Models.Dto;

namespace MagicalVilla_Web.Services.IServices
{
	public interface IAuthService
	{
		Task<T> LoginAsync<T>(LoginRequestDto objToCreate);
		Task<T> RegisterAsync<T>(RegistrationRequestDto objToCreate);
	}
}
