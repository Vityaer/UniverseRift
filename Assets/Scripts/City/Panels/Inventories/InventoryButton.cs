using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.Inventories
{
    public class InventoryButton : MonoBehaviour
    {
        public Button Button;
        public Image Image;

        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private float _animationTime;
        private Tween _tween;
        
        public void Select()
        {
            _tween.Kill();
            _tween = Image.DOColor(_selectedColor, _animationTime).SetEase(Ease.Linear);
        }

        public void Unselect()
        {
            _tween.Kill();
            _tween = Image.DOColor(_unselectedColor, _animationTime).SetEase(Ease.Linear);
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}