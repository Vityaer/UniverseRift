namespace Utils
{
    public static class StringExtensions
    {
        public static string ReplaceForResources(this string path)
        {
            path = path.Replace("Assets/Resources/", "");
            return path;
        }
    }
}
