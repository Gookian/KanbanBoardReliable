using Core;

namespace Kanban.Server.DAL
{
    public class DatabaseRepository
    {
        public static List<Board> GetAllBoards()
        {
            using (Context db = new Context())
            {
                return db.Boards.ToList();
            }
        }

        public static List<Column> GetAllColumns()
        {
            using (Context db = new Context())
            {
                return db.Columns.ToList();
            }
        }

        public static List<Card> GetAllCards()
        {
            using (Context db = new Context())
            {
                return db.Cards.ToList();
            }
        }

        public static List<User> GetAllUsers()
        {
            using (Context db = new Context())
            {
                return db.Users.ToList();
            }
        }

        public static User? GetUserByNameAndPassword(string name, string password)
        {
            using (Context db = new Context())
            {
                return db.Users.FirstOrDefault(x => x.Name == name && x.Password == password);
            }
        }

        public static User? GetUserByName(string name)
        {

            using (Context db = new Context())
            {
                return db.Users.FirstOrDefault(x => x.Name == name);
            }
        }

        public static string GetBorderNameById(Guid id)
        {
            using (Context db = new Context())
            {
                return db.Boards.FirstOrDefault(x => x.Id == id).Name;
            }
        }

        public static List<Column>? GetColumnsByBoardId(Guid id)
        {
            using (Context db = new Context())
            {
                return db.Columns.Where(x => x.BoardId == id).ToList();
            }
        }

        public static List<Card>? GetCardsByColumnId(Guid id)
        {
            using (Context db = new Context())
            {
                return db.Cards.Where(x => x.ColumnId == id).ToList();
            }
        }

        public static User Add(User user)
        {
            using (Context db = new Context())
            {
                db.Users.Add(user);
                db.SaveChanges();

                return user;
            }
        }

        public static Board Add(Board board)
        {
            using (Context db = new Context())
            {
                db.Boards.Add(board);
                db.SaveChanges();

                return board;
            }
        }

        public static Column Add(Column column)
        {
            using (Context db = new Context())
            {
                Board? board = db.Boards.FirstOrDefault(x => x.Id == column.BoardId);

                board.Column.Add(column);

                db.Boards.Update(board);
                db.SaveChanges();

                return column;
            }
        }

        public static Card Add(Card card)
        {
            using (Context db = new Context())
            {
                Column? column = db.Columns.FirstOrDefault(x => x.Id == card.ColumnId);

                column?.Card.Add(card);

                db.Columns.Update(column);
                db.SaveChanges();

                return card;
            }
        }

        public static Board EditNameToBoard(Guid id, string newName)
        {
            using (Context db = new Context())
            {
                Board? board = db.Boards.FirstOrDefault(x => x.Id == id);

                board.Name = newName;

                db.Boards.Update(board);
                db.SaveChanges();

                return board;
            }
        }

        public static Column EditNameToColumn(Guid id, string newName)
        {
            using (Context db = new Context())
            {
                Column? column = db.Columns.FirstOrDefault(x => x.Id == id);

                column.Name = newName;

                db.Columns.Update(column);
                db.SaveChanges();

                return column;
            }
        }

        public static Card EditCard(Card newCard)
        {
            using (Context db = new Context())
            {
                Card? card = db.Cards.FirstOrDefault(x => x.Id == newCard.Id);

                card.Title = newCard.Title;
                card.Description = newCard.Description;
                card.StoryPoint = newCard.StoryPoint;
                card.Date = newCard.Date;


                db.Cards.Update(card);
                db.SaveChanges();

                return card;
            }
        }

        public static Board DeleteBoardById(Guid id)
        {
            using (Context db = new Context())
            {
                Board board = db.Boards.FirstOrDefault(x => x.Id == id);

                if (board != null)
                {
                    db.Boards.Remove(board);
                    db.SaveChanges();

                    return board;
                }

                return null;
            }
        }

        public static Column DeleteColumnById(Guid id)
        {
            using (Context db = new Context())
            {
                Column column = db.Columns.FirstOrDefault(x => x.Id == id);

                if (column != null)
                {
                    db.Columns.Remove(column);
                    db.SaveChanges();

                    return column;
                }

                return null;
            }
        }

        public static Card DeleteCardById(Guid id)
        {
            using (Context db = new Context())
            {
                Card card = db.Cards.FirstOrDefault(x => x.Id == id);

                if (card != null)
                {
                    db.Cards.Remove(card);
                    db.SaveChanges();

                    return card;
                }

                return null;
            }
        }
    }
}
