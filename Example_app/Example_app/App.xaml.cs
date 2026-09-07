using Microsoft.Extensions.DependencyInjection;

namespace Example_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // loome esimese lehe (startPage)

            var startPage = new StartPage();
            //Pakime selle NavigationPAge sisse, et saaksime kasutada navigeerimist
            var navPage = new NavigationPage(startPage)
            {
                BarBackgroundColor = Colors.LightBlue,
                BarTextColor = Colors.White
            };
            return new Window(navPage);
        }
    }
}