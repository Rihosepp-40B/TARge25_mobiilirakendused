
using MultiLangApp.Services;

namespace MultiLangApp
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