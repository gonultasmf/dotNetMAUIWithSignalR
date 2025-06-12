using System;
using dotNetWebApi.Server.Dtos;
using dotNetWebApi.Server.Functions.User;

namespace dotNetWebApi.Server.Endpoints;

public static class AuthenticateEndpoints
{
    public static void MapAuthenticateEndpoints(this WebApplication app)
    {
        app.MapPost("/Authenticate/authenticate", async (IUserFunction service, AuthenticateRequest request) =>
        {
            var response = service.Authenticate(request.LoginId, request.Password);
            if (response == null)
                return Results.BadRequest(new { StatusMessage = "Invalid username or password!" });
            return Results.Ok(response);
        });
    }
}
