using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.DTOs;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _db;

        public TodoService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Todo> CreateAsync(CreateTodoDto dto, int createdByUserId)
        {
            var todo = new Todo
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedByUserId = createdByUserId
            };

            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _db.Todos.FindAsync(id);
            if (todo == null)
            {
                return false;
            }

            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _db.Todos
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _db.Todos
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Todo>> GetByAssignedUserIdAsync(int userId)
        {
            return await _db.Todos
                .Where(t => t.AssignedToUserId == userId)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }

        public async Task<Todo?> UpdateAsync(int id, UpdateTodoDto dto)
        {
            var todo = await _db.Todos.FindAsync(id);
            if (todo == null)
            {
                return null;
            }

            if (dto.Title != null)
            {
                todo.Title = dto.Title;
            }

            if (dto.Description != null)
            {
                todo.Description = dto.Description;
            }

            if (dto.DueDate.HasValue)
            {
                todo.DueDate = dto.DueDate;
            }

            if (dto.IsCompleted.HasValue)
            {
                todo.IsCompleted = dto.IsCompleted.Value;
            }

            if (dto.AssignedToUserId.HasValue)
            {
                todo.AssignedToUserId = dto.AssignedToUserId;
            }

            await _db.SaveChangesAsync();
            return todo;
        }
    }
}