using VContainerUi.Interfaces;

namespace VContainerUi.Messages
{
	public readonly struct MessageFocusWindow
	{
		public readonly IBaseUiController Window;

		public MessageFocusWindow(IBaseUiController window)
		{
			Window = window;
		}
	}
}