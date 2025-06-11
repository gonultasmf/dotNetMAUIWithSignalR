using Microsoft.AspNetCore.SignalR.Client;

namespace dotNetMaui.Mobile;

public partial class MainPage : ContentPage
{
    private readonly HubConnection _connection;
    private Entry myChatMessage;
    private Label chatMessages = new Label()
        .FontSize(18)
        .CenterHorizontal();

    public MainPage()
    {
        Build();
        
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5072/chat")
            .Build();

        _connection.On<string>("MessageReceived",
            (message) =>
            {
                chatMessages.Text += $"{Environment.NewLine}{message}";
            });

        Task.Run(() =>
        {
            Dispatcher.Dispatch(async () =>
                await _connection.StartAsync());
        });
    }

    public void Build()
    {
        this
            .Content(
                new ScrollView()
                    .Content(
                        new VerticalStackLayout()
                            .Padding(30,0)
                            .Spacing(25)
                            .Children(
                                
                                chatMessages,
                                new Entry()
                                    .Assign(out myChatMessage)
                                    .Placeholder("Type your message here")
                                    .FontSize(18)
                                    .CenterHorizontal(),
                                new Button()
                                    .Text("SEND")
                                    .CenterHorizontal()
                                    .OnClicked(OnCounterClicked)
                                    .SemanticHint("Counts the number of times you click")
                            )
                    )
            );
    }


    private async void OnCounterClicked(object? sender, EventArgs e)
    {
        await _connection.InvokeCoreAsync("SendMessage", args: new[] { myChatMessage.Text });

        myChatMessage.Text = String.Empty;
    }
}