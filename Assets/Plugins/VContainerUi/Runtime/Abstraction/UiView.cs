﻿using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainerUi.Interfaces;

namespace VContainerUi.Abstraction
{
    public abstract class UiView : SerializedMonoBehaviour, IUiView, IDisposable
    {
        protected Sequence TweenSequence;

        public List<UiView> AutoInjectObjects = new List<UiView>();

        public bool IsShow { get; private set; }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        void IUiView.Show()
        {
            gameObject.SetActive(true);
            IsShow = true;
            OnShow();
        }

        protected virtual void OnShow()
        {
        }

        void IUiView.Hide()
        {
            OnHide();
        }

        protected virtual void OnHide()
        {
            gameObject.SetActive(false);
            IsShow = false;
        }

        void IUiView.SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        void IUiView.SetOrder(int index)
        {
            var parent = transform.parent;
            if (parent == null)
                return;
            var childCount = parent.childCount - 1;
            transform.SetSiblingIndex(childCount - index);
        }

        IUiElement[] IUiView.GetUiElements()
        {
            return gameObject.GetComponentsInChildren<IUiElement>();
        }

        void IUiView.Destroy()
        {
            Destroy(gameObject);
            TweenSequence.Kill();
        }

        public virtual void Dispose()
        {
            TweenSequence.Kill();
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }
    }
}