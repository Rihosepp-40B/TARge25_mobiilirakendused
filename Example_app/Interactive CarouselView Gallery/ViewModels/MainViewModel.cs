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

        public string AppTitle => AppResources.AppTitle;
        public string ChangeLanguageText => AppResources.ChangeLanguage;
        public string ChangeLanguageIcon => AppResources.ChangeLanguageIcon;
        public string Close => AppResources.Close;

        public ICommand SetEnglishCommand { get; }
        public ICommand SetEstonianCommand { get; }

        public ICommand ShowLanguageSelectionCommand { get; }

        public MainViewModel()
        {
            SetEnglishCommand = new Command(() => ChangeLanguage("en"));
            SetEstonianCommand = new Command(() => ChangeLanguage("et"));

            ShowLanguageSelectionCommand = new Command(async () => await OnShowLanguageSelection());

            LanguageService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string code)
        {
            LanguageService.ChangeLanguage(code);
        }

        private void OnLanguageChanged()
        {
            OnPropertyChanged(nameof(AppTitle));
            OnPropertyChanged(nameof(ChangeLanguageText));
            OnPropertyChanged(nameof(ChangeLanguageIcon));
            OnPropertyChanged(nameof(Close));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async Task OnShowLanguageSelection()
        // Keele nupu vajutamisel kuvatavad valikud
        {
            // Küsi kasutajalt keelt (saate tekstid võtta ka AppResources-ist)
            string response = await Shell.Current.DisplayActionSheet(
                AppResources.ChangeLanguage,
                AppResources.Close, // Või "Cancel"
                null,
                AppResources.English, AppResources.Estonian);

            if (response == AppResources.English)
            {
                ChangeLanguage("en");
            }
            else if (response == AppResources.Estonian)
            {
                ChangeLanguage("et");
            }
        }
    }
}