using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);

            if (token == null || !token.Contains('.'))
                return Unauthorized();

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var msg = await _authService.RegisterAsync(request.Name, request.Email, request.Password);

            if (msg == "A user is already registered in that Email Address!")
                return BadRequest(msg);

            return Created("", msg);
        }

        [Authorize]
        [HttpGet("role")]
        public async Task<IActionResult> GetRole()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == null)
                return NotFound("Role not found");

            return Ok(new { role });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Stateless JWT → handled client-side
            return Ok("Logout successful");
        }
    }
}
