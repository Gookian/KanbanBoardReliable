using Core;
using Kanban.DesktopClient.Models;
using Kanban.DesktopClient.RestAPI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kanban.DesktopClient.Views
{
    /// <summary>
    /// Логика взаимодействия для BorderPage.xaml
    /// </summary>
    public partial class BoardPage : UserControl
    {
        public BoardPage()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindingContext.PersonalBoards = PersonalBoards;
            BindingContext.PlaceToPupup = PlaceToPupup;

            Response response = await ServerAPI.GetBoards();
            List<Board> boards = ServerAPI.ConvertTo<List<Board>>(response.Body);

            BindingContext.PersonalBoards.Children.Clear();

            foreach (var board in boards)
            {
                BindingContext.PersonalBoards.Children.Add(UIFactory.CreateBoard(board));
            }
        }
    }
}
