using KnihovnaAPI.Data;
using KnihovnaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnihovnaAPI.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return await GetAllBooksAsync();
            }

            searchTerm = searchTerm.ToLower();
            return await _context.Books
                .Where(b => b.Name.ToLower().Contains(searchTerm) 
                       || b.Author.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            // Knihy, které mají dostupné kopie (nejsou všechny vypůjčené)
            return await _context.Books
                .Where(b => b.InStock > (
                    _context.Lends
                        .Count(l => l.IdBook == b.Id && l.ReturnedDate == null)
                ))
                .ToListAsync();
        }

        public async Task<int> CreateBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
} 