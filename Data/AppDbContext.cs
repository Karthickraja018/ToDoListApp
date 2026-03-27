using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Todo> Todos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTodos)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTodos)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            var managerHash = BCrypt.Net.BCrypt.HashPassword("Password123!", "$2a$11$7EqJtq98hPqEX7fNZaFWo.");
            var employeeHash = BCrypt.Net.BCrypt.HashPassword("Password123!", "$2a$11$7EqJtq98hPqEX7fNZaFWo.");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "manager1",
                    PasswordHash = managerHash,
                    Role = "Manager"
                },
                new User
                {
                    Id = 2,
                    Username = "employee1",
                    PasswordHash = employeeHash,
                    Role = "Employee"
                }
            );
        }
    }
}
