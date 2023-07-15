using MagicalVilla_API.Models;
using MagicalVilla_API.Models.Dto;

namespace MagicalVilla_API.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
		Task<UserDto> Register(RegistrationRequestDto registrationRequestDto);
	}
}
