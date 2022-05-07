namespace Kanban.Server.Log
{
    public class Input : ILoggerLevel
    {
        public string NameLevel => "input";

        public ConsoleColor Color => ConsoleColor.DarkBlue;
    }
}
