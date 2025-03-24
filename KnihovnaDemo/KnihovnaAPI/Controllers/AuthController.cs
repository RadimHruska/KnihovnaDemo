using KnihovnaAPI.Models;
using KnihovnaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnihovnaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // V produkčním prostředí bychom zde použili JWT nebo jinou formu bezpečného přihlášení
            // Pro demo účely pouze vracíme objekt uživatele (bez hesla)
            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 