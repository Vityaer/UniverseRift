using System;
using System.Collections.Generic;
using LocalizationSystems;
using Plugins.VContainerUi.Runtime.Abstraction;
using Ui.MainMenu.MenuButtons;
using UIController.Buttons;
using UniRx;
using UnityEngine.UI;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;

namespace MainPages.MenuButtons
{
    public class MenuButtonsController : UiController<MenuButtonsView>, IStartable, IDisposable
    {
        private readonly ILocalizationSystem m_localizationSystem;
        private readonly List<MenuButtonView> m_menuButtons = new List<MenuButtonView>();
        private readonly IMenuButtonsData m_menuButtonsData;
        private readonly CompositeDisposable m_disposables = new();
        private readonly ReactiveCommand<int> m_onSwitchButton = new ReactiveCommand<int>();

        private MenuButtonView m_currentButton;

        public IObservable<int> OnSwitchButton => m_onSwitchButton;
        public List<MenuButtonView> MenuButtons => m_menuButtons;

        public MenuButtonsController(IMenuButtonsData menuButtonsData, ILocalizationSystem localizationSystem)
        {
            m_menuButtonsData = menuButtonsData;
            m_localizationSystem = localizationSystem;
        }

        public void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.Content);
        }

        public void AddMenuButton<T>(string id)
            where T : IWindow
        {
            MenuButtonView buttonView = UnityEngine.Object.Instantiate(View.MenuButtonPrefab, View.Content);
            int buttonIndex = m_menuButtons.Count;

            m_menuButtons.Add(buttonView);
            MenuButtonData data = m_menuButtonsData.ButtonData.Find(buttonData => buttonData.Id == id);
            
            if (data != null)
            {
                buttonView.Icon.sprite = data.Icon;
                buttonView.ButtonName.StringReference = m_localizationSystem
                    .GetLocalizedContainer($"MenuButton{data.Text}Name");
            }
            
            buttonView.Button.OnClickAsObservable()
                .Subscribe(_ => m_onSwitchButton.Execute(buttonIndex))
                .AddTo(m_disposables);
        }

        public void SwitchMenuButton(int index)
        {
            if (m_menuButtons[index] == m_currentButton)
                return;

            m_menuButtons[index].OnSelect();

            if (m_currentButton != null)
                m_currentButton.OnDiselect();

            m_currentButton = m_menuButtons[index];
        }

        public void Dispose()
        {
            m_disposables?.Dispose();
        }
    }
}