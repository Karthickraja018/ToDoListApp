using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DTOs
{
    public class CreateTodo
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        [Range(1, 5)]
        public int Priority { get; set; }
    }
}