using UnityEngine;
using UnityEngine.UI;
public class AvatarView : MonoBehaviour
{
    public Image mainImage, borderImage;
    public int levelVip;

    public void SetAvatar(Sprite newAvatar)
    {
        mainImage.sprite = newAvatar;
    }
}
