using Microsoft.Extensions.DependencyInjection;

namespace TicTacToe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {            
            var startPage = new MainPage();
            //Pakime selle NavigationPAge sisse, et saaksime kasutada navigeerimist
            var navPage = new NavigationPage(startPage)
            {
                BarBackgroundColor = Colors.Black,
                BarTextColor = Colors.DarkGoldenrod
            };
            return new Window(navPage);
        }
    }
}