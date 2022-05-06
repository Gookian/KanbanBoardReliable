using Kanban.DesktopClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
