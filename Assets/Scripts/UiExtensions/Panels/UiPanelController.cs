using Common;
using System;
using System.Diagnostics;
using Ui.Misc.Widgets;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Services;

namespace UiExtensions.Scroll.Interfaces
{
    public abstract class UiPanelController<T> : UiController<T>, IInitializable, IStartable, IPopUp, IDisposable
        where T : BasePanel
    {
        [Inject] protected readonly GameController GameController;
        [Inject] protected readonly IUiMessagesPublisherService _messagesPublisher;
        protected readonly CompositeDisposable Disposables = new CompositeDisposable();

        public void Initialize()
        {
            GameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(Disposables);
        }

        public virtual void Start()
        {
            View.CloseButton?.OnClickAsObservable().Subscribe(_ => Close()).AddTo(Disposables);
            View.DimedButton?.OnClickAsObservable().Subscribe(_ => Close()).AddTo(Disposables);
        }

        public override void OnShow()
        {
            UnityEngine.Debug.Log($"show {View.gameObject.name}");
            base.OnShow();
        }

        protected virtual void Close()
        {
            UnityEngine.Debug.Log($"close {View.gameObject.name}");
            _messagesPublisher.BackWindowPublisher.BackWindow();
        }

        public virtual void Dispose()
        {
            Disposables.Dispose();
        }


    }
}