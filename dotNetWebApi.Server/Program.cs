using dotNetWebApi.Server;
using dotNetWebApi.Server.Endpoints;
using dotNetWebApi.Server.Entities;
using dotNetWebApi.Server.Functions.Message;
using dotNetWebApi.Server.Functions.User;
using dotNetWebApi.Server.Functions.UserFriend;
using dotNetWebApi.Server.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ChatAppContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IUserFunction, UserFunction>();
builder.Services.AddTransient<IUserFriendFunction, UserFriendFunction>();
builder.Services.AddTransient<IMessageFunction, MessageFunction>();
builder.Services.AddScoped<UserOperator>();
builder.Services.AddScoped<ChatHub>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

app.MapHub<ChatHub>("/chat");
app.MapAuthenticateEndpoints();
app.MapMessageEndpoints();
app.MapListChatEndpoints();

app.Run();
