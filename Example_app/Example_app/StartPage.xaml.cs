namespace Example_app;

public partial class StartPage : ContentPage
{
	VerticalStackLayout vsl;
	public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(), new FigurePage(), new Timer_Page(), new ValgusFoorPage(), new DateTimePage(), new SteppSliderPage() };
	public List<string> Lehenimed = new List<string>() { "Testid", "Kujundus", "Taimer", "Valgusfoor", "Datetime", "SteppSlider" };
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
		Content = vsl;
	}
}