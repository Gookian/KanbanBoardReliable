namespace Kanban.Server.Log
{
    public class Error : ILoggerLevel
    {
        public string NameLevel => "error";

        public ConsoleColor Color => ConsoleColor.DarkRed;
    }
}
