namespace TicTacToe.Misc
{
    public class InfoPage : ContentPage
    {
        public InfoPage()
        {
            Title = "Mängu info";

            Content = new VerticalStackLayout
            {
                Children =
                {
                    new Label
                    {
                        Padding = 20,
                        Text = "Trips-Traps-Trull mäng kahe osapoole vahel, kus eesmärk on saada esimesena kolm enda märki ritta. Rida võib olla horisontaalne, vertikaalne või diagonaalne.\n\n" +
                        "" +
                        "Mängimiseks on ruutudest koosnev mängu väli. Igat ruutu saab mängu korral kasutada ainult korra.\n\n" +
                        "" +
                        "Mängimiseks enda korra ajal vajuta valitud ruutu ja sinu märgiga tähistatakse see ära.",
                        FontSize = 20,
                        TextColor = Colors.DarkGoldenrod

                    }
                }
            };
        }
    }
}
