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
    public class LendsController : ControllerBase
    {
        private readonly ILendRepository _lendRepository;

        public LendsController(ILendRepository lendRepository)
        {
            _lendRepository = lendRepository;
        }

        // GET: api/lends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lend>>> GetLends()
        {
            var lends = await _lendRepository.GetAllLendsAsync();
            return Ok(lends);
        }

        // GET: api/lends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lend>> GetLend(int id)
        {
            var lend = await _lendRepository.GetLendByIdAsync(id);

            if (lend == null)
            {
                return NotFound();
            }

            return lend;
        }

        // GET: api/lends/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Lend>>> GetUserLends(int userId)
        {
            var lends = await _lendRepository.GetLendsByUserIdAsync(userId);
            return Ok(lends);
        }

        // GET: api/lends/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Lend>>> GetActiveLends()
        {
            var lends = await _lendRepository.GetActiveLendsAsync();
            return Ok(lends);
        }

        // GET: api/lends/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Lend>>> GetOverdueLends()
        {
            var lends = await _lendRepository.GetOverdueLendsAsync();
            return Ok(lends);
        }

        // POST: api/lends
        [HttpPost]
        public async Task<ActionResult<int>> CreateLend(Lend lend)
        {
            try
            {
                var id = await _lendRepository.CreateLendAsync(lend);
                return id;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/lends/5/return
        [HttpPut("{id}/return")]
        public async Task<ActionResult<int>> ReturnBook(int id, Lend lend)
        {
            if (id != lend.Id)
            {
                return BadRequest("ID se neshoduje.");
            }

            if (!lend.ReturnedDate.HasValue)
            {
                return BadRequest("Není specifikováno datum vrácení.");
            }

            var success = await _lendRepository.ReturnBookAsync(id, lend.ReturnedDate.Value);
            if (!success)
            {
                return NotFound("Výpůjčka nebyla nalezena.");
            }

            return id;
        }

        // DELETE: api/lends/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLend(int id)
        {
            var lend = await _lendRepository.GetLendByIdAsync(id);
            if (lend == null)
            {
                return NotFound();
            }

            await _lendRepository.DeleteLendAsync(id);
            return NoContent();
        }
    }
} 