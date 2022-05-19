using System.Windows;

namespace Kanban.DesktopClient.Views
{
    /// <summary>
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public string TitleA { get; set; } = "Error";
        public string DescriptionA { get; set; } = "Description";

        public ErrorWindow(string title, string description)
        {
            TitleA = title;
            DescriptionA = description;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title.Text = TitleA;
            Description.Text = DescriptionA;
            OkButton.Click += OkButton_Click;
        }
    }
}
