
namespace TicTacToe.Game
{
    public class PlayerSetupPage : ContentPage
    {
        private Entry player1Name;
        private Entry player2Name;

        private string player1Image;
        private string player2Image;

        public Player Player1 {  get; private set; }
        public Player Player2 {  get; private set; }

        private readonly Action<Player, Player, bool> playersReady;

        private List<Border> imageButtonsP1 = new List<Border>();
        private List<Border> imageButtonsP2 = new List<Border>();

        private Dictionary<Border, string> imageNames = new Dictionary<Border, string>();

        private Picker gameModePicker;

        public PlayerSetupPage(Action<Player, Player, bool> playersReady)
        {
            this.playersReady = playersReady;
            
            Title = "Mängijad";

            // Vaikimisi pildid
            player1Image = "x.png";
            player2Image = "o.png";

            player1Name = new Entry
            {
                Placeholder = "Mängija 1"
            };

            player2Name = new Entry
            {
                Placeholder = "Mängija 2"
            };

            // Mängija 1 piltide valik
            ScrollView imagesP1 = CreateImageSelection(
                imageButtonsP1,
                imageName =>
                {
                    if (imageName == player2Image)
                    {
                        DisplayAlert("Pilt juba kasutusel", "Mängija 2 kasutab seda pilti.", "OK");
                        return;
                    }
                    player1Image = imageName;
                    UpdateSelectedImage(imageButtonsP1, imageName);
                });

            // Mängija 2 piltide valik
            ScrollView imagesP2 = CreateImageSelection(
                imageButtonsP2,
                imageName =>
                {
                    if (imageName == player1Image)
                    {
                        DisplayAlert("Pilt juba kasutusel", "Mängija 1 kasutab seda pilti.", "OK");
                        return;
                    }
                    player2Image = imageName;
                    UpdateSelectedImage(imageButtonsP2, imageName);
                });

            UpdateSelectedImage(imageButtonsP1, player1Image);
            UpdateSelectedImage(imageButtonsP2, player2Image);

            gameModePicker = new Picker
            {
                Title = "Mängurežiim",
                TextColor = Colors.Red
            };

            gameModePicker.Items.Add("Mängija vs mängija");
            gameModePicker.Items.Add("Mängija vs bot");
            gameModePicker.SelectedIndex = 0;

            Button startButton = new Button
            {
                Text = "Alusta mängu"
            };

            startButton.Clicked += async (sender, e) =>
            {
                string nameP1 = player1Name.Text;

                string nameP2 = player2Name.Text;

                // Kui nimi on tühi, kasutame vaikimisi nime
                if (string.IsNullOrWhiteSpace(nameP1))
                {
                    nameP1 = "Mängija 1";
                }

                if (string.IsNullOrWhiteSpace(nameP2))
                {
                    nameP2 = "Mängija 2";
                }

                // Loome mängijad
                Player1 = new Player
                {
                    Name = nameP1,
                    Image = player1Image
                };

                Player2 = new Player
                {
                    Name = nameP2,
                    Image = player2Image
                };

                // Kontrollime, kas valitud režiim on "Mängija vs bot" (indeks 1)
                bool isBotGame = gameModePicker.SelectedIndex == 1;

                playersReady(Player1, Player2, isBotGame);

                // Sulgeme popupi
                await Navigation.PopModalAsync();
            };

            // Kogu popupi sisu
            VerticalStackLayout vsl = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 15,

                Children =
                {
                    new Label
                    {
                        Text = "Mängija 1",
                        FontSize = 24
                    },

                    player1Name,

                    new Label
                    {
                        Text = "Vali mängija 1 pilt",
                        FontSize = 18
                    },

                    imagesP1,

                    gameModePicker,

                    new Label
                    {
                        Text = "Mängija 2",
                        FontSize = 24
                    },

                    player2Name,

                    new Label
                    {
                        Text = "Vali mängija 2 pilt",
                        FontSize = 18
                    },

                    imagesP2,

                    startButton
                }
            };

            Content = new ScrollView
            {
                Content = vsl
            };
        }

        private ScrollView CreateImageSelection(
            List<Border> buttons,
            Action<string> imageSelected)
        {
            HorizontalStackLayout layout = new HorizontalStackLayout
            {
                Spacing = 10
            };

            string[] playerImages =
            {
                "x.png",
                "o.png",
                "dotnet_bot.png",
                "images.jpg",
                "images2.jpg",
                "images3.jpg"
            };

            foreach (string imageName in playerImages)
            {
                Image image = new Image
                {
                    Source = imageName,
                    WidthRequest = 65,
                    HeightRequest = 65,
                    Aspect = Aspect.AspectFit
                };

                Border border = new Border
                {
                    Content = image,
                    Stroke = Colors.Transparent,
                    StrokeThickness = 3,
                    BackgroundColor = Colors.Black,
                    Padding = 5,
                    WidthRequest = 75,
                    HeightRequest = 75
                };

                TapGestureRecognizer tap = new TapGestureRecognizer();

                tap.Tapped += (sender, e) =>
                {
                    imageSelected(imageName);
                };

                border.GestureRecognizers.Add(tap);

                buttons.Add(border);
                imageNames.Add(border, imageName);
                layout.Children.Add(border);
            }

            return new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                Content = layout
            };
        }

        private void UpdateSelectedImage(
            List<Border> buttons,
            string selectedImage)
        {
            foreach (Border border in buttons)
            {
                if (imageNames[border] == selectedImage)
                {
                    // See pilt on valitud
                    border.BackgroundColor = Colors.LightBlue;
                    border.Stroke = Colors.Blue;
                    border.StrokeThickness = 5;
                }
                else
                {
                    // See pilt ei ole valitud
                    border.BackgroundColor = Colors.Black;
                    border.Stroke = Colors.Transparent;
                    border.StrokeThickness = 0;
                }
            }
        }
    }
}
