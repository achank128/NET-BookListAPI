using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Models
{
	public class Account
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
	}
}
