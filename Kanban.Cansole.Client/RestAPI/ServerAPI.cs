using Core;

namespace Kanban.CansoleClient.RestAPI
{
    public class ServerAPI
    {
        public static MessageService? MessageService { get; set; }

        public static Response PostUser(User user)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "User",
                Body = user
            };

            return MessageService.Send(request);
        }

        public static Response PostBoard(Board board)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "Board",
                Body = board
            };

            return MessageService.Send(request);
        }

        public static Response PostColumn(Column column)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "Column",
                Body = column
            };

            return MessageService.Send(request);
        }

        public static Response PostCard(Card card)
        {
            Request request = new Request()
            {
                Method = "POST",
                Header = "Card",
                Body = card
            };

            return MessageService.Send(request);
        }

        public static Response GetUsers()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Users",
                Body = null
            };

            return MessageService.Send(request);
        }

        public static Response GetBoards()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Boards",
                Body = null
            };

            return MessageService.Send(request);
        }

        public static Response GetColumns()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Columns",
                Body = null
            };

            return MessageService.Send(request);
        }

        public static Response GetCards()
        {
            Request request = new Request()
            {
                Method = "GET",
                Header = "Cards",
                Body = null
            };

            return MessageService.Send(request);
        }
    }
}
