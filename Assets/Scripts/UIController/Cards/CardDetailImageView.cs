using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UIController.Cards
{
    public class CardDetailImageView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetData(string spritePath)
        {
            _image.sprite = SpriteUtils.LoadSprite(spritePath);
        }
    }
}