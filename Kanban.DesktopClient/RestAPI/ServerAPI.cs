using Core;
using System.Threading.Tasks;

namespace Kanban.DesktopClient.RestAPI
{
    public class ServerAPI
    {
        public static MessageService? MessageService { get; set; }

        public static async Task<Response> GetAuthentication(User user)
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Authentication",
                Body = user
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> PostUser(User user)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "User",
                Body = user
            };

            return await MessageService.Send(request);
        }
    }
}
