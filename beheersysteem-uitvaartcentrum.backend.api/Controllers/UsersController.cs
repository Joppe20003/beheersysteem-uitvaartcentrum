using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.User;
using System.Linq;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string? userId = User.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

            List<UserOverviewDTO> users = _userManager.Users
                .Where(user => user.Id != userId)
                .Select(user => new UserOverviewDTO
                {
                    Id = user.Id,
                    UserName = user.UserName
                })
                .ToList();

            return Ok(users);
        }
    }
}
