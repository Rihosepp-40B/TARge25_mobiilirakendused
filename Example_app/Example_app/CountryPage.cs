using System.Collections.ObjectModel;


namespace Example_app
{
    public class Country
    {
        public string Name { get; set; }
        public string CapitalCity { get; set; }
        public int Population { get; set; }
        public string Flag { get; set; }
    }

    public class CountryPage : ContentPage
    {
        ListView list;
        ObservableCollection<Country> countries;
        Entry entryName, entryCapitalCity, entryPopulation, entryFlag;

        public CountryPage()
        {
            countries = new ObservableCollection<Country>
            {
                new Country { Name = "Eesti", CapitalCity = "Tallinn", Population = 1360745, Flag = "Est.png" },
                new Country { Name = "Soome", CapitalCity = "Helsinki", Population = 5652881, Flag = "Fin.png" },
                new Country { Name = "Rootsi", CapitalCity = "Stockholm", Population = 10701047, Flag = "Swe.png" },
                new Country { Name = "Norra", CapitalCity = "Tallinn", Population = 5652989, Flag = "Nor.png" },
            };

            list = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = countries,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label name = new Label { FontSize = 20 };
                    name.SetBinding(Label.TextProperty, "Name"); // Seob name klassi omadusega Name

                    Label capitalCity = new Label();
                    capitalCity.SetBinding(Label.TextProperty, "CapitalCity");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { name, capitalCity }
                        }
                    };
                })
            };
            list.ItemTapped += List_ItemTapped;

            Button btnKustuta = new Button
            {
                Text = "Kustuta",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White
            };

            btnKustuta.Clicked += BtnKustuta_Clicked;

            entryName = new Entry { Placeholder = "Riik" };
            entryCapitalCity = new Entry { Placeholder = "Pealinn" };
            entryPopulation = new Entry { Placeholder = "Rahvaarv" };
            entryFlag = new Entry { Placeholder = "Lipp" };

            /// SIIIn on pooleli

            Button btnLisa = new Button
            {
                Text = "Lisa telefon",
                BackgroundColor = Colors.Green,
                TextColor = Colors.White
            };

            btnLisa.Clicked += BtnLisa_Clicked;

            Content = new StackLayout
            {
                Children =
                {
                    entryNimetus,
                    entryTootja,
                    list,
                    btnKustuta,
                    btnLisa
                }
            };
        }
    }
}
