using dotNetMaui.Mobile.ViewModels;
using FmgLib.MauiMarkup;
using Microsoft.Maui.Controls.Shapes;

namespace dotNetMaui.Mobile.Pages;

public class LoginPage : ContentPage
{
    public LoginPage()
    {
        Build();
    }

    public void Build()
    {
        this
        .Title("LoginPage")
        .Background("#13232f".ToColor())
        .BindingContext(Application.Current?.Handler?.MauiContext?.Services?.GetService<LoginPageViewModel>())
        .Content(
            new Grid()
            .Children(
                new VerticalStackLayout()
                .Margin(30)
                .CenterVertical()
                .Children(
                    new Label()
                    .FontSize(45)
                    .Text("ChatApp")
                    .CenterHorizontal(),

                    new Label()
                    .FontSize(20)
                    .Text("With SignalR")
                    .CenterHorizontal(),

                    new Border()
                    .Margin(0, 45, 0, 0)
                    .Padding(10, 0, 10, 0)
                    .Stroke(Transparent)
                    .StrokeThickness(2)
                    .StrokeShape(new RoundRectangle().CornerRadius(16))
                    .Content(
                        new Entry()
                        .Placeholder("UserName")
                        .TextColor(Black)
                        .ReturnType(ReturnType.Go)
                        .Text(e => e.Getter(static (LoginPageViewModel x) => x.UserName))
                    ),

                    new Border()
                    .Margin(0, 10, 0, 0)
                    .Padding(10, 0, 10, 0)
                    .Stroke(Transparent)
                    .StrokeThickness(2)
                    .StrokeShape(new RoundRectangle().CornerRadius(16))
                    .Content(
                        new Entry()
                        .Placeholder("Password")
                        .IsPassword(true)
                        .TextColor(Black)
                        .ReturnType(ReturnType.Go)
                        .Text(e => e.Getter(static (LoginPageViewModel x) => x.Password))
                    ),

                    new Label()
                    .Text("Forgot Password?")
                    .TextColor("#1e90ff".ToColor())
                    .Margin(0, 10, 0, 0),

                    new Button()
                    .Margin(0, 30, 0, 0)
                    .Text("Login")
                    .TextColor(White)
                    .BackgroundColor("#f0932b".ToColor())
                    .Command(e => e.Getter(static (LoginPageViewModel x) => x.LoginCommand)),

                    new HorizontalStackLayout()
                    .Margin(0, 50, 0, 0)
                    .CenterHorizontal()
                    .Children(
                        new Line()
                        .SizeRequest(50, 0.5)
                        .BackgroundColor(Gray),

                        new Label()
                        .Text("Or continue with")
                        .TextColor("#dfe6e9".ToColor())
                        .Margin(10, 0, 10, 0),

                        new Line()
                        .SizeRequest(50, 0.5)
                        .BackgroundColor(Gray)
                    ),

                    new HorizontalStackLayout()
                    .Margin(0, 30, 0, 0)
                    .CenterHorizontal()
                    .Children(
                        new Border()
                        .Padding(15,10,15,10)
                        .Stroke(Transparent)
                        .StrokeThickness(2)
                        .StrokeShape(new RoundRectangle().CornerRadius(16))
                        .Content(
                            new Image()
                            .Source("google_logo.png")
                            .SizeRequest(32)
                        ),

                        new Border()
                        .Padding(15, 10, 15, 10)
                        .Margin(20,0,0,0)
                        .Stroke(Transparent)
                        .StrokeThickness(2)
                        .StrokeShape(new RoundRectangle().CornerRadius(16))
                        .Content(
                            new Image()
                            .Source("apple_logo.png")
                            .SizeRequest(32)
                        )
                    ),

                    new HorizontalStackLayout()
                    .Margin(0, 40, 0, 0)
                    .CenterHorizontal()
                    .Children(
                        new Label()
                        .Text("Don't have an account?"),

                        new Label()
                        .Text("Sign Up")
                        .TextColor("#1e90ff".ToColor())
                    )
                ),

                new ActivityIndicator()
                .SizeRequest(60)
                .IsRunning(e => e.Getter(static (LoginPageViewModel x) => x.IsProcessing))
            )
        );
    }
}