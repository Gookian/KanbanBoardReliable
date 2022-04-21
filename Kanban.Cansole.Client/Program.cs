using Core;
using Kanban.Server.DAL;

namespace Kanban.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string api = "message";
            ConnectionManager connectionManager = new ConnectionManager(api);
            connectionManager.StartConnection().GetAwaiter().GetResult();
        }
    }
}
