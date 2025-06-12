using dotNetMaui.Mobile.Converters;
using dotNetMaui.Mobile.Helpers;
using dotNetMaui.Mobile.ViewModels;
using FmgLib.MauiMarkup;
using Microsoft.Maui.Controls.Shapes;

namespace dotNetMaui.Mobile.Pages;

public class ChatPage : ContentPage
{
    public ChatPage()
    {
        Build();
    }

    public void Build()
    {
        this
        .Title("ChatPage")
        .OnNavigatedTo(ContentPage_NavigatedTo)
        .Background("#13232f".ToColor())
        .BindingContext(Application.Current?.Handler?.MauiContext?.Services?.GetService<ChatPageViewModel>())
        .Content(
            new RefreshView()
            .Margin(10)
            .IsRefreshing(e => e.Getter(static (ChatPageViewModel x) => x.IsRefreshing))
            .Content(
                new Grid()
                .RowDefinitions(e => e.Absolute(40).Star().Absolute(40))
                .Children(
                    new HorizontalStackLayout()
                    .Children(
                        new Image()
                        .Source(e => e.Getter(static (ChatPageViewModel x) => x.FriendInfo.AvatarSourceName))
                        .SizeRequest(32)
                        .CenterVertical(),

                        new Label()
                        .Text(e => e.Getter(static (ChatPageViewModel x) => x.FriendInfo.UserName))
                        .Margin(10,0,0,0)
                        .FontAttributes(Bold)
                        .FontSize(20)
                        .CenterVertical()
                    ),

                    new HorizontalStackLayout()
                    .AlignBottom()
                    .Children(
                        new Button()
                        .BackgroundColor("#152c39".ToColor())
                        .CornerRadius(45)
                        .SizeRequest(32)
                        .ImageSource(
                            new FontImageSource()
                            .FontFamily("IconFontTypes")
                            .Glyph(IconFontHelper.Camera_alt)
                            .Size(18)
                        ),

                        new Button()
                        .Margin(10,0,0,0)
                        .BackgroundColor("#152c39".ToColor())
                        .CornerRadius(45)
                        .SizeRequest(32)
                        .ImageSource(
                            new FontImageSource()
                            .FontFamily("IconFontTypes")
                            .Glyph(IconFontHelper.Edit)
                            .Size(18)
                        )
                    )
                ),

                new CollectionView()
                .Row(1)
                .Margin(0,10,0,10)
                .ItemsSource(e => e.Getter(static (ChatPageViewModel viewModel) => viewModel.Messages))
                .ItemsLayout(new LinearItemsLayout(ItemsLayoutOrientation.Vertical).ItemSpacing(10))
                .ItemTemplate(() =>
                    new VerticalStackLayout()
                    .Assign(out var vLayout)
                    .HorizontalOptions(e => e.Bindings(
                            new Binding().Path("FromUserId"),
                            new Binding().Path("ToUserId").Source(BindingContext as ChatPageViewModel)
                        )
                        .Converter(new FromUserIdToBackgroudColorConverter())
                    )
                    .Children(
                        new Label()
                        .Margin(5,0,5,0)
                        .FontSize(11)
                        .HorizontalOptions(e => e.Path("HorizontalOptions").Source(vLayout))
                        .Text(e => e.Getter(static (Models.Message model) => model.SendDateTimeStr)),

                        new Border()
                        .Padding(10,6,10,6)
                        .StrokeShape(new RoundRectangle().CornerRadius(10))
                        .BackgroundColor(e => e.Bindings(
                                new Binding().Path("FromUserId"),
                                new Binding().Path("ToUserId").Source(BindingContext as ChatPageViewModel)
                            )
                            .Converter(new FromUserIdToBackgroudColorConverter())
                        )
                        .Content(
                            new Label()
                            .Text(e => e.Getter(static (Models.Message model) => model.Content))
                            .FontSize(12)
                            .TextColor(Black)
                        )
                    )
                ),

                new Grid()
                .Row(2)
                .ColumnDefinitions(e => e.Star().Absolute(40))
                .Children(
                    new Border()
                    .Padding(10,0,10,0)
                    .StrokeShape(new RoundRectangle().CornerRadius(20))
                    .HeightRequest(36)
                    .Stroke(Transparent)
                    .Content(
                        new Entry()
                        .Placeholder("Aa")
                        .Text(e => e.Getter(static (ChatPageViewModel model) => model.Message))
                        .TextColor(Black)
                        .CenterVertical()
                    ),

                    new Button()
                    .Column(1)
                    .BackgroundColor("#152c39".ToColor())
                    .Command(e => e.Getter(static (ChatPageViewModel model) => model.SendMessageCommand))
                    .CornerRadius(45)
                    .SizeRequest(32)
                    .ImageSource(
                        new FontImageSource()
                        .FontFamily("IconFontTypes")
                        .Glyph(IconFontHelper.Send)
                        .Size(18)
                    )
                )
            )
        );
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
	{
		(this.BindingContext as ChatPageViewModel)!.Initialize();
	}
}