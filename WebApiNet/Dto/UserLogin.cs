using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Dto
{
	public class UserLogin
	{
		[Required]
		[MaxLength(50)]
		public string Username { get; set; }

		[Required]
		[MaxLength(50)]
		public string Password { get; set; }
	}
}
