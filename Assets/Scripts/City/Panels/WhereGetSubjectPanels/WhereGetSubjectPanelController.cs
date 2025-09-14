using Common.Resourses;
using UiExtensions.Scroll.Interfaces;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Panels.WhereGetSubjectPanels
{
    public class WhereGetSubjectPanelController : UiPanelController<WhereGetSubjectPanelView>
    {
        public void ShowWhereGetResourcePanel(ResourceType resourceType)
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<WhereGetSubjectPanelController>(openType: OpenType.Additive);
        }
    }
}