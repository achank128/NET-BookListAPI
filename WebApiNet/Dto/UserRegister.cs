using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Dto
{
	public class UserRegister
	{
		[Required]
		[MaxLength(50)]
		public string Username { get; set; }

		[Required]
		[MaxLength(50)]
		public string Password { get; set; }

		[Required]
		public string Email { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
	}
}
