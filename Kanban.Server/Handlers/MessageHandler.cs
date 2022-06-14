using Kanban.Server.SocketsManager;
using System.Net.WebSockets;
using System.Text;
using Core;
using Newtonsoft.Json;
using Kanban.Server.DAL;
using Kanban.Server.Log;

namespace Kanban.Server.Handlers
{
    public class MessageHandler : SocketHandler
    {
        private ConnectionManager connectionManager;

        public MessageHandler(ConnectionManager connectionManager) : base(connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            Token token = connectionManager.GetToken(socket);

            ConsoleLogger.Log(new Info(), $"New connection: {token.Id} / {token.Lifetime}");

            Response response = new Response
            {
                Code = 200,
                Header = "Connceted",
                Body = token
            };

            await SendMessageToAll(response);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
        }
        public static T ConvertTo<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var requestByte = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Request? request = JsonConvert.DeserializeObject<Request>(requestByte);

            ConsoleLogger.Log(new Info(), $"Request {request?.Method} / {request?.Header} accepted");
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            if (request?.Method == "POST")
            {
                if (request.Header == "User")
                    PostUser(request, socket);
                else if (request.Header == "Board")
                    PostBoard(request, socket);
                else if (request.Header == "Column")
                    PostColumn(request, socket);
                else if (request.Header == "Card")
                    PostCard(request, socket);
            }
            else if (request?.Method == "GET")
            {
                if (request.Header == "Authentication")
                    GetAuthentication(request, socket);
                else if(request.Header == "Boards")
                    GetBoards(request, socket);
                else if (request.Header == "ColumnsByBoard")
                    GetColumnsByBoard(request, socket);
                else if (request.Header == "CardsByColumn")
                    GetCardsByColumn(request, socket);
                else if (request.Header == "BoardNameById")
                    BoardNameById(request, socket);
            }
            else if (request?.Method == "PUT")
            {
                if (request.Header == "Card")
                    EditCard(request, socket);
                else if (request.Header == "Column")
                    EditColumnById(request, socket);
                else if (request.Header == "Board")
                    EditBoardById(request, socket);
            }
            else if (request?.Method == "DELETE")
            {
                if (request.Header == "Card")
                    DeleteCardById(request, socket);
                else if (request.Header == "Column")
                    DeleteColumnById(request, socket);
                else if (request.Header == "Board")
                    DeleteBoardById(request, socket);
            }

            startTime.Stop();

            ConsoleLogger.Log(new Debug(), $"Request processing time: {startTime.Elapsed.TotalMilliseconds} ms");
        }

