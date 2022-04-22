namespace Kanban.Server.Log
{
    public class Debug : ILoggerLevel
    {
        public string NameLevel => "debug";

        public ConsoleColor Color => ConsoleColor.DarkBlue;
    }
}
