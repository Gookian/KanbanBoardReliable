using Core;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Server.DAL
{
    public class DatabaseRepository
    {
        private static Context db = new Context();

        public static List<Board> GetAllBoards()
        {
            return db.Boards.ToList();
        }

        public static List<Column> GetAllColumns()
        {
            return db.Columns.ToList();
        }

        public static DbSet<Card> GetAllCards()
        {
            return db.Cards;
        }

        public static DbSet<User> GetAllUsers()
        {
            return db.Users;
        }

        public static User GetUserByNameAndPassword(string name, string password)
        {
            return db.Users.Where(x => x.Name == name && x.Password == password).FirstOrDefault();
        }

        public static User GetUserByName(string name)
        {
            return db.Users.Where(x => x.Name == name).FirstOrDefault();
        }

        public static void Add<T>(T element)
        {
            db.Add(element);
            db.SaveChanges();
        }
    }
}
