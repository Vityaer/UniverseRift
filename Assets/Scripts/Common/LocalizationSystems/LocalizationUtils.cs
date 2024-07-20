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
            //if (result == null || result.WaitForCompletion && result?.Keys.Count == 0 || result.GetLocalizedString() == null || result.GetLocalizedString().Contains("No translation found for "))
            //{
                //Debug.LogError($"Not found localization for: {key} in table: {tableName}");
            //}
            return result;
        }
    }
}
