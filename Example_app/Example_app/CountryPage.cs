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
        Entry entryName, entryCapitalCity, entryPopulation;

        string pickedPicPath = "";
        Label lblPickedPic;

        public CountryPage()
        {

            this.Title = "Riikide haldus";

            countries = new ObservableCollection<Country>
            {
                new Country { Name = "Eesti", CapitalCity = "Tallinn", Population = 1360745, Flag = "est.png" },
                new Country { Name = "Soome", CapitalCity = "Helsinki", Population = 5652881, Flag = "fin.png" },
                new Country { Name = "Rootsi", CapitalCity = "Stockholm", Population = 10701047, Flag = "swe.png" },
                new Country { Name = "Norra", CapitalCity = "Tallinn", Population = 5652989, Flag = "nor.png" },
            };

            // Sisestusväljad
            entryName = new Entry
            {
                Placeholder = "Riik"
            };

            entryCapitalCity = new Entry 
            {
                Placeholder = "Pealinn"
            };

            entryPopulation = new Entry
            { 
                Placeholder = "Rahvaarv", 
                Keyboard = Keyboard.Numeric
            };

            //PILDI VALIMISE KONTROLLID
            Button btnPickPic = new Button
            {
                Text = "📷 Vali pilt galeriist",
                BackgroundColor = Colors.LightBlue
            };

            btnPickPic.Clicked += BtnPickPic_Clicked;


            lblPickedPic = new Label
            { 
                Text = "Pilti pole valitud (kasutatakse vaikimisi pilti)",
                FontSize = 12, 
                TextColor = Colors.Gray 
            };


            // LISAMISE JA KUSTUTAMISE NUPUD
            Button btnAdd = new Button 
            { 
                Text = "Lisa", 
                BackgroundColor = Colors.LightGreen 
            };

            btnAdd.Clicked += BtnAdd_Clicked;


            Button btnDelete = new Button 
            { 
                Text = "Kustuta valitud riik", 
                BackgroundColor = Colors.LightPink };

            btnDelete.Clicked += BtnDelete_Clicked;


            // LISTVIEW JA SELLE KUJUNDUS
            list = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = countries,
                SelectionMode = ListViewSelectionMode.Single
            };

            list.ItemTapped += List_ItemTapped;

            list.ItemTemplate = new DataTemplate(() =>
            {
                // Pildi element
                Image imgPic = new Image
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 10, 0) // Veeris paremal
                };
                imgPic.SetBinding(Image.SourceProperty, "Flag");

                // Tekstide elemendid
                Label lblName = new Label 
                { 
                    FontSize = 18, 
                    FontAttributes = FontAttributes.Bold 
                };

                lblName.SetBinding(Label.TextProperty, "Name");


                Label lblCapitalCity = new Label 
                { 
                    TextColor = Colors.Gray 
                };

                lblCapitalCity.SetBinding(Label.TextProperty, "CapitalCity");


                Label lblPopulation = new Label 
                { TextColor = Colors.DarkBlue, 
                    FontAttributes = FontAttributes.Bold 
                };

                lblPopulation.SetBinding(Label.TextProperty, "Population");


                var textLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center,
                    Children = 
                    { 
                        lblName, 
                        lblCapitalCity, 
                        lblPopulation 
                    }
                };


                // Kogu rea paigutus (Pilt vasakul, tekst paremal)
                var rowLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10),
                    Children = { imgPic, textLayout }
                };

                return new ViewCell { View = rowLayout };
            });


            // PANEME KÕIK LEHELE KOKKU
            this.Content = new StackLayout
            {
                Padding = new Thickness(10),
                Children =
                {
                    entryName,
                    entryCapitalCity,
                    entryPopulation,
                    btnPickPic,   // Uus nupp galerii jaoks
                    lblPickedPic, // Tagasiside silt
                    btnAdd,
                    btnDelete,
                    list
                }
            };
        }

        // --- SÜNDMUSTE TÖÖTLEJAD (Event Handlers) ---

        // Pildi valimine galeriist
        private async void BtnPickPic_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {
                    pickedPicPath = photo.FullPath; // Jätame asukoha meelde
                    lblPickedPic.Text = $"Valitud: {photo.FileName}";
                    lblPickedPic.TextColor = Colors.Green;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Viga", "Pildi valimine ebaõnnestus: " + ex.Message, "OK");
            }
        }

        // Uue lisamine
        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(entryName.Text) && !string.IsNullOrWhiteSpace(entryCapitalCity.Text))
            {
                int population = 0;
                int.TryParse(entryPopulation.Text, out population);

                // Kui pilti ei valitud, kasutame vaikimisi faili
                string picName = string.IsNullOrWhiteSpace(pickedPicPath) ? "pirateflag.png" : pickedPicPath;

                bool duplicateCheck = countries.Any(c => c.Name.Equals(entryName.Text, StringComparison.OrdinalIgnoreCase));

                if (!duplicateCheck)
                {
                    countries.Add(new Country
                    {
                        Name = entryName.Text,
                        CapitalCity = entryCapitalCity.Text,
                        Population = population,
                        Flag = picName
                    });
                }
                else
                {
                    DisplayAlert("Duplkaat", $"Riik: {entryName.Text} on juba olemas", "Sulge");
                }
                

                // Puhastame väljad uue sisestuse jaoks
                entryName.Text = "";
                entryCapitalCity.Text = "";
                entryPopulation.Text = "";

                // Lähtestame pildi valiku oleku
                pickedPicPath = "";
                lblPickedPic.Text = "Pilti pole valitud (kasutatakse vaikimisi pilti)";
                lblPickedPic.TextColor = Colors.Gray;
            }
            else
            {
                DisplayAlert("Viga", "Palun täida vähemalt riigi ja pealinna väljad!", "OK");
            }
        }


        // Kustutamine
        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            Country pickedCountry = list.SelectedItem as Country;

            if (pickedCountry != null)
            {
                bool response = await DisplayAlert("Kinnitus", $"Kas oled kindel, et soovid {pickedCountry.Name} kustutada?", "Jah", "Ei");

                if (response == true)
                {
                    countries.Remove(pickedCountry);
                    list.SelectedItem = null;
                }
            }
            else
            {
                await DisplayAlert("Viga", "Palun vali nimekirjast riik, mida soovid kustutada.", "OK");
            }
        }


        // Loendis reale vajutamine
        private async void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Country pickedCountry = e.Item as Country;

            if (pickedCountry != null)
            {
                await DisplayAlert("Riigi info", $"Pealinn: {pickedCountry.CapitalCity}\nNimetus: {pickedCountry.Name}\nRahvaarv: {pickedCountry.Population}", "Sulge");
            }
        }

        // VAJA TEHA AJAKOHASTAMINE (muutmine)
    }
}
