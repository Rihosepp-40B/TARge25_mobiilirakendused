namespace Example_app;

public partial class ValgusFoorPage : ContentPage
{
	private bool onSisselulitatud = false;

    public ValgusFoorPage()
	{
		InitializeComponent();
	}

	// Nupp Sisse
	private void OnSisseClicked(object sender, EventArgs e)
	{
		onSisselulitatud = true;

		// Aktiveerime värvid
		PunaneBox.Color = Colors.Red;
        KollaneBox.Color = Colors.Yellow;
        RohelineBox.Color = Colors.Green;

        PuhastaTekstid();
        InfoLabel.Text = "Vali valgus";
	}

	// Nupp "Välja"
	private void OnValjaClicked(object sender, EventArgs e)
	{
		onSisselulitatud = false;

        // Muudame värvid halliks
        PunaneBox.Color = Colors.Gray;
        KollaneBox.Color = Colors.Gray;
        RohelineBox.Color = Colors.Gray;

        PuhastaTekstid();
        InfoLabel.Text = "Vali valgus";
	}

	// Tulede vajutamine
	private void OnTuliTapped(object sender, TappedEventArgs e)
	{
		// Kui foor on väljas, hoiatame kasutajat
		if (!onSisselulitatud)
		{
            InfoLabel.Text = "Lülita esmalt foor sisse";
            return;
        }

		// Tühjendame eelmised tekstid kuju sees
        PuhastaTekstid();

        // Kontrollime, millisele tulele vajutati
        string värv = e.Parameter?.ToString();

        switch (värv)
        {
            case "Punane":
                PunaneLabel.Text = "Seisa";
                PunaneBox.Color = Colors.Red;
                KollaneBox.Color = Colors.Gray;
                RohelineBox.Color = Colors.Gray;
                break;
            case "Kollane":
                KollaneLabel.Text = "Valmista";
                PunaneBox.Color = Colors.Gray;
                KollaneBox.Color = Colors.Yellow;
                RohelineBox.Color = Colors.Gray;
                break;
            case "Roheline":
                RohelineLabel.Text = "Sõida";
                PunaneBox.Color = Colors.Gray;
                KollaneBox.Color = Colors.Gray;
                RohelineBox.Color = Colors.Green;
                break;
        }
    }

    private void OnPaevaReziim(object sender, EventArgs e)
    { }


    private void PuhastaTekstid()
    {
        PunaneLabel.Text = "";
        KollaneLabel.Text = "";
        RohelineLabel.Text = "";
    }
}