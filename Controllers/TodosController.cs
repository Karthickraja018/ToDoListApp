using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.DTOs;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private int CurrentUserId
        {
            get
            {
                var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)
                    ?? User.FindFirstValue(ClaimTypes.Name);
                return int.TryParse(idClaim, out var id) ? id : 0;
            }
        }

        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (CurrentUserRole == "Manager")
            {
                var all = await _todoService.GetAllAsync();
                var dtoList = all.Select(t => new TodoDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.DueDate,
                    t.AssignedToUserId,
                    t.CreatedByUserId,
                    t.AssignedToUser?.Username,
                    t.CreatedByUser?.Username
                ));
                return Ok(dtoList);
            }

            var mine = await _todoService.GetByAssignedUserIdAsync(CurrentUserId);
            var mineDtoList = mine.Select(t => new TodoDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.DueDate,
                t.AssignedToUserId,
                t.CreatedByUserId,
                t.AssignedToUser?.Username,
                t.CreatedByUser?.Username
            ));
            return Ok(mineDtoList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (CurrentUserRole == "Manager" || todo.AssignedToUserId == CurrentUserId)
            {
                var dto = new TodoDto(
                    todo.Id,
                    todo.Title,
                    todo.Description,
                    todo.IsCompleted,
                    todo.DueDate,
                    todo.AssignedToUserId,
                    todo.CreatedByUserId,
                    todo.AssignedToUser?.Username,
                    todo.CreatedByUser?.Username
                );
                return Ok(dto);
            }

            return Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(CreateTodoDto dto)
        {
            var todo = await _todoService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateTodoDto dto)
        {
            var todo = await _todoService.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (CurrentUserRole == "Manager" || todo.AssignedToUserId == CurrentUserId)
            {
                var updated = await _todoService.UpdateAsync(id, dto);
                return Ok(updated);
            }

            return Forbid();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _todoService.DeleteAsync(id);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}