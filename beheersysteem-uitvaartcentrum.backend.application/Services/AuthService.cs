using beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using beheersysteem_uitvaartcentrum.backend.application.Security;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;

namespace beheersysteem_uitvaartcentrum.backend.application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                throw new AlreadyExistsException("Kan geen account aanmaken, Email is al in gebruik");
            }

            if (await _userManager.FindByNameAsync(dto.Username) != null)
            {
                throw new AlreadyExistsException("Kan geen account aanmaken, Gebruikersnaam is al in gebruik.");
            }

            IdentityUser user = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

            await _userManager.AddToRoleAsync(user, dto.Role.ToString());
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ForbiddenException("Onjuiste inloggegevens", "Email of wachtwoord ontbreekt");
            }

            IdentityUser? user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new ForbiddenException("Onjuiste inloggegevens", "Email of wachtwoord incorrect");
            }

            string token = await _tokenService.GenerateTokenAsync(user);

            return token;
        }

        public async Task<UserStatusResult?> GetStatusAsync(ClaimsPrincipal user)
        {
            IdentityUser? u = await _userManager.GetUserAsync(user);
            if (u == null) return null;

            IEnumerable<string> roles = await _userManager.GetRolesAsync(u);
            List<string> actions = RolePermissions.GetActionsForRoles(roles.ToList());

            return new UserStatusResult
            {
                IsAuthenticated = true,
                Username = u.UserName,
                UserId = u.Id,
                Roles = roles,
                Actions = actions
            };
        }
    }
}
