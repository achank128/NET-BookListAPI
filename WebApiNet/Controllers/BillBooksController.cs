using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNet.Data;
using WebApiNet.Models;

namespace WebApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillBooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BillBooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BillBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillBook>>> GetBillBooks()
        {
          if (_context.BillBooks == null)
          {
              return NotFound();
          }
            return await _context.BillBooks.ToListAsync();
        }

        // GET: api/BillBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillBook>> GetBillBook(int id)
        {
          if (_context.BillBooks == null)
          {
              return NotFound();
          }
            var billBook = await _context.BillBooks.FindAsync(id);

            if (billBook == null)
            {
                return NotFound();
            }

            return billBook;
        }

        // PUT: api/BillBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillBook(int id, BillBook billBook)
        {
            if (id != billBook.BookId)
            {
                return BadRequest();
            }

            _context.Entry(billBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BillBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BillBook>> PostBillBook(BillBook billBook)
        {
          if (_context.BillBooks == null)
          {
              return Problem("Entity set 'AppDbContext.BillBooks'  is null.");
          }
            _context.BillBooks.Add(billBook);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BillBookExists(billBook.BookId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBillBook", new { id = billBook.BookId }, billBook);
        }

        // DELETE: api/BillBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillBook(int id)
        {
            if (_context.BillBooks == null)
            {
                return NotFound();
            }
            var billBook = await _context.BillBooks.FindAsync(id);
            if (billBook == null)
            {
                return NotFound();
            }

            _context.BillBooks.Remove(billBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BillBookExists(int id)
        {
            return (_context.BillBooks?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
