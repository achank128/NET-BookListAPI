using Azure.Core;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiNet.Data;
using WebApiNet.Dto;
using WebApiNet.Models;

namespace WebApiNet.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly AppSettings _appSettings;
		private readonly IConfiguration _configuration;

		public UserController(AppDbContext context, IOptionsMonitor<AppSettings> optionsMonitor, IConfiguration configuration)
		{
			_context = context;
			_appSettings = optionsMonitor.CurrentValue;
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpPost("register")]
		public async Task<ActionResult<User>> Register(UserRegister userRegister ) { 

			CreatePasswordHash(userRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);

			User newUser = new User();
			newUser.Username = userRegister.Username;
			newUser.PasswordHash = passwordHash;
			newUser.PasswordSalt = passwordSalt;
			newUser.Email = userRegister.Email;
			newUser.Name = userRegister.Name;

			_context.Users.Add(newUser);
			await _context.SaveChangesAsync();			

			return CreatedAtAction("Register", new { id = newUser.Id }, newUser);
		}

		


		[HttpPost("login")]
		public IActionResult Login(UserLogin userLogin)
		{
			var user = _context.Users.SingleOrDefault(p => p.Username== userLogin.Username);
			if (user == null)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "Username Incorrected"
				});
			}

			if(!VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "Wrong Password"
				});
			}

			var token = GenerateToken(user);

			//Success
			return Ok(new ApiResponse
			{
				Success = true,
				Message = "Login successful",
				Data = token
			});
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}

		private Token GenerateToken(User user) {
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] {
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim("Username", user.Username),
					new Claim("Id", user.Id.ToString()),
					new Claim("TokenId", Guid.NewGuid().ToString())
				}),
				Expires = DateTime.UtcNow.AddMinutes(1000),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
			};


			var token = jwtTokenHandler.CreateToken(tokenDescription);
			var assessToken = jwtTokenHandler.WriteToken(token);

			return new Token
			{
				AccessToken = assessToken,
			};
		}

		

	}
}
