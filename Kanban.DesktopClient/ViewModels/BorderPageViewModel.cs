using Prism.Commands;

namespace Kanban.DesktopClient.ViewModels
{
    public class BorderPageViewModel
    {
        public DelegateCommand Board { get; set; }

        public BorderPageViewModel()
        {
            Board = new DelegateCommand(Border_Click);
        }

        private void Border_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.KanbanPage;
        }
    }
}
