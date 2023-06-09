using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards
{
    public class RaceView : MonoBehaviour
    {
        [SerializeField] private Image _imageRace;

        private string _currentRace;

        public void SetData(string newRace)
        {
            _imageRace.sprite = SystemSprites.Instance.GetSprite(newRace);
            _currentRace = newRace;
        }

    }
}