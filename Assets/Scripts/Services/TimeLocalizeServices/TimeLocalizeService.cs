using LocalizationSystems;
using System;
using VContainer.Unity;
using System.Collections.Generic;
using UnityEngine.Localization;
using UI.Utils.Localizations.Extensions;

namespace Services.TimeLocalizeServices
{
    public class TimeLocalizeService : IInitializable
    {
        private readonly ILocalizationSystem _localizationSystem;
        private Dictionary<PartTimeType, LocalizedString> _partTimeLocalizes = new();

        public TimeLocalizeService(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public void Initialize()
        {
            TryLoadLocalizes();

        }

        private void TryLoadLocalizes()
        {
            if (_partTimeLocalizes.Count == 0)
                foreach (var partType in (PartTimeType[])Enum.GetValues(typeof(PartTimeType)))
                    AddTimePartLocalize(partType);
        }

        private void AddTimePartLocalize(PartTimeType partType)
        {
            _partTimeLocalizes.Add(partType, _localizationSystem.GetLocalizedContainer($"Time{partType}Counter"));
        }

        public string TimeSpanConvertToSmallString(TimeSpan interval)
        {
            TryLoadLocalizes();
            var result = string.Empty;
            Dictionary<PartTimeType, int> partTimes = new();
            if (interval.Days > 0)
            {
                partTimes.Add(PartTimeType.Days, interval.Days);
                partTimes.Add(PartTimeType.Hours, interval.Hours);
            }
            else if (interval.Hours > 0)
            {
                partTimes.Add(PartTimeType.Hours, interval.Hours);
                partTimes.Add(PartTimeType.Minutes, interval.Minutes);
            }
            else if (interval.Minutes > 0)
            {
                partTimes.Add(PartTimeType.Minutes, interval.Minutes);
                partTimes.Add(PartTimeType.Seconds, interval.Seconds);
            }
            else
            {
                partTimes.Add(PartTimeType.Seconds, interval.Seconds);
            }

            var requireSpace = false;
            foreach (var part in partTimes)
            {
                if (requireSpace)
                {
                    result += " ";
                }
                else
                {
                    requireSpace = true;
                }

                var localizePartTime = _partTimeLocalizes[part.Key]
                    .WithArguments(new List<object> { part.Value })
                    .GetLocalizedString();

                result += localizePartTime;
            }
            return result;
        }
    }
}
