using Core;
using Kanban.Server.DAL;

namespace Kanban.ConsoleClient
{
    class Program
    {
        private static ConnectionManager connectionManager;

        static void Main(string[] args)
        {
            string api = "message";
            connectionManager = new ConnectionManager(api);
            connectionManager.StartConnection().GetAwaiter().GetResult();
        }
    }
}
