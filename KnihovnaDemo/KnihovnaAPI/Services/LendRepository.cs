using KnihovnaAPI.Data;
using KnihovnaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnihovnaAPI.Services
{
    public class LendRepository : ILendRepository
    {
        private readonly LibraryContext _context;

        public LendRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lend>> GetAllLendsAsync()
        {
            return await _context.Lends
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToListAsync();
        }

        public async Task<Lend> GetLendByIdAsync(int id)
        {
            return await _context.Lends
                .Include(l => l.User)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Lend>> GetLendsByUserIdAsync(int userId)
        {
            return await _context.Lends
                .Include(l => l.Book)
                .Where(l => l.IdUser == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lend>> GetActiveLendsAsync()
        {
            return await _context.Lends
                .Include(l => l.User)
                .Include(l => l.Book)
                .Where(l => l.ReturnedDate == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lend>> GetOverdueLendsAsync()
        {
            // Výpůjčky, které měly být vráceny před 14 dny a nebyly
            var dueDate = DateTime.Now.AddDays(-14);
            return await _context.Lends
                .Include(l => l.User)
                .Include(l => l.Book)
                .Where(l => l.ReturnedDate == null && l.LandedDate < dueDate)
                .ToListAsync();
        }

        public async Task<int> CreateLendAsync(Lend lend)
        {
            // Nejprve zkontrolujeme, zda máme dostatek knih k vypůjčení
            var book = await _context.Books.FindAsync(lend.IdBook);
            if (book == null)
            {
                throw new Exception("Kniha neexistuje.");
            }

            // Zjistit počet aktuálních výpůjček pro tuto knihu
            var lendedCopies = await _context.Lends
                .Where(l => l.IdBook == lend.IdBook && l.ReturnedDate == null)
                .CountAsync();

            if (lendedCopies >= book.InStock)
            {
                throw new Exception("Všechny dostupné kopie této knihy jsou již vypůjčené.");
            }

            _context.Lends.Add(lend);
            await _context.SaveChangesAsync();
            return lend.Id;
        }

        public async Task<bool> ReturnBookAsync(int id, DateTime returnDate)
        {
            var lend = await _context.Lends.FindAsync(id);
            if (lend == null)
            {
                return false;
            }

            lend.ReturnedDate = returnDate;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteLendAsync(int id)
        {
            var lend = await _context.Lends.FindAsync(id);
            if (lend != null)
            {
                _context.Lends.Remove(lend);
                await _context.SaveChangesAsync();
            }
        }
    }
} 