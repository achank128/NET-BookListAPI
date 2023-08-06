using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Dto
{
	public class BookDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
	}
}
