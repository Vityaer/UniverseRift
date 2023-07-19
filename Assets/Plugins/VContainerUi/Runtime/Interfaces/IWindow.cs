using VContainerUi.Model;

namespace VContainerUi.Interfaces
{
	public interface IWindow : IBaseUiController, IPopUp
	{
		string Name { get; }
	}
}