namespace Kanban.Server.Log
{
    public class ConsoleLogger
    {
        public static void Log(ILoggerLevel level, string message)
        {
            Console.ForegroundColor = level.Color;
            Console.Write($"{level.NameLevel}");
            Console.ResetColor();
            Console.WriteLine($": {message}");
        }
    }
}
