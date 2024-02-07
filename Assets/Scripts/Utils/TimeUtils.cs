using System;
using System.Globalization;

namespace Utils
{
    public static class TimeUtils
    {
        public static string GetTimerText(int time)
        {
            var minutes = time / 60;
            var seconds = time % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        public static DateTime ParseTime(string timeString)
        {
            DateTime result;
            try
            {
                result = DateTime.ParseExact(
                timeString,
                Constants.Common.DateTimeFormat,
                CultureInfo.InvariantCulture
                );
            }
            catch
            {
                result = DateTime.Parse(timeString);
            }

            return result;
        }

    }
}