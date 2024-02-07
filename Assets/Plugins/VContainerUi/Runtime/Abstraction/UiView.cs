﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainerUi.Interfaces;

namespace VContainerUi.Abstraction
{
    public abstract class UiView : UIBehaviour, IUiView, IDisposable
    {
        protected Sequence TweenSequence;

        public List<UiView> AutoInjectObjects = new List<UiView>();

        public bool IsShow { get; private set; }

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
    }
}