using TMPro;
using Ui.Misc.Widgets;
using UIController;
using UnityEngine.UI;

namespace Buildings.Mails.LetterPanels
{
    public class LetterPanelView : BasePanel
    {
        public TMP_Text LetterTopic;
        public Image Image;
        public TMP_Text SenderDate;
        public TMP_Text MainText;
        public RewardUIController RewardController;
        public Button GetRewardButton;
    }
}
