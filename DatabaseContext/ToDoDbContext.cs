using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;

namespace ToDoListApp.DatabaseContext
{
    public class ToDoDbContext : DbContext, IToDoDbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options) { }

        public virtual DbSet<ToDoItemModel> ToDoItems {  get; set; }
    }
}
