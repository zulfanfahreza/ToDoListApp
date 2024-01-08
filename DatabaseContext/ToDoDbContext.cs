using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;

namespace ToDoListApp.DatabaseContext
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options) { }

        public DbSet<ToDoItemModel> ToDoItems {  get; set; }
    }
}
