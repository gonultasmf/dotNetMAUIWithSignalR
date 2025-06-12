using System.Collections.ObjectModel;
using dotNetMaui.Mobile.Helpers;
using dotNetMaui.Mobile.ViewModels;
using FmgLib.MauiMarkup;
using Microsoft.Maui.Controls.Shapes;

namespace dotNetMaui.Mobile.Pages;

public class ListChatPage : ContentPage
{
    public ListChatPage()
    {
        Build();
    }

    public void Build()
    {
        this
        .Title("Chats")
        .BindingContext(Application.Current?.Handler?.MauiContext?.Services?.GetService<ListChatPageViewModel>())
        .OnNavigatedTo(ContentPage_NavigatedTo)
        .BackgroundColor("#13232f".ToColor())
        .IconImageSource(
            new FontImageSource()
            .FontFamily("IconFontTypes")
            .Glyph(IconFontHelper.Messenger)
            .Size(20)
        )
        .Content(
            new RefreshView()
            .Margin(10)
            .IsRefreshing(e => e.Getter(static (ListChatPageViewModel x) => x.IsRefreshing))
            .Content(
                new Grid()
                .RowSpacing(10)
                .RowDefinitions(e => e.Absolute(40).Absolute(45).Absolute(90))
                .Children(
                    new Grid()
                    .Children(
                        new HorizontalStackLayout()
                        .Children(
                            new Image()
                            .SizeRequest(32)
                            .Source(e => e.Getter(static (ListChatPageViewModel x) => x.UserInfo.AvatarSourceName)),

                            new Label()
                            .Margin(10, 0, 0, 0)
                            .Text(e => e.Getter(static (ListChatPageViewModel x) => x.UserInfo.UserName))
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

                    new Grid()
                    .Row(1)
                    .Children(
                        new Border()
                        .Stroke(Transparent)
                        .StrokeThickness(2)
                        .StrokeShape(new RoundRectangle().CornerRadius(45))
                        .Padding(0)
                        .BackgroundColor("#152c39".ToColor())
                        .Content(
                            new HorizontalStackLayout()
                            .HeightRequest(48)
                            .CenterVertical()
                            .Children(
                                new Image()
                                .Margin(20, 0, 0, 0)
                                .CenterVertical()
                                .BackgroundColor(Transparent)
                                .Source(
                                    new FontImageSource()
                                    .FontFamily("IconFontTypes")
                                    .Glyph(IconFontHelper.Search)
                                    .Size(18)
                                    .Color(Gray)
                                ),

                                new Label()
                                .Text("Search")
                                .Margin(10,0,0,0)
                                .TextColor(Gray)
                                .CenterVertical()
                            )
                        )
                    ),

                    new Grid()
                    .Row(2)
                    .ColumnDefinitions(e => e.Absolute(72).Star())
                    .Children(
                        new StackLayout()
                        .HeightRequest(72)
                        .Children(
                            new Button()
                            .Margin(10,0,0,0)
                            .BackgroundColor("#152c39".ToColor())
                            .CornerRadius(45)
                            .SizeRequest(54)
                            .ImageSource(
                                new FontImageSource()
                                .FontFamily("IconFontTypes")
                                .Glyph(IconFontHelper.Video_call)
                                .Size(24)
                            ),

                            new Label()
                            .Text("Create video call")
                            .FontSize(12)
                            .CenterHorizontal()
                            .TextCenterHorizontal()
                        ),

                        new CollectionView()
                        .Margin(10, 0, 0, 0)
                        .Column(1)
                        .ItemsSource(e => e.Getter(static (ListChatPageViewModel x) => x.UserFriends))
                        .ItemsLayout(new LinearItemsLayout(ItemsLayoutOrientation.Horizontal).ItemSpacing(10))
                        .ItemTemplate(() =>
                            new VerticalStackLayout()
                            .Children(
                                new Grid()
                                .SizeRequest(58, 54)
                                .Children(
                                    new Image()
                                    .SizeRequest(54)
                                    .Source(e => e.Getter(static (Models.User x) => x.AvatarSourceName))
                                    .Aspect(Aspect.AspectFill)
                                    .AlignTop()
                                    .GestureRecognizers(
                                        new TapGestureRecognizer()
                                        .Command(e => e.Getter(static (ListChatPageViewModel x) => x.OpenChatPageCommand))
                                        .CommandParameter(e => e.Getter(static (Models.User x) => x.Id))
                                    ),

                                    new Ellipse()
                                    .Fill(Green)
                                    .Stroke("#152c39".ToColor())
                                    .StrokeThickness(3)
                                    .SizeRequest(18)
                                    .AlignBottomLeft()
                                    .IsVisible(e => e.Getter(static (Models.User x) => x.IsOnline)),

                                    new Border()
                                    .BackgroundColor(SkyBlue)
                                    .Stroke("#152c39".ToColor())
                                    .StrokeThickness(2)
                                    .StrokeShape(new RoundRectangle().CornerRadius(10))
                                    .HeightRequest(12)
                                    .AlignBottomLeft()
                                    .IsVisible(e => e.Getter(static (Models.User x) => x.IsAway))
                                    .Content(
                                        new Label()
                                        .Text(e => e.Getter(static (Models.User x) => x.AwayDuration))
                                        .TextColor(Black)
                                        .FontSize(10)
                                        .Center()
                                    )
                                ),

                                new Label()
                                .Margin(0, 5, 0, 0)
                                .FontSize(12)
                                .CenterHorizontal()
                                .TextCenterHorizontal()
                                .Text(e => e.Getter(static (Models.User x) => x.UserName))
                            )
                        )
                    ),

                    new ListView()
                    .Row(3)
                    .RowHeight(70)
                    .ItemsSource(e => e.Getter(static (ListChatPageViewModel x) => x.LastestMessages))
                    .SeparatorVisibility(SeparatorVisibility.None)
                    .VerticalScrollBarVisibility(Never)
                    .ItemTemplate(() => 
                        new ViewCell()
                        .View(
                            new HorizontalStackLayout()
                            .GestureRecognizers(
                                new TapGestureRecognizer()
                                .Command(e => e.Getter(static (ListChatPageViewModel x) => x.OpenChatPageCommand))
                                .CommandParameter(e => e.Getter(static (Models.LastestMessage x) => x.UserFriendInfo.Id))
                            )
                            .Children(
                                new Grid()
                                .SizeRequest(58, 54)
                                .Children(
                                    new Image()
                                    .SizeRequest(54)
                                    .Source(e => e.Getter(static (Models.LastestMessage x) => x.UserFriendInfo.AvatarSourceName))
                                    .Aspect(Aspect.AspectFill)
                                    .AlignTop(),

                                    new Ellipse()
                                    .Fill(Green)
                                    .Stroke("#152c39".ToColor())
                                    .StrokeThickness(3)
                                    .SizeRequest(18)
                                    .AlignBottomLeft()
                                    .IsVisible(e => e.Getter(static (Models.LastestMessage x) => x.UserFriendInfo.IsOnline)),

                                    new Border()
                                    .BackgroundColor(SkyBlue)
                                    .Stroke("#152c39".ToColor())
                                    .StrokeThickness(2)
                                    .StrokeShape(new RoundRectangle().CornerRadius(10))
                                    .HeightRequest(12)
                                    .AlignBottomLeft()
                                    .IsVisible(e => e.Getter(static (Models.LastestMessage x) => x.UserFriendInfo.IsAway))
                                    .Content(
                                        new Label()
                                        .Text(e => e.Getter(static (Models.LastestMessage x) => x.UserFriendInfo.AwayDuration))
                                        .TextColor(Black)
                                        .FontSize(10)
                                        .Center()
                                    )
                                ),

                                new StackLayout()
                                .Margin(10,5,0,0)
                                .AlignTop()
                                .Children(
                                    new Label()
                                    .Text(e => e.Getter(static (Models.LastestMessage x) => x.UserFriendInfo.UserName))
                                    .FontSize(17)
                                    .AlignTop()
                                    .TextTop(),

                                    new Label()
                                    .Text(e => e.Getter(static (Models.LastestMessage x) => x.Content))
                                    .FontSize(12)
                                    .Center()
                                    .TextTop()
                                )
                            )
                        )
                    )
                )
            )
        );
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
	{
		(this.BindingContext as ListChatPageViewModel)!.Initialize();
	}
}