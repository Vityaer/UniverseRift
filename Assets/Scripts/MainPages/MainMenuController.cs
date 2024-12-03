using Assets.Scripts.UIController.MenuWindows;
using Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
    public class MainMenuController : IInitializable, IStartable, IDisposable
    {
        [Inject] private readonly MenuButtonsController _menuButtonsController;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;
        [Inject] private readonly IObjectResolver _objectResolver;
        [Inject] private readonly GameController _gameController;

        private readonly CompositeDisposable _disposables = new();

        public OpenMenuPageType OpenType { get; private set; }

        private int _currentPageIndex;

        private List<Action> _windowOpenActions = new List<Action>();
        private ReactiveCommand<int> _onPageIndexChange = new ReactiveCommand<int>();

        public IObservable<int> OnPageIndexChange => _onPageIndexChange;

        public void Initialize()
        {
            _menuButtonsController.OnSwitchButton.Subscribe(PageSwitch).AddTo(_disposables);
            AddMenuWindow<EventWindow>();
            AddMenuWindow<CityWindow>();
            AddMenuWindow<CampaignWindow>();
            AddMenuWindow<SecondCityWindow>();
            AddMenuWindow<ArmyWindow>();
            _gameController.OnLoadedGameData.Subscribe(_ => OpenStartPage()).AddTo(_disposables);
        }


        public void Start()
        {
            OpenStartPage();
        }

        public void OpenStartPage()
        {
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