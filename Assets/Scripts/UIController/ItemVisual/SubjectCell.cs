using System;
using Common;
using TMPro;
using UIController.Animations;
using UniRx;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual
{
    public class SubjectCell : UiView, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler,
        IPointerEnterHandler
    {
        public Image Image;
        public Image Background;
        public Backlight Backlight;
        public TMP_Text Amount;
        public Button Button;
        public SimpleAnimations Animations;

        private ReactiveCommand<SubjectCell> m_onSelect = new();
        private CompositeDisposable m_disposables = new();

        private ScrollRect m_scroll;

        public BaseObject Subject { get; private set; }
        public IObservable<SubjectCell> OnSelect => m_onSelect;

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(m_disposables);
        }

        public void SetScroll(ScrollRect scrollRect)
        {
            m_scroll = scrollRect;
        }

        public void SetData<T>(T baseObject) where T : BaseObject
        {
            Subject = baseObject;
            Image.enabled = true;
            Image.sprite = baseObject.Image;
            //Background.sprite = baseObject.Rating;
            Amount.text = baseObject.EqualsZero ? string.Empty : baseObject.ToString();
            //Backlight?.backlight?.SetActive(true);
        }

        private void OnClick()
        {
            if (Subject != null)
                m_onSelect.Execute(this);
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
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_scroll?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_scroll?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_scroll?.OnEndDrag(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Animations?.Squezze();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Animations?.ToDefaultSize();
        }

        protected override void OnDestroy()
        {
            m_disposables.Dispose();
            base.OnDestroy();
        }
    }
}