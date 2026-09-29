
namespace TicTacToe.Game
{
    public class TicTacToeGame : ContentPage
    {
        // Klassi muutujad
        Grid mainGrid, grid3x3;

        private Image[,] gameImages = new Image[3, 3];

        // true = Mängija 1 kord; false = Mängija 2 kord
        private bool playerTurn = true;
        private bool gameOver = false;

        private Player player1;
        private Player player2;

        private bool setupShown = false;

        private Image player1Image, player2Image;

        private int startPlayer = 0;

        private Button whoStartsButton;

        Label player1Label, player2Label;

        private GameStatistics statistics;

        private bool botGame = false;

        public TicTacToeGame(GameStatistics statistics)
        {
            this.statistics = statistics;

            Title = "Mäng";


            // peamine grid, mille sisse lähevad muud elemendid (mängija pilt, mänguväli)
            mainGrid = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };

            grid3x3 = GameField();
            mainGrid.Add(grid3x3, 0, 1);
            mainGrid.SetColumnSpan(grid3x3, 2);

            player1Image = new Image
            {
                WidthRequest = 150,
                HeightRequest = 150,
                Aspect = Aspect.AspectFit
            };

            player2Image = new Image
            {
                WidthRequest = 150,
                HeightRequest = 150,
                Aspect = Aspect.AspectFit
            };

            mainGrid.Add(player1Image, 0, 0);
            mainGrid.Add(player2Image, 1, 2);

            Button newGameButton = new Button
            {
                Text = "Uus mäng",
                WidthRequest = 150,
                HeightRequest = 25,
                BackgroundColor = Colors.Red,
            };

            newGameButton.Clicked += async (sender, e) =>
            {
                await NewGameButton_Clicked(sender, e);
            };

            whoStartsButton = new Button
            {
                Text = "Kes alustab?",
                WidthRequest = 150,
                HeightRequest = 25,
            };

            whoStartsButton.Clicked += WhoStartsButton_Clicked;

            mainGrid.Add(newGameButton, 0, 3);
            mainGrid.Add(whoStartsButton, 1, 3);

            player1Label = new Label();
            player2Label = new Label();

            mainGrid.Add(player1Label, 1, 0);
            mainGrid.Add(player2Label, 0, 2);


            Content = mainGrid;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (setupShown)
            {
                return;
            }
                        
            await ShowPlayerSetup();

            setupShown = true;
        }

        private async Task ShowPlayerSetup()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            PlayerSetupPage setupPage = new PlayerSetupPage(
                (p1, p2, isBotGame) =>
                {
                    player1 = p1;
                    player2 = p2;

                    botGame = isBotGame;

                    player1Image.Source = player1.Image;
                    player2Image.Source = player2.Image;

                    Player1LabelInfo();

                    // Anname märku, et valik on tehtud
                    tcs.SetResult(true);
                });

            await Navigation.PushModalAsync(setupPage);

