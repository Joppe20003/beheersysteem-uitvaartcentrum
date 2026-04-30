using beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService, IConfiguration config)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _userManager.FindByEmailAsync(dto.Email) != null)
        {
            return BadRequest(new { message = "Kan geen account aanmaken, Email is al in gebruik" });
        }

        if (await _userManager.FindByNameAsync(dto.Username) != null)
        {
            return BadRequest(new { message = "Kan geen account aanmaken, Gebruikersnaam is al in gebruik." });
        }

        IdentityUser user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };
        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, dto.Role.ToString());

        return Ok(new {message = "Account aangemaakt."});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password)) return BadRequest(new { message = "Ongeldige inloggegevens." });

        string token = await _tokenService.GenerateTokenAsync(user);
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
            email = user.Email
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

    [Authorize]
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new
        {
            isAuthenticated = true,
            userId = user.Id,
            email = user.Email,
            roles
        });
    }
}