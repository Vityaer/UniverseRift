namespace VContainerUi.Interfaces
{
    public interface IUiController : IBaseUiController
    {
        bool IsActive { get; }
        bool InFocus { get; }

        void ProcessStateOrder();
        void ProcessState();
    }
}