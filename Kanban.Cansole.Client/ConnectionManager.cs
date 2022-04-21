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

            Console.WriteLine("Connected to server");

            messageService.ResponseAsync();

            while (true)
            {
                var line = Console.ReadLine();

                Request request = new Request
                {
                    Method = "GET",
                    Header = line,
                    Body = null
                };

                messageService.Send(request);
            }
        }

        ~ConnectionManager()
        {
        }
    }
}

/*User user = new User() {
    Id = new Guid(),
    Name = "Gers",
    Password = "fFgSmH"
};*/

/*Card card1 = new Card()
{
    Id = new Guid(),
    Title = "Сходить в магазин",
    Description = "Купить молока и сахара",
    StoryPoint = 7,
    Date = DateTime.Now
};

Card card2 = new Card()
{
    Id = new Guid(),
    Title = "Помыть кошку",
    Description = "Искупать кошку",
    StoryPoint = 7,
    Date = DateTime.Now
};

Column column1 = new Column()
{
    Id = new Guid(),
    Name = "Backlog",
    Card = { card1 }
};

Column column2 = new Column()
{
    Id = new Guid(),
    Name = "To Do",
    Card = { card1, card2 }
};

Column column3 = new Column()
{
    Id = new Guid(),
    Name = "In Process",
    Card = { card2 }
};

Column column4 = new Column()
{
    Id = new Guid(),
    Name = "Done",
    Card = { card2, card1 }
};

Board board = new Board()
{
    Id = new Guid(),
    Name = line,
    Column = { column1, column2, column3, column4 }
};*/