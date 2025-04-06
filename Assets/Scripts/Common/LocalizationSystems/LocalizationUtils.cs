using UnityEngine;
using UnityEngine.Localization;

namespace UI.Utils.Localizations
{
    public static class LocalizationUtils
    {
        public static LocalizedString GetUiLocalizedString(string key)
        {
            return GetLocalizedString(Constants.Localization.UI_TABLE_NAME, key);
        }

        public static LocalizedString GetLocalizedString(string tableName, string key)
        {
            var result = new LocalizedString(tableName, key);
            return result;
        }
    }
}
