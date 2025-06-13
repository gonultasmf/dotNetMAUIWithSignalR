using dotNetMaui.Mobile.Pages;

namespace dotNetMaui.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        this
            .FlyoutBehavior(FlyoutBehavior.Disabled)
            .ShellTabBarIsVisible(false)
            .ShellNavBarIsVisible(false)
            .Items(
                new ShellContent()
                    .Title("LoginPage")
                    .ContentTemplate(() => new LoginPage())
                    .Route(nameof(LoginPage))
            )
            .CurrentItem(Application.Current?.Handler?.MauiContext?.Services?.GetService<LoginPage>());

        Routing.RegisterRoute("ListChatPage", typeof(ListChatPage));
        Routing.RegisterRoute("ChatPage", typeof(ChatPage));
    }
}