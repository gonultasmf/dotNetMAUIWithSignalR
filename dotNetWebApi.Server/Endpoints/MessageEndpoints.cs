using System;
using dotNetWebApi.Server.Dtos;
using dotNetWebApi.Server.Functions.Message;
using dotNetWebApi.Server.Functions.User;

namespace dotNetWebApi.Server.Endpoints;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        app.MapPost("/message/initialize", async (IMessageFunction messageFunction, IUserFunction userFunction, MessageInitalizeRequest request) =>
        {
            var response = new MessageInitalizeResponse
            {
                FriendInfo = userFunction.GetUserById(request.ToUserId),
                Messages =await messageFunction.GetMessages(request.FromUserId, request.ToUserId)
            };

            return Results.Ok(response);
        });
    }
}
