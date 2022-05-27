using Core;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Server.DAL
{
    public class Context : DbContext
    {
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<Column> Columns { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<User> Users { get; set; }

        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server=DESKTOP-UEBLV7T\\SQLEXPRESS;Database=KanbanDB;Trusted_Connection=True;");
        }//(localdb)\\mssqllocaldb
    }
}
