using System.Text.Json.Serialization;
using ToDoApp.Models;

namespace ToDoApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "Employee";

        [JsonIgnore]
        public ICollection<Todo> CreatedTodos { get; set; } = new List<Todo>();

        [JsonIgnore]
        public ICollection<Todo> AssignedTodos { get; set; } = new List<Todo>();
    }
}