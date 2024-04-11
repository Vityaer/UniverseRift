using Common;
using Models.Common;
using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Services;

namespace City.Buildings.Abstractions
{
    public abstract class BaseBuilding<T> : UiController<T>, IStartable, IWindow, IDisposable
        where T : BaseBuildingView
    {
        [Inject] protected readonly CommonGameData CommonGameData;
        [Inject] protected readonly IUiMessagesPublisherService MessagesPublisher;
        [Inject] protected readonly GameController GameController;
        [Inject] protected readonly IObjectResolver Resolver;

        protected CompositeDisposable Disposables = new CompositeDisposable();
        private int levelForAvailableBuilding = 0;

        public string Name => throw new NotImplementedException();

        public void Start()
        {
            AutoInject();
            OnStart();
            View.ButtonCloseBuilding?.OnClickAsObservable().Subscribe(_ => Close()).AddTo(Disposables);
            GameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(Disposables);
        }

        private void AutoInject()
        {
            foreach (var obj in View.AutoInjectObjects)
            {
                if (Resolver == null)
                    Debug.Log($"{View.gameObject.name}");
                Resolver.Inject(obj);
            }
        }

        public virtual void Open()
        {
            if (AvailableFromLevel())
            {
                OpenPage();
            }
        }

        protected bool AvailableFromLevel()
        {
            bool result = CommonGameData.PlayerInfoData.Level >= levelForAvailableBuilding;
            if (result == false)
            {
                //MessageController.Instance.ShowErrorMessage($"Откроется на {levelForAvailableBuilding} уровне");
            }

            return result;
        }

        public virtual void Close()
        {
            ClosePage();
            MessagesPublisher.BackWindowPublisher.BackWindow();
        }

        virtual protected void OnStart() { }
        virtual protected void OpenPage() { }
        virtual protected void ClosePage() { }

        public virtual void Dispose()
        {
            Disposables.Dispose();
        }
    }
}