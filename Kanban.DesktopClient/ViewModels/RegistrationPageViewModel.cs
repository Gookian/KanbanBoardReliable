using Prism.Commands;

namespace Kanban.DesktopClient.ViewModels
{
    public class RegistrationPageViewModel
    {
        public string Login { get; set; } = "";

        public string Password { get; set; } = "";

        public string RePassword { get; set; } = "";

        public DelegateCommand SignIn { get; set; }

        public DelegateCommand SignUp { get; set; }

        public RegistrationPageViewModel()
        {
            SignIn = new DelegateCommand(SignIn_Click);
            SignUp = new DelegateCommand(SignUp_Click);
        }

        private void SignIn_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.AuthorizationPage;
        }

        private void SignUp_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.AuthorizationPage;
        }
    }
}
