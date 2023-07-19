using VContainerUi.Interfaces;

namespace VContainerUi.Messages
{
	public readonly struct MessageShowWindow
	{
		public readonly IBaseUiController Window;

		public MessageShowWindow(IBaseUiController window)
		{
			Window = window;
		}
	}
}