using VContainerUi.Interfaces;

namespace VContainerUi.Model
{
    public class WindowState : IWindowState
    {
        public IBaseUiController CurrentWindow { get; set; }
    }
}