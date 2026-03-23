using System;
using Common;
using Models.Common;
using Plugins.VContainerUi.Runtime.Abstraction;
using Ui.Misc.Widgets;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Services;

namespace UiExtensions.Panels
{
    public abstract class UiPanelController<T> : UiController<T>, IInitializable, IStartable, IPopUp, IDisposable
        where T : BasePanel
    {
        [Inject] protected readonly CommonGameData CommonGameData;
        [Inject] protected readonly GameController GameController;
        [Inject] protected readonly IUiMessagesPublisherService MessagesPublisher;
        [Inject] protected readonly IObjectResolver Resolver;

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
            if (Resolver == null)
            {
                UnityEngine.Debug.LogError($"{View.gameObject.name} without a resolver");
                return;
            }
            
            foreach (var obj in View.AutoInjectObjects)
            {
                Resolver.Inject(obj);
            }
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