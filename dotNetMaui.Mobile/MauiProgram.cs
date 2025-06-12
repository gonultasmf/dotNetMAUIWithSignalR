using dotNetMaui.Mobile.Pages;
using dotNetMaui.Mobile.Services.ChatHub;
using dotNetMaui.Mobile.ViewModels;
using Microsoft.Extensions.Logging;

namespace dotNetMaui.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "IconFontTypes");
            });

        builder.Logging.AddDebug();

        builder.Services
            .AddSingleton<App>()
            .AddSingleton<AppShell>()
            .AddScoped<MainPage>();

        builder.Services.AddSingleton<ChatHub>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<ListChatPage>();
        builder.Services.AddSingleton<ChatPage>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<ListChatPageViewModel>();
		builder.Services.AddSingleton<ChatPageViewModel>();
		builder.Services.AddSingleton<ServiceProvider>();

        return builder.Build();
    }
}