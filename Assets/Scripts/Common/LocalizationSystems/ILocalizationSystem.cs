using System;
using UI.Utils.Localizations.Containers;
using UniRx;
using UnityEngine.Localization;

namespace LocalizationSystems
{
    public interface ILocalizationSystem
    {
        LocalizationUiContainer LocalizationUiContainer { get; }
        string GetString(string key);
        LocalizedString GetLocalizedContainer(string key);
        IObservable<Locale> OnChangeLanguage { get; }
        bool ExistLocaleId(string id);
    }
}
