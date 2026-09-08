namespace Example_app;

public partial class ValgusFoorPage : ContentPage
{
	private bool onSisselulitatud = false;
    private bool paevSisseLulitatud = false;
    private bool ooSisseLulitatud = false;

    public ValgusFoorPage()
	{
		InitializeComponent();
	}

    private void OnPunane()
    {
        PuhastaTekstid();
        PunaneLabel.Text = "Seisa";
        PunaneBox.Color = Colors.Red;
        KollaneBox.Color = Colors.Gray;
        RohelineBox.Color = Colors.Gray;
    }

    private void OnKollane()
    {
        PuhastaTekstid();
        KollaneLabel.Text = "Valmista";
        PunaneBox.Color = Colors.Gray;
        KollaneBox.Color = Colors.Yellow;
        RohelineBox.Color = Colors.Gray;
    }

    private void OnRoheline()
    {
        PuhastaTekstid();
        RohelineLabel.Text = "Sõida";
        PunaneBox.Color = Colors.Gray;
        KollaneBox.Color = Colors.Gray;
        RohelineBox.Color = Colors.Green;
    }

	// Nupp Sisse
	private void OnSisseClicked(object sender, EventArgs e)
	{
		onSisselulitatud = true;
        paevSisseLulitatud = false;
        ooSisseLulitatud = false;

        // Aktiveerime värvid
        PunaneBox.Color = Colors.Red;
        KollaneBox.Color = Colors.Yellow;
        RohelineBox.Color = Colors.Green;

        SisseNupp.IsEnabled = false;
        SisseNupp.BackgroundColor = Colors.Gray;
        PaevaNupp.IsEnabled = true;
        PaevaNupp.BackgroundColor = Colors.DarkOrange;
        OoNupp.IsEnabled = true;
        OoNupp.BackgroundColor = Colors.Black;

        PuhastaTekstid();
        InfoLabel.Text = "Vali valgus";
	}

	// Nupp "Välja"
	private void OnValjaClicked(object sender, EventArgs e)
	{
		onSisselulitatud = false;
        paevSisseLulitatud = false;
        ooSisseLulitatud = false;

        SisseNupp.IsEnabled = true;
        SisseNupp.BackgroundColor = Colors.Green;
        SisseNupp.Text = "Sisse";
        PaevaNupp.IsEnabled = false;
        PaevaNupp.BackgroundColor = Colors.Gray;
        OoNupp.IsEnabled = false;
        OoNupp.BackgroundColor = Colors.Gray;

        PuhastaTekstid();

        // Muudame värvid halliks
        PunaneBox.Color = Colors.Gray;
        KollaneBox.Color = Colors.Gray;
        RohelineBox.Color = Colors.Gray;
        InfoLabel.Text = "Lülita foor sisse";

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

        if (paevSisseLulitatud || ooSisseLulitatud)
        {
            InfoLabel.Text = "Lülita esmalt foor manuaalseks";
            return;
        }

        // Tühjendame eelmised tekstid kuju sees
        PuhastaTekstid();

        // Kontrollime, millisele tulele vajutati
        string värv = e.Parameter?.ToString();

        switch (värv)
        {
            case "Punane":
                OnPunane();
                break;
            case "Kollane":
                OnKollane();
                break;
            case "Roheline":
                OnRoheline();
                break;
        }
    }

    private async void OnPaevaReziim(object sender, EventArgs e)
    {
        paevSisseLulitatud = true;
        ooSisseLulitatud = false;

        InfoLabel.Text = "";

        SisseNupp.IsEnabled = true;
        SisseNupp.BackgroundColor = Colors.Green;
        SisseNupp.Text = "Manuaalne";
        PaevaNupp.IsEnabled = false;
        PaevaNupp.BackgroundColor = Colors.Gray;
        OoNupp.IsEnabled = true;
        OoNupp.BackgroundColor = Colors.Black;

        while (paevSisseLulitatud)
        {
            if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
            OnPunane();
            await Task.Delay(3000);
            if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
            KollaneBox.Color = Colors.Yellow;
            KollaneLabel.Text = "Valmista";
            await Task.Delay(2000);
            if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
            OnRoheline();
            await Task.Delay(3000);
            if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
            for (int i = 0; i < 2; i++)
            {
                RohelineBox.Color = Colors.Gray;
                await Task.Delay(500);
                if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
                RohelineBox.Color = Colors.Green;
                await Task.Delay(500);
                if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
            }
            if (!onSisselulitatud || onSisselulitatud && !paevSisseLulitatud) break;
            OnKollane();
            await Task.Delay(2000);
            
            if (ooSisseLulitatud)
            {
                paevSisseLulitatud = false;
                AlustaOoReziimi();
                break;
            }
        }
    }

    private void OnOoReziim(object sender, EventArgs e)
    {
        ooSisseLulitatud = true;
        InfoLabel.Text = "";

        if (!paevSisseLulitatud)
        {
            AlustaOoReziimi();
        }
    }

    private async void AlustaOoReziimi()
    {

        SisseNupp.IsEnabled = true;
        SisseNupp.BackgroundColor = Colors.Green;
        SisseNupp.Text = "Manuaalne";
        PaevaNupp.IsEnabled = true;
        PaevaNupp.BackgroundColor = Colors.DarkOrange;
        OoNupp.IsEnabled = false;
        OoNupp.BackgroundColor = Colors.Gray;

        while (ooSisseLulitatud && onSisselulitatud)
        {
            OnKollane();
            await Task.Delay(500);
            KollaneBox.Color = Colors.Gray;
            await Task.Delay(500);

            if(paevSisseLulitatud)
            {
                ooSisseLulitatud = false;
                OnPaevaReziim(null, null);
                break;
            }
        }
    }


    private void PuhastaTekstid()
    {
        PunaneLabel.Text = "";
        KollaneLabel.Text = "";
        RohelineLabel.Text = "";
    }
}