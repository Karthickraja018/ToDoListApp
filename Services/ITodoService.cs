using ToDoApp.DTOs;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface ITodoService
    {
        Task<Todo> CreateAsync(CreateTodoDto dto, int createdByUserId);
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<IEnumerable<Todo>> GetByAssignedUserIdAsync(int userId);
        Task<Todo?> GetByIdAsync(int id);
        Task<Todo?> UpdateAsync(int id, UpdateTodoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}