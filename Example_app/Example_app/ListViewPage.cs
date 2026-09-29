using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Example_app
{
    public class Telefon
    {
        public string Nimetus { get; set; }
        public string Tootja { get; set; }
        public int Hind {  get; set; }
        public string Pilt { get; set; }
    }
    public class ListViewPage : ContentPage
    {
        ListView list;
        ObservableCollection<Telefon> telefonid;
        Entry entryNimetus, entrytootja, entryHind, entryPilt;

        public ListViewPage()
        {
            // Konstruktoris andmete algväärtustamine
            telefonid = new ObservableCollection<Telefon>
                {
            new Telefon { Nimetus="Samsung Galaxy S22 Ultra", Tootja="Samsung", Hind=1349, Pilt="Galaxy.png" },
            new Telefon { Nimetus="Xiaomi Mi 11 Lite 5G NE", Tootja="Xiaomi", Hind=399, Pilt="Xiaomi5GNE.png" },
            new Telefon { Nimetus="iPhone 13 mini", Tootja="Apple", Hind=1179, Pilt="iPhone13.png" }
            };

            ListView list = new ListView
            {
                HasUnevenRows = true, // Lubab ridadel olla erineva kõrgusega
                ItemsSource = telefonid,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label nimetus = new Label { FontSize = 20 };
                    nimetus.SetBinding(Label.TextProperty, "Nimetus"); // Seome klassi omadusega "Nimetus"

                    Label hind = new Label();
                    hind.SetBinding(Label.TextProperty, "Hind");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { nimetus, hind }
                        }
                    };
                })
            };
            // Seome sündmuse ListView-ga
            list.ItemTapped += List_ItemTapped;

            Content = new StackLayout
            {
                Children =
                {
                    list
                }
            };
        }

        // Sündmuse töötleja (Event handler)
        private async void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Konverteerime valitud elemendi (e.Item) Telefon objektiks
            Telefon selectedPhone = e.Item as Telefon;

            // Kontrollime alati, kas konverteerimine õnnestus ega poleks null
            if (selectedPhone != null)
            {
                // Kuvame ekraanil hüpikakna
                await DisplayAlert("Valitud mudel", $"{selectedPhone.Tootja} - {selectedPhone.Nimetus}", "OK");
            }
        }
    }
}
