using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards
{
    public class VocationView : MonoBehaviour
    {
        [SerializeField] private Image _imageVocation;
        public void SetData(string newVocation)
        { _imageVocation.sprite = SystemSprites.Instance.GetVocationImage(newVocation); }
    }
}