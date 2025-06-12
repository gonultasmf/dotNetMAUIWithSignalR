using System;
using dotNetWebApi.Server.Dtos;
using dotNetWebApi.Server.Functions.Message;
using dotNetWebApi.Server.Functions.User;
using dotNetWebApi.Server.Functions.UserFriend;

namespace dotNetWebApi.Server.Endpoints;

public static class ListChatEndpoints
{
    public static void MapListChatEndpoints(this WebApplication app)
    {
        app.MapPost("/listChat/initialize", async (IUserFunction userFunction, IUserFriendFunction friendFunction, IMessageFunction messageFunction, int userId) =>
        {
            var response = new ListChatInitializeResponse
            {
                User = userFunction.GetUserById(userId),
                UserFriends = await friendFunction.GetListUserFriend(userId),
                LastestMessages = await messageFunction.GetLastestMessage(userId)
            };
            return Results.Ok(response);
        });
    }
}
