using Core;
using Kanban.DesktopClient.Models;
using Kanban.DesktopClient.RestAPI;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Kanban.DesktopClient.Views
{
    /// <summary>
    /// Логика взаимодействия для KanbanPage.xaml
    /// </summary>
    public partial class KanbanPage : UserControl
    {
        public KanbanPage()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindingContext.SpaceForColumn = SpaceForColumn;
            BindingContext.PlaceToPupup = PlaceToPupup;
            BindingContext.KanbanPageName = KanbanPageName;

            Response response = await ServerAPI.GetBoardNameById(Context.IdTargetBoard);

            KanbanPageName.Text = $"Доски > {response.Body.ToString()}";

            response = await ServerAPI.GetColumnsByBoardId(Context.IdTargetBoard);

            List<Column> columns = ServerAPI.ConvertTo<List<Column>>(response.Body);

            BindingContext.SpaceForColumn.Children.Clear();
            StackPanelRepository.Clear();

            foreach (var column in columns)
            {

                BindingContext.SpaceForColumn.Children.Add(UIFactory.CreateColumn(column));

                response = await ServerAPI.GetCardsByColumnId(column.Id);

                List<Card> cards = ServerAPI.ConvertTo<List<Card>>(response.Body);

                StackPanel stackPanel = StackPanelRepository.GetById(column.Id);

                foreach (var card in cards)
                {
                    stackPanel.Children.Add(UIFactory.CreateCard(card));
                }
            }
        }
    }
}
