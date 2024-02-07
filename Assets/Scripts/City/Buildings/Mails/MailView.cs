using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Mails
{
    public class MailView : BasePanel
    {
        public Button DeleteAllButton;
        public Button GetAllButton;
        public Button AdminFilterButton;
        public Button PlayerFilterButton;
        public ScrollRect Scroll;
        public RectTransform Content;
        public LetterView LetterPrefab;
    }
}
