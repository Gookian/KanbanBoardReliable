using Core;
using Kanban.DesktopClient.Models;
using Kanban.DesktopClient.RestAPI;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            PersonalBoards.Width = this.Width;

            BindingContext.PersonalBoards = PersonalBoards;
            BindingContext.PlaceToPupup = PlaceToPupup;

            Response response = await ServerAPI.GetBoards();
            List<Board> boards = ServerAPI.ConvertTo<List<Board>>(response.Body);

            BindingContext.PersonalBoards.Children.Clear();

            foreach (var board in boards)
            {
                BindingContext.PersonalBoards.Children.Add(UIFactory.CreateBoard(board));
            }

            Context.targetPage = new BoardPage();
        }
    }
}
