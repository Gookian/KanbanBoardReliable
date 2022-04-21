using System.Windows.Controls;

namespace Kanban.DesktopClient
{
    using Views;

    public static class BindingContext
    {
        public static Border MainFrame { get; set; }

        public static Border HomeFrame { get; set; }

        public static UserControl RegistrationPage = new RegistrationPage();

        public static UserControl AuthorizationPage = new AuthorizationPage();

        public static UserControl HomePage = new HomePage();

        public static UserControl BorderPage = new BorderPage();

        public static UserControl KanbanPage = new KanbanPage();
    }
}
