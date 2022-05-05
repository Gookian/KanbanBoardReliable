using Core;

namespace Kanban.ConsoleClient
{
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Kanban.CansoleClient.RestAPI;

    class ConnectionManager
    {
        private readonly string _api;
        private ClientWebSocket client;
        public MessageService messageService;

        public ConnectionManager(string api)
        {
            _api = api;
        }

        public async Task StartConnection()
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri($"ws://localhost:5000/{_api}"), CancellationToken.None);
            messageService = new MessageService(client);
            //ServerAPI.MessageService = messageService;

            Console.WriteLine("Connected to server");

            messageService.ResponseAsync();

            SendLisener();
        }

        public async void SendLisener()
        {
            while (true)
            {
                var command = Console.ReadLine();

                User user = new User()
                {
                    Id = new Guid(),
                    Name = "danil",
                    Password = "101010"
                };

                messageService.Send(new Request
                {
                    Method = "POST",
                    Header = command,
                    Body = user
                });
            }
        }

        ~ConnectionManager()
        {
        }
    }
}