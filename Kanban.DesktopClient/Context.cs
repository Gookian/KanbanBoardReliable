using System.Windows.Controls;

namespace Kanban.DesktopClient
{
    public class Context
    {
        public static ConnectionManager connectionManager { get; set; }

        public static Border CreateBoardLocal { get; set; }
    }
}
