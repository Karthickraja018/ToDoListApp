namespace ToDoApp.DTOs
{
    public record CreateTodoDto(string Title, string? Description, DateTime? DueDate, int? AssignedToUserId);
    public record UpdateTodoDto(string? Title, string? Description, DateTime? DueDate, bool? IsCompleted, int? AssignedToUserId);
    public record TodoDto(
            int Id,
            string Title,
            string? Description,
            bool IsCompleted,
            DateTime? DueDate,
            int? AssignedToUserId,
            int CreatedByUserId,
            string? AssignedToUsername,
            string? CreatedByUsername
        );
}