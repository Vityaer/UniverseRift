using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards
{
    public class VocationView : MonoBehaviour
    {
        [SerializeField] private Image imageVocation;
        public void SetData(string newVocation)
        { imageVocation.sprite = SystemSprites.Instance.GetVocationImage(newVocation); }
    }
}