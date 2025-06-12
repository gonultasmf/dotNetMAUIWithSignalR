using System;
using dotNetWebApi.Server.Functions.User;

namespace dotNetWebApi.Server.Dtos;

public class MessageInitalizeResponse
{
    public User FriendInfo { get; set; } = null!;
    public IEnumerable<Functions.Message.Message> Messages { get; set; } = null!;
}
