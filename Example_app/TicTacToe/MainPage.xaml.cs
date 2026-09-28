using TicTacToe.Game;
using TicTacToe.Misc;

namespace TicTacToe
{
    public partial class MainPage : ContentPage
    {

        VerticalStackLayout vsl;
        ScrollView sv;
        Label title;
        Image image;
        public GameStatistics statistics = new GameStatistics();
        public List<ContentPage> lehed;
        public List<string> Lehenimed = new List<string>() { "Mängima", "Mängu info", "Statistika"  };
        public MainPage()
        {
            lehed = new List<ContentPage>()
            {
                new TicTacToeGame(statistics),
                new InfoPage(),
                new StatisticsPage(statistics)
            };

            title = new Label
            { 
                Text = "TRIPS-TRAPS-TRULL",
                TextColor = Colors.Goldenrod,
                FontSize = 60,
                FontFamily = "Bunpop",
                HeightRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            vsl = new VerticalStackLayout
            {
                BackgroundColor = Color.FromRgb(40, 28, 2),
                Padding = 20,
                Spacing = 20,
                Children =
                {
                    title
                },
            };
            for (int i = 0; i < lehed.Count; i++)
            {
                Button nupp = new Button
                {
                    Text = Lehenimed[i],
                    FontSize = 30,
                    FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                    BackgroundColor = Colors.DarkGoldenrod,
                    TextColor = Colors.Black,
                    CornerRadius = 10,
                    HeightRequest = 60,
                    ZIndex = i
                };
                vsl.Add(nupp);
                nupp.Clicked += (sender, e) =>
                {
                    var valik = lehed[nupp.ZIndex];
                    Navigation.PushAsync(valik);
                };
            }                    

            sv = new ScrollView { Content = vsl };
            Content = sv;
        }
        
    }
}
