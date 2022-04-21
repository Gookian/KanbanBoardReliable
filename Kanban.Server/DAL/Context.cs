using Core;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Server.DAL
{
    public class Context : DbContext
    {
        //public DbSet<Board> Board { get; set; }

        public DbSet<User> Users { get; set; }

        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-UEBLV7T\\SQLEXPRESS;Database=KanbanDB;Trusted_Connection=True;");
        }
    }
}
