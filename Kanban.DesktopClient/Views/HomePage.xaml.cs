using System.Windows;
using System.Windows.Controls;

namespace Kanban.DesktopClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();

            BindingContext.HomeFrame = HomeFrame;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HomeFrame.Child = BindingContext.BoardPage;
        }
    }
}
