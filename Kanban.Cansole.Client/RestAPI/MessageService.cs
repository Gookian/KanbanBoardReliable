using Core;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Kanban.CansoleClient.RestAPI
{
    public class MessageService
    {
        public static ClientWebSocket? client;

        private Response response;

        public MessageService(ClientWebSocket clientWebSocket)
        {
            client = clientWebSocket;
        }

        public Response Send(Request request)
        {
            SendMessage(request);
            ResponseAsync();

            Thread.Sleep(200);

            return response;
        }

        public void SendMessage(Request request)
        {
            var jsonMessage = JsonConvert.SerializeObject(request);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);

            client?.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async void ResponseAsync()
        {
            response = await ReciveAsync();
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
