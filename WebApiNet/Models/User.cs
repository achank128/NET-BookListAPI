using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Username { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
	}
}
