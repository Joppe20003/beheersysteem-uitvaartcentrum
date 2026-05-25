using beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto); // throws AlreadyExistsException or IdentityResult failures as exceptions
        Task<string> LoginAsync(LoginDTO dto); // returns token, throws ForbiddenException on invalid credentials
        Task<UserStatusResult?> GetStatusAsync(ClaimsPrincipal user);
    }
}
