using System;
using System.Threading;
using City.Panels.BlackedPanels;
using City.Panels.DialogPanels;
using City.Panels.TutorialTouchPanels;
using Cysharp.Threading.Tasks;
using MainPages.MenuButtons;
using Sirenix.OdinInspector;
using Ui.MainMenu.MenuButtons;
using UIController.Buttons;
using UniRx;
using UnityEngine;
using VContainer;

namespace Models.Tutorials
{
    [Serializable]
    public class OpenPageTutorial : ITutorialStage
    {
        [Inject] private BlackedPanelController m_blackedPanelController;
        [Inject] private MenuButtonsController m_menuButtonsController;
        [Inject] private MenuButtonsView m_menuButtonsView;
        [Inject] private TutorialTouchController m_tutorialTouchController;
        [Inject] private DialogPanelController m_dialogPanelController;
        
        [LabelWidth(200)]
        public string StartDialogId = string.Empty;
        
        [LabelWidth(200)]
        public int TargetPage;
        
        public async UniTask PlayAsync(CancellationToken token)
        {
            await m_dialogPanelController.ShowDialog(StartDialogId, token);
            
            MenuButtonView button = m_menuButtonsController.MenuButtons[TargetPage];
            await m_blackedPanelController.SelectView(button.GetComponent<RectTransform>());
            m_tutorialTouchController.ShowTouch(button.GetComponent<RectTransform>());
            
            await m_menuButtonsController.OnSwitchButton
                .Where(page => page == TargetPage)
                .First()
                .ToUniTask(cancellationToken: token);
            
            m_tutorialTouchController.HideTouch();
        }
    }
}