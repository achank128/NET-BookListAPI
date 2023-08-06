using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Author { get; set; }
        public string? Description { get; set; }

        [Required]
        public int Price { get; set; }

        public Category Category { get; set; }
		public ICollection<BillBook> BillBooks { get; set; }

	}
}
