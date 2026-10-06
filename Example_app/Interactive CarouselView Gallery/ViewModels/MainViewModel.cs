using Interactive_CarouselView_Gallery.Resources.Localization;
using Interactive_CarouselView_Gallery.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Interactive_CarouselView_Gallery.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //public string Greeting => AppResources.GreetingText;
        //public string ChangeLanguageLabel => AppResources.ChangeLanguage;
        //public string EnglishButton => AppResources.EnglishButton;
        //public string EstonianButton => AppResources.EstonianButton;

        public ICommand SetEnglishCommand { get; }
        public ICommand SetGermanCommand { get; }
        public ICommand SetEstonianCommand { get; }

        public MainViewModel()
        {
            SetEnglishCommand = new Command(() => ChangeLanguage("en"));
            SetEstonianCommand = new Command(() => ChangeLanguage("et"));

            LanguageService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string code)
        {
            LanguageService.ChangeLanguage(code);
        }

        private void OnLanguageChanged()
        {
            //OnPropertyChanged(nameof(Greeting));
            //OnPropertyChanged(nameof(ChangeLanguageLabel));
            //OnPropertyChanged(nameof(EnglishButton));
            //OnPropertyChanged(nameof(EstonianButton));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}