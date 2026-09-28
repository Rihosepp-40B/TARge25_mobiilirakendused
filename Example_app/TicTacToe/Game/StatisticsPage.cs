using TicTacToe.Game;

public class StatisticsPage : ContentPage
{
    private GameStatistics statistics;

    private Label totalGamesLabel;
    private Label player1WinsLabel;
    private Label player2WinsLabel;
    private Label tiesLabel;

    public StatisticsPage(GameStatistics statistics)
    {
        this.statistics = statistics;

        Title = "Statistika";

        totalGamesLabel = new Label();
        player1WinsLabel = new Label();
        player2WinsLabel = new Label();
        tiesLabel = new Label();

        VerticalStackLayout layout = new VerticalStackLayout
        {
            Padding = 30,
            Spacing = 20,

            Children =
            {
                new Label
                {
                    Text = "Mängude statistika",
                    FontSize = 30
                },

                totalGamesLabel,
                player1WinsLabel,
                player2WinsLabel,
                tiesLabel
            }
        };

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        totalGamesLabel.Text = $"Mänge kokku: {statistics.TotalGames}";
        player1WinsLabel.Text = $"Mängija 1 võidud: {statistics.Player1Wins}";
        player2WinsLabel.Text = $"Mängija 2 võidud: {statistics.Player2Wins}";
        tiesLabel.Text = $"Viigid: {statistics.Ties}";
    }
}