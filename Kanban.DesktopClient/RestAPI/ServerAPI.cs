using Core;
using System.Threading.Tasks;

namespace Kanban.DesktopClient.RestAPI
{
    public class ServerAPI
    {
        public static MessageService? MessageService { get; set; }

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

        public static async Task<Response> PostBoard(Board board)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "Board",
                Body = board
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> PostColumn(Column column)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "Column",
                Body = column
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> PostCard(Card card)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "Card",
                Body = card
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> GetUsers()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Users",
                Body = null
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> GetBoards()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Boards",
                Body = null
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> GetColumns()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Columns",
                Body = null
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> GetCards()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Cards",
                Body = null
            };

            return await MessageService.Send(request);
        }
    }
}
