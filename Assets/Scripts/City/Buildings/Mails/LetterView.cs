using Models.Misc;
using System;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Mails
{
    public class LetterView : ScrollableUiView<LetterData>
    {
        [SerializeField] private TMP_Text _letterTopic;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _senderDate;

        public override void SetData(LetterData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            UpdateUi();
        }

        private void UpdateUi()
        {
            throw new NotImplementedException();
        }
    }
}
