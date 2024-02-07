using System;
using UniRx;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.Observers
{
    public abstract class BaseObserverUi : UiView
    {
        protected CompositeDisposable Disposables = new CompositeDisposable();

        protected abstract void Start();

        [Inject]
        public abstract void Construct();
    }
}
