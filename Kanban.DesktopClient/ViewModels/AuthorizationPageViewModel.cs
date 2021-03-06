using Core;
using Kanban.DesktopClient.RestAPI;
using Kanban.DesktopClient.Views;
using Prism.Commands;

namespace Kanban.DesktopClient.ViewModels
{
    public class AuthorizationPageViewModel
    {
        public string Login { get; set; } = "";

        public string Password { get; set; } = "";

        public DelegateCommand SignIn { get; set; }

        public DelegateCommand SignUp { get; set; }

        public AuthorizationPageViewModel()
        {
            SignIn = new DelegateCommand(SignIn_Click);
            SignUp = new DelegateCommand(SignUp_Click);
        }

        private async void SignIn_Click()
        {
            var user = new User
            {
                Id = new System.Guid(),
                Name = Login,
                Password = Password
            };

            var response = await ServerAPI.GetAuthentication(user);

            if (response.Code == 200)
            {
                BindingContext.MainFrame.Child = BindingContext.HomePage;
            }
            else if (response.Code == 500)
            {
                ErrorWindow window = new ErrorWindow($"Пользователь не найден: {response.Code}", $"Пользователь не существует, проверьте вверенные данные!");
                window.Show();
            }
        }

        private void SignUp_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.RegistrationPage;
        }
    }
}
