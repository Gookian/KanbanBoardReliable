using Core;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Kanban.CansoleClient.RestAPI
{
    public class MessageService
    {
        public static ClientWebSocket? client;

        public MessageService(ClientWebSocket clientWebSocket)
        {
            client = clientWebSocket;
        }

        public void Send(Request request)
        {
            SendMessage(request);
            ResponseAsync();
        }

        public void SendMessage(Request request)
        {
            var jsonMessage = JsonConvert.SerializeObject(request);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);

            client?.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async void ResponseAsync()
        {
            var response = await ReciveAsync();
            Console.WriteLine(response.Code);
            Console.WriteLine(response.Header);

            try
            {
                Token? token = JsonConvert.DeserializeObject<Token>(response.Body.ToString());
                Console.WriteLine(token.Id);
                Console.WriteLine(token.Lifetime);
            }
            catch { }
            try
            {
                Console.WriteLine(response.Body.ToString());
            }
            catch { }
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
