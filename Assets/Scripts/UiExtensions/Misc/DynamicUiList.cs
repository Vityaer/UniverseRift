using System;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.DataCheckUtils;

namespace UiExtensions.Misc
{
    public class DynamicUiList<T, TV> : IDisposable
        where T : ScrollableUiView<TV>
    {
        private readonly WrapperPool<T> m_pool;
        private readonly List<T> m_views = new();
        private readonly CompositeDisposable m_disposables = new();
        private readonly ScrollRect m_scrollRect;
        private readonly Action<T> m_onSelect;
        private readonly Action<T> m_onCreate;
        private readonly CommonDictionaries m_commonDictionaries;

        public List<T> Views => m_views;

        public DynamicUiList(T prefab, Transform content, ScrollRect scrollRect, CommonDictionaries commonDictionaries, Action<T> onSelect = null, Action<T> onCreate = null)
        {
            m_pool = new WrapperPool<T>(prefab, OnViewCreate, content);
            m_scrollRect = scrollRect.ThrowIfArgumentNull();
            m_commonDictionaries = commonDictionaries;
            m_onSelect = onSelect;
            m_onCreate = onCreate;
        }

        public void ShowDatas(List<TV> datas)
        {
            ClearList();
            int i = 0;
            foreach (TV data in datas)
            {
                AddElement(data, i);
                i++;
            }
        }

        public void ClearList()
        {
            for (var index = m_views.Count - 1; index >= 0; index--)
                m_pool.Release(m_views[index]);

            m_views.Clear();
        }

        public T AddElement(TV element, int index = -1)
        {
            T view = m_pool.Get();
            view.SetData(element, m_scrollRect, m_commonDictionaries);
            m_views.Add(view);
            if (index != -1)
            {
                view.transform.SetSiblingIndex(index);
            }

            return view;
        }

        public void RemoveElement(T element)
        {
            m_views.Remove(element);
            m_pool.Release(element);
        }

        private void OnViewCreate(T view)
        {
            view.OnSelect.Subscribe(OnViewSelect).AddTo(m_disposables);
            m_onCreate?.Invoke(view);
        }

        private void OnViewSelect(ScrollableUiView<TV> view)
        {
            m_onSelect?.Invoke(view as T);
        }
        
        public void Dispose()
        {
            m_disposables.Dispose();
        }
    }
}
