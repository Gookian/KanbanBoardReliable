using Core;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.DesktopClient.RestAPI
{
    public class MessageService
    {
        public static ClientWebSocket? client;

        public MessageService(ClientWebSocket clientWebSocket)
        {
            client = clientWebSocket;
        }

        public async Task<Response> Send(Request request)
        {
            SendMessage(request);
            return await ResponseAsync();
        }

        public void SendMessage(Request request)
        {
            var jsonMessage = JsonConvert.SerializeObject(request);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);

            client?.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<Response> ResponseAsync()
        {
            return await ReciveAsync();
        }

        public async Task<Response> ReciveAsync()
        {
            var buffer = new byte[1024 * 4];
            await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer);

            return JsonConvert.DeserializeObject<Response>(message);
        }
    }
}
