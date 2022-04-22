namespace Kanban.Server.Log
{
    public class Warn : ILoggerLevel
    {
        public string NameLevel => "warn";

        public ConsoleColor Color => ConsoleColor.DarkYellow;
    }
}
