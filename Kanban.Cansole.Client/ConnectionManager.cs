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
        private MessageService messageService;

        public ConnectionManager(string api)
        {
            _api = api;
        }

        public async Task StartConnection()
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri($"ws://localhost:5000/{_api}"), CancellationToken.None);
            messageService = new MessageService(client);
            ServerAPI.MessageService = messageService;

            Console.WriteLine("Connected to server");

            messageService.ResponseAsync();

            while (true)
            {
                var command = Console.ReadLine();

                var name = "";
                var password = "";
                var result = new Response();

                switch (command)
                {
                    case "Post /user":
                        Console.Write("Введите имя: ");
                        name = Console.ReadLine();
                        Console.Write("Введите пароль: ");
                        password = Console.ReadLine();
                        result = ServerAPI.PostUser(new User()
                        {
                            Id = new Guid(),
                            Name = name,
                            Password = password
                        });
                        Console.WriteLine(result.Code);
                        Console.WriteLine(result.Header);
                        Console.WriteLine(result.Body);
                        break;
                    case "Get /users":
                        result = ServerAPI.GetUsers();
                        Console.WriteLine(result.Code);
                        Console.WriteLine(result.Header);
                        Console.WriteLine(result.Body);
                        break;
                    case "Get /boards":
                        result = ServerAPI.GetUsers();
                        Console.WriteLine(result.Code);
                        Console.WriteLine(result.Header);
                        Console.WriteLine(result.Body);
                        break;
                    case "Get /columns":
                        result = ServerAPI.GetUsers();
                        Console.WriteLine(result.Code);
                        Console.WriteLine(result.Header);
                        Console.WriteLine(result.Body);
                        break;
                    case "Get /cards":
                        result = ServerAPI.GetUsers();
                        Console.WriteLine(result.Code);
                        Console.WriteLine(result.Header);
                        Console.WriteLine(result.Body);
                        break;
                };
            }
        }

        ~ConnectionManager()
        {
        }
    }
}