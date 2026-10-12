using Interactive_CarouselView_Gallery.Services;

namespace Interactive_CarouselView_Gallery
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            var savedLang = Preferences.Get("AppLanguage", "en");
            LanguageService.ChangeLanguage(savedLang);
        }        
    }
}