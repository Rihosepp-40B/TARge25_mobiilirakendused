using System.Globalization;
using Interactive_CarouselView_Gallery.Resources.Localization;
using Microsoft.Maui.Storage;

namespace Interactive_CarouselView_Gallery.Services
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