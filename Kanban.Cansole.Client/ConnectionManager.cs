using System.Text;
using Core;

namespace Kanban.ConsoleClient
{
    using Newtonsoft.Json;
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    class ConnectionManager
    {
        private readonly string _api;
        private ClientWebSocket client;

        public ConnectionManager(string api)
        {
            _api = api;
        }

        public async Task StartConnection()
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri($"ws://localhost:5000/{_api}"), CancellationToken.None);
            Console.WriteLine("Connected to server");

            Listener();

            while (true)
            {
                var line = Console.ReadLine();
                //Guid id = new Guid(line);

                User user = new User() {
                    Id = new Guid(),
                    Name = "Ivan",
                    Password = "QqqEee"
                };

                Request request = new Request
                {
                    Method = "POST",
                    Header = "User",
                    Body = user
                };

                SendMessage(request);
                Listener();
            }
        }

        public async void Listener()
        {
            while (this is not null)
            {
                var result = await ReciveAsync();
                //var user = JsonConvert.DeserializeObject<User>(result.Body.ToString());

                Console.WriteLine(result.Code);
                Console.WriteLine(result.Header);
                Console.WriteLine(result.Body.ToString());
            }
        }

        public void SendMessage(Request request)
        {
            var jsonMessage = JsonConvert.SerializeObject(request);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);

            client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<Response> ReciveAsync()
        {
            var buffer = new byte[1024 * 4];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer);

            return JsonConvert.DeserializeObject<Response>(message);
        }
    }
}
