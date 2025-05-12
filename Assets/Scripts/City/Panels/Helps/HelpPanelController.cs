using LocalizationSystems;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.Helps
{
    public class HelpPanelController : UiPanelController<HelpPanelView>
    {
        [Inject] private readonly ILocalizationSystem _localizationSystem;

        private Transform m_previousHelpParent;
        private RectTransform m_currentHelpContainer;
        
        public void OpenHelp(RectTransform helpContainer)
        {
            m_currentHelpContainer = helpContainer;
            m_previousHelpParent = helpContainer.parent;
            helpContainer.SetParent(View.HelpWrapper);
            
            View.AdvancedHelpPanel.SetActive(true);
            View.SimpleHelpPanel.SetActive(false);
        }

        public void OpenHelp(string helpMessageLocaleId)
        {
            if (string.IsNullOrEmpty(helpMessageLocaleId))
            {
                Debug.LogError("helpMessageLocaleId is null or empty.");
                return;
            }

            View.MainMessage.StringReference = _localizationSystem.GetLocalizedContainer(helpMessageLocaleId);
            
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HelpPanelController>(openType: OpenType.Additive);
            
            View.AdvancedHelpPanel.SetActive(false);
            View.SimpleHelpPanel.SetActive(true);
        }

        protected override void Close()
        {
            if (m_previousHelpParent != null)
            {
                m_currentHelpContainer.SetParent(m_previousHelpParent);
                m_previousHelpParent = null;
                m_currentHelpContainer = null;
            }
            base.Close();
        }
    }
}