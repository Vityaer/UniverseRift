namespace VContainerUi.Model
{
	public class UiControllerState
	{
		public static readonly UiControllerState IsActiveAndFocus = new UiControllerState(true, true, 0);
		public static readonly UiControllerState IsActiveNotFocus = new UiControllerState(true, false, 0);
		public static readonly UiControllerState NotActiveNotFocus = new UiControllerState(false, false, 0);

		public bool IsActive;
		public bool InFocus;
		public int Order;

		public UiControllerState(bool isActive, bool inFocus, int order)
		{
			IsActive = isActive;
			InFocus = inFocus;
			Order = order;
		}
	}
}