        private async void GetAuthentication(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "User authenticated";

            Token token = connectionManager.GetToken(socket);

            User? user = ConvertTo<User>(request.Body);

            if (DatabaseRepository.GetUserByNameAndPassword(user.Name, user.Password) != null)
            {
                DateTime nowDate = DateTime.Now;
                token.Lifetime = nowDate.AddMinutes(5);
                body = token;
            }
            else
            {
                code = 500;
                header = request.Header;
                body = "User not found";
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void GetBoards(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "Borders geted";

            Token token = connectionManager.GetToken(socket);

            List<Board> boards = new List<Board>();

            try
            {
                boards = DatabaseRepository.GetAllBoards();
                body = boards;
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void BoardNameById(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "Border geted";

            Token token = connectionManager.GetToken(socket);

            string name = "";
            Guid id = new Guid();

            try
            {
                id = new Guid(request.Body.ToString());
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            try
            {
                name = DatabaseRepository.GetBorderNameById(id);
                body = name;
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void GetColumnsByBoard(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "Border geted";

            Token token = connectionManager.GetToken(socket);

            List<Column> columns = new List<Column>();
            Guid id = new Guid();

            try
            {
                id = new Guid(request.Body.ToString());
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            try
            {
                columns = DatabaseRepository.GetColumnsByBoardId(id);
                body = columns;
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void GetCardsByColumn(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "Border geted";

            Token token = connectionManager.GetToken(socket);

            List<Card> cards = new List<Card>();
            Guid id = new Guid();

            try
            {
                id = new Guid(request.Body.ToString());
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            try
            {
                cards = DatabaseRepository.GetCardsByColumnId(id);
                body = cards;
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void PostUser(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "The user is posted";

            Token token = connectionManager.GetToken(socket);

            User? user = new User();

            try
            {
                user = ConvertTo<User>(request.Body);
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            if (DatabaseRepository.GetUserByName(user.Name) == null)
            {
                try
                {
                    DatabaseRepository.Add(user);
                }
                catch
                {
                    code = 502;
                    header = "Error";
                    body = "Ошибка добавления в базу данных";
                    ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
                }
            }
            else
            {
                code = 503;
                header = request.Header;
                body = "User already exists";
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void PostBoard(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "";

            Token token = connectionManager.GetToken(socket);

            Board? board = new Board();

            try
            {
                board = ConvertTo<Board>(request.Body);
                body = board;
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            try
            {
                DatabaseRepository.Add(board);
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void PostColumn(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "";

            Token token = connectionManager.GetToken(socket);

            Column column = new Column();

            try
            {
                column = ConvertTo<Column>(request.Body);
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            try
            {
                DatabaseRepository.Add(column);
                body = column;
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        private async void PostCard(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "";

            Token token = connectionManager.GetToken(socket);

            Card card = new Card();

            try
            {
                card = ConvertTo<Card>(request.Body);
            }
            catch
            {
                code = 501;
                header = "Error";
                body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {code}");
            }

            try
            {
                DatabaseRepository.Add(card);
                body = card;
            }
            catch
            {
                code = 502;
                header = "Error";
                body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {code}");
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessage(token, response);
        }

        // Илья норм
        private async void EditCard(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "";

            try
            {
                Card card = ConvertTo<Card>(request.Body);
                DatabaseRepository.EditCard(card);
                body = card;
            }
            catch
            {
                code = 500;
                header = "Error";
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessageToAll(response);
        }

        // Илья норм
        private async void EditColumnById(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "";

            try
            {
                Column column = ConvertTo<Column>(request.Body);
                DatabaseRepository.EditNameToColumn(column.Id, column.Name);
                body = column;
            }
            catch
            {
                code = 500;
                header = "Error";
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessageToAll(response);
        }

        // Илья норм
        private async void EditBoardById(Request request, WebSocket socket)
        {
            int code = 200;
            string header = request.Header;
            object body = "";

            try
            {
                Board board = ConvertTo<Board>(request.Body);
                DatabaseRepository.EditNameToBoard(board.Id, board.Name);
                body = board;
            }
            catch
            {
                code = 500;
                header = "Error";
            }

            Response response = new Response
            {
                Code = code,
                Header = header,
                Body = body
            };

            ConsoleLogger.Log(new Debug(), $"Code: {code}");

            await SendMessageToAll(response);
        }

        // Илья норм
        private async void DeleteCardById(Request request, WebSocket socket)
        {
            Response response = new Response()
            {
                Code = 200,
                Header = request.Header,
                Body = ""
            };

            Card card = new Card();

            try
            {
                card = ConvertTo<Card>(request.Body);
            }
            catch
            {
                response.Code = 501;
                response.Header = "Error";
                response.Body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {response.Code}");
            }

            try
            {
                DatabaseRepository.DeleteCardById(card.Id);
                response.Body = card;
            }
            catch
            {
                response.Code = 502;
                response.Header = "Error";
                response.Body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {response.Code}");
            }

            ConsoleLogger.Log(new Debug(), $"Code: {response.Code}");

            await SendMessageToAll(response);
        }

        // Илья норм
        private async void DeleteColumnById(Request request, WebSocket socket)
        {
            Response response = new Response()
            {
                Code = 200,
                Header = request.Header,
                Body = ""
            };

            Column column = new Column();

            try
            {
                column = ConvertTo<Column>(request.Body);
            }
            catch
            {
                response.Code = 501;
                response.Header = "Error";
                response.Body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {response.Code}");
            }

            try
            {
                DatabaseRepository.DeleteColumnById(column.Id);
                response.Body = column;
            }
            catch
            {
                response.Code = 502;
                response.Header = "Error";
                response.Body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {response.Code}");
            }

            ConsoleLogger.Log(new Debug(), $"Code: {response.Code}");

            await SendMessageToAll(response);
        }

        // Илья норм
        private async void DeleteBoardById(Request request, WebSocket socket)
        {
            Response response = new Response()
            {
                Code = 200,
                Header = request.Header,
                Body = ""
            };

            Board board = new Board();

            try
            {
                board = ConvertTo<Board>(request.Body);
            }
            catch
            {
                response.Code = 501;
                response.Header = "Error";
                response.Body = "Не верный формат тела запроса";
                ConsoleLogger.Log(new Error(), $"Invalid request body format, code: {response.Code}");
            }

            try
            {
                response.Body = DatabaseRepository.DeleteColumnById(board.Id);
            }
            catch
            {
                response.Code = 502;
                response.Header = "Error";
                response.Body = "Ошибка добавления в базу данных";
                ConsoleLogger.Log(new Error(), $"Error adding to the database, code: {response.Code}");
            }

            ConsoleLogger.Log(new Debug(), $"Code: {response.Code}");

            await SendMessageToAll(response);
        }
    }
}
