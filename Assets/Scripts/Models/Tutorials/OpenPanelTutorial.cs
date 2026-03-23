using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using City.Panels.BlackedPanels;
using City.Panels.DialogPanels;
using City.Panels.TutorialTouchPanels;
using Cysharp.Threading.Tasks;
using MainPages.City;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainerUi.Interfaces;

namespace Models.Tutorials
{
    public class OpenPanelTutorial : ITutorialStage
    {
        public string OpenTavernDialogId;
        
        private string[] AllPanelsName => InheritanceUtils.GetAllTypeNames<IUiController>();
        [Inject] private TutorialTouchController m_tutorialTouchController;
        [Inject] private DialogPanelController m_dialogPanelController;

        [Mirror.ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("PanelControllerName")]
        [LabelWidth(200)]
        [ValueDropdown(nameof(AllPanelsName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string PanelControllerName;

        private bool m_done = false;
        private IWindowsController m_windowsController;
        private BlackedPanelController m_blackedPanelController;
        private CityPageView m_cityPageView;
        private CompositeDisposable m_disposables;
        private Type m_targetType;
        
        [Inject]
        public void Construct(IWindowsController windowsController, 
            BlackedPanelController blackedPanelController,
            CityPageView cityPageView)
        {
            m_done = false;
            m_disposables = new CompositeDisposable();
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                m_targetType = assembly.GetTypes().FirstOrDefault(t => t.Name == PanelControllerName);
                if (m_targetType != null)
                    break;
            }
            
            m_windowsController = windowsController;
            m_blackedPanelController = blackedPanelController;
            m_cityPageView = cityPageView;
            
            m_windowsController.OnElementOpen.Subscribe(OnElementOpen);
        }

        private void OnElementOpen(IUiController element)
        {
            if (element == null)
            {
                return;
            }

            if (element.GetType() != m_targetType)
            {
                return;
            }
            
            m_done = true;
        }

        public async UniTask PlayAsync(CancellationToken token)
        {
            await m_dialogPanelController.ShowDialog(OpenTavernDialogId, token);
            m_tutorialTouchController.ShowTouch(m_cityPageView.Tavern.GetComponent<RectTransform>());
            await m_blackedPanelController.SelectView(m_cityPageView.Tavern.GetComponent<RectTransform>());
            await UniTask.WaitUntil(() => m_done, cancellationToken: token);
            m_blackedPanelController.HideSelect();
            m_disposables?.Dispose();
            m_tutorialTouchController.HideTouch();
        }
    }
}