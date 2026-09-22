namespace Example_app;

public partial class StartPage : ContentPage
{
	VerticalStackLayout vsl;
	ScrollView sv;
	public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(), new FigurePage(), new Timer_Page(), new ValgusFoorPage(), new DateTimePage(), new SteppSliderPage(), new RgbPage(), new TreePage(), new PopUpPage(), new GridPage() };
	public List<string> Lehenimed = new List<string>() { "Tekstid", "Kujundus", "Taimer", "Valgusfoor", "Datetime", "SteppSlider", "RGB", "Tree", "PopUp", "Grid" };
	public StartPage()
	{
		vsl = new VerticalStackLayout { Padding = 20, Spacing = 20 };
		for (int i=0;i<lehed.Count; i++)
		{
			Button nupp = new Button
			{
				Text = Lehenimed[i],
				FontSize = 36,
				FontFamily = "luffio",
				BackgroundColor = Colors.LightGray,
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

        // punane test nupp
        Button nulliNupp = new Button
        {
            Text = "Nulli seaded (Testimiseks)",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            CornerRadius = 10,
            HeightRequest = 50,
            Margin = new Thickness(0, 30, 0, 0) // Jätame veidi tühja ruumi üles
        };

        // Mis juhtub nupule vajutades?
        nulliNupp.Clicked += async (sender, e) =>
        {
            // Kustutame seadme mälust meie spetsiifilise võtme
            Preferences.Default.Remove("EsimeneKäivitamine");

            // Anname tagasisidet, et nullimine õnnestus
            await DisplayAlertAsync("Edukalt nullitud", "Mälu on tühjendatud. Kui sa lehe uuesti avad, käitub äpp nagu täiesti uus!", "OK");
        };

        vsl.Add(nulliNupp);

        sv = new ScrollView { Content = vsl };
		Content = sv;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 1. Loeme seadme mälust muutuja "EsimeneKäivitamine".
        // Kui sellist muutuja pole (äpp on uus), annab see vaikimisi väärtuseks 'true'.
        bool onEsimeneStart = Preferences.Default.Get("EsimeneKäivitamine", true);

        // 2. Kui on esimene start, kuvame dialoogiakna
        if (onEsimeneStart)
        {
            bool vastus = await DisplayAlertAsync("Tere tulemast!",
                                    "Tundub, et avasid selle rakenduse esimest korda. Kas soovid näha lühikest juhendit?",
                                    "Jah, palun",
                                    "Ei, saan ise hakkama");

            if (vastus)
            {
                await DisplayAlertAsync("Juhend",
                    "Siin on sinu lühike juhend: vali menüüst sobiv teema ja uuri, kuidas elemendid töötavad!",
                    "Selge");
            }

            // 3. Salvestame info, et esimene käivitamine on tehtud.
            Preferences.Default.Set("EsimeneKäivitamine", false);
        }
    }
}