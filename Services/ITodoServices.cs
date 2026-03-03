using ToDoApp.DTOs;
using ToDoApp.Models;


namespace ToDoApp.Services
{
    public interface ITodoServices
    {
        Task<PagedResponse<Todo>> GetAllAsync(PaginationParams paginationParams);
        Task<Todo> GetByIdAsync(Guid id);
        Task CreateTodoAsync(CreateTodo request);
        Task UpdateTodoAsync(Guid id, UpdateTodo request);
        Task DeleteTodoAsync(Guid id);
    }
}