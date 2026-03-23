using City.Buildings.CityButtons.EventAgent;
using System;
using Common.Db.CommonDictionaries;
using LocalizationSystems;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainerUi.Abstraction;

namespace UiExtensions.Misc
{
    public abstract class ScrollableUiView<T> : UiView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] protected Button Button;

        protected ScrollRect Scroll;
        protected T Data;
        protected CompositeDisposable Disposable = new();
        protected ReactiveCommand<ScrollableUiView<T>> _onSelect = new ReactiveCommand<ScrollableUiView<T>>();

        protected CommonDictionaries CommonDataDictionaries;
        protected ILocalizationSystem LocalizationSystem;
        
        public IObservable<ScrollableUiView<T>> OnSelect => _onSelect;
        public T GetData => Data;

        protected override void Start()
        {
            Button?.OnClickAsObservable().Subscribe(_ => _onSelect.Execute(this)).AddTo(Disposable);
        }

        public virtual void SetData(T data, ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            Data = data;
            Scroll = scrollRect;
            CommonDataDictionaries = commonDictionaries.ThrowIfArgumentNull();
            OnSetData();
        }

        protected virtual void OnSetData()
        {
        }

        public void SetLocalizationSystem(ILocalizationSystem localizationSystem)
        {
            LocalizationSystem = localizationSystem;
        }

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

        public virtual void SetStatus(ScrollableViewStatus received) { }


        public override void Dispose()
        {
            Disposable?.Dispose();
            Disposable = null;
            base.Dispose();
        }


        protected override void OnDestroy()
        {
            Disposable?.Dispose();
            Disposable = null;
            base.OnDestroy();
        }
    }
}
