using Prism.Commands;

namespace Kanban.DesktopClient.ViewModels
{
    public class BoardPageViewModel
    {
        public DelegateCommand Board { get; set; }

        public BoardPageViewModel()
        {
            Board = new DelegateCommand(Border_Click);
        }

        private void Border_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.KanbanPage;
        }
    }
}
