using System;
using UI.Utils.Localizations.Containers;
using UniRx;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VContainer.Unity;

namespace LocalizationSystems
{
    public class LocalizationSystem : ILocalizationSystem, IInitializable, IDisposable
    {
        private readonly LocalizationUiContainer _localizationUiContainer;

        private ReactiveCommand<Locale> _onChangeLanguage = new();
        public LocalizationUiContainer LocalizationUiContainer => _localizationUiContainer;

        public IObservable<Locale> OnChangeLanguage => _onChangeLanguage;

        public LocalizationSystem()
        {
            _localizationUiContainer = new();
        }

        public void Initialize()
        {
            LocalizationSettings.SelectedLocaleChanged += ChangeLocale;
        }

        private void ChangeLocale(Locale locale)
        {
            _onChangeLanguage.Execute(locale);
        }

        public string GetString(string key)
        {
            return _localizationUiContainer?.GetLocalizedContainer(key)?.GetLocalizedString();
        }

        public LocalizedString GetLocalizedContainer(string key)
        {
            return _localizationUiContainer.GetLocalizedContainer(key);
        }

        public bool ExistLocaleId(string id)
        {
            return _localizationUiContainer.ExistLocaleId(id);
        }

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= ChangeLocale;
        }
    }
}
