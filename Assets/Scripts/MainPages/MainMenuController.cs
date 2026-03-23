using System;
using System.Collections.Generic;
using Assets.Scripts.UIController.MenuWindows;
using Common;
using MainPages.MenuButtons;
using Ui.MainMenu.MenuButtons;
using UIController.MenuWindows;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Services;

namespace MainPages
{
    public class MainMenuController : IInitializable, IDisposable
    {
        [Inject] private readonly MenuButtonsController m_menuButtonsController;
        [Inject] private readonly IUiMessagesPublisherService m_messagesPublisher;
        [Inject] private readonly IObjectResolver m_objectResolver;
        [Inject] private readonly GameController m_gameController;

        private readonly CompositeDisposable m_disposables = new();

        private readonly List<Action> m_windowOpenActions = new List<Action>();
        private readonly ReactiveCommand<int> m_onPageIndexChange = new ReactiveCommand<int>();
        
        private int m_currentPageIndex;
        
        public OpenMenuPageType OpenType { get; private set; }
        public IObservable<int> OnPageIndexChange => m_onPageIndexChange;

        public void Initialize()
        {
            m_menuButtonsController.OnSwitchButton.Subscribe(PageSwitch).AddTo(m_disposables);
            AddMenuWindow<EventWindow>();
            AddMenuWindow<CityWindow>();
            AddMenuWindow<CampaignWindow>();
            AddMenuWindow<SecondCityWindow>();
            AddMenuWindow<ArmyWindow>();
            m_gameController.OnLoadedGameData.Subscribe(_ => OpenStartPage()).AddTo(m_disposables);
        }

        private void OpenStartPage()
        {
            OpenPage<CityWindow>(1);
        }

        private void AddMenuWindow<T>() where T : IWindow
        {
            T window = m_objectResolver.Resolve<T>();
            int numWindow = m_windowOpenActions.Count;

            Action action = () => OpenPage<T>(numWindow);
            m_windowOpenActions.Add(action);
            m_menuButtonsController.AddMenuButton<T>(window.Name);
        }

        private void OpenPage<T>(int index) where T : IWindow
        {
            m_menuButtonsController.SwitchMenuButton(index);
            m_messagesPublisher.BackWindowPublisher.BackWindow();
            m_messagesPublisher.OpenWindowPublisher.OpenWindow<T>();
            m_onPageIndexChange.Execute(index);
        }

        private void PageSwitch(int newPageIndex)
        {
            OpenType = OpenMenuPageType.Click;
            m_windowOpenActions[newPageIndex]();
            m_currentPageIndex = newPageIndex;
        }

        void IDisposable.Dispose()
        {
            m_disposables?.Dispose();
        }
    }
}