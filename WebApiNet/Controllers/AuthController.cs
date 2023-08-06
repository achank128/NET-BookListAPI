using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiNet.Data;
using WebApiNet.Dto;
using WebApiNet.Models;

namespace WebApiNet.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _configuration;

		public AuthController(AppDbContext context, IOptionsMonitor<AppSettings> optionsMonitor, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
		{
			return await _context.Accounts.ToListAsync();
		}

		[HttpGet("getme"), Authorize]
		public async Task<ActionResult<object>> GetMyAccount()
		{
			string username = User.FindFirstValue(ClaimTypes.Name);
			return Ok( new
			{
				username
			});
		}

		[HttpPost("register")]
		public async Task<ActionResult<User>> Register(UserRegister userRegister)
		{
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);

			Account newUser = new Account();
			newUser.Username = userRegister.Username;
			newUser.Password = passwordHash;
			newUser.Email = userRegister.Email;
			newUser.Name = userRegister.Name;
			newUser.Role = userRegister.Role;

			_context.Accounts.Add(newUser);
			await _context.SaveChangesAsync();

			string token = GenerateToken(newUser);

			return CreatedAtAction("Register", new { id = newUser.Id }, new ApiResponse {
				Success = true,
				Message = "Register successful",
				Data = new {
					accessToken = token,
					user = newUser
				}
			});
		}

		[HttpPost("login")]
		public ActionResult<User> Login(UserLogin userLogin)
		{
			var user = _context.Accounts.SingleOrDefault(p => p.Username == userLogin.Username);

			if (user.Username != userLogin.Username)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "Username Incorrected."
				});
			}

			if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "Wrong Password."
				});
			}

			string token = GenerateToken(user);

			return Ok(new ApiResponse
			{
				Success = true,
				Message = "Login successful",
				Data = new
				{
					accessToken = token,
					user
				}
			});
		}



		private string GenerateToken(Account user)
		{
			List<Claim> claims = new List<Claim> {
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_configuration.GetSection("AppSettings:SecretKey").Value!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.Now.AddDays(30),
					signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}

	}
}