            await tcs.Task;
        }

        private Grid GameField()
        {
            // Mänguväli
            grid3x3 = new Grid
            {
                WidthRequest = 300,
                HeightRequest = 300
            };

            for (int i = 0; i < 3; i++)
            {
                grid3x3.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                grid3x3.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    Border border = new Border
                    {
                        Stroke = Colors.DarkGoldenrod,
                        StrokeThickness = 2
                    };

                    // Algab tühja pildiga
                    Image image = new Image
                    {
                        Aspect = Aspect.AspectFit
                    };

                    // Salvestame pildi loetellu
                    gameImages[row, column] = image;

                    //pilt piiride sisse
                    border.Content = image;

                    TapGestureRecognizer tap = new TapGestureRecognizer();

                    // Salvestame rea ja veeru muutujad eraldi muutujasse
                    int r = row;
                    int c = column;

                    tap.Tapped += (sender, e) =>
                    {
                        MakeMove(r, c);
                    };

                    border.GestureRecognizers.Add(tap);

                    // Lisame piirid grid'i
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, column);

                    grid3x3.Children.Add(border);
                }
            }
            return grid3x3;
        }

        private async Task NewGameButton_Clicked(object sender, EventArgs e)
        {
            bool changePlayers = await DisplayAlert(
                "Uus mäng",
                "Kas soovid mängijate andmeid muuta?",
                "Jah",
                "Ei");

            if (changePlayers)
            {
                // Avame mängijate seadistamise
                await ShowPlayerSetup();
            }

            // Alustame uut mängu
            ResetGame();
        }

        private void ResetGame()
        {
            // Puhastame kõik 9 mänguruutu
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    gameImages[row, column].Source = null;
                }
            }

            // Mäng ei ole enam läbi
            gameOver = false;

            // Määrab alustava mängiva vastavalt startPlayer andmetele
            if (startPlayer == 1)
            {
                playerTurn = true;
            }
            else if (startPlayer == 2)
            {
                playerTurn = false;
            }
            else
            {
                Random random = new Random();
                playerTurn = random.Next(0, 2) == 0;
            }

            Player1LabelInfo();

            whoStartsButton.IsEnabled = true;

            if (botGame && !playerTurn)
            {
                BotMove();
            }
        }

        private async void WhoStartsButton_Clicked(object sender, EventArgs e)
        {
            string choice = await DisplayActionSheet(
                "Kes alustab?",
                "Loobu",
                null,
                "Mängija 1",
                "Mängija 2",
                "Mäng otsustab");

            if (choice == "Mängija 1")
            {
                startPlayer = 1;
                playerTurn = true;
            }
            else if (choice == "Mängija 2")
            {
                startPlayer = 2;
                playerTurn = false;
            }
            else if (choice == "Mäng otsustab")
            {
                startPlayer = 0;

                Random random = new Random();
                playerTurn = random.Next(0, 2) == 0;
            }
            else
            {
                return;
            }
            Player1LabelInfo();

            if (botGame && !playerTurn && !gameOver)
            {
                BotMove();
            }
        }

        private void MakeMove(int row, int column)
            //Võimaldab mängija käikude tegemist
        {
            if (player1 == null || player2 == null)
            {
                DisplayAlert("Viga", "Mängijad ei ole määratud!", "OK");
                return;
            }

            // Takistab funktsiooni edasi liikumist juhul kui mäng on läbi
            if (gameOver)
            {
                return;
            }

            // Kui ruut on juba kasutatud, siis ei tee midagi
            if (gameImages[row, column].Source != null)
            {
                return;
            }

            // Vastava mängija korra ajal sisestab mängija pildi mänguruudule.
            PlayerTurns(row, column);
            whoStartsButton.IsEnabled = false;

            // Kontrollib, kas võitja on selgunud, kui on siis loeb mängu lõppenuks
            if (Winner())
            {
                gameOver = true;
                return;
            }

            // Kontrollib, kas mäng on viigis, kui on siis loeb mängu lõppenuks
            if (GameIsTie())
            {
                statistics.GameTied();
                EndMessage();
                return;
            }

            // Mängija vahetamine
            playerTurn = !playerTurn;

            Player1LabelInfo();

            if (botGame && !playerTurn)
            {
                BotMove();
            }
        }

        private void BotMove()
        {
            Random random = new Random();

            List<(int row, int column)> freeCells = new List<(int, int)>();

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (gameImages[row, column].Source == null)
                    {
                        freeCells.Add((row, column));
                    }
                }
            }

            if (freeCells.Count == 0)
            {
                return;
            }

            var move = freeCells[random.Next(freeCells.Count)];

            MakeMove(move.row, move.column);
        }

        private void PlayerTurns(int row, int column)
        {
            // Mängija 1 kord
            if (playerTurn)
            {
                gameImages[row, column].Source = player1.Image;
                
            }
            // Mängija 2 kord
            else
            {
                gameImages[row, column].Source = player2.Image;
            }
        }

        private void Player1LabelInfo()
        {
            if (playerTurn)
            {
                player1Label.Text = "Mängija: " + player1.Name + " käik!";
                player2Label.Text = "Mängija: " + player2.Name;
            }
            else
            {
                player2Label.Text = "Mängija: " + player2.Name + " käik!";
                player1Label.Text = "Mängija: " + player1.Name;
            }

        }

        private bool Winner()
        {
            if (!CheckWinner())
            {
                return false;
            }

            string winnerName = "";
            string message = " võitis!";

            if (playerTurn)
            {
                winnerName = player1.Name;

                statistics.Player1Won();
            }
            else
            {
                winnerName = player2.Name;

                statistics.Player2Won();
            }

            EndMessage(winnerName, message);

            return true;
        }

        private async void EndMessage(string winnerName = "", string message = "Viik!")
        {
            bool playAgain = await DisplayAlert("Mäng läbi", winnerName + message + " Kas soovid veel mängida?", "Jah", "Ei");

            if (playAgain)
            {
                await NewGameButton_Clicked(null, EventArgs.Empty);
            }
        }

        private bool CheckWinner()
            //Tagasta kas võidu tingimused on täidetud
        {
             return CheckRows() || CheckColumns() || CheckDiagonals();
        }

        private bool CheckRows()
            //Tagasta kas ühes reas on kolm sama sümbolit
        {
            for (int row = 0; row < 3; row++)
            {
                if (IsSame(row, 0, row, 1, row, 2))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckColumns()
            // Tagasta, kas ühes veerus on kolm sama sümbolit
        {
            for (int column = 0; column < 3; column++)
            {
                if (IsSame(0, column, 1, column, 2, column))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckDiagonals()
            // Tagasta, kas diagonaalis on kolm sama sümbolit
        {
            return IsSame(0, 0, 1, 1, 2, 2) ||
                   IsSame(0, 2, 1, 1, 2, 0);
        }

        private bool IsSame( int row1,int column1, int row2, int column2, int row3, int column3)
            //Tagasta kas kolm kontrollitud pilti on samad
        {
            Image first = gameImages[row1, column1];
            Image second = gameImages[row2, column2];
            Image third = gameImages[row3, column3];

            if (first.Source == null || second.Source == null || third.Source == null)
            {
                return false;
            }

            return first.Source.ToString() == gameImages[row2, column2].Source.ToString() &&
                first.Source.ToString() == gameImages[row3, column3].Source.ToString();
        }

        private bool GameIsTie()
            // Tagasta kas kõik mänguväljad on täidetud.
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (gameImages[row, column].Source == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
