using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Misc.Json;
using Models.Common;
using System.Collections.Generic;
using TMPro;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace City.Panels.Misc.Settings
{
    public class SettingsPanelController : UiPanelController<SettingsPanelView>
    {
        private const string CONTAINER_SAVE_KEY = "SettingsContainer";

        private readonly IJsonConverter _jsonConverter;
        private readonly ILocalizationSystem _localizationSystem;

        private SettingsContainer _settingsContainer;

        public SettingsPanelController(ILocalizationSystem localizationSystem, IJsonConverter jsonConverter)
        {
            _localizationSystem = localizationSystem;
            _jsonConverter = jsonConverter;
        }

        public override void Start()
        {
            LoadSave();
            FillSettings();
            SetData();
            View.LanguageDropdown.OnChangeValue.Subscribe(ChangeLanguage).AddTo(Disposables);
            Save();
            base.Start();
        }

        private void LoadSave()
        {
            if (PlayerPrefs.HasKey(CONTAINER_SAVE_KEY))
            {
                _settingsContainer = _jsonConverter
                    .Deserialize<SettingsContainer>(PlayerPrefs.GetString(CONTAINER_SAVE_KEY));
            }

            if(_settingsContainer == null)
                _settingsContainer = new();
        }

        private void ChangeLanguage(int obj)
        {
            var LocalCode = View.LanguageCodes[obj];
            SelectLanguage(LocalCode);
            Save();
        }

        private void SelectLanguage(string selectedCode)
        {
            _settingsContainer.LanguageCode = selectedCode;

            var locale = LocalizationSettings.AvailableLocales.Locales
                .Find(locale => locale.Identifier.Code == selectedCode);

            if (locale == null)
                LocalizationSettings.AvailableLocales.Locales
                    .Find(locale => locale.Identifier.Code.Equals("en"));

            LocalizationSettings.SelectedLocale = locale;
            LocalizeLanguageDropdown();
        }

        private void FillSettings()
        {
            LocalizeLanguageDropdown();
        }

        private void LocalizeLanguageDropdown()
        {
            var currentIndex = View.LanguageDropdown.Dropdown.value;
            View.LanguageDropdown.Dropdown.ClearOptions();

            var languageNames = new List<TMP_Dropdown.OptionData>();
            foreach (var code in View.LanguageCodes)
            {
                languageNames.Add(new(_localizationSystem.GetString($"Language_{code}_Name")));
            }
            View.LanguageDropdown.Dropdown.AddOptions(languageNames);
            View.LanguageDropdown.SetData(currentIndex);
        }

        private void SetData()
        {
            var index = View.LanguageCodes.FindIndex(code => code.Equals(_settingsContainer.LanguageCode));
            if (index < 0)
                index = 0;

            View.LanguageDropdown.SetData(index);
            var LocalCode = View.LanguageCodes[index];
            SelectLanguage(LocalCode);
        }

        private void Save()
        {
            var save = _jsonConverter.Serialize(_settingsContainer);
            PlayerPrefs.SetString(CONTAINER_SAVE_KEY, save);
        }

        public override void Dispose()
        {
            Save();
            base.Dispose();
        }
    }
}
