using Core;

namespace Kanban.DesktopClient
{
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Kanban.DesktopClient.RestAPI;

    public class ConnectionManager
    {
        private readonly string _api;
        private ClientWebSocket client;
        public MessageService messageService;

        public ConnectionManager(string api)
        {
            _api = api;
        }

        public async void StartConnection()
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri($"ws://localhost:5000/{_api}"), CancellationToken.None);
            messageService = new MessageService(client);
            ServerAPI.MessageService = messageService;

            Console.WriteLine("Connected to server");

            messageService.ResponseAsync();
        }
    }
}