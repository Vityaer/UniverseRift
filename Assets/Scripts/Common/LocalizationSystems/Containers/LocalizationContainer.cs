using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UI.Utils.Localizations.Containers
{
    public abstract class LocalizationContainer
    {
        protected bool _isLocalizationLoad;

        private StringTable _stringTable;
        
        protected Dictionary<string, LocalizedString> Locales = new();
        protected abstract string TableName { get; }

        public ReactiveCommand OnLoadLocalization = new();
        public bool IsLocalizationLoad => _isLocalizationLoad;

        protected async UniTaskVoid WaitLoadLocation()
        {
            _stringTable = await LocalizationSettings.StringDatabase.GetTableAsync(TableName);
            var asyncLoad = LocalizationSettings.InitializationOperation;
            await UniTask.WaitUntil(() => asyncLoad.IsDone);
            _isLocalizationLoad = true;
            OnLoadLocalization.Execute();
        }

        public bool ExistLocaleId(string id)
        {
            return _stringTable.GetEntry(id) != null;
        }

        public string GetString(string key)
        {
            //if(!_isLocalizationLoad)
            //    return string.Empty;
            
            return GetLocalizedContainer(key).GetLocalizedString(string.Empty);
        }

        public async UniTask<string> GetStringAsync(string key)
        {
            var loadAsync = GetLocalizedContainer(key).GetLocalizedStringAsync();
            await UniTask.WaitUntil(() => loadAsync.IsDone);
            return loadAsync.Result;
        }

        public LocalizedString GetLocalizedContainer(string key)
        {
            if (!Locales.ContainsKey(key))
            {
                AddLocalizationContainer(key);
            }
            return Locales[key];
        }

        private void AddLocalizationContainer(string key)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                UnityEngine.Debug.LogError("table name empty");
            }
            var locale = LocalizationUtils.GetLocalizedString(TableName, key);
            Locales.Add(key, locale);
        }
    }
}
