using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.DesktopClient.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            string api = "message";
            ConnectionManager connectionManager = new ConnectionManager(api);
            connectionManager.StartConnection();
        }
    }
}
