using Core;
using Kanban.DesktopClient.RestAPI;
using Kanban.DesktopClient.Views;
using Prism.Commands;
using System;

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

        private async void SignUp_Click()
        {
            var user = new User
            {
                Id = new Guid(),
                Name = Login,
                Password = Password,
                Token = new Token { Id = new Guid(), Lifetime = new DateTime() },
            };

            var response = await ServerAPI.PostUser(user);

            if (response.Code == 200)
            {
                BindingContext.MainFrame.Child = BindingContext.AuthorizationPage;
            }
            else if(response.Code == 503)
            {
                ErrorWindow window = new ErrorWindow($"Имя уже занято: {response.Code}", $"{response.Header} уже существует, попробуйте изменить имя!");
                window.Show();
            }
        }
    }
}
