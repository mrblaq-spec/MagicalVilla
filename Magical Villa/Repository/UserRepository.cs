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
using Microsoft.AspNetCore.Identity;

namespace MagicalVilla_API.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
		private string secretKey;
        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
			IMapper mapper, IConfiguration _configuration, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
			_mapper = mapper;
			secretKey = _configuration.GetValue<string>("ApiSettings:apiSecret");
			_userManager = userManager;
			_roleManager = roleManager;
        }
        public bool IsUniqueUser(string username)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(x=>x.UserName == username);
			if(user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			var user = _db.ApplicationUsers
				.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

			if(user == null || isValid == false)
			{
				return new LoginResponseDto()
				{
					Token = "",
					User = null
				};
			}

			// if user was found generate JWT Token
			var roles = await _userManager.GetRolesAsync(user);
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserName.ToString()),
					new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDto loginResponseDto = new LoginResponseDto
			{
				Token = tokenHandler.WriteToken(token),
				User = _mapper.Map<UserDto>(user),
			};
			return loginResponseDto;
		}

		public async Task<UserDto> Register(RegistrationRequestDto registrationRequestDto)
		{
            //ApplicationUser user = _mapper.Map<LocalUser>(registrationRequestDto);
            ApplicationUser user = new()
			{
				UserName = registrationRequestDto.UserName,
				Email  = registrationRequestDto.UserName,
				NormalizedEmail  = registrationRequestDto.UserName.ToUpper(),
                Name = registrationRequestDto.Name,
			};

			try
			{
				var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
				if (result.Succeeded)
				{
					if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
					{
						await _roleManager.CreateAsync(new IdentityRole("Admin"));
						await _roleManager.CreateAsync(new IdentityRole("Customer"));
                    }
					await _userManager.AddToRoleAsync(user, "Admin");
					var userToReturn = _db.ApplicationUsers
						.FirstOrDefault(u=>u.UserName == registrationRequestDto.UserName);
					return _mapper.Map<UserDto>(userToReturn);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return new UserDto();
		}
	}
}
