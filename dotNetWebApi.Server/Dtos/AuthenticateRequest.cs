using System;

namespace dotNetWebApi.Server.Dtos;

public class AuthenticateRequest
{
    public string LoginId { get; set; } = null!;
    public string Password { get; set; } = null!;
}
