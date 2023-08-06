namespace WebApiNet.Models
{
	public class Bill
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public Customer Customer { get; set; }
		public ICollection<BillBook> BillBooks { get; set;}

	}
}
