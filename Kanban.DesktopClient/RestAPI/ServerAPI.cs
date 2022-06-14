using Core;
using Newtonsoft.Json;
using System;
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

        public static async Task<Response> GetBoardNameById(Guid id)
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "BoardNameById",
                Body = id
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> GetColumnsByBoardId(Guid id)
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "ColumnsByBoard",
                Body = id
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> GetCardsByColumnId(Guid id)
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "CardsByColumn",
                Body = id
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

        public static async Task<Response> EditCard(Card card)
        {
            Request request = new Request()
            {
                Method = "PUT",
                Header = "Card",
                Body = card
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> EditColumn(Column column)
        {
            Request request = new Request()
            {
                Method = "PUT",
                Header = "Column",
                Body = column
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> EditBoard(Board board)
        {
            Request request = new Request()
            {
                Method = "PUT",
                Header = "Board",
                Body = board
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> DeleteCard(Card card)
        {
            Request request = new Request()
            {
                Method = "DELETE",
                Header = "Card",
                Body = card
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> DeleteColumn(Column column)
        {
            Request request = new Request()
            {
                Method = "DELETE",
                Header = "Column",
                Body = column
            };

            return await MessageService.Send(request);
        }

        public static async Task<Response> DeleteBoard(Board board)
        {
            Request request = new Request()
            {
                Method = "DELETE",
                Header = "Board",
                Body = board
            };

            return await MessageService.Send(request);
        }

        public static T ConvertTo<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }
    }
}
