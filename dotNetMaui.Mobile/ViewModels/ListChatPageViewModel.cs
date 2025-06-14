﻿using System;
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
using dotNetMaui.Mobile.Models;
using dotNetMaui.Mobile.Services.ChatHub;
using dotNetMaui.Mobile.Services.ListChat;

namespace dotNetMaui.Mobile.ViewModels
{
    public class ListChatPageViewModel: INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Services.ServiceProvider _serviceProvider;
        private ChatHub _chatHub;

        private HttpClient _httpClient;

        public ListChatPageViewModel(Services.ServiceProvider serviceProvider, ChatHub chatHub)
        {
            UserInfo = new User();
            UserFriends = new ObservableCollection<User>();
            LastestMessages = new ObservableCollection<LastestMessage>();
            _httpClient = new HttpClient();

            RefreshCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    IsRefreshing = true;
                    await GetListFriends();
                }).GetAwaiter().OnCompleted(() =>
                {
                    IsRefreshing = false;
                });
            });

            OpenChatPageCommand = new Command<int>(async (param) =>
            {
                await Shell.Current.GoToAsync($"ChatPage?fromUserId={UserInfo.Id}&toUserId={param}");
            });

            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.Connect();
            _chatHub.AddReceivedMessageHandler(OnReceivedMessage);

            MessagingCenter.Send<string>("StartService", "MessageForegroundService");
            MessagingCenter.Send<string, string[]>("StartService", "MessageNotificationService", new string[] { });

        }

        private User userInfo;
        private ObservableCollection<User> userFriends;
        private ObservableCollection<LastestMessage> lastestMessages;
        private bool isRefreshing;

        async Task GetListFriends()
        {
            var response = await _httpClient.PostAsJsonAsync("/ListChat/Initialize", new { UserId = UserInfo.Id });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ListChatInitializeResponse>();
                UserInfo = result?.User ?? new User();
                UserFriends = new ObservableCollection<User>(result?.UserFriends ?? Enumerable.Empty<User>());
                LastestMessages = new ObservableCollection<LastestMessage>(result?.LastestMessages ?? Enumerable.Empty<LastestMessage>());
            }
            else
            {
                await AppShell.Current.DisplayAlert("ChatApp", "Failed to load friends list.", "OK");
            }
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetListFriends();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
        }

        void OnReceivedMessage(int fromUserId, string message)
        {
            var lastestMessage = LastestMessages.Where(x => x.UserFriendInfo.Id == fromUserId).FirstOrDefault();
            if (lastestMessage != null)
                LastestMessages.Remove(lastestMessage);

            var newLastestMessage = new LastestMessage
            {
                UserId = userInfo.Id,
                Content = message,
                UserFriendInfo = UserFriends.Where(x=>x.Id == fromUserId).FirstOrDefault()
            };

            LastestMessages.Insert(0, newLastestMessage);
            OnPropertyChanged("LastestMessages");

            MessagingCenter.Send<string, string[]>("Notify", "MessageNotificationService", 
                new string[] {newLastestMessage.UserFriendInfo.UserName, newLastestMessage.Content});
        }

        public User UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; OnPropertyChanged(); }
        }
        public ObservableCollection<User> UserFriends
        {
            get { return userFriends; }
            set { userFriends = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LastestMessage> LastestMessages
        {
            get { return lastestMessages; }
            set { lastestMessages = value; OnPropertyChanged(); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; set; }        

        public ICommand OpenChatPageCommand { get; set; }
    }
}
