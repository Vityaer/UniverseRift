using Buildings.Mails.LetterPanels;
using Models.Misc;
using System;
using UiExtensions.Scroll.Interfaces;

namespace City.Buildings.Mails.LetterPanels
{
    public class LetterPanelController : UiPanelController<LetterPanelView>
    {
        private LetterData _letterData;

        public void ShowLetter(LetterView letterView)
        {
            _letterData = letterView.GetData;
            UpdateUi();
        }

        private void UpdateUi()
        {
        }
    }
}
