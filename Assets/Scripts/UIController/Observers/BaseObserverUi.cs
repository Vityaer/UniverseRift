using System;
using UniRx;
using UnityEngine;
using VContainer;

namespace UIController.Observers
{
    public abstract class BaseObserverUi : MonoBehaviour, IDisposable
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
