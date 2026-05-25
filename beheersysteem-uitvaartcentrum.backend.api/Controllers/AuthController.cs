using beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Requests.RegisterRequest dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var appDto = new RegisterDTO
        {
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password,
            Role = dto.Role
        };

        await _authService.RegisterAsync(appDto);

        return Ok(new {message = "Account aangemaakt."});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var appDto = new LoginDTO
        {
            Email = dto.Email,
            Password = dto.Password
        };

        string token = await _authService.LoginAsync(appDto);

        // set token cookie
        CookieOptions cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"] ?? "60"))
        };

        Response.Cookies.Append("X-Access-Token", token, cookieOptions);

        return Ok(new
        {
            message = "Inloggen geslaagd.",
            email = appDto.Email
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("X-Access-Token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,

        });

        return Ok(new { message = "Uitgelogd." });
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        UserStatusResult? status = await _authService.GetStatusAsync(User);

        if (status == null) return Unauthorized();

        return Ok(new
        {
            isAuthenticated = status.IsAuthenticated,
            username = status.Username,
            userId = status.UserId,
            role = status.Roles,
            actions = status.Actions
        });
    }
}