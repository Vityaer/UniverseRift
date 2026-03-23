using System;

namespace Utils.DataCheckUtils
{
    public static class NullRefUtils
    {
        public static T ThrowIfArgumentNull<T>(this T arg) where T : class
        {
            if (arg == null)
            {
                throw new ArgumentNullException($"Name of type - {typeof(T).Name}");
            }

            return arg;
        }
        
        public static string ThrowIfArgumentNullOrEmpty(this string arg, string paramName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentException($"String {{{paramName}}} is null or empty!");
            }

            return arg;
        }
    }
}