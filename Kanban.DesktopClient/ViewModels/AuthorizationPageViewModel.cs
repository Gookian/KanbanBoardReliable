using Core;
using Kanban.DesktopClient.RestAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prism.Commands;
using System.Collections.Generic;

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
            var response = await ServerAPI.GetUsers();

            List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Body.ToString());

            foreach (var user in users)
            {
                if (user.Name == Login && user.Password == Password)
                {
                    BindingContext.MainFrame.Child = BindingContext.HomePage;
                }
            }
        }

        private void SignUp_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.RegistrationPage;
        }
    }
}
