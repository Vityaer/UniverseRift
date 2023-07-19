using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ui.Misc.CustomComponents
{
    public class TextsAutoSize : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();
        [HorizontalGroup("1")] [SerializeField] [Min(0)] private float _minFontSize;
        [HorizontalGroup("1")] [SerializeField] [Min(0)] private float _maxFontSize;

        private TextMeshProUGUI _meshMaxCharacterCount;
        private int _maxCountChar = 0;
        private float _fontSize;

        private void Start()
        {
            RefreshAll();
        }

#if UNITY_EDITOR
        private void Update()
        {
            CheckMaxCountChar();
        }
#endif

        private void CheckMaxCountChar()
        {
            if (_texts.Count == 0)
                return;

            var maxCharacterCount = 0;
            foreach (var textComponent in _texts)
            {
                if (textComponent.text.Length > maxCharacterCount)
                {
                    maxCharacterCount = textComponent.text.Length;
                    _meshMaxCharacterCount = textComponent;
                }
            }

            if (maxCharacterCount != _maxCountChar)
            {
                _maxCountChar = maxCharacterCount;
                RecalculateFontSize();
                Refresh();
            }
        }

        private void RecalculateFontSize()
        {
            var width = _meshMaxCharacterCount.rectTransform.rect.width;
            _fontSize = Mathf.Clamp(width / _maxCountChar * 2, _minFontSize, _maxFontSize);
        }

        private void Refresh()
        {
            foreach (var text in _texts)
            {
                text.fontSize = _fontSize;
            }
        }

        private void OnValidate()
        {
            _maxCountChar = 0;
            CheckMaxCountChar();
        }

        [ContextMenu("RefreshAll")]
        private void RefreshAll()
        {
            if (_texts.Count == 0)
                return;

            CheckMaxCountChar();
            RecalculateFontSize();
            Refresh();
        }
    }
}