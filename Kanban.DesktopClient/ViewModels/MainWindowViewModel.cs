namespace Kanban.DesktopClient.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            string api = "message";
            Context.connectionManager = new ConnectionManager(api);
            Context.connectionManager.StartConnection();
        }
    }
}
