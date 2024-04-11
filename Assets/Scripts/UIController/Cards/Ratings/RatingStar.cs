using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards.Ratings
{
    public class RatingStar : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _animationTime;

        private Tween _tween;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowWithAnimation()
        {
            var oldColor = _image.color;
            oldColor.a = 0f;
            _image.color = oldColor;
            transform.localScale = Vector3.one * 5;

            _tween = DOTween.Sequence()
                .Append(_image.DOFade(1f, _animationTime))
                .Join(transform.DOScale(Vector3.one, _animationTime));
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}
