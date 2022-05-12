using Core;

namespace TestFunctionDataBase
{
    class Program
    {
        public static bool isExit = false;

        static void Main(string[] args)
        {
            while (!isExit)
            {
                Console.WriteLine("Введите команду Базы данных");

                string? command = Console.ReadLine();

                try
                {
                    if (command.ToLower() == "exit")
                    {
                        isExit = true;
                    }
                    else if (command.ToLower() == "getallboards")
                    {
                        List<Board> boards = DatabaseRepository.GetAllBoards();

                        foreach (var board in boards)
                        {
                            Console.WriteLine($"{board.Name}");
                        }
                    }
                    else if (command.ToLower() == "getallcolumns")
                    {
                        List<Column> columns = DatabaseRepository.GetAllColumns();

                        foreach (var column in columns)
                        {
                            Console.WriteLine($"{column.Name}");
                        }
                    }
                    else if (command.ToLower() == "getallcards")
                    {
                        List<Card> cards = DatabaseRepository.GetAllCards();

                        foreach (var card in cards)
                        {
                            Console.WriteLine($"{card.Title} / {card.Description} / {card.StoryPoint} / {card.Date}");
                        }
                    }
                    else if (command.ToLower() == "getallusers")
                    {
                        List<User> users = DatabaseRepository.GetAllUsers();

                        foreach (var user in users)
                        {
                            Console.WriteLine($"{user.Name} / {user.Password}");
                        }
                    }
                    else if (command.ToLower() == "getuserbynameandpassword")
                    {
                        Console.Write("Введите логин:");
                        var login = Console.ReadLine();

                        Console.Write("Введите пароль:");
                        var password = Console.ReadLine();

                        User? user = DatabaseRepository.GetUserByNameAndPassword(login, password);

                        Console.WriteLine($"{user.Name} / {user.Password}");
                    }
                    else if (command.ToLower() == "getuserbyname")
                    {
                        Console.Write("Введите логин:");
                        var login = Console.ReadLine();

                        User? user = DatabaseRepository.GetUserByName(login);

                        Console.WriteLine($"{user.Name} / {user.Password}");
                    }
                    else if (command.ToLower() == "getcolumnsbyboardid")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        List<Column>? columns = DatabaseRepository.GetColumnsByBoardId(id);

                        foreach (Column column in columns)
                        {
                            Console.WriteLine($"{column.Name} / {column.Card.Count}");
                        }
                    }
                    else if (command.ToLower() == "adduser")
                    {
                        Console.Write("Введите имя:");
                        var name = Console.ReadLine();

                        Console.Write("Введите пароль:");
                        var password = Console.ReadLine();

                        User user = new User()
                        {
                            Id = new Guid(),
                            Name = name,
                            Password = password
                        };

                        DatabaseRepository.Add(user);
                    }
                    else if (command.ToLower() == "addboard")
                    {
                        Console.Write("Введите имя:");
                        var name = Console.ReadLine();

                        Board board = new Board()
                        {
                            Id = new Guid(),
                            Name = name
                        };

                        DatabaseRepository.Add(board);
                    }
                    else if (command.ToLower() == "addcolumn")
                    {
                        Console.Write("Введите имя:");
                        var name = Console.ReadLine();

                        Console.Write("Введите id для связи с Board:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Column column = new Column()
                        {
                            Id = new Guid(),
                            Name = name,
                            BoardId = id
                        };

                        DatabaseRepository.Add(column);
                    }
                    else if (command.ToLower() == "addcard")
                    {
                        Console.Write("Введите имя:");
                        var title = Console.ReadLine();

                        Console.Write("Введите описание:");
                        var description = Console.ReadLine();

                        Console.Write("Введите id для связи с Column:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Card card = new Card()
                        {
                            Id = new Guid(),
                            Title = title,
                            Description = description,
                            Date = DateTime.Now,
                            ColumnId = id
                        };

                        DatabaseRepository.Add(card);
                    }
                    else if (command.ToLower() == "editnametoboard")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Console.Write("Введите имя:");
                        var name = Console.ReadLine();

                        Board board = DatabaseRepository.EditNameToBoard(id, name);

                        Console.WriteLine($"{board.Name}");
                    }
                    else if (command.ToLower() == "editnametocolumn")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Console.Write("Введите имя:");
                        var name = Console.ReadLine();

                        Column column = DatabaseRepository.EditNameToColumn(id, name);

                        Console.WriteLine($"{column.Name}");
                    }
                    else if (command.ToLower() == "editcard")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Console.Write("Введите имя:");
                        var title = Console.ReadLine();

                        Console.Write("Введите описание:");
                        var description = Console.ReadLine();

                        Card card = new Card()
                        {
                            Id = id,
                            Title = title,
                            Description = description,
                            Date = DateTime.Now,
                        };

                        Card cardEdit = DatabaseRepository.EditCard(card);

                        Console.WriteLine($"{cardEdit.Title} / {cardEdit.Description} / {cardEdit.StoryPoint} / {cardEdit.Date}");
                    }
                    else if (command.ToLower() == "deleteboardbyid")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Board board = DatabaseRepository.DeleteBoardById(id);

                        Console.WriteLine($"{board.Name}");
                    }
                    else if (command.ToLower() == "deletecolumnbyid")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Column column = DatabaseRepository.DeleteColumnById(id);

                        Console.WriteLine($"{column.Name}");
                    }
                    else if (command.ToLower() == "deletecardbyid")
                    {
                        Console.Write("Введите id:");
                        var idStr = Console.ReadLine();
                        Guid id = new Guid(idStr);

                        Card card = DatabaseRepository.DeleteCardById(id);

                        Console.WriteLine($"{card.Title} / {card.Description} / {card.StoryPoint} / {card.Date}");
                    }
                }
                catch
                {
                    Console.WriteLine($"Что-то не так в {command}");
                }
            }
        }
    }
}