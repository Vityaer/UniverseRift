using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UIController.Buttons
{
    public class MenuButtonView : UiView, IDisposable
    {
        public Button Button;
        public Image Icon;
        public Image Background;
        public TextMeshProUGUI ButtonName;
        public MenuButtonHelper ButtonHelper;

        public Sprite SelectedMenuButton;
        public Sprite DeselectedMenuButton;

        private readonly CompositeDisposable _disposables = new();

        private void Awake()
        {
            OnDiselect();
        }

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

        public new void Dispose()
        {
            _disposables?.Dispose();
            base.Dispose();
        }
    }
}
