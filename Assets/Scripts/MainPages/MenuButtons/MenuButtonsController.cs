using MainPages.MenuButtons;
using System;
using System.Collections.Generic;
using UIController.Buttons;
using UniRx;
using UnityEngine.UI;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;

namespace Ui.MainMenu.MenuButtons
{
    public class MenuButtonsController : UiController<MenuButtonsView>, IStartable, IDisposable
    {
        private readonly List<MenuButtonView> _menuButtons = new List<MenuButtonView>();
        private readonly IMenuButtonsData _menuButtonsData;
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveCommand<int> _onSwitchButton = new ReactiveCommand<int>();

        private MenuButtonView _currentButton;

        public IObservable<int> OnSwitchButton => _onSwitchButton;

        public MenuButtonsController(IMenuButtonsData menuButtonsData)
        {
            _menuButtonsData = menuButtonsData;
        }

        public void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.Content);
        }

        public void AddMenuButton<T>(string name)
            where T : IWindow
        {
            var buttonView = UnityEngine.Object.Instantiate(View.MenuButtonPrefab, View.Content);
            var buttonIndex = _menuButtons.Count;

            _menuButtons.Add(buttonView);
            if (_menuButtonsData.ButtonData.TryGetValue(name, out var data))
            {
                buttonView.Icon.sprite = data.Icon;
                buttonView.ButtonName.text = data.Text;
            }
            buttonView.Button.OnClickAsObservable().Subscribe(_ => _onSwitchButton.Execute(buttonIndex)).AddTo(_disposables);
        }

        public void SwitchMenuButton(int index)
        {
            if (_menuButtons[index] == _currentButton)
                return;

            _menuButtons[index].OnSelect();

            if (_currentButton != null)
                _currentButton.OnDiselect();

            _currentButton = _menuButtons[index];
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}