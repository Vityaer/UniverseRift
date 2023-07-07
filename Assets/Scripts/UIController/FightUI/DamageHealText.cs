using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UIController.FightUI
{
    public class DamageHealText : MonoBehaviour
    {
        public Vector2 Delta = new Vector2(0, 100f);
        public float Speed = 1f;

        [SerializeField] private TextMeshProUGUI _textComponent;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Color _colorDamage;
        [SerializeField] private Color _colorHeal;

        private bool _inWork = false;
        private Sequence _sequence;

        public bool InWork => _inWork;

        public void PlayDamage(float damage, Vector2 pos)
        {
            PlayInfo(damage, pos, _colorDamage);
        }

        public void PlayHeal(float heal, Vector2 pos)
        {
            PlayInfo(heal, pos, _colorHeal);
        }

        private void PlayInfo(float amount, Vector2 pos, Color color)
        {
            if (_inWork == false)
            {
                _inWork = true;
                _textComponent.color = _colorDamage;
                _textComponent.text = $"{Mathf.RoundToInt(amount)}";

                gameObject.SetActive(true);
                _rectTransform.anchoredPosition = pos;

                _sequence = DOTween.Sequence()
                    .Append(_textComponent.DOFade(1f, 0.05f))
                    .Insert(0f, _rectTransform.DOAnchorPos(new Vector2(pos.x + Delta.x, pos.y + Delta.y), Speed).OnComplete(Disable));
            }
        }

        public void Disable()
        {
            _textComponent.DOFade(0f, 0.25f).OnComplete(ClearText);
        }

        public void ClearText()
        {
            gameObject.SetActive(false);
            _textComponent.text = string.Empty;
            _inWork = false;
        }
    }
}