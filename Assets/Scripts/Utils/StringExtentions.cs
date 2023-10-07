using UnityEngine;

namespace Utils
{
    public static class StringExtensions
    {
        public static string ReplaceForResources(this string path)
        {
            path = path.Replace("Assets/Resources/", "");
            return path;
        }

        public static Sprite LoadSpriteFromResources(this string path)
        {
            path = path.Replace("Assets/Resources/", "");
            var pointPosition = path.LastIndexOf('.');
            path = path.Substring(0, pointPosition);

            var sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
                Debug.LogError($"Icon not found, path: {path}");

            return sprite;
        }
    }
}
