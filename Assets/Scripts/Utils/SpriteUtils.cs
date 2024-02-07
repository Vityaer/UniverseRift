using UnityEngine;

namespace Utils
{
    public class SpriteUtils
    {
        public static Sprite LoadSprite(string spritePath)
        {
            if (string.IsNullOrEmpty(spritePath))
                return null;

            var path = spritePath.ReplaceForResources();
            path = path.Replace(".png", "");
            var sprite = Resources.Load<Sprite>(path);
            return sprite;
        }
    }
}