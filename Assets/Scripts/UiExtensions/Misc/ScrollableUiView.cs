using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UiExtensions.Misc
{
    public abstract class ScrollableUiView<T> : UiView, IBeginDragHandler, IDragHandler, IEndDragHandler, IDisposable
    {
        [SerializeField] protected ScrollRect Scroll;
        [SerializeField] private Button Button;

        protected T Data;
        protected CompositeDisposable Disposable;
        private ReactiveCommand<ScrollableUiView<T>> _onSelect = new ReactiveCommand<ScrollableUiView<T>>();

        public IObservable<ScrollableUiView<T>> OnSelect => _onSelect;

        private void Start()
        {
            //Button.OnClickAsObservable().Subscribe(_ => _onSelect.Execute(this)).AddTo(Disposable);
        }

        public abstract void SetData(T data, ScrollRect scrollRect);

        public void OnBeginDrag(PointerEventData eventData)
        {
            Scroll.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Scroll.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Scroll.OnEndDrag(eventData);
        }

        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}
