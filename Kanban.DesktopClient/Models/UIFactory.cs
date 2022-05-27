using Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System;
using Prism.Commands;
using System.Windows.Media.Effects;
using Kanban.DesktopClient.RestAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Services.Dialogs;

namespace Kanban.DesktopClient.Models
{
    public class UIFactory
    {
        static void GoToKanbanBoard_Click(Board board)
        {
            Context.IdTargetBoard = board.Id;
            BindingContext.MainFrame.Child = BindingContext.KanbanPage;
        }

        static async void OnDeleteCard_Click(Card cardDelete)
        {
            var result = MessageBox.Show("Удалить карточку?", "Удаление карточки", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                Response response = await ServerAPI.DeleteCard(cardDelete);

                if (response.Code == 200)
                    await UppdateDataBoard();
            }
        }

        static async void OnDeleteColumn_Click(Column columnDelete)
        {
            var result = MessageBox.Show("Удалить колонку?", "Удаление колонки", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                Response response = await ServerAPI.DeleteColumn(columnDelete);

                if (response.Code == 200)
                    await UppdateDataBoard();
            }
        }

        static async void OnDeleteBoard_Click(Board boardDelete)
        {
            var result = MessageBox.Show("Удалить доску?", "Удаление доски", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                Response response = await ServerAPI.DeleteBoard(boardDelete);
                // Ошибка в бд
                if (response.Code == 200)
                    await UppdateDataBoards();
            }
        }

        static async void AddBoard_Click(TextBox text)
        {
            Board board = new Board()
            {
                Id = new Guid(),
                Name = text.Text
            };

            Response response = await ServerAPI.PostBoard(board);
            if (response.Code != 501 || response.Code != 502)
            {
                Board responseBoard = ServerAPI.ConvertTo<Board>(response.Body);
                BindingContext.PersonalBoards.Children.Add(UIFactory.CreateBoard(responseBoard));
                BindingContext.PlaceToPupup.Children.Clear();
            }
        }

        static async void EditBoard_Click(IdAndNameTransport boardTransport)
        {
            Board board = new Board()
            {
                Id = boardTransport.Id,
                Name = boardTransport.Name.Text,
            };

            Response response = await ServerAPI.EditBoard(board);

            if (response.Code == 200)
            {
                BindingContext.PlaceToPupup.Children.Clear();
                await UppdateDataBoards();
            }
        }

        static async void AddColumn_Click(TextBox text)
        {
            Column column = new Column()
            {
                Id = new Guid(),
                Name = text.Text,
                BoardId = Context.IdTargetBoard
            };

            Response response = await ServerAPI.PostColumn(column);
            if (response.Code != 501 && response.Code != 502)
            {
                Column responseColumn = ServerAPI.ConvertTo<Column>(response.Body);
                BindingContext.SpaceForColumn.Children.Add(UIFactory.CreateColumn(responseColumn));
                BindingContext.PlaceToPupup.Children.Clear();
            }
        }

        static async void EditColumn_Click(IdAndNameTransport columnTransport)
        {
            Column column = new Column()
            {
                Id = columnTransport.Id,
                Name = columnTransport.Name.Text,
            };

            Response response = await ServerAPI.EditColumn(column);

            if (response.Code == 200)
            {
                BindingContext.PlaceToPupup.Children.Clear();
                await UppdateDataBoard();
            }
        }

