using KnihovnaAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnihovnaAPI.Services
{
    public interface ILendRepository
    {
        Task<IEnumerable<Lend>> GetAllLendsAsync();
        Task<Lend> GetLendByIdAsync(int id);
        Task<IEnumerable<Lend>> GetLendsByUserIdAsync(int userId);
        Task<IEnumerable<Lend>> GetActiveLendsAsync();
        Task<IEnumerable<Lend>> GetOverdueLendsAsync();
        Task<int> CreateLendAsync(Lend lend);
        Task<bool> ReturnBookAsync(int id, DateTime returnDate);
        Task DeleteLendAsync(int id);
    }
} 