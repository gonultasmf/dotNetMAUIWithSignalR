using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using dotNetMaui.Mobile.Services.Authenticate;

namespace dotNetMaui.Mobile.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Services.ServiceProvider _serviceProvider;
        private HttpClient _httpClient;

        public LoginPageViewModel(Services.ServiceProvider serviceProvider)
        {
            UserName = "wanda";
            Password = "Abc12345";
            IsProcessing = false;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5072");

            LoginCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (UserName.Trim() == "" || Password.Trim() == "") return;

                IsProcessing = true;
                Login().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
            this._serviceProvider = serviceProvider;
        }

        async Task Login()
        {
            try
            {
                var request = new AuthenticateRequest
                {
                    LoginId = UserName,
                    Password = Password,
                };
                var response = await _httpClient.PostAsJsonAsync("/Authenticate/Authenticate", request);
                // var response = await _serviceProvider.Authenticate(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseModel = await response.Content.ReadFromJsonAsync<AuthenticateResponse>();
                    await Shell.Current.GoToAsync($"ListChatPage?userId={responseModel.Id}");
                }
                else
                {
                    await AppShell.Current.DisplayAlert("ChatApp", "Something went wrong", "OK");
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("ChatApp", ex.Message, "OK");
            }
        }

        private string userName;
        private string password;
        private bool isProcessing;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; set; }
    }
}
