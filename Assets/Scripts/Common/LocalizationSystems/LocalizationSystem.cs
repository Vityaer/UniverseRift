using UI.Utils.Localizations.Containers;
using UnityEngine;

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
    }
}
