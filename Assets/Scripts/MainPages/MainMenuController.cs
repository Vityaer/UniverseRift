using Assets.Scripts.UIController.MenuWindows;
using System;
using System.Collections.Generic;
using Ui.MainMenu.MenuButtons;
using UIController.GameSystems;
using UIController.MenuWindows;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Services;

namespace Ui.MainMenu
{
    public class MainMenuController : IInitializable, IDisposable
    {

        private readonly MenuButtonsController _menuButtonsController;
        private readonly IUiMessagesPublisherService _messagesPublisher;
        private readonly IObjectResolver _objectResolver;
        private readonly CompositeDisposable _disposables = new();

        public bool CanSwipe { get; set; } = true;

        public OpenMenuPageType OpenType { get; private set; }

        private int _currentPageIndex;

        private List<Action> _windowOpenActions = new List<Action>();
        private ReactiveCommand<int> _onPageIndexChange = new ReactiveCommand<int>();

        public IObservable<int> OnPageIndexChange => _onPageIndexChange;

        public MainMenuController(
            MenuButtonsController menuButtonsController,
            IUiMessagesPublisherService messagesPublisher,
            IObjectResolver objectResolver
            )
        {
            _objectResolver = objectResolver;
            _messagesPublisher = messagesPublisher;
            _menuButtonsController = menuButtonsController;
        }

        public void Initialize()
        {
            _menuButtonsController.OnSwitchButton.Subscribe(PageSwitch).AddTo(_disposables);
            AddMenuWindow<EventWindow>();
            AddMenuWindow<CityWindow>();
            AddMenuWindow<CampaignWindow>();
            AddMenuWindow<ArmyWindow>();

            OpenPage<CityWindow>(1);
        }

        private void AddMenuWindow<T>() where T : IWindow
        {
            var window = _objectResolver.Resolve<T>();
            var numWindow = _windowOpenActions.Count;

            Action action = () => OpenPage<T>(numWindow);
            _windowOpenActions.Add(action);
            _menuButtonsController.AddMenuButton<T>(window.Name);
        }

        private void OpenPage<T>(int index) where T : IWindow
        {
            _menuButtonsController.SwitchMenuButton(index);
            _messagesPublisher.BackWindowPublisher.BackWindow();
            _messagesPublisher.OpenWindowPublisher.OpenWindow<T>();
            _onPageIndexChange.Execute(index);
        }

        public void PageSwitch(int newPageIndex)
        {
            OpenType = OpenMenuPageType.Click;
            _windowOpenActions[newPageIndex]();
            _currentPageIndex = newPageIndex;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}