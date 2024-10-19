using DG.Tweening;
using Models.Misc;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Mails
{
    public class LetterView : ScrollableUiView<LetterData>
    {
        private const int FIRST_CHARS_COUNT = 50;

        [SerializeField] private TMP_Text _letterTopic;
        [SerializeField] private TMP_Text _letterFirstChars;
        [SerializeField] private Image _readStatusImage;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _senderDate;

        [Header("Animation")]
        [SerializeField] private float _readFadeAlpha;
        [SerializeField] private float _readAnimationTime;
        [SerializeField] private Sprite _closeLetterSprite;
        [SerializeField] private Sprite _openLetterSprite;

        private Tween _tween;

        public override void SetData(LetterData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            UpdateUi();
        }

        private void UpdateUi()
        {
            ShowReadStatus();

            _letterTopic.text = Data.Topic;
            _senderDate.text = Data.CreateDateTime;


            if (_letterFirstChars != null)
            {
                var firstChars = Data.Message;
                if (firstChars.Length > FIRST_CHARS_COUNT)
                {
                    firstChars = string.Concat(firstChars.Substring(0, FIRST_CHARS_COUNT), "...");
                }

                _letterFirstChars.text = Data.Topic;
                _letterFirstChars.text = firstChars;
            }
        }

        public void ShowReadStatus()
        {
            if (Data.IsOpened)
            {
                _tween.Kill();
                _tween = _background.DOFade(_readFadeAlpha, _readAnimationTime);
                _readStatusImage.sprite = _openLetterSprite;
            }
            else
            {
                _tween.Kill();
                _readStatusImage.sprite = _closeLetterSprite;
                _tween = _background.DOFade(1f, _readAnimationTime);
            }
        }

        protected override void OnDestroy()
        {
            _tween.Kill();
            base.OnDestroy();
        }
    }
}
