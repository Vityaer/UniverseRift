using System.Collections.Generic;
using UnityEngine.Localization;

namespace UI.Utils.Localizations.Extensions
{
    public static class LocaliationExtensions
    {
        public static LocalizedString WithArguments(this LocalizedString localizedString, List<object> args)
        {
            localizedString.Arguments = args;
            return localizedString;
        }
    }
}
