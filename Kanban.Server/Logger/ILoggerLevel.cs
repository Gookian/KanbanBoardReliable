namespace Kanban.Server.Log
{
    public interface ILoggerLevel
    {
        public string NameLevel { get; }

        public ConsoleColor Color { get; }
    }
}
