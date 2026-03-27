using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.DTOs;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private int CurrentUserId
        {
            get
            {
                var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
                return int.TryParse(idClaim, out var id) ? id : 0;
            }
        }

        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (string.Equals(CurrentUserRole, "Manager", StringComparison.OrdinalIgnoreCase))
            {
                var users = await _userService.GetAllAsync();
                var dtos = users.Select(u => new UserDto(u.Id, u.Username, u.Role));
                return Ok(dtos);
            }

            var user = await _userService.GetByIdAsync(CurrentUserId);
            if (user == null)
            {
                return NotFound();
            }

            var dto = new UserDto(user.Id, user.Username, user.Role);
            return Ok(new[] { dto });
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userService.GetByIdAsync(CurrentUserId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserDto(user.Id, user.Username, user.Role));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (string.Equals(CurrentUserRole, "Manager", StringComparison.OrdinalIgnoreCase) || user.Id == CurrentUserId)
            {
                return Ok(new UserDto(user.Id, user.Username, user.Role));
            }

            return Forbid();
        }
    }
}