using VContainerUi.Interfaces;

namespace VContainerUi.Messages
{
	public readonly struct MessageActiveWindow
	{
		public readonly IBaseUiController Window;

		public MessageActiveWindow(IBaseUiController window)
		{
			Window = window;
		}
	}
}