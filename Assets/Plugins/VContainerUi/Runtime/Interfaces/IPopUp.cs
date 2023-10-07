using System;
using UniRx;

namespace VContainerUi.Interfaces
{
	public interface IPopUp
	{
        public IObservable<bool> OnNews { get; }

    }
}