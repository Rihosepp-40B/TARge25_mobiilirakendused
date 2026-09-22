namespace Example_app
{
    public class GridPage : ContentPage
    {
        Grid gr4x1, gr3x3;
        Picker picker; //Piltide valik
        Image image; // piltide näitamiseks
        Switch s_image, s_grid; //piltide ja gridide kuvamiseks/peitmiseks
        Random rnd = new Random(); //piltide juhuslikuks valimiseks

        public GridPage()
        {
            gr4x1 = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };
            picker = new Picker
            {
                Title = "Vali pilt",
                ItemsSource = new List<string> { "Pilt 1", "Pilt 2", "Pilt 3" },
                //HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Center
            };
            picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
            image = new Image
            {
                Source = "dotnet_bot.png",
                Aspect = Aspect.AspectFit
            };

            s_grid = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                IsToggled = false,
                IsEnabled = true
            };

            s_grid.Toggled += (sender, e) =>
            {
                if (e.Value)
                {
                    gr3x3 = Tee_grid3x3(); // loome ja tagastame gridi
                    gr4x1.Add(gr3x3, 0, 2);
                    gr4x1.SetColumnSpan(gr3x3, 2);
                }
                else
                {
                    gr4x1.RemoveAt(4);
                }
            };

            s_image = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                IsToggled = false,
                IsEnabled = true
            };
            s_image.Toggled += (sender, e) =>
            {
                if (e.Value)
                { image.IsVisible = true; }
                else { image.IsVisible = false; }
            };

            gr4x1.Add(picker, 0, 0);
            gr4x1.SetColumnSpan(picker, 2);
            gr4x1.Add(image, 0, 1);
            gr4x1.SetColumnSpan(image, 2);
            gr4x1.Add(s_grid, 0, 3);
            gr4x1.Add(s_image, 1, 3);

            Content = gr4x1;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (picker.SelectedIndex == -1) return; // kui ei ole valitud, siis ei tee midagi.
            if (picker.SelectedIndex == 0) image.Source = "images.jpg";
            else if (picker.SelectedIndex == 1) image.Source = "images2.jpg";
            else if (picker.SelectedIndex == 2) image.Source = "images3.jpg";

        }
        private Grid Tee_grid3x3() // 3x3 grid
        {
            gr3x3 = new Grid();
            for (int i = 0; i < 3; i++)
            {
                gr3x3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                gr3x3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for(int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    BoxView kast = new BoxView { BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)) };
                    gr3x3.Add(kast, c, r);

                    int rida = r;
                    int veerg = c;
                    TapGestureRecognizer tap = new TapGestureRecognizer();
                    tap.Tapped += async (s, args) =>
                    {
                        kast.BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                        await DisplayAlertAsync("Koordinaadid", $"Lahter on: Rida: {rida}, Veerg: {veerg}", "Selge");

                    };
                    kast.GestureRecognizers.Add(tap);
                }
            }
            return gr3x3;
        }
    }
}
