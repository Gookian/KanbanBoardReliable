namespace Kanban.Server.Log
{
    public class Fatal : ILoggerLevel
    {
        public string NameLevel => "fatal";

        public ConsoleColor Color => ConsoleColor.Magenta;
    }
}
