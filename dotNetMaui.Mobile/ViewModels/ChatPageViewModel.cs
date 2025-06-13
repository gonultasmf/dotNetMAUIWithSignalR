using dotNetMaui.Mobile.Models;
using dotNetMaui.Mobile.Services.ChatHub;
using dotNetMaui.Mobile.Services.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;

namespace dotNetMaui.Mobile.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
            ToUserId = int.Parse(HttpUtility.UrlDecode(query["toUserId"].ToString()));
            
        }

        private Services.ServiceProvider _serviceProvider;
        private HttpClient _httpClient;
        private ChatHub _chatHub;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChatPageViewModel(Services.ServiceProvider serviceProvider, ChatHub chatHub)
        {
            Messages = new ObservableCollection<Models.Message>();
            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.AddReceivedMessageHandler(OnReceiveMessage);
            _chatHub.Connect();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5072");
            SendMessageCommand = new Command(async () =>
            {
                try
                {
                    if (Message.Trim() != "")
                    {
                        await _chatHub.SendMessageToUser(FromUserId, ToUserId, Message);

                        Messages.Add(new Models.Message
                        {
                            Content = Message,
                            FromUserId = FromUserId,
                            ToUserId = ToUserId,
                            SendDateTime = DateTime.Now
                        });

                        Message = "";
                    }
                }
                catch (Exception ex)
                {
                    await AppShell.Current.DisplayAlert("ChatApp", ex.Message, "OK");
                }
            });
        }

        async Task GetMessages()
        {
            var request = new MessageInitializeRequest
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
            };

            var response = await _httpClient.PostAsJsonAsync("/Message/Initialize", request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseModel = await response.Content.ReadFromJsonAsync<MessageInitializeReponse>();
                if (responseModel == null) return;
            
                FriendInfo = responseModel.FriendInfo;
                Messages = new ObservableCollection<Models.Message>(responseModel.Messages);
            }
            else
            {
                await AppShell.Current.DisplayAlert("ChatApp", response.StatusCode.ToString(), "OK");
            }
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetMessages();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }

        private void OnReceiveMessage (int fromUserId, string message)
        {
            Messages.Add(new Models.Message
            {
                Content = message,
                FromUserId = ToUserId,
                ToUserId = FromUserId,
                SendDateTime = DateTime.Now
            });
        }

        private int fromUserId;
        private int toUserId;
        private User friendInfo;
        private ObservableCollection<Models.Message> messages;
        private bool isRefreshing;
        private string message;

        public int FromUserId
        {
            get { return fromUserId; }
            set { fromUserId = value; OnPropertyChanged(); }
        }

        public int ToUserId
        {
            get { return toUserId; }
            set { toUserId = value; OnPropertyChanged(); }
        }

        public User FriendInfo
        {
            get { return friendInfo; }
            set { friendInfo = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Models.Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand { get; set; }
    }
}
