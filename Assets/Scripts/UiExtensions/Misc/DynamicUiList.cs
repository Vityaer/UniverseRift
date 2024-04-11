using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UiExtensions.Misc
{
    public class DynamicUiList<T, V> : IDisposable
        where T : ScrollableUiView<V>
    {
        private WrapperPool<T> _pool;
        private List<T> _views = new();
        private CompositeDisposable _disposables = new();
        private ScrollRect _scrollRect;
        private Action<T> _onSelect;
        private Action<T> _onCreate;

        public List<T> Views => _views;

        public DynamicUiList(T prefab, Transform content, ScrollRect scrollRect, Action<T> onSelect = null, Action<T> onCreate = null)
        {
            _pool = new WrapperPool<T>(prefab, OnViewCreate, content);
            _scrollRect = scrollRect;
            _onSelect = onSelect;
            _onCreate = onCreate;
        }

        public void ShowDatas(List<V> datas)
        {
            ClearList();
            foreach (var data in datas)
            {
                AddElement(data);
            }
        }

        public void ClearList()
        {
            for (var index = _views.Count - 1; index >= 0; index--)
                _pool.Release(_views[index]);

            _views.Clear();
        }

        public T AddElement(V element)
        {
            var view = _pool.Get();
            view.SetData(element, _scrollRect);
            _views.Add(view);
            return view;
        }

        public void RemoveElement(T element)
        {
            _views.Remove(element);
            _pool.Release(element);
        }

        private void OnViewCreate(T view)
        {
            view.OnSelect.Subscribe(OnViewSelect).AddTo(_disposables);
            _onCreate?.Invoke(view);
        }

        private void OnViewSelect(ScrollableUiView<V> view)
        {
            _onSelect?.Invoke(view as T);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
