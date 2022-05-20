using Kanban.DesktopClient.Models;
using Prism.Commands;


namespace Kanban.DesktopClient.ViewModels
{
    public class KanbanPageViewModel
    {
        public DelegateCommand CreateColumn { get; set; }
        public DelegateCommand PreviousPage { get; set; }

        public KanbanPageViewModel()
        {
            CreateColumn = new DelegateCommand(CreateColumn_Click);
            PreviousPage = new DelegateCommand(PreviousPage_Click);
        }

        public async void CreateColumn_Click()
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupAddColumn());
        }

        public void PreviousPage_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.HomePage;
            BindingContext.HomeFrame.Child = BindingContext.BoardPage;
        }
    }
}
