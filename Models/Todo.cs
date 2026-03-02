using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class Todo
    {
        public Todo()
        {
            IsCompleted = false;
        }

        [Key]
        //public int Id { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }

    }
}