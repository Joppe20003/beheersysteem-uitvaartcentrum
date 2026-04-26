using Microsoft.AspNetCore.Identity;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(IdentityUser user);
}