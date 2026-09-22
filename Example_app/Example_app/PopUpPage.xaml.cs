namespace Example_app;

public partial class PopUpPage : ContentPage
{
	public PopUpPage()
	{
        // 1. Loome esimese nupu (Lihtne teade)
        Button alertButton = new Button
        {
            Text = "Teade",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        // Seome nupu klikkimise sündmuse funktsiooniga
        alertButton.Clicked += AlertButton_Clicked;

        // 2. Loome teise nupu (Kinnitus)
        Button alertYesNoButton = new Button
        {
            Text = "Jah või ei",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertYesNoButton.Clicked += AlertYesNoButton_Clicked;

        // 3. Loome kolmanda nupu (Valikumenüü)
        Button alertListButton = new Button
        {
            Text = "Valik",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertListButton.Clicked += AlertListButton_Clicked;

        // 5. Loome neljanda nupu
        Button alertQuestButton = new Button
        {
            Text = "Küsimus",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions= LayoutOptions.Center
        };
        alertQuestButton.Clicked += AlertQuestButton_Clicked;
   
        // 4. Paigutame kõik nupud ekraanile üksteise alla

        Content = new VerticalStackLayout
        {
            Spacing = 20, // Jätab nuppude vahele 20 pikslit vaba ruumi
            Padding = new Thickness(0, 50, 0, 0), // Lükkab sisu veidi ülevalt alla
            Children = { alertButton, alertYesNoButton, alertListButton, alertQuestButton }
        };
    }

    // 1. Nupp: Lihtne teade
    private async void AlertButton_Clicked(object? sender, EventArgs e)
    {
        // Kuvab lihtsalt teate ja ootab, kuni kasutaja vajutab "OK"
        await DisplayAlertAsync("Teade", "Teil on uus teade", "OK");
    }

    // 2. Nupp: Jah või ei valik
    private async void AlertYesNoButton_Clicked(Object? sender, EventArgs e)
    {
        // Küsime kasutajalt kinnitust (tagastab true või false)
        bool result = await DisplayAlertAsync("Kinnitus", "Kas oled kindel?", "Olen kindel", "Ei ole kindel");

        // Kuvame uue teate vastavalt sellele, mida kasutaja valis
        // (result ? "Jah" : "Ei") tähendab: kui result on true, kirjuta "Jah", muidu "Ei"
        await DisplayAlertAsync("Teade", "Teie valik on: " + (result ? "Jah" : "Ei"), "OK");
    }
    
    // 3. Nupp: Valikute nimekiri
    private async void AlertListButton_Clicked(object? sender,EventArgs e)
    {
        // Kuvab menüü ja salvestab kasutaja valitud teksti muutujasse "Action"
        string action = await DisplayActionSheetAsync("Mida teha?", "Loobu", "Kustutada", "Tantsida", "laulda", "Joonestada");

        
        // Kontrollime, et kasutaja ei vajutanud lihtsalt kõrvale ega valinud "Loobu"
        if (action != null && action != "Loobu")
        {
            await DisplayAlertAsync("Valik", "Sa valisid tegevuse: " + action, "OK");
        }
    }

    // 4.Nupp valikvastused
    private async void AlertQuestButton_Clicked(object sender, EventArgs e)
    {
        string result1 = await DisplayPromptAsync("Küsimus", "Kuidas läheb?", placeholder: "Tore!");
        string result2 = await DisplayPromptAsync("Vasta", "Millega võrdub 5 + 5?", initialValue: "10", maxLength: 2, keyboard: Keyboard.Numeric);
    }
}