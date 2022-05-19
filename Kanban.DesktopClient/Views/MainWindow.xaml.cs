using System.Windows;

namespace Kanban.DesktopClient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BindingContext.MainFrame = MainFrame;
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            BindingContext.MainFrame.Child = new RegistrationPage();
        }
    }
}
