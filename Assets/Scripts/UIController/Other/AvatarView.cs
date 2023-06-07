using UnityEngine;
using UnityEngine.UI;

namespace UController.Other
{
    public class AvatarView : MonoBehaviour
    {
        public Image mainImage, borderImage;
        public int levelVip;

        public void SetAvatar(Sprite newAvatar)
        {
            mainImage.sprite = newAvatar;
        }
    }
}