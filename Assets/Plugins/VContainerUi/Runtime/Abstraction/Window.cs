﻿using VContainerUi.Interfaces;
using VContainerUi.Model;

namespace VContainerUi.Abstraction
{
	public abstract class Window : IWindow
	{
		public abstract string Name { get; }
        OpenType IBaseUiController.OpenedType { get; set; }

        public abstract void SetState(UiControllerState state, OpenType openedType);
		public abstract void Back();
		public abstract IUiElement[] GetUiElements();
	}
}