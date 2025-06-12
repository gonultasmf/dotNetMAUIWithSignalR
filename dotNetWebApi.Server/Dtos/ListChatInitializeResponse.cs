using System;
using dotNetWebApi.Server.Functions.Message;
using dotNetWebApi.Server.Functions.User;

namespace dotNetWebApi.Server.Dtos;

public class ListChatInitializeResponse
{
    public User User { get; set; } = null!;
    public IEnumerable<User> UserFriends { get; set; } = null!;
    public IEnumerable<LastestMessage> LastestMessages { get; set; } = null!;
}
