using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;

namespace ToDoListApp.DatabaseContext
{
    public interface IToDoDbContext
    {
        DbSet<ToDoItemModel> ToDoItems { get; set; }
        DbSet<UserEntity> Users { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
