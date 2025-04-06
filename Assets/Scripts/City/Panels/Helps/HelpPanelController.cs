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
        
        public void OpenHelp(string helpMessageLocaleId)
        {
            if (string.IsNullOrEmpty(helpMessageLocaleId))
            {
                Debug.LogError("helpMessageLocaleId is null or empty.");
                return;
            }

            View.MainMessage.StringReference = _localizationSystem.GetLocalizedContainer(helpMessageLocaleId);
            
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HelpPanelController>(openType: OpenType.Additive);
            
        }
    }
}