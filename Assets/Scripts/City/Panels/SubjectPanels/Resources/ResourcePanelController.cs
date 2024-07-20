using Common.Resourses;
using LocalizationSystems;
using UiExtensions.Scroll.Interfaces;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Panels.SubjectPanels.Resources
{
    public class ResourcePanelController : UiPanelController<ResourcePanelView>
    {
        private readonly ILocalizationSystem _localizationSystem;

        public ResourcePanelController(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public void ShowData(GameResource resource)
        {
            View.MainImage.SetData(resource);
            View.Name.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{resource.Type}Name");

            View.Desctiption.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{resource.Type}Desctiption");

            MessagesPublisher.OpenWindowPublisher.OpenWindow<ResourcePanelController>(openType: OpenType.Additive);
        }
    }
}
