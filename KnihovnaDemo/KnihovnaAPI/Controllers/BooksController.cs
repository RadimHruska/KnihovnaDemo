using KnihovnaAPI.Models;
using KnihovnaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnihovnaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return Ok(books);
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: api/books/search/{query}
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string query)
        {
            var books = await _bookRepository.SearchBooksAsync(query);
            return Ok(books);
        }

        // GET: api/books/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAvailableBooks()
        {
            var books = await _bookRepository.GetAvailableBooksAsync();
            return Ok(books);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<int>> CreateBook(Book book)
        {
            var id = await _bookRepository.CreateBookAsync(book);
            return id;
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bookRepository.UpdateBookAsync(book);
            }
            catch (Exception)
            {
                var existingBook = await _bookRepository.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteBookAsync(id);
            return NoContent();
        }
    }
}