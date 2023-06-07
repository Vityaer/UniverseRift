using City.General;
using UnityEngine;

namespace UIController.Actions
{
    public class ActionSheet : MainPage
    {
        [SerializeField] private Canvas canvasActionUI;
        [SerializeField] private GameObject background;

        public override void Open()
        {
            base.Open();
            canvasActionUI.enabled = true;
            BackgroundController.Instance.OpenBackground(background);
        }

        public override void Close()
        {
            canvasActionUI.enabled = false;
        }
    }
}