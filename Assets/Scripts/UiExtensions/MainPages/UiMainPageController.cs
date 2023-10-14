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
using City.Buildings.UiBuildings;

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

        protected void RegisterBuilding<V>(V controller, BuildingVisual building) where V : IPopUp
        {
            building.SubscribeOnNews<V>(controller);
            OpenBuildingOnClick<V>(building.BuildingButton);
        }

        private void OpenBuildingOnClick<V>(Button button) where V : IPopUp
        {
            button.OnClickAsObservable().Subscribe(_ => OpenBuilding<V>()).AddTo(Disposables);
        }

        private void OpenBuilding<T>() where T : IPopUp
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<T>(openType: OpenType.Exclusive);
        }

        public override void OnShow()
        {
            base.OnShow();
        }
        public override void OnHide()
        {
            base.OnHide();
        }
        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
