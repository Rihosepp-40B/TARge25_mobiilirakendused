namespace Example_app;

public partial class TextPage : ContentPage
{
	Label lbl;
	Editor editor;
	HorizontalStackLayout hsl;
	List<string> nupud = new List<string>() { "Tagasi", "Avaleht", "Edasi" };
	VerticalStackLayout vsl;
	public TextPage()
	{
		lbl = new Label
		{
			Text = "Pealkiri",
			FontSize = 36,
			FontFamily = "luffio",
			TextColor = Colors.Black,
			HorizontalOptions = LayoutOptions.Center,
			FontAttributes = FontAttributes.Bold
		};
		editor = new Editor
		{
			Placeholder = "Sisesta tekst...",
			PlaceholderColor = Colors.Red,
			FontSize = 18,
			FontAttributes = FontAttributes.Italic,
			HorizontalOptions = LayoutOptions.Center,
		};
		editor.TextChanged += (sender, e) =>
		{
			lbl.Text = editor.Text;
		};

		Button speechButton = new Button
		{
			Text = "Loe Ette",
			FontSize = 22,
            FontFamily = "luffio",
            BackgroundColor = Colors.LightGray,
			TextColor = Colors.BlueViolet,
			CornerRadius = 10
		};

		speechButton.Clicked += Btn_Clicked;

		hsl = new HorizontalStackLayout { Spacing = 20, HorizontalOptions = LayoutOptions.Center };
		for (int j = 0; j < nupud.Count; j++)
		{
			Button nupp = new Button
			{
				Text = nupud[j],
				FontSize = 28,
				FontFamily = "luffio",
				TextColor = Colors.BlueViolet,
				BackgroundColor = Colors.LightGray,
				CornerRadius = 10,
				HeightRequest = 50,
				ZIndex = j
			};
			hsl.Add(nupp);
			nupp.Clicked += Liikumine;
		}
		vsl = new VerticalStackLayout
		{
			Padding = 20,
			Spacing = 15,
			Children = { lbl, editor, speechButton, hsl },
			HorizontalOptions = LayoutOptions.Center
		};
		Content = vsl;
	}
	private void Liikumine(object? sender, EventArgs e)
	{
		Button nupp = sender as Button;
		if(nupp.ZIndex == 0)
		{
			Navigation.PopAsync();
		}
		else if (nupp.ZIndex == 1)
		{
			Navigation.PopToRootAsync();
		}
		else if (nupp.ZIndex == 2)
		{
			Navigation.PushAsync(new FigurePage());
		}
	}
    private async void Btn_Clicked(object? sender, EventArgs e)
    {
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

        SpeechOptions options = new SpeechOptions()
        {
            Pitch = 1.5f, // 0.0 - 2.0
            Volume = 0.75f, // 0.0 - 1.0
            Locale = locales.FirstOrDefault()
        };
        string? text = editor.Text;
		if (string.IsNullOrWhiteSpace(text))
		{
			await DisplayAlert("Viga", "Palun sisesta tekst", "Ok");
			return;
		}
		try
		{
			await TextToSpeech.SpeakAsync(text, options);
		}
		catch (Exception ex)
		{
			await DisplayAlert("TTS viga", ex.Message, "OK");
		}

    }
}