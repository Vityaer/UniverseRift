using City.Buildings.PlayerPanels;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace MainPages.MenuHud
{
    public class MenuHudController : UiController<MenuHudView>, IInitializable, IDisposable
    {
        [Inject] private readonly IObjectResolver _resolver;
        [Inject] protected readonly IUiMessagesPublisherService _messagesPublisher;

        private CompositeDisposable _disposables = new CompositeDisposable();

        public void Initialize()
        {
            _resolver.Inject(View.GoldObserverResource);
            _resolver.Inject(View.DiamondObserverResource);
            View.PlayerPanelButton.OnClickAsObservable().Subscribe(_ => OpenPlayerPanel()).AddTo(_disposables);
        }

        private void OpenPlayerPanel()
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<PlayerPanelController>(openType: OpenType.Additive);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
