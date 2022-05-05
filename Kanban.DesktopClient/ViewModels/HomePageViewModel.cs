using Prism.Commands;
using System;

namespace Kanban.DesktopClient.ViewModels
{
    public class HomePageViewModel
    {
        public string Login { get; set; }

        public DelegateCommand Home { get; set; }

        public DelegateCommand Boards { get; set; }

        public DelegateCommand Reference { get; set; }

        public DelegateCommand Exit { get; set; }

        public HomePageViewModel()
        {
            Home = new DelegateCommand(Home_Click);
            Boards = new DelegateCommand(Boards_Click);
            Reference = new DelegateCommand(Reference_Click);
            Exit = new DelegateCommand(Exit_Click);
        }

        private void Home_Click()
        {
            throw new NotImplementedException();
        }

        private void Boards_Click()
        {
            BindingContext.HomeFrame.Child = BindingContext.BoardPage;
        }

        private void Reference_Click()
        {
            throw new NotImplementedException();
        }

        private void Exit_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.AuthorizationPage;
        }
    }
}
