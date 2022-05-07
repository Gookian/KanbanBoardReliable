using Kanban.Server.Log;

namespace Kanban.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleLogger.Log(new Input(), $"Join the database server {ConfigConnection.ConnectionLineToDataBase} (Y) or change it (N)?");

            var command = Console.ReadLine();
            if (command.ToLower() == "n")
            {
                ConsoleLogger.Log(new Input(), $"Enter connection string: ");
                ConfigConnection.ConnectionLineToDataBase = Console.ReadLine();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}