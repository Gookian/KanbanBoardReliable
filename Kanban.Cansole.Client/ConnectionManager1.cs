﻿using System.Text;
using Core;

namespace Kanban.ConsoleClient
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    class ConnectionManager1
    {
        private readonly string _api;
        private ClientWebSocket client;

        public ConnectionManager1(string api)
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

                Request request = new Request
                {
                    Method = "GET",
                    Header = line,
                    Body = null
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

                try
                {
                    List<Board> users = JsonConvert.DeserializeObject<List<Board>>(result.Body.ToString());

                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Column[0].Card[0].Title);
                    }
                }   catch { Console.WriteLine("ошибка"); }
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

        ~ConnectionManager1()
        {
        }
    }
}
