namespace Kanban.Server.Log
{
    public class Info : ILoggerLevel
    {
        public string NameLevel => "info";

        public ConsoleColor Color => ConsoleColor.DarkGreen;
    }
}
