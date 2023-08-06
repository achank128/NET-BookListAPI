using WebApiNet.Models;

namespace WebApiNet.Services.Repositories
{
    public interface IBookRepository
    {
		Task<List<Book>> GetAll();
		Task<Book?> GetById(int id);
		Task<List<Book>> Create(Book book);
		Task<List<Book>> Update(Book book);
		void Delete(int id);
	}
}
