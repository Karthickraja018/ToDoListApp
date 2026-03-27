namespace ToDoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public int CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
    }
}