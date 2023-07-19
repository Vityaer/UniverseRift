using System;
using UniRx;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.Observers
{
    public abstract class BaseObserverUi : UiView, IDisposable
    {
        protected CompositeDisposable Disposables = new CompositeDisposable();

        protected abstract void Start();

        [Inject]
        public abstract void Construct();

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
