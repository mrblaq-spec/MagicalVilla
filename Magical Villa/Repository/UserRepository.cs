using MagicalVilla_API.Models;
using MagicalVilla_API.Models.Dto;
using MagicalVilla_API.Repository.IRepository;
using MagicalVilla_API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MagicalVilla_API.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
		private string secretKey;
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration _configuration)
        {
            _db = db;
			_mapper = mapper;
			secretKey = _configuration.GetValue<string>("ApiSettings:apiSecret");
        }
        public bool IsUniqueUser(string username)
		{
			var user = _db.LocalUsers.FirstOrDefault(x=>x.UserName == username);
			if(user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower()
			&& u.Password == loginRequestDto.Password);


			if(user == null)
			{
				return new LoginResponseDto()
				{
					Token = "",
					User = null
				};
			}
			// if user was not found generate JWT Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDto loginResponseDto = new LoginResponseDto
			{
				Token = tokenHandler.WriteToken(token),
				User = user
			};
			return loginResponseDto;
		}

		public async Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto)
		{
			LocalUser user = _mapper.Map<LocalUser>(registrationRequestDto);
			/*LocalUser user = new LocalUser()
			{
				UserName = registrationRequestDto.UserName,
				Password = registrationRequestDto.Password,
				Name = registrationRequestDto.Name,
				Role = registrationRequestDto.Role
			};*/
			_db.LocalUsers.Add(user);
			await _db.SaveChangesAsync();
			user.Password = "";
			return user;
		}
	}
}
