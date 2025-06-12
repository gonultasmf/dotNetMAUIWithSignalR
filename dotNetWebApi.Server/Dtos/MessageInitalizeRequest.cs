using System;

namespace dotNetWebApi.Server.Dtos;

public class MessageInitalizeRequest
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
}
