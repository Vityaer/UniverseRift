using System;
using System.Collections.Generic;

namespace VContainerUi.Interfaces
{
	public interface IWindowsController
	{
		void Reset();
		IObservable<IUiController> OnElementClose { get; }
		IObservable<IUiController> OnElementOpen { get; }
	}
}