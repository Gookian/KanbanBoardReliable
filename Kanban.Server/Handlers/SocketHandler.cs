namespace Kanban.Server.Handlers
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Newtonsoft.Json;
    using SocketsManager;

    public abstract class SocketHandler
    {
        public ConnectionManager ConnectionManager { get; set; }

        public SocketHandler(ConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { ConnectionManager.AddSocket(socket); });
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await ConnectionManager.RemoveSocketAsync(ConnectionManager.GetToken(socket));
        }

        public async Task SendMessage(WebSocket socket, Response response)
        {
            var responseJson = JsonConvert.SerializeObject(response, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(responseJson), 0, Encoding.UTF8.GetBytes(responseJson).Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessage(Token token, Response response)
        {
            await SendMessage(ConnectionManager.GetSocketByToken(token), response);
        }

        public async Task SendMessageToAll(Response response)
        {
            foreach (var connection in ConnectionManager.GetAllConnections())
            {
                await SendMessage(connection.Value, response);
            }
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
