using Microsoft.EntityFrameworkCore;
using WebApiNet.Data;
using WebApiNet.Models;

namespace WebApiNet.Services.Repositories
{
	public class BookRepository : IBookRepository
	{
		private readonly AppDbContext _context;

		public BookRepository( AppDbContext context)
		{
			_context = context;
		}

		public Task<List<Book>> Create(Book book)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<Book>> GetAll()
		{
			return await _context.Books!.ToListAsync();
		}

		public Task<Book?> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Book>> Update(Book book)
		{
			throw new NotImplementedException();
		}
	}
}
