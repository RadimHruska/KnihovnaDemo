using KnihovnaAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnihovnaAPI.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<int> CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
} 