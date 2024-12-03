using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UIController.Buttons
{
    public class MenuButtonView : UiView
    {
        public Button Button;
        public Image Icon;
        public Image Background;
        public LocalizeStringEvent ButtonName;
        public MenuButtonHelper ButtonHelper;

        public Sprite SelectedMenuButton;
        public Sprite DeselectedMenuButton;

        private readonly CompositeDisposable _disposables = new();

        public void OnSelect()
        {
            ButtonHelper.Select();
            Background.sprite = SelectedMenuButton;
            ButtonName.gameObject.SetActive(true);
            Button.interactable = false;
        }

        public void OnDiselect()
        {
            ButtonHelper.Diselect();
            Background.sprite = DeselectedMenuButton;
            ButtonName.gameObject.SetActive(false);
            Button.interactable = true;
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
            base.Dispose();
        }
    }
}
