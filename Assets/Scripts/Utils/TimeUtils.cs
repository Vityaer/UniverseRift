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
    }
}