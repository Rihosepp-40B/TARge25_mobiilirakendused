using Interactive_CarouselView_Gallery.Models;
using Interactive_CarouselView_Gallery.Resources.Localization;
using System.Collections.ObjectModel;

namespace Interactive_CarouselView_Gallery.Views
{
    public partial class MainPage : ContentPage
    {
        Grid backgroundGrid, mainGrid, grid, galleryGrid;
        BoxView gradient, gradientLine;
        Label label, summaryLabel;
        Frame frame;

        private CarouselView carouselView;

        // List asendati ObservableCollectioniga
        private ObservableCollection<CarouselItem> items;

        private IDispatcherTimer carouselTimer;
        // Piltide null positsioon (kerimisel suureneb see number
        private int position = 0;

        public MainPage()
        {
            Title = AppResources.AppTitle;

            ToolbarItems.Add(new ToolbarItem
            {
                IconImageSource = AppResources.ChangeLanguageIcon,
                Text = AppResources.ChangeLanguage,
                Order = ToolbarItemOrder.Primary,
                //Command = new Command(async () => await SelectLanguage())
            });

            backgroundGrid = new Grid
            {
                BackgroundColor = Color.FromHex("#131406")
            };


            MainGrid();

            // Initsialiseerime ObservableCollectioni
            items = new ObservableCollection<CarouselItem>
            {
                new CarouselItem
                {
                    Title = "Lasanje",
                    ImageUrl = "https://ichef.bbci.co.uk/food/ic/food_16x9_1600/recipes/slow_cooker_lasagne_57243_16x9.jpg", 
                    ShortSummary = "Short", 
                    Ingredients = new List<Ingredient> 
                    { 
                        new Ingredient { Name = "Jahu", Amount = 200, Unit = "grammi" }
                    },
                    PrepTimeMinutes = 30,
                    CookTimeMinutes = 30,
                    DirectionSteps = new List<Step>
                    {
                        new Step {Name = "Ettevalmistused", Direction = "Valmista ette, bla bla bla"},
                        new Step {Name = "Kastme tegemine", Direction = "Pane potti ja bla bla bla"},
                        new Step {Name = "Kokku panemine", Direction = "Pane küpsetus potti ja bla bla bla"},
                        new Step {Name = "Küpsetamine", Direction = "Ahi on 220 kraadi C soe, pane küpsetus pott ahju ja bla bla bla"},

                    }
                },
                new CarouselItem { Title = "Metsavaikus", ImageUrl = "https://picsum.photos/id/1016/600/400" },
                new CarouselItem { Title = "Järvepeegel", ImageUrl = "https://picsum.photos/id/1018/600/400" }
            };

            GalleryCarousel();

            var indicatorView = CarouselIndicator();

            AutoRotateCarousel();

            mainGrid.Add(carouselView, 0, 0);
            mainGrid.Add(indicatorView, 0, 1);

            backgroundGrid.Children.Add(mainGrid);

            Content = backgroundGrid;
        }

        private void MainGrid()
            // Teeb kahe realise grid'i et alati oleks sisu ekraani mõõdus
        {
            mainGrid = new Grid
            {
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(8, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    },
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                    }
            };
        }

        private void GalleryCarousel()
        {
            // Karusselli loomine
            carouselView = new CarouselView
            {
                ItemsSource = items,
                PeekAreaInsets = new Thickness(40, 0, 40, 0),

                ItemTemplate = new DataTemplate(() =>
                {
                    GalleryFrame();

                    GalleryGrid();
                                        
                    var image = new Image { Aspect = Aspect.AspectFill };
                    image.SetBinding(Image.SourceProperty, "ImageUrl");

                    GalleryGradients();

                    GalleryLabels();
                    
                    grid.Children.Add(image);
                    grid.Children.Add(gradient);
                    grid.Children.Add(gradientLine);
                    grid.Children.Add(label);

                    galleryGrid.Add(summaryLabel, 0, 1);
                                        
                    frame.Content = galleryGrid;

                    return frame;
                })
            };
            carouselView.PositionChanged += CarouselView_PositionChanged;
        }

        private void GalleryFrame()
        {
            frame = new Frame
            {
                // Raam mille sees on pilt, pidli varjunduus, pildi label
                CornerRadius = 15,
                HasShadow = true,
                Padding = 0,
                Margin = new Thickness(5),
                BackgroundColor = Colors.Black
            };
        }

        private void GalleryGrid()
        //Funktsioon Galerii grid tegemiseks
        {
            grid = new Grid();

            galleryGrid = new Grid
            {
                RowDefinitions =
                            {
                                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                            },
                ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                            }
            };

            galleryGrid.Add(grid, 0, 0);
        }

        private void GalleryGradients()
        // See tekitab karuselli galeriis piltidele variundid.
        {
            gradient = new BoxView
            {
                Background = new RadialGradientBrush
                {
                    Center = new Point(0.5, 0.3),
                    Radius = 0.5,
                    GradientStops = new GradientStopCollection
                            {
                                new GradientStop(Colors.Black.WithAlpha(0.7f), 1f),
                                new GradientStop(Colors.Gold.WithAlpha(0.3f), 0.7f),
                                new GradientStop(Colors.Transparent, 0)
                            }
                }
            };
            gradientLine = new BoxView
            {
                Background = new LinearGradientBrush
                {
                    StartPoint = new Point(1, 1),
                    EndPoint = new Point(1, 0),
                    GradientStops = new GradientStopCollection
                            {
                                new GradientStop(Colors.Black.WithAlpha(1), 0.05f),
                                new GradientStop(Colors.Transparent, 0.2f)
                            }
                }
            };
        }

        private void GalleryLabels()
        {
            label = new Label
            {
                TextColor = Colors.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(15),
                VerticalOptions = LayoutOptions.End
            };

            label.SetBinding(Label.TextProperty, "Title");

            summaryLabel = new Label
            {
                TextColor = Colors.White,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(15),
                VerticalOptions = LayoutOptions.Start
            };

            summaryLabel.SetBinding(Label.TextProperty, "ShortSummary");
        }

        private void AutoRotateCarousel()
        {
            carouselTimer = Application.Current.Dispatcher.CreateTimer();
            carouselTimer.Interval = TimeSpan.FromSeconds(4);

            carouselTimer.Tick += (s, e) =>
            {
                if (items == null || items.Count == 0) return;

                position = (position + 1) % items.Count;
                carouselView.Position = position;
            };

            carouselTimer.Start();
        }

        private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            position = e.CurrentPosition;

            //Taaskäivitab 4sekundi aja
            carouselTimer?.Stop();
            carouselTimer?.Start();
        }

        private IndicatorView CarouselIndicator()
        // Piltide indikaator (mullikesed)
        {
            var indicatorView = new IndicatorView
            {
                IndicatorColor = Colors.Gray,
                SelectedIndicatorColor = Colors.Red,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10)
            };

            carouselView.IndicatorView = indicatorView;

            return indicatorView;
        }
    }
}