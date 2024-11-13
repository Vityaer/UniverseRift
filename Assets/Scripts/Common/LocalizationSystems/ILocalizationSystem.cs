using UI.Utils.Localizations.Containers;
using UnityEngine.Localization;

namespace LocalizationSystems
{
    public interface ILocalizationSystem
    {
        LocalizationUiContainer LocalizationUiContainer { get; }
        string GetString(string key);
        LocalizedString GetLocalizedContainer(string key);
    }
}
