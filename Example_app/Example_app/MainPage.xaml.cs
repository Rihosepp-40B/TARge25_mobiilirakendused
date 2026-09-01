namespace Example_app
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
            ResetBtn.Text = $"Tagasi nulli";
            dotnetBot.Rotation += 10;
            dotnetBot.Opacity -= 0.1; // Pilt muutub läbipaistvamaks iga vajutusega
            dotnetBot.Scale -= 0.1; // Pilt muutb väiksemaks iga vajutusega

            //Genereerime juhusliku värvi
            var random = new Random();
            var color = Color.FromRgb(random.Next(256), random.Next(256), random.Next(256)); // Loome juhusliku värvi RGB väärtuste põhjal
            ResetBtn.BackgroundColor = color; // Määrame nupu taustavärviks juhusliku värvi
            
            //Elementide peitmine ja näitamine
            if (count % 10==0) // või count==10
            {
                dotnetBot.IsVisible = false; // Peidamine dotnetBot pildi, kui count on 10
            }
        }

        private void ResetBtn_Clicked(object sender, EventArgs e)
        {
            count = 0;
            CounterBtn.Text = $"Clicked {count} times";
            ResetBtn.Text = $"Reset oli tehtud";
            dotnetBot.Rotation = 0;
            dotnetBot.Opacity = 1;
            dotnetBot.Scale = 1;

            dotnetBot.IsVisible = true; // Teeme dotnetBot pildi nähtavaks, kui count on null
            ResetBtn.ClearValue(Button.BackgroundColorProperty); // Eemaldame nupu taustavärvi, et see naaseks algsesse olekusse
        }

        private void Paremale_Vasakule_Clicked(object sender, EventArgs e)
        {
            if(dotnetBot.HorizontalOptions == LayoutOptions.Start)
            {
                dotnetBot.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                dotnetBot.HorizontalOptions = LayoutOptions.Start;
            }
        }
    }
}
