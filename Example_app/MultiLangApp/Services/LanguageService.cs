using MultiLangApp.Resources.Localization;
using System.Globalization;
using Microsoft.Maui.Storage;

namespace MultiLangApp.Services
{
    public static class LanguageService
    {
        public static event Action? LanguageChanged;

        public static void ChangeLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            AppResources.Culture = culture;

            Preferences.Set("AppLanguage", languageCode);
            LanguageChanged?.Invoke();
        }
    }
}