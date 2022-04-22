using Core;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Server.DAL
{
    public class DatabaseRepository
    {
        private static Context db = new Context();

        public static DbSet<Board> GetAllBoards()
        {
            return db.Boards;
        }

        public static DbSet<Column> GetAllColumns()
        {
            return db.Columns;
        }

        public static DbSet<Card> GetAllCards()
        {
            return db.Cards;
        }

        public static DbSet<User> GetAllUsers()
        {
            return db.Users;
        }

        public static User GetUserById(Guid id)
        {
            var result = db.Users.Where(x => x.Id == id).First();

            return result;
        }

        public static void Add<T>(T element)
        {
            db.Add(element);
            db.SaveChanges();
        }
    }
}
