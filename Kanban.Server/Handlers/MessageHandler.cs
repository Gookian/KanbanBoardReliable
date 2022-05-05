using Kanban.Server.SocketsManager;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
using Core;
using Newtonsoft.Json;
using Kanban.Server.DAL;
using Kanban.Server.Log;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Server.Handlers
{
    public class MessageHandler : SocketHandler
    {
        //private readonly ConcurrentDictionary<Token, WebSocket> _connections = new();

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

            //_connections.TryAdd(token, socket);

            Response response = new Response
            {
                Code = 200,
                Header = "Connceted",
                Body = token
            };

            var responseJson = JsonConvert.SerializeObject(response);

            await SendMessage(token, responseJson);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
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
                /*else if (request.Header == "Board")
                    PostBoard(request);
                else if (request.Header == "Column")
                    PostColumn(request);
                else if (request.Header == "Card")
                    PostCard(request);*/
            }
            else if (request?.Method == "GET")
            {
                if (request.Header == "Authentication")
                    GetAuthentication(request, socket);
                /*if (request.Header == "Users")
                    GetUsers(request);
                else if(request.Header == "Boards")
                    GetBoards(request);
                else if(request.Header == "Columns")
                    GetColumns(request);
                else if(request.Header == "Cards")
                    GetCards(request);*/
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

            User? user = JsonConvert.DeserializeObject<User>(request.Body.ToString());

            User userDB = DatabaseRepository.GetUserByNameAndPassword(user.Name, user.Password);

            if (userDB != null)
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

            var responseJson = JsonConvert.SerializeObject(response);

            await SendMessage(token, responseJson);
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
                user = JsonConvert.DeserializeObject<User>(request.Body.ToString());
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

            var responseJson = JsonConvert.SerializeObject(response);

            await SendMessage(token, responseJson);
        }

        /* private async void PostUser(Request request)
         {
             int code = 200;
             string header = request.Header;
             string body = "The user is posted";

             User user = new User();

             try
             {
                 user = JsonConvert.DeserializeObject<User>(request.Body.ToString());
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
                 DatabaseRepository.Add(user);
             }
             catch
             {
                 code = 500;
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

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void PostBoard(Request request)
         {
             int code = 200;
             string header = request.Header;
             string body = "The board is posted";

             Board board = new Board();

             try
             {
                 board = JsonConvert.DeserializeObject<Board>(request.Body.ToString());
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
                 code = 500;
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

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void PostColumn(Request request)
         {
             int code = 200;
             string header = request.Header;
             string body = "The column is posted";

             Column column = new Column();

             try
             {
                 column = JsonConvert.DeserializeObject<Column>(request.Body.ToString());
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
             }
             catch
             {
                 code = 500;
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

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void PostCard(Request request)
         {
             int code = 200;
             string header = request.Header;
             string body = "The column is posted";

             Card card = new Card();

             try
             {
                 card = JsonConvert.DeserializeObject<Card>(request.Body.ToString());
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
             }
             catch
             {
                 code = 500;
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

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void GetUsers(Request request)
         {
             int code = 200;
             string header = request.Header;
             object body;

             try
             {
                 body = DatabaseRepository.GetAllUsers();
             }
             catch
             {
                 code = 502;
                 header = "Error";
                 body = "Ошибка получения данных из базы данных";
                 ConsoleLogger.Log(new Error(), $"Error getting data from the database, code: {code}");
             }

             Response response = new Response
             {
                 Code = code,
                 Header = header,
                 Body = body
             };

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void GetBoards(Request request)
         {
             int code = 200;
             string header = request.Header;
             object body;

             try
             {
                 body = DatabaseRepository.GetAllBoards();
             }
             catch
             {
                 code = 502;
                 header = "Error";
                 body = "Ошибка получения данных из базы данных";
                 ConsoleLogger.Log(new Error(), $"Error getting data from the database, code: {code}");
             }

             Response response = new Response
             {
                 Code = code,
                 Header = header,
                 Body = body
             };

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void GetColumns(Request request)
         {
             int code = 200;
             string header = request.Header;
             object body;

             try
             {
                 body = DatabaseRepository.GetAllColumns();
             }
             catch
             {
                 code = 502;
                 header = "Error";
                 body = "Ошибка получения данных из базы данных";
                 ConsoleLogger.Log(new Error(), $"Error getting data from the database, code: {code}");
             }

             Response response = new Response
             {
                 Code = code,
                 Header = header,
                 Body = body
             };

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }

         private async void GetCards(Request request)
         {
             int code = 200;
             string header = request.Header;
             object body;

             try
             {
                 body = DatabaseRepository.GetAllCards();
             }
             catch
             {
                 code = 502;
                 header = "Error";
                 body = "Ошибка получения данных из базы данных";
                 ConsoleLogger.Log(new Error(), $"Error getting data from the database, code: {code}");
             }

             Response response = new Response
             {
                 Code = code,
                 Header = header,
                 Body = body
             };

             var responseJson = JsonConvert.SerializeObject(response);

             await SendMessageToAll(responseJson);
         }*/
    }
}
