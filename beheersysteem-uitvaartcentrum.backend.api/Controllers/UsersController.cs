using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.User;

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
            IList<IdentityUser> admins = await _userManager.GetUsersInRoleAsync("Admin");
            List<string> adminIds = admins.Select(a => a.Id).ToList();

            List<UserOverviewDTO> nonAdmins = _userManager.Users
                .Where(u => !adminIds.Contains(u.Id))
                .Select(user => new UserOverviewDTO
                {
                    Id = user.Id,
                    UserName = user.UserName
                })
                .ToList();

            return Ok(nonAdmins);
        }
    }
}
