using UI.Utils.Localizations.Containers;
using UnityEngine;
using UnityEngine.Localization;

namespace LocalizationSystems
{
    public class LocalizationSystem : ILocalizationSystem
    {
        private readonly LocalizationUiContainer _localizationUiContainer;

        public LocalizationUiContainer LocalizationUiContainer => _localizationUiContainer;

        public LocalizationSystem()
        {
            _localizationUiContainer = new();
        }


        public string GetString(string key)
        {
            return _localizationUiContainer.GetLocalizedContainer(key).GetLocalizedString(string.Empty);
        }

        public LocalizedString GetLocalizedContainer(string key)
        {
            return _localizationUiContainer.GetLocalizedContainer(key);
        }
    }
}