        static void OpenPopupAddCard_Click(Column column)
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupCard(column.Id, "Add"));
        }

        static void OpenPopupEditCard_Click(Card card)
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupCard(card.Id, "Edit"));
        }

        static void OpenPopupEditColumn_Click(Column column)
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupColumn(column, "Edit"));
        }

        static void OpenPopupEditBoard_Click(Board board)
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupBoard(board, "Edit"));
        }

        static async void AddCard_Click(CardTransport cardTransport)
        {
            try
            {
                MessageBox.Show(cardTransport.Color.ToString());
                Card card = new Card()
                {
                    Id = new Guid(),
                    Title = cardTransport.Title.Text,
                    Description = cardTransport.Description.Text,
                    StoryPoint = Convert.ToInt32(cardTransport.StoryPoint.Text),
                    Date = DateTime.Parse(cardTransport.Date.Text),
                    Color = cardTransport.Color,
                    ColumnId = cardTransport.Id,
                };

                Response response = await ServerAPI.PostCard(card);
                if (response.Code != 501 && response.Code != 502)
                {
                    Card responseCard = ServerAPI.ConvertTo<Card>(response.Body);

                    StackPanel stackPanel = StackPanelRepository.GetById(responseCard.ColumnId);

                    stackPanel.Children.Add(UIFactory.CreateCard(responseCard));

                    BindingContext.PlaceToPupup.Children.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static async void EditCard_Click(CardTransport cardTransport)
        {
            Card card = new Card()
            {
                Id = cardTransport.Id,
                Title = cardTransport.Title.Text,
                Description = cardTransport.Description.Text,
                StoryPoint = Convert.ToInt32(cardTransport.StoryPoint.Text),
                Color = cardTransport.Color,
                Date = DateTime.Parse(cardTransport.Date.Text),
            };

            Response response = await ServerAPI.EditCard(card);

            if (response.Code == 200)
            {
                BindingContext.PlaceToPupup.Children.Clear();
                await UppdateDataBoard();
            }
        }

        public static Border CreateBoard(Board board)
        {
            DelegateCommand<Board> GoToKanbanBoard = new DelegateCommand<Board>(GoToKanbanBoard_Click);

            DelegateCommand<Board> OnDeleteCard = new DelegateCommand<Board>(OnDeleteBoard_Click);
            DelegateCommand<Board> OnEditCard = new DelegateCommand<Board>(OpenPopupEditBoard_Click);

            MouseBinding mouseBindingDelete = new MouseBinding();
            mouseBindingDelete.Command = OnDeleteCard;
            mouseBindingDelete.MouseAction = MouseAction.LeftClick;
            mouseBindingDelete.CommandParameter = board;

            MouseBinding mouseBindingEdit = new MouseBinding();
            mouseBindingEdit.Command = OnEditCard;
            mouseBindingEdit.MouseAction = MouseAction.LeftClick;
            mouseBindingEdit.CommandParameter = board;

            Border border2 = new Border()
            {
                Width = 5,
                Background = new SolidColorBrush(Color.FromRgb(255, 193, 121)),
                CornerRadius = new CornerRadius(2, 2, 0, 0),
            };

            Border border1 = new Border()
            {
                Width = 3.53,
                Height = 3.53,
                Background = new SolidColorBrush(Color.FromRgb(26, 72, 146)),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new TransformGroup() { Children = { new RotateTransform(45), new TranslateTransform(0, -2) } },
            };

            Grid gridEdit2 = new Grid();
            gridEdit2.RowDefinitions.Add(new RowDefinition());
            gridEdit2.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4) });
            gridEdit2.RenderTransform = new RotateTransform(30);
            gridEdit2.RenderTransformOrigin = new Point(0.5, 0.5);
            gridEdit2.Height = 20;
            gridEdit2.Children.Add(border1);
            gridEdit2.Children.Add(border2);
            Grid.SetRow(border1, 1);

            Border backgroundEdit = new Border();
            backgroundEdit.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            Grid gridEdit1 = new Grid();
            gridEdit1.Children.Add(gridEdit2);
            gridEdit1.Children.Add(backgroundEdit);
            gridEdit1.InputBindings.Add(mouseBindingEdit);

            Border boardLine1 = new Border()
            {
                Height = 4,
                Background = new SolidColorBrush(Color.FromRgb(255, 121, 121)),
                CornerRadius = new CornerRadius(2),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(-45)
            };

            Border boardLine2 = new Border()
            {
                Height = 4,
                Background = new SolidColorBrush(Color.FromRgb(255, 121, 121)),
                CornerRadius = new CornerRadius(2),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(45)
            };

            Border backgroundDelete = new Border();
            backgroundDelete.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            Grid gridDelete = new Grid();
            gridDelete.Width = 20;
            gridDelete.Height = 20;
            gridDelete.Children.Add(boardLine1);
            gridDelete.Children.Add(boardLine2);
            gridDelete.Children.Add(backgroundDelete);
            gridDelete.InputBindings.Add(mouseBindingDelete);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = board.Name;
            textBlock.FontSize = 20;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            Grid gridBoard =new Grid();
            gridBoard.ColumnDefinitions.Add(new ColumnDefinition());
            gridBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
            gridBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
            gridBoard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            gridBoard.RowDefinitions.Add(new RowDefinition());
            gridBoard.Margin = new Thickness(5);
            gridBoard.Children.Add(gridDelete);
            gridBoard.Children.Add(gridEdit1);
            gridBoard.Children.Add(textBlock);

            Grid.SetColumn(gridEdit1, 1);
            Grid.SetColumn(gridDelete, 2);
            Grid.SetColumnSpan(textBlock, 3);
            Grid.SetRowSpan(textBlock, 2);

            MouseBinding mouseBinding = new MouseBinding();
            mouseBinding.Command = GoToKanbanBoard;
            mouseBinding.MouseAction = MouseAction.LeftClick;
            mouseBinding.CommandParameter = board;

            Border border = new Border();
            border.Width = 200;
            border.Height = 120;
            border.Margin = new Thickness(10);
            border.Background = new SolidColorBrush(Color.FromRgb(0, 194, 255));
            border.InputBindings.Add(mouseBinding);
            border.Child = gridBoard;

            return border;
        }

        public static Border CreateColumn(Column column)
        {
            DelegateCommand<Column> OpenPopupAddCard = new DelegateCommand<Column>(OpenPopupAddCard_Click);
            
            DelegateCommand<Column> OnDeleteCard = new DelegateCommand<Column>(OnDeleteColumn_Click);
            DelegateCommand<Column> OnEditCard = new DelegateCommand<Column>(OpenPopupEditColumn_Click);

            MouseBinding mouseBindingDelete = new MouseBinding();
            mouseBindingDelete.Command = OnDeleteCard;
            mouseBindingDelete.MouseAction = MouseAction.LeftClick;
            mouseBindingDelete.CommandParameter = column;

            MouseBinding mouseBindingEdit = new MouseBinding();
            mouseBindingEdit.Command = OnEditCard;
            mouseBindingEdit.MouseAction = MouseAction.LeftClick;
            mouseBindingEdit.CommandParameter = column;

            Border border2 = new Border()
            {
                Width = 5,
                Background = new SolidColorBrush(Color.FromRgb(0, 155, 204)),
                CornerRadius = new CornerRadius(2, 2, 0, 0),
            };

            Border border1 = new Border()
            {
                Width = 3.53,
                Height = 3.53,
                Background = new SolidColorBrush(Color.FromRgb(26, 72, 146)),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new TransformGroup() { Children = { new RotateTransform(45), new TranslateTransform(0, -2) } },
            };

            Grid gridEdit2 = new Grid();
            gridEdit2.RowDefinitions.Add(new RowDefinition());
            gridEdit2.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4) });
            gridEdit2.RenderTransform = new RotateTransform(30);
            gridEdit2.RenderTransformOrigin = new Point(0.5, 0.5);
            gridEdit2.Height = 20;
            gridEdit2.Children.Add(border1);
            gridEdit2.Children.Add(border2);
            Grid.SetRow(border1, 1);

            Border backgroundEdit = new Border();
            backgroundEdit.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            Grid gridEdit1 = new Grid();
            gridEdit1.Children.Add(gridEdit2);
            gridEdit1.Children.Add(backgroundEdit);
            gridEdit1.InputBindings.Add(mouseBindingEdit);

            Border boardLine1 = new Border()
            {
                Height = 4,
                Background = new SolidColorBrush(Color.FromRgb(255, 121, 121)),
                CornerRadius = new CornerRadius(2),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(-45)
            };

            Border boardLine2 = new Border()
            {
                Height = 4,
                Background = new SolidColorBrush(Color.FromRgb(255, 121, 121)),
                CornerRadius = new CornerRadius(2),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(45)
            };

            Border backgroundDelete = new Border();
            backgroundDelete.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            Grid gridDelete = new Grid();
            gridDelete.Width = 20;
            gridDelete.Height = 20;
            gridDelete.Children.Add(boardLine1);
            gridDelete.Children.Add(boardLine2);
            gridDelete.Children.Add(backgroundDelete);
            gridDelete.InputBindings.Add(mouseBindingDelete);
            
            MouseBinding mouseBinding = new MouseBinding();
            mouseBinding.Command = OpenPopupAddCard;
            mouseBinding.MouseAction = MouseAction.LeftClick;
            mouseBinding.CommandParameter = column;

            TextBlock textBlockButton = new TextBlock();
            textBlockButton.Text = "Добавить карточку +";
            textBlockButton.FontSize = 16;
            textBlockButton.FontWeight = FontWeights.Bold;
            textBlockButton.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockButton.VerticalAlignment = VerticalAlignment.Center;
            textBlockButton.Foreground = new SolidColorBrush(Color.FromRgb(57, 62, 70));

            Border borderButton = new Border();
            borderButton.Padding = new Thickness(10);
            borderButton.Margin = new Thickness(10, 5, 10, 5);
            borderButton.Child = textBlockButton;
            borderButton.InputBindings.Add(mouseBinding);

            StackPanel stackPanel = new StackPanel();

            StackPanelRepository.Add(stackPanel, column.Id);

            TextBlock textBlockTitle = new TextBlock();
            textBlockTitle.Text = column.Name;
            textBlockTitle.FontSize = 18;
            textBlockTitle.FontWeight = FontWeights.Bold;
            textBlockTitle.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockTitle.VerticalAlignment = VerticalAlignment.Center;
            textBlockTitle.Foreground = new SolidColorBrush(Color.FromRgb(0, 155, 204));

            Grid gridTitle = new Grid();
            gridTitle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            gridTitle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            gridTitle.ColumnDefinitions.Add(new ColumnDefinition());
            gridTitle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            gridTitle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            gridTitle.Children.Add(textBlockTitle);
            gridTitle.Children.Add(gridEdit1);
            gridTitle.Children.Add(gridDelete);
            Grid.SetColumn(textBlockTitle, 2);
            Grid.SetColumn(gridEdit1, 3);
            Grid.SetColumn(gridDelete, 4);

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.Children.Add(gridTitle);
            grid.Children.Add(stackPanel);
            grid.Children.Add(borderButton);

            Grid.SetRow(gridTitle, 0);
            Grid.SetRow(stackPanel, 1);
            Grid.SetRow(borderButton, 2);

            Border border = new Border();
            border.Width = 250;
            border.Margin = new Thickness(10, 0, 10, 0);
            border.Child = grid;

            return border;
        }

        public static Border CreateCard(Card card)
        {
            DelegateCommand<Card> OnDeleteCard = new DelegateCommand<Card>(OnDeleteCard_Click);
            DelegateCommand<Card> OnEditCard = new DelegateCommand<Card>(OpenPopupEditCard_Click);

            MouseBinding mouseBindingDelete = new MouseBinding();
            mouseBindingDelete.Command = OnDeleteCard;
            mouseBindingDelete.MouseAction = MouseAction.LeftClick;
            mouseBindingDelete.CommandParameter = card;

            MouseBinding mouseBindingEdit = new MouseBinding();
            mouseBindingEdit.Command = OnEditCard;
            mouseBindingEdit.MouseAction = MouseAction.LeftClick;
            mouseBindingEdit.CommandParameter = card;

            Border border2 = new Border()
            {
                Width = 5,
                Background = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
                CornerRadius = new CornerRadius(2, 2, 0, 0),
            };

            Border border1 = new Border()
            {
                Width = 3.53,
                Height = 3.53,
                Background = new SolidColorBrush(Color.FromRgb(26, 72, 146)),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new TransformGroup() { Children = { new RotateTransform(45), new TranslateTransform(0, -2) } },
            };

            Grid gridEdit2 = new Grid();
            gridEdit2.RowDefinitions.Add(new RowDefinition());
            gridEdit2.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4) });
            gridEdit2.RenderTransform = new RotateTransform(30);
            gridEdit2.RenderTransformOrigin = new Point(0.5, 0.5);
            gridEdit2.Height = 20;
            gridEdit2.Children.Add(border1);
            gridEdit2.Children.Add(border2);
            Grid.SetRow(border1, 1);

            Border backgroundEdit = new Border();
            backgroundEdit.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            Grid gridEdit1 = new Grid();
            gridEdit1.Children.Add(gridEdit2);
            gridEdit1.Children.Add(backgroundEdit);
            gridEdit1.InputBindings.Add(mouseBindingEdit);

            Border boardLine1 = new Border()
            {
                Height = 4,
                Background = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
                CornerRadius = new CornerRadius(2),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(-45)
            };

            Border boardLine2 = new Border()
            {
                Height = 4,
                Background = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
                CornerRadius = new CornerRadius(2),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(45)
            };

            Border backgroundDelete = new Border();
            backgroundDelete.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            Grid gridDelete = new Grid();
            gridDelete.Children.Add(boardLine1);
            gridDelete.Children.Add(boardLine2);
            gridDelete.Children.Add(backgroundDelete);
            gridDelete.InputBindings.Add(mouseBindingDelete);

            TextBlock textBlockTitle = new TextBlock()
            {
                Text = card.Title,
                FontWeight = FontWeights.Bold,
                FontSize = 18,
                Foreground = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
                Margin = new Thickness(0, 0, 0, 7),
            };

            TextBlock textBlockDescription = new TextBlock()
            {
                Text = card.Description,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
                Margin = new Thickness(0, 0, 0, 7),
            };

            TextBlock textBlockStoryPoint = new TextBlock()
            {
                Text = "StoryPoint: " + card.StoryPoint.ToString(),
                Foreground = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
            };

            TextBlock textBlockDate = new TextBlock()
            {
                Text = card.Date.ToString(),
                Foreground = new SolidColorBrush(Color.FromRgb(57, 62, 70)),
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(textBlockTitle);
            grid.Children.Add(textBlockDescription);
            grid.Children.Add(textBlockStoryPoint);
            grid.Children.Add(textBlockDate);
            grid.Children.Add(gridDelete);
            grid.Children.Add(gridEdit1);

            Grid.SetColumnSpan(textBlockTitle, 2);
            Grid.SetRow(textBlockDescription, 1);
            Grid.SetColumnSpan(textBlockDescription, 4);
            Grid.SetRow(textBlockStoryPoint, 2);
            Grid.SetRow(textBlockDate, 2);
            Grid.SetColumn(textBlockDate, 1);
            Grid.SetColumnSpan(textBlockDate, 3);
            Grid.SetColumn(gridDelete, 3);
            Grid.SetColumn(gridEdit1, 2);

            Border border = new Border()
            {
                Padding = new Thickness(10),
                Margin = new Thickness(10, 5, 10, 5),
                Child = grid
            };

            if (card.Color == "Red")
                border.Background = Colors.Red;
            else if (card.Color == "Yellow")
                border.Background = Colors.Yellow;
            else if (card.Color == "Green")
                border.Background = Colors.Green;
            else if (card.Color == "Liteblue")
                border.Background = Colors.Liteblue;
            else if (card.Color == "Blue")
                border.Background = Colors.Blue;
            else if (card.Color == "Violet")
                border.Background = Colors.Violet;
            else if (card.Color == "Pink")
                border.Background = Colors.Pink;
            else if (card.Color == "Orange")
                border.Background = Colors.Orange;

            return border;
        }

        public static Border CreatePupupBoard(Board board, string str)
        {
            TextBox textBox = new TextBox();
            textBox.Margin = new Thickness(30, 0, 30, 30);
            textBox.FontSize = 16;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox, "Введите название доски");

            TextBlock name = new TextBlock();
            name.HorizontalAlignment = HorizontalAlignment.Center;
            name.VerticalAlignment = VerticalAlignment.Top;
            name.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            name.FontWeight = FontWeights.Bold;
            name.FontSize = 16;
            name.FontFamily = new FontFamily("Bahnschrift SemiBold");
            name.Margin = new Thickness(0, 8, 0, 0);
            name.Text = "Название доски";

            TextBlock title = new TextBlock();
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.VerticalAlignment = VerticalAlignment.Center;
            title.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            title.FontWeight = FontWeights.Bold;
            title.FontSize = 22;
            title.FontFamily = new FontFamily("Bahnschrift SemiBold");
            if (str == "Add")
            {
                title.Text = "Создание доски";
            }
            else if (str == "Edit")
            {
                title.Text = "Редактирование доски";
            }

            Button buttonAdd = new Button();
            buttonAdd.Width = 150;
            if (str == "Add")
            {
                buttonAdd.Content = "Добавить";
                buttonAdd.Command = new DelegateCommand<TextBox>(AddBoard_Click);
                buttonAdd.CommandParameter = textBox;
            }
            else if (str == "Edit")
            {
                buttonAdd.Content = "Редактировать";
                buttonAdd.Command = new DelegateCommand<IdAndNameTransport>(EditBoard_Click);
                buttonAdd.CommandParameter = new IdAndNameTransport()
                {
                    Id = board.Id,
                    Name = textBox,
                };
            }

            Button buttonClose = new Button();
            buttonClose.Width = 150;
            buttonClose.Content = "Отмена";
            buttonClose.Command = new DelegateCommand(() => { BindingContext.PlaceToPupup.Children.Clear(); });

            Grid gridButton = new Grid();
            gridButton.Margin = new Thickness(20);
            gridButton.ColumnDefinitions.Add(new ColumnDefinition());
            gridButton.ColumnDefinitions.Add(new ColumnDefinition());
            gridButton.Children.Add(buttonAdd);
            gridButton.Children.Add(buttonClose);

            Grid.SetColumn(buttonClose, 1);

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(title);
            grid.Children.Add(name);
            grid.Children.Add(textBox);
            grid.Children.Add(gridButton);

            Grid.SetColumnSpan(title, 2);
            Grid.SetRow(name, 1);
            Grid.SetRow(textBox, 1);
            Grid.SetColumn(textBox, 1);
            Grid.SetRow(gridButton, 2);
            Grid.SetColumnSpan(gridButton, 2);

            DropShadowEffect effect = new DropShadowEffect();
            effect.BlurRadius = 10;
            effect.ShadowDepth = 0;
            effect.RenderingBias = RenderingBias.Quality;
            effect.Opacity = 0.5;

            Border border = new Border();
            border.Width = 500;
            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            border.Effect = effect;
            border.Child = grid;

            return border;
        }

        public static Border CreatePupupColumn(Column column, string str)
        {
            TextBox textBox = new TextBox();
            textBox.Margin = new Thickness(30, 0, 30, 30);
            textBox.FontSize = 16;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox, "Введите название колонки");

            TextBlock name = new TextBlock();
            name.HorizontalAlignment = HorizontalAlignment.Center;
            name.VerticalAlignment = VerticalAlignment.Top;
            name.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            name.FontWeight = FontWeights.Bold;
            name.FontSize = 16;
            name.FontFamily = new FontFamily("Bahnschrift SemiBold");
            name.Margin = new Thickness(0, 8, 0, 0);
            name.Text = "Название колонки";

            TextBlock title = new TextBlock();
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.VerticalAlignment = VerticalAlignment.Center;
            title.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            title.FontWeight = FontWeights.Bold;
            title.FontSize = 22;
            title.FontFamily = new FontFamily("Bahnschrift SemiBold");
            if (str == "Add")
            {
                title.Text = "Создание колонки";
            }
            else if (str == "Edit")
            {
                title.Text = "Редактирование колонки";
            }

            Button buttonAdd = new Button();
            buttonAdd.Width = 150;
            if (str == "Add")
            {
                buttonAdd.Content = "Добавить";
                buttonAdd.Command = new DelegateCommand<TextBox>(AddColumn_Click);
                buttonAdd.CommandParameter = textBox;
            }
            else if (str == "Edit")
            {
                buttonAdd.Content = "Редактировать";
                buttonAdd.Command = new DelegateCommand<IdAndNameTransport>(EditColumn_Click);
                buttonAdd.CommandParameter = new IdAndNameTransport()
                {
                    Id = column.Id,
                    Name = textBox,
                };
            }

            Button buttonClose = new Button();
            buttonClose.Width = 150;
            buttonClose.Content = "Отмена";
            buttonClose.Command = new DelegateCommand(() => { BindingContext.PlaceToPupup.Children.Clear(); });

            Grid gridButton = new Grid();
            gridButton.Margin = new Thickness(20);
            gridButton.ColumnDefinitions.Add(new ColumnDefinition());
            gridButton.ColumnDefinitions.Add(new ColumnDefinition());
            gridButton.Children.Add(buttonAdd);
            gridButton.Children.Add(buttonClose);

            Grid.SetColumn(buttonClose, 1);

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(title);
            grid.Children.Add(name);
            grid.Children.Add(textBox);
            grid.Children.Add(gridButton);

            Grid.SetColumnSpan(title, 2);
            Grid.SetRow(name, 1);
            Grid.SetRow(textBox, 1);
            Grid.SetColumn(textBox, 1);
            Grid.SetRow(gridButton, 2);
            Grid.SetColumnSpan(gridButton, 2);

            DropShadowEffect effect = new DropShadowEffect();
            effect.BlurRadius = 10;
            effect.ShadowDepth = 0;
            effect.RenderingBias = RenderingBias.Quality;
            effect.Opacity = 0.5;

            Border border = new Border();
            border.Width = 500;
            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            border.Effect = effect;
            border.Child = grid;

            return border;
        }

        public static Border CreatePupupCard(Guid id, string str)
        {
            TextBox textBoxDate = new TextBox();
            textBoxDate.Margin = new Thickness(30, 0, 30, 30);
            textBoxDate.FontSize = 16;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxDate, "Введите дату");

            TextBlock date = new TextBlock();
            date.HorizontalAlignment = HorizontalAlignment.Center;
            date.VerticalAlignment = VerticalAlignment.Top;
            date.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            date.FontWeight = FontWeights.Bold;
            date.FontSize = 16;
            date.FontFamily = new FontFamily("Bahnschrift SemiBold");
            date.Margin = new Thickness(0, 8, 0, 0);
            date.Text = "Дата";

            TextBox textBoxStoryPoint = new TextBox();
            textBoxStoryPoint.Margin = new Thickness(30, 0, 30, 30);
            textBoxStoryPoint.FontSize = 16;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxStoryPoint, "Введите значимость");

            TextBlock storyPoint = new TextBlock();
            storyPoint.HorizontalAlignment = HorizontalAlignment.Center;
            storyPoint.VerticalAlignment = VerticalAlignment.Top;
            storyPoint.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            storyPoint.FontWeight = FontWeights.Bold;
            storyPoint.FontSize = 16;
            storyPoint.FontFamily = new FontFamily("Bahnschrift SemiBold");
            storyPoint.Margin = new Thickness(0, 8, 0, 0);
            storyPoint.Text = "StoryPoint";

            TextBox textBoxDescription = new TextBox();
            textBoxDescription.Margin = new Thickness(30, 0, 30, 30);
            textBoxDescription.FontSize = 16;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxDescription, "Введите описание");

            TextBlock description = new TextBlock();
            description.HorizontalAlignment = HorizontalAlignment.Center;
            description.VerticalAlignment = VerticalAlignment.Top;
            description.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            description.FontWeight = FontWeights.Bold;
            description.FontSize = 16;
            description.FontFamily = new FontFamily("Bahnschrift SemiBold");
            description.Margin = new Thickness(0, 8, 0, 0);
            description.Text = "Описание";

            TextBox textBoxName = new TextBox();
            textBoxName.Margin = new Thickness(30, 0, 30, 30);
            textBoxName.FontSize = 16;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxName, "Введите название карточки");

            TextBlock name = new TextBlock();
            name.HorizontalAlignment = HorizontalAlignment.Center;
            name.VerticalAlignment = VerticalAlignment.Top;
            name.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            name.FontWeight = FontWeights.Bold;
            name.FontSize = 16;
            name.FontFamily = new FontFamily("Bahnschrift SemiBold");
            name.Margin = new Thickness(0, 8, 0, 0);
            name.Text = "Название карточки";

            TextBlock title = new TextBlock();
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.VerticalAlignment = VerticalAlignment.Center;
            title.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
            title.FontWeight = FontWeights.Bold;
            title.FontSize = 22;
            title.FontFamily = new FontFamily("Bahnschrift SemiBold");
            if (str == "Add")
            {
                title.Text = "Создание карточки";
            }
            else if (str == "Edit")
            {
                title.Text = "Редактирование карточки";
            }

            Border borderColor = new Border();

            Button buttonAdd = new Button();
            buttonAdd.Width = 150;
            if (str == "Add")
            {
                buttonAdd.Content = "Добавить";
                buttonAdd.Command = new DelegateCommand<CardTransport>(AddCard_Click);
            }
            else if (str == "Edit")
            {
                buttonAdd.Content = "Редактировать";
                buttonAdd.Command = new DelegateCommand<CardTransport>(EditCard_Click);
            }
            buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Blue"
            };

            Button buttonClose = new Button();
            buttonClose.Width = 150;
            buttonClose.Content = "Отмена";
            buttonClose.Command = new DelegateCommand(() => { BindingContext.PlaceToPupup.Children.Clear(); });

            Grid gridButton = new Grid();
            gridButton.Margin = new Thickness(20);
            gridButton.ColumnDefinitions.Add(new ColumnDefinition());
            gridButton.ColumnDefinitions.Add(new ColumnDefinition());
            gridButton.Children.Add(buttonAdd);
            gridButton.Children.Add(buttonClose);

            Grid.SetColumn(buttonClose, 1);

            Button buttonRed = new Button();
            buttonRed.Margin = new Thickness(2);
            buttonRed.Background = Colors.Red;
            buttonRed.Click += (s, e) => { borderColor.Background = Colors.Red; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Red"
            };
            };

            Button buttonYellow = new Button();
            buttonYellow.Margin = new Thickness(2);
            buttonYellow.Background = Colors.Yellow;
            buttonYellow.Click += (s, e) => { borderColor.Background = Colors.Yellow; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Yellow"
            };
            };
            
            Button buttonGreen = new Button();
            buttonGreen.Margin = new Thickness(2);
            buttonGreen.Background = Colors.Green;
            buttonGreen.Click += (s, e) => { borderColor.Background = Colors.Green; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Green"
            };
            };

            Button buttonLiteblue = new Button();
            buttonLiteblue.Margin = new Thickness(2);
            buttonLiteblue.Background = Colors.Liteblue;
            buttonLiteblue.Click += (s, e) => { borderColor.Background = Colors.Liteblue; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Liteblue"
            };
            };

            Button buttonBlue = new Button();
            buttonBlue.Margin = new Thickness(2);
            buttonBlue.Background = Colors.Blue;
            buttonBlue.Click += (s, e) => { borderColor.Background = Colors.Blue; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Blue"
            };
            };

            Button buttonViolet = new Button();
            buttonViolet.Margin = new Thickness(2);
            buttonViolet.Background = Colors.Violet;
            buttonViolet.Click += (s, e) => { borderColor.Background = Colors.Violet; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Violet"
            };
            };

            Button buttonPink = new Button();
            buttonPink.Margin = new Thickness(2);
            buttonPink.Background = Colors.Pink;
            buttonPink.Click += (s, e) => { borderColor.Background = Colors.Pink; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Pink"
            };
            };

            Button buttonOrange = new Button();
            buttonOrange.Margin = new Thickness(2);
            buttonOrange.Background = Colors.Orange;
            buttonOrange.Click += (s, e) => { borderColor.Background = Colors.Orange; buttonAdd.CommandParameter = new CardTransport
            {
                Title = textBoxName,
                Description = textBoxDescription,
                StoryPoint = textBoxStoryPoint,
                Date = textBoxDate,
                Id = id,
                Color = "Orange"
            };
            };

            StackPanel stackPanel = new StackPanel();
            stackPanel.Height = 40;
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            stackPanel.Children.Add(buttonRed);
            stackPanel.Children.Add(buttonYellow);
            stackPanel.Children.Add(buttonGreen);
            stackPanel.Children.Add(buttonLiteblue);
            stackPanel.Children.Add(buttonBlue);
            stackPanel.Children.Add(buttonViolet);
            stackPanel.Children.Add(buttonPink);
            stackPanel.Children.Add(buttonOrange);

            borderColor.Background = Colors.Blue;
            borderColor.Height = 120;
            borderColor.Margin = new Thickness(0, 0, 0, 20);
            borderColor.Child = stackPanel;

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(title);
            grid.Children.Add(borderColor);
            grid.Children.Add(name);
            grid.Children.Add(textBoxName);
            grid.Children.Add(description);
            grid.Children.Add(textBoxDescription);
            grid.Children.Add(storyPoint);
            grid.Children.Add(textBoxStoryPoint);
            grid.Children.Add(date);
            grid.Children.Add(textBoxDate);
            grid.Children.Add(gridButton);

            Grid.SetColumnSpan(title, 2);
            Grid.SetRow(borderColor, 1);
            Grid.SetColumnSpan(borderColor, 2);
            Grid.SetRow(name, 2);
            Grid.SetRow(textBoxName, 2);
            Grid.SetColumn(textBoxName, 1);
            Grid.SetRow(description, 3);
            Grid.SetRow(textBoxDescription, 3);
            Grid.SetColumn(textBoxDescription, 1);
            Grid.SetRow(storyPoint, 4);
            Grid.SetRow(textBoxStoryPoint, 4);
            Grid.SetColumn(textBoxStoryPoint, 1);
            Grid.SetRow(date, 5);
            Grid.SetRow(textBoxDate, 5);
            Grid.SetColumn(textBoxDate, 1);
            Grid.SetRow(gridButton, 6);
            Grid.SetColumnSpan(gridButton, 2);

            DropShadowEffect effect = new DropShadowEffect();
            effect.BlurRadius = 10;
            effect.ShadowDepth = 0;
            effect.RenderingBias = RenderingBias.Quality;
            effect.Opacity = 0.5;

            Border border = new Border();
            border.Width = 500;
            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            border.Effect = effect;
            border.Child = grid;

            return border;
        }

        public static async Task UppdateDataBoard()
        {
            Response response = await ServerAPI.GetBoardNameById(Context.IdTargetBoard);

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

        public static async Task UppdateDataBoards()
        {
            Response response = await ServerAPI.GetBoards();
            List<Board> boards = ServerAPI.ConvertTo<List<Board>>(response.Body);

            BindingContext.PersonalBoards.Children.Clear();

            foreach (var board in boards)
            {
                BindingContext.PersonalBoards.Children.Add(UIFactory.CreateBoard(board));
            }
        }
    }

    public class CardTransport
    {
        public TextBox Title { get; set; }

        public TextBox Description { get; set; }

        public TextBox StoryPoint { get; set; }

        public TextBox Date { get; set; }

        public Guid Id { get; set; }

        public string Color { get; set; }
    }

    public class IdAndNameTransport
    {
        public TextBox Name { get; set; }

        public Guid Id { get; set; }
    }
}
