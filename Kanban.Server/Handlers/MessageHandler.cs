using Kanban.Server.SocketsManager;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
using Core;
using Newtonsoft.Json;
using Kanban.Server.DAL;

namespace Kanban.Server.Handlers
{
    public class MessageHandler : SocketHandler
    {
        private readonly ConcurrentDictionary<User, WebSocket> _connections = new();

        public MessageHandler(ConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = $"User{_connections.Count}"
            };

            _connections.TryAdd(user, socket);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var requestByte = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Request request = JsonConvert.DeserializeObject<Request>(requestByte);

            if (request?.Method == "POST")
            {
                if (request.Header == "User")
                    PostUser(request);
                /*Guid id = new Guid(request.Body.ToString());
                User user = DatabaseRepository.GetUserById(id);

                Response response = new Response
                {
                    Code = 200,
                    Header = request.Header,
                    Body = user
                };

                var responseJson = JsonConvert.SerializeObject(response);

                await SendMessageToAll(responseJson);*/
            }
        }

        private async void PostUser(Request request)
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
                code = 401;
                header = "Error";
                body = "Не верный формат тела запроса";
            }

            try
            {
                DatabaseRepository.Add(user);
            }
            catch
            {
                code = 400;
                header = "Error";
                body = "Ошибка добавления в базу данных";
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
    }
}
