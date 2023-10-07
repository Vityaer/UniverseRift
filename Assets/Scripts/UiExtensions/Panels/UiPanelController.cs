using Common;
using Models.Common;
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
        [Inject] protected readonly CommonGameData CommonGameData;
        [Inject] protected readonly GameController GameController;
        [Inject] protected readonly IUiMessagesPublisherService MessagesPublisher;
        [Inject] protected readonly IObjectResolver _resolver;
        protected readonly CompositeDisposable Disposables = new CompositeDisposable();

        public void Initialize()
        {
            if (CommonGameData == null) { UnityEngine.Debug.Log($"wtf {this.GetType()}"); }
            GameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(Disposables);
        }

        public virtual void Start()
        {
            AutoInject();
            View.CloseButton?.OnClickAsObservable().Subscribe(_ => Close()).AddTo(Disposables);
            View.DimedButton?.OnClickAsObservable().Subscribe(_ => Close()).AddTo(Disposables);
        }

        private void AutoInject()
        {
            foreach (var obj in View.AutoInjectObjects)
            {
                if (_resolver == null)
                    UnityEngine.Debug.Log($"{View.gameObject.name}");
                _resolver.Inject(obj);
            }
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        protected virtual void Close()
        {
            MessagesPublisher.BackWindowPublisher.BackWindow();
        }

        public virtual void Dispose()
        {
            Disposables.Dispose();
        }
    }
}