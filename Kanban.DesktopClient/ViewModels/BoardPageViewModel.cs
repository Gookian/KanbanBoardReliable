using Core;
using Kanban.DesktopClient.Models;
using Prism.Commands;

namespace Kanban.DesktopClient.ViewModels
{
    public class BoardPageViewModel
    {
        public DelegateCommand Board { get; set; }

        public DelegateCommand CreateBoard { get; set; }

        public BoardPageViewModel()
        {
            Board = new DelegateCommand(Border_Click);
            CreateBoard = new DelegateCommand(CreateBoard_Click);
        }

        private void Border_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.KanbanPage;
        }

        private async void CreateBoard_Click()
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupBoard(new Board(),"Add"));
        }
    }
}
