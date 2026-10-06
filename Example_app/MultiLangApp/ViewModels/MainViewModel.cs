using MultiLangApp.Services;
using MultiLangApp.Resources.Localization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MultiLangApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Greeting => AppResources.GreetingText;
        public string ChangeLanguageLabel => AppResources.ChangeLanguage;
        public string EnglishButton => AppResources.EnglishButton;
        public string GermanButton => AppResources.GermanButton;
        public string EstonianButton => AppResources.EstonianButton;

        public ICommand SetEnglishCommand { get; }
        public ICommand SetGermanCommand { get; }
        public ICommand SetEstonianCommand { get; }

        public MainViewModel()
        {
            SetEnglishCommand = new Command(() => ChangeLanguage("en"));
            SetGermanCommand = new Command(() => ChangeLanguage("de"));
            SetEstonianCommand = new Command(() => ChangeLanguage("et"));

            LanguageService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string code)
        {
            LanguageService.ChangeLanguage(code);
        }

        private void OnLanguageChanged()
        {
            OnPropertyChanged(nameof(Greeting));
            OnPropertyChanged(nameof(ChangeLanguageLabel));
            OnPropertyChanged(nameof(EnglishButton));
            OnPropertyChanged(nameof(GermanButton));
            OnPropertyChanged(nameof(EstonianButton));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}