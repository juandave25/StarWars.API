using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarWars.API.Entity.Dto;
using StarWars.API.Services;

namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private const string ValidUsername = "demo";
        private const string ValidPassword = "demo123";

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<string> Login(AuthenticationRequest credentials)
        {
            if (credentials.Username == ValidUsername && credentials.Password == ValidPassword)
            {
                var token = _authService.GenerateJwtToken(credentials.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
