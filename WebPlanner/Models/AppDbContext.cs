using Microsoft.EntityFrameworkCore;

namespace WebPlanner.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Tag> Tags { get;set;}
        public DbSet<ToDoTag> ToDoTag { get;set;}
    }
}
