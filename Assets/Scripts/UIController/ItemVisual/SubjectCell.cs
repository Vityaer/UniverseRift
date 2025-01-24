using Common;
using System;
using TMPro;
using UniRx;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual
{
    public class SubjectCell : UiView
    {
        public Image Image;
        public Image Background;
        public Backlight Backlight;
        public TMP_Text Amount;
        public Button Button;

        private ReactiveCommand<SubjectCell> _onSelect = new();
        private CompositeDisposable _disposables = new();

        public BaseObject Subject { get; private set; }
        public IObservable<SubjectCell> OnSelect => _onSelect;

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(_disposables);
        }

        public void SetData<T>(T baseObject) where T : BaseObject
        {
            Subject = baseObject;
            Image.enabled = true;
            Image.sprite = baseObject.Image;
            //Background.sprite = baseObject.Rating;
            Amount.text = baseObject.EqualsZero ? string.Empty : baseObject.ToString();
            //Backlight?.backlight?.SetActive(true);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        private void OnClick()
        {
            if (Subject != null)
            {
                _onSelect.Execute(this);
            }
        }

        public void Clear()
        {
            Image.enabled = false;
            Amount.text = string.Empty;
            Subject = null;
            //Backlight?.backlight?.SetActive(false);
        }

        public void Disable()
        {
            Clear();
            gameObject.SetActive(false);
        }


        protected override void OnDestroy()
        {
            _disposables.Dispose();
            base.OnDestroy();
        }
    }
}
