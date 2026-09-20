using Microsoft.Maui.Layouts;

namespace Example_app;

public partial class RgbPage : ContentPage
{
    BoxView boxView;

    Slider slider_r;
    Label label_r;

    Slider slider_g;
    Label label_g;

    Slider slider_b;
    Label label_b;

    AbsoluteLayout al;
    public RgbPage()
	{
        //BoxView
        boxView = new BoxView
        {
            Color = Colors.Black,
            WidthRequest = 200,
            HeightRequest = 200,
        };

        slider_r = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 50,
            MinimumTrackColor = Colors.LightGray,
            MaximumTrackColor = Colors.DarkGray,
            ThumbColor = Colors.Red,
            WidthRequest = 300,
        };

        label_r = new Label
        {
            Text = "Red = 32",
            FontSize = 18
        };

        slider_g = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 50,
            HorizontalOptions = LayoutOptions.Center,
            MinimumTrackColor = Colors.LightGray,
            MaximumTrackColor = Colors.DarkGray,
            ThumbColor = Colors.Green,
            WidthRequest = 300,
        };

        label_g = new Label
        {
            Text = "Green = 32",
            FontSize = 18
        };

        slider_b = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 50,
            HorizontalOptions = LayoutOptions.Center,
            MinimumTrackColor = Colors.LightGray,
            MaximumTrackColor = Colors.DarkGray,
            ThumbColor = Colors.Blue,
            WidthRequest = 300,
        };

        label_b = new Label
        {
            Text = "Blue = 32",
            FontSize = 18
        };

        // Sliderite Value muutmine läbi sama sündmuse
        slider_r.ValueChanged += Slider_ValueChanged;
        slider_g.ValueChanged += Slider_ValueChanged;
        slider_b.ValueChanged += Slider_ValueChanged;

        // Asjade kuvamine
            //Kuvatavate asjade nimistu
        al = new AbsoluteLayout
        { 
            Children =
            {
                boxView,
                slider_r,
                label_r,
                slider_g,
                label_g,
                slider_b,
                label_b
            } 
        };

        // BoxView asukoht
        AbsoluteLayout.SetLayoutBounds
            (
                boxView,
                new Rect(0.5, 0.1, 200, 200)
            );

        // Sätib boxView asukoha sõltuvalt ekraanist
        AbsoluteLayout.SetLayoutFlags
            (
                boxView,
                AbsoluteLayoutFlags.PositionProportional
            );

        //Sliderite ja labelite kuvamine
        List<View> controls = new List<View>
        { 
            slider_r,
            label_r,
            slider_g,
            label_g,
            slider_b,
            label_b
        };

        //Tsükkel Sliderite ja labelite asukohaks ekraanil
        for (int i = 0; i < controls.Count; i++)
        {
            double yKoht = 0.35 + i * 0.09; // double on pm float

            AbsoluteLayout.SetLayoutBounds
                (
                    controls[i],
                    // Rect = ristküülik - (X, Y, laius, kõrgus)
                    new Rect(0.5, yKoht, 300, 60)
                );

            AbsoluteLayout.SetLayoutFlags
                (
                    controls[i],
                    AbsoluteLayoutFlags.PositionProportional
                );
        }
        Content = al;

        // Algse värvi määramine
        MuudaVarvi();
    }
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs args)
    {
        // Uuendame värvi
        MuudaVarvi();

        // Näitame sliderite väärtusi
        label_r.Text = $"Red = {(int)slider_r.Value}";
        label_g.Text = $"Green = {(int)slider_g.Value}";
        label_b.Text = $"Blue = {(int)slider_b.Value}";
    }

    private void MuudaVarvi()
    {
        int r = (int)slider_r.Value;
        int g = (int)slider_g.Value;
        int b = (int)slider_b.Value;

        boxView.Color = Color.FromRgb(r, g, b);
    }
}