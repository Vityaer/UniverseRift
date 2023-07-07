using System;
using UniRx;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;
using VContainerUi.Model;
using VContainerUi.Services;
using VContainerUi.Messages;
using Common;
using VContainer.Unity;

namespace UiExtensions.MainPages
{
    public class UiMainPageController<T> : UiController<T>, IInitializable, IDisposable
        where T : UiView
    {
        [Inject] private readonly GameController gameController;
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;
        protected CompositeDisposable Disposables = new CompositeDisposable();

        public void Initialize()
        {
            gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(Disposables);
        }

        protected void OpenBuildingOnClick<T>(Button button) where T : IPopUp
        {
            button.OnClickAsObservable().Subscribe(_ => OpenBuilding<T>()).AddTo(Disposables);
        }

        private void OpenBuilding<T>() where T : IPopUp
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<T>(openType: OpenType.Additive);
        }
        
        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
