namespace Example_app;

public partial class Timer_Page : ContentPage
{
	public Timer_Page()
	{
		InitializeComponent();
	}
	bool on_off = true;
	private async void ShowTime()
	{
		while (on_off)
		{
			timer_btn.Text = DateTime.Now.ToString("T");
			await Task.Delay(1000);
		}
	}

    private void timer_btn_Clicked(object sender, EventArgs e)
    {
		if (on_off)
		{ on_off = true;
		ShowTime();}
    }
}