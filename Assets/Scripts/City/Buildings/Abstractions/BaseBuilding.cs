using City.Panels.SubjectPanels.Common;
using Common;
using Models.Common;
using System;
using City.Panels.Helps;
using LocalizationSystems;
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
        [Inject] protected readonly SubjectDetailController SubjectDetailController;

        [Inject] protected readonly ILocalizationSystem LocalizationSystem;
        [Inject] protected readonly HelpPanelController HelpPanelController;
        
        protected CompositeDisposable Disposables = new();
        private int _levelForAvailableBuilding = 0;

        private string HelpLocalizeStringId => $"{Name}HelpMainMessage";
        
        public string Name => this.GetType().Name;

        public void Start()
        {
            AutoInject();
            OnStart();
            View.HelpButton?.OnClickAsObservable().Subscribe(_ => OpenHelp()).AddTo(Disposables);
            View.ButtonCloseBuilding?.OnClickAsObservable().Subscribe(_ => Close()).AddTo(Disposables);
            GameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(Disposables);
            
            View.HelpButton?.gameObject.SetActive(LocalizationSystem.ExistLocaleId(HelpLocalizeStringId));
        }

        protected virtual void OpenHelp()
        {
            if (View.HelpContainer != null)
            {
                HelpPanelController.OpenHelp(View.HelpContainer);
            }
            else
            {
                HelpPanelController.OpenHelp(HelpLocalizeStringId);
            }

        }

        private void AutoInject()
        {
            foreach (var obj in View.AutoInjectObjects)
            {
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
            bool result = CommonGameData.PlayerInfoData.Level >= _levelForAvailableBuilding;
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