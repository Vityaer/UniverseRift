using VContainerUi.Model;

namespace VContainerUi.Interfaces
{
    public interface IBaseUiController 
    {
        public abstract OpenType OpenedType { get; set; }
        void SetState(UiControllerState state, OpenType openedType = OpenType.Exclusive);
        void Back();
        IUiElement[] GetUiElements();
    }
}