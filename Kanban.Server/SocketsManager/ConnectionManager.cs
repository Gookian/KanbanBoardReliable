using Core;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Kanban.Server.SocketsManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<Token, WebSocket> _connections = new ConcurrentDictionary<Token, WebSocket>();

        public WebSocket GetSocketByToken(Token token)
        {
            return _connections.FirstOrDefault(x => x.Key == token).Value;
        }

        public ConcurrentDictionary<Token, WebSocket> GetAllConnections()
        {
            return _connections;
        }

        public Token GetToken(WebSocket socket)
        {
            return _connections.FirstOrDefault(x => x.Value == socket).Key;
        }

        public async void RemoveConnection(Token token)
        {
            _connections.TryRemove(token, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, $"Connection closed", CancellationToken.None);
        }

        public async Task RemoveSocketAsync(Token token)
        {
            _connections.TryRemove(token, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, $"Connection closed", CancellationToken.None);
        }

        public void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionToken(), socket);
        }

        private Token GetConnectionToken()
        {
            return new Token
            {
                Id = Guid.NewGuid(),
                Lifetime = new DateTime()
            };
        }
    }
}
