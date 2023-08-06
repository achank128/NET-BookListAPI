namespace WebApiNet.Models
{
	public class Customer
	{
		public int Id { get; set; }
		public string Name { get; set; }	
		public string Address { get; set; }
		public DateTime BirthDate { get; set; }
		public ICollection<Bill> Bills { get; set; }

	}
}
