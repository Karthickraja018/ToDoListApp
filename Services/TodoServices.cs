using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using ToDoApp.DTOs;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Models
{
    public class TodoServices : ITodoServices
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TodoServices> _logger;
        private readonly IMapper _mapper;

        public TodoServices(AppDbContext context, ILogger<TodoServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task CreateTodoAsync(CreateTodo request)
        {
            try
            {
                var todo = _mapper.Map<Todo>(request);
                todo.CreatedDate = DateTime.Now;
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the todo item.");
                throw new Exception("An error occurred while creating the todo item.");
            }
        }

        public async Task DeleteTodoAsync(Guid id)
        {

            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new Exception($"No  item found with the id {id}");
            }
        }

        public async Task<PagedResponse<Todo>> GetAllAsync(PaginationParams paginationParams)
        {
            var query = _context.Todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
            {
                query = query.Where(t => 
                    t.Title.Contains(paginationParams.SearchTerm) || 
                    t.Description.Contains(paginationParams.SearchTerm));
            }

            if (paginationParams.IsCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == paginationParams.IsCompleted.Value);
            }

            if (paginationParams.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == paginationParams.Priority.Value);
            }

            if (paginationParams.DueDateFrom.HasValue)
            {
                query = query.Where(t => t.DueDate >= paginationParams.DueDateFrom.Value);
            }

            if (paginationParams.DueDateTo.HasValue)
            {
                query = query.Where(t => t.DueDate <= paginationParams.DueDateTo.Value);
            }

            var totalRecords = await query.CountAsync();

            var todos = await query
                .OrderByDescending(t => t.CreatedDate)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)paginationParams.PageSize);

            return new PagedResponse<Todo>
            {
                Data = todos,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                HasPrevious = paginationParams.PageNumber > 1,
                HasNext = paginationParams.PageNumber < totalPages
            };
        }

        public async Task<Todo> GetByIdAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                throw new KeyNotFoundException($"No Todo item with Id {id} found.");
            }
            return todo;
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodo request)
        {
            try
            {
                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    throw new Exception($"Todo item with id {id} not found.");
                }

                if (request.Title != null)
                {
                    todo.Title = request.Title;
                }

                if (request.Description != null)
                {
                    todo.Description = request.Description;
                }

                if (request.IsCompleted != null)
                {
                    todo.IsCompleted = request.IsCompleted.Value;
                }

                if (request.DueDate != null)
                {
                    todo.DueDate = request.DueDate.Value;
                }

                if (request.Priority != null)
                {
                    todo.Priority = request.Priority.Value;
                }

                todo.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the todo item with id {id}.");
                throw;
            }
        }
    }
}